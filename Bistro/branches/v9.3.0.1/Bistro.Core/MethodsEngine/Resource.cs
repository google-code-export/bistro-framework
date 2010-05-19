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
using Bistro.MethodsEngine.Reflection;
using Bistro.Controllers.Descriptor;

namespace Bistro.MethodsEngine
{
    /// <summary>
    /// Class for resource dependenies analysis
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="name">The resource name.</param>
        public Resource(EngineControllerDispatcher engine, string _name)
        {
            this.name = _name;
            this.engine = engine;
        }

        /// <summary>
        /// Link to the engine
        /// </summary>
        private EngineControllerDispatcher engine;

        /// <summary>
        /// Resource name
        /// </summary>
        private string name;

        /// <summary>
        /// Resource providers bind points list.
        /// </summary>
        private List<IMethodsBindPointDesc> providers = new List<IMethodsBindPointDesc>();

        /// <summary>
        /// Resource dependents bind points list.
        /// </summary>
        private List<IMethodsBindPointDesc> dependents = new List<IMethodsBindPointDesc>();

        /// <summary>
        /// Resource required by bind points list.
        /// </summary>
        private List<IMethodsBindPointDesc> requiredBy = new List<IMethodsBindPointDesc>();

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        /// <value>The resource name.</value>
        public string Name { get { return name; } }

        /// <summary>
        /// Gets the "provider" bind points for this resource.
        /// </summary>
        /// <value>The "provider" bind points for this resource.</value>
        public IEnumerable<IMethodsBindPointDesc> Providers { get { return providers; } }

        /// <summary>
        /// Gets the "dependent" bind points for this resource.
        /// </summary>
        /// <value>The "dependent" bind points for this resource.</value>
        public IEnumerable<IMethodsBindPointDesc> Dependents { get { return dependents; } }

        /// <summary>
        /// Gets the "required by" bind points for this resource.
        /// </summary>
        /// <value>The "required by" bind points for this resource.</value>
        public IEnumerable<IMethodsBindPointDesc> RequiredBy { get { return requiredBy; } }

        /// <summary>
        /// Gets a value indicating whether this resource has no associated bind points.
        /// </summary>
        /// <value><c>true</c> if this resource has no associated bind points; otherwise, <c>false</c>.</value>
        public bool IsEmpty { get { return providers.Count + dependents.Count + requiredBy.Count == 0; } }

        /// <summary>
        /// Adds the provider bind point.
        /// </summary>
        /// <param name="bindPoint">The provider bind point.</param>
        internal void AddProvider(IMethodsBindPointDesc bindPoint)
        {
            providers.Add(bindPoint);
        }

        /// <summary>
        /// Adds the dependent bind point.
        /// </summary>
        /// <param name="bindPoint">The dependent bind point.</param>
        internal void AddDependents(IMethodsBindPointDesc bindPoint)
        {
            dependents.Add(bindPoint);
        }

        /// <summary>
        /// Adds the "required by" bind point. (this resource is required by that bind point)
        /// </summary>
        /// <param name="bindPoint">The "required by" bind point.</param>
        internal void AddRequiredBy(IMethodsBindPointDesc bindPoint)
        {
            requiredBy.Add(bindPoint);
        }

        /// <summary>
        /// Validates this resource.
        /// </summary>
        internal void Validate()
        {
            if (providers.Count == 0 && requiredBy.Count > 0)
                engine.RaiseMissingProvider(this.name, dependents.Concat(requiredBy).Select((bpd) => bpd.Controller));
            string resourceType = null;

            resourceType = ValidateResourceType(Providers, resourceType);
            resourceType = ValidateResourceType(Dependents, resourceType);
            resourceType = ValidateResourceType(RequiredBy, resourceType);
        }

        /// <summary>
        /// Validates the type of the resource for uniformity. 
        /// Calls engine.RaiseInconsistentResourceType in case if type is different between the bind points.
        /// </summary>
        /// <param name="bindPoints">The bind points list</param>
        /// <param name="type">The type name. (null if we don't know the type yet).</param>
        /// <returns>The type name.</returns>
        private string ValidateResourceType(IEnumerable<IMethodsBindPointDesc> bindPoints, string type)
        {
            foreach (IMethodsBindPointDesc bindPoint in bindPoints)
                if (type == null)
                    type = bindPoint.Controller.GetResourceType(name);
                else
                    if (type != bindPoint.Controller.GetResourceType(name))
                        engine.RaiseInconsistentResourceType(bindPoint.Target, this.name, providers.Concat(dependents).Concat(requiredBy).Select((bpd) => bpd.Controller));
            return type;
        }


        /// <summary>
        /// Unregisters the specified bind point.
        /// </summary>
        /// <param name="bindPoint">The bind point.</param>
        internal void Unregister(IMethodsBindPointDesc bindPoint)
        {
            providers.RemoveAll(provider => provider == bindPoint);
            dependents.RemoveAll(dependent => dependent == bindPoint);
            requiredBy.RemoveAll(reqBy => reqBy == bindPoint);
        }



    }
}
