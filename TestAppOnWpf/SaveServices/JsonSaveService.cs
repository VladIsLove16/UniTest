using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
namespace TestAppOnWpf.FileSaveSystem
{
    class JsonSaveService : ISaveService
    {
        const string EXTENSION = ".json";
        const string filename = "data";
        public void SaveData<T>(T data, string folderPath)
        {
            string filePath = Path.Combine(folderPath, filename+ EXTENSION);

            if (filePath == default)
                filePath = typeof(T).ToString();
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

            Loger.PropertyLog("dictCollection::", "JsonSaveService");
            if (data is Dictionary<string,string> dictCollection)
            {
                Loger.PropertyLog("dictCollectionsize: "+ dictCollection.Count, "JsonSaveService");
                foreach (var item in dictCollection)
                {
                    Loger.PropertyLog(item.Value.ToString(), "JsonSaveService");
                }
            }
            Loger.PropertyLog(data.ToString(), filename);
            Loger.PropertyLog(jsonData, filename);
            File.WriteAllText(filePath, jsonData);
        }

        public T LoadData<T>(string folderPath)
        {
            string filePath = Path.Combine(folderPath, filename + EXTENSION);

            if (!File.Exists(filePath))
                return default(T);

            var jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
