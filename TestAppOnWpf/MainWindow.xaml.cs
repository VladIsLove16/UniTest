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
using System.Threading;
using System.Diagnostics;
using static TestAppOnWpf.MainWindow;
using System.CodeDom;
using System.Data.SqlClient;
//Вопросы: 
//1. User строка 21
//2. Куда деть юзера?
//3. Test.LoadFromSql или sqlRep.LoadTestFromSql какой вариант лучше и почему
//4. строка 102 и делегаты для тестов и вопросов(при изменении текущего теста и вопросов) как это оформить... строка 206,76 про тестколлекшн
//5. Коллекции студнентов типов лист и словарь очень сильно себя повторяют;
//6. Как прервать выполнение функции
namespace TestAppOnWpf
{
    public class TestChangedEventArgs : EventArgs
    {
        
    }
    public partial class MainWindow : System.Windows.Window 
    {
        TestCollection TestCollection = new TestCollection();
        IStudentCollection StudentCollection = new StudentDictCollection();
        TxtAndXmlRepository Repository=new TxtAndXmlRepository();
        string AnswersFolder = @"Answers";
        string DatabasePath = "D:\\Projects\\VS\\UniTest\\TestAppOnWpf\\DataBase\\database.xml";
        StopwatchA timer=new StopwatchA();
        public delegate void Notify();
        public event Notify notify;
        public Test CurrentTest
        { 
            get
            { 
                return User.getInstance().CurrentTest; 
            } 
            set
            {
                User.getInstance().CurrentTest = value;
                OnCurrentTestChanged();
                notify?.Invoke();
            }
        }
        public Question CurrentQuestion
        { 
            get
            { return User.getInstance().CurrentQuestion; } 
            set
            {
                User.getInstance().CurrentQuestion = value;
                OnCurrentQuestionChange();
                notify?.Invoke();
            }
        }

        public event EventHandler<TestChangedEventArgs> CurrentTestChanged;


        //Получение первой рабочей страницы

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadDefaultTests();
            LoadStudents();
            NewUserEnter();
            Closing += MainWindow_Closing;
            User.getInstance().OnTestChanged += OnCurrentTestChanged;
           // notify += EventInvoke;
        }

