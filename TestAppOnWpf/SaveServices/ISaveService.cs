namespace TestAppOnWpf.FileSaveSystem
{
    public  interface ISaveService
    {
        void SaveData<T>(T data, string folderPath);
        T LoadData<T>(string folderPath );
    } 

}
