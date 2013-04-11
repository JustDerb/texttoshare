using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;



namespace manageGroup
{
    /// <summary>
    /// Summary description for ManageGroup
    /// </summary>
    public class ManageGroup : Page
    {
        public ManageGroup()
        {
        }


        protected System.Web.UI.WebControls.Label MyLabel;
        protected System.Web.UI.WebControls.Button MyButton;
        protected System.Web.UI.WebControls.TextBox MyTextBox;


        public void updateGroup_Click(Object sender, EventArgs e)
        {

            SqlController controller = new SqlController();
            GroupDAO group;
            String groupName = Request["groupNameBox"];
            String groupTag = Request["groupTagBox"];
            String groupDescription = Request["groupDescripationBox"];
            group = controller.RetrieveGroup(groupTag);
            group.Name = groupName;
            group.GroupTag = groupTag;
            group.Description = groupDescription;

        }

    }



}