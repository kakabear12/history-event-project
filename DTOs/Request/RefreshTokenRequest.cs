﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string RefreshToken { get; set; }

    }
}

