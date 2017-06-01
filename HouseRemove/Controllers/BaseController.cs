using HouseRemove;
using HouseRemove.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HouseRemove.Controllers
{
    public class BaseController : Controller
    {

        protected hrEntitysContainer db = new hrEntitysContainer();

        protected Manager CurrentUser
        {
            get
            {
                if (HttpContext.User.Identity.Name != null)
                {
                    Manager me = db.Managers.SingleOrDefault(m => m.Name.Equals(HttpContext.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (me == null)
                    {
                        FormsAuthentication.SignOut();
                        Response.Redirect("~/Account/Login");
                    }
                    return me;
                }
                return null;
            }
        }

        

        protected decimal CalculateFee(decimal transactionAmount, string settingKey)
        {
            string strRateValue = GetSystemSettingString(settingKey);

            if (strRateValue == null)
                return 0m;
            if (strRateValue.IndexOf("%") >= 0)
            {
                //Fee by percent
                return GetCorrectSettingPercentValue(settingKey) * transactionAmount;
            }
            else
            {
                //Fixed Fee
                return GetSystemSettingDecimal(settingKey);
            }
        }

        protected decimal GetCorrectSettingPercentValue(string settingKey)
        {
            string strRateValue = GetSystemSettingString(settingKey);
            if (strRateValue == null)
                return 0m;
            if (strRateValue.IndexOf("%") >= 0)
            {
                //Setting like 60%
                return decimal.Parse(strRateValue.Replace("%", "")) / 100;
            }
            else
            {
                return GetSystemSettingDecimal(settingKey);        //Setting like  0.6
            }
        }

        protected String GetSystemSettingString(string key)
        {
            var s = db.SystemSettings.SingleOrDefault(m => m.Key.Equals(key));
            if (s == null)
                return null;
            else
                return s.Value;
        }

        protected decimal GetSystemSettingDecimal(string key)
        {
            string value = GetSystemSettingString(key);
            return decimal.Parse(value);
        }

        protected bool GetSystemSettingBoolean(string key)
        {
            string value = GetSystemSettingString(key);
            return bool.Parse(value);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && db != null)
            {
                db.Dispose();
                db = null;
            }
            base.Dispose(disposing);
        }

        protected void SetMyAccountViewModel()
        {
            Manager user = CurrentUser;
            if(user!=null)
                TempData["MyAccount"] = GetMyAccountViewModel(user);
        }

        protected MyAccountViewModel GetMyAccountViewModel(Manager user)
        {
            return new MyAccountViewModel
            {
                Id=user.Id,
                Name=user.Name,
                Role=user.Role                
            };
        }


    }
}