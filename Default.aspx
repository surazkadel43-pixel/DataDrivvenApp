<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">
    <meta charset="utf-8" />
    <title>Staff Respondent Search</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style>
        body {
            background-color: #f8f9fa; /* Light background */
        }
        .header {
            background-color: #007bff; /* Primary blue color */
            color: white;
            padding: 20px;
            border-radius: 5px;
            text-align: center;
            margin-bottom: 20px;
        }
        .filter-section {
            padding: 10px;
            background: white;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }
        .results-section {
            padding: 10px;
            background-color: white;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid border border-3 border-primary mt-5 p-3">
            <div class="header">
                <h2>Staff Respondent Search</h2>
                <asp:LinkButton ID="logOutButton" runat="server" NavigateUrl="~/FirstPage.aspx" class="btn btn-danger text-center" OnClick="logOutButton_Click">Logout</asp:LinkButton>
            </div>
            
            <!-- Selected Filters Section -->
            <div class="row mb-3">
                <div class="col-12">
                    
                    <asp:Panel ID="selectedFiltersList" runat="server" CssClass="selected-filters p-2 d-flex flex-row align-items-center" ></asp:Panel> <!-- To display selected filters with cross icons -->
                    <!-- Clear All Button -->
                    <asp:Button ID="btnClearAll" runat="server" Text="Clear All" CssClass="btn btn-warning mt-2" OnClick="btnClearAll_Click" />
                </div>
            </div>

            <div class="row justify-content-center">
                <div class="col-md-4 col-lg-3 filter-section">
                    <h4>Search Filters</h4>
                    <!-- Placeholder for Filter Section -->
                    <div class="mb-4">
                        <asp:PlaceHolder ID="SearchList" runat="server"></asp:PlaceHolder>
                    </div>

                    
                </div>

                <div class="col-md-8 col-lg-8 results-section">
                    <h4>Search Results</h4>
                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="text-danger small border border-primary p-1 mb-2"></asp:Label>
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped" />
                </div>
            </div>
        </div>
    </form>


    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    
</body>

</html>
