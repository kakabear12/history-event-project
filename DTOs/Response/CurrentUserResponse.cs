using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class CurrentUserResponse
    {
        public string Message { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int TotalScore { get; set; }
        public int TotalQuestion { get; set; }
        public string Role { get; set; }
    }
}
