using System.Windows.Forms;
using TestAppOnWpf.FileSaveSystem;

namespace TestAppOnWpf.SaveLoaderSystem
{
    internal interface ISaveLoader
    {
        public void Load(IRepository repository);

        public void Save(IRepository repository);
    }
}
