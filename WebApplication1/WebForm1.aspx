<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-decoration: underline">
        <b> LINE暱稱:</b>
        <asp:TextBox ID="TextBox1" runat="server" Width="183px"></asp:TextBox>
        <br />
        <br />
        <b>卡號 :</b>
        <asp:TextBox ID="TextBox2" runat="server" Width="188px"></asp:TextBox>
    
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="送出" />
        <br />
        <br />
        <asp:Label ID="Label1" runat="server"></asp:Label>
        <br />
    
    </div>
    </form>
</body>
</html>
