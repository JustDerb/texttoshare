<%@ Page Language="C#" AutoEventWireup="true" Inherits="managePlug.ManagePlugin" src="ManagePlugin.cs" %>
<% if (null == Session["username"])
   {
       Response.Redirect("Login.aspx");
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
            <label for="pluginNameBox">Plugin Name</label>
            <asp:textbox id="pluginNameBox" name="pluginNameBox" placeholder="Plugin Name" runat="server"></asp:textbox>
        </p>
        <p>
            <label for="pluginOwner">Plugin Name</label>
            <asp:textbox id="pluginOwner" name="pluginOwner" placeholder="Owner's Users Name" runat="server"></asp:textbox>
        </p>
        <p>
            <label for="helpTextBox">Plugin Name</label>
            <asp:textbox id="helpTextBox" name="helpTextBox" placeholder="Help Text" runat="server"></asp:textbox>
        </p>
        <p>
            <label for="pluginDescriptionBox">Plugin Name</label>
            <asp:textbox id="pluginDescripationBox" name="pluginDescripationBox" placeholder="Descripation of Plugin" runat="server"></asp:textbox>
        </p>
        <p>
            <label for="versionBox">Plugin Name</label>
            <asp:textbox id="versionBox" name="versionBox" placeholder="Version Number" runat="server"></asp:textbox>
        </p>
        <p>
            <asp:button id="updatePluginButton" name="updatePluginButton" text="Update Plugin" Onclick="updatePlugin_Click" runat="server"></asp:button>
        </p>

        <!--<asp:DropDownList-->
        <!--<asp:DropDownList id="ddlPlugin" runat="server" onload="popluateList" >
            <asp:ListItem Text="<Select Plugin>" Value="0" />
        </asp:DropDownList>-->

    <div>
    
    </div>
    </form>
</body>
</html>
