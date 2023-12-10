using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows.Shapes;
//Вопросы: 
//1. User строка 21
//2. делегаты для тестов и вопросов
namespace TestAppOnWpf
{
    public class TestChangedEventArgs : EventArgs
    {
        
    }
    public partial class MainWindow : System.Windows.Window 
    {
        TestCollection TestCollection = new TestCollection();
        Repository repository=new Repository();
        Students Students=new Students();
        string AnswersFolder = @"Answers";
        string DatabasePath = "D:\\Projects\\VS\\UniTest\\TestAppOnWpf\\DataBase\\database.xml";
        public event EventHandler<TestChangedEventArgs> CurrentTestChanged;
        public Test CurrentTest
        { 
            get
            { return User.getInstance().CurrentTest; } 
            set
            {
                User.getInstance().CurrentTest = value;
                OnCurrentTestChanged();
            }
        }
        public Question CurrentQuestion
        { 
            get
            { return User.getInstance().CurrentQuestion; } 
            set
            {
                User.getInstance().CurrentQuestion = value;
                OnCurrentTestChanged();
            }
        }
            
        
        
        //Получение первой рабочей страницы

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadTests();
            LoadStudentResults();
            NewUserEnter();
            Closing += MainWindow_Closing; 
            
        }
        #region Loading
        public void LoadTests()
        {
            List<Test> tests = repository.LoadTestsFromDirectory();
            foreach(Test test in tests)
            {
                TestCollection.AddTest(test);
            }
            TestTitles.ItemsSource = TestCollection.GetTestTitles();
            TestTitles.SelectedIndex = 0;
        }

        //private void loadstudentsnames()
        //{
        //    observablecollection<string> studentnames = new observablecollection<string>();
        //    string name;
        //    int i = 0;
        //    name = (string)worksheet.cells[i++ + 3, 1].value;
        //    do
        //    {
        //        studentnames.add(name);
        //        name = (string)worksheet.cells[i++ + 3, 1].value;
        //    } while (!string.isnullorempty(name));
        //    namescb.itemssource = studentnames;
        //    namescb.selectedindex = 0;
        //    namescb.text = namescb.items.tostring();
        //}

        private void LoadStudentResults()
        {
            Students.StudentList = MyXmlSerializer.GetStudentResults(DatabasePath);
            NamesCB.ItemsSource= Students.GetStudentNames();
        }
        public void ShowTest(object sender, TestChangedEventArgs e)
        {
            TestTitleBlock.Text =CurrentTest.Title;
            ShowQuestion(CurrentTest.Questions[0]);
            User.getInstance().ClearAnswers();
        }
       
