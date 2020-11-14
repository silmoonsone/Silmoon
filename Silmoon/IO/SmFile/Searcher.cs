using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Silmoon.IO.SmFile
{
    /// <summary>
    /// 目录文件搜索器
    /// </summary>
    public class Searcher
    {
        /// <summary>
        /// 获取制定目录下所有的文件名
        /// </summary>
        /// <param name="path">指定的目录</param>
        /// <param name="processCallBack">进度委托</param>
        /// <returns></returns>
        public string[] GetFiles(string path, SearcherEventHandler processCallBack = null)
        {
            ArrayList arrayList = new ArrayList();
            try
            {
                string[] fs = Directory.GetFiles(path);
                foreach (string file in fs)
                {
                    if (processCallBack != null)
                        processCallBack(file, Path.GetFileName(file), 1);
                    arrayList.Add(file);
                }
            }
            catch { }

            try
            {
                string[] ds = Directory.GetDirectories(path);
                foreach (string dpath in ds)
                {
                    if (processCallBack != null)
                        processCallBack(dpath, "", 2);
                    InternalGetFiles(dpath, arrayList, processCallBack);
                }
            }
            catch { }
            return (string[])arrayList.ToArray(typeof(string));
        }
        void InternalGetFiles(string path, ArrayList array, SearcherEventHandler processCallBack)
        {
            try
            {
                string[] fs = Directory.GetFiles(path);
                foreach (string file in fs)
                {
                    if (processCallBack != null)
                        processCallBack(file, Path.GetFileName(file), 1);
                    array.Add(file);
                }
            }
            catch { }

            try
            {
                string[] ds = Directory.GetDirectories(path);
                foreach (string dpath in ds)
                {
                    if (processCallBack != null)
                        processCallBack(dpath, "", 2);
                    InternalGetFiles(dpath, array, processCallBack);
                }
            }
            catch { }
        }

        /// <summary>
        /// 搜索文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="searchPattern">搜索字符串</param>
        /// <param name="searchOption">搜索选项</param>
        /// <returns>找到的文件名</returns>
        public string[] SearchFile(string path, string searchPattern, SearchOption searchOption, SearcherEventHandler processCallBack = null)
        {
            ArrayList arrayList = new ArrayList();
            try
            {
                string[] fs = Directory.GetFiles(path, searchPattern);
                foreach (string file in fs)
                {
                    if (processCallBack != null)
                        processCallBack(file, Path.GetFileName(file), 1);
                    arrayList.Add(file);
                }
            }
            catch { }
            if (searchOption == SearchOption.AllDirectories)
            {
                try
                {
                    string[] ds = Directory.GetDirectories(path);
                    foreach (string dpath in ds)
                    {
                        if (processCallBack != null)
                            processCallBack(dpath, "", 2);
                        InternalGetFiles(dpath, arrayList, processCallBack);
                    }
                }
                catch { }
            }
            return (string[])arrayList.ToArray(typeof(string));
        }
        void InternalSearchFile(string path, ArrayList array, string searchPattern, SearcherEventHandler processCallBack)
        {
            try
            {
                string[] fs = Directory.GetFiles(path, searchPattern);
                foreach (string file in fs)
                {
                    if (processCallBack != null)
                        processCallBack(file, Path.GetFileName(file), 1);
                    array.Add(file);
                }
            }
            catch { }

            try
            {
                string[] ds = Directory.GetDirectories(path);
                foreach (string dpath in ds)
                {
                    if (processCallBack != null)
                        processCallBack(dpath, "", 2);
                    InternalGetFiles(dpath, array, processCallBack);
                }
            }
            catch { }
        }
    }
    /// <summary>
    /// 表示在搜索文件时的事件处理过程
    /// </summary>
    /// <param name="fullPath">完整路径</param>
    /// <param name="name">文件或者文件夹名</param>
    /// <param name="type"></param>
    public delegate void SearcherEventHandler(string fullPath, string name, int type);
}
