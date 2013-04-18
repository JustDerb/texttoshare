<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>
<% if (null == Session["username"])
   {
       Response.Redirect("Login.aspx");
   }
%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>TextToShare - Home</title>
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
        <link href="Content/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <div class="navbar">
            <div class="navbar-inner">
                <a class="brand" href="#">TextToShare</a>
                <ul class="nav">
                    <li class="active"><a href="#">Home</a></li>
                    <li><a href="#">Link</a></li>
                    <li><a href="#">Link</a></li>
                </ul>
                <form class="navbar-form pull-right" runat="server">
                    <button class="btn" runat="server" onclick="logoutButton_Click">Logout</button>
                </form>
            </div>
        </div>
    </body>

    <script src="Scripts/jquery-1.7.1.min.js"></script>
    <script src="Scripts/boostrap.min.js"></script>
</html>
