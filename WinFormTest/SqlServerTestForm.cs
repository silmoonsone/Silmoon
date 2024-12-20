using Silmoon.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormTest
{
    public partial class SqlServerTestForm : Form
    {
        public SqlServerTestForm()
        {
            InitializeComponent();
        }

        private void ctlMainTestButton_Click(object sender, EventArgs e)
        {
            using SqlExecuter sqlExecuter = new SqlExecuter("Server=(local); Uid=TestDB; Pwd=TestDB; Database=TestDB; TrustServerCertificate=true");
            var user = new User()
            {
                Username = "Silmoon"
            };

            using (var trans = sqlExecuter.BeginTransaction())
            {
                sqlExecuter.CreateTable<User>("users");

                sqlExecuter.AddObject("users", user);
                sqlExecuter.CommitTransaction(trans);
            }

            //sqlExecuter.AddObject("users", user);

        }
    }
}
