using System.ComponentModel.DataAnnotations;

namespace StudentNameMVC.Models
{
    public class AccountViewModel
    {
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Account name is required")]
        [StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string AccountEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public short AccountRole { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [DataType(DataType.Password)]
        public string AccountPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("AccountPassword", ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string RoleName => AccountRole switch
        {
            1 => "Staff",
            2 => "Lecturer",
            _ => "Unknown"
        };
    }
}
