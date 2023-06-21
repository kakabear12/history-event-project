using BusinessObjectsLayer.Models;
using DTOs.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AnswerDAO
    {
        private readonly HistoryEventDBContext context;
        public AnswerDAO(HistoryEventDBContext context)
        {
            this.context = context;
        }
        public async Task<List<Answer>> GetAnswersByQuestionId(int questionId)
        {
            try
            {
                var ans = await context.Answers.Where(a=> a.Question.QuestionId == questionId).ToListAsync();
                return ans;
            }catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<Answer> CreateAnswer(Answer answer)
        {
            try {
                var createAnswer = await context.Answers.AddAsync(answer);
                await context.SaveChangesAsync();
                return createAnswer.Entity;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<Answer> UpdateAnswer(Answer answer)
        {
            try
            {
                var updateA = await context.Answers.SingleOrDefaultAsync(a => a.AnswerId == answer.AnswerId);
                if(updateA == null)
                {
                    throw new CustomException("Answer Not found");
                }
                updateA.AnswerText = answer.AnswerText;
                updateA.IsCorrect = answer.IsCorrect;
                await context.SaveChangesAsync();
                return updateA;
            }catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task DeleteAnswer(int id)
        {
            try
            {
                var deleteA = await context.Answers.SingleOrDefaultAsync(a => a.AnswerId == id);
                if (deleteA == null)
                {
                    throw new CustomException("Answer Not found");
                }
                context.Remove(deleteA);
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
