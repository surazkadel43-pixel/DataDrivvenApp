<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckBoxListUserControl.ascx.cs" Inherits="WebApplication1.CustomControl.CheckBoxListUserControl" %>
<div class="mt-4 p-3 border rounded shadow-sm">
    <!-- Question Label -->
    <asp:Label ID="questionValueLabel" runat="server" CssClass="fw-bold fs-5 text-primary d-block mb-3"></asp:Label>

    <!-- HiddenField to track selected checkboxes -->
    <asp:HiddenField ID="hfSelectedOptions" runat="server" />

    <!-- CheckBox List -->
    <asp:CheckBoxList ID="optionsCheckBoxList" runat="server" RepeatLayout="Flow" CssClass="list-group"></asp:CheckBoxList>
        <!-- Custom list item styling via a loop in code-behind -->
    

    
</div>