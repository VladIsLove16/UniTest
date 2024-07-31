using NanoByte.Common.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TestAppOnWpf.FileSaveSystem;
namespace TestAppOnWpf.SaveLoaderSystem
{
    public class Repository : IRepository
    {
        private Dictionary<string, string> repository=new();
        private ISaveService saveService;
        private string folderPath;
        public Repository(ISaveService saveService)
        {
            this.saveService = saveService;
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            Loger.Log("FolderPath:" + folderPath);
        }
        public void SetData<T>(T data)
        {
            string key = typeof(T).ToString();
            
            string serializedData = JsonConvert.SerializeObject(data);
            Loger.PropertyLog("Setting to Repo:"+ serializedData, "Repository");
            Loger.PropertyLog("Setting to Repo not ser data:"+ data.ToString(), "Repository");
            repository[key] = serializedData;
        }

        public bool TryGetData<T>(out T data)
        {
            string key = typeof(T).ToString();
            if(repository==null)
                repository = new Dictionary<string, string>();
            if (repository.TryGetValue(key,out string serializedString))
            {
                data = JsonConvert.DeserializeObject<T>(serializedString);
                return true;
            }
            data = default;
            return false;
        }
        public void SaveToFile()
        {
            saveService.SaveData(repository, folderPath);
        }
        public void LoadFromFile()
        {
           repository = saveService.LoadData<Dictionary<string, string>>(folderPath);
            if(repository==null)
                repository = new Dictionary<string, string>();  
           Loger.Log("Loaded data:"+ folderPath +" count: "+ repository.Count);
        }
    }
}
