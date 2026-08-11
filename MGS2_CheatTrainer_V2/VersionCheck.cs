using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace MGS2_CheatTrainer_V2
{
    internal class VersionSupport
    {
        private static readonly string Repo = "MGS2-Cheat-Trainer";
        private static readonly string Owner = "sagefantasma";
        private class Tag
        {
            public string Name { get; set; }
            public int? MajorVersion { get; set; }
            public int? MinorVersion { get; set; }
            public int? BuildVersion { get; set; }
            public int? RevisionVersion { get; set; }

            public Tag(string name)
            {
                Name = name.ToLower();
                string number = Name.Contains('v') ? Name.Split('v')[1] : Name;
                string[] parts = number.Split('.');
                MajorVersion = int.TryParse(parts[0], out int majorVersion) ? majorVersion : null;
                if(parts.Length >= 2)
                    MinorVersion = int.TryParse(parts[1], out int minorVersion) ? minorVersion : null;
                if(parts.Length >= 3)
                    BuildVersion = int.TryParse(parts[2], out int buildVersion) ? buildVersion : null;
                if(parts.Length >= 4)
                    RevisionVersion = int.TryParse(parts[3], out int revisionVersion) ? revisionVersion : null;
            }
        }

        public static bool CheckIfNewUpdateExists(string appVersion)
        {
            //v0.1.0.0
            GitHubClient gitHubClient = new(new ProductHeaderValue(Repo));
            IReadOnlyList<Release> releases = gitHubClient.Repository.Release.GetAll(Owner, Repo).Result;

            Tag? highestTag = null;
            foreach(Release release in releases)
            {
                Tag tag = new Tag(release.TagName);

                if(highestTag == null)
                {
                    highestTag = tag;
                }

                if (highestTag != tag)
                    if (CheckIfTagIsNewer(highestTag, tag))
                        highestTag = tag;
            }

            Tag currentVersionTag = new Tag($"v{appVersion}");
            if(string.Equals(currentVersionTag.Name, highestTag!.Name))
            {
                //if the current version is the same as the latest release, there is no update
                return false;
            }
            return !CheckIfTagIsNewer(highestTag, currentVersionTag);
        }

        private static bool CheckIfTagIsNewer(Tag highestTag, Tag tagToCheck)
        {
            if (highestTag?.MajorVersion < tagToCheck.MajorVersion)
            {
                return true;
            }
            else if (highestTag?.MajorVersion == tagToCheck.MajorVersion && highestTag?.MinorVersion < tagToCheck.MinorVersion)
            {
                return true;
            }
            else if (highestTag?.MajorVersion == tagToCheck.MajorVersion && highestTag?.MinorVersion == tagToCheck.MinorVersion
                && highestTag?.BuildVersion < tagToCheck.BuildVersion)
            {
                return true;
            }
            else if (highestTag?.MajorVersion == tagToCheck.MajorVersion && highestTag?.MinorVersion == tagToCheck.MinorVersion
                && highestTag?.BuildVersion == tagToCheck.BuildVersion && highestTag?.RevisionVersion < tagToCheck.RevisionVersion)
            {
                return true;
            }

            return false;
        }
    }
}