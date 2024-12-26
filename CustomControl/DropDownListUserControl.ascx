<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDownListUserControl.ascx.cs" Inherits="WebApplication1.CustomControl.DropDownListUserControl" %>
<div class="mt-4 p-3 border rounded shadow-sm">
    <!-- Question Label -->
    <asp:Label ID="dropDownQuestionName" runat="server" CssClass="fw-bold fs-5 text-primary d-block mb-3"></asp:Label>

    <!-- DropDownList -->
    <asp:DropDownList ID="dropDownListUC" runat="server" CssClass="form-select" aria-label="Select an option"   >
        </asp:DropDownList> <!-- Options will be added dynamically in code-behind -->

   

    
</div>