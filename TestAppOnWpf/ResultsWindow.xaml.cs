using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestAppOnWpf
{
    /// <summary>
    /// Логика взаимодействия для ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        BaseStudentCollection StudentCollection;
        ObservableCollection<ResultData> Results=new ObservableCollection<ResultData>();
        public ResultsWindow(BaseStudentCollection StudentCollection)
        {
            InitializeComponent();
            this.StudentCollection = StudentCollection;
            CreateResultList();
            resultsDataGrid.ItemsSource= Results;
            resultsDataGrid.Columns[1].Width = 100;
            resultsDataGrid.Columns[2].Width = 45;
            resultsDataGrid.Columns[3].Width = 50;
            resultsDataGrid.Columns[4].Width = 50;

        }

        private void CreateResultList()
        {
           foreach(Student student in StudentCollection.GetStudentList())
           {
                foreach(Result result in student.LastResults)
                {
                    Results.Add(new ResultData() { Result = result, StudentName = student.StringName });
                }
           }
        }

        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ResultData result = resultsDataGrid.SelectedItem as ResultData;
            if (result == null) return;
            Loger.PropertyLog(StudentCollection[result.StudentName].StringName+":"+result.StudentName, "student");

        }
        public class ResultData
        {
            public string StudentName { get; set; }
            public Result Result { get;set; }
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
