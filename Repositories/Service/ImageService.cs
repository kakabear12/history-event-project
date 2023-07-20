using AutoMapper;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Repositories.Interfaces;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Service
{
    public interface IImageService
    {
       
        
        Task<ResponseObject<IEnumerable<ImageResponseModel>>> GetAllAsync();
       
      
       
        Task<ResponseObject<TagResponseModel>> AddImageToTag(int tagId, ImageRequestModel imageModel, IFormFile imageFile);
        Task<ResponseObject<PostMetaResponseModel>> AddImageToPostMeta(int metaId, ImageRequestModel imageModel, IFormFile imageFile);
        Task<ResponseObject<EventResponseModel>> AddImageToEvent(int eventId, ImageRequestModel imageModel, IFormFile imageFile);

        Task<ResponseObject<PostResponseModel>> AddImageToPost(int postId, ImageRequestModel imageModel, IFormFile imageFile);


    }
    public class ImageService : IImageService
    {
        private readonly ImageRepository _imageRepository;
        private readonly TagRepository _tagRepository;
        private readonly PostMetaRepository _postMetaRepository;
        private readonly EventsRepository _eventRepository;
        private readonly PostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ImageService(
        ImageRepository imageRepository,
        TagRepository tagRepository,
        PostMetaRepository postMetaRepository,
        EventsRepository eventRepository,
         PostRepository postRepository,
        IConfiguration configuration,
        IMapper mapper)
        {
            _imageRepository = imageRepository;
            _tagRepository = tagRepository;
            _postMetaRepository = postMetaRepository;
            _eventRepository = eventRepository;
            _postRepository= postRepository;    
            _configuration = configuration;
            _mapper = mapper;
        }
        


        

        public async Task<ResponseObject<IEnumerable<ImageResponseModel>>> GetAllAsync()
        {
            var images = await _imageRepository.GetAllAsync();
            var imageModels = _mapper.Map<IEnumerable<ImageResponseModel>>(images);

            return new ResponseObject<IEnumerable<ImageResponseModel>>
            {
                Message = "Images retrieved successfully",
                Data = imageModels
            };
        }

        

        


        public async Task<ResponseObject<EventResponseModel>> AddImageToEvent(int eventId, ImageRequestModel imageModel, IFormFile imageFile)
        {
            var existingEvent = await _eventRepository.GetById(eventId);
            if (existingEvent == null)
            {
                return new ResponseObject<EventResponseModel>
                {
                    Message = "Event not found",
                    Data = null
                };
            }

            var imageEntity = _mapper.Map<Image>(imageModel);

            // Validate the image entity or perform any necessary checks
            // ...

            if (imageFile != null && imageFile.Length > 0)
            {
                // Retrieve the Azure Storage connection string and container name from the configuration
                var connectionString = _configuration["AzureStorage:ConnectionString"];
                var containerName = _configuration["AzureStorage:ContainerName"];

                // Create a BlobServiceClient using the connection string
                var blobServiceClient = new BlobServiceClient(connectionString);

                // Get a reference to the container
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Generate a unique file name for the uploaded image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                // Get a reference to the blob
                var blobClient = containerClient.GetBlobClient(fileName);

                // Upload the image file to the blob
                using (var stream = imageFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
                // Đặt loại MIME cho blob (hình ảnh JPEG hoặc JPG)
                var contentType = imageFile.ContentType.ToLower();
                if (contentType == "image/jpeg" || contentType == "image/jpg")
                {
                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    };
                    await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
                }
                else
                {
                    // Nếu không phải là loại MIME hỗ trợ, bạn có thể đặt loại MIME mặc định tùy ý ở đây.
                    // Ví dụ: nếu không xác định được loại MIME chính xác, bạn có thể đặt loại MIME thành "application/octet-stream".
                    // Tùy chỉnh loại MIME này dựa trên yêu cầu của ứng dụng của bạn.
                    var defaultContentType = "application/octet-stream";
                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = defaultContentType
                    };
                    await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
                }

                // Update the Url field in the imageEntity with the Blob SAS URL
                var sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = fileName,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(24), // Set the expiration time for the SAS URL (24 hours in this example)
                    Protocol = SasProtocol.Https
                };
                // Set the permissions for the SAS URL (in this example, read permissions)
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                // Generate the SAS token for the blob
                var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_configuration["AzureStorage:AccountName"], _configuration["AzureStorage:AccountKey"])).ToString();

                // Combine the Blob URL with the SAS token to create the final SAS URL
                imageEntity.Url = blobClient.Uri + "?" + sasToken;
            }

            // Check if existingEvent.Images is null and initialize a new list if it is null
            existingEvent.Images ??= new List<Image>();

            existingEvent.Images.Add(imageEntity);
            await _eventRepository.UpdateAsync(existingEvent);

            var eventResponseModel = _mapper.Map<EventResponseModel>(existingEvent);
            return new ResponseObject<EventResponseModel>
            {
                Message = "Image added to event successfully",
                Data = eventResponseModel
            };
        }


        public async Task<ResponseObject<PostMetaResponseModel>> AddImageToPostMeta( int metaId, ImageRequestModel imageModel, IFormFile imageFile)
        {
            var postMetaEntity = await _postMetaRepository.GetById( metaId);
            if (postMetaEntity == null)
            {
                return new ResponseObject<PostMetaResponseModel>
                {
                    Message = "Post meta not found",
                    Data = null
                };
            }

            var imageEntity = _mapper.Map<Image>(imageModel);

            if (imageFile != null && imageFile.Length > 0)
            {
                // Retrieve the Azure Storage connection string and container name from the configuration
                var connectionString = _configuration["AzureStorage:ConnectionString"];
                var containerName = _configuration["AzureStorage:ContainerName"];

                // Create a BlobServiceClient using the connection string
                var blobServiceClient = new BlobServiceClient(connectionString);

                // Get a reference to the container
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Generate a unique file name for the uploaded image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                // Get a reference to the blob
                var blobClient = containerClient.GetBlobClient(fileName);

                // Upload the image file to the blob
                using (var stream = imageFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
                // Đặt loại MIME cho blob (hình ảnh JPEG hoặc JPG)
                var contentType = imageFile.ContentType.ToLower();
                if (contentType == "image/jpeg" || contentType == "image/jpg")
                {
                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    };
                    await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
                }
                else
                {
                    // Nếu không phải là loại MIME hỗ trợ, bạn có thể đặt loại MIME mặc định tùy ý ở đây.
                    // Ví dụ: nếu không xác định được loại MIME chính xác, bạn có thể đặt loại MIME thành "application/octet-stream".
                    // Tùy chỉnh loại MIME này dựa trên yêu cầu của ứng dụng của bạn.
                    var defaultContentType = "application/octet-stream";
                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = defaultContentType
                    };
                    await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
                }

                // Update the Url field in the imageEntity with the Blob SAS URL
                var sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = fileName,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(24), // Set the expiration time for the SAS URL (24 hours in this example)
                    Protocol = SasProtocol.Https
                };
                // Set the permissions for the SAS URL (in this example, read permissions)
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                // Generate the SAS token for the blob
                var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_configuration["AzureStorage:AccountName"], _configuration["AzureStorage:AccountKey"])).ToString();

                // Combine the Blob URL with the SAS token to create the final SAS URL
                imageEntity.Url = blobClient.Uri + "?" + sasToken;
            }

            postMetaEntity.Images ??= new List<Image>();
            postMetaEntity.Images.Add(imageEntity);
            await _postMetaRepository.UpdateAsync(postMetaEntity);

            var postMetaResponseModel = _mapper.Map<PostMetaResponseModel>(postMetaEntity);
            return new ResponseObject<PostMetaResponseModel>
            {
                Message = "Image added to post meta successfully",
                Data = postMetaResponseModel
            };
        }

        public async Task<ResponseObject<TagResponseModel>> AddImageToTag(int tagId, ImageRequestModel imageModel, IFormFile imageFile)
        {
            var tagEntity = await _tagRepository.GetById(tagId);
            if (tagEntity == null)
            {
                return new ResponseObject<TagResponseModel>
                {
                    Message = "Tag not found",
                    Data = null
                };
            }

            var imageEntity = _mapper.Map<Image>(imageModel);

            if (imageFile != null && imageFile.Length > 0)
            {
                // Retrieve the Azure Storage connection string and container name from the configuration
                var connectionString = _configuration["AzureStorage:ConnectionString"];
                var containerName = _configuration["AzureStorage:ContainerName"];

                // Create a BlobServiceClient using the connection string
                var blobServiceClient = new BlobServiceClient(connectionString);

                // Get a reference to the container
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Generate a unique file name for the uploaded image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                // Get a reference to the blob
                var blobClient = containerClient.GetBlobClient(fileName);

                // Upload the image file to the blob
                using (var stream = imageFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
                // Đặt loại MIME cho blob (hình ảnh JPEG hoặc JPG)
                var contentType = imageFile.ContentType.ToLower();
                if (contentType == "image/jpeg" || contentType == "image/jpg")
                {
                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    };
                    await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
                }
                else
                {
                    // Nếu không phải là loại MIME hỗ trợ, bạn có thể đặt loại MIME mặc định tùy ý ở đây.
                    // Ví dụ: nếu không xác định được loại MIME chính xác, bạn có thể đặt loại MIME thành "application/octet-stream".
                    // Tùy chỉnh loại MIME này dựa trên yêu cầu của ứng dụng của bạn.
                    var defaultContentType = "application/octet-stream";
                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = defaultContentType
                    };
                    await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
                }

                // Update the Url field in the imageEntity with the Blob SAS URL
                var sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = fileName,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(24), // Set the expiration time for the SAS URL (24 hours in this example)
                    Protocol = SasProtocol.Https
                };
                // Set the permissions for the SAS URL (in this example, read permissions)
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                // Generate the SAS token for the blob
                var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_configuration["AzureStorage:AccountName"], _configuration["AzureStorage:AccountKey"])).ToString();

                // Combine the Blob URL with the SAS token to create the final SAS URL
                imageEntity.Url = blobClient.Uri + "?" + sasToken;
            }
            tagEntity.Images ??= new List<Image>();
            tagEntity.Images.Add(imageEntity);
            await _tagRepository.UpdateAsync(tagEntity);

            var tagResponseModel = _mapper.Map<TagResponseModel>(tagEntity);
            return new ResponseObject<TagResponseModel>
            {
                Message = "Image added to tag successfully",
                Data = tagResponseModel
            };
        }

        
        public async Task<ResponseObject<PostResponseModel>> AddImageToPost(int postId, ImageRequestModel imageModel, IFormFile imageFile)
        {
            var postEntity = await _postRepository.GetPostById(postId);
            if (postEntity == null)
            {
                return new ResponseObject<PostResponseModel>
                {
                    Message = "Post not found",
                    Data = null
                };
            }

            var imageEntity = _mapper.Map<Image>(imageModel);

            if (imageFile != null && imageFile.Length > 0)
            {
                // Retrieve the Azure Storage connection string and container name from the configuration
                var connectionString = _configuration["AzureStorage:ConnectionString"];
                var containerName = _configuration["AzureStorage:ContainerName"];

                // Create a BlobServiceClient using the connection string
                var blobServiceClient = new BlobServiceClient(connectionString);

                // Get a reference to the container
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Generate a unique file name for the uploaded image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                // Get a reference to the blob
                var blobClient = containerClient.GetBlobClient(fileName);

                // Upload the image file to the blob
                using (var stream = imageFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
                // Đặt loại MIME cho blob (hình ảnh JPEG hoặc JPG)
                var contentType = imageFile.ContentType.ToLower();
                if (contentType == "image/jpeg" || contentType == "image/jpg")
                {
                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    };
                    await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
                }
                else
                {
                    // Nếu không phải là loại MIME hỗ trợ, bạn có thể đặt loại MIME mặc định tùy ý ở đây.
                    // Ví dụ: nếu không xác định được loại MIME chính xác, bạn có thể đặt loại MIME thành "application/octet-stream".
                    // Tùy chỉnh loại MIME này dựa trên yêu cầu của ứng dụng của bạn.
                    var defaultContentType = "application/octet-stream";
                    var blobHttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = defaultContentType
                    };
                    await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
                }

                // Update the Url field in the imageEntity with the Blob SAS URL
                var sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = fileName,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(24), // Set the expiration time for the SAS URL (24 hours in this example)
                    Protocol = SasProtocol.Https
                };
                // Set the permissions for the SAS URL (in this example, read permissions)
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                // Generate the SAS token for the blob
                var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_configuration["AzureStorage:AccountName"], _configuration["AzureStorage:AccountKey"])).ToString();

                // Combine the Blob URL with the SAS token to create the final SAS URL
                imageEntity.Url = blobClient.Uri + "?" + sasToken;
            }

            postEntity.Images ??= new List<Image>();
            postEntity.Images.Add(imageEntity);
            await _postRepository.UpdateAsync(postEntity);

            var postResponseModel = _mapper.Map<PostResponseModel>(postEntity);
            return new ResponseObject<PostResponseModel>
            {
                Message = "Image added to post successfully",
                Data = postResponseModel
            };
        }

    }
}
