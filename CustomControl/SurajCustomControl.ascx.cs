using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class SurajTextbox : System.Web.UI.UserControl
    {
        public String getText()
        {
            return TextBoxName.Text;
        }
        public void setLabel(String label)
        {
            LabelName.Text = label;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonUpper_Click(object sender, EventArgs e)
        {
            TextBoxName.Text = TextBoxName.Text.ToUpper();
        }

        protected void ButtonLower_Click(object sender, EventArgs e)
        {
            TextBoxName.Text = TextBoxName.Text.ToLower();
        }

        protected void TextBoxname_TextChanged(object sender, EventArgs e)
        {

        }
    }
}