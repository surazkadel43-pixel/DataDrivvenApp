using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class MemberLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Surveying"] == null)
            {
                Response.Redirect("~/Firstpage.aspx");
            }
        }

        protected void dateOfBirthPicker_SelectionChanged(object sender, EventArgs e)
        {
            //txtdateOfBirth.Text= dateOfBirthPicker.SelectedDate.ToLongDateString(); // ToLongDateString() is a Type format of(Tuesday, July 12, 2016)
            txtdateOfBirth.Text = dateOfBirthPicker.SelectedDate.ToShortDateString(); // ToLongDateString() is a Type format of(12-07-2016)
            dateOfBirthPicker.Visible = false;

        }

        protected void linkButtondateOfBirth_Click(object sender, EventArgs e)
        {
            dateOfBirthPicker.Visible = true;
        }

        protected void buttonLogin_Click(object sender, EventArgs e)
        {
           
            
        }
    }
}