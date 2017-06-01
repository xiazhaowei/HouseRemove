using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRemove.Lib
{
    public enum 管理员状态
    {
        正常,
        锁定
    }

    public enum 管理员角色
    {
        管理员,
        普通用户
    }

    public enum 房屋类型
    {
        公房,
        私房
    }

    public enum 补偿类型
    {
        货币,
        房屋,
        非住宅
    }

    public enum 是否兼用房
    {
        否,
        是
    }

    public enum 使用状况
    {
        自住,
        出租
        
    }

    public enum 房屋用途
    {
        住宅,
        非住宅
    }

    public enum 房屋性质
    {
        私房,
        公房,
        自管,
        代管,
        其他
        
    }

    public enum 签署类型
    {
        待签署,
        已签署
    }

    public enum 未拆迁原因
    {
        析产户,
        产权人或继承人已故,
        对评估价不满,
        对未认定结果有异议,
        要求现房或多套安置房屋,
        代管房,
        非住宅,
        其他
    }


    public enum 文件类型
    {
        声明,
        公告,
        公示
    }
}