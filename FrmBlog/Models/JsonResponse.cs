using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrmBlog.Models
{
    public class JsonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }

    }
}