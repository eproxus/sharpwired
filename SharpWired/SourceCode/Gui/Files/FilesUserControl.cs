#region Information and licence agreements
/**
 * FileUserControl.cs 
 * Created by Ola Lindberg and Peter Holmdahl, 2007-05-10
 * 
 * SharpWired - a Wired client.
 * See: http://www.zankasoftware.com/wired/ for more infromation about Wired
 * 
 * Copyright (C) Ola Lindberg (http://olalindberg.com)
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301 USA
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SharpWired.Model;
using SharpWired.Model.Files;
using System.Collections;
using SharpWired.Gui.Resources;
using SharpWired.Gui.Resources.Icons;
using SharpWired.Connection.Transfers;

namespace SharpWired.Gui.Files
{
    /// <summary>
    /// GUI file listing
    /// </summary>
    public partial class FilesUserControl : UserControl
    {
        #region Variables
        private LogicManager logicManager;
        string output = ""; // Keeps the output through itterations for temporary file listing. Can be removed once the textbox for file listing is obsoleted
        private int mSuspendCount = 0; //Counter for knowing if the cursor is suspended or not.
        #endregion

        #region Properties

        /// <summary>
        /// Gets true if the cursor is suspended. False otherwise.
        /// </summary>
        protected bool Suspended
        {
            get { return mSuspendCount > 0; }
        }

        #endregion

        void FileListingModel_FileListingDoneEvent(FolderNode superRootNode)
        {
            WriteTextToTexBox(textBox1, ""); //TODO: Remove
            output = ""; //TODO: Remove
            WriteTextToTexBox(textBox1, GetFileTreeOutput(superRootNode)); //TODO: Remove

            ClearTreeView(rootTreeView);
			PopulateFileTree(rootTreeView, superRootNode);
        }

        #region General GUI methods
        ///<summary>
        /// Suspends the Control, and sets the Cursor to a waitcursor, if it isn't already.
        ///</summary>
        protected void Suspend()
        {
            // When calling this method, the state is always Suspended.
            // But we only set the cursor if we aren't already suspended.
            // So, if counter is 0, we are the first to call this method, so we set the cursor.
            // But, we also need to increase the counter, and therefore add one.
            // Since the '++' comes after the field name, the increse will be done after
            // the logical check for == 0.
            if (mSuspendCount++ == 0)
                Cursor = Cursors.WaitCursor;
        }

        /// <summary>
        /// Unsuspends the cursor.
        /// </summary>
        /// <returns>True if the cursor isn't suspended.</returns>
        protected bool UnSuspend()
        {
            // Decrease the count (the -- is before the field name, so its decreased before the == 0).
            // And if we're done to zero again, we're no longer suspended and we set the cursor back.
            if (--mSuspendCount == 0)
                Cursor = Cursors.Default;
            return mSuspendCount == 0;
        }
        #endregion

        #region TreeNode
        /// <summary>
		/// Populates the filetree from the given super root.
		/// </summary>
		/// <remarks>Uses callback if necessary.</remarks>
		/// <param name="rootTreeView">The TreeView to populate.</param>
		/// <param name="superRootNode">The root node from the model to populate from.</param>
		private void PopulateFileTree(TreeView rootTreeView, FolderNode superRootNode)
		{
			if (InvokeRequired)
			{
				PopulateFileTreeCallBack callback = new PopulateFileTreeCallBack(PopulateFileTree);
				Invoke(callback, new object[] { rootTreeView, superRootNode });
				return;
			}

			if (rootTreeView.Nodes.Count > 0)
				rootTreeView.Nodes.Clear();

			// Just to put a name on the root in the filetree. alternatively,
			// the tree can skip the server root node, and have several nodes
			// at "level 0" in the tree.
            if (string.IsNullOrEmpty(superRootNode.Name))
                superRootNode.Name = "Server";
			rootTreeView.Nodes.Add(MakeFileNode(superRootNode));

			// Expand all nodes make the test easy and nice.
			rootTreeView.ExpandAll();
		}
        delegate void PopulateFileTreeCallBack(TreeView treeView, FolderNode rootNode);

		/// <summary>
		/// Takes a FileSystemEntry and build a subtree from that,
		/// returnung the WiredTreeNode that represent the FSE given.
		/// </summary>
		/// <param name="fileSystemEntry">The FSE to build a WiredNode from (a subtree).</param>
		/// <returns>The WiredNode that represents the given FSE.</returns>
		private WiredTreeNode MakeFileNode(FileSystemEntry fileSystemEntry)
		{
			if (fileSystemEntry != null)
			{
				WiredTreeNode node = new WiredTreeNode(fileSystemEntry);
                node.ImageIndex = node.IconIndex;
				node.SelectedImageIndex = node.IconIndex;
				if (fileSystemEntry is FolderNode
					&& fileSystemEntry.HasChildren())
				{
					List<FileSystemEntry> children = (fileSystemEntry as FolderNode).Children;
					foreach (FileSystemEntry child in children)
					{
						node.Nodes.Add(MakeFileNode(child));
					}
				}
				return node;
			}
			return new WiredTreeNode("The given node was null, but I didn't feel like trowing an exception!");
		}
        
        /// <summary>
        /// Clears the given tree view.
        /// </summary>
        /// <param name="tree"></param>
        private void ClearTreeView(TreeView tree)
        {
            if (this.InvokeRequired)
            {
                ClearTreeViewCallback clearTreeViewCallback = new ClearTreeViewCallback(ClearTreeView);
                this.Invoke(clearTreeViewCallback, new object[] { tree });
            }
            else
            {
                tree.Nodes.Clear();
            }
        }
        delegate void ClearTreeViewCallback(TreeView tree);

        #endregion

        #region TextBox

        /// <summary>
        /// Generates a string with all folders. TODO: Remove
        /// </summary>
        /// <param name="node"></param>
        private string GetFileTreeOutput(FolderNode node)
        {
            foreach (FileSystemEntry fn in node.Children)
            {
                if (fn is FolderNode)
                {
                    if (((FolderNode)fn).Children != null)
                    {
                        output += "FolderNode: " + fn.Path + System.Environment.NewLine;
                        GetFileTreeOutput((FolderNode)fn);
                    }
                    else
                    {
                        Console.Write("FolderNode " + fn.Path + " has no childrens");
                    }
                }
                else if (fn is FileNode)
                {
                    output += "  FileNode: " + fn.Path + System.Environment.NewLine;
                }
            }
            return output;
        }

        /// <summary>
        /// Writes the folder nodes to the text box. TODO: Remove
        /// </summary>
        /// <param name="textBoxToPopulate"></param>
        /// <param name="textToPopulate"></param>
        private void WriteTextToTexBox(TextBox textBoxToPopulate, string textToPopulate)
        {
            if (this.InvokeRequired)
            {
                WriteTextToTextBoxCallback writeTextToTextBoxCallback = new WriteTextToTextBoxCallback(WriteTextToTexBox);
                this.Invoke(writeTextToTextBoxCallback, new object[] { textBoxToPopulate, textToPopulate });
            }
            else
            {
                textBox1.Text = textToPopulate;
            }
        }
        delegate void WriteTextToTextBoxCallback(TextBox textBoxToPopulate, string textToPopulate);
        #endregion

        #region Listeners + handlers from GUI
        private void rootTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			WiredTreeNode node = (WiredTreeNode)rootTreeView.GetNodeAt(e.Location);
			if (node != null)
			{
				node.TriggerDoubleClicked(e);
				if (node.Tag is FileNode)
				{
					WantDownloadFile(node.Tag as FileNode);
				}
			}
		}

		private void WantDownloadFile(FileNode fileNode)
		{
			logicManager.FileTransferHandler.EnqueueDownload(
				logicManager.ConnectionManager.CurrentServer,
				fileNode,
				logicManager.FileTransferHandler.DefaultDownloadFolder);
		}

        private void rootTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            WiredTreeNode node = (WiredTreeNode)rootTreeView.GetNodeAt(e.Location);
            if (node != null && node.ModelNode is FolderNode)
            {
                Suspend();  //TODO: The suspend / unsuspend thing doesn't work this way since we just send a request to the server. 
                            //We should unsuspend when the request returns. Not when we have sent the request.
                node.TriggerClicked(e);
                this.logicManager.FileListingHandler.ReloadFileList(node.ModelNode.Path);
                UnSuspend();
            }
        }
        #endregion

        #region Initialization
        public void Init(LogicManager logicManager)
        {
            this.logicManager = logicManager;
            logicManager.FileListingHandler.FileListingModel.FileListingDoneEvent += new FileListingModel.FileListingDoneDelegate(FileListingModel_FileListingDoneEvent);
            ImageList rootTreeViewIcons = new ImageList();
            rootTreeViewIcons.ColorDepth = ColorDepth.Depth32Bit;
            IconHandler iconHandler = new IconHandler();

            try
            {
                rootTreeViewIcons.Images.Add(iconHandler.FolderClosed);
                rootTreeViewIcons.Images.Add(iconHandler.File);
            }
            catch (Exception e)
            {
                Console.WriteLine("FileUserControl.cs | Failed to add images for rootTreView. Exception: " + e); //TODO: Throw exception
            }

            rootTreeView.ImageList = rootTreeViewIcons;
        }

        public FilesUserControl()
        {
            InitializeComponent();
        }
        #endregion
    }
}
