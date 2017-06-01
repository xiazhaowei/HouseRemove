using HouseRemove.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HouseRemove.Lib;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;

namespace HouseRemove.Controllers
{
    public class HouseholdController : BaseController
    {
        // GET: Household
        public ActionResult Index(string hremove,string landparcel,string name,string cardnumber,string signstatus,string compensationtype)
        {
            if (TempData.ContainsKey("ModelState"))
            {
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            }

            var datas = db.Householdes.Where(c => c.Id > 0);
            if(!string.IsNullOrEmpty(hremove) && !hremove.Equals("选择项目"))
            {
                datas = datas.Where(c => c.HRemove.Name.Equals(hremove));
            }
            if (!string.IsNullOrEmpty(landparcel))
            {
                datas = datas.Where(c => c.LandParcel.Equals(landparcel,StringComparison.InvariantCultureIgnoreCase));
            }
            if (!string.IsNullOrEmpty(name))
            {
                datas = datas.Where(c=>c.Name.Contains(name));
            }
            if(!string.IsNullOrEmpty(cardnumber))
            {
                datas = datas.Where(c=>c.CardNumber.Contains(cardnumber));
            }
            if(!string.IsNullOrEmpty(signstatus))
            {
                datas = datas.Where(c=>c.SignStatus.Equals(signstatus,StringComparison.CurrentCultureIgnoreCase));
            }
            if (!string.IsNullOrEmpty(compensationtype))
            {
                datas = datas.Where(c => c.CompensationType.Equals(compensationtype, StringComparison.CurrentCultureIgnoreCase));
            }


            ViewBag.hremove = hremove;
            ViewBag.landparcel = landparcel;
            ViewBag.name = name;
            ViewBag.cardnumber = cardnumber;
            ViewBag.signstatus = signstatus;
            ViewBag.compensationtype = compensationtype;

            //选择项目
            var item = new Models.HRemove {Id=0, Name="选择项目"};
            var hremoves = db.HRemoves.Where(c => c.Id>0).ToList();
            hremoves.Insert(0, item);
            var selectList = new SelectList(hremoves, "Name", "Name");
            
            ViewData["list"] = selectList;

            SetMyAccountViewModel();
            return View(datas);
        }

