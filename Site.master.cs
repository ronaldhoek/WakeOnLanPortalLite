using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WakeOnLanLite
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblDebugInfo.Visible = HttpContext.Current.IsDebuggingEnabled;
            if (lblDebugInfo.Visible)
            {
                // Proces user + logged in user
                lblDebugInfo.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            }
        }
    }
}