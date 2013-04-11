<%@ Page Language="C#" AutoEventWireup="true"Inherits="manageGroup.ManageGroup" src="ManageGroup.cs" %>

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
        <asp:button id="updateGroupButton" text="Update Group" Onclick="updateGroup_Click" runat="server"></asp:button>

         <asp:CheckBoxList 
            ID="ComboBox1"
            DataSourceID="" 
            DataTextField="Title" 
            DataValueField="Id" 
            MaxLength="0" 
            runat="server" >
        </asp:CheckBoxList>


    <div>
    
    </div>
    </form>
</body>
</html>
