<%@ Page Language="C#" AutoEventWireup="true" Inherits="Codebehind.Default" src="Default.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Register User</title>
</head>
<body>
   <form id="RegisterForm" runat="server">
        <asp:textbox id="firstNameBox" text="First Name" runat="server"></asp:textbox>
        <asp:textbox id="lastNameBox" text="Last Name" runat="server"></asp:textbox>
        <asp:textbox id="userNameBox" text="userName" runat="server"></asp:textbox>
        <asp:textbox id="PhoneNumberBox" text="Phone Number" runat="server"></asp:textbox>
        <asp:textbox id="CarrierBox" text="Phone Carrier" runat="server"></asp:textbox>
        <asp:textbox id="passwordBox" text="Password" runat="server"></asp:textbox>
        <asp:button id="register" text="Register User" Onclick="Register_Click" runat="server"></asp:button>
        <!--<asp:label id="FirstLabel" runat="server" /> -->
    <div>
    
    </div>
    </form>
 
</body>
</html>
