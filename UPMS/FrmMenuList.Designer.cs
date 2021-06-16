namespace UPMS
{
    partial class FrmMenuList
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvMenus = new System.Windows.Forms.DataGridView();
            this.MenuId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MenuName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modify = new System.Windows.Forms.DataGridViewLinkColumn();
            this.deleteCol = new System.Windows.Forms.DataGridViewLinkColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMenus)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Location = new System.Drawing.Point(27, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(749, 100);
            this.panel1.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(48, 26);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "新增菜单";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvMenus
            // 
            this.dgvMenus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMenus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MenuId,
            this.MenuName,
            this.ParentId,
            this.modify,
            this.deleteCol});
            this.dgvMenus.Location = new System.Drawing.Point(27, 164);
            this.dgvMenus.Name = "dgvMenus";
            this.dgvMenus.RowTemplate.Height = 23;
            this.dgvMenus.Size = new System.Drawing.Size(749, 234);
            this.dgvMenus.TabIndex = 2;
            this.dgvMenus.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMenus_CellContentClick);
            // 
            // MenuId
            // 
            this.MenuId.DataPropertyName = "MenuId";
            this.MenuId.HeaderText = "编号";
            this.MenuId.Name = "MenuId";
            this.MenuId.ReadOnly = true;
            // 
            // MenuName
            // 
            this.MenuName.DataPropertyName = "MenuName";
            this.MenuName.HeaderText = "菜单名称";
            this.MenuName.Name = "MenuName";
            this.MenuName.ReadOnly = true;
            // 
            // ParentId
            // 
            this.ParentId.DataPropertyName = "ParentId";
            this.ParentId.HeaderText = "父级";
            this.ParentId.Name = "ParentId";
            this.ParentId.ReadOnly = true;
            // 
            // modify
            // 
            this.modify.HeaderText = "修改";
            this.modify.Name = "modify";
            this.modify.ReadOnly = true;
            this.modify.Text = "修改";
            this.modify.UseColumnTextForLinkValue = true;
            // 
            // deleteCol
            // 
            this.deleteCol.HeaderText = "删除";
            this.deleteCol.Name = "deleteCol";
            this.deleteCol.ReadOnly = true;
            this.deleteCol.Text = "删除";
            this.deleteCol.UseColumnTextForLinkValue = true;
            // 
            // FrmMenuList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvMenus);
            this.Controls.Add(this.panel1);
            this.Name = "FrmMenuList";
            this.Text = "FrmMenuList";
            this.Load += new System.EventHandler(this.FrmMenuList_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMenus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvMenus;
        private System.Windows.Forms.DataGridViewTextBoxColumn MenuId;
        private System.Windows.Forms.DataGridViewTextBoxColumn MenuName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentId;
        private System.Windows.Forms.DataGridViewLinkColumn modify;
        private System.Windows.Forms.DataGridViewLinkColumn deleteCol;
    }
}