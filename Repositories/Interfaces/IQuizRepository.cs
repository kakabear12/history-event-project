using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz> CreateQuiz(int eventId, Quiz quiz);
        Task<Quiz> GetQuizToDo(int quizId);
        Task GetResultQuiz(int quizId, int questId, int anserId);
        Task<Quiz> GetQuizById(int quizId);
    }
}
