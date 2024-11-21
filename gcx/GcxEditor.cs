using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Buffers.Binary;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;

namespace gcx
{
    internal class GcxEditor
    {
        public string FileName { get; set; }
        private byte[] Timestamp { get; set; }
        private byte[] FileVersion { get; set; }
        private byte[] OffsetTable { get; set; }
        private byte[] Resources { get; set; }
        private byte[] MainBytes { get; set; }
        private byte[] RawContents { get; set; }
        private byte[] TrimmedContents { get; set; }
        private List<DecodedProc> Procedures { get; set; }
        private int _startOfOffsetBlock;
        private int _scriptOffset;
        private int _resourceTableOffset;
        private int _stringTableOffset;
        private int _fontDataOffset;
        private int _key;
        private int _proceduresBodySize;
        private int _proceduresDataLocation;
        private int _mainBodySize;
        private int _mainDataLocation;

        public GcxEditor()
        {
            Procedures = new List<DecodedProc>();
        }

        internal byte[] BuildGcxFile(bool functionsHaveBeenAdded = false)
        {
            List<byte> gcxContents = new List<byte>();

            byte[] procBlock = BuildProcBlock();
            byte[] procTable = BuildProcTable();
            byte[] mainBodySize = BitConverter.GetBytes(_mainBodySize);

            gcxContents.AddRange(Timestamp);
            gcxContents.AddRange(FileVersion);
            gcxContents.AddRange(procTable);
            gcxContents.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            gcxContents.AddRange(OffsetTable);
            gcxContents.AddRange(Resources);
            gcxContents.AddRange(procBlock);
            gcxContents.AddRange(mainBodySize);
            gcxContents.AddRange(MainBytes);

            return gcxContents.ToArray();
        }

        private byte[] BuildProcTable()
        {
            //this new method seems to work perfectly, huzzah
            List<byte> procTable = new List<byte>();
            List<DecodedProc> newOrderedProcs = Procedures.OrderBy(proc => proc.Order).ToList(); //this is a requirement for the gcx format

            foreach (DecodedProc proc in newOrderedProcs)
            {
                if (proc.Name.Contains("main"))
                    continue;
                procTable.AddRange(BitConverter.GetBytes(proc.Order));
                procTable.AddRange(BitConverter.GetBytes(proc.ScriptInitialPosition));
            }

            return procTable.ToArray();
        }

        private byte[] BuildProcBlock()
        {
            List<byte> procBlock = new List<byte>();

            int position = 0;
            foreach (DecodedProc proc in Procedures)
            {
                if (proc.Name.Contains("main"))
                    continue;
                proc.ScriptInitialPosition = position;
                procBlock.AddRange(proc.RawContents);
                position += proc.RawContents.Length;
                /*
                 * We are capturing all the padding now I believe, so I think this can go.
                if (proc.RawContents.SequenceEqual(new byte[] { 0x81, 0x00 }) || proc.RawContents[0] == 0x89 || proc.RawContents[0] == 0x8D)
                {
                    //no padding
                }
                else
                {
                    procBlock.AddRange(new byte[] { 0x00, 0x00 }); //all procs must be ended with the double zero byte, unless its an empty proc
                    position += 2;
                }
                */
            }

            int procBodySize = procBlock.Count;
            procBlock.InsertRange(0, BitConverter.GetBytes(procBodySize));
            return procBlock.ToArray();
        }

