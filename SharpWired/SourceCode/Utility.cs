/**
 * Utility.cs 
 * Created by Ola Lindberg, 2006-06-20
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

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpWired
{
    /// <summary>
    /// Various utilities that are used througout the SharpWired source code.
    /// 
    /// NOTE: This class has derived from the Socio Project. See http://socio.sf.net/
    /// </summary>
    public static class Utility
    {
        /// <summary> 
        /// Get ASCII EOT  
        ///</summary>
        public static string EOT
        {
            get
            {
                return Encoding.ASCII.GetString(new byte[] { 0x04 });
            }
        }
        /// <summary> 
        /// Get ASCII FS  
        ///</summary>
        public static string FS
        {
            get
            {
                return Encoding.ASCII.GetString(new byte[] { 0x1C });
            }
        }
        /// <summary> 
        /// Get ASCII GS  
        ///</summary>
        public static string GS
        {
            get
            {
                return Encoding.ASCII.GetString(new byte[] { 0x1D });
            }
        }
        /// <summary> 
        /// Get ASCII RS  
        ///</summary>
        public static string RS
        {
            get
            {
                return Encoding.ASCII.GetString(new byte[] { 0x1E });
            }
        }
        /// <summary> 
        /// Get ASCII SP   
        ///</summary>
        public static string SP
        {
            get
            {
                return Encoding.ASCII.GetString(new byte[] { 0x20 });
            }
        }

        /// <summary> 
        /// Hash the password with the SHA1 algorithm  
        /// </summary>
        /// <params name="password"> The password in plain text to be hashed</params>
        /// <returns> A lowercase string of hexadecimal characters,
        /// representing a SHA1 hashed password�</returns>
        public static string HashPassword(string password)
        {
            // If the password is more than 0, it should be hashed with SHA1
            if (password.Length > 0)
            {
                password = BitConverter.ToString(
                            new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(
                                        Encoding.UTF8.GetBytes(password)));
                // Unfortunately, we get a string like 00-01-AA-0B- etc, 
                // so we convert it to lowercase and remove the "-"s.
                string[] split = password.ToLower().Split((char)'-');
                password = "";
                foreach (string p in split)
                    password += p;
            }
            return password;
        }

        /// <summary>
        /// Splits the a Wired string by the Utility.FS delimiter.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string[] SplitWiredString(string message)
        {
            // Parse the server information event
            char[] delimiterChars = { Convert.ToChar(Utility.FS) };
            return message.Split(delimiterChars);
        }

        /// <summary>
        /// Converts an integer to a boolean. 
        /// </summary>
        /// <param name="i"></param>
        /// <returns>False if the given integer is 0, true otherwise</returns>
        public static bool ConvertIntToBool(int i)
        {
            if (i == 0)
                return false;
            return true;
        }
    }
}