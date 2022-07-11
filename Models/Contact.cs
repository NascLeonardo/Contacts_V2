
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts_V2.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Nickname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birthday { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public bool isFavorite { get; set; } = false;

        public User User { get; set; }
        public int UserId { get; set; }

    }

}