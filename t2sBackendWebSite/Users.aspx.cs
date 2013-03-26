using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using t2sDbLibrary;



public partial class Users : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SqlController controller = new SqlController();
       // controller.CreateUser();
       // usersAdapter.Insert(1, "lance", "staley", "641-373-2356", "641-373-235@email.uscc.net", 1, 2, false, false, time.Date);



       // GridView1.DataSource = usersAdapter.GetUsers();
        //GridView1.DataBind();
    }
}