using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        static string _manifestFile { get; set; }
        static string _bpAssetsFile { get; set; }

        static List<byte> _manifestContents { get; set; }
        static List<byte> _bpAssetsContents { get; set; }

        private class MGS2ResourceData
        {
            public string Name { get; set; }
            public string Text { get; set; }
            public bool IsManifestFile { get; set; }
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

                List<MGS2ResourceData> missingData = PrepareListOfMissingData(resourceToAdd);
                foreach (MGS2ResourceData dataToAdd in missingData)
                {
                    int insertionPoint = FindInsertionPoint(dataToAdd);
                    byte[] bytesToAdd = AppendLineEnding(Encoding.UTF8.GetBytes(dataToAdd.Text));
                    if (dataToAdd.IsManifestFile)
                    {
                        _manifestContents.InsertRange(insertionPoint, bytesToAdd);
                    }
                    else
                    {
                        _bpAssetsContents.InsertRange(insertionPoint, bytesToAdd);
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

        private static int FindInsertionPoint(MGS2ResourceData resource)
        {
            int index = 0;

            if (resource.IsManifestFile) 
            {
                //manifest files will be just .kms
                byte[] startOfKms = Encoding.UTF8.GetBytes("assets/kms");
                byte[] startOfCv2 = Encoding.UTF8.GetBytes("assets/cv2");
                byte[] eol = new byte[] { 0x0D, 0x0D, 0x0A };
                index = GcxEditor.FindSubArray(_manifestContents.ToArray(), startOfKms);
                //we now have the start of the kms array
                //next we need to find the asset that would come AFTER the asset we're adding
                //(i.e.: if we're adding dog.kms, we need to find cat.kms after dump.kms)
                int endSplitIndex = GcxEditor.FindSubArray(_manifestContents.ToArray(), startOfCv2);
                byte[] kmsArray = new byte[endSplitIndex - index];
                Array.Copy(_manifestContents.ToArray(), kmsArray, kmsArray.Length);
                List<int> splittingIndices = GcxEditor.FindAllSubArray(kmsArray, eol);

                List<byte[]> individualizedResources = SplitResources(splittingIndices, kmsArray);


            }
            else 
            {
                //assets files will be .ctxr and .cmdl
            }

            return index;
        }

        private static object FindPositionInAlphabeticalOrder(List<byte[]> existingResources, byte[] newResource, int indexToCompare, bool reverse=false)
        {
            for (int i = 0; i < existingResources.Count; i++)
            {
                if (existingResources[i][indexToCompare] < newResource[indexToCompare])
                {
                    //letter is earlier in alphabet than new resource
                }
                else if (existingResources[i][indexToCompare] > newResource[indexToCompare])
                {
                    //letter is later in alphabet than new resource
                }
                else
                {
                    //letter is same in alphabet as new resource
                }
            }
        }

        private static List<byte[]> SplitResources(List<int> splittingIndices, byte[] resourceArray)
        {
            //I _think_ this will work to split all the kms resources individually?
            List<byte[]> existingResources = new List<byte[]>();
            for (int i = 0; i < splittingIndices.Count; i++)
            {
                byte[] splitResource;
                if (i < splittingIndices.Count)
                    splitResource = new byte[splittingIndices[i + 1] - splittingIndices[i]];
                else
                    splitResource = new byte[resourceArray.Length - splittingIndices[i]];
                Array.Copy(resourceArray, splittingIndices[i], splitResource, 0, splitResource.Length);
                existingResources.Add(splitResource);
            }

            return existingResources;
        }

        private static List<MGS2ResourceData> PrepareListOfMissingData(string resourceToAdd)
        {
            List<MGS2ResourceData> resourceData = new List<MGS2ResourceData>();

            return resourceData;
        }

        private static byte[] AppendLineEnding(byte[] text)
        {
            byte[] appendedText = new byte[text.Length + 3];
            Array.Copy(text, appendedText, text.Length);
            appendedText[text.Length] = 0x0D;
            appendedText[text.Length + 1] = 0x0D;
            appendedText[text.Length + 2] = 0x0A;
            return appendedText;
        }
    }
}
