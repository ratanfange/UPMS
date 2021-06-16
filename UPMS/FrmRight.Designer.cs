namespace UPMS
{
    partial class FrmRight
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点5");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点6");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("江苏", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("节点2");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("节点4");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("中国", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6});
            this.panel1 = new System.Windows.Forms.Panel();
            this.tvMenus = new System.Windows.Forms.TreeView();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboRoles = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tvMenus);
            this.panel1.Location = new System.Drawing.Point(38, 135);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(717, 281);
            this.panel1.TabIndex = 0;
            // 
            // tvMenus
            // 
            this.tvMenus.CheckBoxes = true;
            this.tvMenus.Location = new System.Drawing.Point(0, 3);
            this.tvMenus.Name = "tvMenus";
            treeNode1.Checked = true;
            treeNode1.Name = "节点5";
            treeNode1.Text = "节点5";
            treeNode2.Checked = true;
            treeNode2.Name = "节点6";
            treeNode2.Text = "节点6";
            treeNode3.Checked = true;
            treeNode3.Name = "节点1";
            treeNode3.Text = "江苏";
            treeNode4.Checked = true;
            treeNode4.Name = "节点2";
            treeNode4.Text = "节点2";
            treeNode5.Checked = true;
            treeNode5.Name = "节点3";
            treeNode5.Text = "节点3";
            treeNode6.Checked = true;
            treeNode6.Name = "节点4";
            treeNode6.Text = "节点4";
            treeNode7.Checked = true;
            treeNode7.Name = "中国";
            treeNode7.Text = "中国";
            this.tvMenus.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7});
            this.tvMenus.Size = new System.Drawing.Size(711, 275);
            this.tvMenus.TabIndex = 0;
            this.tvMenus.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvMenus_AfterCheck);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(530, 68);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "保存";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "当前角色";
            // 
            // cboRoles
            // 
            this.cboRoles.FormattingEnabled = true;
            this.cboRoles.Location = new System.Drawing.Point(112, 68);
            this.cboRoles.Name = "cboRoles";
            this.cboRoles.Size = new System.Drawing.Size(171, 20);
            this.cboRoles.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(658, 68);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "退出";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(347, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "角色权限设置";
            // 
            // FrmRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cboRoles);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.panel1);
            this.Name = "FrmRight";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmRight";
            this.Load += new System.EventHandler(this.FrmRight_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView tvMenus;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboRoles;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
    }
}