using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Repositories.Interfaces;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Service
{
    public interface ITagService
    {
        Task<ResponseObject<IEnumerable<TagResponseModel>>> GetAllTags();
        Task<ResponseObject<TagResponseModel>> GetTagById(int tagId);
        Task<ResponseObject<TagResponseModel>> CreateTag(TagRequestModel request);
        Task<ResponseObject<TagResponseModel>> UpdateTag(int tagId, TagRequestModel request);
        Task<ResponseObject<bool>> DeleteTag(int tagId);
        Task<ResponseObject<IEnumerable<PostResponseModel>>> GetPostsByTagId(int tagId);
        Task<ResponseObject<PostResponseModel>> AddPostToTag(int tagId, int postId);
        Task<ResponseObject<bool>> RemovePostFromTag(int tagId, int postId);
    }
    public class TagService : ITagService
    {
        private readonly TagRepository _tagRepository;
        private readonly PostTagRepository _postTagRepository;
        private readonly PostRepository _postRepository;
        private readonly IMapper _mapper;

        public TagService(TagRepository tagRepository, PostTagRepository postTagRepository,PostRepository postRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _postTagRepository = postTagRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }
        public async Task<ResponseObject<TagResponseModel>> CreateTag(TagRequestModel request)
        {
            var tagEntity = _mapper.Map<Tag>(request);
            await _tagRepository.AddAsync(tagEntity);

            var tagResponse = _mapper.Map<TagResponseModel>(tagEntity);
            return new ResponseObject<TagResponseModel>
            {
                Message = "Tag created successfully",
                Data = tagResponse
            };
        }

        public async Task<ResponseObject<bool>> DeleteTag(int tagId)
        {
            var tag = await _tagRepository.GetById(tagId);
            if (tag == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Tag not found",
                    Data = false
                };
            }

            await _tagRepository.DeleteAsync(tag);

            return new ResponseObject<bool>
            {
                Message = "Tag deleted successfully",
                Data = true
            };
        }

        public async Task<ResponseObject<IEnumerable<TagResponseModel>>> GetAllTags()
        {
            var tags = await _tagRepository.GetAllAsync();
            var tagResponseModels = _mapper.Map<IEnumerable<TagResponseModel>>(tags);

            return new ResponseObject<IEnumerable<TagResponseModel>>
            {
                Message = "Tags retrieved successfully",
                Data = tagResponseModels
            };
        }

        public async Task<ResponseObject<TagResponseModel>> GetTagById(int tagId)
        {
            var tag = await _tagRepository.GetById(tagId);
            if (tag == null)
            {
                return new ResponseObject<TagResponseModel>
                {
                    Message = "Tag not found",
                    Data = null
                };
            }

            var tagResponse = _mapper.Map<TagResponseModel>(tag);

            return new ResponseObject<TagResponseModel>
            {
                Message = "Tag retrieved successfully",
                Data = tagResponse
            };
        }

        public async Task<ResponseObject<TagResponseModel>> UpdateTag(int tagId, TagRequestModel request)
        {
            var tag = await _tagRepository.GetById(tagId);
            if (tag == null)
            {
                return new ResponseObject<TagResponseModel>
                {
                    Message = "Tag not found",
                    Data = null
                };
            }

            _mapper.Map(request, tag);
            await _tagRepository.UpdateAsync(tag);

            var updatedTag = await _tagRepository.GetById(tagId);
            var tagResponse = _mapper.Map<TagResponseModel>(updatedTag);

            return new ResponseObject<TagResponseModel>
            {
                Message = "Tag updated successfully",
                Data = tagResponse
            };
        }

        public async Task<ResponseObject<IEnumerable<PostResponseModel>>> GetPostsByTagId(int tagId)
        {
            var tag = await _tagRepository.GetById(tagId);
            if (tag == null)
            {
                return new ResponseObject<IEnumerable<PostResponseModel>>
                {
                    Message = "Tag not found",
                    Data = null
                };
            }

            var postTags = await _postTagRepository.FindByCondition(x => x.TagId == tagId);
            var postIds = postTags.Select(x => x.PostId).ToList();
            var posts = await _postRepository.FindByCondition(x => postIds.Contains(x.PostId));

            var postResponseModels = _mapper.Map<IEnumerable<PostResponseModel>>(posts);

            return new ResponseObject<IEnumerable<PostResponseModel>>
            {
                Message = "Posts retrieved successfully",
                Data = postResponseModels
            };
        }

        public async Task<ResponseObject<PostResponseModel>> AddPostToTag(int tagId, int postId)
        {
            var tag = await _tagRepository.GetById(tagId);
            if (tag == null)
            {
                return new ResponseObject<PostResponseModel>
                {
                    Message = "Tag not found",
                    Data = null
                };
            }

            var post = await _postRepository.GetById(postId);
            if (post == null)
            {
                return new ResponseObject<PostResponseModel>
                {
                    Message = "Post not found",
                    Data = null
                };
            }

            var postTag = new PostTag
            {
                TagId = tagId,
                PostId = postId
            };

            await _postTagRepository.AddAsync(postTag);

            var postResponse = _mapper.Map<PostResponseModel>(post);

            return new ResponseObject<PostResponseModel>
            {
                Message = "Post added to tag successfully",
                Data = postResponse
            };
        }

        public async Task<ResponseObject<bool>> RemovePostFromTag(int tagId, int postId)
        {
            var tag = await _tagRepository.GetById(tagId);
            if (tag == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Tag not found",
                    Data = false
                };
            }

            var post = await _postRepository.GetById(postId);
            if (post == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Post not found",
                    Data = false
                };
            }

            var postTag = await _postTagRepository.FindSingleByCondition(x => x.TagId == tagId && x.PostId == postId);
            if (postTag == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Post is not associated with the tag",
                    Data = false
                };
            }

            await _postTagRepository.DeleteAsync(postTag);

            return new ResponseObject<bool>
            {
                Message = "Post removed from tag successfully",
                Data = true
            };
        }
    }
}