        public void ShowQuestion(Question question)
        {
            QuestionBlock.Text = question.QuestionString;
            int i = 0;
            foreach (RadioButton el in AnswerMenu.Children)
            {
                el.Content = question.PossibleAnswers[i++];
            }
            if (User.getInstance().Answers[question] != (Answer)(-1))
            {
                if (AnswerMenu.Children[(int)User.getInstance().Answers[question]] is RadioButton radioButton)
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
                        User.getInstance(). Answers[User.getInstance().CurrentQuestion] = (Answer)index;
                    }
                    else index++;
                }
            }
        }
        private void SaveResultToFile()
        {
            MyXmlSerializer.SaveStudentResults(DatabasePath, Students.StudentList);
            //for (int i = 0; i < Students.StudentCount; i++)
            //{
                
                //User.getInstance().CurrentQuestion
                
                //if (worksheet.Cells[i + 3, 1].Value!=null && (string) worksheet.Cells[i + 3, 1].Value == UserName)
                //{   
                //    if (worksheet.Cells[i + 3, TestNumber*3 -1].Value==null)
                //    {
                //        worksheet.Cells[i + 3, TestNumber*3 -1].Value = UserRightAnswers;
                //        worksheet.Cells[i + 3, TestNumber* 3 ].Value = UserWrongAnswers;
                //        worksheet.Cells[i + 3, TestNumber* 3 +1].Value = UserSkippedAnswers;
                //    }
                //    else
                //    {
                //        MessageBoxResult result = MessageBox.Show("Результаты уже сохранены. Перезаписать?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                //        if (result == MessageBoxResult.Yes)
                //        {
                //            worksheet.Cells[i + 3, TestNumber * 3 - 1].Value = UserRightAnswers;
                //            worksheet.Cells[i + 3, TestNumber * 3].Value = UserWrongAnswers;
                //            worksheet.Cells[i + 3, TestNumber * 3 + 1].Value = UserSkippedAnswers;
                //        }

                //    }
                //}
            //}
        }
       
        #endregion
        #region Buttons
        private void PreviousQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (User.getInstance().CurrentQuestion != CurrentTest.Questions[0])
            {
                SaveQuestionAnswer();
                User.getInstance().CurrentQuestion = CurrentTest.Questions[User.getInstance().CurrentQuestion.NumberInTest-1];
                ShowQuestion(User.getInstance().CurrentQuestion);
            }
        }
        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (User.getInstance().CurrentQuestion.Id == CurrentTest.QuestionCount-1) ShowExitDialogBox();
            else
            {
                SaveQuestionAnswer();
                User.getInstance().CurrentQuestion = CurrentTest.Questions[User.getInstance().CurrentQuestion.NumberInTest + 1];
                ShowQuestion(User.getInstance().CurrentQuestion);
            }

        }
        #endregion Buttons
        private void CheckResult()
        {
            foreach(Question question in User.getInstance().Answers.Keys) 
            {
                if (User.getInstance().Answers[question] == question.RightAnswer) User.getInstance().Result.RightAnswers++;
                else if (User.getInstance().Answers[question] == (Answer)(-1)) User.getInstance().Result.Skipped++;
                else User.getInstance().Result.WrongAnswers++;
            }
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
           //save
        }
        private void NewUserEnter()
        {
            User.getInstance().NewUserEnter();
        }
        public void ShowExitDialogBox()
        {
            MessageBoxResult result = MessageBox.Show("Завершить тест?", "Завершение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (string.IsNullOrEmpty((string)NamesCB.SelectedItem))
                    {
                        MessageBox.Show("Вы не ввели имя");
                    }
                    else
                    {
                        //SaveNames();
                       // Console.WriteLine("SaveNames");
                        CheckResult();
                        Console.WriteLine("CheckResult");
                        SaveResultToFile();
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

            MessageBox.Show("Тест успешно завершен\nПравильно ответов: " + User.getInstance().Result.RightAnswers + " из " + CurrentTest.QuestionCount); ;
        }

       
        #region OnChange
        private void OnNameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(NamesCB.SelectedItem!=null)User.getInstance().Name = NamesCB.SelectedItem.ToString();
            Console.WriteLine(User.getInstance().Name);
        }
        
        private void TestChanged(object sender, SelectionChangedEventArgs e)
        {
            // NamesCB.Text = (string)TestTitles.SelectedItem;
            // if (TestTitles.SelectedItem != null) {
            //       currentTest.Number = TestTitles.SelectedIndex+1;
            // }
            CurrentTest = TestCollection.GetTest((string)TestTitles.SelectedItem);
            NewUserEnter();
            OnCurrentQuestionChange();

        }
        void OnCurrentQuestionChange()
        {
            TestTitleBlock.Text=CurrentTest.Title;
            QuestionBlock.Text = CurrentQuestion.QuestionString;
            ShowQuestion(CurrentQuestion);
        }
        public void OnCurrentTestChanged()
        {
            CurrentTestChanged?.Invoke(this, new TestChangedEventArgs());
        }
        private void End_Click(object sender, RoutedEventArgs e)
        {
            SaveQuestionAnswer();
            ShowExitDialogBox();
        }
        #endregion

        #region AddObjects
        private void AddName_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NamesCB.Text)) return;
            if (!Students.GetStudentStringNames().Contains(NamesCB.Text) )
                Students.AddStudent(new Student(NamesCB.Text));
            NamesCB.Text = "";
            NamesCB.ItemsSource = Students.GetStudentStringNames();
        }

    private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                if (!TestCollection.GetTestPathes().Contains(openFileDialog.FileName))
                {
                    TestCollection.AddTest(repository.GetTestFromFile(openFileDialog.FileName));
                }
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
