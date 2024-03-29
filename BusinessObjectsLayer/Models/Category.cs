﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }

        public int ParentId { get; set; }
        public string MetaTitle { get; set; }
        public string Slug { get; set; }
        public string Contents { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