        #region 增删改查
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Add(int id)
        {

            var model = new HouseholdViewModel();
            model.IsPartUse = 是否兼用房.否.ToString();
            model.SignStatus = 签署类型.待签署.ToString();
            if (id>0)
            {
                Household household = db.Householdes.Find(id);
                {
                    if(household!=null)
                    {
                        model.Area = household.Area;
                        model.UsefullArea = household.UsefullArea;
                        model.HouseType = household.HouseType;
                        model.HouseProperty = household.HouseProperty;
                        model.CompensationType = household.CompensationType;
                        model.RemoveTime = household.RemoveTime;
                        model.Name = household.Name;
                        model.Mobile = household.Mobile;
                        model.CardNumber = household.CardNumber;
                        model.PubliclyOwned = household.PubliclyOwned;
                        model.Successor = household.Successor;
                        model.BuildTime = household.BuildTime;
                        model.Address = household.Address;
                        model.HouseUseFor = household.HouseUseFor;
                        model.UseStatus = household.UseStatus;
                        model.IsPartUse = household.IsPartUse;
                        model.LandParcel = household.LandParcel;
                        model.PropertyRightNumber = household.PropertyRightNumber;
                        model.PropertyRightRemark = household.PropertyRightRemark;
                        model.LesseeNumber = household.LesseeNumber;
                        model.AssessPrice = household.AssessPrice;
                        model.ZhuangxiuPrice = household.ZhuangxiuPrice;
                        model.BusinessCompensation = household.BusinessCompensation;
                        model.YingFa = household.YingFa;
                        model.ShiFa = household.ShiFa;
                        model.SignStatus = household.SignStatus;                        
                        model.InspectNumber = household.InspectNumber;
                        if (household.InspectTime.HasValue)
                            model.InspectTime = household.InspectTime.Value;
                        model.HRemoveName = household.HRemove.Name;
                    }
                }
            }

            //选择项目
            var hremoves = db.HRemoves.Where(h => h.Id > 0).ToList();
            var selectList = new SelectList(hremoves, "Name", "Name");
            ViewData["list"] = selectList;


            SetMyAccountViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(HouseholdViewModel model)
        {
            if(ModelState.IsValid)
            {
                int id = 0;
                HRemove hremove = db.HRemoves.SingleOrDefault(m => m.Name.Equals(model.HRemoveName,StringComparison.InvariantCultureIgnoreCase));
                if (hremove == null)
                {
                    return HttpNotFound();
                }

                if(model.Id>0)//修改
                {
                    Household household = db.Householdes.SingleOrDefault(h => h.Id == model.Id);
                    if(household!=null)
                    {
                        //判断状态
                        if(!household.SignStatus.Equals(model.SignStatus))
                        {
                            if(household.SignStatus.Equals(签署类型.已签署.ToString()))
                            {//删除已签署数据
                                household.SignInfo = null;
                                db.SignInfos.Remove(household.SignInfo);
                            }
                            else
                            {
                                household.UnSignInfo = null;
                                db.UnSignInfos.Remove(household.UnSignInfo);
                            }
                        }

                        household.InspectNumber = model.InspectNumber;
                        household.InspectTime = model.InspectTime;

                        household.HRemove = hremove;
                        household.HouseType = model.HouseType;
                        household.CompensationType = model.CompensationType;
                        household.RemoveTime = model.RemoveTime.Value;

                        household.Name = model.Name;
                        household.Mobile = model.Mobile;
                        household.CardNumber = model.CardNumber;
                        household.PubliclyOwned = model.PubliclyOwned;
                        household.Successor = model.Successor;

                        household.BuildTime = model.BuildTime;
                        household.Address = model.Address;
                        household.UseStatus = model.UseStatus;
                        household.HouseProperty = model.HouseProperty;
                        household.HouseUseFor = model.HouseUseFor;
                        household.IsPartUse = model.IsPartUse;
                        household.LandParcel = model.LandParcel;
                        household.Area = model.Area;
                        household.UsefullArea = model.UsefullArea;
                        household.PropertyRightNumber = model.PropertyRightNumber;
                        household.PropertyRightRemark = model.PropertyRightRemark;

                        household.AssessPrice = model.AssessPrice;
                        household.ZhuangxiuPrice = model.ZhuangxiuPrice;
                        household.BusinessCompensation = model.BusinessCompensation;
                        household.YingFa = model.YingFa;
                        household.ShiFa = model.ShiFa;
                        household.SignStatus = model.SignStatus;

                        household.CreateTime = DateTime.Now;

                        db.Entry(household).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        id = household.Id;

                        

                    }
                }
                else
                {
                    Household household = new Household();
                    household.InspectNumber = model.InspectNumber;
                    household.InspectTime = model.InspectTime;

                    household.HRemove = hremove;
                    household.HouseType = model.HouseType;
                    household.CompensationType = model.CompensationType;
                    household.RemoveTime = model.RemoveTime.Value;

                    household.Name = model.Name;
                    household.Mobile = model.Mobile;
                    household.CardNumber = model.CardNumber;
                    household.PubliclyOwned = model.PubliclyOwned;
                    household.Successor = model.Successor;

                    household.BuildTime = model.BuildTime;
                    household.Address = model.Address;
                    household.UseStatus = model.UseStatus;
                    household.HouseProperty = model.HouseProperty;
                    household.HouseUseFor = model.HouseUseFor;
                    household.IsPartUse = model.IsPartUse;
                    household.LandParcel = model.LandParcel;
                    household.Area = model.Area;
                    household.UsefullArea = model.UsefullArea;
                    household.PropertyRightNumber = model.PropertyRightNumber;
                    household.PropertyRightRemark = model.PropertyRightRemark;

                    household.AssessPrice = model.AssessPrice;
                    household.ZhuangxiuPrice = model.ZhuangxiuPrice;
                    household.BusinessCompensation = model.BusinessCompensation;
                    household.YingFa = model.YingFa;
                    household.ShiFa = model.ShiFa;
                    household.SignStatus = model.SignStatus;

                    household.CreateTime = DateTime.Now;

                    db.Householdes.Add(household);
                    db.SaveChanges();

                    id = household.Id;
                }
                

                if(model.SignStatus.Equals(签署类型.已签署.ToString()))
                {
                    return RedirectToAction("EditSignInfo", new { householdid = id });
                }
                else
                {
                    return RedirectToAction("EditUnSignInfo", new { householdid = id });
                }
            }

            return View(model);
        }

        public ActionResult EditSignInfo(int householdid)
        {
            if(householdid==0)
            {
                return RedirectToAction("Index");
            }
            Household household = db.Householdes.Find(householdid);
            if(household==null)
            {
                return RedirectToAction("Index");
            }

            var model=new SignInfoViewModel();
            if(household.SignInfo!=null)
            {
                model.Id = household.SignInfo.Id;
                model.SignNumber = household.SignInfo.SignNumber;
                model.HouseHoldId = household.Id;
                model.GetMoneyUser = household.SignInfo.GetMoneyUser;
                model.CapitalNumber = household.SignInfo.CapitalNumber;
                model.FileCabinetNumber = household.SignInfo.FileCabinetNumber;
                model.BatchNumber = household.SignInfo.BatchNumber;
                if(household.SignInfo.GiveHouseTime.HasValue)
                    model.GiveHouseTime = household.SignInfo.GiveHouseTime.Value;
                if(household.SignInfo.GetMoneyTime.HasValue)
                    model.GetMoneyTime =  household.SignInfo.GetMoneyTime.Value;
                model.Remark = string.IsNullOrEmpty(household.SignInfo.Remark) ? "" : household.SignInfo.Remark;
            }            

            SetMyAccountViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSignInfo(SignInfoViewModel model)
        {
            if(ModelState.IsValid)
            {
                Household household = db.Householdes.Find(model.HouseHoldId);
                if (household == null)
                {
                    return RedirectToAction("Index");
                }

                if(household.SignInfo!=null)
                {
                    household.SignInfo.SignNumber = model.SignNumber;
                    household.SignInfo.CapitalNumber = model.CapitalNumber;
                    household.SignInfo.GetMoneyTime = model.GetMoneyTime;
                    household.SignInfo.GiveHouseTime = model.GiveHouseTime;
                    household.SignInfo.GetMoneyUser = model.GetMoneyUser;
                    household.SignInfo.FileCabinetNumber = model.FileCabinetNumber;
                    household.SignInfo.BatchNumber = model.BatchNumber;
                    household.SignInfo.Remark = model.Remark;
                }
                else
                {
                    household.SignInfo = new SignInfo {
                        SignNumber = model.SignNumber,
                        CapitalNumber=model.CapitalNumber,
                        GetMoneyTime=model.GetMoneyTime,
                        GetMoneyUser=model.GetMoneyUser,
                        GiveHouseTime=model.GiveHouseTime,
                        FileCabinetNumber=model.FileCabinetNumber,
                        BatchNumber=model.BatchNumber,
                        Remark=model.Remark
                    };
                }
                db.SaveChanges();
                ModelState.AddModelError("", "操作成功");
                TempData["ModelState"] = ModelState;
                return RedirectToAction("Index");
            }
            SetMyAccountViewModel();
            return View(model);
        }


        public ActionResult EditUnSignInfo(int householdid)
        {
            if (householdid == 0)
            {
                return RedirectToAction("Index");
            }
            Household household = db.Householdes.Find(householdid);
            if (household == null)
            {
                return RedirectToAction("Index");
            }

            var model = new UnSignInfoViewModel();
            if (household.UnSignInfo != null)
            {
                model.Id = household.UnSignInfo.Id;
                model.UnSignType = household.UnSignInfo.UnSignType;
                model.HouseHoldId = household.Id;                
                model.UnSignRemark = string.IsNullOrEmpty(household.UnSignInfo.UnSignRemark) ? "" : household.UnSignInfo.UnSignRemark;
                if (household.UnSignInfo.YueTanTime.HasValue)
                    model.YueTanTime = household.UnSignInfo.YueTanTime.Value;
                if (household.UnSignInfo.YueTan2Time.HasValue)
                    model.YueTan2Time = household.UnSignInfo.YueTan2Time.Value;
                if (household.UnSignInfo.YueTan3Time.HasValue)
                    model.YueTan3Time = household.UnSignInfo.YueTan3Time.Value;

                if (household.UnSignInfo.PingGuBaoGaoTime.HasValue)
                    model.PingGuBaoGaoTime = household.UnSignInfo.PingGuBaoGaoTime.Value;

                if (household.UnSignInfo.PingGuBaoGaoSongDaTime.HasValue)
                    model.PingGuBaoGaoSongDaTime = household.UnSignInfo.PingGuBaoGaoSongDaTime.Value;

                if (household.UnSignInfo.BaoSongFaGuiKeTime.HasValue)
                    model.BaoSongFaGuiKeTime = household.UnSignInfo.BaoSongFaGuiKeTime.Value;

                if (household.UnSignInfo.TuiJuanTime.HasValue)
                    model.TuiJuanTime = household.UnSignInfo.TuiJuanTime.Value;

                if (household.UnSignInfo.ZaiCiSongBaoTime.HasValue)
                    model.ZaiCiSongBaoTime = household.UnSignInfo.ZaiCiSongBaoTime.Value;

                if (household.UnSignInfo.DanHuBuChangXiaDaTime.HasValue)
                    model.DanHuBuChangXiaDaTime = household.UnSignInfo.DanHuBuChangXiaDaTime.Value;

                if (household.UnSignInfo.DanHuBuChangSongDaTime.HasValue)
                    model.DanHuBuChangSongDaTime = household.UnSignInfo.DanHuBuChangSongDaTime.Value;

                if (household.UnSignInfo.QiSuQiDaoQiTime.HasValue)
                    model.QiSuQiDaoQiTime = household.UnSignInfo.QiSuQiDaoQiTime.Value;

                if (household.UnSignInfo.CuiGaoShuSongDaTime.HasValue)
                    model.CuiGaoShuSongDaTime = household.UnSignInfo.CuiGaoShuSongDaTime.Value;

                if (household.UnSignInfo.QiangZhiZhiXingTiBaoFaGuiKeTime.HasValue)
                    model.QiangZhiZhiXingTiBaoFaGuiKeTime = household.UnSignInfo.QiangZhiZhiXingTiBaoFaGuiKeTime.Value;

                if (household.UnSignInfo.FaYuanLiAnTime.HasValue)
                    model.FaYuanLiAnTime = household.UnSignInfo.FaYuanLiAnTime.Value;

                if (household.UnSignInfo.XiaDaZhuiYuZhiXingCaiDingTime.HasValue)
                    model.XiaDaZhuiYuZhiXingCaiDingTime = household.UnSignInfo.XiaDaZhuiYuZhiXingCaiDingTime.Value;

                if (household.UnSignInfo.CheHuiTime.HasValue)
                    model.CheHuiTime = household.UnSignInfo.CheHuiTime.Value;

                model.CheHuiYuanYin = string.IsNullOrEmpty(household.UnSignInfo.CheHuiYuanYin) ? "" : household.UnSignInfo.CheHuiYuanYin;
            }

            SetMyAccountViewModel();
            return View(model);
        }


        [HttpPost]
        public ActionResult EditUnSignInfo(UnSignInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Household household = db.Householdes.Find(model.HouseHoldId);
                if (household == null)
                {
                    return RedirectToAction("Index");
                }

                if (household.UnSignInfo != null)
                {
                    household.UnSignInfo.UnSignRemark = model.UnSignRemark;
                    household.UnSignInfo.UnSignType = model.UnSignType;

                    household.UnSignInfo.YueTanTime = model.YueTanTime;
                    household.UnSignInfo.YueTan2Time = model.YueTan2Time;
                    household.UnSignInfo.YueTan3Time = model.YueTan3Time;

                    household.UnSignInfo.PingGuBaoGaoTime = model.PingGuBaoGaoTime;
                    household.UnSignInfo.PingGuBaoGaoSongDaTime = model.PingGuBaoGaoSongDaTime;
                    household.UnSignInfo.BaoSongFaGuiKeTime = model.BaoSongFaGuiKeTime;
                    household.UnSignInfo.TuiJuanTime = model.TuiJuanTime;
                    household.UnSignInfo.ZaiCiSongBaoTime = model.ZaiCiSongBaoTime;
                    household.UnSignInfo.DanHuBuChangXiaDaTime = model.DanHuBuChangXiaDaTime;

                    household.UnSignInfo.DanHuBuChangSongDaTime = model.DanHuBuChangSongDaTime;
                    household.UnSignInfo.QiSuQiDaoQiTime = model.QiSuQiDaoQiTime;
                    household.UnSignInfo.CuiGaoShuSongDaTime = model.CuiGaoShuSongDaTime;
                    household.UnSignInfo.QiangZhiZhiXingTiBaoFaGuiKeTime = model.QiangZhiZhiXingTiBaoFaGuiKeTime;
                    household.UnSignInfo.FaYuanLiAnTime = model.FaYuanLiAnTime;
                    household.UnSignInfo.XiaDaZhuiYuZhiXingCaiDingTime = model.XiaDaZhuiYuZhiXingCaiDingTime;
                    household.UnSignInfo.CheHuiTime = model.CheHuiTime;

                    household.UnSignInfo.CheHuiYuanYin = model.CheHuiYuanYin;
                }
                else
                {
                    household.UnSignInfo = new UnSignInfo
                    {
                        UnSignType = model.UnSignType,
                        UnSignRemark = model.UnSignRemark,
                        YueTanTime=model.YueTanTime,
                        YueTan2Time=model.YueTan2Time,
                        YueTan3Time=model.YueTan3Time,
                        PingGuBaoGaoTime = model.PingGuBaoGaoTime,
                        PingGuBaoGaoSongDaTime = model.PingGuBaoGaoSongDaTime,
                        BaoSongFaGuiKeTime = model.BaoSongFaGuiKeTime,
                        TuiJuanTime = model.TuiJuanTime,
                        ZaiCiSongBaoTime = model.ZaiCiSongBaoTime,
                        DanHuBuChangXiaDaTime = model.DanHuBuChangXiaDaTime,
                        DanHuBuChangSongDaTime=model.DanHuBuChangSongDaTime,
                        QiSuQiDaoQiTime=model.QiSuQiDaoQiTime,
                        CuiGaoShuSongDaTime=model.CuiGaoShuSongDaTime,
                        QiangZhiZhiXingTiBaoFaGuiKeTime=model.QiangZhiZhiXingTiBaoFaGuiKeTime,
                        FaYuanLiAnTime=model.FaYuanLiAnTime,
                        XiaDaZhuiYuZhiXingCaiDingTime=model.XiaDaZhuiYuZhiXingCaiDingTime,
                        CheHuiTime=model.CheHuiTime,
                        CheHuiYuanYin=model.CheHuiYuanYin
                    };
                }
                db.SaveChanges();

                ModelState.AddModelError("", "操作成功");
                TempData["ModelState"] = ModelState;
                return RedirectToAction("Index");
            }
            SetMyAccountViewModel();
            return View(model);
        }
        
        public ActionResult Detail(int id)
        {            
            Household household = db.Householdes.Find(id);
            if(household==null)
            {
                return new HttpNotFoundResult();
            }
            SetMyAccountViewModel();
            return View(household);
        }

        public ActionResult Del(int id)
        {
            Household household = db.Householdes.Find(id);
            if(household==null)
            {
                return RedirectToAction("Index");
            }
            if(household.SignInfo!=null)
                db.SignInfos.Remove(household.SignInfo);
            if(household.UnSignInfo!=null)
                db.UnSignInfos.Remove(household.UnSignInfo);
            db.Householdes.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion


        #region   导出
        public ActionResult ExportXls()
        {
            //选择项目
            var hremoves = db.HRemoves.Where(c => c.Id > 0).ToList();
            var selectList = new SelectList(hremoves, "Name", "Name");

            ViewData["list"] = selectList;

            SetMyAccountViewModel();
            return View();
        }


        [HttpPost]
        public ActionResult ToXls(string hremove)
        {
            string tablename = "剩余户数综合统计表";
            #region 查数据
            string daiqianshu = 签署类型.待签署.ToString();
            string yiqianshu = 签署类型.已签署.ToString();
            
            string gongfang = 房屋性质.公房.ToString();
            string sifang = 房屋性质.私房.ToString();
            string daiguan = 房屋性质.代管.ToString();
            string ziguan = 房屋性质.自管.ToString();
            string qita = 房屋性质.其他.ToString();

            string xichanhu = 未拆迁原因.析产户.ToString();
            string chanquanrengu = 未拆迁原因.产权人或继承人已故.ToString();
            string pinggubuman = 未拆迁原因.对评估价不满.ToString();
            string jieguoyiyi = 未拆迁原因.对未认定结果有异议.ToString();
            string yaoxianfang = 未拆迁原因.要求现房或多套安置房屋.ToString();
            string daiguanfang = 未拆迁原因.代管房.ToString();
            string feizhuzhai = 未拆迁原因.非住宅.ToString();
            


            var list = db.HRemoves.SingleOrDefault(h => h.Name.Equals(hremove)).Householdes
                .OrderByDescending(hh=>hh.Id)
                .GroupBy(hh=>hh.LandParcel)
                .Select(g=>new {
                    LandParcel=g.Key,
                    ZongHuShu=g.Count(),
                    YiQianHuShu=g.Where(h=>h.SignStatus.Equals(yiqianshu)).Count(),
                    ShengYuHuShu = g.Where(h => h.SignStatus.Equals(daiqianshu)).Count(),

                    GongFang =g.Where(h=>h.HouseProperty.Equals(gongfang)).Count(),//公房
                    SiFang=g.Where(h=>h.HouseProperty.Equals(sifang)).Count(),
                    DaiGuan=g.Where(h=>h.HouseProperty.Equals(daiguan)).Count(),
                    ZiGuan = g.Where(h => h.HouseProperty.Equals(ziguan)).Count(),
                    QiTa = g.Where(h => h.HouseProperty.Equals(qita)).Count(),

                    XiChanHu = g.Where(h => null!=h.UnSignInfo && h.UnSignInfo.UnSignType.Equals(xichanhu)).Count(),//析产户
                    ChanQuanRenGu = g.Where(h => null != h.UnSignInfo && h.UnSignInfo.UnSignType.Equals(chanquanrengu)).Count(),
                    PingGuBuMan = g.Where(h => null != h.UnSignInfo && h.UnSignInfo.UnSignType.Equals(pinggubuman)).Count(),
                    JieGuoYiYi = g.Where(h => null != h.UnSignInfo && h.UnSignInfo.UnSignType.Equals(jieguoyiyi)).Count(),
                    YaoXianFang = g.Where(h => null != h.UnSignInfo && h.UnSignInfo.UnSignType.Equals(yaoxianfang)).Count(),
                    DaiGuanFang = g.Where(h => null != h.UnSignInfo && h.UnSignInfo.UnSignType.Equals(daiguanfang)).Count(),
                    FeiZhuZhai = g.Where(h => null != h.UnSignInfo && h.UnSignInfo.UnSignType.Equals(feizhuzhai)).Count(),
                    QiTaYuanYin = g.Where(h => null != h.UnSignInfo && h.UnSignInfo.UnSignType.Equals(qita)).Count()

                })
                .ToList();
            #endregion

            #region 创表格

            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet1 = book.CreateSheet("Sheet1");


            IRow rowHead = sheet1.CreateRow(0);
            rowHead.Height = 40 * 20;

            ICell cellHead = rowHead.CreateCell(0);
            cellHead.SetCellValue(hremove+ tablename);
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 18));//合并单元格
            //表名样式
            ICellStyle styleHead = book.CreateCellStyle();
            styleHead.Alignment = HorizontalAlignment.Center;
            styleHead.VerticalAlignment = VerticalAlignment.Center;
            IFont font = book.CreateFont();
            font.Boldweight = short.MaxValue;
            font.FontHeight = 40*10;
            styleHead.BorderLeft = BorderStyle.Thin;
            styleHead.BorderRight = BorderStyle.Thin;
            styleHead.BorderTop = BorderStyle.Thin;
            styleHead.BorderBottom = BorderStyle.Thin;
            styleHead.BottomBorderColor = HSSFColor.Black.Index;
            styleHead.TopBorderColor = HSSFColor.Black.Index;
            styleHead.LeftBorderColor = HSSFColor.Black.Index;
            styleHead.RightBorderColor = HSSFColor.Black.Index;
            styleHead.SetFont(font);
            cellHead.CellStyle = styleHead;
            //标题样式
            ICellStyle styleCell = book.CreateCellStyle();
            styleCell.Alignment = HorizontalAlignment.Center;
            styleCell.VerticalAlignment = VerticalAlignment.Center;
            styleCell.WrapText = true;
            styleCell.BorderLeft = BorderStyle.Thin;
            styleCell.BorderRight = BorderStyle.Thin;
            styleCell.BorderTop = BorderStyle.Thin;
            styleCell.BorderBottom = BorderStyle.Thin;
            styleCell.BottomBorderColor = HSSFColor.Black.Index;
            styleCell.TopBorderColor = HSSFColor.Black.Index;
            styleCell.LeftBorderColor = HSSFColor.Black.Index;
            styleCell.RightBorderColor = HSSFColor.Black.Index;

            IRow rowTitle1 = sheet1.CreateRow(1);
            rowTitle1.Height = 20 * 20;
            rowTitle1.CreateCell(0).SetCellValue("序号");
            rowTitle1.CreateCell(1).SetCellValue("地号");
            rowTitle1.CreateCell(2).SetCellValue("总户数");
            rowTitle1.CreateCell(3).SetCellValue("已签户数");
            var cell4 = rowTitle1.CreateCell(4);
            cell4.SetCellValue("剩余户数");
            cell4.CellStyle = styleCell;
            rowTitle1.CreateCell(5).SetCellValue("剩余户住房基本情况");
            rowTitle1.CreateCell(10).SetCellValue("未搬迁原因");
            rowTitle1.CreateCell(18).SetCellValue("合计");
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 18));//合并单元格
            

