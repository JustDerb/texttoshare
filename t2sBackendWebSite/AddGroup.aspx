﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="users.AddGroup" src="AddGroup.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <form id ="AddGroup" runat = "server">
        <!--<asp:textbox id="ownerNamebox" placeholder="Owner" runat="server"></asp:textbox>-->
        <asp:textbox id="usersNameBox" placeholder="Group's members" runat="server"></asp:textbox>
        <asp:textbox id="moderatorsBox" placeholder="Group Moderators" runat="server"></asp:textbox>
        <asp:textbox id="enabledPluginsBox" placeholder="Enabled Plugins" runat="server"></asp:textbox>
        <asp:textbox id="groupNameBox" placeholder="Group Name" runat="server"></asp:textbox>
         <asp:textbox id="groupTagBox" placeholder="Group Tag" runat="server"></asp:textbox>
         <asp:textbox id="groupDescripationBox" placeholder="Descripation of Group" runat="server"></asp:textbox>
        <asp:button id="addGroupButton" text="Add Group" Onclick="addGroup_Click" runat="server"></asp:button>
   
    <div>
    
    </div>
         </form>
</body>
</html>