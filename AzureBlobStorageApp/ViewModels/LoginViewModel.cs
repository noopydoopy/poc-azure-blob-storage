using System.ComponentModel.DataAnnotations;

namespace AzureBlobStorageApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Azure Connection string is required.")]
        public string ConnectionString { get; set; }
        public string Error { get; set; }
    }
}
