namespace TestAppOnWpf.SaveLoaderSystem
{
    public interface IRepository
    {
        void SetData<T>(T data);
        bool TryGetData<T>(out T data);
        void SaveToFile();
        void LoadFromFile();
    }
}