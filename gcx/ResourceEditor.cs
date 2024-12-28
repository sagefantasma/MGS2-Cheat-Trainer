using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gcx
{
    internal static class ResourceEditor
    {
        static List<byte> _manifestContents { get; set; }
        static List<byte> _bpAssetsContents { get; set; }

        static byte[] EOL = new byte[] { 0x0D, 0x0D, 0x0A };

        private class MGS2ResourceData
        {
            public string Name { get; set; }
            public string Text { get; set; }
            public FileType FileType { get; set; }
            public Resource Resource { get; set; }
        }

        private class LevelResources
        {
            public bool CheckForDuplicates(MGS2ResourceData resource)
            {
                switch (resource.FileType)
                {
                    case FileType.Kms:
                        if (Manifest.KmsFiles.Any(kms => kms.Contains(resource.Resource.CommonName)))
                            return false;
                        break;
                    case FileType.Ctxr:
                        //technically this wont work correctly
                        if (BpAssets.CtxrFiles.Any(ctxr => ctxr.Contains(resource.Resource.CommonName)))
                            return false;
                        break;
                    case FileType.Cmdl:
                        if (BpAssets.KmsFiles.Any(cmdl => cmdl.Contains(resource.Resource.CommonName)))
                            return false;
                        break;
                    case FileType.Tri:
                        //technically this wont work correctly
                        if (Manifest.TriFiles.Any(tri => tri.Contains(resource.Resource.CommonName)))
                            return false;
                        break;
                }

                return true;
            }

            public BpAssets BpAssets { get; set; } = new BpAssets();
            public Manifest Manifest { get; set; } = new Manifest();
        }

        private class BpAssets
        {
            public List<string> CtxrFiles { get; set; } = new List<string>();
            public List<string> EvmFiles { get; set; } = new List<string>(); //can be null
            public List<string> KmsFiles { get; set; } = new List<string>();

            /*
         * bp_assets file order:
         * 
         * alphabetically stored ctxr files
         * alphabetically stored evm files
         * alphabetically stored kms files
         */
            public byte[] ToBytes()
            {
                List<byte> bytes = new List<byte>();
                CtxrFiles.Sort();
                KmsFiles.Sort();
                foreach (string resource in this.CtxrFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.EvmFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.KmsFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }

                return bytes.ToArray();
            }
        }

        private class Manifest
        {
            public List<string> TriFiles { get; set; } = new List<string>();
            public List<string> HzxFiles { get; set; } = new List<string>();
            public List<string> VarFiles { get; set; } = new List<string>();
            public List<string> SarFiles { get; set; } = new List<string>();
            public List<string> RowFiles { get; set; } = new List<string>();
            public List<string> O2dFiles { get; set; } = new List<string>();
            public List<string> MarFiles { get; set; } = new List<string>();
            public List<string> Lt2Files { get; set; } = new List<string>();
            public List<string> KmsFiles { get; set; } = new List<string>();
            public List<string> FarFiles { get; set; } = new List<string>();
            public List<string> EvmFiles { get; set; } = new List<string>(); //can be null
            public List<string> Cv2Files { get; set; } = new List<string>();
            public List<string> AnmFiles { get; set; } = new List<string>();
            public List<string> GcxFiles { get; set; } = new List<string>();


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
            public byte[] ToBytes()
            {
                List<byte> bytes = new List<byte>();
                TriFiles.Sort();
                KmsFiles.Sort();
                KmsFiles.Reverse();

                foreach(string resource in this.TriFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.HzxFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.VarFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.SarFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.RowFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.O2dFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.MarFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.Lt2Files)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.KmsFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.FarFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.EvmFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.Cv2Files)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.AnmFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }
                foreach (string resource in this.GcxFiles)
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(resource));
                    //bytes.AddRange(EOL);
                }

                return bytes.ToArray();
            }
        }

        enum FileType
        {
            Kms,
            Cmdl,
            Ctxr,
            Tri
        }

        private static LevelResources CollectExistingResources()
        {
            LevelResources existingResources = new LevelResources();
            List<byte[]> individualizedBpAssets = SplitResources(_bpAssetsContents.ToArray());
            List<byte[]> individualizedManifest = SplitResources(_manifestContents.ToArray());

            foreach (byte[] resource in individualizedBpAssets)
            {
                string resourceString = Encoding.UTF8.GetString(resource);
                if (resourceString.StartsWith("textures/"))
                {
                    existingResources.BpAssets.CtxrFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/evm/"))
                {
                    existingResources.BpAssets.EvmFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/kms/"))
                {
                    existingResources.BpAssets.KmsFiles.Add(resourceString);
                }
                else if(resourceString != "")
                {
                    throw new NotImplementedException("Unexpected asset type!");
                }
            }

            foreach (byte[] resource in individualizedManifest)
            {
                string resourceString = Encoding.UTF8.GetString(resource);
                if (resourceString.StartsWith("assets/tri/"))
                {
                    existingResources.Manifest.TriFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/hzx/"))
                {
                    existingResources.Manifest.HzxFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/var/"))
                {
                    existingResources.Manifest.VarFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/sar/"))
                {
                    existingResources.Manifest.SarFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/row/"))
                {
                    existingResources.Manifest.SarFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/o2d"))
                {
                    existingResources.Manifest.O2dFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/mar/"))
                {
                    existingResources.Manifest.MarFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/lt2/"))
                {
                    existingResources.Manifest.Lt2Files.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/kms/"))
                {
                    existingResources.Manifest.KmsFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/far/"))
                {
                    existingResources.Manifest.FarFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/evm/"))
                {
                    existingResources.Manifest.EvmFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/cv2/"))
                {
                    existingResources.Manifest.Cv2Files.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/anm/"))
                {
                    existingResources.Manifest.AnmFiles.Add(resourceString);
                }
                else if (resourceString.StartsWith("assets/gcx/"))
                {
                    existingResources.Manifest.GcxFiles.Add(resourceString);
                }
                else if(resourceString != "")
                {
                    throw new NotImplementedException("Unexpected asset type!");
                }
            }

            return existingResources;
        }

        public static void AddResources(string gcxFile, string resourceSuperDirectory, List<string> resourcesToAdd)
        {
            try
            {
                //TODO: need to add a duplicate checker
                DirectoryInfo resourceSuperDirectoryInfo = new DirectoryInfo(resourceSuperDirectory);
                DirectoryInfo gcxResourceDirectory = resourceSuperDirectoryInfo.GetDirectories(gcxFile).FirstOrDefault();
                FileInfo bpAssets = gcxResourceDirectory.GetFiles("bp_assets.txt").FirstOrDefault();
                FileInfo manifest = gcxResourceDirectory.GetFiles("manifest.txt").FirstOrDefault();
                _bpAssetsContents = File.ReadAllBytes(bpAssets.FullName).ToList();
                _manifestContents = File.ReadAllBytes(manifest.FullName).ToList();

                List<MGS2ResourceData> missingData = new List<MGS2ResourceData>();
                foreach(string resourceToAdd in resourcesToAdd)
                {
                    missingData.AddRange(PrepareListOfDataToAdd(resourceToAdd));
                }
                ReplaceStageNames(missingData, gcxFile);

                bool modifiedManifest = false;
                bool modifiedBpAssets = false;

                LevelResources levelResources = CollectExistingResources();

                foreach(MGS2ResourceData dataToAdd in missingData)
                {
                    if (!levelResources.CheckForDuplicates(dataToAdd))
                    {
                        continue;
                    }

                    switch (dataToAdd.FileType)
                    {
                        case FileType.Cmdl:
                            levelResources.BpAssets.KmsFiles.Add(dataToAdd.Text);
                            break;
                        case FileType.Ctxr:
                            levelResources.BpAssets.CtxrFiles.Add(dataToAdd.Text);
                            break;
                        case FileType.Kms:
                            levelResources.Manifest.KmsFiles.Add(dataToAdd.Text);
                            break;
                        case FileType.Tri:
                            levelResources.Manifest.TriFiles.Add(dataToAdd.Text);
                            break;
                    }
                }

                Directory.CreateDirectory(gcxFile);
                File.WriteAllBytes($"{gcxFile}/bp_assets.txt", levelResources.BpAssets.ToBytes());
                File.WriteAllBytes($"{gcxFile}/manifest.txt", levelResources.Manifest.ToBytes());
            }
            catch(Exception ex)
            {

            }
        }

        public static bool AddResource(string gcxFile, string resourceSuperDirectory, string resourceToAdd)
        {
            //TODO: retool this. This is causing problems(weirdly only with the manifest)
            //that are entirely avoidable. Instead of _inserting_ data, let's do something
            //similar to what we did with the gcx: just entirely recreate the data. The master files
            //seemed to reveal that it at least _somewhat_ works in theory. So, if we instead
            //create lists of all of the file types and create the file from those lists, that
            //should, in theory, make this work more reliably.
            try
            {
                //Thread.Sleep(2000); //determined this is NOT an I/O timing issue
                DirectoryInfo resourceSuperDirectoryInfo = new DirectoryInfo(resourceSuperDirectory);
                DirectoryInfo gcxResourceDirectory = resourceSuperDirectoryInfo.GetDirectories(gcxFile).FirstOrDefault();
                FileInfo bpAssets = gcxResourceDirectory.GetFiles("bp_assets.txt").FirstOrDefault();
                FileInfo manifest = gcxResourceDirectory.GetFiles("manifest.txt").FirstOrDefault();
                _bpAssetsContents = File.ReadAllBytes(bpAssets.FullName).ToList();
                _manifestContents = File.ReadAllBytes(manifest.FullName).ToList();

                List<MGS2ResourceData> missingData = PrepareListOfDataToAdd(resourceToAdd);
                ReplaceStageNames(missingData, gcxFile);
                bool modifiedManifest = false;
                bool modifiedBpAssets = false;
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
                        modifiedManifest = true;
                    }
                    else
                    {
                        _bpAssetsContents.InsertRange(insertionPoint, Encoding.UTF8.GetBytes(dataToAdd.Text));
                        modifiedBpAssets = true;
                    }
                }

                if (modifiedBpAssets)
                {
                    File.WriteAllBytes(bpAssets.FullName, _bpAssetsContents.ToArray());
                }
                if (modifiedManifest)
                {
                    File.WriteAllBytes(manifest.FullName, _manifestContents.ToArray());
                }
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
                    splitResource = new byte[resourceArray.Length - splittingIndices[i - 1]];
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
            kmsFile.Resource = resource;
            resourceData.Add(kmsFile);

            if (resource.Ctxr != "")
            {
                MGS2ResourceData ctxrFile = new MGS2ResourceData();

                ctxrFile.Text = resource.Ctxr;
                ctxrFile.FileType = FileType.Ctxr;
                ctxrFile.Resource = resource;
                resourceData.Add(ctxrFile);
            }

            MGS2ResourceData cmdlFile = new MGS2ResourceData();

            cmdlFile.Text = resource.Cmdl;
            cmdlFile.FileType = FileType.Cmdl;
            cmdlFile.Resource = resource;
            resourceData.Add(cmdlFile);

            if(resource.Tri != "")
            {
                MGS2ResourceData triFile = new MGS2ResourceData();

                triFile.Text = resource.Tri;
                triFile.FileType = FileType.Tri;
                triFile.Resource = resource;
                resourceData.Add(triFile);
            }

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
