using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HouseRemove.Models;
using HouseRemove.Helper;

namespace HouseRemove.Controllers
{
    public class RedHeadFileController : BaseController
    {
        // GET: RedHeadFile
        public ActionResult Index()
        {
            if (TempData.ContainsKey("ModelState"))
            {
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            }
            
            var model = db.RedHeadFiles.Where(f => f.Id > 0);
            SetMyAccountViewModel();
            return View(model);
        }

        public ActionResult Add(int id)
        {
            var model = new RedHeadFileViewModel();
            model.PubTime = DateTime.Now;
            model.ZhiXingTime = DateTime.Now;
            if (id > 0)
            {
                RedHeadFile redheadfile = db.RedHeadFiles.Find(id);
                if (redheadfile == null)
                {
                    return new HttpNotFoundResult();
                }
                model.Id = redheadfile.Id;
                model.Type = redheadfile.Type;
                model.Title = redheadfile.Title;
                model.PubTime = redheadfile.PubTime;
                model.ZhiXingTime = redheadfile.ZhiXingTime;
                model.Org = redheadfile.Org;
            }

            SetMyAccountViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(RedHeadFileViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.Id>0)
                {
                    RedHeadFile redheadfile = db.RedHeadFiles.Find(model.Id);
                    if(redheadfile==null)
                    {
                        return new HttpNotFoundResult();
                    }
                    redheadfile.Title = model.Title;
                    redheadfile.Org = model.Org;
                    redheadfile.FilePath = model.FilePath;
                    redheadfile.PubTime = model.PubTime;
                    redheadfile.ZhiXingTime = model.ZhiXingTime;
                    redheadfile.Type = model.Type;
                    db.Entry(redheadfile).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    db.RedHeadFiles.Add(new RedHeadFile {
                        Title=model.Title,
                        Org=model.Org,
                        PubTime=model.PubTime,
                        ZhiXingTime=model.ZhiXingTime,
                        Type=model.Type,
                        FilePath=model.FilePath,
                        CreateTime=DateTime.Now
                    });
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            SetMyAccountViewModel();
            return View(model);
        }

        public ActionResult Del(int id)
        {
            if(id>0)
            {
                RedHeadFile redheadfile = db.RedHeadFiles.Find(id);
                if(redheadfile!=null)
                {
                    db.RedHeadFiles.Remove(redheadfile);
                    db.SaveChanges();                    
                }
            }

            return RedirectToAction("Index");
        }


        public ActionResult Upload()
        {
            var requestHelper = new RequestHelper(this.Request);
            var filePath = requestHelper.SaveImageToServer();

            return Content(filePath);
        }
    }
}