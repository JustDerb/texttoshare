<%@ Page Language="C#" AutoEventWireup="true" Inherits="Codebehind.RegisterUser" Src="RegisterUser.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TextToShare - Register</title>
    <link href="Content/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="RegisterForm" class="tts-form" runat="server">
        <h1>Register</h1>
        <p>
            <label for="firstNameBox">First Name</label>
            <asp:TextBox ID="firstNameBox" name="firstNameBox" placeholder="First Name" runat="server" required></asp:TextBox>
        </p>
        <p>
            <label for="lastNameBox">Last Name</label>
            <asp:TextBox ID="lastNameBox" name="lastNameBox" placeholder="Last Name" runat="server" required></asp:TextBox>
        </p>
        <p>
            <label for="userNameBox">Username</label>
            <asp:TextBox ID="userNameBox" name="userNameBox" placeholder="Username" runat="server" required></asp:TextBox>
        </p>
        <p>
            <label for="userNameBox">Phone Number</label>
            <asp:TextBox ID="phoneNumberBox" name="phoneNumberBox" placeholder="Phone Number" runat="server" required></asp:TextBox>
        </p>
        <p>
            <label for="carrierBox">Carrier</label>
            <asp:TextBox ID="carrierBox" name="carrierBox" placeholder="Phone Carrier" runat="server" required></asp:TextBox>
        </p>
        <p>
            <label for="passwordBox">Password</label>
            <asp:TextBox ID="passwordBox" name="passwordBox" TextMode="Password" placeholder="Password" runat="server" required></asp:TextBox>
        </p>
        <p>
            <asp:Button ID="register" placeholder="Register User" OnClick="Register_Click" runat="server" Text="Register" required></asp:Button>
        </p>
        <p>
            <a class="tts-button" href="Login.aspx">Login Here</a>
        </p>
    </form>
</body>
</html>
