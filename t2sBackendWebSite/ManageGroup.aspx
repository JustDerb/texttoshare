﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageGroup.aspx.cs" Inherits="ManageGroup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title><asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link href="Content/themes/redmond/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css">
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

            <form id="updateform" class="form-horizontal" runat="server">
                <div class="row-fluid">
                    <div class="span6">
                        <h2 class="tts-form-heading">Group Information</h2>
                        <div class="control-group">
                            <label class="control-label" for="groupNameBox">Group Name</label>
                            <div class="controls">
                                <asp:TextBox id="groupNameBox" name="groupNameBox" class="input-block-level" placeholder="Group Name" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="groupTagBox">Group Tag</label>
                            <div class="controls">
                                <asp:TextBox id="groupTagBox" class="input-block-level" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="groupDescriptionBox">Group Description</label>
                            <div class="controls">
                                <asp:TextBox id="groupDescriptionBox" name="groupDescriptionBox" class="input-block-level" placeholder="Description of Group" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row-fluid">
                    <div class="span6">
                        <div class="well" style="padding: 8px 0;">
                            <ul class="nav nav-list">
                                <li class="nav-header">Owner</li>                
                            </ul>
                            <asp:TextBox id="groupOwner" TextMode="MultiLine" class="input-block-level" runat="server" ReadOnly="true"></asp:TextBox>

                            <ul class="nav nav-list">
                                <li class="nav-header">Moderators</li>
                            </ul>
                            <asp:TextBox id="groupModerators" TextMode="MultiLine" class="input-block-level" runat="server"></asp:TextBox>

                            <ul class="nav nav-list">
                                <li class="nav-header">Users</li>
                            </ul>
                            <asp:TextBox id="groupUsers" TextMode="MultiLine" class="input-block-level" runat="server"></asp:TextBox>

                        </div>
                    </div>
                    <div class="span6">
                        <div class="well" style="padding: 8px 0;">
                            <ul class="nav nav-list">
                                <li class="nav-header">Plugins</li>
                                <asp:Literal ID="groupPluginList" runat="server"></asp:Literal>
                                <li><a href="ManagePlugins.aspx?grouptag=<%= HttpUtility.HtmlEncode(_currentGroup.GroupTag) %>">Manage Plugins</a></li>
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="row-fluid">
                    <div class="span12">
                        <div class="control-group">
                            <div class="controls">
                                <asp:Button id="updateGroupButton" name="updateGroupButton" class="btn btn-primary pull-right" text="Update Group Information" Onclick="UpdateGroupMetadata_Click" runat="server" />
                                <br />
                                <hr />
                                <asp:Button ID="deleteGroupButton" name="deleteGroupButton" class="btn btn-danger pull-right" Text="Delete Group" OnClick="deleteGroupButton_Click" runat="server" />
                            </div>
                        </div>
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
            $('#groupOwner').textareainputsupport({
                source: function (request, response) {
                    $.ajax({
                        url: "Users.json.aspx",
                        dataType: "json",
                        data: {
                            search: request.term
                        },
                        success: function (data) {
                            response($.each(data.Users, function (item) {
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
                    $('#groupModerators').val(item.item.value);
                    return false;
                },
                focus: function (event, item) {
                    $('#groupModerators').val(item.item.value);
                    return false;
                }
            });

            $('#groupModerators').textareainputsupport({
                source: function (request, response) {
                    $.ajax({
                        url: "Users.json.aspx",
                        dataType: "json",
                        data: {
                            search: request.term
                        },
                        success: function (data) {
                            response($.each(data.Users, function (item) {
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
                    sList = $('#groupModerators').val().split(",");
                    var prevNum = sList.length;
                    sList.splice((sList.length - 1), 1);
                    required = sList.join(',');
                    if (prevNum > 1)
                        $('#groupModerators').val(required + ', ' + (item.item.value));
                    else
                        $('#groupModerators').val(required + (item.item.value));
                    return false;
                },
                focus: function (event, item) {
                    sList = $('#groupModerators').val().split(",");
                    var prevNum = sList.length;
                    sList.splice((sList.length - 1), 1);
                    required = sList.join(',');
                    if (prevNum > 1)
                        $('#groupModerators').val(required + ', ' + (item.item.value));
                    else
                        $('#groupModerators').val(required + (item.item.value));
                    return false;
                }
            });

            $('#groupUsers').textareainputsupport({
                source: function (request, response) {
                    $.ajax({
                        url: "Users.json.aspx",
                        dataType: "json",
                        data: {
                            search: request.term
                        },
                        success: function (data) {
                            response($.each(data.Users, function (item) {
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
                    sList = $('#groupUsers').val().split(",");
                    var prevNum = sList.length;
                    sList.splice((sList.length - 1), 1);
                    required = sList.join(',');
                    if (prevNum > 1)
                        $('#groupUsers').val(required + ', ' + (item.item.value));
                    else
                        $('#groupUsers').val(required + (item.item.value));
                    return false;
                },
                focus: function (event, item) {
                    sList = $('#groupUsers').val().split(",");
                    var prevNum = sList.length;
                    sList.splice((sList.length - 1), 1);
                    required = sList.join(',');
                    if (prevNum > 1)
                        $('#groupUsers').val(required + ', ' + (item.item.value));
                    else
                        $('#groupUsers').val(required + (item.item.value));
                    return false;
                }
            });

        });
  </script>
</html>
