<%@ Page Language="C#" AutoEventWireup="true" Inherits="users.AddGroup" Src="AddGroup.cs" %>
<% if (null == Session["username"])
   {
       Response.Redirect("Login.aspx");
   }
%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>TextToShare - Add Group</title>
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form id="AddGroup" class="tts-form" method="post" runat="server">
            <!--<asp:textbox id="ownerNamebox" placeholder="Owner" runat="server"></asp:textbox>-->
            <p>
                <label for="groupNameBox">Group Name</label>
                <asp:TextBox ID="groupNameBox" name="groupNameBox" placeholder="Group Name" runat="server" required></asp:TextBox>
            </p>
            <p>
                <label for="groupTagBox">Group Name</label>
                <asp:TextBox ID="groupTagBox" name="groupTagBox" placeholder="Group Tag" runat="server" required></asp:TextBox>
            </p>
             <p>
                 <label for="groupOwner">Group Owner</label>
                 <asp:TextBox ID="groupOwner" name="groupOwner" placeholder="Group Owner" runat="server" required></asp:TextBox>
            </p>
            <p>
                <label for="groupDescriptionBox">Group Description</label>
                <asp:TextBox ID="groupDescripationBox" name="groupDescriptionBox" placeholder="Descripation of Group" runat="server" required></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="addGroupButton" Text="Add Group" OnClick="addGroup_Click" runat="server"></asp:Button>
            </p>
        </form>
    </body>
</html>
