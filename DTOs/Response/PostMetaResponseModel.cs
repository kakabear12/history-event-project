using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class PostMetaResponseModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Keys { get; set; }
        public string Contents { get; set; }
    }
}
