﻿using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Service
{

    public interface IPostService
    {
        Task<ResponseObject<PostResponseModel>> GetPostById(int id);
        Task<ResponseObject<IEnumerable<PostResponseModel>>> GetAllPosts();
        Task<ResponseObject<PostResponseModel>> CreatePost(CreatePostRequestModel request);
        Task<ResponseObject<bool>> UpdatePost(int id, UpdatePostRequestModel request);
        Task<ResponseObject<bool>> DeletePost(DeletePostRequestModel request);

        Task<ResponseObject<PostResponseModel>> GetPostsByAuthorId(int authorId);
    }

    public class PostService : IPostService
    {
        private readonly PostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostService(PostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<ResponseObject<PostResponseModel>> CreatePost(CreatePostRequestModel request)
        {
            var postEntity = _mapper.Map<Post>(request);
            await _postRepository.AddAsync(postEntity);

            var postResponseModel = _mapper.Map<PostResponseModel>(postEntity);
            return new ResponseObject<PostResponseModel>
            {
                Message = "Post created successfully",
                Data = postResponseModel
            };
        }

        public async Task<ResponseObject<bool>> UpdatePost(int id, UpdatePostRequestModel request)
        {
            var post = await _postRepository.GetById(id);

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

        public async Task<ResponseObject<bool>> DeletePost(DeletePostRequestModel request)
        {
            var post = await _postRepository.GetById(request.PostId);

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
            var post = await _postRepository.GetById(id);

            if (post == null)
            {
                return new ResponseObject<PostResponseModel>
                {
                    Message = "Post not found",
                    Data = null
                };
            }

            var postResponseModel = _mapper.Map<PostResponseModel>(post);

            return new ResponseObject<PostResponseModel>
            {
                Message = "Post retrieved successfully",
                Data = postResponseModel
            };
        }

        public async Task<ResponseObject<IEnumerable<PostResponseModel>>> GetAllPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            var postResponseModels = _mapper.Map<IEnumerable<PostResponseModel>>(posts);

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
    }

}