using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class UserReponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int TotalScore { get; set; }
        public int TotalQuestion { get; set; }
        public string Role { get; set; }
    }
}
