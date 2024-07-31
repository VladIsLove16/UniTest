using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
namespace TestAppOnWpf.SaveLoaderSystem
{
    internal abstract class SaveLoader<TData,TService> : ISaveLoader
    {
        public void Load(IRepository repository)
        {
            var service = App.ServiceProvider.GetService<TService>();
            if (repository.TryGetData(out TData data))
            {
                SetupData(data, service);
                Debug.Write("Loaded to service" + service.ToString());
            }
            else
                SetupDefaultData(service);
            Debug.Write("Loaded to service" + "service");
        }

        public void Save(IRepository repository)
        {
            Loger.PropertyLog("SaveLoader is Saving", "SaveLoader");
            var service = App.ServiceProvider.GetService<TService>();
            TData data = GetData(service);
            if(data is List<Student> students)
            {
                Loger.PropertyLog("data is List<Student> size:" + students.Count, "SaveLoader");
                foreach (var student in students) {
                    Loger.PropertyLog(student.ToString(), "SaveLoader");
                }
            }
            repository.SetData(data);
        }
        public abstract TData GetData(TService service);
        public abstract void SetupData(TData data, TService service);
        public virtual void SetupDefaultData(TService service) { }
    }
}
