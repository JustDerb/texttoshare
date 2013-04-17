<%@ Page Language="C#" AutoEventWireup="true" Inherits="ManageUsers.manageUsers" src="manageUsers.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="manageUserForm" runat="server">
   
        <h1>Manage User</h1>
        <p>
            <label for="firstNameBox">First Name</label>
            <asp:TextBox ID="firstNameBox" name="firstNameBox"   runat="server"  placeholder="First Name"  ></asp:TextBox>
        </p>
        <p>
            <label for="lastNameBox">Last Name</label>
            <asp:TextBox ID="lastNameBox" name="lastNameBox" placeholder="Last Name" runat="server" ></asp:TextBox>
        </p>
        <p>
            <label for="userNameBox">Username</label>
            <asp:TextBox ID="userNameBox" name="userNameBox" placeholder="Username" runat="server" ></asp:TextBox>
        </p>
        <p>
            <label for="phoneNumberBox">Phone Number</label>
            <asp:TextBox ID="phoneNumberBox" name="phoneNumberBox" placeholder="Phone Number" runat="server" ></asp:TextBox>
        </p>
        <p>
            <label for="carrierBox">Carrier</label>
            <asp:TextBox ID="carrierBox" name="carrierBox" placeholder="Phone Carrier" runat="server" ></asp:TextBox>
        </p>
        <asp:DropDownList id="AllPlugin" runat="server" onload="popluatePluginList" >
            <asp:ListItem Text="<Plugins Made>" Value="0" />
        </asp:DropDownList>


        <p>
            <asp:Button ID="register" placeholder="Update User" OnClick="update_Click" runat="server" Text="Update" ></asp:Button>
        </p>
    </form>
</body>
</html>
