namespace MESFeedClient
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
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.OracleItemMESService = new System.ServiceProcess.ServiceInstaller();
            this.OracleInventoryMESService = new System.ServiceProcess.ServiceInstaller();
            this.OracleWorkOrderMESService = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // OracleItemMESService
            // 
            this.OracleItemMESService.ServiceName = "OracleMESService";
            // 
            // OracleInventoryMESService
            // 
            this.OracleInventoryMESService.ServiceName = "OracleInventoryMESService";
            // 
            // OracleWorkOrderMESService
            // 
            this.OracleWorkOrderMESService.ServiceName = "OracleWorkOrderMESService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.OracleItemMESService,
            this.OracleInventoryMESService,
            this.OracleWorkOrderMESService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller OracleItemMESService;
        private System.ServiceProcess.ServiceInstaller OracleInventoryMESService;
        private System.ServiceProcess.ServiceInstaller OracleWorkOrderMESService;
    }
}