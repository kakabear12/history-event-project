using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class UpdateRoleRequest
    {
        [Required(ErrorMessage = "Id is required field.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Role is required field.")]
        [EnumDataType(typeof(Role), ErrorMessage = "Invalid role. Role are: Admin, Member, Editor")]
        public string Role { get; set; }
    }
}
