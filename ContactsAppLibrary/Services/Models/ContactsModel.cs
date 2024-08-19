using System.ComponentModel.DataAnnotations;

namespace ContactsAppLibrary.Services.Models
{
    public class ContactsModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public AddressModel? Address { get; set; } = new AddressModel();
    }
}
