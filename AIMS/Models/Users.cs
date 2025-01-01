using System.ComponentModel.DataAnnotations;

namespace AIMS.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Branch { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}

