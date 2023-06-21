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
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AnswerDAO answerDAO;
        public AnswerRepository(AnswerDAO answerDAO)
        {
            this.answerDAO = answerDAO;
        }
        public async Task<Answer> CreateAnswer(Answer answer)
        {
            return await answerDAO.CreateAnswer(answer);
        }

        public async Task DeleteAnswer(int answerId)
        {
            await answerDAO.DeleteAnswer(answerId);
        }

        public async Task<List<Answer>> GetAnswersByQuestionId(int questionId)
        {
            return await answerDAO.GetAnswersByQuestionId(questionId);
        }

        public async Task<Answer> UpdateAnswer(Answer answer)
        {
            return await answerDAO.UpdateAnswer(answer);
        }
    }
}
