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
                var ans = await context.Answers.Include(c => c.Question).Where(a=> a.Question.QuestionId == questionId).ToListAsync();
                return ans;
            }catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<Answer> CreateAnswer(Answer answer)
        {
            try {
                var quest = await context.Questions.Include(c=> c.Answers).SingleOrDefaultAsync(c => c.QuestionId == answer.Question.QuestionId);
                if (quest == null)
                {
                    throw new CustomException("Question not found");
                }
                if (quest.Answers.Count > 0 && answer.IsCorrect == true)
                {
                    if(quest.Answers.Any(c=> c.IsCorrect == true)){
                        throw new CustomException("The question has the correct answer");
                    }
                }
                if(quest.Answers.Any(c=> c.AnswerText == answer.AnswerText))
                {
                    throw new CustomException("The answer had existed");
                }
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
                var quest = await context.Questions.Include(c => c.Answers).SingleOrDefaultAsync(c => c.QuestionId == answer.Question.QuestionId);
                if (quest == null)
                {
                    throw new CustomException("Question not found");
                }
                if (quest.Answers.Count > 0 && answer.IsCorrect == true)
                {
                    if (quest.Answers.Any(c => c.IsCorrect == true && c.AnswerId != answer.AnswerId))
                    {
                        throw new CustomException("The question has the correct answer");
                    }
                }
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
