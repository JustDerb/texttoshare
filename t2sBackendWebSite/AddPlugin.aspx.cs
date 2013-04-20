using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;


public partial class AddPlugin : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        PageTitle.Text = "Text2Share - Add Plugin";
    }

    public void AddPlugin_Click(Object sender, EventArgs e)
    {
        SqlController controller = new SqlController();
        PluginDAO plugin = new PluginDAO();
        UserDAO owner = new UserDAO();
        plugin.Description = Request["pluginDescripationBox"];
        plugin.HelpText = Request["helpTextBox"];
        plugin.IsDisabled = false;
        plugin.VersionNum = Request["versionBox"];
        plugin.Name = Request["pluginNameBox"];
        //   Request.fil
        String ownerUserName = Request["pluginOwner"];
        try
        {
            owner = controller.RetrieveUserByUserName(ownerUserName);
            if (owner != null)
            {
                plugin.OwnerID = owner.UserID;
            }
            //fill upload
            if (filMyFile.PostedFile != null)
            {
                // File was sent
                // Get a reference to PostedFile object
                HttpPostedFile myFile = filMyFile.PostedFile;
                // Get size of uploaded file
                int nFileLen = myFile.ContentLength;
                // Allocate a buffer for reading of the file
                byte[] myData = new byte[nFileLen];
                // Read uploaded file from the Stream
                myFile.InputStream.Read(myData, 0, nFileLen);
                string strFilename = Path.GetFileName(myFile.FileName);
                string path = LUADefinitions.LuaScriptLocation + strFilename;

                WriteToFile(path, myData);
                controller.CreatePlugin(plugin);
                Response.Write(" Plugin " + strFilename + " has been added successfully");
            }
            else
            {
                // No file
                Response.Write("NO file attached. Plugin couldn't be added");
            }
        }
        catch (ArgumentNullException)
        {
            Response.Write("Owner username is Null");
        }
        catch (CouldNotFindException)
        {
            Response.Write("Could not find the owner ");
        }
        catch (EntryAlreadyExistsException)
        {
            Response.Write("Plugin Already Exists");
        }
        /**catch (SqlException)
        {
            Response.Write("SQL Exception: Error adding plugin");
        }*/
       

    }


    // Writes file to current folder
    private void WriteToFile(string strPath, byte[] Buffer)
    {
        // Create a file
        FileStream newFile = new FileStream(strPath, FileMode.Create);
        // Write data to the file
        newFile.Write(Buffer, 0, Buffer.Length);
        // Close file
        newFile.Close();
    }


}