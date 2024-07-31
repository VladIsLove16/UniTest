using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestAppOnWpf
{
    /// <summary>
    /// Логика взаимодействия для ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        IStudentCollection StudentCollection;
        ObservableCollection<ResultData> Results = new ObservableCollection<ResultData>();
        public ResultsWindow(IStudentCollection StudentCollection)
        {
            InitializeComponent();
            this.StudentCollection = StudentCollection;
            CreateResultList();
            resultsDataGrid.ItemsSource = Results;
            resultsDataGrid.Columns[1].Width = 100;
            resultsDataGrid.Columns[2].Width = 45;
            resultsDataGrid.Columns[3].Width = 50;
            resultsDataGrid.Columns[4].Width = 50;

        }

        private void CreateResultList()
        {
            foreach (Student student in StudentCollection.GetStudentList())
            {
                Loger.PropertyLog("Окошко результатов студент: " + student.ToString(), "ResultsWindow");
                foreach (TestResult result in student.GetLastResults())
                {
                    Loger.PropertyLog("студент: " + student.ToString() + "result:" + result.ToString(), "ResultsWindow");
                    Results.Add(new ResultData(student.StringName ,result));
                }
            }
        }

        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ResultData result = resultsDataGrid.SelectedItem as ResultData;
            if (result == null) return;
            Loger.PropertyLog(StudentCollection[result.StudentName].StringName + ":" + result.StudentName, "student");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResultData result = resultsDataGrid.SelectedItem as ResultData;
            if (result == null) return;
            StudentCollection.Delete(result.StudentName);
            Results.Remove(result);

        }
        private void myDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        }
    }
}
