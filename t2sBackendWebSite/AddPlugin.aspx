<%@ Page Language="C#" AutoEventWireup="true"  Inherits="plugin.AddPlugin" src="AddPlugin.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="formAddPlugin" method="post" runat="server" enctype="multipart/form-data">
        <asp:textbox id="pluginNameBox" placeholder="Plugin Name" runat="server"></asp:textbox>
        <asp:textbox id="pluginOwner" placeholder="Owner's Users Name" runat="server"></asp:textbox>
        <asp:textbox id="helpTextBox" placeholder="Help Text" runat="server"></asp:textbox>
        <asp:textbox id="pluginDescripationBox" placeholder="Descripation of Plugin" runat="server"></asp:textbox>
        <asp:textbox id="versionBox" placeholder="VersionNumber" runat="server"></asp:textbox>
        <input id="filMyFile" type="file" runat="server"/>
        <asp:button id="addPluginButton" text="Add Plugin" Onclick="AddPlugin_Click" runat="server"></asp:button>
    </form>
    <div>
 
    </div>
</body>
</html>
