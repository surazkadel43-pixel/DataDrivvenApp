using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Classes;

namespace WebApplication1.CustomControl
{
    public partial class RadioButtonListUserControl : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the text of the RadioButtonList question.
        /// </summary>
        public string RadioQuestionText
        {
            get { return radioQuestionLabel.Text; }
            set { radioQuestionLabel.Text = value; }
        }

        /// <summary>
        /// Gets the selected index from the RadioButtonList.
        /// Throws an exception if no option is selected.
        /// </summary>
        /// <exception cref="QuestionValidationException">
        /// Thrown when no option is selected in the RadioButtonList.
        /// </exception>
        public int SelectedIndex
        {
            get
            {
                int selectedIndex = radioButtonListUC.SelectedIndex;

                if (selectedIndex >= 0)
                {
                    return selectedIndex;  // Return the selected index
                }
                else
                {
                    // If no item is selected, throw an exception
                    throw new QuestionValidationException("No option selected in RadioButtonList");
                }
            }
            set
            {
                if (value >= 0 && value < radioButtonListUC.Items.Count)
                {
                    radioButtonListUC.SelectedIndex = value; // Set the selected index
                }
                else
                {
                    //radioButtonListUC.SelectedIndex = value;
                    // If the index is out of range, throw an exception
                    throw new QuestionValidationException( "Index is out of range for the RadioButtonList items.");
                }
            }
        }

        /// <summary>
        /// Gets the selected value from the RadioButtonList.
        /// Throws an exception if no option is selected.
        /// </summary>
        /// <exception cref="QuestionValidationException">
        /// Thrown when no option is selected in the RadioButtonList.
        /// </exception>
        public string SelectedValues
        {
            get
            {
                string selectedValue;
                if (!string.IsNullOrEmpty(radioButtonListUC.SelectedValue))
                {
                    selectedValue = radioButtonListUC.SelectedValue;
                }
                else
                {
                    // Handle the case where no option is selected
                    throw new QuestionValidationException("No option selected in RadioButtonList");
                }

                return selectedValue;
            }
        }
        /// <summary>
        /// Property to clear the selection of the RadioButtonList.
        /// </summary>
        public bool ClearSelection
        {
            set
            {
                if (value)
                {
                    radioButtonListUC.SelectedIndex = -1; // Unselect any selected radio button
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        // Property for selected values as a list

        /// <summary>
        /// Adds a new ListItem to the RadioButtonList. 
        /// Throws an exception if the ListItem is null or if the RadioButtonList is not initialized.
        /// </summary>
        /// <param name="listItem">The ListItem to be added to the RadioButtonList.</param>
        /// <exception cref="QuestionValidationException">
        /// Thrown when the ListItem is null or if the RadioButtonList control is not initialized.
        /// </exception>
        public void addItem(ListItem listItem) {

            if (listItem == null)
            {
               throw new QuestionValidationException(" Question List is null in RadioButtonLisat");
            }
            else
            {
                radioButtonListUC.Items.Add(listItem);
            }
        }
       
    }
}