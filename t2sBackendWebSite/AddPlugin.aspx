<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddPlugin.aspx.cs" Inherits="AddPlugin" %>
<% if (null == Session["username"])
   {
       Response.Redirect("Login.aspx");
   }
   else
   {
       pluginOwner.Text = Session["username"].ToString();
   }
%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>TextToServe - Add a Plugin</title>
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
        <form id="formAddPlugin" class="tts-form" method="post" runat="server" enctype="multipart/form-data">
            <h2 class="tts-form-heading">Add Plugin</h2>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox ID="pluginNameBox" class="input-block-level" name="pluginNameBox" placeholder="Plugin Name" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox ID="pluginOwner" class="input-block-level" name="pluginOwner" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox ID="pluginDescriptionBox" class="input-block-level" name="pluginDescription" placeholder="Description of Plugin" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox ID="helpTextBox" class="input-block-level" name="helpTextBox" placeholder="Help Text" runat="server" title="Tells the user what this plugin does when they text 'HELP plugin'." required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox ID="versionBox" class="input-block-level" name="versionBox" placeholder="Version Number (1.0.0, etc)" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <input id="filMyFile" class="input-block-level" name="filMyFile" type="file" accept=".lua" runat="server" title="Select the .lua script file that this plugin will use." required/>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:Button ID="addPluginButton" class="input-block-level" Text="Add Plugin" OnClick="AddPlugin_Click" runat="server"></asp:Button>
                </div>
            </div>
        </form>
    </body>
</html>
