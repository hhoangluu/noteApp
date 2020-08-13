using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;

namespace HLNote
{
    class QuestionService
    {
        private ObservableCollection<Question> _questions;
        private DatabaseConnector<Question> _dbConnector;
        public QuestionService()
        {
            _dbConnector = new DatabaseConnector<Question>();
        }
        public void GetData()
        {
            if (!_dbConnector.HasData())
            {
                var questionList = new List<Question> {
                    new Question { QuestionStr = "Where do you live when you was young?"},
                    new Question { QuestionStr = "Which primary school did you study?"},
                    new Question { QuestionStr = "Who was your first girl/boy friend?"},
                    new Question { QuestionStr = "What is your most favorite food?"},
                    new Question { QuestionStr = "Do you like me?"},
                };
                foreach (var question in questionList)
                    _dbConnector.InsertData(question);
            }
            _questions = _dbConnector.LoadData();
        }
        public QuestionService(Question[] questions)
        {
            _questions = new ObservableCollection<Question>(questions);
        }
        public ObservableCollection<Question> Questions { get { return _questions; } set { _questions = value; } }
        public bool RemoveQuestion(Question question)
        {
            if (_questions == null)
                return false;
            return _questions.Remove(question);
        }
        public void AddQuestion(Question question)
        {
            if (_questions == null)
                return;
            _questions.Add(question);
        }
        public void ReplaceQuestion(Question question)
        {
            if (_questions == null)
                return;
            var preUpdate = _questions.Single(c => c.QuestionId == question.QuestionId);
            var index = _questions.IndexOf(preUpdate);
            _questions[index] = question;
        }

    }
}
