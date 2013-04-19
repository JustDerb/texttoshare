<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagePlugin.aspx.cs" Inherits="ManagePlugin" %>

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
        <title></title>
        <link href="Content/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form id="managePlugin" runat="server">
            <p>
                <asp:DropDownList ID="Plugins" runat="server">
                    <asp:ListItem Text="<Select Plugin>" Value="0" />
                </asp:DropDownList>
            </p>
            <p>
                <label for="pluginNameBox">Plugin Name</label>
                <asp:TextBox ID="pluginNameBox" name="pluginNameBox" placeholder="Plugin Name" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="pluginOwner">Plugin Owner</label>
                <asp:TextBox ID="pluginOwner" name="pluginOwner" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="helpTextBox">Help Text</label>
                <asp:TextBox ID="helpTextBox" name="helpTextBox" placeholder="Help Text" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="pluginDescriptionBox">Plugin Description</label>
                <asp:TextBox ID="pluginDescripationBox" name="pluginDescripationBox" placeholder="Descripation of Plugin" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="versionBox">Plugin Version</label>
                <asp:TextBox ID="versionBox" name="versionBox" placeholder="Version Number" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="updatePluginButton" name="updatePluginButton" Text="Update Plugin" OnClick="updatePlugin_Click" runat="server"></asp:Button>
            </p>

            <div>
            </div>
        </form>
    </body>
</html>
