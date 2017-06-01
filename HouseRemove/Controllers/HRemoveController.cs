using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HouseRemove.Models;

namespace HouseRemove.Controllers
{
    public class HRemoveController : BaseController
    {
        // GET: HRemove
        public ActionResult Index()
        {
            if (TempData.ContainsKey("ModelState"))
            {
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            }


            var model = db.HRemoves.Where(h => h.Id > 0);

            SetMyAccountViewModel();
            return View(model);
        }

        public ActionResult Add(int id)
        {
            var model = new HRemoveViewModel();
            if (id>0)
            {
                HRemove hremove = db.HRemoves.SingleOrDefault(h=>h.Id==id);
                if(hremove!=null)
                {
                    model.Id = hremove.Id;
                    model.Name = hremove.Name;
                }
            }
            SetMyAccountViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(HRemoveViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.Id>0)
                {
                    HRemove hremove = db.HRemoves.Find(model.Id);
                    if (hremove == null)
                    {
                        return new HttpNotFoundResult();
                    }

                    hremove.Name = model.Name;
                    db.Entry(hremove).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    

                    db.HRemoves.Add(new HRemove
                    {
                        Name = model.Name
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
                HRemove hremove = db.HRemoves.Find(id);
                if(hremove!=null && hremove.Householdes.Count==0)
                {
                    db.HRemoves.Remove(hremove);
                    db.SaveChanges();
                    ModelState.AddModelError("", "操作成功。");
                    TempData["ModelState"] = ModelState;
                }
                else
                {
                    ModelState.AddModelError("", "项目包含拆迁户不能删除。");
                    TempData["ModelState"] = ModelState;
                }
                
            }
            return RedirectToAction("Index");
        }
    }
}