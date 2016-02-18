using System;
using System.Web.UI;

namespace Groceries_Application
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_signIn_OnServerClick(object sender, EventArgs e)
        {
            if (username.Value.ToLower() == "admin" && password.Value.ToLower() == "admin")
            {
                Response.Redirect("Views/MainPage.aspx");
            }
            else
            {
                Label1.Text = "Please check username and password";
            }
        }
    }
}