using BusinessObjectsLayer.Models;
using DataAccessLayer;
using DTOs.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Interfaces;
using Repositories.JWT;
using Repositories.Repository;
using Repositories.Service;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HistoryEventDBContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();

            

            services.AddScoped<QuizDAO>();
            services.AddScoped<AnswerDAO>();
            services.AddScoped<EventDAO>();
            services.AddScoped<UserDAO>();
            services.AddScoped<QuestionDAO>();
            services.AddScoped<RefreshTokenDAO>();
            services.AddScoped<AccessTokenBlacklistDAO>();
            services.AddScoped<CategoryDAO>();
            services.AddScoped<TokenGenerator>();
            services.AddScoped<AccessTokenBlacklistFilter>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<EventsRepository>();
            services.AddScoped<IEventService, EventService>();

            services.AddScoped<PostRepository>();
            services.AddScoped<IPostService, PostService>();

            services.AddScoped<PostMetaRepository>();
            services.AddScoped<IPostMetaService, PostMetaService>();


            services.AddScoped<PostTagRepository>();
            services.AddScoped<TagRepository>();
            services.AddScoped<ITagService, TagService>();

            services.AddScoped<PostCommentRepository>();
            services.AddScoped<IPostCommentService, PostCommentService>();


            services.AddScoped<ImageRepository>();
            services.AddScoped<IImageService, ImageService>();

            services.AddScoped<UsersRepository>();


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Type");
                });
            });
            services.AddControllersWithViews();
            services.AddAutoMapper
                    (typeof(AutoMapperProfile).Assembly);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options => {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = Configuration["JWT:Issuer"],
                         ValidAudience = Configuration["JWT:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                     };
                 });
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var token = context.SecurityToken as JwtSecurityToken;

                        if (token != null && token.ValidTo < DateTime.UtcNow)
                        {
                            context.Fail("Token has expired.");
                        }
                    }
                };
            });

            services.AddControllers();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });
            /*app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1")

            ) ;*/
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1");

                // Set the authorization options for Swagger UI
                c.DocExpansion(DocExpansion.None);
                c.DefaultModelsExpandDepth(-1);
                c.EnableDeepLinking();
                c.EnableValidator();
                c.DisplayRequestDuration();
                c.EnableFilter();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
