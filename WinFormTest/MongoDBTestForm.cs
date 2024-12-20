using MongoDB.Driver;
using Silmoon.Data.MongoDB;
using Silmoon.Extension.Models.Enums;
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
    public partial class MongoDBTestForm : Form
    {
        MongoExecuter MongoExecuter { get; set; } = new MongoExecuter("mongodb://localhost:27017", "TestDB");
        IClientSessionHandle session;

        public MongoDBTestForm()
        {
            InitializeComponent();
            session = MongoExecuter.StartSession(false);
        }

        private void ctlStartSessionButton_Click(object sender, EventArgs e) => session.StartTransaction();

        private void ctlWriteBySessionButton_Click(object sender, EventArgs e)
        {
            var personal = new Personal
            {
                Name = "Silmoon",
                Age = 30,
                Sex = Sex.Male
            };

            MongoExecuter.AddObject("Personal", personal, session);
        }

        private void ctlWriteButton_Click(object sender, EventArgs e)
        {
            var personal = new Personal
            {
                Name = "Silmoon2",
                Age = 30,
                Sex = Sex.Male
            };

            var collection = MongoExecuter.GetCollection<Personal>("Personal");
            collection.InsertOne(personal);
        }

        private void ctlCommitSessionButton_Click(object sender, EventArgs e) => session.CommitTransaction();

        private void ctlAbortSessionButton_Click(object sender, EventArgs e) => session.AbortTransaction();
    }
    public class Personal : IdObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
    }
}
