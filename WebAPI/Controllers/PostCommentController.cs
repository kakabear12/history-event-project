using AutoMapper;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/postcomments")]
    [ApiController]
    public class PostCommentController : ControllerBase
    {
        private readonly IPostCommentService _postCommentService;
        private readonly IMapper _mapper;

        public PostCommentController(IPostCommentService postCommentService, IMapper mapper)
        {
            _postCommentService = postCommentService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Get all post comments")]
        public async Task<ActionResult<ResponseObject<IEnumerable<PostCommentResponseModel>>>> GetAllPostComments()
        {
            var response = await _postCommentService.GetAllPostComments();
            return Ok(response);
        }


        [HttpGet("{commentId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Get a specific post comment by ID")]
        public async Task<ActionResult<ResponseObject<PostCommentResponseModel>>> GetPostCommentById(int commentId)
        {
            var response = await _postCommentService.GetPostCommentById(commentId);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Create a new post comment")]
        public async Task<ActionResult<ResponseObject<PostCommentResponseModel>>> CreatePostComment([FromBody] PostCommentRequestModel request)
        {
            var response = await _postCommentService.CreatePostComment(request);
            return Ok(response);
        }

        [HttpPut("{commentId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Update a post comment by ID")]
        public async Task<ActionResult<ResponseObject<PostCommentResponseModel>>> UpdatePostComment(int commentId, [FromBody] PostCommentRequestModel request)
        {
            var response = await _postCommentService.UpdatePostComment(commentId, request);
            return Ok(response);
        }

        [HttpDelete("{commentId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Delete a post comment by ID")]
        public async Task<ActionResult<ResponseObject<bool>>> DeletePostComment(int commentId)
        {
            var response = await _postCommentService.DeletePostComment(commentId);
            return Ok(response);
        }

        [HttpGet("{commentId}/childcomments")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Get all child comments of a post comment")]
        public async Task<ActionResult<ResponseObject<IEnumerable<PostCommentResponseModel>>>> GetAllChildComments(int commentId)
        {
            var response = await _postCommentService.GetAllChildComments(commentId);
            return Ok(response);
        }

        [HttpGet("{commentId}/childcomments/{childCommentId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Get a specific child comment of a post comment")]
        public async Task<ActionResult<ResponseObject<PostCommentResponseModel>>> GetChildCommentById(int commentId, int childCommentId)
        {
            var response = await _postCommentService.GetChildCommentById(commentId, childCommentId);
            return Ok(response);
        }

        [HttpPost("{commentId}/childcomments")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Create a new child comment for a post comment")]
        public async Task<ActionResult<ResponseObject<PostCommentResponseModel>>> CreateChildComment(int commentId, [FromBody] PostCommentRequestModel request)
        {
            var response = await _postCommentService.CreateChildComment(commentId, request);
            return Ok(response);
        }

        [HttpPut("{commentId}/childcomments/{childCommentId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Update a child comment of a post comment")]
        public async Task<ActionResult<ResponseObject<PostCommentResponseModel>>> UpdateChildComment(int commentId, int childCommentId, [FromBody] PostCommentRequestModel request)
        {
            var response = await _postCommentService.UpdateChildComment(commentId, childCommentId, request);
            return Ok(response);
        }


        [HttpDelete("{commentId}/childcomments/{childCommentId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Delete a child comment of a post comment")]
        public async Task<ActionResult<ResponseObject<bool>>> DeleteChildComment(int commentId, int childCommentId)
        {
            var response = await _postCommentService.DeleteChildComment(commentId, childCommentId);
            return Ok(response);
        }

    }
}
