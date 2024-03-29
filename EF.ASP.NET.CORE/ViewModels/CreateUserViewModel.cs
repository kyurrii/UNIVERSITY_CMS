﻿namespace EF.ASP.NET.CORE.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
