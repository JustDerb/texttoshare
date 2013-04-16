<%@ Page Language="C#" AutoEventWireup="true" Inherits="managePlug.ManagePlugin" src="ManagePlugin.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="managePlugin" runat="server" >

        <<asp:textbox id="pluginNameBox" placeholder="Plugin Name" runat="server"></asp:textbox>
        <asp:textbox id="pluginOwner" placeholder="Owner's Users Name" runat="server"></asp:textbox>
        <asp:textbox id="helpTextBox" placeholder="Help Text" runat="server"></asp:textbox>
        <asp:textbox id="pluginDescripationBox" placeholder="Descripation of Plugin" runat="server"></asp:textbox>
        <asp:textbox id="versionBox" placeholder="VersionNumber" runat="server"></asp:textbox>
        <asp:button id="updatePluginButton" text="Update Plugin" Onclick="updatePlugin_Click" runat="server"></asp:button>
        <!--<asp:DropDownList-->
        <asp:DropDownList id="ddlPlugin" runat="server" onload="popluateList" >
            <asp:ListItem Text="<Select Plugin>" Value="0" />
        </asp:DropDownList>

    <div>
    
    </div>
    </form>
</body>
</html>
