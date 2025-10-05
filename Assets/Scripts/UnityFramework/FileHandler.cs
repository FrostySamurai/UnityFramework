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

            folderPath = Path.Combine(Application.persistentDataPath, folderPath);
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