﻿using System.ComponentModel.DataAnnotations;

namespace Portfolio.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
