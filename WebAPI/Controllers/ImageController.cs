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

        [HttpPost]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Create an image")]
        public async Task<ActionResult<ResponseObject<ImageResponseModel>>> CreateImage([FromForm] ImageRequestModel imageModel, IFormFile imageFile)
        {
            var response = await _imageService.CreateAsync(imageModel, imageFile);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Update an image by ID")]
        public async Task<ActionResult<ResponseObject<ImageResponseModel>>> UpdateImage(int id, [FromForm] ImageRequestModel imageModel, IFormFile imageFile)
        {
            var response = await _imageService.UpdateImage(id, imageModel, imageFile);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get an image by ID")]
        public async Task<ActionResult<ResponseObject<ImageResponseModel>>> GetImageById(int id)
        {
            var response = await _imageService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all images")]
        public async Task<ActionResult<ResponseObject<IEnumerable<ImageResponseModel>>>> GetAllImages()
        {
            var response = await _imageService.GetAllAsync();
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Delete an image by ID")]
        public async Task<ActionResult<ResponseObject<bool>>> DeleteImage(int id)
        {
            var response = await _imageService.DeleteAsync(id);
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
        public async Task<ActionResult<ResponseObject<PostMetaResponseModel>>> AddImageToPostMeta(int postId, int metaId, [FromForm] ImageRequestModel imageModel, IFormFile imageFile)
        {
            var response = await _imageService.AddImageToPostMeta(postId, metaId, imageModel, imageFile);
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


    }
}
