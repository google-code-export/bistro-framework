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
using Bistro.Controllers;
using Bistro.Controllers.Dispatch;
using Bistro.Configuration;

namespace Bistro.Controllers
{
    /// <summary>
    /// Default implementation of a controller handler
    /// </summary>
    public class ControllerManagerFactory: IControllerManagerFactory
    {
        IControllerManager instance;
        Application application;

        public ControllerManagerFactory(Application application, SectionHandler configuration)
        {
            this.application = application;

            instance = GetInstanceImpl(application);
        }

        /// <summary>
        /// Retrieves an instance of the manager.
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public IControllerManager GetManagerInstance()
        {
            return instance;
        }

        /// <summary>
        /// Gets the instance impl.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns></returns>
        public virtual IControllerManager GetInstanceImpl(Application application)
        {
            var mgr = new ControllerManager(application);
            mgr.Load();

            return mgr;
        }
    }
}
