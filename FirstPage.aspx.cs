using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class RegisterPage1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void staffLogin_Click(object sender, EventArgs e)
        {
            Session["StaffLogin"] = true;
            Response.Redirect("~/StaffLogin.aspx");
        }

        protected void researchSession_Click(object sender, EventArgs e)
        {
            Session["Surveying"] = true;
            Response.Redirect("~/SurveyQuestion.aspx");
        }
    }
}