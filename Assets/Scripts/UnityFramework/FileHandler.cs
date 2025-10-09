using System.Collections.Generic;
using System.IO;
using Mirzipan.Extensions.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace Samurai.UnityFramework
{
    public static class FileHandler
    {
        private const string LogTag = nameof(FileHandler);
        
        // TODO: not the best way to do this
        private static JsonSerializerSettings SerializerSettings { get; } = new()
        {
            TypeNameHandling = TypeNameHandling.All, // TODO: maybe not all?
        };
        
        public static void Save<T>(T data, string folderPath, string fileName, string extension = "sav")
        {
            string filePath = GetFilePath(folderPath, fileName, extension);
            if (fileName.IsNullOrEmpty())
            {
                Log.Error($"File path is invalid. Can't save..", LogTag);
                return;
            }

            
            var type = typeof(T);
            if (!type.IsSerializable)
            {
                Log.Error($"Can't save type '{type.Name}'! Type is not serializable..", LogTag);
                return;
            }

            folderPath = GetFolderPath(folderPath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using var file = File.Open(filePath, FileMode.Create);
            using var writer = new BinaryWriter(file);
            writer.Write(JsonConvert.SerializeObject(data, SerializerSettings));
            Log.Debug($"File saved on path '{filePath}'.", LogTag);
        }

        public static T Load<T>(string folderPath, string fileName, string extension = "sav")
        {
            string filePath = GetFilePath(folderPath, fileName, extension);
            if (filePath.IsNullOrEmpty())
            {
                Log.Error("File path is invalid. Can't load..", LogTag);
                return default;
            }
            
            var type = typeof(T);
            if (!type.IsSerializable)
            {
                Log.Error($"Can't load type '{type.Name}'! Type is not serializable..", LogTag);
                return default;
            }

            if (!File.Exists(filePath))
            {
                return default;
            }

            using var file = File.OpenRead(filePath);
            using var reader = new BinaryReader(file);
            string content = reader.ReadString();
            var state = JsonConvert.DeserializeObject<T>(content, SerializerSettings);
            Log.Debug($"Loaded file on path '{filePath}'.", LogTag);
            return state;
        }

        public static void GetAllFileNames(List<string> files, string path, string extension = null, bool recursive = false, bool includeExtension = false)
        {
            GetAllFilePaths(files, path, extension, recursive);
            for (int i = 0; i < files.Count; i++)
            {
                if (includeExtension)
                {
                    files[i] = Path.GetFileName(files[i]);
                }
                else
                {
                    files[i] = Path.GetFileNameWithoutExtension(files[i]);
                }
            }
        }
        
        public static void GetAllFilePaths(List<string> files, string path, string extension = null, bool recursive = false)
        {
            string folderPath = GetFolderPath(path);
            if (!Directory.Exists(folderPath))
            {
                return;
            }
            
            string searchPattern = extension.NotNullOrEmpty() ? $"*.{extension}" : "*";
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            files.AddRange(Directory.GetFiles(folderPath, searchPattern, searchOption));
        }

        public static void GetAllDirectories(List<string> directories, string path, bool recursive = false)
        {
            string folderPath = GetFolderPath(path);
            if (!Directory.Exists(folderPath))
            {
                return;
            }
            
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            directories.AddRange(Directory.GetDirectories(folderPath, "*", searchOption));
        }

        private static string GetFolderPath(string folderPath)
        {
            if (folderPath.IsNullOrEmpty())
            {
                return null;
            }
            
            return Path.Combine(Application.persistentDataPath, folderPath);
        }

        private static string GetFilePath(string folderPath, string fileName, string extension)
        {
            if (folderPath.IsNullOrEmpty() || fileName.IsNullOrEmpty() || extension.IsNullOrEmpty())
            {
                return null;
            }

            string path = Path.Combine(Application.persistentDataPath, folderPath, fileName);
            return $"{path}.{extension}";
        }
    }
}