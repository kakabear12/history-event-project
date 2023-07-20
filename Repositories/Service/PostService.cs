using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Repositories.Interfaces;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Service
{

    public interface IPostService
    {
        Task<ResponseObject<PostResponseModel>> GetPostById(int id);
        Task<ResponseObject<IEnumerable<PostResponseModel>>> GetAllPosts();
        Task<ResponseObject<PostResponseModel>> CreatePost(User user, CreatePostRequestModel request);
        Task<ResponseObject<bool>> UpdatePost(User user, int id, UpdatePostRequestModel request);
        Task<ResponseObject<bool>> DeletePost(User user, DeletePostRequestModel request);

        Task<ResponseObject<PostResponseModel>> GetPostsByAuthorId(int authorId);

        Task<ResponseObject<IEnumerable<PostResponseModel>>> SearchPostByCategoryName(string categoryName);

        Task<ResponseObject<IEnumerable<PostResponseModel>>> SearchPostByMetaTitle(string keyword);

    }

    public class PostService : IPostService
    {
        private readonly PostRepository _postRepository;
       
        private readonly IMapper _mapper;
        private readonly UsersRepository _userRepository;
        private readonly CatesgoryRepository _categoryRepository;
        private readonly EventsRepository _eventsRepository;

        public PostService(PostRepository postRepository, UsersRepository userRepository, CatesgoryRepository categoryRepository,EventsRepository eventRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _eventsRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<ResponseObject<PostResponseModel>> CreatePost(User user, CreatePostRequestModel request)
        {
            var postEntity = _mapper.Map<Post>(request);
            postEntity.Author = user;

            // Lấy danh sách các Category từ database
            var categories = new List<Category>();
            if (request.CategoryNames != null && request.CategoryNames.Any())
            {
                foreach (var categoryName in request.CategoryNames)
                {
                    var existingCategory = await _categoryRepository.GetCategoryByName(categoryName);
                    if (existingCategory != null)
                    {
                        categories.Add(existingCategory);
                    }
                    // Không tạo mới Category nếu không tồn tại, như yêu cầu của bạn
                }
            }
            // Gán danh sách Category vào Post
            postEntity.Categories = categories;

            // Lấy danh sách các Event từ database
            var events = new List<Event>();
            if (request.EventNames != null && request.EventNames.Any())
            {
                foreach (var eventName in request.EventNames)
                {
                    var existingEvent = await _eventsRepository.GetEventByName(eventName);
                    if (existingEvent != null)
                    {
                        events.Add(existingEvent);
                    }
                    // Không tạo mới Event nếu không tồn tại, như yêu cầu của bạn
                }
            }
            // Gán danh sách Event vào Post
            postEntity.Events = events;

            await _postRepository.AddAsync(postEntity);

            var author = await _userRepository.GetUserById(user.UserId);

            var postResponseModel = _mapper.Map<PostResponseModel>(postEntity);

            postResponseModel.AuthorName = author.Name;
            postResponseModel.CategoryNames = request.CategoryNames;
            postResponseModel.EventNames = request.EventNames;
            return new ResponseObject<PostResponseModel>
            {
                Message = "Post created successfully",
                Data = postResponseModel
            };
        }


        public async Task<ResponseObject<bool>> UpdatePost(User user, int id, UpdatePostRequestModel request)
        {
            var post = await _postRepository.GetById(id);
            post.Author = user;
            if (post == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Post not found",
                    Data = false
                };
            }

             
            _mapper.Map(request, post); // Update the properties of the post entity

            await _postRepository.UpdateAsync(post);

            return new ResponseObject<bool>
            {
                Message = "Post updated successfully",
                Data = true
            };
        }

        public async Task<ResponseObject<bool>> DeletePost(User user, DeletePostRequestModel request)
        {
            var post = await _postRepository.GetById(request.PostId);
            post.Author = user;
            if (post == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Post not found",
                    Data = false
                };
            }

            await _postRepository.DeleteAsync(post);

            return new ResponseObject<bool>
            {
                Message = "Post deleted successfully",
                Data = true
            };
        }

        public async Task<ResponseObject<PostResponseModel>> GetPostById(int id)
        {
            var post = await _postRepository.GetPostById(id);

            if (post == null)
            {
                return new ResponseObject<PostResponseModel>
                {
                    Message = "Post not found",
                    Data = null
                };
            }

            var postResponseModel = _mapper.Map<PostResponseModel>(post);
            // Truy xuất thông tin người dùng từ repository
            var author = await _userRepository.GetUserById(post.AuthorId);
            postResponseModel.AuthorName = author?.Name;
            return new ResponseObject<PostResponseModel>
            {
                Message = "Post retrieved successfully",
                Data = postResponseModel
            };
           

        }
       


        public async Task<ResponseObject<IEnumerable<PostResponseModel>>> GetAllPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            var postResponseModels = new List<PostResponseModel>();

            foreach (var post in posts)
            {
                var postResponseModel = _mapper.Map<PostResponseModel>(post);

                // Truy xuất thông tin người dùng từ repository
                var author = await _userRepository.GetUserById(post.AuthorId);
                postResponseModel.AuthorName = author?.Name;

                postResponseModels.Add(postResponseModel);
            }

            return new ResponseObject<IEnumerable<PostResponseModel>>
            {
                Message = "Posts retrieved successfully",
                Data = postResponseModels
            };
        }

        public async Task<ResponseObject<PostResponseModel>> GetPostsByAuthorId(int authorId)
        {
            var posts = await _postRepository.GetPostsByAuthorId(authorId);
            var postResponseModels = _mapper.Map<PostResponseModel>(posts);

            return new ResponseObject<PostResponseModel>
            {
                Message = "Posts retrieved successfully",
                Data = postResponseModels
            };
        }

        public async Task<ResponseObject<IEnumerable<PostResponseModel>>> SearchPostByCategoryName(string categoryName)
        {
            var posts = await _postRepository.GetPostsByCategoryName(categoryName);
            if (posts == null)
            {
                return new ResponseObject<IEnumerable<PostResponseModel>>
                {
                    Message = "Post not found",
                    Data = null
                };
            }

            var postResponseModels = new List<PostResponseModel>();

            foreach (var post in posts)
            {
                var postResponseModel = _mapper.Map<PostResponseModel>(post);

                // Truy xuất thông tin người dùng từ repository
                var author = await _userRepository.GetUserById(post.AuthorId);
                postResponseModel.AuthorName = author?.Name;

                postResponseModels.Add(postResponseModel);
            }

            return new ResponseObject<IEnumerable<PostResponseModel>>
            {
                Message = $"Posts in category '{categoryName}' retrieved successfully",
                Data = postResponseModels
            };
        }

        public async Task<ResponseObject<IEnumerable<PostResponseModel>>> SearchPostByMetaTitle(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) // Kiểm tra nếu keyword là null hoặc rỗng
            {
                var posts = await _postRepository.GetAllAsync(); // Lấy tất cả các bài post

                var postResponseModels = new List<PostResponseModel>();

                foreach (var post in posts)
                {
                    var postResponseModel = _mapper.Map<PostResponseModel>(post);

                    // Truy xuất thông tin người dùng từ repository
                    var author = await _userRepository.GetUserById(post.AuthorId);
                    postResponseModel.AuthorName = author?.Name;

                    postResponseModels.Add(postResponseModel);
                }

                return new ResponseObject<IEnumerable<PostResponseModel>>
                {
                    Message = "Posts retrieved successfully",
                    Data = postResponseModels
                };
            }
            else
            {
                // Xử lý như cũ khi có keyword
                var posts = await _postRepository.SearchPostsByMetaTitle(keyword);

                if (posts == null)
                {
                    return new ResponseObject<IEnumerable<PostResponseModel>>
                    {
                        Message = "Posts not found",
                        Data = null
                    };
                }

                var postResponseModels = new List<PostResponseModel>();

                foreach (var post in posts)
                {
                    var postResponseModel = _mapper.Map<PostResponseModel>(post);

                    // Truy xuất thông tin người dùng từ repository
                    var author = await _userRepository.GetUserById(post.AuthorId);
                    postResponseModel.AuthorName = author?.Name;

                    postResponseModels.Add(postResponseModel);
                }

                return new ResponseObject<IEnumerable<PostResponseModel>>
                {
                    Message = "Posts retrieved successfully",
                    Data = postResponseModels
                };
            }
        }


    }

}
