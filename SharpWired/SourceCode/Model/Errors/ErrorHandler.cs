#region Information and licence agreements
/*
 * ErrorHandler.cs 
 * Created by Ola Lindberg 2007-12-15
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
using SharpWired.Connection;
using SharpWired.Connection.Bookmarks;
using SharpWired.MessageEvents;

namespace SharpWired.Model.Errors
{
    /// <summary>
    /// Reads error messages from various sources (ie the connection layer) and 
    /// raises nicer error messages to consume by GUI
    /// </summary>
    public class ErrorHandler
    {
        private LogicManager logicManager;

        /// <summary>
        /// Report a Connection Exception
        /// </summary>
        /// <param name="ce"></param>
        public void ReportConnectionExceptionError(ConnectionException ce)
        {
            StringBuilder errorDescription = new StringBuilder();
            StringBuilder solutionIdea = new StringBuilder();
            if (ce.Message == "HostNotFound")
            {
                errorDescription.Append("The server you tried connecting to doesn't exist.");
                solutionIdea.Append("Make sure the host name for the server you tried connecting to is correct.");
            }
            else if(ce.Message == "NoRouteToTost")
            {
                errorDescription.Append("A socket operation was attempted to an unreachable host");
                solutionIdea.Append("Check the host name you tried connecting to. Make sure it's correct.");
            }
            else //TODO: Handle more types of errors. Add the errors to SecureSocket.cs as well
            {
                errorDescription.Append("An unknown error occured.");
                solutionIdea.Append("Please report this error in the SharpWired bug tracker at http://sourceforge.net/tracker/?group_id=187504&atid=921567. Error message is: " + ce);
            }

            LoginToServerFailedEvent(errorDescription.ToString(), solutionIdea.ToString(), ce.Bookmark);
        }

        /// <summary>
        /// Delegate for ConnectionErrorEvent
        /// </summary>
        /// <param name="errorDescription"></param>
        /// <param name="solutionIdea"></param>
        /// <param name="bookmark"></param>
        public delegate void LoginToServerFailedDelegate(string errorDescription, string solutionIdea, Bookmark bookmark);
        /// <summary>
        /// Event triggered when loggin in to the server failed
        /// </summary>
        public event LoginToServerFailedDelegate LoginToServerFailedEvent;

        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorHandler(LogicManager logicManager)
        {
            this.logicManager = logicManager;

            logicManager.ConnectionManager.Messages.LoginFailedEvent += new Messages.LoginFailedEventHandler(Messages_LoginFailedEvent);
            logicManager.ConnectionManager.Messages.BannedEvent += new Messages.BannedEventHandler(Messages_BannedEvent);
            logicManager.ConnectionManager.Messages.ClientNotFoundEvent += new Messages.ClientNotFoundEventHandler(Messages_ClientNotFoundEvent);
        }

        #region Listeners to connection layer - messages
        void Messages_LoginFailedEvent(object sender, MessageEventArgs_Messages messageEventArgs)
        {
            Bookmark currentBookmark = logicManager.ConnectionManager.CurrentBookmark;

            StringBuilder errorDescription = new StringBuilder();
            StringBuilder solutionIdea = new StringBuilder();

            errorDescription.Append("Login failure caused by a problem with the login or password when connecting to " + currentBookmark.Server.ServerName + ".");
            solutionIdea.Append("Check your login name and password and try again.");

            LoginToServerFailedEvent(errorDescription.ToString(), solutionIdea.ToString(), currentBookmark);
        }

        void Messages_ClientNotFoundEvent(object sender, MessageEventArgs_Messages messageEventArgs)
        {
            Bookmark currentBookmark = logicManager.ConnectionManager.CurrentBookmark;

            StringBuilder errorDescription = new StringBuilder();
            StringBuilder solutionIdea = new StringBuilder();

            errorDescription.Append("Login failure since the login name was not found on the server " + currentBookmark.Server.ServerName + ".");
            solutionIdea.Append("Check your login name and try again.");

            LoginToServerFailedEvent(errorDescription.ToString(), solutionIdea.ToString(), currentBookmark);
        }

        void Messages_BannedEvent(object sender, MessageEventArgs_Messages messageEventArgs)
        {
            Bookmark currentBookmark = logicManager.ConnectionManager.CurrentBookmark;

            StringBuilder errorDescription = new StringBuilder();
            StringBuilder solutionIdea = new StringBuilder();

            errorDescription.Append("Login failure since the login name was banned on the server " + currentBookmark.Server.ServerName + ".");
            solutionIdea.Append("Check your login name and try again or ask the server administrator.");

            LoginToServerFailedEvent(errorDescription.ToString(), solutionIdea.ToString(), currentBookmark);
        }
        #endregion
    }
}
