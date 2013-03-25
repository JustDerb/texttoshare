<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Users.aspx.cs"
    Inherits="Users" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>View All Users in a GridView</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>
            All Users</h2>
        <p>
            <asp:GridView ID="GridView1" runat="server"
             CssClass="DataWebControlStyle">
               <HeaderStyle CssClass="HeaderStyle" />
               <AlternatingRowStyle CssClass="AlternatingRowStyle" />
            </asp:GridView>
             </p>
    </div>
    </form>
</body>
</html>