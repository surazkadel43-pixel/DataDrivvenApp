using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Classes;
using WebApplication1.CustomControl;
using System.Web.Configuration;
using System.Collections;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Net;

namespace WebApplication1
{
    public partial class SurveyQuestion : System.Web.UI.Page
    {
        private string surajConnectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
        private int questionOrder;
        private int relatedQuestionId;
        private int lastQuestionOrder;
        private int totalQuestion;
        RealQuestion realQuestion = new RealQuestion();
        List<Answer> answers = new List<Answer>();
        QuestionDisplayesd questionDisplayesd = new QuestionDisplayesd();
        List<QuestionDisplayesd> questionDisplayedList = new List<QuestionDisplayesd>();
        #region Properties
        public int QuestionOrder
        {
            get
            {
                return this.questionOrder;
            }
            set
            {
                this.questionOrder = value;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)  // Check if this is the first time the page is being loaded
            {
                if(Session["Surveying"] == null)
                {
                    Response.Redirect("~/Firstpage.aspx");
                }
                // Check if "QuestionDisplayedList" is null in the session
                if (Session["QuestionDisplayedList"] == null)
                {
                    // Initialize a new QuestionDisplayesd object if the list is null
                    QuestionDisplayesd questionDisplayesd = new QuestionDisplayesd();
                    questionDisplayesd.QuestionOrderDatabase = 1;  // Set the database question order
                    questionDisplayesd.QuestionOrderDisplayed = 1; // Set the displayed order of the question
                    questionDisplayesd.QuestionOrder = 1;
                    questionDisplayesd.QuestionId = 0;            // Initialize QuestionId to 0
                    //questionDisplayesd.TotalNumberOFQuestionInDatabase = GetTotalQuestionsWithNonZeroOrder();
                    this.questionDisplayedList.Add(questionDisplayesd); // Add the questionDisplayesd to the list
                    Session["QuestionDisplayedList"] = this.questionDisplayedList; // Store it in the session

                    
                }

                totalQuestion = GetTotalQuestionsWithNonZeroOrder();


                if (Session["LastQuestionOrder"] == null)
                {
                    Session["LastQuestionOrder"] = 0; // Set empty error message if none exists
                    lastQuestionOrder = (int)Session["LastQuestionOrder"];

                }
                else
                {
                     lastQuestionOrder = (int)Session["LastQuestionOrder"] ;

                }
                // Handle the error message session
                if (Session["ErrorMessage"] == null)
                {
                    Session["ErrorMessage"] = ""; // Set empty error message if none exists
                   
                }
                else
                {
                    lblErrorMessage.Text = (string)Session["ErrorMessage"]; // Display the error message if exists
                }
                // Initialize the session for answers if not already done
                if (Session["Answers"] == null)
                {
                    Session["Answers"] = new List<Answer>(); // Create a new list if not available

                   
                }
                else
                {
                    answers = Session["Answers"] as List<Answer>; // Retrieve the existing answers from session
                    int anwerCount = answers.Count; // Count the number of answers
                }
                if (Session["ButtonPrevious"] == null)
                {
                    Session["ButtonPrevious"] = false; // Create a new list if not available

                   
                }
                // Retrieve the QuestionDisplayedList from session
                this.questionDisplayedList = Session["QuestionDisplayedList"] as List<QuestionDisplayesd>;
                
                // Get the last item in the list for the current question display
                this.questionDisplayesd = this.questionDisplayedList[lastQuestionOrder];
                
                // Set the question order and related question ID from the last item
                questionOrder = this.questionDisplayesd.QuestionOrderDisplayed;
                relatedQuestionId = this.questionDisplayesd.RelatedQuestion;
               
                // Load the question and options (currently using hardcoded values)
                realQuestion = Load_Question_with_Option(questionOrder, relatedQuestionId);
                if(realQuestion == null  && questionOrder <= totalQuestion)
                {

                }
                Display_Question_with_Option(realQuestion, answers); // Display the loaded question and options

                // Handle the visibility of the "Previous" button
                if (questionOrder == 1)
                {
                    btnPrevious.Visible = false;
                    Session["ButtonPrevious"] = false;// Hide the "Previous" button when on the first question
                }
                else
                {
                    btnPrevious.Visible = true;
                }


                

                
            }
            else
            {
                // Code runs if the page has already been posted back (i.e., after form submission or redirection)

                // Retrieve the QuestionDisplayedList from session
                this.questionDisplayedList = Session["QuestionDisplayedList"] as List<QuestionDisplayesd>;
                lastQuestionOrder = (int)Session["LastQuestionOrder"];
                // Get the last item in the list for the current question display
                //this.questionDisplayesd = this.questionDisplayedList.Last();
                this.questionDisplayesd = this.questionDisplayedList[lastQuestionOrder];

                // Set the question order and related question ID from the last item
                questionOrder = this.questionDisplayesd.QuestionOrderDisplayed;
                if (questionOrder == 1)
                {
                    Session["ButtonPrevious"] = false; // Hide the "Previous" button when on the first question
                }
                relatedQuestionId = this.questionDisplayesd.RelatedQuestion;
                answers = Session["Answers"] as List<Answer>;
                // Load the question and options (currently using hardcoded values)
                realQuestion = Load_Question_with_Option(questionOrder, relatedQuestionId);
                if(realQuestion == null)
                {

                }
                Display_Question_with_Option(realQuestion, answers ); // Display the loaded question and options

                
            }



        }

