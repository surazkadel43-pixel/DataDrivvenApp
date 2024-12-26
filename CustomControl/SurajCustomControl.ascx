<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurajCustomControl.ascx.cs" Inherits="WebApplication1.SurajTextbox" %>

<!-- Bootstrap styled Label -->
<asp:Label ID="LabelName" runat="server" CssClass="form-label" Text="Label:  "></asp:Label>

<!-- Bootstrap styled TextBox -->
<asp:TextBox ID="TextBoxName" runat="server" CssClass="form-control" OnTextChanged="TextBoxName_TextChanged" AutoPostBack="true"></asp:TextBox>