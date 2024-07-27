using System.Collections.Generic;
using System.Windows.Forms;

namespace TestAppOnWpf.SaveLoaderSystem
{
    internal class SaveLoadersManager
    {
        List<ISaveLoader> saveLoaders;
        IRepository repository;
        ApplicationContext context;
        public SaveLoadersManager(IRepository repository, ApplicationContext context)
        {
            this.repository = repository;
            this.context = context;
            saveLoaders.Add(new StudentsSaveLoader());
        }
        public void Load()
        {
            repository.Load();
            foreach (ISaveLoader loader in saveLoaders)
            {
                loader.Load(context, repository);
            }
        }
        public void Save()
        {
            foreach (ISaveLoader loader in saveLoaders)
            {
                loader.Save(context, repository);
            }
            repository.Save();
        }
    }
}
