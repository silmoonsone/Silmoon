using Silmoon.Extension.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTesting;

public class App : AppBase, ISqlObject
{
    public int id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Guid UserGuid { get; set; }
    public string AppPrivateKey { get; set; }
    public string DisplayName { get; set; }
    public DateTime created_at { get; set; } = DateTime.Now;
}
public class AppBase
{
    public string AppId { get; set; }
    public string SignatureKey { get; set; }
    public string EncryptKey { get; set; }
    public Sex Level { get; set; }
    public UserGrade Status { get; set; }
}
public interface ISqlObject
{
    int id { get; set; }

    [DisplayName("创建日期")]
    DateTime created_at { get; set; }
}