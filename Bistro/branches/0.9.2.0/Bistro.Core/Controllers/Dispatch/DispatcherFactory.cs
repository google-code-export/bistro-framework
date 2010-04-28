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
using Bistro.Configuration;

namespace Bistro.Controllers.Dispatch
{
    /// <summary>
    /// Default <see cref="IDispatcherFactory"/> implementation
    /// </summary>
    public class DispatcherFactory : IDispatcherFactory
    {
        /// <summary>
        /// Instance pointer
        /// </summary>
        private IControllerDispatcher instance;

        /// <summary>
        /// Application object pointer
        /// </summary>
        private Application application;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherFactory"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public DispatcherFactory(Application application, SectionHandler configuration)
        {
            this.application = application;
            instance = GetDispatcherImpl(application);
        }

        /// <summary>
        /// Creates the dispatcher.
        /// </summary>
        /// <returns></returns>
        public IControllerDispatcher GetDispatcherInstance()
        {
            return instance;
        }

        /// <summary>
        /// Gets a dispatcher implementation. Users wishing to just change the Dispatcher 
        /// and not the lifecycle thereof, should override this method.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns></returns>
        public virtual IControllerDispatcher GetDispatcherImpl(Application application)
        {
            return new ControllerDispatcher(application);
        }
    }
}
