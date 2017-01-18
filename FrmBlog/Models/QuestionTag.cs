using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrmBlog.Models
{
    public class QuestionTag
    {
        public long TagId { get; set; }
        public long QuestionId { get; set; }
    }
}