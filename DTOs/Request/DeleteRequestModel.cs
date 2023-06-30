using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class DeletePostRequestModel
    {
        [Required(ErrorMessage = "PostId is a required field")]
        public int PostId { get; set; }
    }
}
