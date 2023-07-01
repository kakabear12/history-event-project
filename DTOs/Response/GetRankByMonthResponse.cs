using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class GetRankByResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Score { get; set; }
    }
}
