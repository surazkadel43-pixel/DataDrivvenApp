<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RadioButtonListUserControl.ascx.cs" Inherits="WebApplication1.CustomControl.RadioButtonListUserControl" %>
<div class="mt-4 p-3 border rounded shadow-sm">
    <!-- Question Label -->
    <asp:Label ID="radioQuestionLabel" runat="server" CssClass="fw-bold fs-5 text-primary d-block mb-3"></asp:Label>

    <!-- Radio Button List -->
    <asp:RadioButtonList ID="radioButtonListUC" runat="server" RepeatLayout="Flow" CssClass="list-group">
       
    </asp:RadioButtonList>

    <%--<!-- Required Field Validator -->
    <asp:RequiredFieldValidator 
        ID="RadioButtonValidator" 
        runat="server" 
        ControlToValidate="radioButtonListUC" 
        InitialValue="" 
        ErrorMessage="Please select an option." 
        ForeColor="Red" 
        CssClass="text-danger mt-2" />--%>
</div>


