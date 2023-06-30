using AutoMapper;
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

    public interface IPostCommentService
    {
        Task<ResponseObject<IEnumerable<PostCommentResponseModel>>> GetAllPostComments();
        Task<ResponseObject<PostCommentResponseModel>> GetPostCommentById(int commentId);
        Task<ResponseObject<PostCommentResponseModel>> CreatePostComment(PostCommentRequestModel request);
        Task<ResponseObject<PostCommentResponseModel>> UpdatePostComment(int commentId, PostCommentRequestModel request);
        Task<ResponseObject<bool>> DeletePostComment(int commentId);
        Task<ResponseObject<IEnumerable<PostCommentResponseModel>>> GetAllChildComments(int commentId);
        Task<ResponseObject<PostCommentResponseModel>> GetChildCommentById(int commentId, int childCommentId);
        Task<ResponseObject<PostCommentResponseModel>> CreateChildComment(int commentId, PostCommentRequestModel request);
        Task<ResponseObject<PostCommentResponseModel>> UpdateChildComment(int commentId, int childCommentId, PostCommentRequestModel request);
        Task<ResponseObject<bool>> DeleteChildComment(int commentId, int childCommentId);
    }
    public class PostCommentService : IPostCommentService
    {
        private readonly PostCommentRepository _postCommentRepository;
        private readonly IMapper _mapper;

        public PostCommentService(PostCommentRepository postCommentRepository, IMapper mapper)
        {
            _postCommentRepository = postCommentRepository;
            _mapper = mapper;
        }
        public async Task<ResponseObject<PostCommentResponseModel>> CreatePostComment(PostCommentRequestModel request)
        {
            var commentEntity = _mapper.Map<PostComment>(request);
            await _postCommentRepository.AddAsync(commentEntity);

            var commentResponse = _mapper.Map<PostCommentResponseModel>(commentEntity);
            return new ResponseObject<PostCommentResponseModel>
            {
                Message = "Post comment created successfully",
                Data = commentResponse
            };
        }

        public async Task<ResponseObject<bool>> DeletePostComment(int commentId)
        {
            var comment = await _postCommentRepository.GetById(commentId);
            if (comment == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Post comment not found",
                    Data = false
                };
            }

            await _postCommentRepository.DeleteAsync(comment);

            return new ResponseObject<bool>
            {
                Message = "Post comment deleted successfully",
                Data = true
            };
        }

        public async Task<ResponseObject<IEnumerable<PostCommentResponseModel>>> GetAllPostComments()
        {
            var comments = await _postCommentRepository.GetAllAsync();
            var commentResponseModels = _mapper.Map<IEnumerable<PostCommentResponseModel>>(comments);

            return new ResponseObject<IEnumerable<PostCommentResponseModel>>
            {
                Message = "Post comments retrieved successfully",
                Data = commentResponseModels
            };
        }

        public async Task<ResponseObject<PostCommentResponseModel>> GetPostCommentById(int commentId)
        {
            var comment = await _postCommentRepository.GetById(commentId);
            if (comment == null)
            {
                return new ResponseObject<PostCommentResponseModel>
                {
                    Message = "Post comment not found",
                    Data = null
                };
            }

            var commentResponse = _mapper.Map<PostCommentResponseModel>(comment);

            return new ResponseObject<PostCommentResponseModel>
            {
                Message = "Post comment retrieved successfully",
                Data = commentResponse
            };
        }

        public async Task<ResponseObject<PostCommentResponseModel>> UpdatePostComment(int commentId, PostCommentRequestModel request)
        {
            var comment = await _postCommentRepository.GetById(commentId);
            if (comment == null)
            {
                return new ResponseObject<PostCommentResponseModel>
                {
                    Message = "Post comment not found",
                    Data = null
                };
            }

            _mapper.Map(request, comment);
            await _postCommentRepository.UpdateAsync(comment);

            var updatedComment = await _postCommentRepository.GetById(commentId);
            var commentResponse = _mapper.Map<PostCommentResponseModel>(updatedComment);

            return new ResponseObject<PostCommentResponseModel>
            {
                Message = "Post comment updated successfully",
                Data = commentResponse
            };
        }

        public async Task<ResponseObject<IEnumerable<PostCommentResponseModel>>> GetAllChildComments(int commentId)
        {
            var comment = await _postCommentRepository.GetById(commentId);
            if (comment == null)
            {
                return new ResponseObject<IEnumerable<PostCommentResponseModel>>
                {
                    Message = "Post comment not found",
                    Data = null
                };
            }

            var childComments = comment.ChildPostComments;
            var childCommentResponseModels = _mapper.Map<IEnumerable<PostCommentResponseModel>>(childComments);

            return new ResponseObject<IEnumerable<PostCommentResponseModel>>
            {
                Message = "Child comments retrieved successfully",
                Data = childCommentResponseModels
            };
        }

        public async Task<ResponseObject<PostCommentResponseModel>> GetChildCommentById(int commentId, int childCommentId)
        {
            var comment = await _postCommentRepository.GetById(commentId);
            if (comment == null)
            {
                return new ResponseObject<PostCommentResponseModel>
                {
                    Message = "Post comment not found",
                    Data = null
                };
            }

            var childComment = comment.ChildPostComments.FirstOrDefault(c => c.Id == childCommentId);
            if (childComment == null)
            {
                return new ResponseObject<PostCommentResponseModel>
                {
                    Message = "Child comment not found",
                    Data = null
                };
            }

            var childCommentResponse = _mapper.Map<PostCommentResponseModel>(childComment);

            return new ResponseObject<PostCommentResponseModel>
            {
                Message = "Child comment retrieved successfully",
                Data = childCommentResponse
            };
        }

        public async Task<ResponseObject<PostCommentResponseModel>> CreateChildComment(int commentId, PostCommentRequestModel request)
        {
            var comment = await _postCommentRepository.GetById(commentId);
            if (comment == null)
            {
                return new ResponseObject<PostCommentResponseModel>
                {
                    Message = "Post comment not found",
                    Data = null
                };
            }

            var childCommentEntity = _mapper.Map<PostComment>(request);
            comment.ChildPostComments.Add(childCommentEntity);
            await _postCommentRepository.UpdateAsync(comment);

            var childCommentResponse = _mapper.Map<PostCommentResponseModel>(childCommentEntity);

            return new ResponseObject<PostCommentResponseModel>
            {
                Message = "Child comment created successfully",
                Data = childCommentResponse
            };
        }

        public async Task<ResponseObject<PostCommentResponseModel>> UpdateChildComment(int commentId, int childCommentId, PostCommentRequestModel request)
        {
            var comment = await _postCommentRepository.GetById(commentId);
            if (comment == null)
            {
                return new ResponseObject<PostCommentResponseModel>
                {
                    Message = "Post comment not found",
                    Data = null
                };
            }

            var childComment = comment.ChildPostComments.FirstOrDefault(c => c.Id == childCommentId);
            if (childComment == null)
            {
                return new ResponseObject<PostCommentResponseModel>
                {
                    Message = "Child comment not found",
                    Data = null
                };
            }

            _mapper.Map(request, childComment);
            await _postCommentRepository.UpdateAsync(comment);

            var updatedChildComment = comment.ChildPostComments.FirstOrDefault(c => c.Id == childCommentId);
            var childCommentResponse = _mapper.Map<PostCommentResponseModel>(updatedChildComment);

            return new ResponseObject<PostCommentResponseModel>
            {
                Message = "Child comment updated successfully",
                Data = childCommentResponse
            };
        }

        public async Task<ResponseObject<bool>> DeleteChildComment(int commentId, int childCommentId)
        {
            var comment = await _postCommentRepository.GetById(commentId);
            if (comment == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Post comment not found",
                    Data = false
                };
            }

            var childComment = comment.ChildPostComments.FirstOrDefault(c => c.Id == childCommentId);
            if (childComment == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Child comment not found",
                    Data = false
                };
            }

            comment.ChildPostComments.Remove(childComment);
            await _postCommentRepository.UpdateAsync(comment);

            return new ResponseObject<bool>
            {
                Message = "Child comment deleted successfully",
                Data = true
            };
        }
    }
}
