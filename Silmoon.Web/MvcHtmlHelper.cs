using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Silmoon.Web
{
    public class MvcHtmlHelper
    {
        public static List<SelectListItem> MakeSelectEnumListItems<TEnum>(object selectedItem = null)
        {
            var type = typeof(TEnum);
            if (type.IsEnum)
            {
                var names = type.GetEnumNames();
                var values = type.GetEnumValues();
                var result = new List<SelectListItem>();
                for (int i = 0; i < names.Length; i++)
                {
                    var item = new SelectListItem() { Text = names[i], Value = values.GetValue(i).ToString() };
                    result.Add(item);
                    if (selectedItem != null && (int)values.GetValue(i) == (int)selectedItem)
                        item.Selected = true;
                }
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
