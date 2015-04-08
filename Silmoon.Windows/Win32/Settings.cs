using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Silmoon.Windows.Win32
{
    public sealed class Settings
    {
        public static void SetAutoRun(string objectName, string path)
        {
            RegistryKey hklm = Registry.LocalMachine;
            try
            {
                RegistryKey run = hklm.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.ReadWriteSubTree);
                run.SetValue(objectName, path, RegistryValueKind.String);
                hklm.Close();
            }
            catch
            { }
        }
        public static void DelAutoRun(string objectName)
        {
            RegistryKey hklm = Registry.LocalMachine;
            try
            {
                RegistryKey run = hklm.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.ReadWriteSubTree);
                run.DeleteValue(objectName, false);
                hklm.Close();
            }
            catch
            { }
        }
        public static bool IsAutoRun(string objectName)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey run = hklm.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            string[] names = run.GetValueNames();
            foreach (string name in names)
            {
                if (name == objectName)
                    return true;
            }
            return false;
        }
    }
}