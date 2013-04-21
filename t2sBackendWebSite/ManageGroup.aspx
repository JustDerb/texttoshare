<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageGroup.aspx.cs" Inherits="ManageGroup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title><asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
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
                    <a class="brand" href="Index.aspx">Text2Share</a>
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

        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span6">
                    <form id="updateform" class="form-horizontal" runat="server">
                        <h2 class="tts-form-heading">Group Information</h2>
                        <div class="control-group">
                            <label class="control-label" for="groupNameBox">Group Name</label>
                            <div class="controls">
                                <asp:textbox id="groupNameBox" name="groupNameBox" class="input-block-level" placeholder="Group Name" runat="server" required></asp:textbox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="groupTagBox">Group Tag</label>
                            <div class="controls">
                                <asp:textbox id="groupTagBox" class="input-block-level" runat="server" required></asp:textbox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="groupDescriptionBox">Group Description</label>
                            <div class="controls">
                                <asp:textbox id="groupDescriptionBox" name="groupDescriptionBox" class="input-block-level" placeholder="Description of Group" runat="server" required></asp:textbox>
                            </div>
                        </div>
                        <div class="control-group">
                            <div class="controls">
                                <asp:button id="updateGroupButton" name="updateGroupButton" class="input-block-level" text="Update Group Information" Onclick="updateGroup_Click" runat="server" required></asp:button>
                            </div>
                        </div>
                        <div class="control-group">
                            <asp:Label id="invalidEntries" class="control-label text-error" runat="server"></asp:Label>
                        </div>
                    </form>
                </div>
            </div>
        </div>

<%
         //<!--             <div class="control-group">
         //       <div class="controls">
         //           <asp:DropDownList id="groupPlugin" runat="server" onload="popluateGroupList" >
         //               <asp:ListItem Text="<Select Plugin>" Value="0" />
         //           </asp:DropDownList>
         //           <asp:Button ID="pluginEnable" runat="server" text="Enable Plugin" onclick= "enablePlugin" />
         //       </div>
         //   </div>
         //   <div class="control-group">
         //       <div class="controls">
         //           <asp:DropDownList id="enabledPlugins" runat="server" onload="retrieveEnabledGroupPlugins" >
         //               <asp:ListItem Text="<Currently Enabled Plugins>" Value="0" />
         //           </asp:DropDownList>
         //           <asp:button id="unEnable" text="UnEnable Plugin" Onclick="unEnablePlugin_Click" runat="server"></asp:button>
         //       </div>
         //   </div> -->   
    
%>


    </body>
</html>
