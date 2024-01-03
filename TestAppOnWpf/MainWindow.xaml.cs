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
using MySqlX.XDevAPI.Common;

namespace TestAppOnWpf
{
    public partial class MainWindow : Window 
    {
        TestCollection TestCollection = new TestCollection();
        BaseStudentCollection StudentCollection = new StudentDictCollection();
        BaseTestLoader testLoader=new WordandTxtTestLoader("D:\\Projects\\VS\\UniTest\\TestAppOnWpf\\Tests\\", "D:\\Projects\\VS\\UniTest\\TestAppOnWpf\\Answers\\");
        BaseRepository Repository=new XMLRepository("D:\\Projects\\VS\\UniTest\\TestAppOnWpf\\DataBase\\database.xml");
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
            TestCollection.AddTests(testLoader.LoadDefaultTests());
            OnTestCollectionChanged();//опять таки паттерн обсервер
        }
        private void LoadStudents()
        {
            Console.WriteLine($"Загрузка списка студентов из файла: {Repository.StudentResultsPath}...");
            StudentCollection.Set(Repository.GetStudentsFromFile());
            foreach(Student student1 in StudentCollection.GetStudentList()) {
                student1.Print();
            }
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
        private void SaveStudentToRepository()
        {
            Loger.Log("SavingStudentToRepository");
            Repository.SaveStudentsToFile(StudentCollection.GetStudentList());
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

        private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                Test test = testLoader.LoadTestFromDirectory(openFileDialog.FileName);
                TestCollection.AddTest(test);
                OnTestCollectionChanged();//и это тоже не здесь надо писать 
            }
        }

        private void OnTestCollectionChanged()
        {
            TestTitles.ItemsSource=TestCollection.GetTestTitles();
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            //save
        }
        private void NewUserEnter()
        {
            Loger.Log("NewUserEnter");
            User.getInstance().NewUserEnter();
            Testing = false;
            StopTimer();
            ResetTimer();
            HideTestBoxes();
            ClearSelectedAnswer();
        }

        private void ClearSelectedAnswer()
        {
            foreach (RadioButton button in AnswerMenu.Children)
            {
                button.IsChecked = false;
            }
        }
        #endregion Buttons
        private void StartTest(object sender, RoutedEventArgs e)
        {
            if (Testing) { MessageBox.Show("Тестирование уже идёт!"); return; }
            if (CurrentTest == null) { MessageBox.Show("Выберите тест"); return; }
            if (!SetUserName()) return;
            if (ResultExists())
            {
                MessageBoxResult result = MessageBox.Show("Результаты за выбранный тест уже существуют", "Начать тест?", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                };
            }
            Testing = true;
            StartTimer();
            ShowTestBoxes();
            foreach (Answer a in User.getInstance().Answers.Answers.Values)
            {
                Loger.Log("Ответ:"+a);
            }
        }

        private bool ResultExists()
        {
            if (StudentCollection.Contains(User.getInstance().GetName()))
            {
                Loger.Log("У студента " + User.getInstance().GetName() + "уже есть результаты ");
                Loger.Log("за тесты ");
                foreach(TestResult testResult in StudentCollection[User.getInstance().GetName()].AllResults)
                {
                    Loger.Log(testResult.TestTitle+":");
                    foreach ( Result Result in testResult.Results)
                    {
                        Loger.Log(Result.Print());
                    }
                }
                Loger.Log("при этом за текущий тест:" + CurrentTest.Title);
                if (StudentCollection[User.getInstance().GetName()].ContainsTestResult(CurrentTest))//cопоставление по ссылкам, а должно быть по стрингам как обычно
                {
                    Loger.Log("Результаты есть. ");
                    return true;
                }
            }
            Loger.Log("Результаты отсутствуют");
            return false;
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
                        SaveTestResult();
                        SaveStudentToRepository();
                        Loger.Log("ответы " + User.getInstance().Result.RightAnswers + User.getInstance().Result.WrongAnswers+ User.getInstance().Result.TimeString);
                        ShowResultBox();
                        NewUserEnter();
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
            if (NamesCB.SelectedItem != null)
                SetUserName();
        }
        private void TestChanged(object sender, SelectionChangedEventArgs e)
        {
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
            Loger.Log("CurrentTest:"+CurrentTest.Title);
            foreach(var item in CurrentTest.GetQuestionCollection().GetQuestions()) { Loger.Log(item.QuestionString); }
            CurrentTest.ShuffleQuestions();
            foreach (var item in CurrentTest.GetQuestionCollection().GetQuestions()) { Loger.Log(item.QuestionString); }
            SetCurrentQuestionAsDefault();
            StopTest();
            ShowCurrentTest();
        }
        void OnCurrentQuestionChange()
        {
            if (CurrentQuestion == null) return;
            Loger.Log("текущий вопрос:"+CurrentQuestion.QuestionString);
            Loger.Log("Правильный ответ:"+CurrentQuestion.RightAnswer);
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
            Loger.Log("SavingTestResult");
            User.getInstance().SetResult();
            string name = User.getInstance().GetName();
            StudentCollection.AddResult(name, CurrentTest, User.getInstance().Result);

        }
        private bool SetUserName()
        {
            if (NamesCB.Text == "") { EmptyNameCB(); return false; }
            User.getInstance().SetName(NamesCB.Text.ToString());
            Loger.Log("Текущий пользователь:" + User.getInstance().GetName());
            return true;
        }
        private void StartTimer()
        {
            Console.WriteLine("TimerStart...");
            timer.Start();
            timer.timer.Tick += TimerTick;//плохо, надо добавить обсервера.
            labelTime.Text =timer.GetElapsedTimeStr();
            Console.WriteLine("TimerStarted.");
        }
        private void StopTimer()
        {
            timer.Stop();
        }
        private void ResetTimer()
        {
            timer. Reset();
            labelTime .Text = timer.GetElapsedTimeStr();
        }
        public void TimerTick(object sender, EventArgs e)
        {
            labelTime.Text = timer.GetElapsedTimeStr();
            User.getInstance().ElapsedTime = timer.GetElapsedTime();
        }
        #endregion
       
        private void Resultsbtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
