namespace SharpWired.Gui.Files
{
    partial class FileTreeControl
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
            this.rootTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // rootTreeView
            // 
            this.rootTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootTreeView.Location = new System.Drawing.Point(0, 0);
            this.rootTreeView.Name = "rootTreeView";
            this.rootTreeView.Size = new System.Drawing.Size(336, 333);
            this.rootTreeView.TabIndex = 6;
            this.rootTreeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rootTreeView_MouseDoubleClick);
            this.rootTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rootTreeView_MouseClick);
            // 
            // FileTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rootTreeView);
            this.Name = "FileTreeControl";
            this.Size = new System.Drawing.Size(336, 333);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView rootTreeView;
    }
}
