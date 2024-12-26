using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class StaffLogin : System.Web.UI.Page
    {
        string connectionString = "Data Source=SQL5112.site4now.net;Initial Catalog=db_9ab8b7_324dda12247;User Id=db_9ab8b7_324dda12247_admin;Password=csF4ChaS;";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StaffLogin"] == null)
            {
                Response.Redirect("~/Firstpage.aspx");
            }
            // Check if the user is already logged in. If yes, redirect to Default.aspx
            if (Session["SucessfullLogin"] != null && (bool)Session["SucessfullLogin"] == true)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        

        protected void buttonLogin_Click(object sender, EventArgs e)
        {
            // username: "suraj", password: "suraj123"
            
            /*  'INSERT INTO staff (username, password, registeredAt)
            VALUES('newstaff', CONVERT(VARCHAR(MAX), HASHBYTES('SHA2_256', 'newpassword123'), 2), GETDATE());' - run this into the sql schema to add another data */
            string userUsername = username.Text;
            string userPassword = password.Text;
            // Authenticate the staff member
            bool isAuthenticated = AuthenticateStaff(userUsername, userPassword);

            if (isAuthenticated)
            {
                // On successful login, store session and redirect
                Session["SucessfullLogin"] = true;
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                // Show error message if authentication failed
                CustomValidator customValidator = new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "Invalid username or password. Please try again.",
                    //ValidationGroup = "StaffLoginGroup"
                };
                Page.Validators.Add(customValidator);
            }

            
        }
        private bool AuthenticateStaff(string username, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // SQL query to retrieve the staff's stored password hash
                    string query = "SELECT password FROM staff WHERE username = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        string storedPasswordHash = null;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                storedPasswordHash = reader["password"].ToString();
                            }
                        }

                        if (storedPasswordHash != null)
                        {
                            // Hash the entered password and compare it to the stored hash
                            string hashedPassword = HashPassword(password);
                            this.password.Text = hashedPassword;
                            return hashedPassword == storedPasswordHash;
                            

                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., database connection errors)
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));  // Convert each byte to a hex string
                }
                return sb.ToString().ToUpper();
            }
        }
    }
}