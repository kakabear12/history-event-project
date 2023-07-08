using BusinessObjectsLayer.Models;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuestionDAO questionDAO;
        public QuestionRepository(QuestionDAO questionDAO)
        {
            this.questionDAO = questionDAO;
        }
        public async Task<Question> CreateQuestion(Question q)
        {
            return await questionDAO.CreateQuestion(q);
        }

        public async Task DeleteQuestion(int id)
        {
            await questionDAO.DeleteQuestion(id);
        }

        public async Task<IEnumerable<Question>> GetAllFinishedQuestions()
        {
            return await questionDAO.GetQuesttionsFinished();
        }

        public async Task<IEnumerable<Question>> GetAllQuestions()
        {
            return await questionDAO.GetQuestionsAsync();
        }

        public async Task<Question> GetQuestionById(int id)
        {
            return await questionDAO.GetQuestionById(id);
        }

        public async Task<List<Question>> GetQuestionsByEventId(int eventId)
        {
            return await questionDAO.GetQuestionsByEventId(eventId);
        }

        public async Task<Question> UpdateQuestion(Question q)
        {
            return await questionDAO.UpdateQuestion(q);
        }
    }
}
