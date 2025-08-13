using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DecorVista.Models
{
    public class Designer
    {
        [Key]
        public int Id { get; set; }

        // Personal Information
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Profile Image")]
        public string? ProfileImagePath { get; set; }

        // Professional Information
        [Required]
        [Display(Name = "Specialization")]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Years of Experience")]
        public int YearsOfExperience { get; set; }

        [Display(Name = "Skills / Expertise (comma separated)")]
        public string? Skills { get; set; }

        [Display(Name = "Portfolio Link")]
        public string? PortfolioLink { get; set; }

        [Display(Name = "Work Categories (comma separated)")]
        public string? WorkCategories { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Consultation Fee")]
        public decimal? ConsultationFee { get; set; }

        [Display(Name = "Available Days & Time Slots")]
        public string? Availability { get; set; }

        // Location & Contact Details
        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        public string? PostalCode { get; set; }

        // About Section
        [Required]
        [Display(Name = "Short Bio")]
        public string Bio { get; set; } = string.Empty;

        [Display(Name = "Design Philosophy")]
        public string? DesignPhilosophy { get; set; }

        public string? LanguagesSpoken { get; set; }

        // Social Links
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? LinkedInLink { get; set; }
        public string? PinterestLink { get; set; }

        public string? Certifications { get; set; }
        public string? PreferredProjectTypes { get; set; }
        public int? MaxProjectsAtOnce { get; set; }

        // Registration Info
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // Approval Status
        public string? Status { get; set; } = "Pending";
    }
}