        /// <summary>
        /// Dynamically displays a question along with its associated options based on the question type.
        /// The method handles various question types such as RadioButtonList, DropDownList, CheckBoxList, and TextBox.
        /// It also handles disabled questions and ensures that appropriate controls are displayed based on the question type.
        /// </summary>
        /// <param name="realQuestion">The question object containing the question text, options, and question type.</param>
        /// <exception cref="QuestionValidationException">Thrown when a question is disabled or an invalid option is found.</exception>
        /// <exception cref="System.Exception">Thrown for any internal errors that occur during the question display process.</exception>
        protected void Display_Question_with_Option(RealQuestion realQuestion, List<Answer> savedAnswers)
        {
            // Clear the current contents of the placeholder
            QuestionList.Controls.Clear(); // This clears all existing controls inside the placeholder
            bool buttonPrevious =(bool) Session["ButtonPrevious"] ;
            int targetQuestionId = 0;
            List<Answer> answerList = new List<Answer>();
            if (buttonPrevious) {
                 answerList = new List<Answer>();
                var lastAnswer = savedAnswers[savedAnswers.Count - 1];
                 targetQuestionId = lastAnswer.QuestionId;

                // Step 3: Remove all answers from savedAnswers that have the same QuestionId as the last answer
                for (int i = savedAnswers.Count - 1; i >= 0; i--)
                {
                    // Check if the answer belongs to the same question
                    if (savedAnswers[i].QuestionId == targetQuestionId)
                    {
                        answerList.Add(savedAnswers[i]);  // Remove the answer with the matching QuestionId
                    }
                    else
                    {
                        break;
                    }
                }
            }
            

            if (buttonPrevious && realQuestion.QuestionId == targetQuestionId)
            {
                try
                {
                    // Create a Label for the question text
                    Label questionLabel = new Label();
                    questionLabel.Text = realQuestion.QuestionText;

                    // Check if the question is disabled if disable is = 0 means, question can be display in the screen  and if the diabale =1 question cannot be displayed
                    if (realQuestion.QuestionDisable == 1)
                    {
                        questionLabel.ForeColor = System.Drawing.Color.Gray; // Optional: Gray out the text if disabled
                        questionLabel.Text += " (This question is disabled)"; // Optional: Add message indicating disabled
                        lbltitle.Text = questionLabel.Text;
                        throw new QuestionValidationException("This question is disabled");
                    }



                    // Check the type of the question (RadioButtonList, DropDownList, CheckBoxList)
                    switch (realQuestion.QuestionType)
                    {
                        case "RadioButtonList":


                            // Load the control and cast it directly
                            RadioButtonListUserControl radioButtonListUserControl = (RadioButtonListUserControl)this.LoadControl("~/CustomControl/RadioButtonListUserControl.ascx");
                            radioButtonListUserControl.ID = "radioUC";
                            QuestionList.Controls.Add(radioButtonListUserControl); // Add RadioButtonList to PlaceHolder
                            radioButtonListUserControl.RadioQuestionText = realQuestion.QuestionText;
                            foreach (string option in realQuestion.OptionText)
                            {

                                ListItem listItem = new ListItem
                                {
                                    Text = option,
                                    Value = option
                                };
                                listItem.Attributes.Add("class", "list-group-item list-group-item-action text-danger fw-bolder "); // Bootstrap list item styling
                                radioButtonListUserControl.addItem(listItem);
                            }

                            foreach (Answer answer in answerList)
                            {
                                if (answer.OptionOrder != null && answer.QuestionId == targetQuestionId)
                                {
                                    radioButtonListUserControl.SelectedIndex = answer.OptionOrder.Value;
                                }

                            }

                            break;

                        case "DropDownList":


                            DropDownListUserControl dropDownListUserControl = (DropDownListUserControl)this.LoadControl("~/CustomControl/DropDownListUserControl.ascx");
                            dropDownListUserControl.ID = "dropDownUC";
                            QuestionList.Controls.Add(dropDownListUserControl);
                            dropDownListUserControl.DropDownQuestionText = realQuestion.QuestionText;

                            // Optional: Add a default option to the dropdown with a non-empty value
                            ListItem defaultItem = new ListItem("Select an option", "0");
                            dropDownListUserControl.addItem(defaultItem);

                            foreach (string option in realQuestion.OptionText)
                            {
                                // Check if the option is valid (not empty)
                                if (!string.IsNullOrWhiteSpace(option))
                                {
                                    ListItem listItem = new ListItem
                                    {
                                        Text = option,
                                        Value = option // Ensure Value is not an empty string
                                    };

                                    listItem.Attributes.Add("class", "list-group-item list-group-item-action text-danger fw-bolder ");

                                    // Add the item to the DropDownList
                                    dropDownListUserControl.addItem(listItem);
                                }
                                else
                                {
                                    throw new QuestionValidationException("The option is Empty");
                                }
                            }
                            foreach (Answer answer in answerList)
                            {
                                if (answer.OptionOrder != null && answer.QuestionId == targetQuestionId)
                                {
                                    dropDownListUserControl.SelectedIndex = answer.OptionOrder.Value;
                                }

                            }
                            break;

                        case "CheckBoxList":


                            // Load and add the CheckBoxListUserControl dynamically
                            CheckBoxListUserControl checkBoxListUserControl =
                                (CheckBoxListUserControl)this.LoadControl("~/CustomControl/CheckBoxListUserControl.ascx");

                            checkBoxListUserControl.ID = "checkBoxUC";
                            QuestionList.Controls.Add(checkBoxListUserControl);
                            checkBoxListUserControl.CheckBoxQuestionText = realQuestion.QuestionText;

                            foreach (string option in realQuestion.OptionText)
                            {
                                ListItem listItem = new ListItem
                                {
                                    Text = option,
                                    Value = option
                                };
                                listItem.Attributes.Add("class", "list-group-item list-group-item-action text-danger fw-bolder "); // Bootstrap list item styling
                                checkBoxListUserControl.addItem(listItem);
                            }
                            QuestionList.Controls.Add(checkBoxListUserControl); // Add CheckBoxList to PlaceHolder
                            List<int> indices = new List<int>();
                            foreach (Answer answer in answerList)
                            {
                                if (answer.OptionOrder != null && answer.QuestionId == targetQuestionId)
                                    indices.Add(answer.OptionOrder.Value);

                            }
                            if (indices.Count > 0)
                            {
                                checkBoxListUserControl.SelectedIndices = indices;
                            }

                            break;

                        case "TextBox":
                            // Load and add the TextBoxUserControl dynamically
                            TextboxUserControl textBoxUserControl =
                                (TextboxUserControl)this.LoadControl("~/CustomControl/TextBoxUserControl.ascx");

                            textBoxUserControl.ID = "textBoxUC";
                            QuestionList.Controls.Add(textBoxUserControl);
                            textBoxUserControl.TextBoxQuestionText = realQuestion.QuestionText;  // Set the question text for the TextBox



                            QuestionList.Controls.Add(textBoxUserControl); // Add TextBox to PlaceHolder

                            foreach (Answer answer in answerList)
                            {
                                if (answer.OptionOrder == null && answer.QuestionId == targetQuestionId)
                                {
                                    textBoxUserControl.SelectedValue = answer.AnswerText;
                                }

                            }
                            break;

                        default:

                           
                            break;
                    }
                }
                catch (QuestionValidationException ex)
                {
                    lblErrorMessage.Text = ex.Message;
                    Session["ErrorMessage"] = lblErrorMessage.Text;
                }
                catch (System.Exception ex)
                {
                    lblErrorMessage.Text = "Internal error, please contact the system admin. Error details: " + ex.Message;
                }
                deleteLastAnswer(savedAnswers);
            }
            else
            {
                try
                {
                    // Create a Label for the question text
                    Label questionLabel = new Label();
                    questionLabel.Text = realQuestion.QuestionText;

                    // Check if the question is disabled if disable is = 0 means, question can be display in the screen  and if the diabale =1 question cannot be displayed
                    if (realQuestion.QuestionDisable == 1)
                    {
                        questionLabel.ForeColor = System.Drawing.Color.Gray; // Optional: Gray out the text if disabled
                        questionLabel.Text += " (This question is disabled)"; // Optional: Add message indicating disabled
                        lbltitle.Text = questionLabel.Text;
                        throw new QuestionValidationException("This question is disabled");
                    }



                    // Check the type of the question (RadioButtonList, DropDownList, CheckBoxList)
                    switch (realQuestion.QuestionType)
                    {
                        case "RadioButtonList":


                            // Load the control and cast it directly
                            RadioButtonListUserControl radioButtonListUserControl = (RadioButtonListUserControl)this.LoadControl("~/CustomControl/RadioButtonListUserControl.ascx");
                            radioButtonListUserControl.ID = "radioUC";
                            QuestionList.Controls.Add(radioButtonListUserControl); // Add RadioButtonList to PlaceHolder
                            radioButtonListUserControl.RadioQuestionText = realQuestion.QuestionText;
                            foreach (string option in realQuestion.OptionText)
                            {
                                ListItem listItem = new ListItem
                                {
                                    Text = option,
                                    Value = option
                                };
                                listItem.Attributes.Add("class", "list-group-item list-group-item-action text-danger fw-bolder "); // Bootstrap list item styling
                                radioButtonListUserControl.addItem(listItem);
                            }



                            break;

                        case "DropDownList":


                            DropDownListUserControl dropDownListUserControl = (DropDownListUserControl)this.LoadControl("~/CustomControl/DropDownListUserControl.ascx");
                            dropDownListUserControl.ID = "dropDownUC";
                            QuestionList.Controls.Add(dropDownListUserControl);
                            dropDownListUserControl.DropDownQuestionText = realQuestion.QuestionText;

                            // Optional: Add a default option to the dropdown with a non-empty value
                            ListItem defaultItem = new ListItem("Select an option", "0");
                            dropDownListUserControl.addItem(defaultItem);

                            foreach (string option in realQuestion.OptionText)
                            {
                                // Check if the option is valid (not empty)
                                if (!string.IsNullOrWhiteSpace(option))
                                {
                                    ListItem listItem = new ListItem
                                    {
                                        Text = option,
                                        Value = option // Ensure Value is not an empty string
                                    };

                                    listItem.Attributes.Add("class", "list-group-item list-group-item-action text-danger fw-bolder ");

                                    // Add the item to the DropDownList
                                    dropDownListUserControl.addItem(listItem);
                                }
                                else
                                {
                                    throw new QuestionValidationException("The option is Empty");
                                }
                            }
                            // Add DropDownList to PlaceHolder
                            break;

                        case "CheckBoxList":


                            // Load and add the CheckBoxListUserControl dynamically
                            CheckBoxListUserControl checkBoxListUserControl =
                                (CheckBoxListUserControl)this.LoadControl("~/CustomControl/CheckBoxListUserControl.ascx");

                            checkBoxListUserControl.ID = "checkBoxUC";
                            QuestionList.Controls.Add(checkBoxListUserControl);
                            checkBoxListUserControl.CheckBoxQuestionText = realQuestion.QuestionText;

                            foreach (string option in realQuestion.OptionText)
                            {
                                ListItem listItem = new ListItem
                                {
                                    Text = option,
                                    Value = option
                                };
                                listItem.Attributes.Add("class", "list-group-item list-group-item-action text-danger fw-bolder "); // Bootstrap list item styling
                                checkBoxListUserControl.addItem(listItem);
                            }
                            QuestionList.Controls.Add(checkBoxListUserControl); // Add CheckBoxList to PlaceHolder
                            break;

                        case "TextBox":
                            // Load and add the TextBoxUserControl dynamically
                            TextboxUserControl textBoxUserControl =
                                (TextboxUserControl)this.LoadControl("~/CustomControl/TextBoxUserControl.ascx");

                            textBoxUserControl.ID = "textBoxUC";
                            QuestionList.Controls.Add(textBoxUserControl);
                            textBoxUserControl.TextBoxQuestionText = realQuestion.QuestionText;  // Set the question text for the TextBox



                            QuestionList.Controls.Add(textBoxUserControl); // Add TextBox to PlaceHolder
                            break;

                        default:


                            btnNext.Visible = false;
                            btnPrevious.Visible = false;
                            btnSubmitAnonymously.Visible = true;
                            btnRegisterAndSubmit.Visible = true;
                            Session["ErrorMessage"] = "";
                            // Add a styled message to the QuestionList placeholder for unsupported control type
                            var message = "<div class='alert alert-info text-center h2' role='alert'>Thank you for taking part in the research.</div>";
                            QuestionList.Controls.Add(new LiteralControl(message));  // Add the styled message to the placeholder
                            break;
                    }
                }
                catch (QuestionValidationException ex)
                {
                    lblErrorMessage.Text = ex.Message;
                    Session["ErrorMessage"] = lblErrorMessage.Text;
                }
                catch (System.Exception ex)
                {
                    lblErrorMessage.Text = "Internal error, please contact the system admin. Error details: " + ex.Message;
                }
            }

        }

