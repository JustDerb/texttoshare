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
    <form>
        <asp:textbox id="rFirstNamebox" text="First Name" runat="server"></asp:textbox>
        <asp:textbox id="rLastNameBox" text="Last Name" runat="server"></asp:textbox>
        <asp:textbox id="ruserNameBox" text="userName" runat="server"></asp:textbox>
        <asp:textbox id="rPhoneNumber" text="Phone Number" runat="server"></asp:textbox>
        <asp:textbox id="rCarrierBox" text="Phone Carrier" runat="server"></asp:textbox>
        <asp:button id="getUser" text="Get User" Onclick="getRegister_Click" runat="server"></asp:button>


    </form>


</body>
</html>
