#region Information and licence agreements
/*
 * SharpWiredMain.cs 
 * Created by Ola Lindberg, 2006-07-23
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

using SharpWired;
using SharpWired.Connection;
using SharpWired.Connection.Bookmarks;
using SharpWired.Model;
using SharpWired.Controller;
using SharpWired.Gui;

namespace SharpWired
{
    class SharpWiredMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            SharpWiredMain sharpWired = new SharpWiredMain();
        }

        public SharpWiredMain() {
            SharpWiredModel model = new SharpWiredModel();
            SharpWiredController sharpWiredController = new SharpWiredController(model);
            SharpWiredGui sharpWiredGui = new SharpWiredGui(model, sharpWiredController);
        }
    }
}
