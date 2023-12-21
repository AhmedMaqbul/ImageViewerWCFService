namespace ImageViewerWindowsServiceHost
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.imageWindosServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.imageWindowsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // imageWindosServiceProcessInstaller
            // 
            this.imageWindosServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.imageWindosServiceProcessInstaller.Password = null;
            this.imageWindosServiceProcessInstaller.Username = null;
            // 
            // imageWindowsServiceInstaller
            // 
            this.imageWindowsServiceInstaller.ServiceName = "ImageViewerWindowsService";
            this.imageWindowsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.imageWindosServiceProcessInstaller,
            this.imageWindowsServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller imageWindosServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller imageWindowsServiceInstaller;
    }
}