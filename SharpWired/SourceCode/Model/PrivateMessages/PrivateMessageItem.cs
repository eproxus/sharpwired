#region Information and licence agreements
/*
 * PrivateMessageItem.cs 
 * Created by Ola Lindberg, 2007-12-20
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
using SharpWired.Model.Users;

namespace SharpWired.Model.PrivateMessages
{
    /// <summary>
    /// Represents one private message - Sent or received
    /// </summary>
    public class PrivateMessageItem
    {
        private UserItem userItem;
        private string message;
        private DateTime timeStamp;

        /// <summary>
        /// Get the user sending or receiving this message
        /// </summary>
        public UserItem UserItem
        {
            get { return userItem; }
        }

        /// <summary>
        /// Get the private message
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Get the timestamp for this message.
        /// NOTE! Wired protocol doesn't have support for this so it is the
        /// time when the message was received or sent.
        /// </summary>
        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrivateMessageItem(UserItem user, String message)
        {
            this.userItem = user;
            this.message = message;
            this.timeStamp = DateTime.Now;
        }
    }
}