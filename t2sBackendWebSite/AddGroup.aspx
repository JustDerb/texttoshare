<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddGroup.aspx.cs" Inherits="GetUser" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>TextToShare - Add Group</title>
        <link href="Content/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
			body {
				padding-top: 40px;
				padding-bottom: 40px;
				background-color: #f5f5f5;
			}
        </style>
    </head>
    <body>
        <form id="AddGroup" class="tts-form" method="post" runat="server">
            <h2 class="tts-form-heading">Add Group</h2>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="groupNameBox" class="input-block-level" name="groupNameBox" placeholder="Group Name" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="groupTagBox" class="input-block-level" name="groupTagBox" placeholder="Group Tag" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="groupOwner" class="input-block-level" name="groupOwner" placeholder="Group Owner" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="groupDescriptionBox" class="input-block-level" name="groupDescriptionBox" placeholder="Description of Group" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:Button id="addGroupButton" class="btn btn-large btn-primary" Text="Add Group" OnClick="addGroup_Click" runat="server"></asp:Button>
                    <a class="btn btn-large pull-right" href="ManageGroup.aspx">Cancel</a>
                </div>
            </div>
        </form>
    </body>
</html>
