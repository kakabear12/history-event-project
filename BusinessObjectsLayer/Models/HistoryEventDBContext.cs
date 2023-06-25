using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class HistoryEventDBContext : DbContext
    {
        public HistoryEventDBContext()
        {
        }

        public HistoryEventDBContext(DbContextOptions<HistoryEventDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<PostContent> PostContents { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionQuiz> QuestionQuizzes { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<AccessTokenBlacklist> AccessTokenBlacklists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDB"));
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
