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
        Task<ResponseObject<PostResponseModel>> GetPostByIdForEditor(int id, int editorUserId);
        Task<ResponseObject<IEnumerable<PostResponseModel>>> GetAllPosts();
        Task<ResponseObject<IEnumerable<PostResponseModel>>> GetAllPostsForEditor(int editorUserId);
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
        private readonly ImageRepository _imageRepository;
        private readonly PostMetaRepository _postMetaRepository;


        public PostService(PostMetaRepository postMetaRepository, ImageRepository imageRepository,PostRepository postRepository, UsersRepository userRepository, CatesgoryRepository categoryRepository,EventsRepository eventRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _imageRepository= imageRepository;
            _postMetaRepository= postMetaRepository;
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

            // Xóa các bản ghi liên kết trong bảng "CategoryPost" của bài viết
            post.Categories?.Clear();

            // Lấy danh sách các Category từ database và thêm vào bài viết
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
            // Gán danh sách Category mới vào Post
            post.Categories = categories;

            // Xóa các bản ghi liên kết trong bảng "EventPost" của bài viết
            post.Events?.Clear();

            // Lấy danh sách các Event từ database và thêm vào bài viết
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
            // Gán danh sách Event mới vào Post
            post.Events = events;

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


            // Lấy danh sách tên category và gán vào postResponseModel
            var categoryNames = post.Categories?.Select(category => category.CategoryName).ToList();
            postResponseModel.CategoryNames = categoryNames;


            // Lấy danh sách tên event và gán vào postResponseModel
            var eventNames = post.Events?.Select(events => events.EventName).ToList();
            postResponseModel.EventNames = eventNames;

            // Lấy danh sách các PostMetaResponseModel và gán vào postResponseModel
            var postMetas = _mapper.Map<List<PostMetaResponseModel>>(post.PostMetas);
            postResponseModel.PostMetas = postMetas;

            // Lấy danh sách các ImageResponseModel và gán vào postResponseModel
            var images = _mapper.Map<List<ImageResponseModel>>(post.Images);
            postResponseModel.Images = images;
            return new ResponseObject<PostResponseModel>
            {
                Message = "Post retrieved successfully",
                Data = postResponseModel
            };
           

        }

        public async Task<ResponseObject<PostResponseModel>> GetPostByIdForEditor(int id, int editorUserId)
        {
            // Get the user's post by ID
            var post = await _postRepository.GetPostByIdForEditor(id, editorUserId);

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


            // Lấy danh sách tên category và gán vào postResponseModel
            var categoryNames = post.Categories?.Select(category => category.CategoryName).ToList();
            postResponseModel.CategoryNames = categoryNames;


            // Lấy danh sách tên event và gán vào postResponseModel
            var eventNames = post.Events?.Select(events => events.EventName).ToList();
            postResponseModel.EventNames = eventNames;

            // Lấy danh sách các PostMetaResponseModel và gán vào postResponseModel
            var postMetas = _mapper.Map<List<PostMetaResponseModel>>(post.PostMetas);
            postResponseModel.PostMetas = postMetas;

            // Lấy danh sách các ImageResponseModel và gán vào postResponseModel
            var images = _mapper.Map<List<ImageResponseModel>>(post.Images);
            postResponseModel.Images = images;
            return new ResponseObject<PostResponseModel>
            {
                Message = "Post retrieved successfully",
                Data = postResponseModel
            };


        }



        public async Task<ResponseObject<IEnumerable<PostResponseModel>>> GetAllPosts()
        {
            var posts = await _postRepository.GetAllAsyncs();
            var postResponseModels = new List<PostResponseModel>();

            foreach (var post in posts)
            {
                var postResponseModel = _mapper.Map<PostResponseModel>(post);

                // Truy xuất thông tin người dùng từ repository
                var author = await _userRepository.GetUserById(post.AuthorId);               
                postResponseModel.AuthorName = author?.Name;


                // Use the CategoryRepository to get the categories by name
                var categoryNames = new List<string>();
                if (post.Categories != null && post.Categories.Any())
                {
                    foreach (var category in post.Categories)
                    {
                        var categoryEntity = await _categoryRepository.GetCategoryByName(category.CategoryName);
                        if (categoryEntity != null)
                        {
                            categoryNames.Add(categoryEntity.CategoryName);
                        }
                    }
                }
                postResponseModel.CategoryNames = categoryNames;

                // Lấy danh sách tên event và gán vào postResponseModel
                var eventNames = post.Events?.Select(events => events.EventName).ToList();
                 postResponseModel.EventNames = eventNames;

                 // Lấy danh sách các PostMetaResponseModel và gán vào postResponseModel
                  var postMetas = _mapper.Map<List<PostMetaResponseModel>>(post.PostMetas);
                  postResponseModel.PostMetas = postMetas;

                 // Lấy danh sách các ImageResponseModel và gán vào postResponseModel
                   var images = _mapper.Map<List<ImageResponseModel>>(post.Images);
                   postResponseModel.Images = images;
                postResponseModels.Add(postResponseModel);
            }

            return new ResponseObject<IEnumerable<PostResponseModel>>
            {
                Message = "Posts retrieved successfully",
                Data = postResponseModels
            };
        }
        public async Task<ResponseObject<IEnumerable<PostResponseModel>>> GetAllPostsForEditor(int editorUserId)
        {
            // Get the user's posts
            var posts = await _postRepository.GetPostsByAuthorId(editorUserId);
            var postResponseModels = new List<PostResponseModel>();

            foreach (var post in posts)
            {
                var postResponseModel = _mapper.Map<PostResponseModel>(post);

                // Truy xuất thông tin người dùng từ repository
                var author = await _userRepository.GetUserById(post.AuthorId);
                postResponseModel.AuthorName = author?.Name;

                // Use the CategoryRepository to get the categories by name
                var categoryNames = new List<string>();
                if (post.Categories != null && post.Categories.Any())
                {
                    foreach (var category in post.Categories)
                    {
                        var categoryEntity = await _categoryRepository.GetCategoryByName(category.CategoryName);
                        if (categoryEntity != null)
                        {
                            categoryNames.Add(categoryEntity.CategoryName);
                        }
                    }
                }
                postResponseModel.CategoryNames = categoryNames;

                // Lấy danh sách tên event và gán vào postResponseModel
                var eventNames = post.Events?.Select(events => events.EventName).ToList();
                postResponseModel.EventNames = eventNames;

                // Lấy danh sách các PostMetaResponseModel và gán vào postResponseModel
                var postMetas = _mapper.Map<List<PostMetaResponseModel>>(post.PostMetas);
                postResponseModel.PostMetas = postMetas;

                // Lấy danh sách các ImageResponseModel và gán vào postResponseModel
                var images = _mapper.Map<List<ImageResponseModel>>(post.Images);
                postResponseModel.Images = images;
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


                // Lấy danh sách tên category và gán vào postResponseModel
                var categoryNames = post.Categories?.Select(category => category.CategoryName).ToList();
                postResponseModel.CategoryNames = categoryNames;

                // Lấy danh sách tên event và gán vào postResponseModel
                var eventNames = post.Events?.Select(events => events.EventName).ToList();
                postResponseModel.EventNames = eventNames;

                // Lấy danh sách các PostMetaResponseModel và gán vào postResponseModel
                var postMetas = _mapper.Map<List<PostMetaResponseModel>>(post.PostMetas);
                postResponseModel.PostMetas = postMetas;

                // Lấy danh sách các ImageResponseModel và gán vào postResponseModel
                var images = _mapper.Map<List<ImageResponseModel>>(post.Images);
                postResponseModel.Images = images;
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


                    // Lấy danh sách tên category và gán vào postResponseModel
                    var categoryNames = post.Categories?.Select(category => category.CategoryName).ToList();
                    postResponseModel.CategoryNames = categoryNames;

                    // Lấy danh sách tên event và gán vào postResponseModel
                    var eventNames = post.Events?.Select(events => events.EventName).ToList();
                    postResponseModel.EventNames = eventNames;

                    // Lấy danh sách các PostMetaResponseModel và gán vào postResponseModel
                    var postMetas = _mapper.Map<List<PostMetaResponseModel>>(post.PostMetas);
                    postResponseModel.PostMetas = postMetas;

                    // Lấy danh sách các ImageResponseModel và gán vào postResponseModel
                    var images = _mapper.Map<List<ImageResponseModel>>(post.Images);
                    postResponseModel.Images = images;
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

                    // Lấy danh sách tên category và gán vào postResponseModel
                    var categoryNames = post.Categories?.Select(category => category.CategoryName).ToList();
                    postResponseModel.CategoryNames = categoryNames;

                    // Lấy danh sách tên event và gán vào postResponseModel
                    var eventNames = post.Events?.Select(events => events.EventName).ToList();
                    postResponseModel.EventNames = eventNames;

                    // Lấy danh sách các PostMetaResponseModel và gán vào postResponseModel
                    var postMetas = _mapper.Map<List<PostMetaResponseModel>>(post.PostMetas);
                    postResponseModel.PostMetas = postMetas;

                    // Lấy danh sách các ImageResponseModel và gán vào postResponseModel
                    var images = _mapper.Map<List<ImageResponseModel>>(post.Images);
                    postResponseModel.Images = images;
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