        /// <summary>
        /// Loads a question and its associated options from the database based on the provided question order and related question ID.
        /// If the <paramref name="questionOrder"/> is greater than 0 and <paramref name="relatedQuestionId"/> is 0, 
        /// it loads a question based on the order. If <paramref name="relatedQuestionId"/> is greater than 0, it loads a related question.
        /// </summary>
        /// <param name="questionOrder">
        /// The order of the question to be loaded. If this is greater than 0, the method loads the question based on this order.
        /// </param>
        /// <param name="relatedQuestionId">
        /// The ID of a related question. If this is greater than 0, the method loads the related question based on this ID.
        /// </param>
        /// <returns>
        /// A <see cref="RealQuestion"/> object containing the question details and its associated options.
        /// </returns>
        /// <exception cref="QuestionValidationException">
        /// Thrown when there is a validation issue specific to the question loading process.
        /// </exception>
        /// <exception cref="System.Exception">
        /// Thrown for any other general errors that occur during database connection or data retrieval.
        /// </exception>
        protected RealQuestion Load_Question_with_Option(int questionOrder, int relatedQuestionId)
        {
            RealQuestion realQuestion = new RealQuestion();
            if(questionOrder>0 && relatedQuestionId == 0)
            {
                try
                {


                    if (surajConnectionString.Equals("dev"))
                    {
                        surajConnectionString = AppConstant.DevConnectionString;

                    }
                    else if (surajConnectionString.Equals("Test"))
                    {
                        surajConnectionString = AppConstant.TestConnectionString;
                    }
                    else
                    {
                        surajConnectionString = AppConstant.ProductionConnectionString;
                    }
                    string query = @"
                                        SELECT 
                                            question.id AS QuestionId,
                                            question.question AS QuestionText,
                                            question.type AS QuestionType,
                                            question.questionOrder AS QuestionOrder,
                                            question.disabled AS questionEnable,
                                            question.optionAllowed AS OptionAllowed,
                                            question.required AS QuestionRequire,
                                            options.id AS OptionId,
                                            options.option_text AS OptionText,

                                            options.disabled AS optionEnable,
                                            options.optionOrder AS OptionOrder,
                                            options.relatedQuestion AS RelatedQuestion
                                        FROM question 
                                        LEFT JOIN options ON question.id = options.question_id
                                        WHERE  question.questionOrder = @QuestionOrder
                                        ORDER BY question.questionOrder, options.optionOrder;";

                    using (SqlConnection conn = new SqlConnection(surajConnectionString))
                    {

                        conn.Open();
                        SqlCommand myCommand = new SqlCommand(query, conn);
                        
                        myCommand.Parameters.AddWithValue("@questionOrder", questionOrder);

                        using (SqlDataReader reader = myCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    realQuestion.QuestionId = Convert.ToInt32(reader["QuestionId"]);
                                    realQuestion.QuestionText = reader["QuestionText"].ToString();
                                    realQuestion.QuestionType = reader["QuestionType"].ToString();
                                    realQuestion.QuestionOrder = reader["QuestionOrder"] != DBNull.Value
                                                                                            ? Convert.ToInt32(reader["QuestionOrder"])
                                                                                            : 0;
                                    realQuestion.QuestionDisable = Convert.ToInt32(reader["questionEnable"]);
                                    realQuestion.OptionAllowed = Convert.ToInt32(reader["OptionAllowed"]); // Assign OptionAllowed
                                    realQuestion.QuestionRequired = Convert.ToBoolean(reader["QuestionRequire"]);
                                    // Safely add OptionId, checking for DBNull and assigning null if it is DBNull
                                    realQuestion.OptionId.Add(reader["OptionId"] != DBNull.Value
                                        ? (int?)Convert.ToInt32(reader["OptionId"])  // Convert to nullable int if not DBNull
                                        : null);  // Assign null if DBNull

                                    // Safely add OptionText, checking for DBNull
                                    realQuestion.OptionText.Add(reader["OptionText"] != DBNull.Value
                                        ? reader["OptionText"].ToString()
                                        : string.Empty); // Use empty string as default if null

                                    // Safely add RelatedQuestion, checking for DBNull
                                    realQuestion.RelatedQuestion.Add(reader["RelatedQuestion"] != DBNull.Value
                                        ? Convert.ToBoolean(reader["RelatedQuestion"])
                                        : false); // Use false as default if null

                                }
                                reader.Close();
                            }
                        }

                    }

                }
                catch (QuestionValidationException ex)
                {
                    lblErrorMessage.Text = "Internal error, plesae contact Suraj. Error details: " + ex.Message;

                    Session["ErrorMessage"] = lblErrorMessage.Text;
                }
                catch (System.Exception ex)
                {
                    lbltitle.Text = "Internal error, please contact the system admin. Error details: " + ex.Message;
                }
            }
                
