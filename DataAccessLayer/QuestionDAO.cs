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
                var qts = await context.Questions.Where(q => q.Answers.Count > 0 &&
                q.Answers.Any(a=> a.IsCorrect == true)
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
        public
    }
}
