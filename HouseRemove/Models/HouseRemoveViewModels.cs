using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRemove.Models
{
    public class HouseholdViewModel
    {
        public int Id { get; set; }

        [Required]
        public string HRemoveName { get; set; }

        [Display(Name ="房屋类型")]
        [Required]
        public string HouseType { get; set; }

        [Display(Name = "房屋性质")]
        [Required]
        public string HouseProperty { get; set; }

        [Display(Name = "补偿类型")]
        [Required]
        public string CompensationType { get; set; }

        [Display(Name = "项目时间")]
        [Required]
        public DateTime? RemoveTime { get; set; }

        [Display(Name = "姓名")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "身份证号")]
        [Required]
        public string CardNumber { get; set; }

        [Display(Name = "手机号")]
        [Required]
        public string Mobile { get; set; }

        [Display(Name = "公有权人")]
        public string PubliclyOwned { get; set; }

        [Display(Name = "继承人")]
        public string Successor { get; set; }

        [Display(Name = "建造时间")]
        [Required]
        public string BuildTime { get; set; }

        [Display(Name = "使用类型")]
        [Required]
        public string HouseUseFor { get; set; }

        [Display(Name = "使用状况")]
        [Required]
        public string UseStatus { get; set; }

        [Display(Name = "是否兼用房")]
        [Required]
        public string IsPartUse { get; set; }

        [Display(Name = "地号")]
        [Required]
        public string LandParcel { get; set; }

        [Display(Name = "建造面积")]
        [Required]
        public float Area { get; set; }

        [Display(Name = "实用面积")]
        [Required]
        public float UsefullArea { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "产权编号")]
        public string PropertyRightNumber { get; set; }

        [Display(Name = "产权备注")]
        public string PropertyRightRemark { get; set; }

        [Display(Name = "承租单")]
        public string LesseeNumber { get; set; }

        [Display(Name = "评估价格")]
        [Required]
        public decimal AssessPrice { get; set; }

        [Display(Name = "装修补偿")]
        [Required]
        public decimal ZhuangxiuPrice { get; set; }

        [Display(Name = "经营性补偿")]
        [Required]
        public decimal BusinessCompensation { get; set; }

        [Display(Name = "应发补偿")]
        [Required]
        public decimal YingFa { get; set; }

        [Display(Name = "实发补偿")]
        [Required]
        public decimal ShiFa { get; set; }
        
        [Display(Name ="验房单号")]
        public string InspectNumber { get; set; }

        [Display(Name ="验房时间")]
        public DateTime? InspectTime { get; set; }


        [Display(Name = "签署状态")]
        [Required]
        public string SignStatus { get; set; }
        
    }

    public class SignInfoViewModel
    {
        public int Id { get; set; }

        public int HouseHoldId { get; set; }

        [Display(Name ="签署协议编号")]
        [Required]
        public string SignNumber { get; set; }

        [Display(Name = "资金表号")]
        [Required]
        public string CapitalNumber { get; set; }

        public DateTime? GiveHouseTime { get; set; }

        public DateTime? GetMoneyTime { get; set; }

        public string GetMoneyUser { get; set; }

        [Display(Name = "归档档案柜")]
        [Required]
        public string FileCabinetNumber { get; set; }

        [Display(Name = "批次编号")]
        [Required]
        public string BatchNumber { get; set; }

        public string Remark { get; set; }
    }

    public class UnSignInfoViewModel
    {
        public int Id { get; set; }

        public int HouseHoldId { get; set; }

        [Display(Name = "未签署类型")]
        [Required]
        public string UnSignType { get; set; }

        public string UnSignRemark { get; set; }

        public DateTime? YueTanTime { get; set; }
        public DateTime? YueTan2Time { get; set; }
        public DateTime? YueTan3Time { get; set; }

        public DateTime? PingGuBaoGaoTime { get; set; }
        public DateTime? PingGuBaoGaoSongDaTime { get; set; }
        public DateTime? BaoSongFaGuiKeTime { get; set; }
        public DateTime? TuiJuanTime { get; set; }

        public DateTime? ZaiCiSongBaoTime { get; set; }
        public DateTime? DanHuBuChangXiaDaTime { get; set; }

        public DateTime? DanHuBuChangSongDaTime { get; set; }
        public DateTime? QiSuQiDaoQiTime { get; set; }
        public DateTime? CuiGaoShuSongDaTime { get; set; }
        public DateTime? QiangZhiZhiXingTiBaoFaGuiKeTime { get; set; }
        public DateTime? FaYuanLiAnTime { get; set; }
        public DateTime? XiaDaZhuiYuZhiXingCaiDingTime { get; set; }
        public DateTime? CheHuiTime { get; set; }

        public string CheHuiYuanYin { get; set; }
    }
}