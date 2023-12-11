﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppOnWpf
{
    public class QuestionCollection
    {
        private List<Question> Questions=new List<Question>();
        public Question this[int index]
        {
            get
            {
                return Questions[index];
            }
            set
            {
                Questions[index] = value;
            }
        }
        public int QuestionCount
        {
            get
            {
                return Questions.Count;
            }
        }
        public List<Question> GetQuestions()
        {
            return Questions;
        }
        public void AddQuestion(Question question)
        {
            question.Id = QuestionCount;
            question.NumberInTest = QuestionCount ;
            Questions.Add(question);
        }
        public void ShuffleQuestions()
        {
            Random r = new Random();
            for (int i = QuestionCount-1; i > 0; i--)
            {
                int j = r.Next(0, i);
                Swap(i, j);
            }
            for (int i = 0; i < Questions.Count; i++)
            {
              //  Console.WriteLine(Questions[i].NumberInTest+" " + Questions[i].Id );
            }
            ShuffleAnswers();
        }

        private void ShuffleAnswers()
        {
            foreach (Question question in Questions) { question.ShuffleAnswers(); }
        }

        private void Swap(int i, int j)
        {

            Question a = Questions[i];
            Question b = Questions[j];
            int tempNum = a.NumberInTest;
            a.NumberInTest = j;
            b.NumberInTest= tempNum;
            Questions[i] = Questions[j];
            Questions[j] = a;
        }
    }
}
