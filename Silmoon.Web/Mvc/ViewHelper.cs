using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Silmoon.Web.Mvc
{
    public class ViewHelper
    {
        public static IEnumerable<SelectListItem> GetSelectListItems(string[] values, string value)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            if (!values.Contains(value))
            {
                result.Add(new SelectListItem() { Text = value, Value = value, Selected = true });
            }

            foreach (var item in values)
            {
                result.Add(new SelectListItem() { Text = item, Value = item, Selected = item == value });
            }

            return result;
        }
        public static IEnumerable<SelectListItem> GetSelectListItems(Enum value)
        {
            return EnumHelper.GetSelectList(value.GetType(), value);
        }
    }
}
