<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextboxUserControl.ascx.cs" Inherits="WebApplication1.CustomControl.TextboxUserControl" %>
<div class="mt-4 p-3 border rounded shadow-sm">
    <!-- Question Label -->
    <asp:Label ID="questionText" runat="server" CssClass="fw-bold fs-5 text-primary d-block mb-3"></asp:Label>

    <!-- TextBox -->
    <asp:TextBox ID="textBoxUC" runat="server" CssClass="form-control" placeholder="Enter your answer here"></asp:TextBox>

    <!-- Required Field Validator -->
    <%--<asp:RequiredFieldValidator 
        ID="TextBoxValidator" 
        runat="server" 
        ControlToValidate="textBoxUC" 
        ErrorMessage="This field is required." 
        ForeColor="Red" 
        CssClass="text-danger mt-2" />--%>
</div>