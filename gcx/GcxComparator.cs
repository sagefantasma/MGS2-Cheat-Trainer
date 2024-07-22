using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    [Obsolete("This class was largely helpful for dipping into GCX files with no idea what I was doing, but isn't really useful at all now. Refer to our online documentation if you want to understand GCX files a bit better, or use our GcxExplorer to learn hands-on!")]
    internal class GcxComparator
    {
        class ByteMatch
        {
            public string File1Name { get; set; }
            public string File2Name { get; set; }
            public int File1StartIndex { get; set; }
            public int File2StartIndex {  get; set; }
            public int MatchLength { get; set; }
            public string MatchString { get; set; }
        }

        private static List<ByteMatch> CompareOtherFilesToKnownMatches(List<ByteMatch> matches, List<string> fileContents, List<string> fileNames)
        {
            List<ByteMatch> matchesFoundInOtherFiles = new List<ByteMatch>();
            int index = 0;
            foreach (string fileContent in fileContents)
            {
                foreach(ByteMatch match in matches)
                {
                    if (fileContent.Contains(match.MatchString))
                    {
                        matchesFoundInOtherFiles.Add(new ByteMatch
                        {
                            File1Name = match.File1Name,
                            File2Name = Path.GetFileName(fileNames[index].Trim(new char[] { '/', '"' })),
                            File1StartIndex = match.File1StartIndex,
                            File2StartIndex = fileContent.IndexOf(match.MatchString),
                            MatchLength = match.MatchLength,
                            MatchString = match.MatchString
                        });
                    }
                }
                index++;
            }

            return matchesFoundInOtherFiles;
        }

        private static List<ByteMatch> CompareTwoFiles(string file1, string file2, int minimumMatchLength)
        {
            char[] file1Array = file1.ToCharArray();
            char[] file2Array = file2.ToCharArray();
            List<ByteMatch> matchingIndexRanges = new List<ByteMatch>();

            for(int i = 0; i < file1Array.Length; i++)
            {
                int consecutiveMatches = 0;
                for(int j = 0; j < file2Array.Length; j++)
                {
                    if(i + j >= file1Array.Length)
                    {
                        break;
                    }
                    if (file1Array[i + j] == file2Array[j])
                    {
                        consecutiveMatches++;
                    }
                    else
                    {
                        if(consecutiveMatches >= minimumMatchLength)
                        {
                            matchingIndexRanges.Add(new ByteMatch()
                            {
                                File1Name = "File1",
                                File2Name = "File2",
                                File1StartIndex = i,
                                File2StartIndex = j,
                                MatchLength = consecutiveMatches,
                                MatchString = file1.Substring(i, consecutiveMatches)
                            });
                            i += consecutiveMatches;
                        }
                        consecutiveMatches = 0;
                    }
                }
            }

            return matchingIndexRanges;
        }

        private static void DisplayMatches(List<ByteMatch> matchingIndexRanges)
        {
            do
            {
                Console.Clear();
                int i = 1;
                foreach(ByteMatch match in matchingIndexRanges)
                {
                    Console.WriteLine($"Match {i}: {match.MatchLength / 3} consecutive bytes matched, starting at index {match.File1StartIndex} in {match.File1Name} and {match.File2StartIndex} in {match.File2Name}");
                    i++;
                }

                int chosenIndex;
                bool validChoice = false;
                do
                {
                    Console.Write("\nPlease choose a valid match from above to examine: ");
                    validChoice = int.TryParse(Console.ReadLine().ToString(), out chosenIndex);
                    if(chosenIndex > 0 && chosenIndex <= matchingIndexRanges.Count)
                    {
                        validChoice = true;
                    }
                    else
                    {
                        validChoice = false;
                    }
                }while(!validChoice);

                Console.Clear();

                Console.WriteLine($"Match {chosenIndex} contents:\n");
                Console.WriteLine(matchingIndexRanges[chosenIndex -1].MatchString);
                Console.WriteLine("Press 'N' to stop viewing matches from these two files, or any other key to view a different match: ");
            } while (Console.ReadKey().Key != ConsoleKey.N);
        }

        public static void CompareGCXFiles(List<string> fileList, int minimumMatchLength)
        {
            List<string> fileContents = new List<string>();
            foreach(string fileInfo in fileList)
            {
                string trimmedFileInfo = fileInfo.Trim(new char[] { '/', '"' });
                string contents = BitConverter.ToString(File.ReadAllBytes(trimmedFileInfo));
                fileContents.Add(contents);
            }

            List<ByteMatch> matches = CompareTwoFiles(fileContents[0], fileContents[1], minimumMatchLength);
            matches.ForEach(match => match.File1Name = fileList[0].Split('\\').Last());
            matches.ForEach(match => match.File2Name = fileList[1].Split('\\').Last());
            if (matches.Count > 0)
            {
                if (fileList.Count > 2)
                {
                    List<ByteMatch> expandedMatches = CompareOtherFilesToKnownMatches(matches, fileContents.Skip(2).ToList(), fileList.Skip(2).ToList());
                    expandedMatches.AddRange(matches);
                    List<int> requiredMatchCount = new List<int>();
                    foreach (ByteMatch mainMatch in expandedMatches)
                    {
                        if(expandedMatches.Count(subMatch => subMatch.File1StartIndex == mainMatch.File1StartIndex) == fileList.Count - 1)
                        {
                            requiredMatchCount.Add(mainMatch.File1StartIndex);
                        }
                    }

                    expandedMatches.RemoveAll(match => !requiredMatchCount.Contains(match.File1StartIndex));

                    Console.Write($"\n\n{expandedMatches.Count} matches found. Press Y to compare to another file and REMOVE any matches that are found there: ");
                    if(Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        Console.Write("\nPlease provide a file to remove matches from: ");
                        string file = Console.ReadLine();
                        List<ByteMatch> matchesToRemove = CompareOtherFilesToKnownMatches(expandedMatches, new List<string> { BitConverter.ToString(File.ReadAllBytes(file.Trim(new char[] { '/', '"' }))) }, new List<string> { file.Trim(new char[] { '/', '"' }) });
                        foreach(ByteMatch matchToRemove in matchesToRemove)
                        {
                            if(expandedMatches.Any(match => match.File1StartIndex == matchToRemove.File1StartIndex))
                            {
                                expandedMatches.RemoveAll(match => match.File1StartIndex == matchToRemove.File1StartIndex);
                            }
                        }
                    }

                    if (expandedMatches.Count > 0)
                    {
                        DisplayMatches(expandedMatches.OrderBy(x => x.File1StartIndex).ToList());
                    }
                    else
                    {
                        Console.WriteLine("No suitable matches found across all provided files.");
                    }
                }
                else
                {
                    DisplayMatches(matches);
                }
            }
        }
    }
}
