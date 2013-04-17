<%@ Page Language="C#" AutoEventWireup="true" Inherits="manageGroup.ManageGroup" src="ManageGroup.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="updateform" runat="server">
        <asp:textbox id="groupNameBox" placeholder="Group Name" runat="server"></asp:textbox>
        <asp:textbox id="groupTagBox" placeholder="Group Tag" runat="server"></asp:textbox>
        <asp:textbox id="groupDescripationBox" placeholder="Descripation of Group" runat="server"></asp:textbox>
        <asp:DropDownList id="groupPlugin" runat="server" onload="popluateGroupList" >
            <asp:ListItem Text="<Select Plugin>" Value="0" />
        </asp:DropDownList>
        <asp:Button ID="pluginEnable" runat="server" text="Enable Plugin" onclick= "enablePlugin" />

        <asp:DropDownList id="enabledPlugins" runat="server" onload="retrieveEnabledGroupPlugins" >
            <asp:ListItem Text="<Currently Enabled Plugins>" Value="0" />
        </asp:DropDownList>
        <asp:button id="unEnable" text="UnEnable Plugin" Onclick="unEnablePlugin_Click" runat="server"></asp:button>

        <asp:button id="updateGroupButton" text="Update Group" Onclick="updateGroup_Click" runat="server"></asp:button>
    <div>
    
    </div>
    </form>
</body>
</html>
