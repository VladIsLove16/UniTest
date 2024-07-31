using System.Collections.Generic;
using System.Windows.Forms;

namespace TestAppOnWpf.SaveLoaderSystem
{
    public class SaveLoadersManager
    {
        List<ISaveLoader> saveLoaders=new();
        IRepository repository;
        public SaveLoadersManager(IRepository repository)
        {
            this.repository = repository;
            saveLoaders.Add(new StudentsSaveLoaderByData());
        }
        public void Load()
        {
            repository.LoadFromFile();
            foreach (ISaveLoader loader in saveLoaders)
            {
                loader.Load( repository);
            }
        }
        public void Save()
        {
            foreach (ISaveLoader loader in saveLoaders)
            {

                Loger.PropertyLog("Loader saving: " + loader.ToString(), "SaveLoadersManager");
                loader.Save( repository);
            }
            repository.SaveToFile();
        }
    }
}
