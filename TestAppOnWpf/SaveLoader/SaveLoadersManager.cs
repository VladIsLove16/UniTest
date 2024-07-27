using System.Collections.Generic;
using System.Windows.Forms;

namespace TestAppOnWpf.SaveLoaderSystem
{
    internal class SaveLoadersManager
    {
        List<ISaveLoader> saveLoaders;
        Repository repository;
        ApplicationContext context;
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
