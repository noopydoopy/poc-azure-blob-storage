using Azure.Storage.Blobs.Models;
using AzureBlobStorageApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobStorageApp.Pages.Containers
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAzureStorageService _azureStorageService;

        [BindProperty]
        public List<BlobItem> Items { get; set; }

        [BindProperty]
        public IFormFile FromFile { get; set; }

        public IndexModel(IAzureStorageService azureStorageService)
        {
            _azureStorageService = azureStorageService;
        }

        public async Task<IActionResult> OnGetAsync(string containerName)
        {
            var connString = User.Identity.Name;
            var blobItems = await _azureStorageService.GetBlobByContainerAsync(connString, containerName);
            Items = blobItems;
            return Page();
        }

        public async Task<IActionResult> OnGetBlob(string containerName, string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            var connString = User.Identity.Name;
            var file = await _azureStorageService.DownloadBlobAsync(connString, containerName, fileName);

            var fileResult = new FileStreamResult(file.Content.ToStream(), file.Details.ContentType)
            {
                FileDownloadName = fileName
            };

            return fileResult;
        }

        public async Task<IActionResult> OnPostAsync(string containerName)
        {
            var connString = User.Identity.Name;
            if (FromFile != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await FromFile.CopyToAsync(memoryStream);
                    await _azureStorageService.UploadBlobAsync(connString, containerName, FromFile.FileName, FromFile.ContentType, memoryStream);
                }
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetDeleteFile(string containerName, string fileName)
        {
            var connString = User.Identity.Name;
            await _azureStorageService.DeleteBlobAsync(connString, containerName, fileName);
            return RedirectToPage("./Index");
        }
    }
}