        internal string CallDecompiler(string file)
        {
            //remove first 4 bytes, then call python script
            //make note of the output as we will use it for the rest of the explorer
            FileName = file;
            RawContents = File.ReadAllBytes(FileName);
            Timestamp = new byte[4]; //LCGB
            FileVersion = new byte[4];
            TrimmedContents = new byte[RawContents.Length - 4];
            Array.Copy(RawContents, Timestamp, Timestamp.Length);
            Array.Copy(RawContents, 4, FileVersion, 0, FileVersion.Length);
            Array.Copy(RawContents, 4, TrimmedContents, 0, TrimmedContents.Length);
            File.WriteAllBytes("sanitizedGcx", TrimmedContents);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "gcx_decompiler.exe",
                Arguments = @".\sanitizedGcx .",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            Process decompilingProcess = Process.Start(startInfo);

            decompilingProcess.WaitForExit();

            string decompilingOutput = decompilingProcess.StandardOutput.ReadToEnd();
            string[] decompilingOutputs = decompilingOutput.Split('\n');
            foreach(string output in decompilingOutputs)
            {
                if (string.IsNullOrWhiteSpace(output))
                    continue;
                string[] kvp = output.Split(':');
                string key = kvp[0].Trim();
                string value = kvp[1].Trim();
                ParseKeyValuePair(key, value);
            }
            OffsetTable = new byte[20];
            Array.Copy(TrimmedContents, _startOfOffsetBlock, OffsetTable, 0, OffsetTable.Length); //TODO: VERIFY (might be okay?)
            Resources = new byte[_scriptOffset - 20];
            Array.Copy(TrimmedContents, _startOfOffsetBlock + 20, Resources, 0, Resources.Length);
            _mainDataLocation = _proceduresDataLocation + _proceduresBodySize + 4;
            MainBytes = new byte[_mainBodySize];
            Array.Copy(TrimmedContents, _mainDataLocation, MainBytes, 0, _mainBodySize); //TODO: verify (seems okay)
            return decompilingOutput;
        }

        private void ParseKeyValuePair(string key, string value)
        {
            if (key.Contains("strres_block_top"))
            {
                _startOfOffsetBlock = int.Parse(value);
            }
            else if(key.Contains("script offset"))
            {
                _scriptOffset = int.Parse(value);
            }
            else if(key.Contains("resource table offset"))
            {
                _resourceTableOffset = int.Parse(value);
            }
            else if(key.Contains("string table offset"))
            {
                _stringTableOffset = int.Parse(value);
            }
            else if(key.Contains("font data offset"))
            {
                _fontDataOffset = int.Parse(value);
            }
            else if (key.Contains("key"))
            {
                _key = int.Parse(value);
            }
            else if (key.Contains("proc_body_size"))
            {
                _proceduresBodySize = int.Parse(value);
            }
            else if (key.Contains("proc_body_data"))
            {
                _proceduresDataLocation = int.Parse(value);
            }
            else if (key.Contains("main_body_size"))
            {
                _mainBodySize = int.Parse(value);
            }
        }

        internal List<DecodedProc> BuildContentTree()
        {
            //take the out file and break it down into main function and all procs
            string decompiledFile = "._sanitizedGcx_out";

            string[] linedContents = File.ReadAllLines(decompiledFile);
            linedContents = linedContents.Where(line => line != "").ToArray();

            List<DecodedProc> functions = new List<DecodedProc>();

            string currentFunction = "";
            string currentFunctionName = "";
            for(int i = 0; i < linedContents.Length; i++)
            {
                //either new function, or end of one
                if (linedContents[i][0] != '\t')
                {
                    if (linedContents[i][0] == '}')
                    {
                        //end of function
                        if (currentFunctionName.Contains("40ADFE"))
                        {
                            int a = 2 + 2;
                        }
                        byte[] functionData = GetFunctionByteData(currentFunctionName, out int procTablePosition, out int scriptPos);
                        if (!currentFunctionName.ToLower().Contains("main"))
                        {
                            uint order = Convert.ToUInt32(currentFunctionName.Replace("proc_0x", "").Trim(), 16);
                            functions.Add(new DecodedProc(currentFunctionName, order, functionData, currentFunction, procTablePosition, scriptPos));

                        }
                        else
                            functions.Add(new DecodedProc(currentFunctionName, 0, functionData, currentFunction, procTablePosition, scriptPos));
                    }
                    else
                    {
                        //start of function
                        int lastIndex = linedContents[i].LastIndexOf('{');
                        if (lastIndex == -1)
                            continue;
                        currentFunctionName = linedContents[i].Substring(0, lastIndex != -1 ? lastIndex : linedContents[i].Length);
                        currentFunction = "";
                    }
                }
                else
                {
                    currentFunction += linedContents[i];
                }
            }

            Procedures = functions;
            List<DecodedProc> carbonCopy = new List<DecodedProc>();
            foreach(DecodedProc proc in functions)
            {
                carbonCopy.Add(proc);
            }
            return carbonCopy;
        }

