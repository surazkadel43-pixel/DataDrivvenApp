<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyQuestion.aspx.cs" Inherits="WebApplication1.SurveyQuestion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />

</head>
<body>
    <form id="form1" runat="server">
    <div class="container mt-5">
        <div class="card shadow">
            <div class="card-body">
                <!-- Survey Title -->
                <h3 class="card-title mb-4 text-center text-primary">Survey</h3>

                <!-- Question Title Label -->
                <asp:Label ID="lbltitle" runat="server" CssClass="fs-4 text-secondary mb-4 d-block text-center"></asp:Label>

                

                <!-- Placeholder for Questions -->
                <div class="mb-4">
                    <asp:PlaceHolder ID="QuestionList" runat="server"></asp:PlaceHolder>
                </div>

                <!-- Error Message Label -->
                <div class="mb-3">
                     <asp:Label ID="lblErrorMessage" runat="server" CssClass="text-danger small border border-primary p-1"></asp:Label>
                </div>
                <!-- Navigation Buttons -->
                <div class="d-flex justify-content-between">
                    <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btn-outline-secondary" OnClick="btnPrevious_Click" CausesValidation="True" />
                    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="btnNext_Click" CausesValidation="True"/>
                </div>
                <!-- Submit Anonymously or Register & Submit Button (Initially hidden) -->
                <div class="mt-4 text-center">
                    <asp:Button ID="btnSubmitAnonymously" runat="server" Text="Submit Anonymously" CssClass="btn btn-success" OnClick="btnSubmitAnonymously_Click" Visible="False" />
                    <asp:Button ID="btnRegisterAndSubmit" runat="server" Text="Register & Submit" CssClass="btn btn-success" OnClick="btnRegisterAndSubmit_Click" Visible="False" />
                </div>
            </div>
        </div>
    </div>
</form>
</body>
</html>
