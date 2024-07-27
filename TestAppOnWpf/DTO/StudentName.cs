namespace TestAppOnWpf
{
    public class StudentName
    {
        string name = "default";
        public string Name
        {
            get
            {
                return name;
            }
            internal set
            {
                name = value;
            }
        }
    }
}