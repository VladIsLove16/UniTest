using Newtonsoft.Json;
using System.IO;
namespace TestAppOnWpf.FileSaveSystem
{
    class JsonSaveService : ISaveService
    {
        const string EXTENSION = ".json";
        public void SaveData<T>(T data, string folderPath)
        {
            string filePath = Path.Combine(folderPath, "\\", typeof(T).ToString(), EXTENSION);

            if (filePath == default)
                filePath = typeof(T).ToString();
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }

        public T LoadData<T>(string folderPath)
        {
            string filePath = Path.Combine(folderPath, typeof(T).ToString(), EXTENSION);

            if (!File.Exists(filePath))
                return default(T);

            var jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
