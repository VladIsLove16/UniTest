using System.Diagnostics;
using System.Windows.Forms;
namespace TestAppOnWpf.SaveLoaderSystem
{
    internal abstract class SaveLoader<TData,TService> : ISaveLoader
    {
        public void Load(ApplicationContext context, IRepository repository)
        {
            //var service = new TService();//get from context
            //if (repository.TryGetData(out TData data))
            //{
            //    SetupData(data, service);
            //    Debug.Write("Loaded to service" + service.ToString());
            //}
            //else
            //    SetupDefaultData(service);
            Debug.Write("Loaded to service" + "service");
        }

        public void Save(ApplicationContext context, IRepository repository)
        {
            //var service = new TService();//get from context
            //TData data = GetData(service);
            //repository.SetData(data);
            //Debug.Write("Get from service" + service.ToString());
            Debug.Write("Get from service" + "service");
        }
        public abstract void SetupData(TData data, TService service);
        public abstract TData GetData(TService service);
        public virtual void SetupDefaultData(TService service) { }
    }
}
