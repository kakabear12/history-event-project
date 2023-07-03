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
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizDAO quizDAO;
        public QuizRepository(QuizDAO quizDAO)
        {
            this.quizDAO = quizDAO;
        }
        public async Task<Quiz> CreateQuiz(int eventId, Quiz quiz)
        {
            return await quizDAO.CreateQuiz(eventId, quiz);
        }

        public async Task<List<Quiz>> GetAllQuizsByUserId(int userId)
        {
            return await quizDAO.GetQuizzessByUserId(userId);
        }

        public async Task<Quiz> GetQuizById(int quizId)
        {
           return await quizDAO.GetQuizById(quizId);
        }

        public async Task<Quiz> GetQuizToDo(int quizId)
        {
            return await quizDAO.GetQuizToDo(quizId);
        }

        public async Task GetResultQuiz(int quizId, int questId, int anserId)
        {
            await quizDAO.GetResultQuiz(quizId, questId, anserId);
        }
    }
}
