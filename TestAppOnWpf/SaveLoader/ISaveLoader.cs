using System.Windows.Forms;
using TestAppOnWpf.FileSaveSystem;

namespace TestAppOnWpf.SaveLoaderSystem
{
    internal interface ISaveLoader
    {
        public void Load(ApplicationContext context, IRepository repository);

        public void Save(ApplicationContext context, IRepository repository);
    }
}