            IRow rowTitle2 = sheet1.CreateRow(2);
            rowTitle2.Height = 30 * 20;
            var cell5 = rowTitle2.CreateCell(5);cell5.SetCellValue("公房");cell5.CellStyle = styleCell;
            var cell6 = rowTitle2.CreateCell(6);cell6.SetCellValue("私房");cell6.CellStyle = styleCell;
            var cell7 = rowTitle2.CreateCell(7);cell7.SetCellValue("代管");cell7.CellStyle = styleCell;
            var cell8 = rowTitle2.CreateCell(8);cell8.SetCellValue("自管");cell8.CellStyle = styleCell;
            var cell9 = rowTitle2.CreateCell(9);cell9.SetCellValue("其他");cell9.CellStyle = styleCell;
            var cell10 = rowTitle2.CreateCell(10);cell10.SetCellValue("析产户");cell10.CellStyle = styleCell;
            var cell11 = rowTitle2.CreateCell(11); cell11.SetCellValue("产权人/承租人已故"); cell11.CellStyle = styleCell;
            var cell12 = rowTitle2.CreateCell(12); cell12.SetCellValue("对评估价不满"); cell12.CellStyle = styleCell;
            var cell13 = rowTitle2.CreateCell(13); cell13.SetCellValue("对未认定结果有异议"); cell13.CellStyle = styleCell;
            var cell14 = rowTitle2.CreateCell(14); cell14.SetCellValue("要求现房或多套房屋安置"); cell14.CellStyle = styleCell;
            var cell15 = rowTitle2.CreateCell(15); cell15.SetCellValue("代管房"); cell15.CellStyle = styleCell;
            var cell16 = rowTitle2.CreateCell(16); cell16.SetCellValue("其他"); cell16.CellStyle = styleCell;
            var cell17 = rowTitle2.CreateCell(17);cell17.SetCellValue("非住宅");cell17.CellStyle = styleCell;

            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 0, 0));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 1, 1));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 2, 2));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 3, 3));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 4, 4));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 18, 18));//合并单元格

            sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 5, 9));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 10, 17));//合并单元格

            #endregion

            #region 填数据

            int i = 3;
            foreach(var item in list)
            {
                IRow rowContent = sheet1.CreateRow(i);
                var cellContent0 = rowContent.CreateCell(0);cellContent0.SetCellValue(i-2);cellContent0.CellStyle = styleCell;
                var cellContent1 = rowContent.CreateCell(1);cellContent1.SetCellValue(item.LandParcel);cellContent1.CellStyle = styleCell;
                var cellContent2 = rowContent.CreateCell(2); cellContent2.SetCellValue(item.ZongHuShu); cellContent2.CellStyle = styleCell;
                var cellContent3 = rowContent.CreateCell(3); cellContent3.SetCellValue(item.YiQianHuShu); cellContent3.CellStyle = styleCell;
                var cellContent4 = rowContent.CreateCell(4); cellContent4.SetCellValue(item.ShengYuHuShu); cellContent4.CellStyle = styleCell;

                var cellContent5 = rowContent.CreateCell(5); cellContent5.SetCellValue(item.GongFang); cellContent5.CellStyle = styleCell;
                var cellContent6 = rowContent.CreateCell(6); cellContent6.SetCellValue(item.SiFang); cellContent6.CellStyle = styleCell;
                var cellContent7 = rowContent.CreateCell(7); cellContent7.SetCellValue(item.DaiGuan); cellContent7.CellStyle = styleCell;
                var cellContent8 = rowContent.CreateCell(8); cellContent8.SetCellValue(item.ZiGuan); cellContent8.CellStyle = styleCell;
                var cellContent9 = rowContent.CreateCell(9); cellContent9.SetCellValue(item.QiTa); cellContent9.CellStyle = styleCell;

                var cellContent10 = rowContent.CreateCell(10); cellContent10.SetCellValue(item.XiChanHu); cellContent10.CellStyle = styleCell;
                var cellContent11 = rowContent.CreateCell(11); cellContent11.SetCellValue(item.ChanQuanRenGu); cellContent11.CellStyle = styleCell;
                var cellContent12 = rowContent.CreateCell(12); cellContent12.SetCellValue(item.PingGuBuMan); cellContent12.CellStyle = styleCell;
                var cellContent13 = rowContent.CreateCell(13); cellContent13.SetCellValue(item.JieGuoYiYi); cellContent13.CellStyle = styleCell;
                var cellContent14 = rowContent.CreateCell(14); cellContent14.SetCellValue(item.YaoXianFang); cellContent14.CellStyle = styleCell;
                var cellContent15 = rowContent.CreateCell(15); cellContent15.SetCellValue(item.DaiGuanFang); cellContent15.CellStyle = styleCell;
                var cellContent16 = rowContent.CreateCell(16); cellContent16.SetCellValue(item.FeiZhuZhai); cellContent16.CellStyle = styleCell;
                var cellContent17 = rowContent.CreateCell(17); cellContent17.SetCellValue(item.QiTaYuanYin); cellContent17.CellStyle = styleCell;

                var cellContent18 = rowContent.CreateCell(18); cellContent18.SetCellValue(item.ZongHuShu); cellContent18.CellStyle = styleCell;
            }

            #endregion

            #region 输出到客户端

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", hremove+tablename+".xls");

            #endregion
        }

        [HttpPost]
        public ActionResult ToXls2(string hremove)
        {
            string tablename = "未拆迁户统计表";
            #region 查数据
            string daiqianshu = 签署类型.待签署.ToString();

            var list = db.HRemoves.SingleOrDefault(h => h.Name.Equals(hremove)).Householdes
                .Where(hh=>hh.SignStatus.Equals(daiqianshu) && null!=hh.UnSignInfo)
                .OrderByDescending(hh => hh.Id) 
                .ToList();
            #endregion

            #region 创表格
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet1 = book.CreateSheet("Sheet1");


            IRow rowHead = sheet1.CreateRow(0);
            rowHead.Height = 40 * 20;

            ICell cellHead = rowHead.CreateCell(0);
            cellHead.SetCellValue(hremove + tablename);
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 21));//合并单元格
            //表名样式
            ICellStyle styleHead = book.CreateCellStyle();
            styleHead.Alignment = HorizontalAlignment.Center;
            styleHead.VerticalAlignment = VerticalAlignment.Center;
            IFont font = book.CreateFont();
            font.Boldweight = short.MaxValue;
            font.FontHeight = 40 * 10;
            styleHead.BorderLeft = BorderStyle.Thin;
            styleHead.BorderRight = BorderStyle.Thin;
            styleHead.BorderTop = BorderStyle.Thin;
            styleHead.BorderBottom = BorderStyle.Thin;
            styleHead.BottomBorderColor = HSSFColor.Black.Index;
            styleHead.TopBorderColor = HSSFColor.Black.Index;
            styleHead.LeftBorderColor = HSSFColor.Black.Index;
            styleHead.RightBorderColor = HSSFColor.Black.Index;
            styleHead.SetFont(font);
            cellHead.CellStyle = styleHead;
            //标题样式
            ICellStyle styleCell = book.CreateCellStyle();
            styleCell.Alignment = HorizontalAlignment.Center;
            styleCell.VerticalAlignment = VerticalAlignment.Center;
            styleCell.WrapText = true;
            styleCell.BorderLeft = BorderStyle.Thin;
            styleCell.BorderRight = BorderStyle.Thin;
            styleCell.BorderTop = BorderStyle.Thin;
            styleCell.BorderBottom = BorderStyle.Thin;
            styleCell.BottomBorderColor = HSSFColor.Black.Index;
            styleCell.TopBorderColor = HSSFColor.Black.Index;
            styleCell.LeftBorderColor = HSSFColor.Black.Index;
            styleCell.RightBorderColor = HSSFColor.Black.Index;

            IRow rowTitle1 = sheet1.CreateRow(1);
            rowTitle1.Height = 20 * 20;
            rowTitle1.CreateCell(0).SetCellValue("序号");
            rowTitle1.CreateCell(1).SetCellValue("地号");
            rowTitle1.CreateCell(2).SetCellValue("产权人/承租人");
            rowTitle1.CreateCell(3).SetCellValue("身份证号");
            var cell4 = rowTitle1.CreateCell(4);
            cell4.SetCellValue("地址");
            cell4.CellStyle = styleCell;
            rowTitle1.CreateCell(5).SetCellValue("房屋性质");
            rowTitle1.CreateCell(8).SetCellValue("原房屋使用面积（m²）");
            rowTitle1.CreateCell(9).SetCellValue("原房屋建筑面积（m²）");
            rowTitle1.CreateCell(10).SetCellValue("房屋征收评估价（元）");
            rowTitle1.CreateCell(11).SetCellValue("装修补偿费用（元）");
            rowTitle1.CreateCell(12).SetCellValue("经营性补偿金（元）");
            rowTitle1.CreateCell(13).SetCellValue("实际应发补偿金（元）");
            rowTitle1.CreateCell(14).SetCellValue("房屋使用状况");
            rowTitle1.CreateCell(16).SetCellValue("联系电话");
            rowTitle1.CreateCell(17).SetCellValue("签订协议时间");
            rowTitle1.CreateCell(18).SetCellValue("未搬迁原因");
            rowTitle1.CreateCell(19).SetCellValue("情况说明及社会关系");
            rowTitle1.CreateCell(20).SetCellValue("约谈时间");
            rowTitle1.CreateCell(21).SetCellValue("类别");
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 18));//合并单元格


            IRow rowTitle2 = sheet1.CreateRow(2);
            rowTitle2.Height = 30 * 20;
            var cell5 = rowTitle2.CreateCell(5); cell5.SetCellValue("公房"); cell5.CellStyle = styleCell;
            var cell6 = rowTitle2.CreateCell(6); cell6.SetCellValue("私房"); cell6.CellStyle = styleCell;
            var cell7 = rowTitle2.CreateCell(7); cell7.SetCellValue("代管"); cell7.CellStyle = styleCell;
            
            var cell10 = rowTitle2.CreateCell(14); cell10.SetCellValue("出租"); cell10.CellStyle = styleCell;
            var cell11 = rowTitle2.CreateCell(15); cell11.SetCellValue("自用"); cell11.CellStyle = styleCell;
            

            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 0, 0));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 1, 1));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 2, 2));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 3, 3));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 4, 4));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 8, 8));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 9, 9));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 10, 10));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 11, 11));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 12, 12));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 13, 13));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 16, 16));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 17, 17));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 18, 18));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 19, 19));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 20, 20));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 21, 21));//合并单元格

            sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 5, 7));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 14, 15));//合并单元格

            #endregion

            #region 填数据

            int i = 3;
            foreach (var item in list)
            {
                IRow rowContent = sheet1.CreateRow(i);
                var cellContent0 = rowContent.CreateCell(0); cellContent0.SetCellValue(i - 2); cellContent0.CellStyle = styleCell;
                var cellContent1 = rowContent.CreateCell(1); cellContent1.SetCellValue(item.LandParcel); cellContent1.CellStyle = styleCell;
                var cellContent2 = rowContent.CreateCell(2); cellContent2.SetCellValue(item.Name + "/"+item.Successor); cellContent2.CellStyle = styleCell;
                var cellContent3 = rowContent.CreateCell(3); cellContent3.SetCellValue(item.CardNumber); cellContent3.CellStyle = styleCell;
                var cellContent4 = rowContent.CreateCell(4); cellContent4.SetCellValue(item.Address); cellContent4.CellStyle = styleCell;

                var cellContent5 = rowContent.CreateCell(5); cellContent5.SetCellValue(item.HouseProperty.Equals(房屋性质.公房.ToString())? "√":""); cellContent5.CellStyle = styleCell;
                var cellContent6 = rowContent.CreateCell(6); cellContent6.SetCellValue(item.HouseProperty.Equals(房屋性质.私房.ToString()) ? "√" : ""); cellContent6.CellStyle = styleCell;
                var cellContent7 = rowContent.CreateCell(7); cellContent7.SetCellValue(item.HouseProperty.Equals(房屋性质.代管.ToString()) ? "√" : ""); cellContent7.CellStyle = styleCell;

                var cellContent8 = rowContent.CreateCell(8); cellContent8.SetCellValue(item.UsefullArea); cellContent8.CellStyle = styleCell;
                var cellContent9 = rowContent.CreateCell(9); cellContent9.SetCellValue(item.Area); cellContent9.CellStyle = styleCell;

                var cellContent10 = rowContent.CreateCell(10); cellContent10.SetCellValue(item.AssessPrice.ToString()); cellContent10.CellStyle = styleCell;
                var cellContent11 = rowContent.CreateCell(11); cellContent11.SetCellValue(item.ZhuangxiuPrice.ToString()); cellContent11.CellStyle = styleCell;
                var cellContent12 = rowContent.CreateCell(12); cellContent12.SetCellValue(item.BusinessCompensation.ToString()); cellContent12.CellStyle = styleCell;
                var cellContent13 = rowContent.CreateCell(13); cellContent13.SetCellValue(item.YingFa.ToString()); cellContent13.CellStyle = styleCell;
                var cellContent14 = rowContent.CreateCell(14); cellContent14.SetCellValue(item.UseStatus.Equals(使用状况.出租.ToString()) ? "√" : ""); cellContent14.CellStyle = styleCell;
                var cellContent15 = rowContent.CreateCell(15); cellContent15.SetCellValue(item.UseStatus.Equals(使用状况.自住.ToString()) ? "√" : ""); cellContent15.CellStyle = styleCell;
                var cellContent16 = rowContent.CreateCell(16); cellContent16.SetCellValue(item.Mobile); cellContent16.CellStyle = styleCell;
                var cellContent17 = rowContent.CreateCell(17); cellContent17.SetCellValue(""); cellContent17.CellStyle = styleCell;

                var cellContent18 = rowContent.CreateCell(18); cellContent18.SetCellValue(item.UnSignInfo.UnSignType); cellContent18.CellStyle = styleCell;
                var cellContent19 = rowContent.CreateCell(19); cellContent19.SetCellValue(item.UnSignInfo.UnSignRemark); cellContent19.CellStyle = styleCell;
                var cellContent20 = rowContent.CreateCell(20); cellContent20.SetCellValue(item.UnSignInfo.YueTanTime.ToString()); cellContent20.CellStyle = styleCell;
                var cellContent21 = rowContent.CreateCell(21); cellContent21.SetCellValue(""); cellContent21.CellStyle = styleCell;
            }

            #endregion

            #region 输出到客户端

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", hremove + tablename + ".xls");

            #endregion
        }

        [HttpPost]
        public ActionResult ToXls3(string hremove)
        {
            string tablename = "住宅征收补偿金明细表";

            #region 查数据
            string yiqianshu = 签署类型.已签署.ToString();

            var list = db.HRemoves.SingleOrDefault(h => h.Name.Equals(hremove)).Householdes
                .Where(hh => hh.SignStatus.Equals(yiqianshu) && null != hh.SignInfo)
                .OrderByDescending(hh => hh.Id)
                .ToList();
            #endregion


            #region 创表格
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet1 = book.CreateSheet("Sheet1");


            IRow rowHead = sheet1.CreateRow(0);
            rowHead.Height = 40 * 20;

            ICell cellHead = rowHead.CreateCell(0);
            cellHead.SetCellValue(hremove + tablename);
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 21));//合并单元格
            //表名样式
            ICellStyle styleHead = book.CreateCellStyle();
            styleHead.Alignment = HorizontalAlignment.Center;
            styleHead.VerticalAlignment = VerticalAlignment.Center;
            IFont font = book.CreateFont();
            font.Boldweight = short.MaxValue;
            font.FontHeight = 40 * 10;
            styleHead.BorderLeft = BorderStyle.Thin;
            styleHead.BorderRight = BorderStyle.Thin;
            styleHead.BorderTop = BorderStyle.Thin;
            styleHead.BorderBottom = BorderStyle.Thin;
            styleHead.BottomBorderColor = HSSFColor.Black.Index;
            styleHead.TopBorderColor = HSSFColor.Black.Index;
            styleHead.LeftBorderColor = HSSFColor.Black.Index;
            styleHead.RightBorderColor = HSSFColor.Black.Index;
            styleHead.SetFont(font);
            cellHead.CellStyle = styleHead;
            //标题样式
            ICellStyle styleCell = book.CreateCellStyle();
            styleCell.Alignment = HorizontalAlignment.Center;
            styleCell.VerticalAlignment = VerticalAlignment.Center;
            styleCell.WrapText = true;
            styleCell.BorderLeft = BorderStyle.Thin;
            styleCell.BorderRight = BorderStyle.Thin;
            styleCell.BorderTop = BorderStyle.Thin;
            styleCell.BorderBottom = BorderStyle.Thin;
            styleCell.BottomBorderColor = HSSFColor.Black.Index;
            styleCell.TopBorderColor = HSSFColor.Black.Index;
            styleCell.LeftBorderColor = HSSFColor.Black.Index;
            styleCell.RightBorderColor = HSSFColor.Black.Index;

            IRow rowTitle1 = sheet1.CreateRow(1);
            rowTitle1.Height = 20 * 20;
            rowTitle1.CreateCell(0).SetCellValue("序号");
            rowTitle1.CreateCell(1).SetCellValue("姓名");
            rowTitle1.CreateCell(2).SetCellValue("房屋地址");
            rowTitle1.CreateCell(3).SetCellValue("身份证号");
            rowTitle1.CreateCell(4).SetCellValue("协议编号");
            rowTitle1.CreateCell(5).SetCellValue("使用性质");
            rowTitle1.CreateCell(7).SetCellValue("应发补偿金（元）");
            rowTitle1.CreateCell(8).SetCellValue("补偿方式");
            rowTitle1.CreateCell(10).SetCellValue("实发金额（元）");
            rowTitle1.CreateCell(11).SetCellValue("领取人签/章");
            rowTitle1.CreateCell(12).SetCellValue("领取时间");
            rowTitle1.CreateCell(13).SetCellValue("备注");
            
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 13));//合并单元格


            IRow rowTitle2 = sheet1.CreateRow(2);
            rowTitle2.Height = 30 * 20;
            var cell5 = rowTitle2.CreateCell(5); cell5.SetCellValue("住宅"); cell5.CellStyle = styleCell;
            var cell6 = rowTitle2.CreateCell(6); cell6.SetCellValue("兼用房"); cell6.CellStyle = styleCell;

            var cell10 = rowTitle2.CreateCell(8); cell10.SetCellValue("房屋"); cell10.CellStyle = styleCell;
            var cell11 = rowTitle2.CreateCell(9); cell11.SetCellValue("货币"); cell11.CellStyle = styleCell;


            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 0, 0));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 1, 1));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 2, 2));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 3, 3));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 4, 4));//合并单元格

            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 7, 7));//合并单元格

            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 10, 10));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 11, 11));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 12, 12));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 2, 13, 13));//合并单元格
            

            sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 5, 6));//合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(1, 1, 8, 9));//合并单元格

            #endregion

            #region 填数据

            int i = 3;
            foreach (var item in list)
            {
                IRow rowContent = sheet1.CreateRow(i);
                var cellContent0 = rowContent.CreateCell(0); cellContent0.SetCellValue(i - 2); cellContent0.CellStyle = styleCell;
                var cellContent1 = rowContent.CreateCell(1); cellContent1.SetCellValue(item.Name); cellContent1.CellStyle = styleCell;
                var cellContent2 = rowContent.CreateCell(2); cellContent2.SetCellValue(item.Address); cellContent2.CellStyle = styleCell;
                var cellContent3 = rowContent.CreateCell(3); cellContent3.SetCellValue(item.CardNumber); cellContent3.CellStyle = styleCell;
                var cellContent4 = rowContent.CreateCell(4); cellContent4.SetCellValue(item.SignInfo.SignNumber); cellContent4.CellStyle = styleCell;

                var cellContent5 = rowContent.CreateCell(5); cellContent5.SetCellValue(item.IsPartUse.Equals(是否兼用房.否.ToString()) ? "√" : ""); cellContent5.CellStyle = styleCell;
                var cellContent6 = rowContent.CreateCell(6); cellContent6.SetCellValue(item.IsPartUse.Equals(是否兼用房.是.ToString()) ? "√" : ""); cellContent6.CellStyle = styleCell;
                var cellContent7 = rowContent.CreateCell(7); cellContent7.SetCellValue(item.YingFa.ToString()); cellContent7.CellStyle = styleCell;

                var cellContent8 = rowContent.CreateCell(8); cellContent8.SetCellValue(item.CompensationType.Equals(补偿类型.房屋.ToString()) ? "√" : ""); cellContent8.CellStyle = styleCell;
                var cellContent9 = rowContent.CreateCell(9); cellContent9.SetCellValue(item.CompensationType.Equals(补偿类型.货币.ToString()) ? "√" : ""); cellContent9.CellStyle = styleCell;

                var cellContent10 = rowContent.CreateCell(10); cellContent10.SetCellValue(item.ShiFa.ToString()); cellContent10.CellStyle = styleCell;
                var cellContent11 = rowContent.CreateCell(11); cellContent11.SetCellValue(item.SignInfo.GetMoneyUser); cellContent11.CellStyle = styleCell;
                var cellContent12 = rowContent.CreateCell(12); cellContent12.SetCellValue(item.SignInfo.GetMoneyTime.ToString()); cellContent12.CellStyle = styleCell;
                var cellContent13 = rowContent.CreateCell(13); cellContent13.SetCellValue(item.SignInfo.Remark); cellContent13.CellStyle = styleCell;
                
            }

            #endregion

            #region 输出到客户端

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", hremove + tablename + ".xls");

            #endregion
        }



        #endregion
    }
}