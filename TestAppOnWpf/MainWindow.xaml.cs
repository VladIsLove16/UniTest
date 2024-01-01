using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.IO;
using static TestAppOnWpf.MainWindow;
namespace TestAppOnWpf
{
    public partial class MainWindow : Window 
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
            }
        }
        public Question CurrentQuestion
        { 
            get
            { return User.getInstance().CurrentQuestion; } 
            set
            {
                User.getInstance().CurrentQuestion = value;
            }
        }

        public object Answers
        {
            get
            {
                return User.getInstance().Answers;
            }
        }

        public bool Testing { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadDefaultTests();
            LoadStudents();
            NewUserEnter();
            Closing += MainWindow_Closing;
            User.getInstance().OnTestChanged += OnCurrentTestChanged;
            User.getInstance().OnQuestionChanged += OnCurrentQuestionChange;
          
        }
        #region Loading
        public void LoadDefaultTests()
        {
            TestCollection.AddTests(Repository.LoadTestsFromDirectory());
            OnTestCollectionChanged();//опять таки паттерн обсервер
            CurrentTest = TestCollection[0];
        }
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
                        User.getInstance().SaveAnswer(CurrentQuestion, (Answer)index);
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
            if (User.getInstance().CurrentQuestion != CurrentTest.GetQuestionCollection()[0])
            {
                SaveQuestionAnswer();
                CurrentQuestion = CurrentTest.GetQuestionCollection()[CurrentQuestion.NumberInTest -1];
                
            }
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (User.getInstance().CurrentQuestion.NumberInTest == CurrentTest.QuestionCount-1) ShowExitDialogBox();
            else
            {
                SaveQuestionAnswer();
                GoToTheNextQuestion();
                //хочу здесь написать  User.getInstance().NextQuestion();
                                                                                               //но это не вызовет OnCurrentQuestionChange
            }

        }

        private void GoToTheNextQuestion()
        {
            CurrentQuestion = CurrentTest.GetQuestionCollection()[CurrentQuestion.NumberInTest + 1];
        }

        private void End_Click(object sender, RoutedEventArgs e)
        {
            if (!Testing) { MessageBox.Show("Тестирование ещё не началось"); return; }
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
            if (CurrentTest == null) { MessageBox.Show("Выберите тест"); return; }
            if (!SetUserName()) return;
            Testing = true;
            StartTimer();
            ShowTestBoxes();
            foreach (Answer a in User.getInstance().Answers.Answers.Values)
            {
                Log("Ответ:"+a);
            }
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
           //save
        }
        private void NewUserEnter()
        {
            User.getInstance().NewUserEnter();
            Testing = false;    
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

                        Log("Правильных ответов" + User.getInstance().Result.RightAnswers);
                        ShowResultBox();
                        Console.WriteLine("ShowResults");
                        NewUserEnter();
                        Console.WriteLine("NewUserEnter");
                        Log("Правильных ответов"+User.getInstance().Result.RightAnswers);
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
            MessageBox.Show(User.getInstance().GetName()+", Тест успешно завершен" +
                "\nПравильно ответов: " + User.getInstance().Result.RightAnswers + " из " + CurrentTest.QuestionCount+ 
                "\nПропущено: " + User.getInstance().Result.Skipped+
                "\nЗатрачено: " + User.getInstance().Result.Time 
                );
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
            Test test= TestCollection.GetTest((string)TestTitles.SelectedItem);
            if (test == null)
            {
                MessageBox.Show("test==null");
            }
            else
                CurrentTest = test;
        }
        private void SetCurrentQuestionAsDefault()
        {
            CurrentQuestion = CurrentTest.GetQuestionCollection()[0];
        }
        private void StopTest()
        {
            
        }
        private void ShowCurrentTest()
        {
            TestTitleBlock.Text = CurrentTest.Title;
            ShowCurrentQuestion();
        }
        public void OnCurrentTestChanged()
        {
            if (CurrentTest == null) return;
            Log("CurrentTes:"+CurrentTest.Title);
            foreach(var item in CurrentTest.GetQuestionCollection().GetQuestions()) { Log(item.QuestionString); }
            CurrentTest.ShuffleQuestions();
            foreach (var item in CurrentTest.GetQuestionCollection().GetQuestions()) { Log(item.QuestionString); }
            SetCurrentQuestionAsDefault();
            StopTest();
            ShowCurrentTest();
        }
        void OnCurrentQuestionChange()
        {
            if (CurrentQuestion == null) return;
            Log("текущий вопрос:"+CurrentQuestion.QuestionString);
            Log("Правильный ответ:"+CurrentQuestion.RightAnswer);
            ShowCurrentQuestion();

        }
        private void ShowCurrentQuestion()
        {
            if (CurrentQuestion == null) { Debug.WriteLine("CurrentQuestion==Null"); return; }
            ShowQuestionString();
            ShowPossibleAnswers();
            ShowSelectedAnswer();
            Console.WriteLine(CurrentQuestion.NumberInTest + " " + CurrentQuestion.Id);
        }
        private void ShowQuestionString()
        {
            QuestionBlock.Text = CurrentQuestion.QuestionString;
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
        private void ShowSelectedAnswer()
        {
            if (User.getInstance().Answers[CurrentQuestion] != (Answer)(-1))
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
            labelTime .Text = timer.GetElapsedTime();
        }
        public void ShowTime(object sender, EventArgs e)
        {
            labelTime.Text = timer.GetElapsedTime();
        }
        #endregion
        private void Log(string a)
        {
            bool log = true;
            if (log)
                Debug.WriteLine(a);
        }
        private void Resultsbtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
