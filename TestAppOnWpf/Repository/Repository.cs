using NanoByte.Common.Storage;
using System.Collections.Generic;
using TestAppOnWpf.FileSaveSystem;
namespace TestAppOnWpf.SaveLoaderSystem
{
    public class Repository : IRepository
    {
        private Dictionary<string, object> repository;
        private ISaveService saveService;
        private string folderPath;
        public Repository(ISaveService saveService,string folderPath)
        {
            this.saveService = saveService;
            this.folderPath = folderPath;
        }
        public void SetData<T>(T data)
        {
            string key = typeof(T).ToString();
            repository[key] = data;
        }

        public bool TryGetData<T>(out T data)
        {
            string key = typeof(T).ToString();
            if (repository.TryGetValue(key,out object serializedobject))
            {
                data = (T)serializedobject;
                return true;
            }
            data = default;
            return false;
        }
        public void Save()
        {
            saveService.SaveData(repository, folderPath);
        }
        public void Load()
        {
           repository = saveService.LoadData<Dictionary<string, object>>(folderPath);
        }
    }
}
