using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetAllQuestions();
        Task<IEnumerable<Question>> GetAllFinishedQuestions();
        Task<Question> CreateQuestion(Question q);
        Task DeleteQuestion(int id);
        Task<Question> UpdateQuestion(Question q);
        Task<Question> GetQuestionById(int id);
        Task<List<Question>> GetQuestionsByEventId(int eventId);
    }
}
