<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddGroup.aspx.cs" Inherits="GetUser" %>
<% if (null == Session["username"])
   {
       Response.Redirect("Login.aspx");
   }
%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>TextToShare - Add Group</title>
        <link href="Content/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form id="AddGroup" class="tts-form" method="post" runat="server">
            <h1>Add a New Group</h1>
            <p>
                <label for="groupNameBox">Group Name</label>
                <asp:TextBox ID="groupNameBox" name="groupNameBox" placeholder="Group Name" runat="server" required></asp:TextBox>
            </p>
            <p>
                <label for="groupTagBox">Group Tag</label>
                <asp:TextBox ID="groupTagBox" name="groupTagBox" placeholder="Group Tag" runat="server" required></asp:TextBox>
            </p>
             <p>
                 <label for="groupOwner">Group Owner</label>
                 <asp:TextBox ID="groupOwner" name="groupOwner" placeholder="Group Owner" runat="server" required></asp:TextBox>
            </p>
            <p>
                <label for="groupDescriptionBox">Group Description</label>
                <asp:TextBox ID="groupDescriptionBox" name="groupDescriptionBox" placeholder="Descripation of Group" runat="server" required></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="addGroupButton" Text="Add Group" OnClick="addGroup_Click" runat="server"></asp:Button>
            </p>
        </form>
    </body>
</html>
