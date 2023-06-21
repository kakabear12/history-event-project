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
    public class QuestionDAO
    {
        private readonly HistoryEventDBContext context;
        public QuestionDAO(HistoryEventDBContext context)
        {
            this.context = context;
        }
        public async Task<List<Question>> GetQuestionsAsync()
        {
            try {
                return await context.Questions.ToListAsync();
            }catch (Exception ex) {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<List<Question>> GetQuesttionsFinished()
        {
            try {
                var qts = await context.Questions.Where(q => q.Answers.Count > 0
                && q.Answers.Count(a => a.IsCorrect) == 1
                ).ToListAsync();
                if(qts.Count == 0) {
                    throw new CustomException("Not found the question finished.");
                }
                return qts;
            } catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<Question> CreateQuestion(Question q)
        {
            try
            {
                var newQ = await context.Questions.AddAsync(q);
                await context.SaveChangesAsync();
                return newQ.Entity;
            }catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task DeleteQuestion(int id)
        {
            try
            {
                var questions = context.Questions.FirstOrDefault(q=> q.QuestionId == id);
                if(questions == null)
                {
                    throw new Exception("Not found question");
                }
                if(questions.Answers.Count > 0)
                {
                    foreach (var ans in questions.Answers)
                    {
                        context.Answers.Remove(ans);
                    }
                }
                context.Questions.Remove(questions);
                await context.SaveChangesAsync();

            }catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<Question> UpdateQuestion(Question question) {
            try
            {
                var updateQ = await context.Questions.FirstOrDefaultAsync(q => q.QuestionId == question.QuestionId);
                if(updateQ == null)
                {
                    throw new Exception("Question not found");
                }
                updateQ.DifficultyLevel = question.DifficultyLevel;
                updateQ.QuestionText = question.QuestionText;
                await context.SaveChangesAsync();
                return updateQ;
            }catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