        internal void InsertNewProcedureToFile(DecodedProc procedure)
        {
            if(!Procedures.Any(proc => proc.Name == procedure.Name))
                Procedures.Add(procedure);
            //is it really just that simple? yep. i did well for once FeelsGoodMan
        }

        private byte[] ConvertFunctionNameToByteRepresentation(string functionName)
        {
            if (functionName.Contains("proc_0x"))
            {
                //proc table type 1
                functionName = functionName.Replace("proc_0x", "").Trim();
            }
            
            int arraySize = (int)Math.Ceiling(functionName.Length / 2d);
            string[] functionNamePairings = new string[arraySize];
            int index = 0;
            if (functionName.Length % 2 != 0)
            {
                //odd length
                functionNamePairings[0] = functionName.Substring(0, 1);
                index++;
            }
            else
            {
                //even length
                functionNamePairings[0] = functionName.Substring(0, 2);
                index += 2;
            }
            functionNamePairings[1] = functionName.Substring(index, 2);
            index += 2;
            if (functionName.Length > 4)
                functionNamePairings[2] = functionName.Substring(index, 2);
            index += 2;
            if (functionName.Length > 6)
                functionNamePairings[3] = functionName.Substring(index, 2);


            functionNamePairings = functionNamePairings.Reverse().ToArray(); //reverse the order, as this is how they are expressed in the gcx file
                                                                                //in proc table, function will be denoted as FF FF FF 00 YY YY 00 00 ; where FFFFFF is the function name, and YY YY is the offset

            byte[] functionNameBytes = new byte[arraySize];
            for (int i = 0; i < functionNameBytes.Length; i++)
            {
                functionNameBytes[i] = byte.Parse(functionNamePairings[i], NumberStyles.HexNumber);
            }

            return functionNameBytes;
        }

        private byte[] GetFunctionSize(byte procByte, int procLocation)
        {
            switch (procByte)
            {
                case 0x81:
                    //empty function, size 2
                    return new byte[2];
                case 0x82:
                    //unknown, size 3
                    return new byte[3];
                case 0x83:
                    //unknown, size 4
                    return new byte[4];
                case 0x84:
                    //unknown, size 5
                    return new byte[5];
                case 0x85:
                    //unknown, size 6
                    return new byte[6];
                case 0x86:
                    //call another function, size 7(including procByte)
                    return new byte[7];
                case 0x87:
                    //unknown, size 8
                    return new byte[8];
                case 0x88:
                    //unknown, size 9
                    return new byte[9];
                case 0x89:
                    //set a variable, size 10(including procByte)
                    return new byte[10];
                case 0x8A:
                    //seems like calling a function with a variable param, size 11(including procByte)
                    return new byte[11];
                case 0x8B:
                    //call two functions, size 12(including procByte)
                    return new byte[12];
                case 0x8C:
                    //gonna go out on a limb here and guess size 13(including procByte)
                    return new byte[13];
                case 0x8D:
                    //"normal function", size == next byte + 2(to capture header of procByte & size)
                    return new byte[TrimmedContents[procLocation + 1] + 2];
                case 0x8E:
                    //"large function", size == next two bytes + 3(to capture header of procByte & size)
                    return new byte[BitConverter.ToInt16(TrimmedContents, procLocation + 1) + 3];
                default:
                    //need to add, not sure what other cases there may be;
                    MessageBox.Show($"Unknown function call at: {procLocation} - {procByte}");
                    throw new NotImplementedException();
            }
        }

