
using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models
{
    public class Lead
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lead name is required.")]
        [StringLength(100)]
        [Display(Name = "Lead Name")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(150)]
        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        [Required(ErrorMessage = "Please select a lead source.")]
        [StringLength(50)]
        public string Source { get; set; } = "Website";

        [Required(ErrorMessage = "Please select a lead status.")]
        [StringLength(30)]
        public string Status { get; set; } = "New";

        [Required(ErrorMessage = "Please select a priority.")]
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        [Range(0, 1000000000,
            ErrorMessage = "Expected value must be between 0 and 1,000,000,000.")]
        [Display(Name = "Expected Value")]
        public decimal ExpectedValue { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        [StringLength(450)]
        [Display(Name = "Assigned Sales Executive")]
        public string? AssignedTo { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}