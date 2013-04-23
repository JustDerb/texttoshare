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
    private UserDAO _currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        _currentUser = Session["userDAO"] as UserDAO;

        PageTitle.Text = "Text2Share - Add Plugin";
    }

    public void AddPlugin_Click(Object sender, EventArgs e)
    {
        PluginDAO plugin = new PluginDAO()
        {
            Name = Request["pluginNameBox"],
            Description = Request["pluginDescriptionBox"],
            HelpText = Request["helpTextBox"],
            IsDisabled = false,
            VersionNum = Request["versionBox"],
            OwnerID = _currentUser.UserID
        };

        if (!System.IO.Path.GetExtension(filMyFile.PostedFile.FileName).EndsWith(".lua"))
        {
            invalidPlugin.Text = @"The selected file must be a file with extension "".lua"" to upload.";
            filMyFile.Focus();
            return;
        }

        IDBController controller = new SqlController();

        // Request.fil
        try
        {
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
                invalidPlugin.Text = "No file attached. Plugin couldn't be added";
                return;
            }
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException)
        {
            // Shouldn't happen
        }
        catch (EntryAlreadyExistsException)
        {
            invalidPlugin.Text = "A plugin with that name already exists. Please try again.";
            pluginNameBox.Focus();
            return;
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