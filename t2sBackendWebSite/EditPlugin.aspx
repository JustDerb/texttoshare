<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditPlugin.aspx.cs" Inherits="EditPlugin" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
    <link href="Content/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css" media="screen">
        #editor { 
            position: relative;
            width: 100%;
            height: 1000px;
        }
        #t2sluaapi {
            height:600px;
            overflow:auto;
        }
        body {
            padding-top: 60px; /* 60px to make the container go all the way to the bottom of the topbar */
        }
    </style>
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="http://rawgithub.com/ajaxorg/ace-builds/master/src-noconflict/ace.js" type="text/javascript" charset="utf-8"></script>
</head>
<body>
    <div class="navbar navbar-inverse navbar-fixed-top">
      <div class="navbar-inner">
        <div class="container">
          <a class="brand" href="#">Text2Share</a>
          <div class="nav-collapse collapse">
            <ul class="nav">
              <li class="active"><a href="#">Link1</a></li>
              <li><a href="#about">Link2</a></li>
              <li><a href="#contact">Link3</a></li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span7 well well-small">
                <h5 style="margin-top:0">Lua Editor</h5>
                <div id="editor"></div>
            </div>
          <div class="span5">
              <h3>Plugin Name</h3>
              <form method="post">
                  <label for="versionInput">Version</label>
                  <input class="input-small" id="versionInput" name="versionInput" type="text" placeholder="Version" />
                  <textarea class="input-block-level" id="descriptionInput" name="descriptionInput" rows="3" placeholder="Description..."></textarea>
                  <label for="helpInput">Help</label>
                  <input class="input-large" id="helpInput" name="helpInput" type="text" placeholder="Help text..." />
                  <label for="levelInput">Access</label>
                  <select name="levelInput" id="levelInput">
                      <option>Users &amp; Moderators</option>
                      <option>Moderators</option>
                  </select>
                  <textarea style="display:none" id="editorText" name="editorText" rows="3" runat="server"></textarea>
                  <input type="submit" class="tts-button" value="Save changes" />
              </form>
              <h4>Text2Share Lua API</h4>
              <div id="t2sluaapi" class="well">
                  <h5>Messages</h5>
                  <dl>
                      <!-- MESSAGE API CALLS -->
                      <dt><code>GetMessageArgumentString() : String</code></dt>
                      <dd>Get plugin arguments.</dd>
                      <dt><code>GetMessageArgumentList() : Key/Value Map</code></dt>
                      <dd>Get plugin arguments as a list.</dd>
                      <dt><code>GetMessageCommand() : String</code></dt>
                      <dd>Get plugin command.</dd>
                      <dt><code>GetSenderId() : String</code></dt>
                      <dd>Get sender ID.</dd>
                  </dl>
                  <h5>Groups</h5>
                  <dl>
                      <!-- GROUP API CALLS -->
                      <dt><code>GetUserIdList() : Key/Value Map</code></dt>
                      <dd>Get list of group users ID.</dd>
                      <dt><code>GetModeratorIdList() : Key/Value Map</code></dt>
                      <dd>Get list of group moderators ID</dd>
                      <dt><code>GetOwnerId() : String</code></dt>
                      <dd>Get owner ID.</dd>
                      <dt><code>GetGroupDescription() : String</code></dt>
                      <dd>Get the group description</dd>
                      <dt><code>GetGroupName() : String</code></dt>
                      <dd>Get the group name.</dd>
                      <dt><code>GetGroupTag() : String</code></dt>
                      <dd>Get the group tag.</dd>
                  </dl>
                  <h5>Users</h5>
                  <dl>
                      <!-- USER API CALLS -->
                      <dt><code>GetUserFirstName(userID) : String</code></dt>
                      <dd>Get the users first name.</dd>
                      <dt><code>GetUserLastName(userID) : String</code></dt>
                      <dd>Get the users last name.</dd>
                      <dt><code>GetUsername(userID) : String</code></dt>
                      <dd>Get the users username.</dd>
                      <dt><code>GetUserIsSuppressed(userID) : Boolean</code></dt>
                      <dd>Get whether the user is suppressed (will not recieve messages)</dd>
                      <dt><code>GetUserIsBanned(userID) : Boolean</code></dt>
                      <dd>Get whether the user is banned (will not recieve messages)</dd>
                      <dt><code>GetUserCarrier(userID) : String</code></dt>
                      <dd>Get the users phone carrier</dd>
                  </dl>
                  <h5>Plugins</h5>
                  <dl>
                      <!-- PLUGIN API CALLS -->
                      <dt><code>SetValue(key, value) : nil</code></dt>
                      <dd>Set plugin key/value.</dd>
                      <dt><code>GetValue(key) : String</code></dt>
                      <dd>Get plugin key/value</dd>
                      <dt><code>SetValue(key, value, userId) : nil</code></dt>
                      <dd>Set user key/value</dd>
                      <dt><code>GetValue(key, userId) : String</code></dt>
                      <dd>Get user key/value</dd>
                      <dt><code>SendMessage(userId, message) : nil</code></dt>
                      <dd>Send a text to a user</dd>
                      <dt><code>HTTPDownloadText(URL) : String</code></dt>
                      <dd>Retrieves the body text of any website URL (must be less than 1 MB to download). 
                          This function will return <code>nil</code> if it cannot find/complete the request.</dd>
                  </dl>
              </div>
          </div>
        </div>
    </div>
      

    <script>
        var editor = ace.edit("editor");
        editor.setTheme("ace/theme/crimson_editor");
        editor.getSession().setMode("ace/mode/lua");
        editor.getSession().setTabSize(4);
        editor.getSession().setUseSoftTabs(true);
        editor.setHighlightActiveLine(true);
        editor.setShowPrintMargin(false);

        var textarea = $('textarea[name="editorText"]');
        textarea.hide();
        editor.getSession().setValue(textarea.val());
        editor.getSession().on('change', function () {
            textarea.val(editor.getSession().getValue());
        });
    </script>
</body>
</html>
