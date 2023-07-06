using AutoMapper;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/postmetas")]
    [ApiController]
    public class PostMetaController : ControllerBase
    {
        private readonly IPostMetaService _postMetaService;

        public PostMetaController(IPostMetaService postMetaService)
        {
            _postMetaService = postMetaService;
        }

        

        [HttpGet("{postId}/{metaId}")]       
        [SwaggerOperation(Summary = "Get post meta by post ID and meta ID")]
        public async Task<ActionResult<ResponseObject<PostMetaResponseModel>>> GetPostMetaById(int postId, int metaId)
        {
            var response = await _postMetaService.GetPostMetaById(postId, metaId);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Create post meta")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ResponseObject<PostMetaResponseModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request")]
        public async Task<ActionResult<ResponseObject<PostMetaResponseModel>>> CreatePostMeta(PostMetaRequestModel request)
        {
            var response = await _postMetaService.CreatePostMeta(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Update post meta")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ResponseObject<PostMetaResponseModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request")]
        public async Task<ActionResult<ResponseObject<PostMetaResponseModel>>> UpdatePostMeta(int id, PostMetaRequestModel request)
        {
            var response = await _postMetaService.UpdatePostMeta(id, request);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{postId}/{metaId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Delete post meta by post ID and meta ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ResponseObject<bool>))]
        public async Task<ActionResult<ResponseObject<bool>>> DeletePostMeta(int postId, int metaId)
        {
            var response = await _postMetaService.DeletePostMeta(postId, metaId);
            if (!response.Data)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get post meta with image by ID")]
        public async Task<ActionResult<ResponseObject<PostMetaResponseModel>>> GetPostMetaWithImageById(int id)
        {
            var response = await _postMetaService.GetPostMetaWithImageById(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpGet]        
        [SwaggerOperation(Summary = "Get all post metas")]
        public async Task<ActionResult<ResponseObject<IEnumerable<PostMetaResponseModel>>>> GetAllPostMeta()
        {
            var response = await _postMetaService.GetAllPostMeta();
            return Ok(response);
        }


        [HttpGet("post/{postId}/meta")]       
        [SwaggerOperation(Summary = "Get post meta by post ID")]
        public async Task<ActionResult<ResponseObject<IEnumerable<PostMetaResponseModel>>>> GetPostMetaByPostId(int postId)
        {
            var response = await _postMetaService.GetPostMetaByPostId(postId);
            if (response.Data == null || !response.Data.Any())
            {
                return NotFound(response);
            }

            return Ok(response);
        }



    }
}
