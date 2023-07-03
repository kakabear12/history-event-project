using AutoMapper;
using Azure.Storage.Blobs;
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
        Task<ResponseObject<ImageResponseModel>> CreateAsync(ImageRequestModel imageModel, IFormFile imageFile);

        Task<ResponseObject<ImageResponseModel>> UpdateImage(int id, ImageRequestModel imageModel, IFormFile imageFile);
        Task<ResponseObject<ImageResponseModel>> GetByIdAsync(int id);
        Task<ResponseObject<IEnumerable<ImageResponseModel>>> GetAllAsync();
       
      
        Task<ResponseObject<bool>> DeleteAsync(int id);
        Task<ResponseObject<TagResponseModel>> AddImageToTag(int tagId, ImageRequestModel imageModel, IFormFile imageFile);
        Task<ResponseObject<PostMetaResponseModel>> AddImageToPostMeta(int postId, int metaId, ImageRequestModel imageModel, IFormFile imageFile);
        Task<ResponseObject<EventResponseModel>> AddImageToEvent(int eventId, ImageRequestModel imageModel, IFormFile imageFile);


    }
    public class ImageService : IImageService
    {
        private readonly ImageRepository _imageRepository;
        private readonly TagRepository _tagRepository;
        private readonly PostMetaRepository _postMetaRepository;
        private readonly EventsRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ImageService(
        ImageRepository imageRepository,
        TagRepository tagRepository,
        PostMetaRepository postMetaRepository,
        EventsRepository eventRepository,
        IConfiguration configuration,
        IMapper mapper)
        {
            _imageRepository = imageRepository;
            _tagRepository = tagRepository;
            _postMetaRepository = postMetaRepository;
            _eventRepository = eventRepository;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ResponseObject<ImageResponseModel>> CreateAsync(ImageRequestModel imageModel, IFormFile imageFile)
        {

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

                // Update the Url field in the imageEntity with the URL of the uploaded image
                imageEntity.Url = blobClient.Uri.ToString();
            }

            await _imageRepository.AddAsync(imageEntity);

            var createdImageModel = _mapper.Map<ImageResponseModel>(imageEntity);
            return new ResponseObject<ImageResponseModel>
            {
                Message = "Image created successfully",
                Data = createdImageModel
            };
        }


        public async Task<ResponseObject<ImageResponseModel>> GetByIdAsync(int id)
        {
            var image = await _imageRepository.GetById(id);
            if (image == null)
            {
                return new ResponseObject<ImageResponseModel>
                {
                    Message = "Image not found",
                    Data = null
                };
            }

            var imageModel = _mapper.Map<ImageResponseModel>(image);
            return new ResponseObject<ImageResponseModel>
            {
                Message = "Image retrieved successfully",
                Data = imageModel
            };
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

        public async Task<ResponseObject<bool>> DeleteAsync(int id)
        {
            var image = await _imageRepository.GetById(id);
            if (image == null)
            {
                return new ResponseObject<bool>
                {
                    Message = "Image not found",
                    Data = false
                };
            }

            await _imageRepository.DeleteAsync(image);

            return new ResponseObject<bool>
            {
                Message = "Image deleted successfully",
                Data = true
            };
        }

        public async Task<ResponseObject<ImageResponseModel>> UpdateImage(int id, ImageRequestModel imageModel, IFormFile imageFile)
        {
            var existingImage = await _imageRepository.GetById(id);
            if (existingImage == null)
            {
                return new ResponseObject<ImageResponseModel>
                {
                    Message = "Image not found",
                    Data = null
                };
            }

            // Update the existing image entity with the new data
            var imageProperties = imageModel.GetType().GetProperties();
            foreach (var property in imageProperties)
            {
                var newValue = property.GetValue(imageModel);
                if (newValue != null)
                {
                    var existingImageProperty = existingImage.GetType().GetProperty(property.Name);
                    if (existingImageProperty != null && existingImageProperty.CanWrite)
                    {
                        existingImageProperty.SetValue(existingImage, newValue);
                    }
                }
            }

            // ...

            // Validate the updated image entity or perform any necessary checks
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

                // Update the Url field in the existing imageEntity with the URL of the uploaded image
                existingImage.Url = blobClient.Uri.ToString();
            }

            await _imageRepository.UpdateAsync(existingImage);

            var updatedImageModel = _mapper.Map<ImageResponseModel>(existingImage);
            return new ResponseObject<ImageResponseModel>
            {
                Message = "Image updated successfully",
                Data = updatedImageModel
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

                // Update the Url field in the imageEntity with the URL of the uploaded image
                imageEntity.Url = blobClient.Uri.ToString();
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


        public async Task<ResponseObject<PostMetaResponseModel>> AddImageToPostMeta(int postId, int metaId, ImageRequestModel imageModel, IFormFile imageFile)
        {
            var postMetaEntity = await _postMetaRepository.GetPostMetaById(postId, metaId);
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

                // Update the Url field in the imageEntity with the URL of the uploaded image
                imageEntity.Url = blobClient.Uri.ToString();
            }

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

                // Update the Url field in the imageEntity with the URL of the uploaded image
                imageEntity.Url = blobClient.Uri.ToString();
            }

            tagEntity.Images.Add(imageEntity);
            await _tagRepository.UpdateAsync(tagEntity);

            var tagResponseModel = _mapper.Map<TagResponseModel>(tagEntity);
            return new ResponseObject<TagResponseModel>
            {
                Message = "Image added to tag successfully",
                Data = tagResponseModel
            };
        }

    }
}
