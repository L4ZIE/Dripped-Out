using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
public class BlobStorageController : ControllerBase
{
    [HttpPost("/uploadfile")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        string blobUri = string.Empty;
        BlobServiceClient blobServiceClient =
            new BlobServiceClient(Environment.GetEnvironmentVariable("blobconnectionstring"));
        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient("products");

        using (var stream = file.OpenReadStream())
        {
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            await blobClient.UploadAsync(stream, true);
            blobUri = blobClient.Uri.ToString();
        }
        return Ok(blobUri);
    }

    [HttpGet("/getimage")]
    public async Task<IActionResult> GetImage(string blobUri)
    {
        using (var client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(blobUri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();

                return File(content, "image/jpeg");
            }
            else
            {
                return null;
            }
        }
        
    }
}