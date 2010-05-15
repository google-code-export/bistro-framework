/****************************************************************************
 * 
 *  Bistro Framework Copyright © 2003-2009 Hill30 Inc
 *
 *  This file is part of Bistro Framework.
 *
 *  Bistro Framework is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Bistro Framework is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Bistro Framework.  If not, see <http://www.gnu.org/licenses/>.
 *  
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Bistro.Configuration.Logging
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class SeverityLevelAttribute : Attribute
    {
        public SeverityLevelAttribute(Severity severity)
        {
            this.severity = severity;
        }

        Severity severity;
        public Severity Severity { get { return severity; } }
    }

    public enum Severity
    {
        Critical = 4,
        Error = 3,
        Warning = 2,
        Information = 1,
        Message = 0
    }

    [Flags]
    public enum SeverityMask
    {
        None = 0x00,  // Special value

        #region Severity Levels
        Critical = 0x10,
        Error = 0x08,
        Warning = 0x04,
        Information = 0x02,
        Message = 0x01,
        #endregion

        #region Severity Masks
        All = Critical | Error | Warning | Information | Message,
        Errors = Critical | Error,
        Warnings = Warning,
        Messages = Information | Message
        #endregion
    }
}