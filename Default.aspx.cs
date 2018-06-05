using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WakeOnLanLite
{
    public partial class Default : System.Web.UI.Page
    {
        private void ShowError(string msg)
        {
            lblErrors.Text = msg;
            lblErrors.Visible = true;
        }
        private void ShowMessage(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.Visible = true;
        }
        private void UpdateRefreshTime()
        {
            lblRefreshtime.Text = DateTime.Now.ToString("H:mm:ss");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrors.Visible = false;
            lblMessage.Visible = false;
        }
        protected void btnRefreshClick(object sender, EventArgs e)
        {
            gvComputers.DataBind();
        }
        protected void tmrRefreshComputers_Tick(object sender, EventArgs e)
        {
            gvComputers.DataBind();
        }
        protected void cbAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            tmrRefreshComputers.Enabled = ((CheckBox)sender).Checked;
        }
        protected void gvComputers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // e.CommandArgument = row index
                int rownum = int.Parse(e.CommandArgument.ToString());
                GridView gv = (GridView)e.CommandSource;

                // Regel info (naam) uitlezen
                string info = ((GridView)e.CommandSource).Rows[rownum].Cells[0].Text;

                if (e.CommandName.Equals("action", StringComparison.OrdinalIgnoreCase))
                {
                    // Status
                    if (((CheckBox)gv.Rows[rownum].Cells[4].Controls[0]).Checked)
                    {
                        // Host uitlezen
                        string host = ((GridView)e.CommandSource).Rows[rownum].Cells[1].Text;
                        // Afsluiten obv host
                        WOL.AquilaWolLibrary.Shutdown(host, WOL.AquilaWolLibrary.ShutdownFlags.Shutdown);
                        // Notify user
                        ShowMessage("Afsluitcommando is verstuurd naar '" + info + "'.");
                    }
                    else
                    {
                        // MAC-adress uitlezen
                        string mac = ((GridView)e.CommandSource).Rows[rownum].Cells[2].Text;
                        // Wakker maken obv MAC
                        WOL.AquilaWolLibrary.WakeUp(mac);
                        // Notify user
                        ShowMessage("Opstartcommando is verstuurd naar '" + info + "'.");
                    }
                }
                else if (e.CommandName.Equals("rdpdownload", StringComparison.OrdinalIgnoreCase))
                {
                    // Hostnaam uitlezen
                    string host = ((GridView)e.CommandSource).Rows[rownum].Cells[1].Text;

                    // Basis RDP bestand uitlezen
                    System.IO.TextReader t = new System.IO.StreamReader(HttpContext.Current.Server.MapPath(@"~\App_Data\BaseRDP.txt"));
                    string rdp = t.ReadToEnd();
                    t.Close();

                    // Speciale elementen vervangen
                    rdp = Regex.Replace(rdp, "%username%", HttpContext.Current.User.Identity.Name, RegexOptions.IgnoreCase);
                    rdp = Regex.Replace(rdp, "%hostname%", host, RegexOptions.IgnoreCase);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + info + ".rdp\"");
                    Response.Charset = "utf-8";
                    Response.ContentType = "application/rdp";
                    Response.Output.Write(rdp);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception err)
            {
                ShowError(err.Message);
            }
        }
        protected void odsComputers_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            UpdateRefreshTime();
        }
    }
}