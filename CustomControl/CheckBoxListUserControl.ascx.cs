using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Classes;

namespace WebApplication1.CustomControl
{
    public partial class CheckBoxListUserControl : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the text of the CheckBoxQuestion.
        /// </summary>
        public string CheckBoxQuestionText
        {
            get { return questionValueLabel.Text; }
            set { questionValueLabel.Text = value; }
        }

        /// <summary>
        /// Gets the list of selected values from the CheckBoxList.
        /// Throws an exception if no option is selected.
        /// </summary>
        /// <exception cref="QuestionValidationException">Thrown when no option is selected from the CheckBoxList.</exception>
        public List<string> SelectedValues
        {
            get
            {
                List<string> selectedValues = new List<string>();

                // Check each item in the CheckBoxList
                bool anySelected = false;
                foreach (ListItem item in optionsCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        selectedValues.Add(item.Value); // Add the selected value to the list
                        anySelected = true;
                    }
                }

                // If no option is selected, throw an exception
                if (!anySelected)
                {
                    throw new QuestionValidationException("No option selected from checkBoxlist");
                }

                return selectedValues;
            }
        }

        /// <summary>
        /// Indexer to get the value of a specific item in the CheckBoxList by its position.
        /// Throws an exception if the position is out of range.
        /// </summary>
        /// <param name="position">The position of the item to retrieve.</param>
        /// <returns>The value of the item at the specified position.</returns>
        /// <exception cref="QuestionValidationException">Thrown when the position is out of range for the CheckBoxList items.</exception>
        public string this[int position]
        {
            get
            {
                // Check if the position is valid
                if (position < 0 || position >= optionsCheckBoxList.Items.Count)
                {
                    throw new QuestionValidationException("Position is out of range for the CheckBoxList items.");
                }

                // Return the value at the specified position
                return optionsCheckBoxList.Items[position].Value;
            }

            set
            {
                // Check if the position is valid
                if (position < 0 || position >= optionsCheckBoxList.Items.Count)
                {
                    throw new QuestionValidationException("Position is out of range for the CheckBoxList items.");
                }

                // Set the value at the specified position
                optionsCheckBoxList.Items[position].Value = value;
            }
        }
        

        /// <summary>
        /// Gets the list of selected indices from the CheckBoxList.
        /// </summary>
        /// <returns>A list of selected indices from the CheckBoxList.</returns>
        public List<int> SelectedIndices
        {
            get
            {
                List<int> selectedIndices = new List<int>();

                // Loop through each item in the CheckBoxList
                for (int i = 0; i < optionsCheckBoxList.Items.Count; i++)
                {
                    if (optionsCheckBoxList.Items[i].Selected)
                    {
                        selectedIndices.Add(i); // Add the index of the selected item
                    }
                }

                return selectedIndices;
            }
            set
            {
                // Clear all current selections
                foreach (ListItem item in optionsCheckBoxList.Items)
                {
                    item.Selected = false; // Unselect all items
                }

                // Loop through the indices and select the corresponding items
                foreach (int index in value)
                {
                    if (index >= 0 && index < optionsCheckBoxList.Items.Count)
                    {
                        optionsCheckBoxList.Items[index].Selected = true; // Select the item at the given index
                    }
                    else
                    {
                        // If the index is out of range, throw an exception
                        throw new QuestionValidationException( $"Index {index} is out of range for the CheckBoxList items.");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the index of the first selected item in the CheckBoxList.
        /// Throws an exception if no item is selected.
        /// </summary>
        /// <returns>The index of the first selected item.</returns>
        /// <exception cref="QuestionValidationException">Thrown when no option is selected in the CheckBoxList.</exception>
        public int SelectedIndex
        {
            get
            {
                int selectedIndex = -1;
                // Check if any item is selected
                for (int i = 0; i < optionsCheckBoxList.Items.Count; i++)
                {
                    if (optionsCheckBoxList.Items[i].Selected)
                    {
                        selectedIndex = i;  // Store the index of the selected item
                        break;  // Exit the loop after finding the first selected item
                    }
                }

                if (selectedIndex >= 0)
                {
                    return selectedIndex;  // Return the first selected index
                }
                else
                {
                    // If no item is selected, throw an exception
                    throw new QuestionValidationException("No option selected in CheckBoxList");
                }
            }
        }

        /// <summary>
        /// Property to clear the selection of the CheckBoxList.
        /// </summary>
        public bool ClearSelection
        {
            set
            {
                if (value)
                {
                    // Unselect all checkboxes
                    foreach (ListItem item in optionsCheckBoxList.Items)
                    {
                        item.Selected = false;
                    }
                }
            }
        }

        #endregion




        protected void Page_Load(object sender, EventArgs e)
        {

        }
        // Inside DropDownListUserControl.ascx.cs


        /// <summary>
        /// Adds a new ListItem to the options CheckBoxList.
        /// Throws an exception if the provided ListItem is null.
        /// </summary>
        /// <param name="listItem">The ListItem to be added to the CheckBoxList.</param>
        /// <exception cref="QuestionValidationException">
        /// Thrown when the provided ListItem is null.
        /// </exception>
        public void addItem(ListItem listItem)
        {

            if (listItem == null)
            {
                throw new QuestionValidationException(" Question List is null");
            }
            else
            {
                optionsCheckBoxList.Items.Add(listItem);
            }
        }
    }
}