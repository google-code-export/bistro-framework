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
using System.Collections;
using BindPointDescriptor = Bistro.Controllers.Descriptor.ControllerDescriptor.BindPointDescriptor;
using Bistro.Controllers.Descriptor;
using System.Diagnostics;
using Bistro.Controllers.Security;
using Bistro.Controllers.Dispatch;
using System.Reflection;
using Bistro.Configuration.Logging;
using System.Text.RegularExpressions;

namespace Bistro.Controllers.Dispatch
{
    /// <summary>
    /// Manages controller application to urls
    /// </summary>
    public class ControllerDispatcher : IControllerDispatcher
    {
        /// <summary>
        /// A wildcard denoting a single url component
        /// </summary>
        private const string localWildCard = "*";

        /// <summary>
        /// A wild card enoting multiple (1 or more) url components
        /// </summary>
        private const string globalWildCard = "?";

        /// <summary>
        /// Regular expression for capturing leading ampersands in a query string
        /// </summary>
        private Regex leadingAmpRE = new Regex("^&+", RegexOptions.Compiled);
        /// <summary>
        /// Regular expression for capturing leading ampersands in a query string
        /// </summary>
        private Regex trailingAmpRE = new Regex("&+$", RegexOptions.Compiled);

        /// <summary>
        /// Regular expression for capturing consequtive ampersands in a query string
        /// </summary>
        private Regex dupedAmpRE = new Regex("(?<=&)&", RegexOptions.Compiled);

        enum Messages
        {
            [DefaultMessage("A bind point has not been configured for {0}")]
            NoConfiguredBindPoint,
            [DefaultMessage("A bind point could not be found for {0}")]
            NoMatchingBindPoint,
            [DefaultMessage("Execution path for {0} is \r\n{1}")]
            ExecutionPath,
            [DefaultMessage("Found execution path in {0} ms over a set of {1} bind points")]
            PathCalculation
        }

        /// <summary>
        /// Mapping of urls to bind points
        /// </summary>
        Dictionary<string, List<BindPointDescriptor>> map = new Dictionary<string, List<BindPointDescriptor>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDispatcher"/> class.
        /// </summary>
        public ControllerDispatcher(Application application) { logger = application.LoggerFactory.GetLogger(GetType()); }

        /// <summary>
        /// Our logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Registers the controller with the dispatcher.
        /// </summary>
        /// <param name="info">The controller info.</param>
        public virtual void RegisterController(ControllerDescriptor info)
        {
            foreach (BindPointDescriptor bindPoint in info.Targets)
            {
                List<BindPointDescriptor> descriptors = null;

                if (!map.TryGetValue(bindPoint.Target, out descriptors))
                {
                    descriptors = new List<BindPointDescriptor>();
                    map.Add(bindPoint.Target, descriptors);
                }

                int i = 0;
                foreach (BindPointDescriptor comparedBindPoint in descriptors)
                {
                    if (comparedBindPoint.Priority > bindPoint.Priority)
                        break;

                    i++;
                }

                descriptors.Insert(i, bindPoint);
            }
        }

        /// <summary>
        /// Normalizes the url and splits it by slashes, not presenting a blank element if the 
        /// url begins with a slash
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private string[] smartUrlSplit(string url)
        {
            // trim any excess whitespace, and also the leading /
            string workingCopy = url.Trim().TrimStart('/');

            return BindPointUtilities.GetBindComponents(url);
        }

        /// <summary>
        /// Gets an ordered list of controller types that should service the given url. The ordering
        /// is defined by the priority value marked on the class
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="bindPoint">The bind point.</param>
        /// <returns></returns>
        public virtual ControllerInvocationInfo[] GetControllers(string requestUrl)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var controllers = GetFullControllerList(requestUrl);
            logger.Report(Messages.PathCalculation, sw.ElapsedMilliseconds.ToString(), map.Count.ToString());

            var securityControllers = new List<ControllerInvocationInfo>();

            int i = 0;
            while (i < controllers.Count)
                if (typeof(ISecurityController).IsAssignableFrom(controllers[i].BindPoint.Controller.ControllerType as Type))
                {
                    securityControllers.Add(controllers[i]);
                    controllers.RemoveAt(i);
                }
                else
                    i++;

            // we can't just sort, because the standard sort may re-arrange the existing order.
            // we just want to move all security controllers to the top of the chain
            controllers.InsertRange(0, securityControllers);

            return controllers.ToArray();
        }

