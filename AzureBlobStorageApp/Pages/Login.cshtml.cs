using AzureBlobStorageApp.Services;
using AzureBlobStorageApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AzureBlobStorageApp.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IAzureStorageService _azureStorageService;

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }

        public LoginModel(IAzureStorageService azureStorageService, ILogger<LoginModel> logger)
        {
            _azureStorageService = azureStorageService;
            _logger = logger;
        }

        public IActionResult OnGet(string ReturnUrl = "/Index")
        {
            if (User.Identity.IsAuthenticated)
            {
                if(!string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToPage(ReturnUrl);
                return RedirectToPage("/Index");
            }
            ViewData["Error"] = "";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                bool isConnectionValid = await _azureStorageService.VerifyConnectionStringAsync(LoginViewModel.ConnectionString);
                if (isConnectionValid)
                {
                    ViewData["Error"] = "";
                    var claims = new List<Claim>() {
                        new Claim(ClaimTypes.Name, LoginViewModel.ConnectionString)
                    };
                    var identity = new ClaimsIdentity(claims, "AADCookies");
                    var claimPrincipal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("AADCookies", claimPrincipal);
                    return RedirectToPage("Index");
                }
                else
                {
                    ViewData["Error"] = "Your connection string is not valid.";
                }
            }
            return Page();
        }
    }
}
