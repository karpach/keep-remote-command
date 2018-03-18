using System;
using System.Drawing;
using System.Windows.Forms;

namespace Karpach.Remote.Keep.Command
{
    public class SampleCommandSettingsForm : Form
    {
        public readonly KeepCommandSettings Settings;

        private Button _btnOk;
        private Button _btnCancel;
        private Label _lbCommandName;
        private TextBox _txtCommandName;
        private Label _lbDelay;
        private Label _lbListId;
        private TextBox _txtListId;
        private Label _lbGoogleUserName;
        private TextBox _txtGmailEmail;
        private Label _lbGmailPassword;
        private TextBox _txtGmailPassword;
        private CheckBox _chkHeadless;
        private Label _lbChromeProfileFolder;
        private TextBox _txtChromeProfileFolder;
        private ToolTip _ttChromeProfileFolder;
        private System.ComponentModel.IContainer components;
        private NotifyIcon notifyIcon1;
        private TextBox _txtDelay;

        public SampleCommandSettingsForm(KeepCommandSettings settings)
        {
            InitializeComponent();
            Settings = settings;
            _txtCommandName.Text = Settings.CommandName;
            _txtDelay.Text = Settings.ExecutionDelay?.ToString() ?? "0";
            _txtGmailEmail.Text = Settings.GoogleUserName;
            _txtGmailPassword.Text = Settings.GooglePassword;
            _txtListId.Text = Settings.ListId;
            _chkHeadless.Checked = Settings.Headless;
            _txtChromeProfileFolder.Text = Settings.ChromeProfileFolder;
        }                

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleCommandSettingsForm));
            this._btnOk = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._lbCommandName = new System.Windows.Forms.Label();
            this._txtCommandName = new System.Windows.Forms.TextBox();
            this._lbDelay = new System.Windows.Forms.Label();
            this._txtDelay = new System.Windows.Forms.TextBox();
            this._lbListId = new System.Windows.Forms.Label();
            this._txtListId = new System.Windows.Forms.TextBox();
            this._lbGoogleUserName = new System.Windows.Forms.Label();
            this._txtGmailEmail = new System.Windows.Forms.TextBox();
            this._lbGmailPassword = new System.Windows.Forms.Label();
            this._txtGmailPassword = new System.Windows.Forms.TextBox();
            this._chkHeadless = new System.Windows.Forms.CheckBox();
            this._lbChromeProfileFolder = new System.Windows.Forms.Label();
            this._txtChromeProfileFolder = new System.Windows.Forms.TextBox();
            this._ttChromeProfileFolder = new System.Windows.Forms.ToolTip(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(167, 286);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 7;
            this._btnOk.Text = "Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(248, 286);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 8;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _lbCommandName
            // 
            this._lbCommandName.AutoSize = true;
            this._lbCommandName.Location = new System.Drawing.Point(83, 25);
            this._lbCommandName.Name = "_lbCommandName";
            this._lbCommandName.Size = new System.Drawing.Size(88, 13);
            this._lbCommandName.TabIndex = 0;
            this._lbCommandName.Text = "Command Name:";
            // 
            // _txtCommandName
            // 
            this._txtCommandName.Location = new System.Drawing.Point(186, 22);
            this._txtCommandName.Name = "_txtCommandName";
            this._txtCommandName.Size = new System.Drawing.Size(271, 20);
            this._txtCommandName.TabIndex = 0;
            // 
            // _lbDelay
            // 
            this._lbDelay.AutoSize = true;
            this._lbDelay.Location = new System.Drawing.Point(70, 61);
            this._lbDelay.Name = "_lbDelay";
            this._lbDelay.Size = new System.Drawing.Size(101, 13);
            this._lbDelay.TabIndex = 0;
            this._lbDelay.Text = "Execution delay ms:";
            // 
            // _txtDelay
            // 
            this._txtDelay.Location = new System.Drawing.Point(186, 58);
            this._txtDelay.Name = "_txtDelay";
            this._txtDelay.Size = new System.Drawing.Size(271, 20);
            this._txtDelay.TabIndex = 1;
            // 
            // _lbListId
            // 
            this._lbListId.AutoSize = true;
            this._lbListId.Location = new System.Drawing.Point(138, 176);
            this._lbListId.Name = "_lbListId";
            this._lbListId.Size = new System.Drawing.Size(38, 13);
            this._lbListId.TabIndex = 0;
            this._lbListId.Text = "List Id:";
            // 
            // _txtListId
            // 
            this._txtListId.Location = new System.Drawing.Point(186, 173);
            this._txtListId.Name = "_txtListId";
            this._txtListId.Size = new System.Drawing.Size(271, 20);
            this._txtListId.TabIndex = 4;
            // 
            // _lbGoogleUserName
            // 
            this._lbGoogleUserName.AutoSize = true;
            this._lbGoogleUserName.Location = new System.Drawing.Point(107, 98);
            this._lbGoogleUserName.Name = "_lbGoogleUserName";
            this._lbGoogleUserName.Size = new System.Drawing.Size(64, 13);
            this._lbGoogleUserName.TabIndex = 0;
            this._lbGoogleUserName.Text = "Gmail Email:";
            // 
            // _txtGmailEmail
            // 
            this._txtGmailEmail.Location = new System.Drawing.Point(186, 95);
            this._txtGmailEmail.Name = "_txtGmailEmail";
            this._txtGmailEmail.Size = new System.Drawing.Size(271, 20);
            this._txtGmailEmail.TabIndex = 2;
            // 
            // _lbGmailPassword
            // 
            this._lbGmailPassword.AutoSize = true;
            this._lbGmailPassword.Location = new System.Drawing.Point(86, 134);
            this._lbGmailPassword.Name = "_lbGmailPassword";
            this._lbGmailPassword.Size = new System.Drawing.Size(85, 13);
            this._lbGmailPassword.TabIndex = 0;
            this._lbGmailPassword.Text = "Gmail Password:";
            // 
            // _txtGmailPassword
            // 
            this._txtGmailPassword.Location = new System.Drawing.Point(186, 131);
            this._txtGmailPassword.Name = "_txtGmailPassword";
            this._txtGmailPassword.PasswordChar = '*';
            this._txtGmailPassword.Size = new System.Drawing.Size(271, 20);
            this._txtGmailPassword.TabIndex = 3;
            // 
            // _chkHeadless
            // 
            this._chkHeadless.AutoSize = true;
            this._chkHeadless.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkHeadless.Location = new System.Drawing.Point(15, 249);
            this._chkHeadless.Name = "_chkHeadless";
            this._chkHeadless.Size = new System.Drawing.Size(174, 17);
            this._chkHeadless.TabIndex = 6;
            this._chkHeadless.Text = "Headless Mode (experimental) :";
            this._chkHeadless.UseVisualStyleBackColor = true;
            // 
            // _lbChromeProfileFolder
            // 
            this._lbChromeProfileFolder.AutoSize = true;
            this._lbChromeProfileFolder.Location = new System.Drawing.Point(17, 213);
            this._lbChromeProfileFolder.Name = "_lbChromeProfileFolder";
            this._lbChromeProfileFolder.Size = new System.Drawing.Size(152, 13);
            this._lbChromeProfileFolder.TabIndex = 0;
            this._lbChromeProfileFolder.Text = "Optional Chrome Profile Folder:";
            // 
            // _txtChromeProfileFolder
            // 
            this._txtChromeProfileFolder.Location = new System.Drawing.Point(186, 210);
            this._txtChromeProfileFolder.Name = "_txtChromeProfileFolder";
            this._txtChromeProfileFolder.Size = new System.Drawing.Size(271, 20);
            this._txtChromeProfileFolder.TabIndex = 5;
            this._ttChromeProfileFolder.SetToolTip(this._txtChromeProfileFolder, "C:/Users/username/AppData/Local/Google/Chrome/User Data/Profile Test/");
            // 
            // _ttChromeProfileFolder
            // 
            this._ttChromeProfileFolder.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this._ttChromeProfileFolder.ToolTipTitle = "Example";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // SampleCommandSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 326);
            this.Controls.Add(this._chkHeadless);
            this.Controls.Add(this._txtChromeProfileFolder);
            this.Controls.Add(this._txtListId);
            this.Controls.Add(this._txtGmailPassword);
            this.Controls.Add(this._txtGmailEmail);
            this.Controls.Add(this._lbChromeProfileFolder);
            this.Controls.Add(this._txtDelay);
            this.Controls.Add(this._lbListId);
            this.Controls.Add(this._lbGmailPassword);
            this.Controls.Add(this._lbGoogleUserName);
            this.Controls.Add(this._txtCommandName);
            this.Controls.Add(this._lbDelay);
            this.Controls.Add(this._lbCommandName);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SampleCommandSettingsForm";
            this.Text = "Sample Command Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void _btnOk_Click(object sender, EventArgs e)
        {
            Settings.CommandName = _txtCommandName.Text;
            int n;
            Settings.ExecutionDelay = int.TryParse(_txtDelay.Text, out n) ? n : 0;
            Settings.ListId = _txtListId.Text;
            Settings.GoogleUserName = _txtGmailEmail.Text;
            Settings.GooglePassword = _txtGmailPassword.Text;
            Settings.Headless = _chkHeadless.Checked;
            Settings.ChromeProfileFolder = _txtChromeProfileFolder.Text;            
            Close();
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }       
    }
}
