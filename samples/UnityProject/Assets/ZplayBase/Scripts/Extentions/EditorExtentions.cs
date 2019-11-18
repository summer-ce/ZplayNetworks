#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Extentions
{
    public static class EditorExtentions
    {
        /// <summary>
        /// Returns relative filepath of the existing assets.
        /// </summary>
        /// <param name="requestedFileName"></param>
        /// <param name="targetDirectory">omit this parameter to start search from the root project folder</param>
        /// <returns>null if no existing file were found</returns>
        public static String FindAssetPath(String requestedFileName, String targetDirectory = null)
        {
            if (targetDirectory == null)
                targetDirectory = Application.dataPath;

            // Process the list of files found in the directory.
            var fileEntries = Directory.GetFiles(targetDirectory);
            foreach (var fileName in fileEntries)
                if (Path.GetFileName(fileName) == requestedFileName)
                {
                    var directoryName = Path.GetDirectoryName(fileName);
                    if (directoryName == null)
                        return null;

                    var resourcesStartIndex = directoryName.IndexOf("Resources", StringComparison.InvariantCulture);
                    if (resourcesStartIndex < 0)
                    {
                        Debug.LogError(String.Format("Asset named {0} was found at {1}. Unity will be unable to load it. Please place it inside 'Resource' folder or in any of it's subdirectories.", requestedFileName, directoryName));
                        return null;
                    }
                    if (resourcesStartIndex + "Resources".Length + 1 < directoryName.Length)
                        directoryName =
                            directoryName.Substring(resourcesStartIndex + "Resources".Length + 1).Replace('\\', '/') + '/'; //+1 removes extra '/'
                    else
                        directoryName = "";

                    var result = directoryName + Path.GetFileNameWithoutExtension(fileName);
                    return result;
                }

            // Recurse into subdirectories of this directory.
            var subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (var subdirectory in subdirectoryEntries)
            {
                var result = FindAssetPath(requestedFileName, subdirectory);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
#endif