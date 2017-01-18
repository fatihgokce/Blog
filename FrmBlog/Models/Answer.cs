using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace FrmBlog.Models
{ 
    public class Answer
    {
        public long AnswerId { get; set; }
        public long QuestionId { get; set; }
        public string Reply { get; set; }
        public DateTime Date { get; set; }
        public string IPAddress { get; set; }
        public long? UserId { get; set; }
        public bool BestReply { get; set; }
        public State State { get; set; }
        public User User { get; set; }
    }
}