#region Information and licence agreements
/**
 * WiredTreeNode.cs 
 * Created by Ola Lindberg, 2007-05-30
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
using System.Text;
using System.Windows.Forms;
using SharpWired.Model.Files;

namespace SharpWired.Gui.Files
{
    /// <summary>
    /// Represents a node in the wired file tree. 
    /// Holds a reference to the node in the wired file tree model.
    /// </summary>
    class WiredTreeNode : TreeNode
    {
        private FileSystemEntry modelNode;
        private int iconIndex;

        /// <summary>
        /// Gets the model node for this tree node
        /// </summary>
        public FileSystemEntry ModelNode
        {
            get { return modelNode; }
        }

        /// <summary>
        /// Gets or sets the index for this nodes icon
        /// </summary>
        public int IconIndex
        {
            get { return iconIndex; }
            set { iconIndex = value; }
        }
	
        /// <summary>
        /// Constructor
        /// </summary>
        public WiredTreeNode(FileSystemEntry modelNode): base(modelNode.Name) {
            this.modelNode = modelNode;

            if (modelNode is FolderNode)
                iconIndex = 0;
            else if (modelNode is FileNode)
                iconIndex = 1;
        }

		/// <summary>
		/// A constructor that allows for creation of node with a text, as the
		/// base class TreeNode does.
		/// </summary>
		/// <param name="text">The text for the node.</param>
		public WiredTreeNode(string text): base(text)
		{
        }

        #region Events and event handler methods from GUI

        event MouseEventHandler DoubleClicked;
		
		internal void TriggerDoubleClicked(MouseEventArgs args)
		{
			OnDoubleClicked(args);
		}

		private void OnDoubleClicked(MouseEventArgs args)
		{
			if (DoubleClicked != null)
			{
				DoubleClicked(this, args);
			}
		}

        internal void TriggerClicked(MouseEventArgs args)
        {
            OnClicked(args);
        }

        private void OnClicked(MouseEventArgs args)
        {
        }
        #endregion
    }
}