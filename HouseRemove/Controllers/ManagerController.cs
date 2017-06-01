using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HouseRemove.Models;
using HouseRemove.Lib;

namespace HouseRemove.Controllers
{
    public class ManagerController : BaseController
    {
        // GET: Manager
        public ActionResult Index()
        {
            if (TempData.ContainsKey("ModelState"))
            {
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            }

            var model = db.Managers.Where(m => !m.Name.Equals("admin"));                

            SetMyAccountViewModel();
            return View(model);
        }


        public ActionResult Add(int id,string type)
        {
            var model = new ManagerViewModel();
            if(id>0)
            {
                Manager manager = db.Managers.SingleOrDefault(c=>c.Id==id);
                if(manager!=null)
                {
                    model.Id = manager.Id;
                    model.Name = manager.Name;
                    model.Role = manager.Role;
                    model.State = manager.Status;
                    model.Password = manager.Password;
                }
            }
            ViewBag.type = type;
            SetMyAccountViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(ManagerViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.Id>0)
                {//edit
                    Manager manager = db.Managers.Find(model.Id);
                    if(manager!=null)
                    {
                        manager.Name = model.Name;
                        manager.Role = model.Role;
                        manager.Status = model.State;
                        manager.Password = model.Password;

                        db.Entry(manager).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        ModelState.AddModelError("", "操作成功");
                        TempData["ModelState"] = ModelState;

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return new HttpNotFoundResult();
                    }
                }
                else
                {
                    db.Managers.Add(new Manager {
                        Name = model.Name,
                        Password=model.Password,
                        Role=model.Role,
                        Status=model.State
                    });
                    db.SaveChanges();

                    ModelState.AddModelError("", "操作成功");
                    TempData["ModelState"] = ModelState;

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
                Manager manager = db.Managers.Find(id);
                string role = 管理员角色.管理员.ToString();
                if(manager!=null)
                {
                    db.Managers.Remove(manager);
                    db.SaveChanges();

                    ModelState.AddModelError("", "操作成功");
                    TempData["ModelState"] = ModelState;

                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}