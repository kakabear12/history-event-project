using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class ValidateRefreshTokenResponse : BaseResponse
    {
        public int UserId { get; set; }

    }
}
