using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [HttpGet]     
        [SwaggerOperation(Summary = "Get all tags")]
        public async Task<ActionResult<ResponseObject<IEnumerable<TagResponseModel>>>> GetAllTags()
        {
            var response = await _tagService.GetAllTags();
            return Ok(response);
        }

        [HttpGet("{tagId}")]        
        [SwaggerOperation(Summary = "Get tag by ID")]
        public async Task<ActionResult<ResponseObject<TagResponseModel>>> GetTagById(int tagId)
        {
            var response = await _tagService.GetTagById(tagId);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Create tag")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ResponseObject<TagResponseModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request")]
        public async Task<ActionResult<ResponseObject<TagResponseModel>>> CreateTag(TagRequestModel request)
        {
            request.Slug = GenerateSlug(request.Slug);
            var response = await _tagService.CreateTag(request);
            return Ok(response);
        }

        [HttpPut("{tagId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Update tag")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ResponseObject<TagResponseModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request")]
        public async Task<ActionResult<ResponseObject<TagResponseModel>>> UpdateTag(int tagId, TagRequestModel request)
        {
            var response = await _tagService.UpdateTag(tagId, request);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{tagId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Delete tag by ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ResponseObject<bool>))]
        public async Task<ActionResult<ResponseObject<bool>>> DeleteTag(int tagId)
        {
            var response = await _tagService.DeleteTag(tagId);
            if (!response.Data)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{tagId}/posts")]       
        [SwaggerOperation(Summary = "Get posts by tag ID")]
        public async Task<ActionResult<ResponseObject<IEnumerable<PostResponseModel>>>> GetPostsByTagId(int tagId)
        {
            var response = await _tagService.GetPostsByTagId(tagId);
            if (response.Data == null || !response.Data.Any())
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("{tagId}/posts/{postId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Add post to tag")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ResponseObject<PostResponseModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request")]
        public async Task<ActionResult<ResponseObject<PostResponseModel>>> AddPostToTag(int tagId, int postId)
        {
            var response = await _tagService.AddPostToTag(tagId, postId);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{tagId}/posts/{postId}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "Remove post from tag")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ResponseObject<bool>))]
        public async Task<ActionResult<ResponseObject<bool>>> RemovePostFromTag(int tagId, int postId)
        {
            var response = await _tagService.RemovePostFromTag(tagId, postId);
            if (!response.Data)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


        private string GenerateSlug(string title)
        {
            string normalizedTitle = NormalizeTitle(title);

            // Loại bỏ các ký tự đặc biệt và thay thế bằng dấu gạch ngang "-"
            string slug = RemoveSpecialCharacters(normalizedTitle);

            // Loại bỏ các từ không cần thiết khỏi slug
            slug = RemoveStopWords(slug);

            // Loại bỏ các dấu gạch ngang liên tiếp
            slug = RemoveDuplicateDashes(slug);

            // Loại bỏ các dấu gạch ngang ở đầu và cuối chuỗi
            slug = slug.Trim('-');

            return slug.ToLower();
        }

        private string NormalizeTitle(string title)
        {
            // Chuyển tiêu đề thành chữ thường
            string normalizedTitle = title.ToLower();

            // Loại bỏ dấu câu và ký tự đặc biệt
            normalizedTitle = RemovePunctuation(normalizedTitle);

            // Loại bỏ dấu từ tiếng Việt
            normalizedTitle = RemoveVietnameseSigns(normalizedTitle);

            return normalizedTitle;
        }

        private string RemovePunctuation(string text)
        {
            // Loại bỏ dấu câu và ký tự đặc biệt
            string[] punctuation = { ",", ".", "!", "?", ";", ":", "'", "\"", "(", ")", "[", "]", "{", "}", "<", ">", "/", "\\" };
            foreach (var character in punctuation)
            {
                text = text.Replace(character, "");
            }

            return text;
        }

        private string RemoveVietnameseSigns(string text)
        {
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            var formD = text.Normalize(NormalizationForm.FormD);
            var result = regex.Replace(formD, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            return result;
        }

        private string RemoveSpecialCharacters(string text)
        {
            // Loại bỏ các ký tự đặc biệt và thay thế bằng dấu gạch ngang "-"
            string slug = Regex.Replace(text, "[^a-zA-Z0-9-]", "-");

            // Loại bỏ các dấu gạch ngang liên tiếp
            slug = Regex.Replace(slug, "-+", "-");

            // Loại bỏ các dấu gạch ngang ở đầu và cuối chuỗi
            slug = slug.Trim('-');

            return slug;
        }

        private string RemoveStopWords(string text)
        {
            // Các từ không cần thiết muốn loại bỏ khỏi slug
            string[] stopWords = { "a", "an", "the", "and", "or", "but", "on", "in", "with", "to" };

            foreach (var word in stopWords)
            {
                // Tìm và thay thế các từ không cần thiết bằng dấu gạch ngang
                text = text.Replace($"-{word}-", "-");
            }

            return text;
        }

        private string RemoveDuplicateDashes(string text)
        {
            // Loại bỏ các dấu gạch ngang liên tiếp
            string slug = Regex.Replace(text, "-+", "-");

            return slug;
        }
    }
}
