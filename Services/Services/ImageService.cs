using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Services.Interface;

namespace Services.Services
{
    public class ImageService : IImageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public ImageService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureBlobStorage");
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = configuration.GetValue<string>("BlobContainerName");
        }
        private async Task<BlobContainerClient> GetContainerClientAsync()
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();
            return containerClient;
        }

        public Task<bool> DeleteImageAsync(string publicId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            try
            {
                BlobContainerClient containerClient = await GetContainerClientAsync();

                string blobPath = $"{folderName}/{file.FileName}";
                BlobClient blobClient = containerClient.GetBlobClient(blobPath);

                using (Stream stream = file.OpenReadStream())
                {
                    string contentType = GetContentType(file.FileName);
                    var headers = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    };

                    Response<BlobContentInfo> response = await blobClient.UploadAsync(stream, headers);

                    if (response.GetRawResponse().IsError)
                    {
                        // Xử lý lỗi khi tải lên
                        return null;
                    }

                    return blobClient.Uri.AbsoluteUri;
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                throw ex;
            }
        }

        private string GetContentType(string fileName)
        {
            string contentType;
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }

            return contentType;
        }

    }
}
