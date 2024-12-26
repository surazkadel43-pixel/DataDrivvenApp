using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Classes;

namespace WebApplication1
{
    public partial class RegisterMember : System.Web.UI.Page
    {
       
        string connectionString = "Data Source=SQL5112.site4now.net;Initial Catalog=db_9ab8b7_324dda12247;User Id=db_9ab8b7_324dda12247_admin;Password=csF4ChaS;";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["Surveying"] == null)
                {
                    Response.Redirect("~/Firstpage.aspx");
                }
            }

        }

        protected void buttonRegister_Click(object sender, EventArgs e)
        {
            // Retrieve the values from the form controls
            string firstNameValue = firstName.Text;
            string lastNameValue = lastName.Text;
            string dobValue = txtdateOfBirth.Text;
            string phoneNumberValue = phoneNumber.Text;
            // Get the IP address of the client
            string clientIpAddress = GetClientIpAddress();

            // Insert the respondent and get the respondent_id
            int respondentId = InsertRespondent(clientIpAddress);
            if (respondentId > 0)
            {
                // Assuming answers are stored in session
                List<Answer> answers = Session["Answers"] as List<Answer>;

                // Save the answers to the database
                SaveAnswersToDatabase(respondentId, answers);

                // Optionally, save member data here (e.g. firstName, lastName, dob, phone)
                InsertMember(firstNameValue, lastNameValue, dobValue, phoneNumberValue, respondentId);

                HideFormAndShowThankYou();

                //Response.Redirect("~/RegisterMember.aspx");
            }
            else
            {
                // Add the exception message to ValidationSummary via CustomValidator
                CustomValidator customValidator = new CustomValidator
                {
                    IsValid = false, // Marks the form as invalid
                    ErrorMessage = "Did not found Respondant", // Sets the error message to the exception's message
                    
                };

                // Add the custom validator to the page's validators collection
                Page.Validators.Add(customValidator);
               
            }

           // Response.Redirect("~/RegisterMember.aspx");
        }
        protected void InsertMember(string firstName, string lastName, string dob, string phone, int respondantId)
        {
            try
            {
                string insertMemberQuery = @"
            INSERT INTO member (givenName, lastName, DOB, phone, respondant_id)
            VALUES (@GivenName, @LastName, @DOB, @Phone, @RespondantId)";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insertMemberQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@GivenName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@DOB", DateTime.Parse(dob));  // Ensure DOB is in Date format
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@RespondantId", respondantId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Add the exception message to ValidationSummary via CustomValidator
                CustomValidator customValidator = new CustomValidator
                {
                    IsValid = false, // Marks the form as invalid
                    ErrorMessage = ex.Message, // Sets the error message to the exception's message
                    
                };

                // Add the custom validator to the page's validators collection
                Page.Validators.Add(customValidator);
            }
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
                // Add the exception message to ValidationSummary via CustomValidator
                CustomValidator customValidator = new CustomValidator
                {
                    IsValid = false, // Marks the form as invalid
                    ErrorMessage = ex.Message, // Sets the error message to the exception's message
                   
                };

                // Add the custom validator to the page's validators collection
                Page.Validators.Add(customValidator);
            }
        }


        protected int InsertRespondent(string clientIpAddress)
        {
            //string connectionString = "Data Source=SQL5112.site4now.net;Initial Catalog=db_9ab8b7_324dda12247;User Id=db_9ab8b7_324dda12247_admin;Password=csF4ChaS;";
            int respondentId = 0;
            try
            {
                

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
                // Add the exception message to ValidationSummary via CustomValidator
                CustomValidator customValidator = new CustomValidator
                {
                    IsValid = false, // Marks the form as invalid
                    ErrorMessage = ex.Message, // Sets the error message to the exception's message
                    
                };

                // Add the custom validator to the page's validators collection
                Page.Validators.Add(customValidator);
            }


            return respondentId;
        }

        
        private void HideFormAndShowThankYou()
        {
            // Hide the registration form entirely
            registerMember.Visible = false;


            homePage.Text = "Home Page";
            homePage.NavigateUrl = "~/Firstpage.aspx";
            
            // Show the success message
            header.Text = "Thank you for participating in the research!";
            header.Visible = true; // Show the success message
            
            firstName.Text = "";
            lastName.Text = "";
            txtdateOfBirth.Text = "";
            phoneNumber.Text = "";

            // Clear the session after saving the data
            Session.Abandon();

        }

        protected void DOBPicker_SelectionChanged(object sender, EventArgs e)
        {
            // Set the selected date into the txtdateOfBirth TextBox
            txtdateOfBirth.Text = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
            // Hide the calendar after a date is selected
            Calendar1.Visible = false;
        }

        // Button click event to show the calendar
        protected void btnShowCalendar_Click(object sender, EventArgs e)
        {
            // Toggle the visibility of the calendar
            Calendar1.Visible = !Calendar1.Visible;
        }
    }
}