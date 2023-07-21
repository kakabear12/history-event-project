using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        

        

        

        [HttpGet]
        [SwaggerOperation(Summary = "Get all images")]
        public async Task<ActionResult<ResponseObject<IEnumerable<ImageResponseModel>>>> GetAllImages()
        {
            var response = await _imageService.GetAllAsync();
            return Ok(response);
        }

        


        [HttpPost("tags/{tagId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Add an image to a tag")]
        public async Task<ActionResult<ResponseObject<TagResponseModel>>> AddImageToTag(int tagId, [FromForm] ImageRequestModel imageModel, IFormFile imageFile)
        {
            var response = await _imageService.AddImageToTag(tagId, imageModel, imageFile);
            return Ok(response);
        }

        [HttpPost("postmeta/{postId}/meta/{metaId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Add an image to post meta")]
        public async Task<ActionResult<ResponseObject<PostMetaResponseModel>>> AddImageToPostMeta( int metaId, [FromForm] ImageRequestModel imageModel, IFormFile imageFile)
        {
            var response = await _imageService.AddImageToPostMeta(metaId, imageModel, imageFile);
            return Ok(response);
        }

        [HttpPost("events/{eventId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Add an image to an event")]
        public async Task<ActionResult<ResponseObject<EventResponseModel>>> AddImageToEvent(int eventId, [FromForm] ImageRequestModel imageModel, IFormFile imageFile)
        {
            var response = await _imageService.AddImageToEvent(eventId, imageModel, imageFile);
            return Ok(response);
        }


        [HttpPost("posts/{postId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Add an image to a post")]
        public async Task<ActionResult<ResponseObject<PostResponseModel>>> AddImageToPost(int postId, [FromForm] ImageRequestModel imageModel, IFormFile imageFile)
        {
            var response = await _imageService.AddImageToPost(postId, imageModel, imageFile);
            return Ok(response);
        }


    }
}
