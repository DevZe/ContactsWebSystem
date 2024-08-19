using System.ComponentModel.DataAnnotations;

namespace ContactsAppLibrary.Services.Models.Auth
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
