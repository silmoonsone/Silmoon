using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using Silmoon;
using System.Windows.Forms;
using System.ComponentModel;

namespace Silmoon.IO
{
    public sealed class FileWatcher
    {
        ArrayList _watcherArr = new ArrayList();

        public event RenamedEventHandler RenameEvent;
        public event FileSystemEventHandler DeleteEvent;
        public event FileSystemEventHandler CreateEvent;
        public event FileSystemEventHandler EditEvent;
        public event FileEventHander FileEvent;
        public event CancelEventHandler OnStart;
        public event CancelEventHandler OnStop;
        private bool running = false;

        public bool Running
        {
            get { return running; }
            set { running = value; }
        }

        public FileWatcher()
        {

        }
        public FileWatcher(bool initDirvers, bool subDirector, bool throwException)
        {
            try
            {
                if (initDirvers)
                {
                    foreach (DriveInfo di in DriveInfo.GetDrives())
                    {
                        try
                        {
                            FileSystemWatcher fsw = new FileSystemWatcher();
                            fsw.Path = di.Name;
                            fsw.IncludeSubdirectories = subDirector;
                            fsw.Changed += new FileSystemEventHandler(fsw_Changed);
                            fsw.Created += new FileSystemEventHandler(fsw_Created);
                            fsw.Deleted += new FileSystemEventHandler(fsw_Deleted);
                            fsw.Renamed += new RenamedEventHandler(fsw_Renamed);
                            _watcherArr.Add(fsw);
                        }
                        catch { if (throwException) throw new Exception(di.Name + "ÅÌ·û´ò¿ªÊ§°Ü£¡"); }
                    }
                }
            }
            catch (Exception e) { MessageBox.Show(e.Message); MessageBox.Show(e.ToString()); }
        }
        public void InitDirvers(bool subDirector)
        {
            foreach (DriveInfo di in DriveInfo.GetDrives())
            {
                if (di.IsReady)
                {
                    try
                    {
                        FileSystemWatcher fsw = new FileSystemWatcher();
                        fsw.Path = di.Name;
                        fsw.IncludeSubdirectories = subDirector;
                        fsw.Changed += new FileSystemEventHandler(fsw_Changed);
                        fsw.Created += new FileSystemEventHandler(fsw_Created);
                        fsw.Deleted += new FileSystemEventHandler(fsw_Deleted);
                        fsw.Renamed += new RenamedEventHandler(fsw_Renamed);
                        _watcherArr.Add(fsw);
                    }
                    catch { throw new Exception(di.Name + "ÅÌ·û´ò¿ªÊ§°Ü£¡"); }
                }
            }
        }

        void fsw_Renamed(object sender, RenamedEventArgs e)
        {
            if (FileEvent != null) fileEvents(this, new FileEventArgs(e.ChangeType, e.FullPath, e.Name, e));
            if (RenameEvent != null) RenameEvent(sender, e);
        }
        void fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            if (FileEvent != null) fileEvents(this, new FileEventArgs(e.ChangeType, e.FullPath, e.Name, null));
            if (DeleteEvent != null) DeleteEvent(sender, e);
        }
        void fsw_Created(object sender, FileSystemEventArgs e)
        {
            if (FileEvent != null) fileEvents(this, new FileEventArgs(e.ChangeType, e.FullPath, e.Name, null));
            if (CreateEvent != null) CreateEvent(sender, e);
        }
        void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            if (FileEvent != null) fileEvents(this, new FileEventArgs(e.ChangeType, e.FullPath, e.Name, null));
            if (EditEvent != null) EditEvent(sender, e);
        }
        void fileEvents(object sender, FileEventArgs e)
        {
            if (FileEvent != null) FileEvent(sender, e);
        }

        public void Start()
        {
            CancelEventArgs args = new CancelEventArgs();
            args.Cancel = false;
            if (OnStart != null) OnStart(this, args);
            if (args.Cancel) return;

            for (int i = 0; i < _watcherArr.Count; i++)
            {
                FileSystemWatcher fsw = (FileSystemWatcher)_watcherArr[i]; ;
                fsw.EnableRaisingEvents = true;
            }
            running = true;
        }
        public void Stop()
        {
            CancelEventArgs args = new CancelEventArgs();
            args.Cancel = false;
            if (OnStop != null) OnStop(this, args);
            if (args.Cancel) return;

            for (int i = 0; i < _watcherArr.Count; i++)
            {
                FileSystemWatcher fsw = (FileSystemWatcher)_watcherArr[i]; ;
                fsw.EnableRaisingEvents = false;
            }
            running = false;
        }
    }
    public class FileEventArgs
    {
        public readonly WatcherChangeTypes ChangeType;
        public readonly string FullPath;
        public readonly string Name;
        public readonly RenamedEventArgs RenameName;

        public FileEventArgs(WatcherChangeTypes changeType, string fullname, string name, RenamedEventArgs renameArgs)
        {
            this.ChangeType = changeType;
            this.FullPath = fullname;
            this.Name = name;
            RenameName = renameArgs;
        }
    }
    public delegate void FileEventHander (object sender,FileEventArgs e);
}
