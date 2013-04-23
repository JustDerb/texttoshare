<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Verification.aspx.cs" Inherits="Verification" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title><asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
        <link href="Content/bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
			body {
				padding-top: 40px;
				padding-bottom: 40px;
				background-color: #f5f5f5;
			}
        </style>
        <link href="Content/bootstrap/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form class="tts-form" id="form1" runat="server">
            <h2 class="tts-form-heading">Verification Code</h2>
            <p>
                <strong>Your generated code is:</strong> <h3><asp:Literal ID="verificationCode" runat="server"></asp:Literal></h3>
            </p>
            <p>
                Please text the generated code to:
            </p>
            <p>    
                <strong class="text-center"><asp:Literal ID="t2sAccountEmail" runat="server"></asp:Literal></strong>
            </p>
            <p>
                to set your phone number in the system. After a few moments, press the "Refresh" button to 
                reload the page. If your number has been set, you will be redirected to the home page.
            </p>
            <div class="control-group">
                <div class="controls">
                    <asp:Button ID="reloadOrRedirect" class="btn btn-primary" OnClick="ReloadOrRedirect_Click" Text="Refresh" runat="server" />
                    <a href="Login.aspx" class="btn">Back to Login</a>
                </div>
            </div>
            <div class="control-group">
                <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
            </div>
        </form>
    </body>
</html>
