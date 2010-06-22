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
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Bistro.Configuration.Logging
{
    /// <summary>
    /// Default logger implementation. This class will log to debug-out
    /// </summary>
    public class DefaultLogger : ILogger
    {
        /// <summary>
        /// Reports the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="args">The args.</param>
        public void Report(Enum code, params string[] args)
        {
            Report(code, null, args);
        }

        /// <summary>
        /// Reports the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="e">The e.</param>
        /// <param name="args">The args.</param>
        public void Report(Enum code, Exception e, params string[] args)
        {
            DoReport(GetMessageFormat(code), args);
            ReportException("Exception", e);
        }

        /// <summary>
        /// Retrieves the message format from the DefaultMessage attribute decorating the
        /// message code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected string GetMessageFormat(Enum code)
        {
            System.Reflection.FieldInfo fi = code.GetType().GetField(code.ToString());
            var attributes = (DefaultMessageAttribute[])fi.GetCustomAttributes(typeof(DefaultMessageAttribute), false);
            return (attributes.Length > 0)
                ? attributes[0].Message
                : code.GetType().DeclaringType.FullName + "." + code.ToString();
        }

        /// <summary>
        /// Retrieves the message format from the DefaultMessage attribute decorating the
        /// message code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected Severity GetMessageSeverity(Enum code)
        {
            System.Reflection.FieldInfo fi = code.GetType().GetField(code.ToString());
            var attributes = (SeverityLevelAttribute[])fi.GetCustomAttributes(typeof(SeverityLevelAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Severity : Severity.Information;
        }

        /// <summary>
        /// Reports the exception.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="e">The e.</param>
        private void ReportException(string header, Exception e)
        {
            if (e == null)
                return;
            ReportException("Inner Exception: ", e.InnerException);
            DoReport("\t\t" + header + ": " + e.Message);
            DoReport("\t\tStack Trace : \n\n");
            DoReport(e.StackTrace);
        }

        /// <summary>
        /// Actually writes out to debug out. Classes wishing to simply intercept and redirect 
        /// the message should override this method.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments for formatting the message.</param>
        protected virtual void DoReport(string message, params string[] args) {
            Debug.WriteLine(String.Format(message, args));
        }
    }
}
