<%@ Page Language="C#" AutoEventWireup="true" Inherits="login.Login" src="login.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <asp:textbox id="userNameBox" placeholder="UserName" runat="server" ></asp:textbox>
        <asp:textbox id="passwordBox" TextMode="Password" placeholder="Password" runat="server" ></asp:textbox>
         <asp:button id="login" Onclick="Login_Click" runat="server" Text="Login"></asp:button>

          <a href="RegisterUser.aspx">Register Here</a>
    <div>
    
    </div>
    </form>
</body>
</html>
