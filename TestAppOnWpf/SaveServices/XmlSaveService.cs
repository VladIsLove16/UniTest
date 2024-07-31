using System.IO;
using System.Xml.Serialization;
namespace TestAppOnWpf.FileSaveSystem
{
    class XmlSaveService : ISaveService
    {
        const string filename = "data";
        const string EXTENSION = ".xml";
        public void SaveData<T>(T data, string folderPath)
        {
            string filePath = Path.Combine( folderPath, typeof(T).ToString(), EXTENSION);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data);
            }
        }

        public T LoadData<T>(string folderPath)
        {
            string filePath = Path.Combine(folderPath, typeof(T).ToString(), EXTENSION);
            if (!File.Exists(folderPath))
                return default(T);

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
   
}
