<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberLogin.aspx.cs" Inherits="WebApplication1.MemberLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" /> 
   <meta name="viewport" content="width=device-width, initial-scale=1" />
<title>Practice Bootstrap </title>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
 <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css"/>
    <style>
    
 </style>
</head>
<body class="bg-light">
    
    <div class="bg-primary mb-3 p-5 text-center text-light" >
         <div class="display-2"> Welcome To The AITr </div>
         
    </div>
    
    <div class="container-lg bg-info border border-danger border-2 ">
        <!-- Member Register -->
        <div class="row bg-light p-3 m-3 align-items-center justify-content-center ">
            <div  class="col-6 col-sm-8  border border-2 border-danger  p-3 m-3" >
                  <h5 class="display-3 text-center ">Welcome Member </h5>
                <form id="registerMember" runat="server" class=" m-3 p-3">
                    <div class="form-group mb-3">
                      <label for="firstName">Given Name: </label>
                      <asp:TextBox ID="firstName"  runat="server" class="form-control"></asp:TextBox>
                   </div>
                     <div class="form-group mb-3">
                        <label for="lasttName">Last Name: </label>
                        <asp:TextBox ID="lastName"  runat="server" class="form-control"></asp:TextBox>
                     </div>
                    <div class="form-group mb-3">
                        
                         <label for="txtdateOfBirth">Your Date of Birth:  </label>
                        <asp:Calendar ID="dateOfBirthPicker" runat="server" OnSelectionChanged="dateOfBirthPicker_SelectionChanged" Visible="False" BackColor="White" BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" Height="200px" Width="220px">
                            <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                            <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                            <OtherMonthDayStyle ForeColor="#999999" />
                            <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                            <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                            <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                            <WeekendDayStyle BackColor="#CCCCFF" />
                         </asp:Calendar>
                        <asp:TextBox ID="txtdateOfBirth"  runat="server" class="form-control"></asp:TextBox>
                        <asp:LinkButton ID="linkButtondateOfBirth" runat="server" class="btn btn-outline-primary mt-2" OnClick="linkButtondateOfBirth_Click"><i class="bi bi-calendar"></i></asp:LinkButton>

                    </div>
                     <div class="form-group mb-3">
                        <label for="phoneNumber">Contact Number: </label>
                        <asp:TextBox ID="phoneNumber"  runat="server" class="form-control"></asp:TextBox>
                     </div>
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

        <!--Member Register-->
          

        
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
