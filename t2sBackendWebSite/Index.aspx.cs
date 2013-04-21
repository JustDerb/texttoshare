using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class Index : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        PageTitle.Text = "Text2Share - Home";
        retrieveGroups();
    }

    /// <summary>
    /// Retrieves groups from the database associated with the current user in session.
    /// </summary>
    private void retrieveGroups()
    {
        Response.Write("Outside the if statement");

        if (null != Session["userid"])
        {
            Response.Write("Made it in the if statement");
            List<GroupDAO> ownedGroups = new List<GroupDAO>();
            List<GroupDAO> moderatedGroups = new List<GroupDAO>();
            List<GroupDAO> userIsInGroups = new List<GroupDAO>();

            try
            {
                IDBController controller = new SqlController();
                ownedGroups = controller.GetGroupsUserIsOwnerOf(Session["userid"] as int?);
                moderatedGroups = controller.GetGroupsUserIsModeratorOf(Session["userid"] as int?);
                userIsInGroups = controller.GetGroupsUserIsMemberOf(Session["userid"] as int?);
            }
            catch (SqlException)
            {
                groupsUserOwns.Text = "<li>An error occurred gathering group information. Please try again later.</li>";
                return;
            }

            printGroupsToPage(ownedGroups, groupsUserOwns, "<li>You do not own any groups. Press the \"+\" button to create a new one.</li>");
            printGroupsToPage(moderatedGroups, groupsUserModerates, "<li>You are not the moderator of any groups.</li>");
            printGroupsToPage(userIsInGroups, groupsUserIsIn, "<li>You are not a user of any groups.</li>");
        }
    }

    /// <summary>
    /// Prints the basic group information to the 
    /// </summary>
    /// <param name="groups"></param>
    /// <param name="pageLiteral"></param>
    /// <param name="zeroGroupCountMessage"></param>
    private void printGroupsToPage(List<GroupDAO> groups, Literal pageLiteral, string zeroGroupCountMessage)
    {
        StringBuilder groupBuilder = new StringBuilder();
        if (0 == groups.Count)
        {
            groupBuilder.Append(zeroGroupCountMessage);
        }
        else
        {
            foreach (GroupDAO group in groups)
            {
                groupBuilder.Append(string.Format(@"<li><a href=""ManageGroup.aspx"">{0} ({1})</a></li>", group.Name, group.GroupTag));
            }
        }

        pageLiteral.Text = groupBuilder.ToString();
    }

    public void retrievePlugins(Object sender, EventArgs e)
    {
        SqlController control = new SqlController();
        try
        {
            //String response = HttpContext.Current.Session["username"].ToString();
            // Response.Write(response);

            UserDAO user = control.RetrieveUserByUserName(Session["username"].ToString());
            List<PluginDAO> plugins = control.GetPluginsOwnedByUser(user);
            String[] names = new String[plugins.Count];
            ListItem[] list = new ListItem[plugins.Count];
            for (int i = 0; i < plugins.Count; i++)
            {
                ListItem item = new ListItem(plugins.ElementAt(i).Name);
                list[i] = item;
            }
            DropDownList PluginsOwned = ((DropDownList)((DropDownList)sender).Parent.FindControl("PluginsOwned"));
            PluginsOwned.Items.AddRange(list);

        }
        catch (ArgumentNullException)
        {

        }
        catch (CouldNotFindException)
        {

        }
    }


}