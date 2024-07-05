using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace gcx
{
    internal class gcx_editor
    {
        public string FileName { get; set; }
        private byte[] RawContents { get; set; }
        private byte[] TrimmedContents { get; set; }
        private List<GCX_Object.Procedure> Procedures { get; set; }

        public gcx_editor()
        {
            Procedures = new List<GCX_Object.Procedure>();
        }

        internal string CallDecompiler(string file)
        {
            //remove first 4 bytes, then call python script
            //make note of the output as we will use it for the rest of the explorer
            RawContents = File.ReadAllBytes(file);
            TrimmedContents = new byte[RawContents.Length - 4];
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
            
            return decompilingProcess.StandardOutput.ReadToEnd();
        }

        internal Dictionary<string,string> BuildContentTree()
        {
            //take the out file and break it down into main function and all procs
            string decompiledFile = "._sanitizedGcx_out";

            string[] linedContents = File.ReadAllLines(decompiledFile);
            linedContents = linedContents.Where(line => line != "").ToArray();

            Dictionary<string, string> functions = new Dictionary<string, string>();

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
                        functions.Add(currentFunctionName, currentFunction);
                    }
                    else
                    {
                        //start of function
                        currentFunctionName = linedContents[i].Substring(0, linedContents[i].LastIndexOf('{'));
                        currentFunction = "";
                    }
                }
                else
                {
                    currentFunction += linedContents[i];
                }
            }

            foreach(KeyValuePair<string,string> kvp in functions)
            {
                byte[] functionData = FindFunctionInFile(kvp.Key, out int procTablePosition, out int scriptPos);
                Procedures.Add(new GCX_Object.Procedure(kvp.Key, functionData, kvp.Value, procTablePosition, scriptPos));
            }

            return functions;
        }

        private byte[] FindFunctionInFile(string functionName, out int procTablePosition, out int scriptPos)
        {
            byte[] functionData = null;

            functionName = functionName.Replace("proc_0x", "");
            string[] functionNamePairings = new string[3];
            if(functionName.Length % 2 != 0)
            {
                //odd length
                functionNamePairings[0] = functionName.Substring(0, 1);
            }
            else
            {
                //even length
                functionNamePairings[0] = functionName.Substring(0, 2);
            }
            functionNamePairings[1] = functionName.Substring(1, 2);
            functionNamePairings[2] = functionName.Substring(3, 2);

            functionNamePairings = functionNamePairings.Reverse().ToArray(); //reverse the order, as this is how they are expressed in the gcx file
            //in proc table, function will be denoted as FF FF FF 00 YY YY 00 00 ; where FFFFFF is the function name, and YY YY is the offset

            if (functionName.ToLower().Trim() != "main")
            {
                byte[] functionNameBytes = new byte[3];
                for (int i = 0; i < functionNameBytes.Length; i++)
                {
                    functionNameBytes[i] = byte.Parse(functionNamePairings[i], NumberStyles.HexNumber);
                }

                procTablePosition = FindSubArray(RawContents, functionNameBytes);

                scriptPos = BitConverter.ToInt32(RawContents, procTablePosition + 4);
            }
            else
            {
                procTablePosition = 0; //main is not in proc table
                scriptPos = -1; //TODO: get main pos from the decompiler
            }

            return functionData; //TODO: populate with data KEK
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

        }

        internal void SaveGcxFile()
        {
            //we can either "recompile", or direct edit bytes... honestly probably easier to "recompile" lmao, but i don't think i can reliably do that.
            //instead, it might actually be simpler to allow the modifications, then do automated math to find the function's contents and do byte modifications


        }

        internal void CompareFunctionWithOriginal(string currentContents, string originalContents)
        {
            string sanitizedCurrentContents = Regex.Replace(currentContents, @"\s+", ""); //all the whitespace has been added artificially by the decompiler and i am 
            string sanitizedOriginalContents = Regex.Replace(originalContents, @"\s+", ""); //also editing it for readability in this. so we need to get rid of all of it


        }
    }
}
