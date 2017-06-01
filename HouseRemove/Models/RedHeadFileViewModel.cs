using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRemove.Models
{
    public class RedHeadFileViewModel
    {
        public int Id { get; set; }

        [Display(Name ="请选择{0}")]
        [Required]
        public string Type { get; set; }

        [Display(Name ="文件标题")]
        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime PubTime { get; set; }
        [Required]
        public DateTime ZhiXingTime { get; set; }
        [Required]
        public string Org { get; set; }

        public string FilePath { get; set; }

        
    }
}