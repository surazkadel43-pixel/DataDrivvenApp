<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffLogin.aspx.cs" Inherits="WebApplication1.StaffLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" /> 
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Practice Bootstrap</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
</head>
<body class="bg-light">
    
    <div class="bg-primary mb-3 p-5 text-center text-light">
        <div class="display-2"> Welcome To The AITr </div>
    </div>
    
    <div class="container-lg bg-info border border-danger border-2">
        <!-- Staff Login -->
        <div class="row bg-light p-3 m-3 align-items-center justify-content-center">
            
            <div class="col-6 border border-2 border-danger p-3 m-3">
                <h5 class="display-3 text-center">Welcome Staff</h5>
                
                <form id="staffLogin" runat="server" class="m-3 p-3">
                    
                    <!-- Validation Summary -->
                    <asp:ValidationSummary ID="validationSummary" runat="server" CssClass="alert alert-danger" HeaderText="Please fix the following errors:" />

                    <!-- Username Field -->
                    <div class="form-group mb-3">
                        <label for="username">Username:</label>
                        <asp:TextBox ID="username" runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="username" ErrorMessage="Username is required." CssClass="text-danger" />
                    </div>

                    <!-- Password Field -->
                    <div class="form-group mb-3">
                        <label for="password">Password:</label>
                        <asp:TextBox ID="password" runat="server" class="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="password" ErrorMessage="Password is required." CssClass="text-danger" />
                    </div>

                    <!-- Login Button -->
                    <asp:Button ID="buttonLogin" runat="server" Text="Login" class="btn btn-primary" OnClick="buttonLogin_Click" />
                </form>

                <hr />
                <div class="mt-3 text-center text-3">
                    <p>Back To Home Page? 
                        <asp:HyperLink ID="homePage" runat="server" NavigateUrl="~/FirstPage.aspx" class="btn btn-danger text-center">Home Page</asp:HyperLink>
                    </p>
                </div>
            </div>
        </div>
        <!-- Staff Login End Here -->
    </div>

    <!-- Footer -->
    <footer class="bg-info text-center text-lg-start mt-5">
        <div class="container p-3">
            <div class="row">
                <div class="col-md-6 mb-3 mb-md-0">
                    <span class="ms-2">&copy; 2024 AITr</span>
                </div>
            </div>
        </div>
    </footer>

</body>
</html>
