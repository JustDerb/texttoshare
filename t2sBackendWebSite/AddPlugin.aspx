<%@ Page Language="C#" AutoEventWireup="true" Inherits="plugin.AddPlugin" Src="AddPlugin.cs" %>
<% if (null == Session["username"])
   {
       Response.Redirect("Login.aspx");
   }
%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>TextToServe - Add a Plugin</title>
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form id="formAddPlugin" class="tts-form" method="post" runat="server" enctype="multipart/form-data">
            <h1>Add a Plugin</h1>
            <p>
                <label for="pluginNameBox">Plugin Name</label>
                <asp:TextBox ID="pluginNameBox" name="pluginNameBox" placeholder="Plugin Name" runat="server" required></asp:TextBox>
            </p>
            <p>
                <label for="pluginDescriptionBox">Plugin Description</label>
                <asp:TextBox ID="pluginDescriptionBox" name="pluginDescription" placeholder="Description of Plugin" runat="server" required></asp:TextBox>
            </p>
            <p>
                <label for="helpTextBox">Help Text</label>
                <asp:TextBox ID="helpTextBox" name="helpTextBox" placeholder="Help Text" runat="server" title="Tells the user what this plugin does when they text 'HELP plugin'." required></asp:TextBox>
            </p>
            <p>
                <label for="versionBox">Initial Version</label>
                <asp:TextBox ID="versionBox" name="versionBox" placeholder="Version Number" runat="server" required></asp:TextBox>
            </p>
            <p>
                <label for="filMyFile">.lua Plugin File</label>
                <input id="filMyFile" name="filMyFile" type="file" accept=".lua" runat="server" title="Select the .lua script file that this plugin will use." required/>
            </p>
            <p>
                <asp:Button ID="addPluginButton" Text="Add Plugin" OnClick="AddPlugin_Click" runat="server"></asp:Button>
            </p>
        </form>
    </body>
</html>
