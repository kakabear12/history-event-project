using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController (IPostService postService, IUserRepository userRepository)
        {
            _postService = postService;
                       
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For get post by id")]
        public async Task<ActionResult<ResponseObject<PostResponseModel>>> GetPostById(int id)
        {
            var response = await _postService.GetPostById(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For get list of posts")]
        public async Task<ActionResult<ResponseObject<IEnumerable<PostResponseModel>>>> GetAllPosts()
        {
            var response = await _postService.GetAllPosts();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For create post")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ResponseObject<PostResponseModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request")]
        public async Task<ActionResult<ResponseObject<PostResponseModel>>> CreatePost(CreatePostRequestModel request)
        {                      
            request.Slug = GenerateSlug(request.Slug);
            var response = await _postService.CreatePost(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For update post by id")]
        public async Task<ActionResult<ResponseObject<bool>>> UpdatePost(int id, UpdatePostRequestModel request)
        {
            var response = await _postService.UpdatePost(id, request);

            if (!response.Data)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpDelete("{id}")]
         [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For delete post by id")]
        public async Task<ActionResult<ResponseObject<bool>>> DeletePost(int id)
        {
            var deleteRequest = new DeletePostRequestModel
            {
                PostId = id
            };

            var response = await _postService.DeletePost(deleteRequest);
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
