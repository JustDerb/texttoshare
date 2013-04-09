<%@ Application Language="C#" %>


<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        //AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

        // Code that runs on application startup
        string LocalDatabase = AppDomain.CurrentDomain.BaseDirectory;// +@"..\t2sBackend\t2sBackend\bin\Debug\MainDatabase.mdf";
        LocalDatabase = LocalDatabase.Substring(0, LocalDatabase.IndexOf(@"\t2sBackendWebSite"));
        LocalDatabase += @"\t2sBackend\t2sBackend\bin\Debug\MainDatabase.mdf";
        SqlController.CONNECTION_STRING = @"Data Source=(LocalDB)\v11.0;Integrated Security=True;AttachDbFilename="+LocalDatabase;
            
         
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
