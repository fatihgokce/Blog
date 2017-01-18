using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FrmBlog.Models
{
    public class Tag
    {
        public long TagId { get; set; }
        [Required(ErrorMessage = "ismini giriniz.")]
        public string TagName { get; set; }
        public DateTime RecordDate { get; set; }
        public int CategoryId { get; set; }
    }
}