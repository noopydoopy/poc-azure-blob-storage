using Azure.Storage.Blobs.Models;
using AzureBlobStorageApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageApp.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAzureStorageService _azureStorageService;

        [BindProperty]
        public List<BlobContainerItem> Containers { get; set; }
        [BindProperty]
        public string ContainerName { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IAzureStorageService azureStorageService)
        {
            _logger = logger;
            _azureStorageService = azureStorageService;
        }

        public async Task<IActionResult> OnGet()
        {
            var connString = User.Identity.Name;
            var containers = await _azureStorageService.GetBlobContainerAsync(connString);
            Containers = containers;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var connString = User.Identity.Name;
            if (!string.IsNullOrEmpty(ContainerName))
            {
                await _azureStorageService.CreateContainerAsync(connString, ContainerName);
            }

            return RedirectToPage("/Index");
        }
    }
}
