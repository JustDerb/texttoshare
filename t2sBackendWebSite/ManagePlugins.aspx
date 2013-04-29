<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagePlugins.aspx.cs" Inherits="ManagePlugins" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title><asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link href="Content/themes/redmond/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
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

            .ui-autocomplete {
                width: 25%;
                list-style-type: none;
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

        <div class="container">
            <% if (showErrorMessage)
               { %>
            <div class="row-fluid">
                <div class="span12">
                    <div class="alert alert-error">
                        <button type="button" class="close" data-dismiss="alert">&times;</button>
                        <strong>Error!</strong> <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <% } %>
            <% if (showSuccessMessage)
               { %>
            <div class="row-fluid">
                <div class="span12">
                    <div class="alert alert-success">
                        <button type="button" class="close" data-dismiss="alert">&times;</button>
                        <strong>Success!</strong> <asp:Literal ID="successMessage" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <% } %>

            <div class="row-fluid">
                <div class="span12">
                    <h3>Manage Plugins</h3>
                </div>
            </div>
            <form id="updateform" class="form-horizontal" runat="server">
                <div class="row-fluid">
                    <div class="span6">
                        <div class="control-group">
                            <label class="control-label" for="groupNameBox">Disabled Plugins</label>
                            <div class="controls">
                                <asp:TextBox id="disabledPlugins" name="disabledPlugins" TextMode="MultiLine" class="input-block-level" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="span6">
                        <div class="control-group">
                            <label class="control-label" for="groupNameBox">Enabled Plugins</label>
                            <div class="controls">
                                <asp:TextBox id="enabledPlugins" name="enabledPlugins" TextMode="MultiLine" class="input-block-level" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row-fluid">
                    <div class="span12">
                        <asp:Button ID="submitButton" class="btn btn-primary pull-right" Text="Update Plugins" OnClick="submitPluginsButton_Click" runat="server" />
                    </div>
                </div>
             </form>
        </div>
    </body>

    <script src="Scripts/jquery-1.7.1.min.js"></script>
    <script src="Scripts/jquery-ui-1.8.20.min.js"></script>
    <script src="Scripts/rangyinputs_jquery.min.js"></script>
    <script src="Scripts/textinputs_jquery_src.js"></script>
    <script src="Scripts/jquery.textarea-input-support.js"></script>

    <script type="text/javascript">
        $(function () {
            $('#enabledPlugins').textareainputsupport({
                source: function (request, response) {
                    $.ajax({
                        url: "Plugins.json.aspx",
                        dataType: "json",
                        data: {
                            search: request.term
                        },
                        success: function (data) {
                            response($.each(data.Plugins, function (item) {
                                return {
                                    label: item,
                                    value: item
                                }
                            }));
                        }
                    });
                },
                minLength: 2,
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                },
                select: function (event, item) {
                    sList = $('#enabledPlugins').val().split(",");
                    var prevNum = sList.length;
                    sList.splice((sList.length - 1), 1);
                    required = sList.join(',');
                    if (prevNum > 1)
                        $('#enabledPlugins').val(required + ', ' + (item.item.value));
                    else
                        $('#enabledPlugins').val(required + (item.item.value));
                    return false;
                },
                focus: function (event, item) {
                    sList = $('#enabledPlugins').val().split(",");
                    var prevNum = sList.length;
                    sList.splice((sList.length - 1), 1);
                    required = sList.join(',');
                    if (prevNum > 1)
                        $('#enabledPlugins').val(required + ', ' + (item.item.value));
                    else
                        $('#enabledPlugins').val(required + (item.item.value));
                    return false;
                }
            });

        });
  </script>
</html>

