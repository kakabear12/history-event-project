﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class ResponseObject
    {
        public string Message { get; set; }
        public Object Data { get; set; }
    }

    public class ResponseObject<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