        private void EventInvoke()
        {
            Console.WriteLine("event Invoked");
        }
        #region Loading
        public void LoadDefaultTests()
        {
            TestCollection.AddTests(Repository.LoadTestsFromDirectory());
            OnTestCollectionChanged();//опять таки паттерн обсервер
            CurrentTest = TestCollection[0];
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

        private void LoadStudents()
        {
            Console.WriteLine($"Загрузка списка студентов из файла: {DatabasePath}...");
            StudentCollection.Set(Repository.GetStudentsFromFile(DatabasePath));
            NamesCB.ItemsSource= StudentCollection.GetNames();//такую функцию нужно не здесь написать, а выполнять каждый раз когда меняется список студентов
            foreach(string student in StudentCollection.GetNames())
            {
                Console.WriteLine($"student {student} загружен");
            }
        }
        #endregion
        #region Showing
        public void ShowTestBoxes()
        {
            TestTitleBlock.Visibility = Visibility.Visible;
            QuestionBlock.Visibility = Visibility.Visible;
            AnswerMenu.Visibility = Visibility.Visible;
        }
        public void HideTestBoxes()
        {
            TestTitleBlock.Visibility = Visibility.Collapsed;
            QuestionBlock.Visibility = Visibility.Collapsed;
            AnswerMenu.Visibility = Visibility.Collapsed;
        }

        private void ShowPossibleAnswers()
        {
            List<string> possibleAnswers = CurrentQuestion.GetPossibleAnswers();
            int i = 0;
            foreach (RadioButton el in AnswerMenu.Children)
            {
                el.Content = possibleAnswers[i++];
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
        private void SaveStudentResult()
        {
            Repository.SaveStudents(DatabasePath, StudentCollection.Get());
        }
       
        #endregion
        #region Buttons
        private void PreviousQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (User.getInstance().CurrentQuestion != CurrentTest.GetQuestions()[0])
            {
                SaveQuestionAnswer();
                CurrentQuestion = CurrentTest.GetQuestions()[CurrentQuestion.NumberInTest -1];
                
            }
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (User.getInstance().CurrentQuestion.NumberInTest == CurrentTest.QuestionCount-1) ShowExitDialogBox();
            else
            {
                SaveQuestionAnswer();
                CurrentQuestion = CurrentTest.GetQuestions()[CurrentQuestion.NumberInTest + 1];//хочу здесь написать  User.getInstance().NextQuestion();
                                                                                               //но это не вызовет OnCurrentQuestionChange
            }

        }
        private void End_Click(object sender, RoutedEventArgs e)
        {
            SaveQuestionAnswer();
            ShowExitDialogBox();
        }
        private void AddName_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NamesCB.Text)) return;
            if (!StudentCollection.GetNames().Contains(NamesCB.Text))
                StudentCollection.Add(new Student(NamesCB.Text));
            NamesCB.Text = "";
            NamesCB.ItemsSource = StudentCollection.GetNames();
        }

        private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                Test test = Repository.GetTestFromFile(openFileDialog.FileName);
                TestCollection.AddTest(test);
                OnTestCollectionChanged();//и это тоже не здесь надо писать 
            }
        }

        private void OnTestCollectionChanged()
        {
            TestTitles.ItemsSource=TestCollection.GetTestTitles();
        }
        #endregion Buttons
        private void StartTest(object sender, RoutedEventArgs e)
        {
            if (!SetUserName()) return;
            StartTimer();
            ShowTestBoxes();
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
           //save
        }
        private void NewUserEnter()
        {
            User.getInstance().NewUserEnter();
            StopTimer();
            ResetTimer();
            HideTestBoxes();
            ClearSelectedAnswer();
        }

        private void ClearSelectedAnswer()
        {
           foreach(RadioButton button in AnswerMenu.Children)
            {
                button.IsChecked = false;
            }
        }
        #region messageBoxes
        public void ShowExitDialogBox()
        {
            MessageBoxResult result = MessageBox.Show("Завершить тест?", "Завершение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (User.getInstance().GetName()==null || User.getInstance().GetName() == "")
                    {
                        EmptyUserNameError();
                    }
                    else
                    {
                        //SaveNames();
                       // Console.WriteLine("SaveNames");
                        SaveTestResult();
                        Console.WriteLine("SaveTestResult");
                        SaveStudentResult();
                        Console.WriteLine("SaveStudentResults");
                        ShowResultBox();
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

        

        private void ShowResultBox()
        {
            MessageBox.Show("Тест успешно завершен\nПравильно ответов: " + User.getInstance().Result.RightAnswers + " из " + CurrentTest.QuestionCount); ;
        }
        #endregion
        #region ErrorMessages
        private void EmptyUserNameError()
        {
            MessageBox.Show("Имя пользователя пусто или отсутствует");
        }

        private void EmptyNameCB()
        {
            MessageBox.Show("Вы не ввели имя");
        }
        
        #region OnChange
        private void OnNameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("Текущий пользователь: "+User.getInstance().GetName());
        }


        private void TestChanged(object sender, SelectionChangedEventArgs e)
        {
            // NamesCB.Text = (string)TestTitles.SelectedItem;
            // if (TestTitles.SelectedItem != null) {
            //       currentTest.Number = TestTitles.SelectedIndex+1;
            // }
            CurrentTest = TestCollection.GetTest((string)TestTitles.SelectedItem);
        }
        public void OnCurrentTestChanged()
        {
            CurrentTestChanged?.Invoke(this, new TestChangedEventArgs());
            CurrentTest.ShuffleQuestions();
            User.getInstance().SetCurrentTest(CurrentTest);
            StopTest();
            ShowCurrentTest();
        }

        private void StopTest()
        {
            
        }

        private void ShowCurrentTest()
        {
            TestTitleBlock.Text = CurrentTest.Title;
            ShowCurrentQuestion();
        }
        void OnCurrentQuestionChange()
        {
            ShowCurrentQuestion();
        }
        private void ShowCurrentQuestion()
        {
            QuestionBlock.Text = CurrentQuestion.QuestionString;
            ShowPossibleAnswers();
            ShowSelectedAnswer();
            Console.WriteLine(CurrentQuestion.NumberInTest + " " + CurrentQuestion.Id);
        }

        private void ShowSelectedAnswer()
        {

            if (User.getInstance().Answers[User.getInstance().CurrentQuestion] != (Answer)(-1))
            {
                if (AnswerMenu.Children[(int)User.getInstance().Answers[CurrentQuestion]] is RadioButton radioButton)
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
        private void ComboBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
        }

        private void SaveTestResult()
        {
            User.getInstance().SetTestResult();
            User.getInstance().Result.Time = timer.ElapsedTime;
            Student student = User.ToStudent();
            string name = student.stringName;
            if (!StudentCollection.Contains(name))
            {
                StudentCollection.Add(student);
            }
            else
            {
                StudentCollection[name].AddResult(User.getInstance().CurrentTest,User.getInstance().Result);
            }

        }
        private bool SetUserName()
        {
            if (NamesCB.Text == "") { EmptyNameCB(); return false; }
            User.getInstance().SetName(NamesCB.Text.ToString());
            return true;
        }

        

        private void StartTimer()
        {
            Console.WriteLine("TimerStart...");
            timer.Start();
            timer.timer.Tick += ShowTime;//плохо, надо добавить обсервера.
            labelTime.Text =timer.GetElapsedTime();
            Console.WriteLine("TimerStarted.");
        }
        private void StopTimer()
        {
            timer.Stop();
        }
        private void ResetTimer()
        {
            timer. Reset();
            labelTime.Text = timer.GetElapsedTime();
        }
        public void ShowTime(object sender, EventArgs e)
        {
            labelTime.Text = timer.GetElapsedTime();
        }
        #endregion
    }
}
