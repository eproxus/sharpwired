#region Information and licence agreements
/*
 * FileDetailsControl.cs
 * Created by Ola Lindberg and Peter Holmdahl, 2007-09-29
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
using SharpWired.Model.Files;
using SharpWired.Gui.Resources.Icons;
using System.Diagnostics;
using SharpWired.Model;
using SharpWired.Controller;

namespace SharpWired.Gui.Files {
    /// <summary>
    /// Represents a detail view of the currently selected nodes
    /// </summary>
    public partial class Details : SharpWiredGuiBase {

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Details() {
            InitializeComponent();
        }
        #endregion

        #region Events & Listeners
        public delegate void NodeDelegate(FileSystemEntry node);
        public event NodeDelegate SelectFolderNodeChange;
        public event NodeDelegate RequestDownload;

        delegate void NodeListToListViewCallback(List<FileSystemEntry> update);

        public void OnSelectedFolderNodeChanged(object sender, WiredNodeArgs selectedNode) {
            detailsListView.Clear();
            if (selectedNode.Node is FolderNode)
                ((FolderNode)selectedNode.Node).FolderNodeUpdatedEvent += OnFolderNodeDoneLoading;
        }

        /// <summary>
        /// Display the given nodes in the details view.
        /// </summary>
        /// <param name="updatedNode"></param>
        void OnFolderNodeDoneLoading(FolderNode updatedNode) {
            updatedNode.FolderNodeUpdatedEvent -= OnFolderNodeDoneLoading;
            NodeListToListView(updatedNode.Children);
        }

        private void detailsListView_MouseClick(object sender, MouseEventArgs e) {
            // We use right click to download for now at least
            if (e.Button == MouseButtons.Right)
                RequestDownload(((WiredListNode)detailsListView.GetItemAt(e.X, e.Y)).ModelNode);
        }

        private void detailsListView_MouseDoubleClick(object sender, MouseEventArgs e) {
            WiredListNode node = (WiredListNode)detailsListView.GetItemAt(e.X, e.Y);
            if (node != null
                && node.ModelNode is FolderNode
                && SelectFolderNodeChange != null) {
                SelectFolderNodeChange(node.ModelNode);
            }
        }

        private void detailsListView_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                if (detailsListView.SelectedItems.Count == 1) {
                    FileSystemEntry n = ((WiredListNode)detailsListView.SelectedItems[0]).ModelNode;
                    if (n != null && SelectFolderNodeChange != null)
                        SelectFolderNodeChange(n);
                }
            }
        }
        #endregion

        #region Methods
        public override void Init(SharpWiredModel model, SharpWiredController controller) {
            base.Init(model, controller);

            ImageList fileViewIcons = new ImageList();
            fileViewIcons.ColorDepth = ColorDepth.Depth32Bit;
            IconHandler iconHandler = new IconHandler();

            try {
                fileViewIcons.Images.Add(iconHandler.FolderClosed);
                fileViewIcons.Images.Add(iconHandler.File);
            } catch (Exception e) {
                Debug.WriteLine("FileUserControl.cs | Failed to add images for rootTreView. Exception: " + e); //TODO: Throw exception
            }

            detailsListView.SmallImageList = fileViewIcons;
            detailsListView.LargeImageList = fileViewIcons;
            detailsListView.View = View.Details;
        }

        /// <summary>
        /// Replace the items in this listview with the given list
        /// </summary>
        /// <param name="newNodes"></param>
        private void NodeListToListView(List<FileSystemEntry> newNodes) {
            if (this.InvokeRequired) {
                NodeListToListViewCallback callback = new NodeListToListViewCallback(NodeListToListView);
                this.Invoke(callback, new object[] { newNodes });
            } else {
                detailsListView.Columns.Clear();

                detailsListView.Sorting = SortOrder.Ascending; //TODO: Sort folders before files
                detailsListView.LabelEdit = true;
                detailsListView.AllowColumnReorder = true;

                detailsListView.Columns.Add("Name", 200);
                detailsListView.Columns.Add("Size", 50, HorizontalAlignment.Right);
                detailsListView.Columns.Add("Added", 150);
                detailsListView.Columns.Add("Modified", 150);

                detailsListView.Items.Clear();
                foreach (FileSystemEntry child in newNodes) {
                    WiredListNode wln = new WiredListNode(child);
                    wln.ImageIndex = wln.IconIndex;
                    wln.StateImageIndex = wln.IconIndex;
                    wln.SubItems.Add(wln.Size);
                    wln.SubItems.Add(wln.Created.ToString());
                    wln.SubItems.Add(wln.Modified.ToString());

                    this.detailsListView.Items.Add(wln);
                }
            }
        }
        #endregion
    }
}
