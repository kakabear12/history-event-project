﻿using BusinessObjectsLayer.Models;
using DTOs.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class UpdateUserModel
    {
        [Required(ErrorMessage = "UserId is required field.")]
        public int UserId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Name is required field.")]
        [MaxLength(60, ErrorMessage = "Name maximum is 60 characters.")]
        public string Name { get; set; }
        [Required]
        [CustomDateRange(ErrorMessage = "Birthday must be before 1/1/2018.")]
        public DateTime Birthday { get; set; }
    }
}
