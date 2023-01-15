
namespace myLoginForm//PhotoEditor00001
{
    partial class LoginForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblUserIdentity = new System.Windows.Forms.Label();
            this.txtBxUserIdentity = new System.Windows.Forms.TextBox();
            this.txtBxUserPassword = new System.Windows.Forms.TextBox();
            this.lblUserPassword = new System.Windows.Forms.Label();
            this.btnCheckCred = new System.Windows.Forms.Button();
            this.btnEditUserData = new System.Windows.Forms.Button();
            this.lblUserRights = new System.Windows.Forms.Label();
            this.cmbbxUserRights = new System.Windows.Forms.ComboBox();
            this.lblPosition = new System.Windows.Forms.Label();
            this.nudLeftBorder = new System.Windows.Forms.NumericUpDown();
            this.nudRightBorder = new System.Windows.Forms.NumericUpDown();
            this.nudTopBorder = new System.Windows.Forms.NumericUpDown();
            this.nudBottomBorder = new System.Windows.Forms.NumericUpDown();
            this.lblImageDisplayDimensions = new System.Windows.Forms.Label();
            this.lblSmallImage = new System.Windows.Forms.Label();
            this.nudSmallImageHeight = new System.Windows.Forms.NumericUpDown();
            this.nudSmallImageWidth = new System.Windows.Forms.NumericUpDown();
            this.nudLargeImageHeight = new System.Windows.Forms.NumericUpDown();
            this.nudLargeImageWidth = new System.Windows.Forms.NumericUpDown();
            this.lblLargeImageDimensions = new System.Windows.Forms.Label();
            this.lblCategoryDefs = new System.Windows.Forms.Label();
            this.lblMotifType = new System.Windows.Forms.Label();
            this.cmbBxExistingMotifTypes = new System.Windows.Forms.ComboBox();
            this.txtBxMotifTypeAdding = new System.Windows.Forms.TextBox();
            this.btnAddMotifType = new System.Windows.Forms.Button();
            this.btnAddEventType = new System.Windows.Forms.Button();
            this.txtbxEventTypeToAdd = new System.Windows.Forms.TextBox();
            this.cmbBxExistingEventTypes = new System.Windows.Forms.ComboBox();
            this.lblEventType = new System.Windows.Forms.Label();
            this.btnAddContentType = new System.Windows.Forms.Button();
            this.txtBxContentTypeToAdd = new System.Windows.Forms.TextBox();
            this.cmbBxExistingContentTypes = new System.Windows.Forms.ComboBox();
            this.lblContentType = new System.Windows.Forms.Label();
            this.btnAddRelationType = new System.Windows.Forms.Button();
            this.txtBxRelationTypeToAdd = new System.Windows.Forms.TextBox();
            this.cmbBxExistingRelationTypes = new System.Windows.Forms.ComboBox();
            this.lblRelationType = new System.Windows.Forms.Label();
            this.btnAddNationalityType = new System.Windows.Forms.Button();
            this.txtBxNationalityTypeToAdd = new System.Windows.Forms.TextBox();
            this.cmbBxExistingNationalityTypes = new System.Windows.Forms.ComboBox();
            this.lblNationalityType = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeftBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRightBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBottomBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSmallImageHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSmallImageWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLargeImageHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLargeImageWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.No;
            this.pictureBox1.Location = new System.Drawing.Point(6, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(333, 346);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblUserIdentity
            // 
            this.lblUserIdentity.AutoSize = true;
            this.lblUserIdentity.Location = new System.Drawing.Point(3, 361);
            this.lblUserIdentity.Name = "lblUserIdentity";
            this.lblUserIdentity.Size = new System.Drawing.Size(84, 13);
            this.lblUserIdentity.TabIndex = 500;
            this.lblUserIdentity.Text = "User Identity     :";
            // 
            // txtBxUserIdentity
            // 
            this.txtBxUserIdentity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBxUserIdentity.Location = new System.Drawing.Point(135, 359);
            this.txtBxUserIdentity.Name = "txtBxUserIdentity";
            this.txtBxUserIdentity.Size = new System.Drawing.Size(203, 20);
            this.txtBxUserIdentity.TabIndex = 1;
            this.txtBxUserIdentity.TextChanged += new System.EventHandler(this.txtBxUserIdentity_TextChanged);
            // 
            // txtBxUserPassword
            // 
            this.txtBxUserPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBxUserPassword.Enabled = false;
            this.txtBxUserPassword.Location = new System.Drawing.Point(135, 385);
            this.txtBxUserPassword.Name = "txtBxUserPassword";
            this.txtBxUserPassword.PasswordChar = '*';
            this.txtBxUserPassword.Size = new System.Drawing.Size(204, 20);
            this.txtBxUserPassword.TabIndex = 2;
            this.txtBxUserPassword.UseSystemPasswordChar = true;
            this.txtBxUserPassword.TextChanged += new System.EventHandler(this.txtBxUserPassword_TextChanged);
            // 
            // lblUserPassword
            // 
            this.lblUserPassword.AutoSize = true;
            this.lblUserPassword.Location = new System.Drawing.Point(3, 387);
            this.lblUserPassword.Name = "lblUserPassword";
            this.lblUserPassword.Size = new System.Drawing.Size(84, 13);
            this.lblUserPassword.TabIndex = 501;
            this.lblUserPassword.Text = "User Password :";
            // 
            // btnCheckCred
            // 
            this.btnCheckCred.BackColor = System.Drawing.SystemColors.Control;
            this.btnCheckCred.Enabled = false;
            this.btnCheckCred.Location = new System.Drawing.Point(6, 410);
            this.btnCheckCred.Name = "btnCheckCred";
            this.btnCheckCred.Size = new System.Drawing.Size(170, 28);
            this.btnCheckCred.TabIndex = 502;
            this.btnCheckCred.Text = "Check Credentials";
            this.btnCheckCred.UseVisualStyleBackColor = false;
            this.btnCheckCred.Click += new System.EventHandler(this.btnCheckCred_Click);
            // 
            // btnEditUserData
            // 
            this.btnEditUserData.BackColor = System.Drawing.SystemColors.Control;
            this.btnEditUserData.Enabled = false;
            this.btnEditUserData.Location = new System.Drawing.Point(179, 410);
            this.btnEditUserData.Name = "btnEditUserData";
            this.btnEditUserData.Size = new System.Drawing.Size(161, 28);
            this.btnEditUserData.TabIndex = 503;
            this.btnEditUserData.Text = "Edit User Data";
            this.btnEditUserData.UseVisualStyleBackColor = false;
            this.btnEditUserData.Click += new System.EventHandler(this.btnEditUserData_Click);
            // 
            // lblUserRights
            // 
            this.lblUserRights.AutoSize = true;
            this.lblUserRights.Location = new System.Drawing.Point(353, 10);
            this.lblUserRights.Name = "lblUserRights";
            this.lblUserRights.Size = new System.Drawing.Size(77, 13);
            this.lblUserRights.TabIndex = 504;
            this.lblUserRights.Text = "User Rights    :";
            this.lblUserRights.Visible = false;
            // 
            // cmbbxUserRights
            // 
            this.cmbbxUserRights.BackColor = System.Drawing.SystemColors.Control;
            this.cmbbxUserRights.FormattingEnabled = true;
            this.cmbbxUserRights.Items.AddRange(new object[] {
            "Select...",
            "Open",
            "Limited",
            "Medium",
            "Relative",
            "Secret",
            "QualifiedSecret"});
            this.cmbbxUserRights.Location = new System.Drawing.Point(437, 7);
            this.cmbbxUserRights.Name = "cmbbxUserRights";
            this.cmbbxUserRights.Size = new System.Drawing.Size(182, 21);
            this.cmbbxUserRights.TabIndex = 3;
            this.cmbbxUserRights.Visible = false;
            this.cmbbxUserRights.SelectedIndexChanged += new System.EventHandler(this.cmbbxUserRights_SelectedIndexChanged);
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(353, 62);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(77, 13);
            this.lblPosition.TabIndex = 506;
            this.lblPosition.Text = "Position          :";
            this.lblPosition.Visible = false;
            // 
            // nudLeftBorder
            // 
            this.nudLeftBorder.Location = new System.Drawing.Point(437, 60);
            this.nudLeftBorder.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudLeftBorder.Name = "nudLeftBorder";
            this.nudLeftBorder.Size = new System.Drawing.Size(88, 20);
            this.nudLeftBorder.TabIndex = 5;
            this.nudLeftBorder.Visible = false;
            this.nudLeftBorder.ValueChanged += new System.EventHandler(this.nudLeftBorder_ValueChanged);
            // 
            // nudRightBorder
            // 
            this.nudRightBorder.Location = new System.Drawing.Point(531, 60);
            this.nudRightBorder.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.nudRightBorder.Name = "nudRightBorder";
            this.nudRightBorder.Size = new System.Drawing.Size(88, 20);
            this.nudRightBorder.TabIndex = 6;
            this.nudRightBorder.Visible = false;
            this.nudRightBorder.ValueChanged += new System.EventHandler(this.nudRightBorder_ValueChanged);
            // 
            // nudTopBorder
            // 
            this.nudTopBorder.Location = new System.Drawing.Point(483, 34);
            this.nudTopBorder.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudTopBorder.Name = "nudTopBorder";
            this.nudTopBorder.Size = new System.Drawing.Size(88, 20);
            this.nudTopBorder.TabIndex = 4;
            this.nudTopBorder.Visible = false;
            this.nudTopBorder.ValueChanged += new System.EventHandler(this.nudTopBorder_ValueChanged);
            // 
            // nudBottomBorder
            // 
            this.nudBottomBorder.Location = new System.Drawing.Point(483, 86);
            this.nudBottomBorder.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.nudBottomBorder.Name = "nudBottomBorder";
            this.nudBottomBorder.Size = new System.Drawing.Size(88, 20);
            this.nudBottomBorder.TabIndex = 7;
            this.nudBottomBorder.Visible = false;
            this.nudBottomBorder.ValueChanged += new System.EventHandler(this.nudBottomBorder_ValueChanged);
            // 
            // lblImageDisplayDimensions
            // 
            this.lblImageDisplayDimensions.AutoSize = true;
            this.lblImageDisplayDimensions.Location = new System.Drawing.Point(399, 109);
            this.lblImageDisplayDimensions.Name = "lblImageDisplayDimensions";
            this.lblImageDisplayDimensions.Size = new System.Drawing.Size(172, 13);
            this.lblImageDisplayDimensions.TabIndex = 511;
            this.lblImageDisplayDimensions.Text = "------ Image Display Dimensions ------";
            this.lblImageDisplayDimensions.Visible = false;
            // 
            // lblSmallImage
            // 
            this.lblSmallImage.AutoSize = true;
            this.lblSmallImage.Location = new System.Drawing.Point(353, 129);
            this.lblSmallImage.Name = "lblSmallImage";
            this.lblSmallImage.Size = new System.Drawing.Size(73, 13);
            this.lblSmallImage.TabIndex = 512;
            this.lblSmallImage.Text = "Small Image  :";
            this.lblSmallImage.Visible = false;
            // 
            // nudSmallImageHeight
            // 
            this.nudSmallImageHeight.Location = new System.Drawing.Point(531, 127);
            this.nudSmallImageHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudSmallImageHeight.Name = "nudSmallImageHeight";
            this.nudSmallImageHeight.Size = new System.Drawing.Size(88, 20);
            this.nudSmallImageHeight.TabIndex = 9;
            this.nudSmallImageHeight.Visible = false;
            this.nudSmallImageHeight.ValueChanged += new System.EventHandler(this.nudSmallImageHeight_ValueChanged);
            // 
            // nudSmallImageWidth
            // 
            this.nudSmallImageWidth.Location = new System.Drawing.Point(437, 127);
            this.nudSmallImageWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudSmallImageWidth.Name = "nudSmallImageWidth";
            this.nudSmallImageWidth.Size = new System.Drawing.Size(88, 20);
            this.nudSmallImageWidth.TabIndex = 8;
            this.nudSmallImageWidth.Visible = false;
            this.nudSmallImageWidth.ValueChanged += new System.EventHandler(this.nudSmallImageWidth_ValueChanged);
            // 
            // nudLargeImageHeight
            // 
            this.nudLargeImageHeight.Location = new System.Drawing.Point(531, 150);
            this.nudLargeImageHeight.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.nudLargeImageHeight.Name = "nudLargeImageHeight";
            this.nudLargeImageHeight.Size = new System.Drawing.Size(88, 20);
            this.nudLargeImageHeight.TabIndex = 11;
            this.nudLargeImageHeight.Visible = false;
            this.nudLargeImageHeight.ValueChanged += new System.EventHandler(this.nudLargeImageHeight_ValueChanged);
            // 
            // nudLargeImageWidth
            // 
            this.nudLargeImageWidth.Location = new System.Drawing.Point(437, 150);
            this.nudLargeImageWidth.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.nudLargeImageWidth.Name = "nudLargeImageWidth";
            this.nudLargeImageWidth.Size = new System.Drawing.Size(88, 20);
            this.nudLargeImageWidth.TabIndex = 10;
            this.nudLargeImageWidth.Visible = false;
            this.nudLargeImageWidth.ValueChanged += new System.EventHandler(this.nudLargeImageWidth_ValueChanged);
            // 
            // lblLargeImageDimensions
            // 
            this.lblLargeImageDimensions.AutoSize = true;
            this.lblLargeImageDimensions.Location = new System.Drawing.Point(353, 152);
            this.lblLargeImageDimensions.Name = "lblLargeImageDimensions";
            this.lblLargeImageDimensions.Size = new System.Drawing.Size(75, 13);
            this.lblLargeImageDimensions.TabIndex = 515;
            this.lblLargeImageDimensions.Text = "Large Image  :";
            this.lblLargeImageDimensions.Visible = false;
            // 
            // lblCategoryDefs
            // 
            this.lblCategoryDefs.AutoSize = true;
            this.lblCategoryDefs.Location = new System.Drawing.Point(415, 175);
            this.lblCategoryDefs.Name = "lblCategoryDefs";
            this.lblCategoryDefs.Size = new System.Drawing.Size(143, 13);
            this.lblCategoryDefs.TabIndex = 516;
            this.lblCategoryDefs.Text = "------ Category Definitions ------";
            this.lblCategoryDefs.Visible = false;
            // 
            // lblMotifType
            // 
            this.lblMotifType.AutoSize = true;
            this.lblMotifType.Location = new System.Drawing.Point(353, 196);
            this.lblMotifType.Name = "lblMotifType";
            this.lblMotifType.Size = new System.Drawing.Size(72, 13);
            this.lblMotifType.TabIndex = 517;
            this.lblMotifType.Text = "Motif Type    :";
            this.lblMotifType.Visible = false;
            // 
            // cmbBxExistingMotifTypes
            // 
            this.cmbBxExistingMotifTypes.BackColor = System.Drawing.SystemColors.Control;
            this.cmbBxExistingMotifTypes.FormattingEnabled = true;
            this.cmbBxExistingMotifTypes.Location = new System.Drawing.Point(437, 192);
            this.cmbBxExistingMotifTypes.Name = "cmbBxExistingMotifTypes";
            this.cmbBxExistingMotifTypes.Size = new System.Drawing.Size(182, 21);
            this.cmbBxExistingMotifTypes.TabIndex = 11;
            this.cmbBxExistingMotifTypes.Visible = false;
            this.cmbBxExistingMotifTypes.SelectedIndexChanged += new System.EventHandler(this.cmbBxExistingMotifTypes_SelectedIndexChanged);
            // 
            // txtBxMotifTypeAdding
            // 
            this.txtBxMotifTypeAdding.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBxMotifTypeAdding.Location = new System.Drawing.Point(356, 216);
            this.txtBxMotifTypeAdding.Name = "txtBxMotifTypeAdding";
            this.txtBxMotifTypeAdding.Size = new System.Drawing.Size(263, 20);
            this.txtBxMotifTypeAdding.TabIndex = 518;
            this.txtBxMotifTypeAdding.Visible = false;
            this.txtBxMotifTypeAdding.TextChanged += new System.EventHandler(this.txtBxMotifTypeAdding_TextChanged);
            // 
            // btnAddMotifType
            // 
            this.btnAddMotifType.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddMotifType.Enabled = false;
            this.btnAddMotifType.Location = new System.Drawing.Point(565, 216);
            this.btnAddMotifType.Name = "btnAddMotifType";
            this.btnAddMotifType.Size = new System.Drawing.Size(54, 20);
            this.btnAddMotifType.TabIndex = 519;
            this.btnAddMotifType.Text = "ADD";
            this.btnAddMotifType.UseVisualStyleBackColor = false;
            this.btnAddMotifType.Click += new System.EventHandler(this.btnAddMotifType_Click);
            // 
            // btnAddEventType
            // 
            this.btnAddEventType.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddEventType.Enabled = false;
            this.btnAddEventType.Location = new System.Drawing.Point(565, 266);
            this.btnAddEventType.Name = "btnAddEventType";
            this.btnAddEventType.Size = new System.Drawing.Size(54, 20);
            this.btnAddEventType.TabIndex = 523;
            this.btnAddEventType.Text = "ADD";
            this.btnAddEventType.UseVisualStyleBackColor = false;
            this.btnAddEventType.Click += new System.EventHandler(this.btnAddEventType_Click);
            // 
            // txtbxEventTypeToAdd
            // 
            this.txtbxEventTypeToAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtbxEventTypeToAdd.Location = new System.Drawing.Point(356, 266);
            this.txtbxEventTypeToAdd.Name = "txtbxEventTypeToAdd";
            this.txtbxEventTypeToAdd.Size = new System.Drawing.Size(263, 20);
            this.txtbxEventTypeToAdd.TabIndex = 12;
            this.txtbxEventTypeToAdd.Visible = false;
            this.txtbxEventTypeToAdd.TextChanged += new System.EventHandler(this.txtbxEventTypeToAdd_TextChanged);
            // 
            // cmbBxExistingEventTypes
            // 
            this.cmbBxExistingEventTypes.BackColor = System.Drawing.SystemColors.Control;
            this.cmbBxExistingEventTypes.FormattingEnabled = true;
            this.cmbBxExistingEventTypes.Location = new System.Drawing.Point(437, 242);
            this.cmbBxExistingEventTypes.Name = "cmbBxExistingEventTypes";
            this.cmbBxExistingEventTypes.Size = new System.Drawing.Size(182, 21);
            this.cmbBxExistingEventTypes.TabIndex = 12;
            this.cmbBxExistingEventTypes.Visible = false;
            this.cmbBxExistingEventTypes.SelectedIndexChanged += new System.EventHandler(this.cmbBxExistingEventTypes_SelectedIndexChanged);
            // 
            // lblEventType
            // 
            this.lblEventType.AutoSize = true;
            this.lblEventType.Location = new System.Drawing.Point(353, 246);
            this.lblEventType.Name = "lblEventType";
            this.lblEventType.Size = new System.Drawing.Size(74, 13);
            this.lblEventType.TabIndex = 521;
            this.lblEventType.Text = "Event Type   :";
            this.lblEventType.Visible = false;
            // 
            // btnAddContentType
            // 
            this.btnAddContentType.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddContentType.Enabled = false;
            this.btnAddContentType.Location = new System.Drawing.Point(565, 316);
            this.btnAddContentType.Name = "btnAddContentType";
            this.btnAddContentType.Size = new System.Drawing.Size(54, 20);
            this.btnAddContentType.TabIndex = 527;
            this.btnAddContentType.Text = "ADD";
            this.btnAddContentType.UseVisualStyleBackColor = false;
            this.btnAddContentType.Click += new System.EventHandler(this.btnAddContentType_Click);
            // 
            // txtBxContentTypeToAdd
            // 
            this.txtBxContentTypeToAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBxContentTypeToAdd.Location = new System.Drawing.Point(356, 316);
            this.txtBxContentTypeToAdd.Name = "txtBxContentTypeToAdd";
            this.txtBxContentTypeToAdd.Size = new System.Drawing.Size(263, 20);
            this.txtBxContentTypeToAdd.TabIndex = 524;
            this.txtBxContentTypeToAdd.Visible = false;
            this.txtBxContentTypeToAdd.TextChanged += new System.EventHandler(this.txtBxContentTypeToAdd_TextChanged);
            // 
            // cmbBxExistingContentTypes
            // 
            this.cmbBxExistingContentTypes.BackColor = System.Drawing.SystemColors.Control;
            this.cmbBxExistingContentTypes.FormattingEnabled = true;
            this.cmbBxExistingContentTypes.Location = new System.Drawing.Point(437, 292);
            this.cmbBxExistingContentTypes.Name = "cmbBxExistingContentTypes";
            this.cmbBxExistingContentTypes.Size = new System.Drawing.Size(182, 21);
            this.cmbBxExistingContentTypes.TabIndex = 525;
            this.cmbBxExistingContentTypes.Visible = false;
            this.cmbBxExistingContentTypes.SelectedIndexChanged += new System.EventHandler(this.cmbBxExistingContentTypes_SelectedIndexChanged);
            // 
            // lblContentType
            // 
            this.lblContentType.AutoSize = true;
            this.lblContentType.Location = new System.Drawing.Point(353, 296);
            this.lblContentType.Name = "lblContentType";
            this.lblContentType.Size = new System.Drawing.Size(80, 13);
            this.lblContentType.TabIndex = 526;
            this.lblContentType.Text = "Content Type  :";
            this.lblContentType.Visible = false;
            // 
            // btnAddRelationType
            // 
            this.btnAddRelationType.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddRelationType.Enabled = false;
            this.btnAddRelationType.Location = new System.Drawing.Point(565, 367);
            this.btnAddRelationType.Name = "btnAddRelationType";
            this.btnAddRelationType.Size = new System.Drawing.Size(54, 20);
            this.btnAddRelationType.TabIndex = 531;
            this.btnAddRelationType.Text = "ADD";
            this.btnAddRelationType.UseVisualStyleBackColor = false;
            this.btnAddRelationType.Click += new System.EventHandler(this.btnAddRelationType_Click);
            // 
            // txtBxRelationTypeToAdd
            // 
            this.txtBxRelationTypeToAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBxRelationTypeToAdd.Location = new System.Drawing.Point(356, 367);
            this.txtBxRelationTypeToAdd.Name = "txtBxRelationTypeToAdd";
            this.txtBxRelationTypeToAdd.Size = new System.Drawing.Size(263, 20);
            this.txtBxRelationTypeToAdd.TabIndex = 528;
            this.txtBxRelationTypeToAdd.Visible = false;
            this.txtBxRelationTypeToAdd.TextChanged += new System.EventHandler(this.txtBxRelationTypeToAdd_TextChanged);
            // 
            // cmbBxExistingRelationTypes
            // 
            this.cmbBxExistingRelationTypes.BackColor = System.Drawing.SystemColors.Control;
            this.cmbBxExistingRelationTypes.FormattingEnabled = true;
            this.cmbBxExistingRelationTypes.Location = new System.Drawing.Point(437, 343);
            this.cmbBxExistingRelationTypes.Name = "cmbBxExistingRelationTypes";
            this.cmbBxExistingRelationTypes.Size = new System.Drawing.Size(182, 21);
            this.cmbBxExistingRelationTypes.TabIndex = 529;
            this.cmbBxExistingRelationTypes.Visible = false;
            this.cmbBxExistingRelationTypes.SelectedIndexChanged += new System.EventHandler(this.cmbBxExistingRelationTypes_SelectedIndexChanged);
            // 
            // lblRelationType
            // 
            this.lblRelationType.AutoSize = true;
            this.lblRelationType.Location = new System.Drawing.Point(353, 347);
            this.lblRelationType.Name = "lblRelationType";
            this.lblRelationType.Size = new System.Drawing.Size(82, 13);
            this.lblRelationType.TabIndex = 530;
            this.lblRelationType.Text = "Relation Type  :";
            this.lblRelationType.Visible = false;
            // 
            // btnAddNationalityType
            // 
            this.btnAddNationalityType.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddNationalityType.Enabled = false;
            this.btnAddNationalityType.Location = new System.Drawing.Point(565, 417);
            this.btnAddNationalityType.Name = "btnAddNationalityType";
            this.btnAddNationalityType.Size = new System.Drawing.Size(54, 20);
            this.btnAddNationalityType.TabIndex = 535;
            this.btnAddNationalityType.Text = "ADD";
            this.btnAddNationalityType.UseVisualStyleBackColor = false;
            this.btnAddNationalityType.Click += new System.EventHandler(this.btnAddNationalityType_Click);
            // 
            // txtBxNationalityTypeToAdd
            // 
            this.txtBxNationalityTypeToAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBxNationalityTypeToAdd.Location = new System.Drawing.Point(356, 417);
            this.txtBxNationalityTypeToAdd.Name = "txtBxNationalityTypeToAdd";
            this.txtBxNationalityTypeToAdd.Size = new System.Drawing.Size(263, 20);
            this.txtBxNationalityTypeToAdd.TabIndex = 532;
            this.txtBxNationalityTypeToAdd.Visible = false;
            this.txtBxNationalityTypeToAdd.TextChanged += new System.EventHandler(this.txtBxNationalityTypeToAdd_TextChanged);
            // 
            // cmbBxExistingNationalityTypes
            // 
            this.cmbBxExistingNationalityTypes.BackColor = System.Drawing.SystemColors.Control;
            this.cmbBxExistingNationalityTypes.FormattingEnabled = true;
            this.cmbBxExistingNationalityTypes.Location = new System.Drawing.Point(437, 393);
            this.cmbBxExistingNationalityTypes.Name = "cmbBxExistingNationalityTypes";
            this.cmbBxExistingNationalityTypes.Size = new System.Drawing.Size(182, 21);
            this.cmbBxExistingNationalityTypes.TabIndex = 533;
            this.cmbBxExistingNationalityTypes.Visible = false;
            this.cmbBxExistingNationalityTypes.SelectedIndexChanged += new System.EventHandler(this.cmbBxExistingNationalityTypes_SelectedIndexChanged);
            // 
            // lblNationalityType
            // 
            this.lblNationalityType.AutoSize = true;
            this.lblNationalityType.Location = new System.Drawing.Point(353, 397);
            this.lblNationalityType.Name = "lblNationalityType";
            this.lblNationalityType.Size = new System.Drawing.Size(81, 13);
            this.lblNationalityType.TabIndex = 534;
            this.lblNationalityType.Text = "Nationality Tpe:";
            this.lblNationalityType.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 446);
            this.Controls.Add(this.btnAddNationalityType);
            this.Controls.Add(this.txtBxNationalityTypeToAdd);
            this.Controls.Add(this.cmbBxExistingNationalityTypes);
            this.Controls.Add(this.lblNationalityType);
            this.Controls.Add(this.btnAddRelationType);
            this.Controls.Add(this.txtBxRelationTypeToAdd);
            this.Controls.Add(this.cmbBxExistingRelationTypes);
            this.Controls.Add(this.lblRelationType);
            this.Controls.Add(this.btnAddContentType);
            this.Controls.Add(this.txtBxContentTypeToAdd);
            this.Controls.Add(this.cmbBxExistingContentTypes);
            this.Controls.Add(this.lblContentType);
            this.Controls.Add(this.btnAddEventType);
            this.Controls.Add(this.txtbxEventTypeToAdd);
            this.Controls.Add(this.cmbBxExistingEventTypes);
            this.Controls.Add(this.lblEventType);
            this.Controls.Add(this.btnAddMotifType);
            this.Controls.Add(this.txtBxMotifTypeAdding);
            this.Controls.Add(this.cmbBxExistingMotifTypes);
            this.Controls.Add(this.lblMotifType);
            this.Controls.Add(this.lblCategoryDefs);
            this.Controls.Add(this.nudLargeImageHeight);
            this.Controls.Add(this.nudLargeImageWidth);
            this.Controls.Add(this.lblLargeImageDimensions);
            this.Controls.Add(this.nudSmallImageHeight);
            this.Controls.Add(this.nudSmallImageWidth);
            this.Controls.Add(this.lblSmallImage);
            this.Controls.Add(this.lblImageDisplayDimensions);
            this.Controls.Add(this.nudBottomBorder);
            this.Controls.Add(this.nudTopBorder);
            this.Controls.Add(this.nudRightBorder);
            this.Controls.Add(this.nudLeftBorder);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.cmbbxUserRights);
            this.Controls.Add(this.lblUserRights);
            this.Controls.Add(this.btnEditUserData);
            this.Controls.Add(this.btnCheckCred);
            this.Controls.Add(this.txtBxUserPassword);
            this.Controls.Add(this.lblUserPassword);
            this.Controls.Add(this.txtBxUserIdentity);
            this.Controls.Add(this.lblUserIdentity);
            this.Controls.Add(this.pictureBox1);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLeftBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRightBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBottomBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSmallImageHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSmallImageWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLargeImageHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLargeImageWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblUserIdentity;
        private System.Windows.Forms.TextBox txtBxUserIdentity;
        private System.Windows.Forms.TextBox txtBxUserPassword;
        private System.Windows.Forms.Label lblUserPassword;
        private System.Windows.Forms.Button btnCheckCred;
        private System.Windows.Forms.Button btnEditUserData;
        private System.Windows.Forms.Label lblUserRights;
        private System.Windows.Forms.ComboBox cmbbxUserRights;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.NumericUpDown nudLeftBorder;
        private System.Windows.Forms.NumericUpDown nudRightBorder;
        private System.Windows.Forms.NumericUpDown nudTopBorder;
        private System.Windows.Forms.NumericUpDown nudBottomBorder;
        private System.Windows.Forms.Label lblImageDisplayDimensions;
        private System.Windows.Forms.Label lblSmallImage;
        private System.Windows.Forms.NumericUpDown nudSmallImageHeight;
        private System.Windows.Forms.NumericUpDown nudSmallImageWidth;
        private System.Windows.Forms.NumericUpDown nudLargeImageHeight;
        private System.Windows.Forms.NumericUpDown nudLargeImageWidth;
        private System.Windows.Forms.Label lblLargeImageDimensions;
        private System.Windows.Forms.Label lblCategoryDefs;
        private System.Windows.Forms.Label lblMotifType;
        private System.Windows.Forms.ComboBox cmbBxExistingMotifTypes;
        private System.Windows.Forms.TextBox txtBxMotifTypeAdding;
        private System.Windows.Forms.Button btnAddMotifType;
        private System.Windows.Forms.Button btnAddEventType;
        private System.Windows.Forms.TextBox txtbxEventTypeToAdd;
        private System.Windows.Forms.ComboBox cmbBxExistingEventTypes;
        private System.Windows.Forms.Label lblEventType;
        private System.Windows.Forms.Button btnAddContentType;
        private System.Windows.Forms.TextBox txtBxContentTypeToAdd;
        private System.Windows.Forms.ComboBox cmbBxExistingContentTypes;
        private System.Windows.Forms.Label lblContentType;
        private System.Windows.Forms.Button btnAddRelationType;
        private System.Windows.Forms.TextBox txtBxRelationTypeToAdd;
        private System.Windows.Forms.ComboBox cmbBxExistingRelationTypes;
        private System.Windows.Forms.Label lblRelationType;
        private System.Windows.Forms.Button btnAddNationalityType;
        private System.Windows.Forms.TextBox txtBxNationalityTypeToAdd;
        private System.Windows.Forms.ComboBox cmbBxExistingNationalityTypes;
        private System.Windows.Forms.Label lblNationalityType;
    }
}