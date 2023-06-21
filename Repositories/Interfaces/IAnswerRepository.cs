using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<List<Answer>> GetAnswersByQuestionId(int questionId);
        Task<Answer> CreateAnswer(Answer answer);
        Task<Answer> UpdateAnswer(Answer answer);
        Task DeleteAnswer(int answerId);

    }
}
