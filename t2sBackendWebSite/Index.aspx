<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>TextToShare - Home</title>
        <link href="Content/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <div class="navbar">
            <div class="navbar-inner">
                <div class="container">
                    <a class="brand" href="#">TextToShare</a>
                    <ul class="nav" role="navigation">
                        <li class="dropdown">
                            <a id="groupDrop" class="dropdown-toggle" data-toggle="dropdown" href="#">Groups<b class="caret"></b></a>
                            <ul class="dropdown-menu" role="menu" aria-labelledby="groupDrop">
                                <li><a role="menuitem" href="AddGroup.aspx">Add Group</a></li>
                                <li><a role="menuitem" href="ManageGroup.aspx">Manage Groups</a></li>
                            </ul>
                        </li>
                        <li class="dropdown">
                            <a id="pluginDrop" class="dropdown-toggle" data-toggle="dropdown" href="#">Plugins<b class="caret"></b></a>
                            <ul class="dropdown-menu" role="menu" aria-labelledby="pluginDrop">
                                <li><a role="menuitem" href="AddPlugin.aspx">Add Plugin</a></li>
                                <li><a role="menuitem" href="ManagePlugin.aspx">Manage Plugins</a></li>
                            </ul>
                        </li>
                        <li class="dropdown">
                            <a id="userDrop" class="dropdown-toggle" data-toggle="dropdown" href="#">Settings<b class="caret"></b></a>
                            <ul class="dropdown-menu" role="menu" aria-labelledby="userDrop">
                                <li><a role="menuitem" href="ManageUser.aspx">Edit User Information</a></li>
                            </ul>
                        </li>
                    </ul>
                    <form class="navbar-form pull-right" runat="server">
                        <button class="btn" runat="server" onclick="logoutButton_Click">Logout</button>
                    </form>
                </div>
            </div>
        </div>
    </body>

    <script src="Scripts/jquery-1.7.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</html>