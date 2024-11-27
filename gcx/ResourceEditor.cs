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
            
            }
            else 
            {
            
            }

            return index;
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
