using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Classes;

namespace WebApplication1.CustomControl
{
    public partial class DropDownListUserControl : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the text of the DropDownList question.
        /// </summary>
        public string DropDownQuestionText
        {
            get { return dropDownQuestionName.Text; }
            set { dropDownQuestionName.Text = value; }
        }

        /// <summary>
        /// Gets the selected value from the DropDownList.
        /// Throws an exception if no option is selected or if the selected value is empty.
        /// </summary>
        /// <exception cref="QuestionValidationException">
        /// Thrown when no option is selected or the selected value is empty.
        /// </exception>
        public string SelectedValues
        {
            get
            {
                string selectedValue;

                // Check if an option is selected in the DropDownList
                if (dropDownListUC.SelectedIndex >= 0 && !string.IsNullOrEmpty(dropDownListUC.SelectedValue))
                {
                    selectedValue = dropDownListUC.SelectedValue;
                }
                else
                {
                    // Throw an exception if no option is selected or if the selected value is empty
                    throw new QuestionValidationException("No option selected in the dropdownlist");
                }

                return selectedValue;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && dropDownListUC.Items.FindByValue(value) != null)
                {
                    dropDownListUC.SelectedValue = value; // Set the selected value
                }
                else
                {
                    // Throw an exception if the value is invalid or not in the dropdown list
                    throw new ArgumentException("The specified value is not valid or does not exist in the dropdownlist.", nameof(value));
                }
            }
        }

        /// <summary>
        /// Gets the selected index from the DropDownList.
        /// Throws an exception if no option is selected.
        /// </summary>
        /// <exception cref="QuestionValidationException">
        /// Thrown when no option is selected.
        /// </exception>
        public int SelectedIndex
        {
            get
            {
                int selectedIndex = dropDownListUC.SelectedIndex;

                if (selectedIndex >= 0)
                {
                    return selectedIndex;  // Return the selected index
                }
                else
                {
                    // If no item is selected, throw an exception
                    throw new QuestionValidationException("No option selected in DropDownList");
                }
            }
            set
            {
                if (value >= 0 && value < dropDownListUC.Items.Count)
                {
                    dropDownListUC.SelectedIndex = value; // Set the selected index
                }
                else
                {
                    // If the index is out of range, throw an exception
                    throw new ArgumentOutOfRangeException(nameof(value), "Index is out of range for the DropDownList items.");
                }
            }
        }
        /// <summary>
        /// Property to clear the selection of the DropDownList.
        /// </summary>
        public bool ClearSelection
        {
            set
            {
                if (value)
                {
                    // Set the selected index to -1 to clear the selection (or set it to the default index)
                    dropDownListUC.SelectedIndex = -1;
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Adds a new ListItem to the DropDownList. 
        /// Ensures that the ListItem has valid Text and Value properties before adding it.
        /// Throws an exception if the ListItem is null, has empty Text or Value, or if the DropDownList is not initialized.
        /// </summary>
        /// <param name="listItem">The ListItem to be added to the DropDownList.</param>
        /// <exception cref="QuestionValidationException">
        /// Thrown when the ListItem is null, or has empty Text or Value, or if the DropDownList is not initialized.
        /// </exception>
        public void addItem(ListItem listItem)
        {
            // Ensure that the ListItem has valid Text and Value
            if (listItem == null || string.IsNullOrEmpty(listItem.Text) || string.IsNullOrEmpty(listItem.Value))
            {
                throw new QuestionValidationException("ListItem Text or Value cannot be null or empty.");
            }
            else
            {
                if (dropDownListUC != null)
                {
                    dropDownListUC.Items.Add(listItem); // Adding item to DropDownList
                }
                else
                {
                    throw new QuestionValidationException("DropDownList control is not initialized.");
                }
            }
        }
    }
}