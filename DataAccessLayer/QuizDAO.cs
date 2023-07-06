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
    public class QuizDAO
    {
        private readonly HistoryEventDBContext context;
        public QuizDAO(HistoryEventDBContext context)
        {
            this.context = context;
        } 
        public async Task<Quiz> CreateQuiz(int eventId, Quiz quiz)
        {
            try
            {
                var qts = await context.Questions.Include(c => c.Event).Where(q => q.Answers.Count > 1
                && q.Answers.Count(a => a.IsCorrect) == 1 && q.Event.EventId == eventId
                ).ToListAsync();
                if (qts.Count == 0)
                {
                    throw new CustomException("Not found the question");
                }
                if(qts.Count < quiz.NumberQuestion) {
                    throw new CustomException("The number of questions is not enough to create a quiz");
                }
                if(qts.Count(q => q.DifficultyLevel == DifficultyLevel.Hard) < quiz.NumberQuestion * 2 / 10)
                {
                    throw new CustomException("The number of questions in hard level is not enough to create a quiz");
                }
                else if (qts.Count(q => q.DifficultyLevel == DifficultyLevel.Easy) < quiz.NumberQuestion * 5 / 10)
                {
                    throw new CustomException("The number of questions in easy level is not enough to create a quiz");
                }
                else if (qts.Count(q => q.DifficultyLevel == DifficultyLevel.Normal) < quiz.NumberQuestion * 3 / 10)
                {
                    throw new CustomException("The number of questions in normal level is not enough to create a quiz");
                }
                var listEasyQ = qts.Where(q => q.DifficultyLevel == DifficultyLevel.Easy).ToList();
                var listNormalQ = qts.Where(q => q.DifficultyLevel == DifficultyLevel.Normal).ToList();
                var listHardQ = qts.Where(q => q.DifficultyLevel == DifficultyLevel.Hard).ToList();

                var randomQuizE = this.GetRandomQuestions(listEasyQ, quiz.NumberQuestion * 5 / 10);
                var randomQuizN = this.GetRandomQuestions(listNormalQ, quiz.NumberQuestion * 3 / 10);
                var randomQuizH = this.GetRandomQuestions(listHardQ, quiz.NumberQuestion * 2 / 10);
                
                var randomQuiz = randomQuizE.Concat(randomQuizN).Concat(randomQuizH);
               
                var quizFinsished = await context.Quizzes.AddAsync(quiz);
                foreach(Question q in randomQuiz)
                {
                    QuestionQuiz questionQuiz = new QuestionQuiz();
                    if (q.DifficultyLevel == DifficultyLevel.Easy)
                    {
                        questionQuiz = new QuestionQuiz { 
                            Question = q,
                            Quiz = quizFinsished.Entity,
                            QuestionTime = 10
                        };
                    }
                    else if (q.DifficultyLevel == DifficultyLevel.Normal)
                    {
                        questionQuiz = new QuestionQuiz
                        {
                            Question = q,
                            Quiz = quizFinsished.Entity,
                            QuestionTime = 15
                        };
                    }
                    else if (q.DifficultyLevel == DifficultyLevel.Hard)
                    {
                        questionQuiz = new QuestionQuiz
                        {
                            Question = q,
                            Quiz = quizFinsished.Entity,
                            QuestionTime = 20
                        };
                    }
                    await context.QuestionQuizzes.AddAsync(questionQuiz);
                }

                await context.SaveChangesAsync();
                return quizFinsished.Entity;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<Quiz> GetQuizToDo(int quizId)
        {
            try
            {
                var quiz = await context.Quizzes
                .Include(q => q.QuestionQuizzes)
                    .ThenInclude(q => q.Question)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuizId == quizId);
                if(quiz == null)
                {
                    throw new CustomException("Quiz not found");
                }
                if(quiz.StartTime != null || quiz.EndTime != null) 
                {
                    throw new CustomException("You have made this quiz");
                }
                quiz.StartTime = DateTime.Now;
                await context.SaveChangesAsync();
                return quiz;
            }catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<Quiz> GetQuizById(int quizId)
        {
            try
            {
                var quiz = await context.Quizzes.Include(c=> c.User).SingleOrDefaultAsync(c=> c.QuizId == quizId);
                if(quiz == null)
                {
                    throw new CustomException("Quiz not found");
                }
                return quiz;
            }catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task GetResultQuiz(int quizId, int questId, int anserId)
        {
            try
            {
                var quiz = await context.Quizzes.Include(q => q.QuestionQuizzes)
                    .ThenInclude(q => q.Question)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuizId == quizId);

                DateTime currentTime = DateTime.Now;
                TimeSpan timeElapsed = currentTime - quiz.StartTime.Value;
                if (timeElapsed.TotalSeconds >= quiz.Time)
                {
                    throw new CustomException("Exceeded the allotted time");
                }
                if (quiz == null)
                {
                    throw new CustomException("Quiz not found");
                }
                if(quiz.QuestionQuizzes.Any(c=> c.Question.QuestionId == questId) == false)
                {
                    throw new CustomException("Question not existed this quiz");
                }
                var quest = await context.Questions.Include(c => c.Answers).SingleOrDefaultAsync(q => q.QuestionId == questId);
                if (quest == null)
                {
                    throw new CustomException("Question not found");
                }
                if(quest.Answers.Any(c=> c.AnswerId == anserId) == false)
                {
                    throw new CustomException("Answer not found");
                }
                if(quest.Answers.FirstOrDefault(a=> a.IsCorrect == true).AnswerId == anserId)
                {
                    quiz.Score += 1;
                    quiz.EndTime = DateTime.Now;
                }
                else
                {
                    quiz.EndTime = DateTime.Now;
                }
                var user = await context.Users.SingleOrDefaultAsync(c => c.UserId == quiz.User.UserId);
                if (user == null)
                {
                    throw new CustomException("User not found");
                }
                user.TotalQuestion +=1;
                user.TotalScore = quiz.Score;
                await context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<List<Quiz>> GetQuizzessByUserId(int userId)
        {
            try
            {
                var quizzess = await context.Quizzes.Where(c=> c.User.UserId == userId)
                    .OrderByDescending(c=> c.EndTime)
                    .ToListAsync();
                return quizzess;
            }catch(Exception ex)
            {
                throw new CustomException(ex.ToString());
            }
        }
        private List<Question> GetRandomQuestions(List<Question> questions, int number)
        {
            Random random = new Random();
            List<Question> randomQuestions = new List<Question>();

            List<int> indices = Enumerable.Range(0, questions.Count).ToList();

            
            while (randomQuestions.Count < number && indices.Count > 0)
            {
                int randomIndex = random.Next(0, indices.Count);

                Question question = questions[indices[randomIndex]];

                randomQuestions.Add(question);

                indices.RemoveAt(randomIndex);
            }

            return randomQuestions;
        }


    }
}
