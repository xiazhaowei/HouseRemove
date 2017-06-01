using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HouseRemove.Extensions
{
    public static class ListControlUtil
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="name">控件name</param>
        /// <param name="lists">可选值列表</param>
        /// <param name="repeatDirection">排版方向</param>
        /// <param name="type">checkbox/radio</param>
        /// <param name="stateValue">选中值</param>
        /// <returns></returns>
        public static MvcHtmlString GenerateHtml(string name, ICollection<string> lists, RepeatDirection repeatDirection, string type, object stateValue)
        {
            StringBuilder html = new StringBuilder();
            int i = 0;
            bool isCheckBox = type == "checkbox";
            if (repeatDirection == RepeatDirection.Horizontal)
            {
                foreach (var listitem in lists)
                {
                    i++;
                    string id = string.Format("{0}_{1}", name, i);                    

                    bool isChecked = false;
                    if (isCheckBox)
                    {
                        IEnumerable<string> currentValues = stateValue as IEnumerable<string>;
                        isChecked = (null != currentValues && currentValues.Equals(listitem));
                    }
                    else
                    {
                        string currentValue = stateValue as string;
                        isChecked = (null != currentValue && listitem == currentValue);
                    }

                    html.Append( GenerateRadioHtml(name, id, listitem, listitem, isChecked, type,repeatDirection));
                }
            }
            
            return new MvcHtmlString(html.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">控件Name</param>
        /// <param name="id">控件Id</param>
        /// <param name="labelText">显示文本</param>
        /// <param name="value">值文本</param>
        /// <param name="isChecked"></param>
        /// <param name="type">控件type</param>
        /// <returns></returns>
        private static string GenerateRadioHtml(string name, string id, string labelText, string value, bool isChecked, string type, RepeatDirection repeatDirection)
        {
            StringBuilder sb = new StringBuilder();

            TagBuilder label = new TagBuilder("label");
            //label.MergeAttribute("for", id);
            label.SetInnerText(labelText);
            if (type.Equals("radio"))
            {
                if(repeatDirection == RepeatDirection.Horizontal)
                    label.AddCssClass("radio-inline");
                else
                    label.AddCssClass("radio");
            }
            if (type.Equals("checkbox"))
            {                
                if (repeatDirection == RepeatDirection.Horizontal)                
                    label.AddCssClass("checkbox-inline");
                else
                    label.AddCssClass("checkbox");
            }
            

            TagBuilder input = new TagBuilder("input");
            input.GenerateId(id);
            input.MergeAttribute("name", name);
            input.MergeAttribute("type", type);
            input.MergeAttribute("value", value);
            if (isChecked)
            {
                input.MergeAttribute("checked", "checked");
            }
            label.InnerHtml += input.ToString();
            sb.AppendLine(label.ToString());
            return sb.ToString();
        }
    }
}