using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Classes;

namespace WebApplication1.CustomControl
{
    public partial class TextboxUserControl : System.Web.UI.UserControl
    {
        #region Properties

        // Expose the ID of the inner TextBox control
        public string TextBoxID
        {
            get { return textBoxUC.ID; } // Replace `txtBox` with the actual ID of your TextBox
        }

        public TextBox InnerTextBox
        {
            get { return textBoxUC; } // txtBox is the ID of the TextBox control in the user control
        }
        /// <summary>
        /// Gets or sets the text of the TextBox question.
        /// </summary>
        public string TextBoxQuestionText
        {
            get { return questionText.Text; }
            set
            {
                questionText.Text = value;

                // Check if the question text is "Email" and add validation dynamically
                if (value.Equals("Email Address", StringComparison.OrdinalIgnoreCase))
                {
                    AddEmailValidator();
                }
                // Check if the question text is "Post Code" and add validation dynamically
                else if (value.Equals("Postcode", StringComparison.OrdinalIgnoreCase))
                {
                    AddPostCodeValidator();
                }
                else if (value.Equals("Home Suburb", StringComparison.OrdinalIgnoreCase))
                {
                    AddSuburbValidator();
                }
                else if (value.Equals("Respondent ID", StringComparison.OrdinalIgnoreCase))
                {
                    AddRespondentIdValidator();
                }

            }
        }

        /// <summary>
        /// Gets the value entered in the TextBox.
        /// Throws an exception if the TextBox is empty.
        /// </summary>
        /// <exception cref="QuestionValidationException">
        /// Thrown when no value is provided in the TextBox.
        /// </exception>
        public string SelectedValue
        {
            get
            {
                string value;

                // Get the value from the TextBox
                string textBoxValue = textBoxUC.Text;

                if (!string.IsNullOrEmpty(textBoxValue))
                {
                    value = textBoxUC.Text;
                }
                else
                {
                    // Throw an exception if the TextBox is empty
                    throw new QuestionValidationException("No answer provided in TextBox");
                }


                return value;
            }
            set
            {
                // Allow the assignment of a value to the TextBox
                if (value != null)
                {
                    textBoxUC.Text = value;
                }
                else
                {
                    // Optionally, you can handle the case where the value is null (e.g., set the TextBox to empty)
                    textBoxUC.Text = string.Empty;  // Or throw an exception if needed
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure the validator is added during the page lifecycle if necessary
            if (TextBoxQuestionText != null)
            {
                if (TextBoxQuestionText.Equals("Email", StringComparison.OrdinalIgnoreCase))
                {
                    AddEmailValidator();
                }
                else if (TextBoxQuestionText.Equals("Post Code", StringComparison.OrdinalIgnoreCase))
                {
                    AddPostCodeValidator();
                }
                else if (TextBoxQuestionText.Equals("Suburb", StringComparison.OrdinalIgnoreCase))
                {
                    AddSuburbValidator();
                }
                


            }
        }
        /// <summary>
        /// Adds an email validation regex validator to the TextBox.
        /// </summary>
        private void AddEmailValidator()
        {
            RegularExpressionValidator emailValidator = new RegularExpressionValidator
            {
                ID = "emailValidator",
                ControlToValidate = textBoxUC.ID, // Ensure this ID matches the TextBox control
                ValidationExpression = @"^[^@\s]+@[^@\s]+\.[^@\s]+$", // Standard email regex
                ErrorMessage = "Please enter a valid email address.",
                CssClass = "text-danger", // Optional: Add styling class
                Display = ValidatorDisplay.Dynamic,
                EnableClientScript = true // Allow client-side validation
            };

            // Add the validator dynamically to the control's Controls collection
            this.Controls.Add(emailValidator);
        }

        /// <summary>
        /// Adds a post code validation regex validator to the TextBox.
        /// </summary>
        private void AddPostCodeValidator()
        {
            RegularExpressionValidator postCodeValidator = new RegularExpressionValidator
            {
                ID = "postCodeValidator",
                ControlToValidate = textBoxUC.ID, // Ensure this ID matches the TextBox control
                ValidationExpression = @"^\d{4}$", // Standard alphanumeric and space regex for postcodes
                ErrorMessage = "Please enter a valid 4-digit postcode.",
                CssClass = "text-danger", // Optional: Add styling class
                Display = ValidatorDisplay.Dynamic,
                EnableClientScript = true // Allow client-side validation
            };

            // Add the validator dynamically to the control's Controls collection
            this.Controls.Add(postCodeValidator);
        }
        /// <summary>
        /// Adds a suburb validation regex validator to the TextBox.
        /// </summary>
        private void AddSuburbValidator()
        {
            RegularExpressionValidator suburbValidator = new RegularExpressionValidator
            {
                ID = "suburbValidator",
                ControlToValidate = textBoxUC.ID, // Ensure this ID matches the TextBox control
                ValidationExpression = @"^[A-Za-z]+$", // Only alphabetic characters
                ErrorMessage = "Please enter a valid suburb (letters only).",
                CssClass = "text-danger", // Optional: Add styling class
                Display = ValidatorDisplay.Dynamic,
                EnableClientScript = true // Allow client-side validation
            };

            this.Controls.Add(suburbValidator);
        }

        private void AddRespondentIdValidator()
        {
            // RequiredFieldValidator to ensure the field is not empty
            RequiredFieldValidator reqValidator = new RequiredFieldValidator
            {
                ID = "reqValidator_RespondentID",
                ControlToValidate = textBoxUC.ID, // Assuming txtBox is the TextBox ID in the control
                ErrorMessage = "Respondent ID is required.",
                CssClass = "text-danger",
                Display = ValidatorDisplay.Dynamic
            };
            this.Controls.Add(reqValidator);

            // RegularExpressionValidator to ensure only numeric input
            RegularExpressionValidator regexValidator = new RegularExpressionValidator
            {
                ID = "regexValidator_RespondentID",
                ControlToValidate = textBoxUC.ID, // Assuming txtBox is the TextBox ID in the control
                ValidationExpression = @"^\d+$", // Only numbers
                ErrorMessage = "Respondent ID must be numeric.",
                CssClass = "text-danger",
                Display = ValidatorDisplay.Dynamic
            };
            this.Controls.Add(regexValidator);
        }

    }
}