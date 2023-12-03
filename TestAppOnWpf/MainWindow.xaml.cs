using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Linq;
namespace TestAppOnWpf
{
    public partial class MainWindow : System.Windows.Window 
    {
        Student[] Students;
        string StudentsPath;
        ObservableCollection<string> TestsPathes = new ObservableCollection<string>();
        int TestNumber=1;
        string UserName = "";

        List<Task> Questions;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadTests();
            LoadStudentsNames();
            NewUserEnter();
            Closing += MainWindow_Closing; 
            
        }
        #region Loading
        private void LoadStudentResults()
        {
            for (int i = 0; i < 15; i++)
            {
                string student = (string)worksheet.Cells[i + 1, 1].Value;
                Result.StudentResults[student] = new StudentResult();
                for (int j = 0; j < 3; j++)
                {
                    //    Result.StudentResults[student].TestResults[j] = new result();
                    //     = (int)worksheet.Cells[i + 1, j+2].Value;
                }
            }
        }
        private void LoadTests()
        {
            TestsNames.ItemsSource = TestsPathes;
            string folderPath = @"Tests";
            try
            {
                string[] files = Directory.GetFiles(folderPath, "*.txt");
                foreach (string file in files)
                {
                    TestsPathes.Add(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении списка файлов: {ex.Message}");
            }
            TestsNames.SelectedIndex = 0;
        }
        private void LoadStudentsNames()
        {
            StudentsNames = new ObservableCollection<string>();
            string name;
            int i = 0;
            name = (string)worksheet.Cells[i++ + 3, 1].Value;
            do
            {
                StudentsNames.Add(name);
                name = (string)worksheet.Cells[i++ + 3, 1].Value;
            } while (!string.IsNullOrEmpty(name));
            NamesCB.ItemsSource = StudentsNames;
            NamesCB.SelectedIndex = 0;
            NamesCB.Text = NamesCB.Items.ToString();
        }
        public void LoadQuestion(int num)
        {
            Question.Text = Questions[num];
            int i = 0;
            foreach (RadioButton el in AnswerMenu.Children)
            {
                el.Content = Answers[num, i++];
            }
            if (UserAnswers[num] != -1)
            {
                if (AnswerMenu.Children[UserAnswers[num]] is RadioButton radioButton)
                {
                    radioButton.IsChecked = true;
                }
            }
            else
            {
                foreach (RadioButton el in AnswerMenu.Children)
                {
                    el.IsChecked = false;
                }
            }
        }
        public void LoadTest(string filepath)
        {
            Console.WriteLine(filepath);
            var srcEncoding = Encoding.GetEncoding(1251);
            using (var src = new StreamReader(filepath, encoding: srcEncoding))
            {
                if (TestNumber == 1)
                    QuestionsCount = 30;
                else QuestionsCount = 20;
                QuestionsCount = 20;
                TestName.Text = src.ReadLine();
                Questions = new string[QuestionsCount];
                Answers = new string[QuestionsCount, 4];
                UserAnswers = new int[QuestionsCount];
                RightAnswers = new int[QuestionsCount];
                
                for (int i = 0; i < UserAnswers.Length; i++) { UserAnswers[i] = -1; }
                for (int i = 0; i < QuestionsCount; i++)
                {
                    Console.WriteLine("QuestionsCount:"+i);
                    while (string.IsNullOrEmpty(line = src.ReadLine())) { }
                    Console.WriteLine(line);
                    Questions[i] = line;
                    for (int j = 0; j < 4; j++)
                    {
                        while ((line = src.ReadLine()) == null) { }
                        Console.WriteLine(line);
                        Answers[i, j] = line;
                    }
                }
            };
            LoadAnswers();
            LoadQuestion(0);
        }
        public void LoadAnswers()
        {
            string path = "Answers\\Answers2.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                for (int i = 0; i < QuestionsCount; i++)
                {
                    Console.WriteLine(reader.ReadLine());
                        //RightAnswers[i] = int.Parse(reader.ReadLine()) -1;
                    Console.WriteLine("LoadAnswers" + i);
                }
                
            }
        }
        #endregion
        #region Savings
        private void SaveQuestionAnswer()
        {
            int index = 0;
            foreach (var el in AnswerMenu.Children)
            {
                if (el is RadioButton button)
                {
                    if (button.IsChecked == true)
                    {
                        UserAnswers[QuestionNumber] = index;
                    }
                    else index++;
                }
            }
        }
        private void SaveNames()
        {
            List<string> people = new List<string>(StudentsNames);
            people.Sort();
            StudentsNames = new ObservableCollection<string>(people);
            for (int i = 0; i < StudentsNames.Count; i++)
            {
                if (string.IsNullOrEmpty(StudentsNames[i]))
                    {
                    StudentsNames.RemoveAt(i);
                    i--;  }
                else
                worksheet.Cells[i+3 , 1].Value = StudentsNames[i];
            }
            package.Save();
        }
       private void CheckResult()
        {
            for (int i = 0; i < QuestionsCount; i++)
            {
                if (RightAnswers[i] == UserAnswers[i]) UserRightAnswers++;
                else if (UserAnswers[i] == -1) UserSkippedAnswers++;
                else UserWrongAnswers++;
                
            }
        }
        private void SaveResult()
        {
            for (int i = 0; i < StudentsNames.Count; i++)
            {
                
                if (worksheet.Cells[i + 3, 1].Value!=null && (string) worksheet.Cells[i + 3, 1].Value == UserName)
                {   
                    if (worksheet.Cells[i + 3, TestNumber*3 -1].Value==null)
                    {
                        worksheet.Cells[i + 3, TestNumber*3 -1].Value = UserRightAnswers;
                        worksheet.Cells[i + 3, TestNumber * 3 ].Value = UserWrongAnswers;
                        worksheet.Cells[i + 3, TestNumber * 3 +1].Value = UserSkippedAnswers;
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Результаты уже сохранены. Перезаписать?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            worksheet.Cells[i + 3, TestNumber * 3 - 1].Value = UserRightAnswers;
                            worksheet.Cells[i + 3, TestNumber * 3].Value = UserWrongAnswers;
                            worksheet.Cells[i + 3, TestNumber * 3 + 1].Value = UserSkippedAnswers;
                        }

                    }
                }
            }
        }
        #endregion
        #region Buttons
        private void PreviousQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionNumber != 0)
            {
                SaveQuestionAnswer();
                QuestionNumber--;
                LoadQuestion(QuestionNumber);
            }
        }
        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionNumber == QuestionsCount-1) ShowExitDialogBox();
            else
            {
                SaveQuestionAnswer();
                QuestionNumber++;
                LoadQuestion(QuestionNumber);
            }

        }
        #endregion Buttons
       
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
           //save
        }
        private void NewUserEnter()
        {
            }
        public void ShowExitDialogBox()
        {
            MessageBoxResult result = MessageBox.Show("Завершить тест?", "Завершение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (string.IsNullOrEmpty(UserName))
                    {
                        MessageBox.Show("Вы не ввели имя");
                    }
                    else
                    {
                        SaveNames();
                        Console.WriteLine("SaveNames");
                        CheckResult();
                        Console.WriteLine("CheckResult");
                        SaveResult();
                        Console.WriteLine("SaveResult");
                        ShowResults();
                        Console.WriteLine("ShowResults");
                        NewUserEnter();
                        Console.WriteLine("NewUserEnter");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось сохранить результаты");
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void ShowResults()
        {
            
            MessageBox.Show("Тест успешно завершен\nПравильно ответов: "+ UserRightAnswers +" из "+ QuestionsCount);
        }

       
        #region OnChange
        private void OnNameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(NamesCB.SelectedItem!=null)UserName = NamesCB.SelectedItem.ToString();
            Console.WriteLine(UserName);
        }
        
        private void TestChanged(object sender, SelectionChangedEventArgs e)
        {
            // NamesCB.Text = (string)TestsNames.SelectedItem;
            if (TestsNames.SelectedItem != null) {
                    TestNumber = TestsNames.SelectedIndex+1;
            }
            LoadTest((string)TestsNames.SelectedItem);

        }

        private void End_Click(object sender, RoutedEventArgs e)
        {
            QuestionNumber = QuestionsCount-1;
        }
        #endregion

        #region AddObjects
        private void AddName_Click(object sender, RoutedEventArgs e)
        {
            if(!studentsNames.Contains(NamesCB.Text) && !string.IsNullOrEmpty( NamesCB.Text))
                StudentsNames.Add(NamesCB.Text);
            NamesCB.Text = "";
        }

    private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                if (!TestsPathes.Contains(openFileDialog.FileName));
                TestsPathes.Add(openFileDialog.FileName);
            }
        }
        #endregion
        private void ComboBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
           

            // Добавление userInput в коллекцию Items
            
        }
        
    }
}
