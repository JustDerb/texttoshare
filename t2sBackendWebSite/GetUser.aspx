<%@ Page Language="C#" AutoEventWireup="true" Inherits="users.GetUser" src="GetUser.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <form id ="QueryUsers" runat = "server">
        <asp:textbox id="rFirstNamebox" text="First Name" runat="server"></asp:textbox>
        <asp:textbox id="rLastNameBox" text="Last Name" runat="server"></asp:textbox>
        <asp:textbox id="ruserNameBox" text="userName" runat="server"></asp:textbox>
        <asp:textbox id="rPhoneNumber" text="Phone Number" runat="server"></asp:textbox>
        <asp:textbox id="rCarrierBox" text="Phone Carrier" runat="server"></asp:textbox>
        <asp:button id="getUser" text="Get User" Onclick="getUser_Click" runat="server"></asp:button>
   
    <div>
    
    </div>
         </form>
</body>
</html>
