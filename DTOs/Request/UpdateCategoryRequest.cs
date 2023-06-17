using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class UpdateCategoryRequest
    {
        [Required(ErrorMessage = "Category id is a required field.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category name is a required field.")]
        public string CategoryName { get; set; }
    }
}
