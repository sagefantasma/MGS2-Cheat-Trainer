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

namespace gcx
{
    internal class GcxEditor
    {
        public string FileName { get; set; }
        private byte[] Signature { get; set; }
        private byte[] RawContents { get; set; }
        private byte[] TrimmedContents { get; set; }
        internal List<DecodedProc> Procedures { get; set; }
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

        internal string CallDecompiler(string file)
        {
            //remove first 4 bytes, then call python script
            //make note of the output as we will use it for the rest of the explorer
            FileName = file;
            RawContents = File.ReadAllBytes(FileName);
            Signature = new byte[4];
            TrimmedContents = new byte[RawContents.Length - 4];
            Array.Copy(RawContents, Signature, Signature.Length);
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
            _mainDataLocation = _proceduresDataLocation + _proceduresBodySize + 4;
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
            return functions;
        }

        internal void InsertNewProcedureToFile(DecodedProc procedure)
        {
            //7/21/24 notes: I think we're getting pretty damn close now. I am able to at least insert the script without breaking the decoder,
            //and without crashing the game. But, the decoder isn't reading the data, and it doesnt change anything in MGS2.
            //It's very possible there's still even more work required to make this work(i.e. modifying the manifests and shit), but next
            //i need to get the decoder to actually recognize the spliced in script on reload.
            byte[] newTrimmedData = new byte[TrimmedContents.Length + procedure.ScriptLength + 8];
            //insert 8 bytes at end of proc list, before strres_block_top
            int newTrimmedDataIndex = 0;
            int oldContentsIndex = 0;
            Array.Copy(TrimmedContents, newTrimmedData, _startOfOffsetBlock);
            oldContentsIndex += _startOfOffsetBlock;
            newTrimmedDataIndex += _startOfOffsetBlock - 8;

            //set first 4 bytes to reversed proc name
            byte[] procId = ConvertFunctionNameToByteRepresentation(procedure.Name);
            byte[] newProcBlock = new byte[8];
            procId.CopyTo(newProcBlock, 0);

            //increase proc body size by procedure's data length
            _proceduresBodySize += procedure.ScriptLength;
            _proceduresDataLocation += 8;

            //set last 4 of step 1's bytes to proc offset
            //TODO: instead of going back from main data location, let's try advancing by the size of the proc body size(before it was increased by new script)
            //to see if that would fix this problem with only some files letting us splice shit in? would also allow us to confirm something different isnt wrong
            //int procLocation = _mainDataLocation - 4; // -4 to get at start of main body size, then +8 to account for the added proc table value?
            int procLocation = _proceduresDataLocation + _proceduresBodySize - procedure.ScriptLength - 8;
            procLocation &= 0xFFFFFF;
            byte[] byteLocation = BitConverter.GetBytes(procLocation - _proceduresDataLocation + 8); 
            byteLocation.CopyTo(newProcBlock, 4);
            newProcBlock.CopyTo(newTrimmedData, newTrimmedDataIndex);
            newTrimmedDataIndex += 16;

            //set inserted data to proc's data
            //right now we are at the startOfOffsetBlock. We want to copy all the way from there to where main starts.
            int lengthToCopy = procLocation - _startOfOffsetBlock; //TODO: verify... the logic seems sound to me?
            
            Array.Copy(TrimmedContents, oldContentsIndex, newTrimmedData, newTrimmedDataIndex, lengthToCopy);
            oldContentsIndex += lengthToCopy;
            newTrimmedDataIndex += lengthToCopy;
            procedure.RawContents.CopyTo(newTrimmedData, newTrimmedDataIndex);
            newTrimmedDataIndex += procedure.ScriptLength;

            Array.Copy(TrimmedContents, oldContentsIndex, newTrimmedData, newTrimmedDataIndex, TrimmedContents.Length - oldContentsIndex);
            byte[] newProcBodySize = BitConverter.GetBytes(_proceduresBodySize);
            Array.Copy(newProcBodySize, 0, newTrimmedData, _proceduresDataLocation - 4, 4);

            byte[] extendedRawContents = new byte[RawContents.Length + 8 + procedure.ScriptLength];
            

            Array.Copy(RawContents, extendedRawContents, 4);
            Array.Copy(newTrimmedData, 0, extendedRawContents, 4, newTrimmedData.Length);


            if (extendedRawContents.Length % 16 != 0)
            {
                byte[] squaredOffContents = new byte[extendedRawContents.Length + 16 - extendedRawContents.Length % 16];
                Array.Copy(extendedRawContents, squaredOffContents, extendedRawContents.Length);
                File.WriteAllBytes(FileName, squaredOffContents);
            }
            else
                File.WriteAllBytes(FileName, extendedRawContents);
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

        private byte[] GetFunctionByteData(string functionName, out int procTablePosition, out int scriptPos)
        { 
            //TODO: this isnt quite working correctly, sadge
            byte[] functionData = null;

            if (functionName.ToLower().Trim() != "main")
            {
                if (functionName.Contains("proc_0x"))
                {
                    byte[] functionNameBytes = ConvertFunctionNameToByteRepresentation(functionName);

                    procTablePosition = FindSubArray(TrimmedContents, functionNameBytes);
                    scriptPos = BitConverter.ToInt32(TrimmedContents, procTablePosition + 4);
                    scriptPos = scriptPos & 0xFFFFFF;
                    functionData = new byte[TrimmedContents[_proceduresDataLocation + scriptPos + 1]];
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

        internal void BuildElementList()
        {
            //TODO: should we even bother keeping this?
        }

        internal void SaveGcxFile(Dictionary<string, byte[]> modifications)
        {
            foreach(KeyValuePair<string, byte[]> modification in modifications)
            {
                DecodedProc modifiedProcedure = Procedures.Find(proc => proc.Name.Contains(modification.Key));

                modifiedProcedure.RawContents = modification.Value;
            }

            foreach(DecodedProc procedure in Procedures)
            {
                Array.Copy(procedure.RawContents, 0, TrimmedContents, procedure.ScriptInitialPosition + _proceduresDataLocation, procedure.ScriptLength);
            }

            Array.Copy(TrimmedContents, 0, RawContents, 4, TrimmedContents.Length);

            File.WriteAllBytes(FileName, RawContents);
        }

        internal void CompareFunctionWithOriginal(string currentContents, string originalContents)
        {
            string sanitizedCurrentContents = Regex.Replace(currentContents, @"\s+", ""); //all the whitespace has been added artificially by the decompiler and i am 
            string sanitizedOriginalContents = Regex.Replace(originalContents, @"\s+", ""); //also editing it for readability in this. so we need to get rid of all of it


        }
    }
}
