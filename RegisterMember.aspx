<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterMember.aspx.cs" Inherits="WebApplication1.RegisterMember" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" /> 
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Practice Bootstrap</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css"/>
</head>
<body class="bg-light">
    <div class="bg-primary mb-3 p-5 text-center text-light">
        <div class="display-2"> Welcome To The AITr </div>
    </div>
    <div class="container-lg bg-info border border-danger border-2">
        <!-- Member Register -->
        <div class="row bg-light p-3 m-3 align-items-center justify-content-center">
            <div class="col-6 col-sm-8 border border-2 border-danger p-3 m-3">
                <asp:Label ID="header" runat="server" class="display-3 text-center" Text="Label">Register as a Member</asp:Label>
                <form id="registerMember" runat="server" class="m-3 p-3">
                    <asp:ValidationSummary ID="validationSummary" runat="server" CssClass="alert alert-danger" HeaderText="Please fix the following errors:" />
                    
                    <div class="form-group mb-3">
                        <label for="firstName">Given Name:</label>
                        <asp:TextBox ID="firstName" runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="firstName" ErrorMessage="First Name is required." CssClass="text-danger" />
                        <asp:RegularExpressionValidator ID="revFirstName" runat="server" ControlToValidate="firstName" ErrorMessage="First Name must contain only letters." CssClass="text-danger" ValidationExpression="^[A-Za-z]+$" />
                    </div>

                    <div class="form-group mb-3">
                        <label for="lastName">Last Name:</label>
                        <asp:TextBox ID="lastName" runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="lastName" ErrorMessage="Last Name is required." CssClass="text-danger" />
                        <asp:RegularExpressionValidator ID="revLastName" runat="server" ControlToValidate="lastName" ErrorMessage="Last Name must contain only letters." CssClass="text-danger" ValidationExpression="^[A-Za-z]+$" />
                    </div>

                    <div class="form-group mb-3">
                        <label for="txtdateOfBirth">Your Date of Birth:</label>
                        <asp:Calendar ID="Calendar1" runat="server" Visible="False" OnSelectionChanged="DOBPicker_SelectionChanged" BackColor="White" BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" Height="200px" Width="220px">
                            <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                            <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                            <OtherMonthDayStyle ForeColor="#999999" />
                            <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                            <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                            <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                            <WeekendDayStyle BackColor="#CCCCFF" />
                        </asp:Calendar>
                        <asp:TextBox ID="txtdateOfBirth" runat="server" class="form-control" placeholder="YYYY-MM-DD"></asp:TextBox>
                        <asp:Button ID="btnShowCalendar" runat="server" Text="Date" CssClass="btn btn-primary mt-1" OnClick="btnShowCalendar_Click" AutoPostBack="false" CausesValidation="false" />
                        <asp:RequiredFieldValidator ID="rfvDOB" runat="server" ControlToValidate="txtdateOfBirth" ErrorMessage="Date of Birth is required." CssClass="text-danger" />
                        <asp:RegularExpressionValidator ID="revDOB" runat="server" ControlToValidate="txtdateOfBirth" ErrorMessage="Enter a valid date in YYYY-MM-DD format." CssClass="text-danger" ValidationExpression="^\d{4}-\d{2}-\d{2}$" />
                    </div>

                    <div class="form-group mb-3">
                        <label for="phoneNumber">Contact Number:</label>
                        <asp:TextBox ID="phoneNumber" runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="phoneNumber" ErrorMessage="Phone number is required." CssClass="text-danger" />
                        <asp:CompareValidator ID="cvPhoneNumber" runat="server" ControlToValidate="phoneNumber" Operator="DataTypeCheck" Type="Integer" ErrorMessage="Phone number must be numeric." CssClass="text-danger" />
                        <asp:RegularExpressionValidator ID="revPhoneNumber" runat="server" ControlToValidate="phoneNumber" ErrorMessage="Phone number must be exactly 10 digits." CssClass="text-danger" ValidationExpression="^\d{10}$" />
                    </div>

                    <asp:Button ID="buttonRegister" runat="server" Text="Register" class="btn btn-primary" OnClick="buttonRegister_Click" />
                </form>
                <hr />
                <div class="mt-3 text-center text-3">
                    <p>Back To ? 
                        <asp:HyperLink ID="homePage" runat="server" NavigateUrl="~/SurveyQuestion.aspx" class="btn btn-danger text-center">Survey Page</asp:HyperLink>
                    </p>
                    <asp:HyperLink ID="homeButton" runat="server" Visible="false" NavigateUrl="~/FirstPage.aspx" class="btn btn-danger text-center">Home Page</asp:HyperLink>
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
</body>
</html>
