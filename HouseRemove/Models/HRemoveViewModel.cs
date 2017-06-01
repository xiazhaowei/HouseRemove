using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRemove.Models
{
    public class HRemoveViewModel
    {
        public int Id { get; set; }

        [Display(Name ="请输入名称")]
        [Required]
        public string Name { get; set; }
    }

    public class ManagerViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string State { get; set; }
    }
}