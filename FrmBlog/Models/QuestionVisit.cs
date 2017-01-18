using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrmBlog.Models
{
    public class QuestionVisit
    {
        public long ID { get; set; }
        public long QuestionId { get; set; }
        public long? UserId { get; set; }
        public DateTime Date { get; set; }
        public string IPAddress { get; set; }
        public string Referans { get; set; }
    }
}