            if (relatedQuestionId > 0 )
            {
                try
                {


                    if (surajConnectionString.Equals("dev"))
                    {
                        surajConnectionString = AppConstant.DevConnectionString;

                    }
                    else if (surajConnectionString.Equals("Test"))
                    {
                        surajConnectionString = AppConstant.TestConnectionString;
                    }
                    else
                    {
                        surajConnectionString = AppConstant.ProductionConnectionString;
                    }
                    string query = @"
                                SELECT 
                                    question.id AS QuestionId,
                                    question.question AS QuestionText,
                                    question.type AS QuestionType,
                                    question.questionOrder AS QuestionOrder,
                                    question.disabled AS questionEnable,
                                    question.optionAllowed AS OptionAllowed,
                                    question.required AS QuestionRequire,
                                    options.id AS OptionId,
                                    options.option_text AS OptionText,
                                    options.disabled AS optionEnable,
                                    options.optionOrder AS OptionOrder,
                                    options.relatedQuestion AS RelatedQuestion
                                FROM question 
                                LEFT JOIN options ON question.id = options.question_id
                                WHERE question.id = @QuestionId 
                                ORDER BY question.questionOrder, options.optionOrder;";

                    using (SqlConnection conn = new SqlConnection(surajConnectionString))
                    {

                        conn.Open();
                        SqlCommand myCommand = new SqlCommand(query, conn);

                        myCommand.Parameters.AddWithValue("@QuestionId", relatedQuestionId);
                        
                        
                        using (SqlDataReader reader = myCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    realQuestion.QuestionId = Convert.ToInt32(reader["QuestionId"]);
                                    realQuestion.QuestionText = reader["QuestionText"].ToString();
                                    realQuestion.QuestionType = reader["QuestionType"].ToString();
                                    realQuestion.QuestionOrder = reader["QuestionOrder"] != DBNull.Value
                                                                                            ? Convert.ToInt32(reader["QuestionOrder"])
                                                                                            : 0;
                                    realQuestion.QuestionDisable = Convert.ToInt32(reader["questionEnable"]);
                                    realQuestion.OptionAllowed = Convert.ToInt32(reader["OptionAllowed"]); // Assign OptionAllowed
                                    realQuestion.QuestionRequired = Convert.ToBoolean(reader["QuestionRequire"]);
                                    // Safely add OptionId, checking for DBNull and assigning null if it is DBNull
                                    realQuestion.OptionId.Add(reader["OptionId"] != DBNull.Value
                                        ? (int?)Convert.ToInt32(reader["OptionId"])  // Convert to nullable int if not DBNull
                                        : null);  // Assign null if DBNull

                                    // Safely add OptionText, checking for DBNull
                                    realQuestion.OptionText.Add(reader["OptionText"] != DBNull.Value
                                        ? reader["OptionText"].ToString()
                                        : string.Empty); // Use empty string as default if null

                                    // Safely add RelatedQuestion, checking for DBNull
                                    realQuestion.RelatedQuestion.Add(reader["RelatedQuestion"] != DBNull.Value
                                        ? Convert.ToBoolean(reader["RelatedQuestion"])
                                        : false); // Use false as default if null


                                }
                                reader.Close();
                            }
                        }

                    }

                }
                catch (QuestionValidationException ex)
                {
                    lblErrorMessage.Text = "Internal error, plesae contact Suraj. Error details: " + ex.Message;

                    Session["ErrorMessage"] = lblErrorMessage.Text;
                }
                catch (System.Exception ex)
                {
                    lbltitle.Text = "Internal error, please contact the system admin. Error details: " + ex.Message;
                }
            }
               
            
            return realQuestion;
        }
        /// <summary>
        /// Retrieves the current control from the <see cref="QuestionList"/> placeholder.
        /// This function checks if there are any controls in the placeholder and returns the first control if present.
        /// </summary>
        /// <returns>
        /// The first <see cref="Control"/> in the <see cref="QuestionList"/> placeholder, or <c>null</c> if there are no controls in the placeholder.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if there is an unexpected state where the placeholder does not contain any controls but is expected to have one.
        /// </exception>
        protected Control getCurrentControl()
        {

            if (QuestionList.Controls.Count > 0)
            {
                // Return the first control in the Placeholder
                return QuestionList.Controls[0];
            }
            else
            {
                // Return null if there are no controls in the Placeholder
                return null;
            }
        }

        public int GetTotalQuestionsWithNonZeroOrder()
        {
            int totalQuestions = 0;
            string connectionString = "Data Source=SQL5112.site4now.net;Initial Catalog=db_9ab8b7_324dda12247;User Id=db_9ab8b7_324dda12247_admin;Password=csF4ChaS;";



            try
            {
               
                // SQL query to get the total number of questions with questionOrder not equal to zero
                string query = @"
                            SELECT COUNT(*) AS TotalQuestions
                            FROM question
                            WHERE questionOrder != 0;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the query and get the result
                        totalQuestions = (int)command.ExecuteScalar(); // ExecuteScalar returns the first column of the first row
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Internal error, plesae contact Suraj. Error details: " + ex.Message;
                Session["ErrorMessage"] = lblErrorMessage.Text;
            }

            return totalQuestions;
        }

        /// <summary>
        /// Retrieves the list of related question IDs based on the answers provided for a given question.
        /// This function checks whether the answer's OptionId matches any option in the realQuestion.OptionId list and 
        /// whether the corresponding related question is enabled (true).
        /// </summary>
        /// <param name="answers">
        /// A list of <see cref="Answer"/> objects representing the user's answers.
        /// </param>
        /// <param name="realQuestion">
        /// A <see cref="RealQuestion"/> object that contains the options and related questions for a given question.
        /// </param>
        /// <returns>
        /// A list of nullable integers representing the OptionIds of the answers that have related questions enabled.
        /// If no related questions are found, returns <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either the <paramref name="answers"/> or <paramref name="realQuestion"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="realQuestion"/> contains mismatched OptionId and RelatedQuestion counts.
        /// </exception>
        protected List<int?> getRelatedQuestionIds(List<Answer> answers, RealQuestion realQuestion)
        {
           
            List<int?> optionIdList = new List<int?>();
            // Check if the answers contain any related questions
            if (realQuestion.RelatedQuestion != null && realQuestion.RelatedQuestion.Count > 0 && realQuestion.OptionId != null)
            {
                
                for (int i=0; i <= answers.Count - 1; i++)
                {
                    // Check if answers[i].OptionId is not null
                    if (answers[i].OptionId != null)
                    {
                        // Get the index of OptionId in realQuestion.OptionId
                        int index = realQuestion.OptionId.IndexOf(answers[i].OptionId);

                        // Check if the index is valid and realQuestion.RelatedQuestion has a value at that index
                        if (index >= 0 && index < realQuestion.RelatedQuestion.Count)
                        {
                            // Only add to the list if the related question is true (assuming it's a bool)
                            if (realQuestion.RelatedQuestion[index])
                            {
                                optionIdList.Add(answers[i].OptionId);
                            }
                        }
                    }

                }
                
            }
            else
            {
                optionIdList = null;
            }

            // All checks passed
            return optionIdList;
        }

        /// <summary>
        /// Retrieves answers from the specified control based on the type of control and stores them in a list.
        /// It also handles validation for each control type, ensuring that selections are made where appropriate.
        /// </summary>
        /// <param name="control">The control from which the answers are extracted (RadioButtonList, CheckBoxList, TextBox, DropDownList).</param>
        /// <param name="realQuestion">The question object containing question details and possible options.</param>
        /// <returns>A list of <see cref="Answer"/> objects that represent the answers submitted by the user.</returns>
        /// <exception cref="QuestionValidationException">
        /// Thrown if the user does not select an option or provide input where required, or if an unsupported control type is encountered.
        /// </exception>
        protected List<Answer> getAnswerFromControl(Control control, RealQuestion realQuestion)
        {
            List<Answer> answers = new List<Answer>();
            bool dependentAnswerFlag = (realQuestion.QuestionOrder == 0) ? true : false;
            try
            {
                switch (control)
                {
                    case RadioButtonListUserControl radioButtonListUserControl:
                        // Collect single selected value from RadioButtonListUserControl
                        int selectedIndex = radioButtonListUserControl.SelectedIndex;
                        // Flag dependentAnswer as true if the QuestionOrder is 0
                        
                        answers.Add(new Answer
                        {
                            QuestionId = realQuestion.QuestionId,
                            
                            AnswerText = radioButtonListUserControl.SelectedValues,
                            OptionId = realQuestion.OptionId[selectedIndex],
                            DependentAnswers = dependentAnswerFlag,
                            OptionOrder = selectedIndex

                        });
                        break;

                    case CheckBoxListUserControl checkBoxListUserControl:
                        // Collect multiple selected values from CheckBoxListUserControl
                        List<string> selectedValuesCheckBox = checkBoxListUserControl.SelectedValues;
                        foreach (int index in checkBoxListUserControl.SelectedIndices)
                        {
                            answers.Add(new Answer
                            {
                                QuestionId = realQuestion.QuestionId,
                                
                                AnswerText = checkBoxListUserControl[index],
                                OptionId = realQuestion.OptionId[index],
                                DependentAnswers = dependentAnswerFlag,
                                OptionOrder = index

                            });
                        }
                        break;

                    case TextboxUserControl textboxUserControl:
                        // Single value input from TextboxUserControl
                        answers.Add(new Answer
                        {
                            QuestionId = realQuestion.QuestionId,
                           
                            AnswerText = textboxUserControl.SelectedValue,
                            OptionId = null,
                            DependentAnswers = dependentAnswerFlag,
                            OptionOrder = null
                        });
                        break;

                    case DropDownListUserControl dropDownListUserControl:
                        // Single selected value from DropDownListUserControl
                        int dropDownSelectedIndex = dropDownListUserControl.SelectedIndex;
                        string selectedValue = dropDownListUserControl.SelectedValues;
                        // Check if the selected value is "0"
                        if (selectedValue == "0")
                        {
                            throw new QuestionValidationException("Invalid selection: First choice is not allowed");
                        }
                        // Ensure the selected index is within the bounds of the OptionId list
                        if (dropDownSelectedIndex < 0 || dropDownSelectedIndex > realQuestion.OptionId.Count)
                        {
                            throw new QuestionValidationException("Selected index is out of range.");
                        }
                        // Add the answer to the list
                        answers.Add(new Answer
                        {
                            QuestionId = realQuestion.QuestionId,
                            
                            AnswerText = selectedValue,
                            OptionId = realQuestion.OptionId[dropDownSelectedIndex - 1],
                            DependentAnswers = dependentAnswerFlag,
                            OptionOrder = dropDownSelectedIndex
                        });
                        break;

                    default:
                        throw new QuestionValidationException("Un supportted Control Type");
                        
                }
            }
            catch (QuestionValidationException ex)
            {
                lblErrorMessage.Text = "Error: " + ex.Message;
                Session["ErrorMessage"] = lblErrorMessage.Text;


            }
            return answers;
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Session["ButtonPrevious"] = false;
            btnPrevious.Visible = true;
            try
            {
                // Retrieve the current control (question or survey element) being interacted with
                Control currentControl = getCurrentControl();

                // Get the user's answers from the current control for the given question
                this.answers = getAnswerFromControl(currentControl, realQuestion);
                if (!realQuestion.QuestionRequired && answers.Count <= 0)
                {
                    List<QuestionDisplayesd> questionDisplayedListSaving = Session["QuestionDisplayedList"] as List<QuestionDisplayesd>;



                    // Get the last question's display order in the sequence
                    int questionOrderDisplayed = questionDisplayedListSaving[lastQuestionOrder].QuestionOrderDisplayed;
                    // Update the last displayed question's details with the current question
                    questionDisplayedListSaving[lastQuestionOrder].QuestionId = realQuestion.QuestionId;
                    questionDisplayedListSaving[lastQuestionOrder].QuestionOrderDatabase = realQuestion.QuestionOrder;

                    lastQuestionOrder = (int)Session["LastQuestionOrder"];
                    lastQuestionOrder++;
                    Session["LastQuestionOrder"] = lastQuestionOrder;

                    questionOrderDisplayed++;

                    questionDisplayedListSaving.Add(new QuestionDisplayesd(questionOrderDisplayed, 0, questionOrder));
                    bool dependentAnswerFlag = (realQuestion.QuestionOrder == 0) ? true : false;
                    List<Answer> answers = new List<Answer>();
                    answers.Add(new Answer
                    {
                        QuestionId = realQuestion.QuestionId,
                        AnswerTime = DateTime.Now,
                        AnswerText = "",
                        OptionId = null,
                        OptionOrder = null,
                        DependentAnswers = dependentAnswerFlag

                    });
                    addAnswerOrder(ref answers);

                    // Save the updated list of displayed questions in the session
                    Session["QuestionDisplayedList"] = questionDisplayedListSaving;
                    // Clear any existing error messages
                    Session["ErrorMessage"] = "";
                    Response.Redirect("SurveyQuestion.aspx");
                    return;

                }
                // Get the related questions based on the answers (dependent or conditional questions)
                // If there are related questions, the list of optionIds will be checked in the dependent_question table
                List<int?> relatedQuestion = getRelatedQuestionIds(answers, realQuestion);

                // Validate the user's answers
                // Ensure that the number of answers is within the allowed range for the question
                if (answers != null && answers.Count > 0 && answers.Count <= realQuestion.OptionAllowed)
                {
                    // Retrieve the list of questions that have been displayed in the session
                    List<QuestionDisplayesd> questionDisplayedListSaving = Session["QuestionDisplayedList"] as List<QuestionDisplayesd>;



                    // Get the last question's display order in the sequence
                    int questionOrderDisplayed = questionDisplayedListSaving[lastQuestionOrder].QuestionOrderDisplayed;
                    // Update the last displayed question's details with the current question
                    questionDisplayedListSaving[lastQuestionOrder].QuestionId = realQuestion.QuestionId;
                    questionDisplayedListSaving[lastQuestionOrder].QuestionOrderDatabase = realQuestion.QuestionOrder;

                    lastQuestionOrder = (int)Session["LastQuestionOrder"];
                    lastQuestionOrder++;
                    Session["LastQuestionOrder"] = lastQuestionOrder;

                    // Check if there are related questions to display
                    if (relatedQuestion == null || relatedQuestion.Count <= 0)
                    {

                        // If no related questions, increment the order and add a new question to the display list
                        questionOrderDisplayed++;


                        // Mark the related question ID as 0 (no related questions)
                        // questionDisplayedListSaving.Last().RelatedQuestion = 0;
                        if (questionDisplayedListSaving.Count > lastQuestionOrder)
                        {
                            if (!(questionDisplayedListSaving[lastQuestionOrder].RelatedQuestion > 0))
                            {
                                questionDisplayedListSaving[lastQuestionOrder].RelatedQuestion = 0;
                                questionDisplayedListSaving.Add(new QuestionDisplayesd(questionOrderDisplayed, 0, questionOrder));

                            }
                        }
                        else
                        {
                            //questionDisplayedListSaving[lastQuestionOrder].RelatedQuestion = 0;
                            questionDisplayedListSaving.Add(new QuestionDisplayesd(questionOrderDisplayed, 0, questionOrder));
                        }



                        // Clear any existing error messages
                        Session["ErrorMessage"] = "";
                    }
                    else
                    {
                        // If there are related questions, retrieve the related question IDs from the database
                        List<int> releteQuestionId = getRelatedQuestionDatabase(relatedQuestion);

                        // Remove duplicates from the related question IDs list // need further modification
                        List<int> singleQuestionId = releteQuestionId.Distinct().ToList();

                        // Store the last related question ID in the session

                        foreach (int questionId in singleQuestionId)
                        {

                            // Add the related question to the question displayed list
                            questionDisplayedListSaving.Add(new QuestionDisplayesd(questionOrderDisplayed, questionId, questionOrder));
                            questionOrder++;

                            Session["relatedQuestionId"] = questionId;
                        }


                        // Clear any existing error messages
                        Session["ErrorMessage"] = "";
                    }

                    // Save the updated list of displayed questions in the session
                    Session["QuestionDisplayedList"] = questionDisplayedListSaving;
                    addAnswerOrder(ref answers);

                }
                else
                {
                    // If the answers are invalid (either no answers or too many answers), display an error message
                    if (string.IsNullOrEmpty(lblErrorMessage.Text))
                    {
                        // If there is no error message already, display a default error message
                        lblErrorMessage.Text = "The answer is required.";
                        Session["ErrorMessage"] = lblErrorMessage.Text;

                        // If the number of selected answers doesn't match the allowed number, display another error
                        if (answers.Count != realQuestion.OptionAllowed)
                        {
                            lblErrorMessage.Text = "Too many options selected.";
                            Session["ErrorMessage"] = lblErrorMessage.Text;
                        }
                    }
                    else
                    {
                        // If there's already an error message in the session, display it
                        lblErrorMessage.Text = Session["ErrorMessage"] as string;
                    }


                }
            }
            catch (QuestionValidationException ex)
            {
                // Catch and handle any validation exceptions
                lblErrorMessage.Text = "Error: " + ex.Message;
                Session["ErrorMessage"] = lblErrorMessage.Text;
            }

            // Finally, redirect to the SurveyQuestion page
            Response.Redirect("SurveyQuestion.aspx");



        }



        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            Session["ButtonPrevious"] = true;
            try
            {
                // Retrieve the list of displayed questions from the session
                List<QuestionDisplayesd> questionDisplayedListSaving = Session["QuestionDisplayedList"] as List<QuestionDisplayesd>;

                //Get the current question order, related question ID, and question ID from the last displayed question
                int questionOrder = questionDisplayedListSaving.Last().QuestionOrderDisplayed;
                int relatedQuestionId = questionDisplayedListSaving.Last().RelatedQuestion;
                int questionId = questionDisplayedListSaving.Last().QuestionId;

                //// Get the current question order, related question ID, and question ID from the last displayed question
                //int questionOrder = questionDisplayedListSaving[lastQuestionOrder].QuestionOrderDisplayed;
                //int relatedQuestionId = questionDisplayedListSaving[lastQuestionOrder].RelatedQuestion;
                //int questionId = questionDisplayedListSaving[lastQuestionOrder].QuestionId;

                // If there is a related question, no action needed for the related question ID
                if ((relatedQuestionId > 0))
                {
                    // Uncomment this line to reset the related question if needed
                    // questionDisplayedListSaving.Last().RelatedQuestion = 0;
                }
                // If the current question is not the first, allow navigating to the previous question
                else if (questionOrder > 1)
                {
                    questionOrder--; // Move to the previous question
                }

                // If the question order is 1 (first question), hide the "Previous" button
                if (questionOrder == 1)
                {
                    btnPrevious.Visible = false; // Hide the "Previous" button when on the first question
                }

                // Retrieve the list of saved answers from the session
                List<Answer> savedAnswers = Session["Answers"] as List<Answer>;
                //deleteLastAnswer(savedAnswers);
                /* if (savedAnswers != null && savedAnswers.Count > 0)
                 {
                     // Step 1: Retrieve the last answer from the list
                     var lastAnswer = savedAnswers[savedAnswers.Count - 1];

                     // Step 2: Define the target QuestionId to remove answers related to the same question
                     int targetQuestionId = lastAnswer.QuestionId;

                     // Step 3: Remove all answers from savedAnswers that have the same QuestionId as the last answer
                     for (int i = savedAnswers.Count - 1; i >= 0; i--)
                     {
                         // Check if the answer belongs to the same question
                         if (savedAnswers[i].QuestionId == targetQuestionId)
                         {
                             savedAnswers.RemoveAt(i);  // Remove the answer with the matching QuestionId
                         }
                     }

                     // Save the updated list of answers back to the session after modification
                     Session["Answers"] = savedAnswers;
                 }*/

                lastQuestionOrder = (int)Session["LastQuestionOrder"];
                lastQuestionOrder--;
                Session["LastQuestionOrder"] = lastQuestionOrder;
                // Remove the last displayed question from the list
                questionDisplayedListSaving.RemoveAt(questionDisplayedListSaving.Count - 1);

                // Update the session with the modified list of displayed questions
                Session["QuestionDisplayedList"] = questionDisplayedListSaving;

                // Clear any existing error messages from the session
                Session["ErrorMessage"] = "";
            }
            catch (Exception ex)
            {
                // Catch any errors, display them to the user, and save the error message in the session
                lblErrorMessage.Text = "Error: " + ex.Message;
                Session["ErrorMessage"] = lblErrorMessage.Text;
            }

            /*if (savedAnswers != null && savedAnswers.Count > 0)
            {
                // Get the answerOrder of the last answer
                int targetAnswerOrder = savedAnswers[savedAnswers.Count - 1].AnswerOrder;

                // Remove all answers from the end of the list with the same answerOrder
                for (int i = savedAnswers.Count - 1; i >= 0; i--)
                {

                    if (savedAnswers[i].AnswerOrder == targetAnswerOrder )
                    {
                        savedAnswers.RemoveAt(i);
                    }
                    else
                    {
                        // Stop the loop as soon as a different answerOrder is found
                        break;
                    }
                }

                // Save the updated list back to the session
                Session["Answers"] = savedAnswers;
            }*/

            Response.Redirect("SurveyQuestion.aspx");



        }

        protected void addAnswerOrder(ref List<Answer> answers)
        {
            // Retrieve the saved answers from the session and determine the next answer order
            List<Answer> savedAnswers = Session["Answers"] as List<Answer>;
            int orderanswer = savedAnswers.Any() ? savedAnswers.Last().AnswerOrder : 0;

            // Loop through the current answers and assign them order numbers
            for (int i = 0; i < answers.Count; i++)
            {
                if (savedAnswers.Count <= 0)
                {
                    // If no answers have been saved yet, add the first answer with the appropriate order
                    answers[i].AnswerOrder++;
                    savedAnswers.Add(answers[i]);
                }
                else
                {
                    // If answers already exist, handle the insertion of the new answer in the list
                    if (answers.Count > 1 && answers[i].QuestionId == realQuestion.QuestionId)
                    {
                        // For multiple answers, ensure correct order by adjusting the answer order
                        orderanswer++;
                        answers[i].AnswerOrder = orderanswer;
                        savedAnswers.Add(answers[i]);
                        orderanswer--;
                    }
                    else
                    {
                        // For single answers or other cases, increment the order and add the answer
                        answers[i].AnswerOrder = ++orderanswer;
                        savedAnswers.Add(answers[i]);
                    }
                }
            }
            // Save the updated answers back to the session
            Session["Answers"] = savedAnswers;
        }

        protected void deleteLastAnswer(List<Answer> savedAnswers)
        {
            if (savedAnswers != null && savedAnswers.Count > 0)
            {
                // Step 1: Retrieve the last answer from the list
                var lastAnswer = savedAnswers[savedAnswers.Count - 1];

                // Step 2: Define the target QuestionId to remove answers related to the same question
                int targetQuestionId = lastAnswer.QuestionId;

                // Step 3: Remove all answers from savedAnswers that have the same QuestionId as the last answer
                for (int i = savedAnswers.Count - 1; i >= 0; i--)
                {
                    // Check if the answer belongs to the same question
                    if (savedAnswers[i].QuestionId == targetQuestionId)
                    {
                        savedAnswers.RemoveAt(i);  // Remove the answer with the matching QuestionId
                    }
                }

                // Save the updated list of answers back to the session after modification
                Session["Answers"] = savedAnswers;
            }
            else
            {
                throw new QuestionValidationException("This savedAnswer is null!");
            }
        }

        protected void fillAnswerInControls( List<Answer> savedAnswers)
        {
            
            Control control = getCurrentControl();

            try
            {
                if (savedAnswers.Count > 0)
                {
                    List<Answer> answerList = new List<Answer>();
                    var lastAnswer = savedAnswers[savedAnswers.Count - 1];
                    int targetQuestionId = lastAnswer.QuestionId;

                    // Step 3: Remove all answers from savedAnswers that have the same QuestionId as the last answer
                    for (int i = savedAnswers.Count - 1; i >= 0; i--)
                    {
                        // Check if the answer belongs to the same question
                        if (savedAnswers[i].QuestionId == targetQuestionId)
                        {
                            answerList.Add(savedAnswers[i]);  // Remove the answer with the matching QuestionId
                        }
                        else
                        {
                            break;
                        }
                    }


                    switch (control)
                    {
                        case RadioButtonListUserControl radioButtonListUserControl:


                            foreach (Answer answer in answerList)
                            {
                                if (answer.OptionOrder != null && answer.QuestionId == targetQuestionId)
                                {
                                    radioButtonListUserControl.SelectedIndex = answer.OptionOrder.Value;
                                }

                            }
                            break;
                        case CheckBoxListUserControl checkBoxListUserControl:
                            List<int> indices = new List<int>();
                            foreach (Answer answer in answerList)
                            {
                                if (answer.OptionOrder != null && answer.QuestionId == targetQuestionId)
                                    indices.Add(answer.OptionOrder.Value);

                            }
                            if (indices.Count > 0)
                            {
                                checkBoxListUserControl.SelectedIndices = indices;
                            }


                            break;

                        case TextboxUserControl textboxUserControl:
                            foreach (Answer answer in answerList)
                            {
                                if (answer.OptionOrder != null && answer.QuestionId == targetQuestionId)
                                {
                                    textboxUserControl.SelectedValue = answer.AnswerText;
                                }

                            }

                            break;

                        case DropDownListUserControl dropDownListUserControl:
                            foreach (Answer answer in answerList)
                            {
                                if (answer.OptionOrder != null && answer.QuestionId == targetQuestionId)
                                {
                                    dropDownListUserControl.SelectedIndex = answer.OptionOrder.Value;
                                }

                            }
                            break;

                        default:
                            throw new QuestionValidationException("Un supportted Control Type");

                    }
                }
            }
            catch (QuestionValidationException ex)
            {
                lblErrorMessage.Text = "Error: " + ex.Message;
                Session["ErrorMessage"] = lblErrorMessage.Text;


            }
           
        }

        /// <summary>
        /// Retrieves a list of related question IDs from the database based on a list of option IDs.
        /// The method constructs and executes a SQL query that selects related questions for the provided option IDs.
        /// It handles SQL exceptions and general exceptions during the execution of the query.
        /// </summary>
        /// <param name="optionIdList">A list of nullable integers representing option IDs for which related questions are being fetched.</param>
        /// <returns>A list of integers containing the IDs of related questions based on the provided option IDs.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the `optionIdList` is empty or contains no valid values (all options are null).</exception>
        /// <exception cref="SqlException">Thrown when there is an issue executing the SQL query (e.g., connection issues, query errors).</exception>
        /// <exception cref="System.Exception">Thrown for any other unexpected errors during the execution of the method.</exception>
        protected List<int> getRelatedQuestionDatabase(List<int?> optionIdList)
        {
            List<int> questionIds = new List<int>();

            // Base query with a placeholder for the IN clause
            string queryBase = @"
                                SELECT 
                                    dependent_question.dependent_question_id AS questionId,
                                    dependent_question.option_Id AS optionId
                                FROM dependent_question
                                LEFT JOIN options ON dependent_question.option_Id = options.id
                                WHERE dependent_question.option_Id IN ({0})
                                ORDER BY dependent_question.option_Id;
                                 ";

            try
            {
                using (SqlConnection connection = new SqlConnection(surajConnectionString))
                {
                    connection.Open();

                    if (optionIdList.Any(x => x.HasValue))
                    {
                        // Create parameter placeholders like @id0, @id1, etc.
                        var parameterNames = optionIdList
                            .Where(x => x.HasValue)
                            .Select((_, index) => $"@id{index}")
                            .ToArray();

                        // Build the final query with the IN clause
                        string query = string.Format(queryBase, string.Join(",", parameterNames));

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Add parameters for each value in optionIdList
                            for (int i = 0; i < optionIdList.Count; i++)
                            {
                                if (optionIdList[i].HasValue)
                                {
                                    command.Parameters.AddWithValue(parameterNames[i], optionIdList[i].Value);
                                }
                            }

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Loop through the result set and add questionIds
                                while (reader.Read())
                                {
                                    questionIds.Add(reader.GetInt32(reader.GetOrdinal("questionId")));
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("The optionIdList is empty or contains no valid values.");
                    }
                }
            }
            catch (QuestionValidationException ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
            catch (SqlException ex)
            {
                // Handle any SQL-related exceptions
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions
                Console.WriteLine("Error: " + ex.Message);
            }

            return questionIds;

        }

        protected void btnSubmitAnonymously_Click(object sender, EventArgs e)
        {
            // get mac address and save it to the database
            string clientIpAddress = GetClientIpAddress();
            List<Answer> answers = Session["Answers"] as List<Answer>;
            int respondantId = 0;
            if (clientIpAddress != null)
            {
                 respondantId = InsertRespondent(clientIpAddress);
            }
            if( respondantId != 0 && answers != null && answers.Count>0)
            {
                SaveAnswersToDatabase(respondantId, answers);
                InsertMember(respondantId);
                Session.Abandon();
                Response.Redirect("~/Firstpage.aspx");
            }
            else
            {
                lblErrorMessage.Text = "Did not fint the respondant in the data base";
                Session["ErrorMessage"] = lblErrorMessage.Text;
                Response.Redirect("SurveyQuestion.aspx");
            }
            


        }
        

        protected void btnRegisterAndSubmit_Click(object sender, EventArgs e)
        {
            
            // go to register member page and after that save it to the data base
            Response.Redirect("RegisterMember.aspx");
        }

        public string GetClientIpAddress()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            // In case of multiple proxies, split the IPs and get the first one
            if (ipAddress.Contains(","))
            {
                ipAddress = ipAddress.Split(',')[0].Trim();
            }
            // Convert any IPv6 loopback (::1) or its variations to IPv4 loopback (127.0.0.1)
            if (IPAddress.TryParse(ipAddress, out IPAddress parsedIp))
            {
                if (IPAddress.IsLoopback(parsedIp))
                {
                    ipAddress = "127.0.0.1";
                }
            }

            return ipAddress;
        }

        public void SaveAnswersToDatabase(int respondantId, List<Answer> answers)
        {
            string connectionString = "Data Source=SQL5112.site4now.net;Initial Catalog=db_9ab8b7_324dda12247;User Id=db_9ab8b7_324dda12247_admin;Password=csF4ChaS;";

            try
            {
                // Define your SQL query for inserting answers
                string insertAnswerQuery = @"
                        INSERT INTO Answer (answer_text,answeredAt, question_id, option_id, respondant_id, answerOrder)
                        VALUES (@AnswerText,@AnswerTime,  @QuestionId, @OptionId, @RespondantId, @AnswerOrder)";

                // Using SQLConnection to insert data
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert answers with the associated RespondentId
                    foreach (var answer in answers)
                    {
                        using (var command = new SqlCommand(insertAnswerQuery, connection))
                        {
                            command.Parameters.AddWithValue("@AnswerText", answer.AnswerText);
                            command.Parameters.AddWithValue("@AnswerTime", answer.AnswerTime);
                            command.Parameters.AddWithValue("@QuestionId", answer.QuestionId);
                            command.Parameters.AddWithValue("@OptionId", (object)answer.OptionId ?? DBNull.Value); // Handle nullable OptionId
                            command.Parameters.AddWithValue("@RespondantId", respondantId);
                            command.Parameters.AddWithValue("@AnswerOrder", answer.AnswerOrder);

                            command.ExecuteNonQuery(); // Insert each answer
                        }
                    }
                }

                Console.WriteLine("Answers saved successfully.");
            }
            catch (QuestionValidationException ex)
            {
                lblErrorMessage.Text = ex.Message;
                Session["ErrorMessage"] = lblErrorMessage.Text;
            }
        }


        protected int InsertRespondent(string clientIpAddress)
        {
            string connectionString = "Data Source=SQL5112.site4now.net;Initial Catalog=db_9ab8b7_324dda12247;User Id=db_9ab8b7_324dda12247_admin;Password=csF4ChaS;";
            int respondentId = 0;
            try
            {
                 if (surajConnectionString.Equals("dev"))
                {
                    surajConnectionString = AppConstant.DevConnectionString;

                }
                else if (surajConnectionString.Equals("Test"))
                {
                    surajConnectionString = AppConstant.TestConnectionString;
                }
                else
                {
                    surajConnectionString = AppConstant.ProductionConnectionString;
                }

                string insertQuery = @"
                                    INSERT INTO respondant (ipAddress)
                                    OUTPUT INSERTED.respondant_id
                                    VALUES (@IpAddress);";

                // Ensure the connection string is valid before proceeding
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string is invalid.");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@IpAddress", clientIpAddress);

                        // Get the inserted respondent_id
                        respondentId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (QuestionValidationException ex)
            {
                lblErrorMessage.Text = ex.Message;
                Session["ErrorMessage"] = lblErrorMessage.Text;
            }
            

            return respondentId;
        }

        protected void InsertMember(int respondentId)
        {
            string connectionString = "Data Source=SQL5112.site4now.net;Initial Catalog=db_9ab8b7_324dda12247;User Id=db_9ab8b7_324dda12247_admin;Password=csF4ChaS;";
            try
            {
                // Default values for missing data
                string givenName =  "Anonymous" ;
                string familyName =  "Anonymous" ;
                DateTime? dateOfBirth =  (DateTime?)null ;
                string phoneNumber =  null;

                // SQL query with handling for NULL values in dob and phone
                string insertMemberQuery = @"
                    INSERT INTO member (givenName, lastName, DOB, phone, respondant_id)
                    VALUES (@GivenName, @LastName, @DOB, @Phone, @RespondantId)";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insertMemberQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@GivenName", givenName);
                        cmd.Parameters.AddWithValue("@LastName", familyName);
                        cmd.Parameters.AddWithValue("@DOB",  DBNull.Value);  // Use NULL if dob is empty
                        cmd.Parameters.AddWithValue("@Phone",  DBNull.Value);  // Use NULL if phone is empty
                        cmd.Parameters.AddWithValue("@RespondantId", respondentId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                Session["ErrorMessage"] = lblErrorMessage.Text;
            }
        }


    }
}