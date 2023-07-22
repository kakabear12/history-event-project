using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using System.Collections.Generic;

namespace WebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, User>();
            CreateMap<User, CurrentUserResponse>();
            CreateMap<User, UserReponse>();
            CreateMap<UpdateUserModel, User>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<CreateQuestionRequest, Question>();
            CreateMap<Question, QuestionResponse>();
            CreateMap<UpdateQuestionRequest, Question>();
            CreateMap<Answer, AnswerResponse>();
            CreateMap<CreateAnswerRequest, Answer>();
            CreateMap<UpdateAnswerRequest, Answer>();
            CreateMap<CreateQuizRequest, Quiz>();
            CreateMap<Quiz, QuizResponse>();
            CreateMap<Quiz, GetQuizResponse>();
            CreateMap<QuestionQuiz, QuestionQuizResponse>();
            CreateMap<Question, GetQuestionResponse>();
            CreateMap<Answer, GetAnswerToDoResponse>();
            CreateMap<Quiz, QuizResultResponse>();

            //events
            CreateMap<EventRequestModel, Event>();
            CreateMap<Event, EventResponseModel>();

            //posts
            CreateMap<CreatePostRequestModel, Post>();
            CreateMap<UpdatePostRequestModel, Post>();
            CreateMap<DeletePostRequestModel,Post>();
            CreateMap<Post, PostResponseModel>();
            CreateMap<Post, PostResponse>();

            //postmeta
            CreateMap<PostMetaRequestModel, PostMeta>();
            CreateMap<PostMeta, PostMetaResponseModel>();

            //Tag
            CreateMap<TagRequestModel, Tag>();
            CreateMap<Tag, TagResponseModel>();

            //PostComment
            CreateMap<PostCommentRequestModel, PostComment>();
            CreateMap<PostComment, PostCommentResponseModel>();

            //Image
            CreateMap<ImageRequestModel, Image>();
            CreateMap<Image, ImageResponseModel>();
        }
    }
}