        private byte[] GetFunctionByteData(string functionName, out int procTablePosition, out int scriptPos)
        { 
            //this seems to be working correctly(for now). need to do more extensive testing and such to be 100% certain
            byte[] functionData = null;

            if (functionName.ToLower().Trim() != "main")
            {
                if (functionName.Contains("proc_0x"))
                {
                    byte[] functionNameBytes = ConvertFunctionNameToByteRepresentation(functionName);

                    procTablePosition = FindSubArray(TrimmedContents, functionNameBytes);
                    scriptPos = BitConverter.ToInt32(TrimmedContents, procTablePosition + 4);
                    scriptPos = scriptPos & 0xFFFFFF;
                    int procLocation = _proceduresDataLocation + scriptPos;
                    byte procByte = TrimmedContents[procLocation];

                    functionData = GetFunctionSize(procByte, procLocation);
                    //DOING A BRAINDUMP THAT MIGHT CHANGE HOW I DO THIS IN A MUCH MORE INTELLIGENT WAY GOING FORWARD:
                    //i THINK, in actuality, the length of each function does NOT include the first few bytes
                    //INSTEAD, i think it starts after the length denomination, which i believe would make absolutely everything fall into place(maybe?)
                    //that would fix the issue i am seeing with the 8D functions sometimes ending in A0 00 or 00 00,
                    //it would also fix the issue with the empty 81 functions
                    //i believe it would fix the stupid fuckin 8E functions as well
                    //i dont know for certain about 89 or 86, but it would make sense for those too i think(in some way)
                    //^ implemented this in GetFunctionSize(), looks like its working perfectly
                    /*if (procByte == 0x8D)
                    {
                        functionData = new byte[TrimmedContents[procLocation + 1] + 2]; //to try and capture the A0 00 endings and 00 00 endings
                    }
                    else if (procByte == 0x81)
                    {
                        functionData = new byte[TrimmedContents[procLocation + 1]]; //could probably just sent len to 2 here

                    }
                    else if (procByte == 0x89)
                    {
                        functionData = new byte[10]; // this seems to work correctly
                                                     //i'm doing 10 instead of 9 to capture the zero padding
                    }
                    else if (procByte == 0x86)
                    {
                        functionData = new byte[5]; // this seems to work correctly
                    }
                    else
                    {
                        //this seems to work!
                        uint size = (uint) BitConverter.ToUInt16(TrimmedContents, procLocation + 1) + 1;
                        functionData = new byte[size];
                    }
                    if (functionData.Length == 0)
                        functionData = new byte[2];
                    */
                }
                else
                {
                    //proc table type 2
                    int functionId = int.Parse(functionName.Replace("proc_id_", "")) - 1;

                    procTablePosition = functionId * 4;
                    scriptPos = BitConverter.ToInt32(TrimmedContents, procTablePosition + 4);
                    scriptPos = scriptPos & 0xFFFFFF;

                    functionData = new byte[TrimmedContents[_proceduresDataLocation + scriptPos + 1]];
                }
                //8E functions are breaking this for some functions I think. need to determine how and why(maybe related to the dumb way of doing things im doing them now)
                Array.Copy(TrimmedContents, _proceduresDataLocation + scriptPos, functionData, 0, functionData.Length);
            }
            else
            {
                procTablePosition = -1; //main is never in proc table
                scriptPos = _mainDataLocation;
                functionData = new byte[_mainBodySize];
                Array.Copy(TrimmedContents, scriptPos, functionData, 0, functionData.Length);
            }

            

            return functionData;
        }

        private static int FindSubArray(byte[] largeArray, byte[] subArray)
        {
            if (largeArray == null || subArray == null)
                throw new ArgumentNullException("Arrays cannot be null");

            if (subArray.Length == 0)
                return 0;

            for (int i = 0; i <= largeArray.Length - subArray.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < subArray.Length; j++)
                {
                    if (largeArray[i + j] != subArray[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                    return i;
            }

            return -1; // Subarray not found
        }
    }
}
