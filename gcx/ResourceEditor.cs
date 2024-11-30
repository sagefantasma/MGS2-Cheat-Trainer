using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    internal static class ResourceEditor
    {
        /*
         * In order to successfully edit a resource file, we need to do the following:
         * 1. Identify the missing resource
         * 2. Find the bp_assets.txt and manifest.txt
         * (this class' responsibility starts here)
         * 3. Prepare the list of missing data (ctxr, cmdl, kms)
         * 4. Find where the ctxr and cmdl files will fit into bp_assets, and where kms will fit into manifest
         *          (grouped by filetype, then alphabetical order(ish xdd))
         * 5. Add kms line(s) to the manifest (end with 0D 0D 0A)
         * 6. Add ctxr and cmdl lines to bp_assets (end with 0D 0D 0A)
         */

        /*
         * bp_assets file order:
         * 
         * alphabetically stored ctxr files
         * alphabetically stored evm files
         * alphabetically stored kms files
         */

        /*
         * manifest file order:
         * alphabetically stored tri files
         * alphabetically stored? hzx files
         * var files
         * sar files
         * reverse-alphabetically stored? row files
         * mar files
         * lt2 files
         * reverse-alphabetically stored kms files
         * reverse-alphabetically stored cv2 files
         * gcx file
         */

        static List<byte> _manifestContents { get; set; }
        static List<byte> _bpAssetsContents { get; set; }

        static byte[] EOL = new byte[] { 0x0D, 0x0D, 0x0A };

        private class MGS2ResourceData
        {
            public string Name { get; set; }
            public string Text { get; set; }
            public FileType FileType { get; set; }
        }

        enum FileType
        {
            Kms,
            Cmdl,
            Ctxr
        }


        public static bool AddResource(string gcxFile, string resourceSuperDirectory, string resourceToAdd)
        {
            try
            {
                DirectoryInfo resourceSuperDirectoryInfo = new DirectoryInfo(resourceSuperDirectory);
                DirectoryInfo gcxResourceDirectory = resourceSuperDirectoryInfo.GetDirectories(gcxFile).FirstOrDefault();
                FileInfo bpAssets = gcxResourceDirectory.GetFiles("bp_assets.txt").FirstOrDefault();
                FileInfo manifest = gcxResourceDirectory.GetFiles("manifest.txt").FirstOrDefault();
                _bpAssetsContents = File.ReadAllBytes(bpAssets.FullName).ToList();
                _manifestContents = File.ReadAllBytes(manifest.FullName).ToList();

                List<MGS2ResourceData> missingData = PrepareListOfDataToAdd(resourceToAdd);
                ReplaceStageNames(missingData, gcxFile);
                foreach (MGS2ResourceData dataToAdd in missingData)
                {
                    int insertionPoint = FindInsertionIndex(dataToAdd);
                    if(insertionPoint == -1) //-1 means the new resource would be a duplicate, so don't add it
                    {
                        continue;
                    }
                    if (dataToAdd.FileType == FileType.Kms)
                    {
                        _manifestContents.InsertRange(insertionPoint, Encoding.UTF8.GetBytes(dataToAdd.Text));
                    }
                    else
                    {
                        _bpAssetsContents.InsertRange(insertionPoint, Encoding.UTF8.GetBytes(dataToAdd.Text));
                    }
                }

                File.WriteAllBytes(bpAssets.FullName, _bpAssetsContents.ToArray());
                File.WriteAllBytes(manifest.FullName, _manifestContents.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static int FindInsertionIndex(MGS2ResourceData resource)
        {
            int index;

            if (resource.FileType == FileType.Kms) 
            {
                byte[] startOfKms = Encoding.UTF8.GetBytes("assets/kms");
                byte[] endOfKms = Encoding.UTF8.GetBytes(".kms");
                byte[] encodedText = Encoding.UTF8.GetBytes(resource.Text);
                index = MockAddResource(_manifestContents.ToArray(), startOfKms, endOfKms, encodedText, false);
            }
            else if(resource.FileType == FileType.Cmdl)
            {
                byte[] startOfKms = Encoding.UTF8.GetBytes("assets/kms");
                byte[] endOfKms = Encoding.UTF8.GetBytes(".cmdl");
                byte[] encodedText = Encoding.UTF8.GetBytes(resource.Text);
                index = MockAddResource(_bpAssetsContents.ToArray(), startOfKms, endOfKms, encodedText, true);
            }
            else
            {
                byte[] startOfCmdl = Encoding.UTF8.GetBytes("textures/flatlist");
                byte[] endOfCmdl = Encoding.UTF8.GetBytes(".ctxr");
                byte[] encodedText = Encoding.UTF8.GetBytes(resource.Text);
                index = MockAddResource(_bpAssetsContents.ToArray(), startOfCmdl, endOfCmdl, encodedText, true);
            }

            return index;
        }
        private static int MockAddResource(byte[] mainContents, byte[] startOfBlock, byte[] endOfBlock, byte[] encodedText, bool storedAlphabetically)
        {
            int index = GcxEditor.FindSubArray(mainContents, startOfBlock);
            //next we need to find the asset that would come AFTER the asset we're adding
            //(i.e.: if we're adding dog.kms, we need to find cat.kms after dump.kms)
            int endSplitIndex = GcxEditor.FindAllSubArray(mainContents, endOfBlock).LastOrDefault() + endOfBlock.Length;
            byte[] resourceArray = new byte[endSplitIndex - index];
            Array.Copy(mainContents, index, resourceArray, 0, resourceArray.Length);

            List<byte[]> individualizedResources = SplitResources(resourceArray);

            if (DetermineIfDuplicate(individualizedResources, encodedText))
            {
                return -1;
            }

            int resourceArrayIndex = DetermineNewOrdering(individualizedResources, encodedText, storedAlphabetically);

            index += resourceArrayIndex;

            return index;
        }

        private static int DetermineNewOrdering(List<byte[]> individualizedResources, byte[] encodedText, bool storedAlphabetically)
        {
            individualizedResources = AlphabetizeResources(individualizedResources, encodedText, storedAlphabetically);
            int resourceArrayIndex = 0;
            foreach (byte[] resource in individualizedResources)
            {
                if (!resource.SequenceEqual(encodedText))
                {
                    resourceArrayIndex += resource.Length;
                }
                else
                {
                    break;
                }
            }

            return resourceArrayIndex;
        }

        private static List<byte[]> AlphabetizeResources(List<byte[]> existingResources, byte[] newResource, bool storedAlphabetically = true)
        {
            existingResources.Add(newResource);
            
            List<string> stringedResources = new List<string>();

            foreach (byte[] resource in existingResources)
            {
                stringedResources.Add(Encoding.UTF8.GetString(resource));
            }
            
            stringedResources.Sort();
            if (!storedAlphabetically)
                stringedResources.Reverse();

            existingResources.Clear();
            foreach(string resource in stringedResources)
            {
                existingResources.Add(Encoding.UTF8.GetBytes(resource));
            }

            return existingResources;
        }

        private static List<byte[]> SplitResources(byte[] resourceArray)
        {
            List<int> splittingIndices = GcxEditor.FindAllSubArray(resourceArray, EOL);
            for (int i = 0; i < splittingIndices.Count; i++) //want to include the EOL in each item
            {
                splittingIndices[i] += 3;
            }

            List<byte[]> resourceList = new List<byte[]>();
            int positionInArray = 0;
            for (int i = 0; i < splittingIndices.Count; i++)
            {
                byte[] splitResource;
                if(i == 0)
                {
                    splitResource = new byte[splittingIndices[i]];
                }
                else if (i < splittingIndices.Count - 1)
                    splitResource = new byte[splittingIndices[i] - splittingIndices[i-1]];
                else
                    splitResource = new byte[resourceArray.Length - splittingIndices[i]];
                Array.Copy(resourceArray, positionInArray, splitResource, 0, splitResource.Length);
                resourceList.Add(splitResource);
                positionInArray += splitResource.Length;
            }

            return resourceList;
        }

        private static List<MGS2ResourceData> PrepareListOfDataToAdd(string resourceToAdd)
        {
            List<MGS2ResourceData> resourceData = new List<MGS2ResourceData>();

            Resource resource = Resource.LookupResource(resourceToAdd);

            MGS2ResourceData kmsFile = new MGS2ResourceData();

            kmsFile.Text = resource.Kms;
            kmsFile.FileType = FileType.Kms;
            resourceData.Add(kmsFile);

            MGS2ResourceData ctxrFile = new MGS2ResourceData();

            ctxrFile.Text = resource.Ctxr;
            ctxrFile.FileType = FileType.Ctxr;
            resourceData.Add(ctxrFile);

            MGS2ResourceData cmdlFile = new MGS2ResourceData();

            cmdlFile.Text = resource.Cmdl;
            cmdlFile.FileType = FileType.Cmdl;
            resourceData.Add(cmdlFile);

            return resourceData;
        }

        private static void ReplaceStageNames(List<MGS2ResourceData> resourceData, string stageName)
        {
            foreach(MGS2ResourceData resource in resourceData)
            {
                string replacementString = $"stage/{stageName}/cache";
                resource.Text = resource.Text.Replace("stage/XXXX/cache", replacementString);
            }
        }

        private static bool DetermineIfDuplicate(List<byte[]> existingResources, byte[] newResource)
        {
            foreach (byte[] existingResource in existingResources)
            {
                if (existingResource.SequenceEqual(newResource))
                    return true;
            }

            return false;
        }
    }
}