        /// <summary>
        /// Gets the full controller list.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns></returns>
        private List<ControllerInvocationInfo> GetFullControllerList(string requestUrl)
        {
            Func<List<ControllerInvocationInfo>> init = () => new List<ControllerInvocationInfo>();
            var before = init();
            var payload = init();
            var after = init();
            var teardown = init();

            // make sure the match is only done based on url component, and not the parameters
            // if there are any parameters, they'll be handled later
            string[] splitQueryString = requestUrl.Split('?');
            string[] requestComponents = smartUrlSplit(splitQueryString[0]);

            foreach (string bindPoint in map.Keys)
            {
                int matchDepth;
                Dictionary<string, string> parameterValues = new Dictionary<string, string>();

                if (!Match(requestComponents, smartUrlSplit(bindPoint), out matchDepth, parameterValues))
                    continue;

                foreach (BindPointDescriptor descriptor in map[bindPoint])
                {
                    List<ControllerInvocationInfo> list = before;
                    switch (descriptor.ControllerBindType)
                    {
                        case BindType.Before:
                            list = before;
                            break;
                        case BindType.Payload:
                            list = payload;
                            break;
                        case BindType.After:
                            list = after;
                            break;
                        case BindType.Teardown:
                            list = teardown;
                            break;
                    }

                    // if there are query string parameters, populate them by name, and not positionally
                    if (splitQueryString.Length == 2)
                    {
                        // if there are any errant leading or trailing ampersands, get rid of them
                        string[] queryStringParameters = CleanQueryString(splitQueryString[1]).Split('&', '=');
                        for (int i = 0; i + 1 < queryStringParameters.Length; i += 2)
                            if (descriptor.ParameterFields.ContainsKey(queryStringParameters[i]) && !parameterValues.ContainsKey(queryStringParameters[i]))
                                parameterValues.Add(queryStringParameters[i], queryStringParameters[i + 1]);
                    }

                    list.Add(new ControllerInvocationInfo(descriptor, parameterValues, matchDepth));
                }
            }

            Func<int, int, int> nonZero = (a, b) => a == 0 ? b : a;
            Comparison<ControllerInvocationInfo> compare =
                (x, y) => nonZero(x.MatchDepth.CompareTo(y.MatchDepth), y.BindPoint.Priority.CompareTo(x.BindPoint.Priority));

            before.Sort(compare);
            payload.Sort(compare);
            after.Sort(compare);
            teardown.Sort(compare);

            after.InsertRange(0, payload);
            after.InsertRange(0, before);
            after.AddRange(teardown);

            new DependencyHelper().EnforceDependencies(after);

            StringBuilder path = new StringBuilder();
            foreach (ControllerInvocationInfo info in after)
                path.Append(info.BindPoint.Controller.ControllerTypeName).Append(" based on ").Append(info.BindPoint.Target).Append("\r\n");

            logger.Report(Messages.ExecutionPath, requestUrl, path.ToString());

            return after;
        }

        /// <summary>
        /// Cleans query strings of leading and duplicate ampersands
        /// </summary>
        /// <param name="queryString">the query string</param>
        /// <returns></returns>
        protected virtual string CleanQueryString(string queryString)
        {
            return
                dupedAmpRE.Replace(
                    trailingAmpRE.Replace(
                        leadingAmpRE.Replace(queryString, String.Empty),
                        String.Empty),
                    String.Empty);
        }

        /// <summary>
        /// Matches the specified request URL.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="bindPoint">The bind point.</param>
        /// <param name="matchDepth">The match depth.</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        private bool Match(string[] requestUrl, string[] bindPoint, out int matchDepth, Dictionary<string, string> parameterValues)
        {
            int requestIndex = 0;
            int bindIndex = 0;
            matchDepth = 0;

            while (bindIndex < bindPoint.Length)
            {
                // if there are more bind components than there are url components, we don't have a match.
                // however, if all the remaining bind components are parameter components, it is a match
                // and we simply set all of the values to null.
                if (requestIndex >= requestUrl.Length)
                {
                    List<string> possibleNulls = new List<string>(bindPoint.Length - bindIndex);
                    for (int i = bindIndex; i < bindPoint.Length; i++)
                    {
                        if (!(IsParameterComponent(bindPoint[i])))
                            return false;

                        possibleNulls.Add(bindPoint[i].TrimStart('{').TrimEnd('}'));
                    }

                    foreach (string nullParameter in possibleNulls)
                        parameterValues.Add(nullParameter, null);

                    return true;
                }

                string bindComponent = bindPoint[bindIndex];
                matchDepth = requestIndex;

                // local wildcard means that the current component doesn't matter. accept and move on.
                if (bindComponent == localWildCard)
                {
                    requestIndex++;
                    bindIndex++;

                    continue;
                }

                // same as local wild card, but we need to capture the value as a parameter
                if (bindComponent.StartsWith("{") && bindComponent.EndsWith("}"))
                {
                    parameterValues.Add(bindComponent.Trim('{', '}'), requestUrl[requestIndex]);
                    requestIndex++;
                    bindIndex++;

                    continue;
                }

                // global wild card means that the requestUrl from this point forward can have any value (including {})
                // we need to stop once we match the next component
                if (bindComponent.Equals(globalWildCard))
                {
                    // the global wild card is the end of the bind string. we have a match.
                    if (bindIndex + 1 == bindPoint.Length)
                        return true;

                    bindIndex++;
                    bindComponent = bindPoint[bindIndex];

                    // scan through the request structure to find the next matching component
                    // e.g. - bindPoint "/hello/?/you" should match request /hello/how/are/you
                    // skipping the "/how/are" piece, and resume the match on "/you". However,
                    // the same bind point should not match request "/hello/world", as there is 
                    // no trailing "you". note that this disallows the syntax "/hello/?/*" and 
                    // "/hello/?/?", but does allow "/hello/?/something/*", and so on.
                    while (requestIndex < requestUrl.Length && requestUrl[requestIndex] != bindComponent)
                        requestIndex++;

                    // ran out of url components while looking for trailing match. no match.
                    if (requestIndex == requestUrl.Length)
                        return false;
                }

                if (!bindComponent.Equals(requestUrl[requestIndex], StringComparison.OrdinalIgnoreCase))
                    return false;

                requestIndex++;
                bindIndex++;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the component denotes a parameter component.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <returns>
        /// 	<c>true</c> if the component denotes a parameter component; otherwise, <c>false</c>.
        /// </returns>
        private bool IsParameterComponent(string bindComponent)
        {
            return bindComponent.StartsWith("{") && bindComponent.EndsWith("}");
        }

        /// <summary>
        /// Determines whether the specified url has a controller explicitly bound to it
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns>
        /// 	<c>true</c> if an exact binding exists; otherwise, <c>false</c>.
        /// </returns>
        public bool HasExactBind(string requestUrl)
        {
            return map.ContainsKey(requestUrl);
        }
    }
}