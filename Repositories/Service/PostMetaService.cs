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
    public interface IPostMetaService
    {
        Task<ResponseObject<IEnumerable<PostMetaResponseModel>>> GetPostMetaByPostId(int PostId);
        Task<ResponseObject<PostMetaResponseModel>> GetPostMetaById(int postId, int Id);
        Task<ResponseObject<PostMetaResponseModel>> GetPostMetaWithImageById( int Id);
        Task<ResponseObject<PostMetaResponseModel>> CreatePostMeta(PostMetaRequestModel request);
        Task<ResponseObject<PostMetaResponseModel>> UpdatePostMeta(int id, PostMetaRequestModel request);
        Task<ResponseObject<bool>> DeletePostMeta(int postId, int Id);
        Task<ResponseObject<IEnumerable<PostMetaResponseModel>>> GetAllPostMeta();
    }
    public class PostMetaService : IPostMetaService
    {
        private readonly PostMetaRepository _postMetaRepository;
        private readonly IMapper _mapper;

        public PostMetaService(PostMetaRepository postMetaRepository, IMapper mapper)
        {
            _postMetaRepository = postMetaRepository;
            _mapper = mapper;
        }   

        public async Task<ResponseObject<PostMetaResponseModel>> CreatePostMeta(PostMetaRequestModel request)
        {
            var postmetaEntity = _mapper.Map<PostMeta>(request);
            await _postMetaRepository.AddAsync(postmetaEntity);

            var postmetaResponse = _mapper.Map<PostMetaResponseModel>(postmetaEntity);
            return new ResponseObject<PostMetaResponseModel>
            {
                Message ="PostMeta Created successfully",
                Data= postmetaResponse
            };
        }

        public async Task<ResponseObject<bool>> DeletePostMeta(int postId, int Id)
        {
            var postMeta = await _postMetaRepository.GetPostMetaById(postId, Id);

            if (postMeta == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "PostMeta not found",
                    Data = false
                };
            }

            await _postMetaRepository.DeleteAsync(postMeta);

            return new ResponseObject<bool>
            {
                Message = "PostMeta deleted successfully",
                Data = true
            };
        }

        public async Task<ResponseObject<IEnumerable<PostMetaResponseModel>>> GetAllPostMeta()
        {
            var postMetas = await _postMetaRepository.GetAllAsync();
            var postMetaResponseModels = _mapper.Map<IEnumerable<PostMetaResponseModel>>(postMetas);

            return new ResponseObject<IEnumerable<PostMetaResponseModel>>
            {
                Message = "PostMetas retrieved successfully",
                Data = postMetaResponseModels
            };
        }

        public async Task<ResponseObject<PostMetaResponseModel>> GetPostMetaById(int postId, int Id)
        {
            var postMeta = await _postMetaRepository.GetPostMetaById(postId, Id);

            if (postMeta == null)
            {
                return new ResponseObject<PostMetaResponseModel>
                {
                    Message = "PostMeta not found",
                    Data = null
                };
            }

            var postMetaResponseModel = _mapper.Map<PostMetaResponseModel>(postMeta);

            return new ResponseObject<PostMetaResponseModel>
            {
                Message = "PostMeta retrieved successfully",
                Data = postMetaResponseModel
            };
        }

        public async Task<ResponseObject<IEnumerable<PostMetaResponseModel>>> GetPostMetaByPostId(int PostId)
        {
            var postMetas = await _postMetaRepository.GetPostMetaByPostId(PostId);
            var postMetaResponseModels = _mapper.Map<IEnumerable<PostMetaResponseModel>>(postMetas);

            return new ResponseObject<IEnumerable<PostMetaResponseModel>>
            {
                Message = "PostMetas retrieved successfully",
                Data = postMetaResponseModels
            };
        }

        public async Task<ResponseObject<PostMetaResponseModel>> GetPostMetaWithImageById(int Id)
        {
            var postmeta = await _postMetaRepository.GetPostMetaWithImageById(Id);

            if (postmeta == null)
            {
                return new ResponseObject<PostMetaResponseModel>
                {
                    Message = "Postmeta not found",
                    Data = null
                };
            }

            var postmetaResponseModel = _mapper.Map<PostMetaResponseModel>(postmeta);


            return new ResponseObject<PostMetaResponseModel>
            {
                Message = "Postmeta retrieved successfully",
                Data = postmetaResponseModel
            };
        }

        public async Task<ResponseObject<PostMetaResponseModel>> UpdatePostMeta(int id, PostMetaRequestModel request)
        {
            var postMeta = await _postMetaRepository.GetById(id);

            if (postMeta == null)
            {
                return new ResponseObject<PostMetaResponseModel>
                {
                    Message = "PostMeta not found",
                    Data = null
                };
            }

            // Update the properties of the postMeta entity
            postMeta.Keys = request.Keys;
            postMeta.Contents = request.Contents;

            await _postMetaRepository.UpdateAsync(postMeta);

            var postMetaResponseModel = _mapper.Map<PostMetaResponseModel>(postMeta);

            return new ResponseObject<PostMetaResponseModel>
            {
                Message = "PostMeta updated successfully",
                Data = postMetaResponseModel
            };
        }
    }
}
