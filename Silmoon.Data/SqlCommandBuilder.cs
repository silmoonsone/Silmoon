using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Silmoon.Data
{
    public class SqlCommandBuilder
    {
        public SqlOperate Operate { get; set; }
        public string TableName { get; set; }
        public string[] SelectingFields { get; set; }
        public int SelectTopN = 0;

        public string SqlSelectCommandString(string tableName, string[] selectingFields)
        {
            string result = "SELECT";
            if (SelectTopN > 0) result += " TOP " + SelectTopN;
            if (SelectingFields == null || result.Length == 0)
                result += " *";
            result += SmString.MergeStringArray(SelectingFields, ",", true, "", "");
            result += " FROM " + tableName;

            return result;
        }

        public class SelectQueryParams
        {
            public string TableName { get; set; }
            public string[] SelectingFields { get; set; }
            public int SelectTopN { get; set; }

        }
        public enum SqlOperate
        {
            SELECT, INSERT, UPDATE, DELETE,
        }
        public class WhereParams
        {
            public class WhereParam
            {
                public enum WhereLogic
                {
                    AND, OR
                }
                public WhereLogic Logic { get; set; }
                public string Key { get; set; }
                public string Value { get; set; }
            }
            ArrayList array = new ArrayList();
            public bool AddWhere(WhereParam.WhereLogic logic, string key, string value)
            {
                foreach (WhereParam item in array)
                    if (item.Key == key) return false;
                WhereParam p = new WhereParam();
                p.Logic = logic;
                p.Key = key;
                p.Value = value;
                array.Add(p);
                return true;
            }
            public bool RemoveWhere(string key)
            {
                foreach (WhereParam item in array)
                {
                    if (item.Key == key)
                    {
                        array.Remove(item);
                        return true;
                    }
                }
                return false;
            }
            public WhereParam this[string key]
            {
                get
                {
                    foreach (WhereParam item in array)
                    {
                        if (item.Key == key)
                            return item;
                    }
                    return null;
                }
                set
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        if (((WhereParam)array[i]).Key == key)
                            array[i] = value;
                    }
                }
            }
        }
    }
}