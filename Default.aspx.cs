using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Collections.IEnummerable;


using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using WebApplication1.Classes;
using WebApplication1.CustomControl;
using System.IO;


namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        private string _surajConnectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
        private string surajConnectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
        List<RealQuestion> questionList = new List<RealQuestion> { new RealQuestion() };

        //AppConstant appConstant=new AppConstant();
        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {
                // Initially bind the grid with all data
                if (Session["SucessfullLogin"] == null)
                {
                    Response.Redirect("~/StaffLogin.aspx");
                }
                if (Session["SelectedValues"] == null)
                {
                    Session["SelectedValues"] = new List<SelectedValues>();
                }
                

            }
            try
            {
                int respondantID;
                
                

                BindDropdownsAndListBox();// Initialize dropdown lists or bind data (optional)
                questionList = Load_Question_with_Option();
                Display_Question_with_Option_in_SearchList(questionList);
                
                if (Session["RespondantID"] == null)
                {
                    BindGridView();

                }
                else
                {
                    respondantID = (int)Session["RespondantID"];
                    BindGridView(respondantID);
                }
            }
            catch (QuestionValidationException ex)
            {
                // Log or display the error
                lblErrorMessage.Text = "Error: " + ex.Message;
            }



        }
        // Method to bind dropdowns (Age Range, State/Territory, etc.)
        private void BindDropdownsAndListBox()
        {
            string genderQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";
            string ageRangeQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";
            string stateQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";
            string bankQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";
            string bankServicesQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";
            string newspaperQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";
            string newsSectionQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";
            string sportsQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";
            string destinationQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";
            string doodQuery = "SELECT o.option_text FROM options o JOIN question q ON o.question_id = q.id Where q.id = 1;";



            // Add more logic here to populate the other dropdowns like gender, state, bank, etc.
        }

        protected void Display_Question_with_Option_in_SearchList(List<RealQuestion> questionList)
        {
            try
            {
                // Clear the PlaceHolder before adding new controls
                SearchList.Controls.Clear();

                int counter = 0; // Declare and initialize the counter

                foreach (RealQuestion realQuestion in questionList)
                {


                    // Create a container div for collapsible content
                    Panel cardPanel = new Panel();
                    cardPanel.CssClass = "card mb-3";

                    // Card header with toggle button
                    Panel cardHeader = new Panel();
                    cardHeader.CssClass = "card-header";
                    // Create ASP.NET Button
                    LinkButton collapseButton = new LinkButton();
                    collapseButton.ID = $"heading{realQuestion.QuestionId}";
                    collapseButton.CssClass = "btn btn-link text-decoration-none ";
                    collapseButton.Attributes.Add("type", "button");
                    collapseButton.Attributes.Add("data-toggle", "collapse");
                    collapseButton.Attributes.Add("data-target", $"#collapse{realQuestion.QuestionId}");
                    collapseButton.Attributes.Add("aria-expanded", "false");
                    collapseButton.Attributes.Add("aria-controls", $"collapse{realQuestion.QuestionId}");
                    // Set the CommandArgument to store the questionId (or the entire realQuestion object)
                    collapseButton.CommandArgument = realQuestion.QuestionId.ToString(); // or realQuestion if needed

                    // Set the OnClick handler
                    collapseButton.Click += new EventHandler(CollapseButton_Click);
                    // Add styled text with a span
                    collapseButton.Text = $@"
                             <span id='icon{realQuestion.QuestionId}' class='accordion-icon me-2'>&#43;</span>
                             <span style='white-space: normal; word-wrap: break-word;'>
                            {realQuestion.QuestionText}
                         </span>";

                    // Enable text as HTML
                    collapseButton.Text = HttpUtility.HtmlDecode(collapseButton.Text); // Ensure it renders HTML

                    // Add to the header
                    cardHeader.Controls.Add(collapseButton);
                    // Collapsible card body
                    Panel cardBody = new Panel();
                    cardBody.ID = $"collapse{realQuestion.QuestionId}";
                    cardBody.CssClass = "collapse card-body";
                    cardBody.Attributes["aria-labelledby"] = $"heading{realQuestion.QuestionId}";

                    // Add the appropriate user control based on the question type
                    switch (realQuestion.QuestionType)
                    {



                        case "RadioButtonList":

                            // Load the RadioButtonListUserControl
                            RadioButtonListUserControl radioButtonListUserControl =
                                    (RadioButtonListUserControl)this.LoadControl("~/CustomControl/RadioButtonListUserControl.ascx");
                            radioButtonListUserControl.ID = $"radioUC_{realQuestion.QuestionId}";
                            radioButtonListUserControl.RadioQuestionText = realQuestion.QuestionText;
                            // Access the RadioButtonList inside the user control
                            RadioButtonList radioButtonList = (RadioButtonList)radioButtonListUserControl.FindControl("radioButtonListUC");

                            // Enable AutoPostBack so the selection triggers a postback
                            radioButtonList.AutoPostBack = true;
                            radioButtonList.SelectedIndexChanged += new EventHandler((sender, e) => RadioButtonList_SelectedIndexChanged(sender, e, realQuestion));
                            // Add options to the RadioButtonList
                            foreach (string option in realQuestion.OptionText)
                            {
                                if (!string.IsNullOrWhiteSpace(option))
                                {
                                    ListItem listItem = new ListItem
                                    {
                                        Text = option,
                                        Value = option
                                    };
                                    listItem.Attributes.Add("class", "list-group-item list-group-item-action text-danger fw-bolder");

                                    radioButtonListUserControl.addItem(listItem);
                                }
                            }


                            // Add the div wrapper (containing the control) to the PlaceHolder
                            cardBody.Controls.Add(radioButtonListUserControl);

                            break;

                        case "DropDownList":
                            // Load the DropDownListUserControl
                            DropDownListUserControl dropDownListUserControl =
                                (DropDownListUserControl)this.LoadControl("~/CustomControl/DropDownListUserControl.ascx");
                            dropDownListUserControl.ID = $"dropDownUC_{realQuestion.QuestionId}";
                            dropDownListUserControl.DropDownQuestionText = realQuestion.QuestionText;

                            // Access the DropDownList inside the user control
                            DropDownList dropDownList = (DropDownList)dropDownListUserControl.FindControl("dropDownListUC");

                            // Enable AutoPostBack so the selection triggers a postback
                            dropDownList.AutoPostBack = true;
                            dropDownList.SelectedIndexChanged += new EventHandler((sender, e) => DropDownList_SelectedIndexChanged(sender, e, realQuestion));
                            // Add options to the DropDownList
                            ListItem defaultItem = new ListItem("Select an option", "0");
                            dropDownListUserControl.addItem(defaultItem); // Optional default item

                            foreach (string option in realQuestion.OptionText)
                            {
                                if (!string.IsNullOrWhiteSpace(option))
                                {
                                    ListItem listItem = new ListItem
                                    {
                                        Text = option,
                                        Value = option
                                    };
                                    listItem.Attributes.Add("class", "list-group-item-action list-group-item text-danger");
                                    dropDownListUserControl.addItem(listItem);
                                }
                            }

                            // Add the control to the PlaceHolder
                            cardBody.Controls.Add(dropDownListUserControl);
                            break;

                        case "CheckBoxList":
                            // Load the CheckBoxListUserControl
                            CheckBoxListUserControl checkBoxListUserControl =
                                (CheckBoxListUserControl)this.LoadControl("~/CustomControl/CheckBoxListUserControl.ascx");
                            checkBoxListUserControl.ID = $"checkBoxUC_{realQuestion.QuestionId}";
                            checkBoxListUserControl.CheckBoxQuestionText = realQuestion.QuestionText;

                            // Access the CheckBoxList inside the user control
                            CheckBoxList checkBoxList = (CheckBoxList)checkBoxListUserControl.FindControl("optionsCheckBoxList");

                            // Enable AutoPostBack so the selection triggers a postback
                            checkBoxList.AutoPostBack = true;
                            checkBoxList.SelectedIndexChanged += new EventHandler((sender, e) => CheckBoxList_SelectedIndexChanged(sender, e, realQuestion));
                            // Add options to the CheckBoxList
                            foreach (string option in realQuestion.OptionText)
                            {
                                if (!string.IsNullOrWhiteSpace(option))
                                {
                                    ListItem listItem = new ListItem
                                    {
                                        Text = option,
                                        Value = option
                                    };
                                    listItem.Attributes.Add("class", "list-group-item list-group-item-action text-danger fw-bolder");
                                    checkBoxListUserControl.addItem(listItem);
                                }
                            }

                            // Add the control to the PlaceHolder
                            cardBody.Controls.Add(checkBoxListUserControl);
                            break;

                        case "TextBox":
                            // Load the TextBoxUserControl
                            TextboxUserControl textBoxUserControl =
                                (TextboxUserControl)this.LoadControl("~/CustomControl/TextBoxUserControl.ascx");
                            textBoxUserControl.ID = $"textBoxUC_{realQuestion.QuestionId}";
                            textBoxUserControl.TextBoxQuestionText = realQuestion.QuestionText;

                            // Create a Search Button
                            Button searchButton = new Button
                            {
                                ID = $"searchBtn_{realQuestion.QuestionId}",
                                Text = "Search",
                                CssClass = "btn btn-primary mt-2" // Add Bootstrap classes for styling
                            };

                            // Add an event handler for the Search button
                            searchButton.Click += new EventHandler((sender, e) => SearchButton_Click(sender, e, realQuestion));

                            

                            // Create a container div for the TextBox, Validators, and Button (optional for layout purposes)
                            Panel textBoxContainer = new Panel
                            {
                                CssClass = "d-flex flex-column gap-2" // Use Flexbox for spacing and alignment
                            };

                            // Add the TextBoxUserControl, validators, and SearchButton to the container
                            textBoxContainer.Controls.Add(textBoxUserControl);
                            
                            textBoxContainer.Controls.Add(searchButton);

                            // Add the container to the PlaceHolder
                            cardBody.Controls.Add(textBoxContainer);
                            break;

                    }

                    // Add header and body to the card panel
                    cardPanel.Controls.Add(cardHeader);
                    cardPanel.Controls.Add(cardBody);

                    // Add the card panel to the SearchList placeholder
                    SearchList.Controls.Add(cardPanel);

                    counter++; // Increment counter for unique IDs
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = $"An error occurred: {ex.Message}";
            }
        }

        protected void RadioButtonList_SelectedIndexChanged(object sender, EventArgs e, RealQuestion realQuestion)
        {
            // Get the RadioButtonList control
            RadioButtonList radioButtonList = (RadioButtonList)sender;

            // Get the selected value from the RadioButtonList
            string selectedValue = radioButtonList.SelectedItem.Text;  // Get the selected text

            // Retrieve the List of SelectedValues from the session
            List<SelectedValues> selectedValuesList = Session["SelectedValues"] as List<SelectedValues>;



            // Check if the list already contains a SelectedValues object for the current question
            var selectedValueEntry = selectedValuesList.FirstOrDefault(x => x.QuestionId == realQuestion.QuestionId);

            if (selectedValueEntry != null)
            {
                // Update the existing entry's selected value
                selectedValueEntry.UserSelectedValues = selectedValue;
            }
            else
            {
                // Create a new SelectedValues object and add it to the list
                selectedValuesList.Add(new SelectedValues
                {
                    QuestionId = realQuestion.QuestionId,
                    UserSelectedValues = selectedValue
                });
            }

            // Store the updated list back into the session
            Session["SelectedValues"] = selectedValuesList;
            Session["RespondantID"] = null;


            UpdateSelectedFiltersDisplay(selectedValuesList);

            // Call BindGridViewForSearch with the selected values wrapped in a list (this triggers your search logic)
            BindGridViewForSearch(realQuestion, selectedValuesList);

            // Find the panel that controls the collapsible section related to this question
            Panel collapsiblePanel = (Panel)SearchList.FindControl($"collapse{realQuestion.QuestionId}");
            LinkButton collapseButton = (LinkButton)SearchList.FindControl($"heading{realQuestion.QuestionId}");

            if (collapsiblePanel != null && collapseButton != null)
            {
                // Ensure the collapsible section stays expanded
                collapsiblePanel.CssClass = collapsiblePanel.CssClass.Replace("collapse", "").Trim();
                collapsiblePanel.CssClass += " show"; // Keep it expanded
                collapseButton.Attributes["aria-expanded"] = "true"; // Indicate that the panel is expanded
            }


        }



        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e, RealQuestion realQuestion)
        {
            DropDownList dropDownList = (DropDownList)sender;

            string selectedValue = dropDownList.SelectedValue;  // Get the selected value
            string selectedText = dropDownList.SelectedItem.Text;  // Get the selected text

            // Retrieve the List of SelectedValues from the session
            List<SelectedValues> selectedValuesList = Session["SelectedValues"] as List<SelectedValues>;



            // Find if the QuestionId already exists in the list, and update or add the new selection
            var existingSelectedValue = selectedValuesList.FirstOrDefault(sv => sv.QuestionId == realQuestion.QuestionId);

            if (existingSelectedValue != null)
            {
                // Update the existing value
                existingSelectedValue.UserSelectedValues = selectedValue;
            }
            else
            {
                // Add a new SelectedValues object to the list
                selectedValuesList.Add(new SelectedValues
                {
                    QuestionId = realQuestion.QuestionId,
                    UserSelectedValues = selectedValue
                });
            }
            if (selectedValue == "0")
            {
                // Remove the element(s) from the list where QuestionId matches the current question and UserSelectedValues is "0"
                selectedValuesList.RemoveAll(sv => sv.QuestionId == realQuestion.QuestionId && sv.UserSelectedValues == "0");

            }
            // Store the updated List back into the session
            Session["SelectedValues"] = selectedValuesList;
            Session["RespondantID"] = null;
            UpdateSelectedFiltersDisplay(selectedValuesList);

            // Bind the grid view with the updated selected values list
            BindGridViewForSearch(realQuestion, selectedValuesList);


            // Find the panel that controls the collapsible section related to this question
            Panel collapsiblePanel = (Panel)SearchList.FindControl($"collapse{realQuestion.QuestionId}");
            LinkButton collapseButton = (LinkButton)SearchList.FindControl($"heading{realQuestion.QuestionId}");

            if (collapsiblePanel != null && collapseButton != null)
            {
                // Ensure the collapsible section stays expanded
                collapsiblePanel.CssClass = collapsiblePanel.CssClass.Replace("collapse", "").Trim();
                collapsiblePanel.CssClass += " show"; // Keep it expanded
                collapseButton.Attributes["aria-expanded"] = "true"; // Indicate that the panel is expanded
            }


        }




        protected void CheckBoxList_SelectedIndexChanged(object sender, EventArgs e, RealQuestion realQuestion)
        {
            CheckBoxList checkBoxList = (CheckBoxList)sender;

            // Retrieve the List of SelectedValues from the session
            List<SelectedValues> selectedValuesList = Session["SelectedValues"] as List<SelectedValues>;


            // Loop through each item in the CheckBoxList and update the selected values list
            foreach (ListItem item in checkBoxList.Items)
            {
                if (item.Selected)
                {
                    // Add the selected item only if it doesn't already exist
                    if (!selectedValuesList.Any(sv =>
                        sv.QuestionId == realQuestion.QuestionId &&
                        sv.UserSelectedValues == item.Value))
                    {
                        selectedValuesList.Add(new SelectedValues
                        {
                            QuestionId = realQuestion.QuestionId,
                            UserSelectedValues = item.Value
                        });
                    }
                }
                else
                {
                    // Remove the deselected item from the list
                    selectedValuesList.RemoveAll(sv =>
                        sv.QuestionId == realQuestion.QuestionId &&
                        sv.UserSelectedValues == item.Value);
                }
            }

            // Update the session with the modified list
            Session["SelectedValues"] = selectedValuesList;
            Session["RespondantID"] = null;
            UpdateSelectedFiltersDisplay(selectedValuesList);
            // Call BindGridViewForSearch with the updated list of selected values
            BindGridViewForSearch(realQuestion, selectedValuesList);

            // Find the panel that controls the collapsible section
            Panel collapsiblePanel = (Panel)SearchList.FindControl($"collapse{realQuestion.QuestionId}");
            LinkButton collapseButton = (LinkButton)SearchList.FindControl($"heading{realQuestion.QuestionId}");

            if (collapsiblePanel != null && collapseButton != null)
            {
                // Ensure the collapsible section stays expanded
                collapsiblePanel.CssClass = collapsiblePanel.CssClass.Replace("collapse", "").Trim();
                collapsiblePanel.CssClass += " show"; // Keep it expanded
                collapseButton.Attributes["aria-expanded"] = "true"; // Indicate that the panel is expanded
            }


        }

        public void SearchButton_Click(object sender, EventArgs e, RealQuestion realQuestion)
        {
            // Get the Button control that triggered the event
            Button searchButton = (Button)sender;

            // Find the TextBoxUserControl within the placeholder
            TextboxUserControl textBoxUserControl = (TextboxUserControl)SearchList.FindControl($"textBoxUC_{realQuestion.QuestionId}");

            if (textBoxUserControl != null)
            {
                // Find the TextBox inside the TextBoxUserControl (replace "txtResponse" with the actual ID of the TextBox)
                TextBox textBox = (TextBox)textBoxUserControl.FindControl("textBoxUC");

                // Check if the TextBox is found and retrieve its value
                if (textBox != null)
                {
                    string searchValue = textBox.Text;

                    // Try to convert the searchValue to an integer
                    if (int.TryParse(searchValue, out int respondentId))
                    {
                        // If the conversion is successful, store it in the session
                        Session["RespondantID"] = respondentId;

                        
                    }
                    else
                    {
                        Session["RespondantID"] = null;
                        // If the conversion fails (invalid Respondent ID format)
                        lblErrorMessage.Text = "Please enter a valid Respondent ID to search.";
                    }
                }
                
            }
           

            SearchList.Controls.Clear();
            Session["SelectedValues"] = null;
            // Optionally clear the selected filters list if needed
            selectedFiltersList.Controls.Clear();
            Response.Redirect("~/Default.aspx");
        }




        protected void CollapseButton_Click(object sender, EventArgs e)
        {
            LinkButton collapseButton = (LinkButton)sender;
            string questionId = collapseButton.CommandArgument; // Retrieve the CommandArgument (QuestionId)

            // Find the icon within the LinkButton
            Literal icon = (Literal)collapseButton.FindControl($"icon{questionId}");

            if (icon != null)
            {
                // Toggle the icon
                if (icon.Text.Contains("&#43;")) // If icon is '+'
                {
                    icon.Text = "&#45;"; // Change to '-'
                }
                else
                {
                    icon.Text = "&#43;"; // Change back to '+'
                }
            }
        }
        private void BindGridView(int respondantId)
        {
           
                try
                {
                    // Set the connection string based on the environment
                    if (_surajConnectionString.Equals("dev"))
                    {
                        _surajConnectionString = AppConstant.DevConnectionString;
                    }
                    else if (_surajConnectionString.Equals("Test"))
                    {
                        _surajConnectionString = AppConstant.TestConnectionString;
                    }
                    else
                    {
                        _surajConnectionString = AppConstant.ProductionConnectionString;
                    }

                    // Define the query
                    string query = @"
                        SELECT 
                            r.respondant_id,
                            m.givenName AS FirstName,
                            m.lastName AS LastName,
                            DATEDIFF(YEAR, m.DOB, GETDATE()) - 
                            CASE
                                WHEN MONTH(m.DOB) > MONTH(GETDATE()) OR 
                                     (MONTH(m.DOB) = MONTH(GETDATE()) AND DAY(m.DOB) > DAY(GETDATE())) 
                                THEN 1
                                ELSE 0
                            END AS Age,
                            q.question AS Question,
                            a.answer_text AS Answer,
                            a.answerOrder AS AnswerOrder
                        FROM 
                            respondant r
                        Left JOIN 
                            member m ON r.respondant_id = m.respondant_id
                        LEFT JOIN 
                            answer a ON a.respondant_id = r.respondant_id
                        LEFT JOIN 
                            question q ON q.id = a.question_id
                        LEFT JOIN 
                            options o ON o.id = a.option_id
                        WHERE 
                            r.respondant_id = @respondantId
                        ORDER BY 
                             m.lastName ASC;";

                    // Create a DataTable to hold the results
                    DataTable dt = new DataTable();

                    // Prepare connection and command
                    using (SqlConnection conn = new SqlConnection(_surajConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // Add the parameter to the SQL command
                            cmd.Parameters.AddWithValue("@respondantId", respondantId);
                            // Open the connection
                            conn.Open();

                            // Fill the DataTable
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);

                            // Bind the GridView if data is available
                            if (dt.Rows.Count > 0)
                            {
                                GridView1.DataSource = dt;
                                GridView1.DataBind();
                                lblErrorMessage.Text = $"{dt.Rows.Count} matching result(s) found for Respoondant ID = {respondantId}."; // Display row count
                            }
                            else
                            {
                                // Handle no data found
                                lblErrorMessage.Text = $"No matching results Respoondant ID = {respondantId}.";
                                GridView1.DataSource = null;
                                GridView1.DataBind();
                            }
                        }
                    }
                }
                catch (QuestionValidationException ex)
                {

                    lblErrorMessage.Text = "ERROR " + ex.Message;

                }
            
            
            
        }

        private void BindGridView()
        {
            
            
                try
                {
                    // Set the connection string based on the environment
                    if (_surajConnectionString.Equals("dev"))
                    {
                        _surajConnectionString = AppConstant.DevConnectionString;
                    }
                    else if (_surajConnectionString.Equals("Test"))
                    {
                        _surajConnectionString = AppConstant.TestConnectionString;
                    }
                    else
                    {
                        _surajConnectionString = AppConstant.ProductionConnectionString;
                    }

                    // Define the query
                    string query = @"
                        SELECT 
                            r.respondant_id,
                            m.givenName AS FirstName,
                            m.lastName AS LastName,
                            DATEDIFF(YEAR, m.DOB, GETDATE()) - 
                            CASE
                                WHEN MONTH(m.DOB) > MONTH(GETDATE()) OR 
                                     (MONTH(m.DOB) = MONTH(GETDATE()) AND DAY(m.DOB) > DAY(GETDATE())) 
                                THEN 1
                                ELSE 0
                            END AS Age,
                            q.question AS Question,
                            a.answer_text AS Answer,
                            a.answerOrder AS AnswerOrder
                        FROM 
                            respondant r
                        Left JOIN 
                            member m ON r.respondant_id = m.respondant_id
                        LEFT JOIN 
                            answer a ON a.respondant_id = r.respondant_id
                        LEFT JOIN 
                            question q ON q.id = a.question_id
                        LEFT JOIN 
                            options o ON o.id = a.option_id

                        ORDER BY 
                             m.lastName ASC;";

                    // Create a DataTable to hold the results
                    DataTable dt = new DataTable();

                    // Prepare connection and command
                    using (SqlConnection conn = new SqlConnection(_surajConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            
                            // Open the connection
                            conn.Open();

                            // Fill the DataTable
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);

                            // Bind the GridView if data is available
                            if (dt.Rows.Count > 0)
                            {
                                GridView1.DataSource = dt;
                                GridView1.DataBind();
                                lblErrorMessage.Text = $"{dt.Rows.Count} matching result(s) found."; // Display row count
                            }
                            else
                            {
                                // Handle no data found
                                lblErrorMessage.Text = "No matching results found.";
                                GridView1.DataSource = null;
                                GridView1.DataBind();
                            }
                        }
                    }
                }
                catch (QuestionValidationException ex)
                {

                    lblErrorMessage.Text = "ERROR " + ex.Message;

                }
            
            

        }

 
        private void BindGridViewForSearch(RealQuestion question, List<SelectedValues> selectedValuesList)
        {
            try
            {
                // Get selected values
                List<string> selectedValues = selectedValuesList.Select(sv => sv.UserSelectedValues).ToList();

                if (selectedValues == null || selectedValues.Count == 0)
                {
                    lblErrorMessage.Text = "No selection made.";
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    return;
                }

                // Start constructing the base SQL query
                string query = @"
                    SELECT 
                             r.respondant_id,
                             m.givenName AS FirstName,
                             m.lastName AS LastName,
                             DATEDIFF(YEAR, m.DOB, GETDATE()) - 
                                 CASE
                                     WHEN MONTH(m.DOB) > MONTH(GETDATE()) OR 
                                          (MONTH(m.DOB) = MONTH(GETDATE()) AND DAY(m.DOB) > DAY(GETDATE())) 
                                     THEN 1
                                     ELSE 0
                                 END AS Age,
                             q.question AS Question,
                             a.answer_text AS Answer,
                             a.answerOrder AS AnswerOrder
                         FROM 
                             respondant r
                         LEFT JOIN 
                             member m ON r.respondant_id = m.respondant_id
                         LEFT JOIN 
                             answer a ON a.respondant_id = r.respondant_id
                         LEFT JOIN 
                             question q ON q.id = a.question_id
                         WHERE 
                             a.answer_text IN ({0})
                        
                         ORDER BY 
                             m.lastName ASC, r.respondant_id;";

                // Create parameter placeholders for each condition in selectedValues
                var parameterNames = new List<string>();
                for (int i = 0; i < selectedValues.Count; i++)
                {
                    parameterNames.Add("@param" + i);
                }

                // Replace the placeholder in the query with the parameter names
                query = string.Format(query, string.Join(", ", parameterNames));

                using (SqlConnection conn = new SqlConnection(_surajConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Add parameters for all selected values
                    for (int i = 0; i < selectedValues.Count; i++)
                    {
                        cmd.Parameters.AddWithValue(parameterNames[i], selectedValues[i]);
                    }

                    conn.Open();

                    // Create a DataTable to hold the results
                    DataTable dt = new DataTable();

                    // Use SqlDataAdapter to fill the DataTable
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    // Count the occurrences of each respondant_id in the DataTable
                    var respondentIdCount = new Dictionary<int, int>();

                    // Loop through the DataTable to count the occurrences of each respondent_id
                    foreach (DataRow row in dt.Rows)
                    {
                        int respondentId = Convert.ToInt32(row["respondant_id"]);

                        // Increment the count for this respondent_id
                        if (respondentIdCount.ContainsKey(respondentId))
                        {
                            respondentIdCount[respondentId]++;
                        }
                        else
                        {
                            respondentIdCount[respondentId] = 1;
                        }
                    }

                    // Create a new list to hold the rows that match the criteria
                    DataTable filteredDt = dt.Clone(); // Create a copy of the DataTable schema to hold the matching rows

                    // Now check if the respondent_id appears exactly selectedValues.Count times
                    foreach (DataRow row in dt.Rows)
                    {
                        int respondentId = Convert.ToInt32(row["respondant_id"]);

                        // If the respondent_id appears exactly selectedValues.Count times, add to the filtered DataTable
                        if (respondentIdCount[respondentId] == selectedValues.Count)
                        {
                            filteredDt.ImportRow(row); // Add the row to the filtered DataTable
                        }
                    }

                    // Bind the filtered data to the GridView
                    if (filteredDt.Rows.Count > 0)
                    {
                        GridView1.DataSource = filteredDt;
                        GridView1.DataBind();
                        lblErrorMessage.Text = $"{filteredDt.Rows.Count} matching result(s) found.";
                    }
                    else
                    {
                        lblErrorMessage.Text = "No matching results found.";
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                    }
                }

            }
            catch (QuestionValidationException ex)
            {
                lblErrorMessage.Text = "ERROR " + ex.Message;
            }
        }


        protected List<RealQuestion> Load_Question_with_Option()
        {
            List<RealQuestion> listRealQuestion = new List<RealQuestion>();
            // Add the Respondent ID question at the start of the list
            RealQuestion respondentIdQuestion = new RealQuestion
            {
                QuestionId = -1, // Unique identifier for the custom Respondent ID question
                QuestionText = "Respondent ID", // The label for the Respondent ID input
                QuestionType = "TextBox", // Indicates this is a TextBox input
                QuestionOrder = 0, // Ensures this question appears first
                QuestionDisable = 0, // 0 means the question is enabled by default
                OptionAllowed = 0, // No options are allowed since it's a TextBox
                QuestionRequired = true, // Respondent ID is mandatory
                OptionId = new List<int?> { null }, // Initializes with one null entry
                OptionText = new List<string> { string.Empty }, // Initializes with one empty string
                RelatedQuestion = new List<bool> { false } // Initializes with one false entry
            };
            listRealQuestion.Add(respondentIdQuestion); // Insert at the beginning
            RealQuestion currentRealQuestion = null;  // To track the current real question

            try
            {
                // Determine connection string based on the environment
                switch (surajConnectionString)
                {
                    case "dev":
                        surajConnectionString = AppConstant.DevConnectionString;
                        break;
                    case "Test":
                        surajConnectionString = AppConstant.TestConnectionString;
                        break;
                    default:
                        surajConnectionString = AppConstant.ProductionConnectionString;
                        break;
                }

                // Query to fetch question and option details
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
                                WHERE question.type IN ('RadioButtonList', 'DropDownList', 'CheckBoxList')
                                ORDER BY question.id";

                // Open database connection and execute the query
                using (SqlConnection conn = new SqlConnection(surajConnectionString))
                {
                    conn.Open();
                    SqlCommand myCommand = new SqlCommand(query, conn);

                    using (SqlDataReader reader = myCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int questionId = Convert.ToInt32(reader["QuestionId"]);

                                // If QuestionId changes, add the previous question to the list
                                if (currentRealQuestion != null && currentRealQuestion.QuestionId != questionId)
                                {
                                    listRealQuestion.Add(currentRealQuestion);  // Add previous question
                                    currentRealQuestion = new RealQuestion(); // Start new RealQuestion
                                }

                                // Initialize currentRealQuestion if it's null
                                if (currentRealQuestion == null)
                                {
                                    currentRealQuestion = new RealQuestion();
                                }

                                currentRealQuestion.QuestionId = Convert.ToInt32(reader["QuestionId"]);
                                currentRealQuestion.QuestionText = reader["QuestionText"].ToString();
                                currentRealQuestion.QuestionType = reader["QuestionType"].ToString();
                                currentRealQuestion.QuestionOrder = reader["QuestionOrder"] != DBNull.Value
                                                                                        ? Convert.ToInt32(reader["QuestionOrder"])
                                                                                        : 0;
                                currentRealQuestion.QuestionDisable = Convert.ToInt32(reader["questionEnable"]);
                                currentRealQuestion.OptionAllowed = Convert.ToInt32(reader["OptionAllowed"]); // Assign OptionAllowed
                                currentRealQuestion.QuestionRequired = Convert.ToBoolean(reader["QuestionRequire"]);
                                // Safely add OptionId, checking for DBNull and assigning null if it is DBNull
                                currentRealQuestion.OptionId.Add(reader["OptionId"] != DBNull.Value
                                    ? (int?)Convert.ToInt32(reader["OptionId"])  // Convert to nullable int if not DBNull
                                    : null);  // Assign null if DBNull

                                // Safely add OptionText, checking for DBNull
                                currentRealQuestion.OptionText.Add(reader["OptionText"] != DBNull.Value
                                    ? reader["OptionText"].ToString()
                                    : string.Empty); // Use empty string as default if null

                                // Safely add RelatedQuestion, checking for DBNull
                                currentRealQuestion.RelatedQuestion.Add(reader["RelatedQuestion"] != DBNull.Value
                                    ? Convert.ToBoolean(reader["RelatedQuestion"])
                                    : false); // Use false as default if null
                            }

                            // Add the last question to the list
                            if (currentRealQuestion != null)
                            {
                                listRealQuestion.Add(currentRealQuestion);
                            }
                        }
                    }
                }
               
            }
            catch (QuestionValidationException ex)
            {
                lblErrorMessage.Text = "Internal error, please contact Suraj. Error details: " + ex.Message;
               
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Internal error, please contact the system admin. Error details: " + ex.Message;
            }

            return listRealQuestion;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void logOutButton_Click(object sender, EventArgs e)
        {
            // Optionally, abandon the session entirely if needed
            Session.Abandon();
            Response.Redirect("~/FirstPage.aspx");
        }

        

        protected void btnClearAll_Click(object sender, EventArgs e)
        {

            SearchList.Controls.Clear();
            Session["SelectedValues"] = null;
            Session["RespondantID"] = null;


            // Optionally clear the selected filters list if needed
            selectedFiltersList.Controls.Clear();
            Response.Redirect("~/Default.aspx");
        }

        // Event handler when a filter is selected

        protected void UpdateSelectedFiltersDisplay(List<SelectedValues> selectedValuesList)
        {
            // Clear the existing controls
            selectedFiltersList.Controls.Clear();

            // Group selected values by question for better display
            var groupedFilters = selectedValuesList.GroupBy(sv => sv.QuestionId);

            foreach (var group in groupedFilters)
            {
                // Create a container for each question's filters with horizontal layout
                Panel questionPanel = new Panel { CssClass = " d-flex flex-row align-items-center" };

                foreach (var value in group)
                {
                    // Create a container for each filter item
                    Panel filterItem = new Panel { CssClass = "d-flex flex-row align-items-center m-1" };

                    // Add a label for each selected value
                    Label filterLabel = new Label
                    {
                        Text = value.UserSelectedValues,
                        CssClass = "bg-primary text-white fw-bold px-2 py-1 rounded m-1"
                    };

                   

                    // Add the label and remove button to the filterItem container
                    filterItem.Controls.Add(filterLabel);
                    

                    // Add the filterItem to the questionPanel container
                    questionPanel.Controls.Add(filterItem);
                }

                // Add the questionPanel to the selectedFiltersList container
                selectedFiltersList.Controls.Add(questionPanel);
            }
        }


        

    }
}