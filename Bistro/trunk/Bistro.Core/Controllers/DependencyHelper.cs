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
using Bistro.Controllers.Descriptor;

namespace Bistro.Controllers
{
    /// <summary>
    /// Assists in ordering and enforcing dependencies. This class is reusable, but not thread-safe
    /// </summary>
    class DependencyHelper
    {
        /// <summary>
        /// A mapping of context values to lists of controllers that require them
        /// </summary>
        Dictionary<string, List<ControllerInvocationInfo>> requirements = new Dictionary<string, List<ControllerInvocationInfo>>();

        /// <summary>
        /// A mapping of context values to lists of controllers that depend on them
        /// </summary>
        Dictionary<string, List<ControllerInvocationInfo>> dependencies = new Dictionary<string, List<ControllerInvocationInfo>>();

        /// <summary>
        /// A mapping of context values to lists of controllers that provide on them
        /// </summary>
        Dictionary<string, List<ControllerInvocationInfo>> providers = new Dictionary<string, List<ControllerInvocationInfo>>();

        /// <summary>
        /// used to track the structure we're building out, so that if we move something, we move everything it needs
        /// </summary>
        Dictionary<ControllerInvocationInfo, List<ControllerInvocationInfo>> finalDependencies = new Dictionary<ControllerInvocationInfo, List<ControllerInvocationInfo>>();

        /// <summary>
        /// retrieves the list under the requested key. if the key isn't present, the list is
        /// created and added
        /// </summary>
        /// <param name="key"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        private List<ControllerInvocationInfo> getAndCreateList(string key, Dictionary<string, List<ControllerInvocationInfo>> map)
        {
            List<ControllerInvocationInfo> res = null;
            if (map.TryGetValue(key, out res))
                return res;

            res = new List<ControllerInvocationInfo>();
            map.Add(key, res);
            return res;
        }

        /// <summary>
        /// Gets the and create list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="map">The map.</param>
        /// <returns></returns>
        private List<ControllerInvocationInfo> getAndCreateList(ControllerInvocationInfo key, Dictionary<ControllerInvocationInfo, List<ControllerInvocationInfo>> map)
        {
            List<ControllerInvocationInfo> res = null;
            if (map.TryGetValue(key, out res))
                return res;

            res = new List<ControllerInvocationInfo>();
            map.Add(key, res);
            return res;
        }

        /// <summary>
        /// reorders the list of controllers so that parameter dependencies are met.
        /// this implementation is simplistic, and does not look for cyclical dependencies.
        /// </summary>
        /// <param name="after"></param>
        internal void EnforceDependencies(List<ControllerInvocationInfo> after)
        {
            populateMaps(after);

            process(requirements, true);
            process(dependencies, false);
            sort(after, 0);
        }

        /// <summary>
        /// Sorts the supplied list based on finalDependencies. The method checks each element
        /// of the list against the keys of finalDependencies. If a match occurs, it then
        /// makes sure that the controller is earlier in the list that the contents of the key
        /// </summary>
        /// <param name="after"></param>
        private void sort(List<ControllerInvocationInfo> after, int iterationCount)
        {
            if (iterationCount > after.Count)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ControllerInvocationInfo info in finalDependencies.Keys)
                {
                    sb.Append("\r\n").Append(info.BindPoint.Controller.ControllerType.Name).Append(" is a required/requested resource for");
                    foreach (ControllerInvocationInfo dep in finalDependencies[info])
                        sb.Append("\r\n\t").Append(dep.BindPoint.Controller.ControllerType.Name);
                }

                sb.Insert(0, "Possible cyclical dependency detected:\r\n");
                
                throw new ApplicationException(sb.ToString());
            }

            int i = -1;
            bool resort = false;
            while (++i < after.Count)
            {
                List<ControllerInvocationInfo> dependents;
                if (!finalDependencies.TryGetValue(after[i], out dependents))
                    continue;

                foreach (ControllerInvocationInfo dep in dependents)
                {
                    int index = after.IndexOf(dep);

                    if (index < i)
                    {
                        // it's possible to be your own dependent. you may modify
                        // an inbound value, so you're both a dependent and a provider
                        if (dep == after[i])
                            continue;

                        after.RemoveAt(index);
                        after.Insert(i, dep);

                        //the insert effectively moves this item down in the list
                        //TODO: AP this needs to be reviewed. original unit test didn't catch this
                        // with the addition of a standard "payload" controller, i started seeing 
                        // the payload controller bumped up higher than it should be. seems to be
                        // related to this, though i'm not quite sure yet.
                        /*if (i != (after.Count - 1))
                        {
                            i++;
                        }*/
                        resort = true;
                    }
                }
            }

            // to make sure that indirect dependencies are addressed
            // we need to have at least one clean sort pass 
            if (resort)
                sort(after, iterationCount + 1);
        }

        /// <summary>
        /// Ensures that all controllers given are satisfied by the contents of providers. As one or more 
        /// providers of the same value are given, they are loaded into the finalDependencies list. 
        /// </summary>
        /// <param name="requirements"></param>
        /// <param name="failOnNotMet"></param>
        private void process(Dictionary<string, List<ControllerInvocationInfo>> map, bool failOnNotMet)
        {
            foreach (string key in map.Keys)
            {
                List<ControllerInvocationInfo> prov;
                if (!providers.TryGetValue(key, out prov))
                {
                    if (failOnNotMet)
                        throw new InvalidOperationException("Requirement " + key + " was not met by any provider");
                }
                else
                    // there can be multiple providers of the same parameter. all of them have to be 
                    // loaded into the list.
                    foreach (ControllerInvocationInfo info in prov)
                        // for each provider, we need to load in its list of dependent controllers
                        foreach (ControllerInvocationInfo dep in map[key])
                            getAndCreateList(info, finalDependencies).Add(dep);
            }
        }

        /// <summary>
        /// Loads the required/dependent/provider maps based on the list of controllers
        /// </summary>
        /// <param name="sortedList">The sorted list of controllers.</param>
        private void populateMaps(List<ControllerInvocationInfo> sortedList)
        {
            requirements.Clear();
            dependencies.Clear();
            providers.Clear();
            finalDependencies.Clear();

            foreach (ControllerInvocationInfo controller in sortedList)
            {
                ControllerDescriptor info = controller.BindPoint.Controller;
                foreach (string req in info.Requires)
                    getAndCreateList(req, requirements).Add(controller);
                foreach (string dep in info.DependsOn)
                    getAndCreateList(dep, dependencies).Add(controller);
                foreach (string prov in info.Provides)
                    getAndCreateList(prov, providers).Add(controller);
            }
        }
    }
}
