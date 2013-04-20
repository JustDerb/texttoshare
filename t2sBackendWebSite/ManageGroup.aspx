<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageGroup.aspx.cs" Inherits="ManageGroup" %>
<%
    Session["groupTag"] = "test";
    groupTagBox.Text = Session["groupTag"].ToString();
%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <link href="Content/bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            body {
				padding-top: 60px;
				padding-bottom: 40px;
			}

            @media (max-width: 980px) {
                .navbar-text .pull-right {
                    float: none;
                    padding-left: 5px;
                    padding-right: 5px;
                }
            }
        </style>
        <link href="Content/bootstrap/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    </head>
    <body>

        <div class="navbar navbar-fixed-top">
            <div class="navbar-inner">
                <div class="container-fluid">
                    <button type="button" class="btn btn-navbar collapsed" data-toggle="collapse" data-target=".nav-collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="brand" href="#">Text2Share</a>
                    <div class="nav-collapse collapse">
                        <ul class="nav pull-right" role="navigation">
                            <li>
                                <a href="ManageUser.aspx">Settings</a>
                            </li>
                            <li class="divider-vertical"></li>
                            <li>
                                <asp:HyperLink id="logoutButton" runat="server" href="Logout.aspx">Logout</asp:HyperLink>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

        <form id="updateform" runat="server">
            <asp:textbox id="groupNameBox" placeholder="Group Name" runat="server"></asp:textbox>
            <asp:textbox id="groupTagBox"  runat="server"></asp:textbox>
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
