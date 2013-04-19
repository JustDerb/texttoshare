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
</head>
<body>
    <form id="managePlugin" runat="server">
        <p>
            <asp:DropDownList id="Plugins" runat="server" >
                <asp:ListItem Text="<Select Plugin>" Value="0" />
            </asp:DropDownList>
        </p>
        <p>
            <label for="pluginNameBox">Plugin Name</label>
            <asp:textbox id="pluginNameBox" name="pluginNameBox" placeholder="Plugin Name" runat="server"></asp:textbox>
        </p>
        <p>
            <label for="pluginOwner">Plugin Owner</label>
            <asp:textbox id="pluginOwner" name="pluginOwner"  runat="server"></asp:textbox>
        </p>
        <p>
            <label for="helpTextBox">Help Text</label>
            <asp:textbox id="helpTextBox" name="helpTextBox" placeholder="Help Text" runat="server"></asp:textbox>
        </p>
        <p>
            <label for="pluginDescriptionBox">Plugin Description</label>
            <asp:textbox id="pluginDescripationBox" name="pluginDescripationBox" placeholder="Descripation of Plugin" runat="server"></asp:textbox>
        </p>
        <p>
            <label for="versionBox">Plugin Version</label>
            <asp:textbox id="versionBox" name="versionBox" placeholder="Version Number" runat="server"></asp:textbox>
        </p>
        <p>
            <asp:button id="updatePluginButton" name="updatePluginButton" text="Update Plugin" Onclick="updatePlugin_Click" runat="server"></asp:button>
        </p>

    <div>
    
    </div>
    </form>
</body>
</html>
