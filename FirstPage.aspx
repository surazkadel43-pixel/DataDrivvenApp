<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FirstPage.aspx.cs" Inherits="WebApplication1.RegisterPage1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" /> 
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Practice Bootstrap</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
</head>
<body class="bg-light">
    <!-- Begin Form -->
    <form id="form1" runat="server">
        <!-- Top Header -->
        <div class="bg-primary mb-3 p-3 text-center text-light">
            <div class="display-5">Welcome To The AITr</div>
        </div>

        <!-- Container -->
        <div class="container-lg bg-info border border-danger border-2">
            <!-- Research Session -->
            <div class="row bg-light p-3 mb-3 align-items-center justify-content-center">
                <div class="col-9 border border-2 border-danger">
                    <div class="card border-danger border-2">
                        <div class="card-body text-center py-4">
                            <div class="card-title">Attend The Research Session</div>
                            <div class="lead card-subtitle mb-2 text-body-secondary">Join the Session Here</div>
                            
                            <!-- LinkButton inside form -->
                            <asp:LinkButton ID="researchSession" runat="server" OnClick="researchSession_Click" class="btn btn-primary text-center">Click Here</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Become a Member Section -->
            <hr />
            <div class="row bg-light p-3 mb-3 align-items-center justify-content-center">
                <div class="col-9 border border-2 border-danger">
                    <div class="card border-primary border-2">
                        <div class="card-body text-center py-5">
                            <p class="card-title text-danger">Staff Login</p>
                            <p class="lead card-subtitle mb-2 text-body-secondary">If you are a Staff, Login as a Staff</p>
                            
                            <!-- Staff LinkButton -->
                            <asp:LinkButton ID="staffLogin" runat="server" NavigateUrl="~/StaffLogin.aspx" class="btn btn-danger text-center" OnClick="staffLogin_Click">Staff</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
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
    </form>
    <!-- End Form -->
</body>
</html>
