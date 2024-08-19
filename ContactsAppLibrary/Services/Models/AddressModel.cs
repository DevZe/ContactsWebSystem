using System.ComponentModel.DataAnnotations;

namespace ContactsAppLibrary.Services.Models
{
    public class AddressModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StreetNumber { get; set; } = string.Empty;
        [Required]
        public string StreetName { get; set; } = string.Empty;
        [Required]
        public string Suburb { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Province { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public string ZipCode { get; set; } = string.Empty;
    }
}
