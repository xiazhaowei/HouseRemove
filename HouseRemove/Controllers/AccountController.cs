using HouseRemove.Lib;
using HouseRemove.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HouseRemove.Controllers
{
    public class AccountController : BaseController
    {

        public ActionResult LockScreen(string name)
        {
            ViewBag.name = name;
            return View();
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Manager user = db.Managers.SingleOrDefault(m => m.Name.Equals(model.Mobile, StringComparison.InvariantCultureIgnoreCase)
                    && m.Password.Equals(model.Password));
                if (user != null)
                {
                    if (user.Status.Equals(管理员状态.正常.ToString()))
                    {
                        FormsAuthentication.SetAuthCookie(user.Name, false);
                        //Save Member login log
                        //String userClient = Request.Browser.Browser + " " + Request.Browser.Version + " (" + Request.Browser.Platform + ")";                       
                        return RedirectToLocal(returnUrl);
                    }                    
                    else if (user.Status.Equals(管理员状态.锁定.ToString()))
                    {
                        ModelState.AddModelError("", "帐号锁定，请与管理员联系!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "用户名/密码错误!");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {              

        //        if (db.Members.Count(m => m.Mobile.Equals(model.Mobile, StringComparison.InvariantCultureIgnoreCase)) > 0)
        //        {
        //            ModelState.AddModelError("", "手机号已被注册!");
        //            return View(model);
        //        }               
        //        try
        //        {
        //            Member newUser = new Member
        //            {
        //                Mobile = model.Mobile,
        //                Password = model.Password,
        //                RegisterTime = DateTime.Now,
        //                CompanyName = model.CompanyName,
        //                UserName = model.UserName,
        //                Status = 会员状态.正常.ToString()
        //            };
        //            db.Members.Add(newUser);                    

        //            db.SaveChanges();
        //            ViewBag.ActionMessage = "注册成功！请返回登录。";

        //            //自动登录
        //            FormsAuthentication.SetAuthCookie(newUser.Mobile, false);
        //            return RedirectToAction("Index","Home");
        //        }
        //        catch (DbEntityValidationException dbEx)
        //        {                    
        //        }

        //        //return View(new RegisterViewModel());
                
        //        return RedirectToAction("Login");
        //    }

        //    return View(model);
        //}

        //
        // POST: /Account/LogOff
        public ActionResult LogOff(string returnUrl)
        {
            FormsAuthentication.SignOut();
            if (returnUrl != null)
                return RedirectToLocal(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public string GetErrorMessageForKey(ModelStateDictionary dictionary, string key)
        {
            return dictionary[key].Errors.Count > 0 ? dictionary[key].Errors.First().ErrorMessage : null;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}