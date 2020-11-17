using Silmoon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Silmoon.Web.Mvc
{
    public static class ViewHelper
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
        public static IEnumerable<SelectListItem> GetSelectListItems(IName[] values, IName value)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            if (!values.Contains(value))
            {
                result.Add(new SelectListItem() { Text = value.Name, Value = value.Name, Selected = true });
            }

            foreach (var item in values)
            {
                result.Add(new SelectListItem() { Text = value.Name, Value = value.Name, Selected = item.Name == value.Name });
            }

            return result;
        }
        public static IEnumerable<SelectListItem> GetSelectListItems(IName[] values, string value)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            if (!values.Contains(value))
            {
                result.Add(new SelectListItem() { Text = value, Value = value, Selected = true });
            }

            foreach (var item in values)
            {
                result.Add(new SelectListItem() { Text = item.Name, Value = item.Name, Selected = item.Name == value });
            }

            return result;
        }
        public static IEnumerable<SelectListItem> GetSelectListItems(INameValue[] values, INameValue value)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            if (!values.Contains(value))
            {
                result.Add(new SelectListItem() { Text = value.Name, Value = value.Value, Selected = true });
            }

            foreach (var item in values)
            {
                result.Add(new SelectListItem() { Text = value.Name, Value = value.Value, Selected = item.Value == value.Value });
            }

            return result;
        }
        public static IEnumerable<SelectListItem> GetSelectListItems(Enum value)
        {
            return EnumHelper.GetSelectList(value.GetType(), value);
        }

        public static bool Contains(this IName[] names, IName name)
        {
            foreach (var item in names)
            {
                if (item.Name == name.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Contains(this IName[] names, string name)
        {
            foreach (var item in names)
            {
                if (item.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Contains(this INameValue[] names, INameValue name)
        {
            foreach (var item in names)
            {
                if (item.Name == name.Name)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
