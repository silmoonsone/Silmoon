using System;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Silmoon.Web.Page
{
    public class RewriteMasterPage : System.Web.UI.MasterPage
    {
        public RewriteMasterPage()
        { }
        protected override void Render(HtmlTextWriter writer)
        {
            if (writer is System.Web.UI.Html32TextWriter)
            {
                writer = new FormFixerHtml32TextWriter(writer.InnerWriter);
            }
            else
            {
                writer = new FormFixerHtmlTextWriter(writer.InnerWriter);
            }
            base.Render(writer);
        }
    }
}
