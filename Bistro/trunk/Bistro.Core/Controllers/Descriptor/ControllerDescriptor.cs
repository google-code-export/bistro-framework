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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Bistro.Controllers.Descriptor.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Bistro.Configuration.Logging;
using Bistro.Controllers.OutputHandling;
using System.Configuration;
using Bistro.Interfaces;
using Bistro.MethodsEngine.Reflection;
using Bistro.Controllers.Security;
using Bistro.Controllers.Descriptor.Wrappers;

namespace Bistro.Controllers.Descriptor
{

    /// <summary>
    /// Manages information about a single controller. All bind matches for the same controller
    /// will be represented within a single descriptor class.
    /// </summary>
    public class ControllerDescriptor : IComparable, IControllerDescriptor, IMethodsControllerDesc
    {
        /// <summary>
        /// A single bind point. This struct maintains a many to one relationship with a single 
        /// controller class and describes the contents of all Bind attributes attached to it.
        /// </summary>
        public struct BindPointDescriptor : IBindPointDescriptor, IMethodsBindPointDesc
        {
            /// <summary>
            /// Gets or sets the target bind url.
            /// </summary>
            /// <value>The target.</value>
            public string Target { get; private set; }

            /// <summary>
            /// field to store link to the parent controller
            /// </summary>
            private ControllerDescriptor controller;

            /// <summary>
            /// field to store targetComponents
            /// </summary>
            private string[] targetComponents;


            /// <summary>
            /// Gets the length of the bind in facets.
            /// </summary>
            /// <value>The length of the bind in facets.</value>
            public int BindLength { get; private set; }

            /// <summary>
            /// Gets or sets the controller bind type.
            /// </summary>
            /// <value>The type of the controller bind.</value>
            public BindType ControllerBindType { get; private set; }

            /// <summary>
            /// Gets or sets the priority.
            /// </summary>
            /// <value>The priority.</value>
            public int Priority { get; private set; }

            /// <summary>
            /// Gets or sets the parameter fields.
            /// </summary>
            /// <value>The parameter fields.</value>
            public Dictionary<string, MemberInfo> ParameterFields { get; private set; }

            /// <summary>
            /// Link to the parent controller
            /// </summary>
            public IControllerDescriptor Controller 
            {
                get
                {
                    return controller;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BindPoint"/> struct.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <param name="bindType">Type of the bind.</param>
            /// <param name="priority">The priority.</param>
            public BindPointDescriptor(string target, BindType bindType, int priority, ControllerDescriptor _controller)
                : this()
            {
                targetComponents = BindPointUtilities.GetBindComponents(target);
                BindLength = targetComponents.Length;
                controller = _controller;
                Target = BindPointUtilities.GetBindPath(target);
                ControllerBindType = bindType;
                Priority = priority;

                parseTarget();
            }

            /// <summary>
            /// Parses the target bind url for parameter requests.
            /// </summary>
            private void parseTarget()
            {
                ParameterFields = new Dictionary<string, MemberInfo>();

                for (int i = 0; i < targetComponents.Length; i++)
                {
                    string token = targetComponents[i];
                    var type = Controller.ControllerType as Type;

                    if (token.StartsWith("{") && token.EndsWith("}"))
                    {
                        string tokenName = token.Substring(1, token.Length - 2);
                        if (type != null)
                        {
                            MemberInfo[] members =
                                type.GetMember(
                                    tokenName,
                                    MemberTypes.Property | MemberTypes.Field,
                                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                            if (members.Length >= 1)
                                ParameterFields.Add(tokenName, members[0]);
                            else
                            {
                                // F# members are mangled by appending an @ followed by an address
                                var mangledToken = tokenName + '@';
                                foreach (MemberInfo member in
                                    type.GetMembers(
                                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                                {
                                    if (member.Name.StartsWith(mangledToken))
                                    {
                                        ParameterFields.Add(tokenName, member);
                                        break;
                                    }
                                }

                            }
                        }
                        else
                            // if not found, still add it, maybe someone can make use of it later.
                            ParameterFields.Add(tokenName, null);
                    }
                }
            }

            #region IMethodsBindPointDesc Members

            /// <summary>
            /// Return controller description as an interface appropriate for methods engine
            /// </summary>
            IMethodsControllerDesc IMethodsBindPointDesc.Controller
            {
                get { return controller; }
            }

            #endregion
        }

        /// <summary>
        /// A Tuple of a MemberInfo and bool describing a CookieField attribute
        /// </summary>
        public struct CookieFieldDescriptor
        {
            /// <summary>
            /// Gets or sets the field.
            /// </summary>
            /// <value>The field.</value>
            public MemberInfo Field { get; private set; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="CookieFieldDescriptor"/> is outbound.
            /// </summary>
            /// <value><c>true</c> if outbound; otherwise, <c>false</c>.</value>
            public bool Outbound { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="CookieFieldDescriptor"/> struct.
            /// </summary>
            /// <param name="field">The field.</param>
            /// <param name="outbound">if set to <c>true</c> [outbound].</param>
            public CookieFieldDescriptor(MemberInfo field, bool outbound)
                : this()
            {
                Field = field;
                Outbound = outbound;
            }
        }

        private enum Messages
        {
            [DefaultMessage("{0} resource {1}.{2} doesn't have a scope specified. Defaulting to Request.")]
            UnspecifiedScope
        }

        private enum Warnings
        {
            [DefaultMessage("Resource {0} was not found in the controller descriptor {1}")]
            ResourceNotFound
        }

        private enum Exceptions
        {
            [DefaultMessage("{0}.{1} is a duplicate field or property. Check the base classes of the controller for members with the same name.")]
            DuplicateField
        }

        /// <summary>
        /// A list of bind points linked to this controller.
        /// </summary>
        public IEnumerable<IBindPointDescriptor> Targets 
        {
            get
            {
                return targets.Select(bpd => (IBindPointDescriptor)bpd);
            }
        }

        /// <summary>
        /// A list of context variables that affect the operation of this controller
        /// </summary>
        public List<string> DependsOn { get; protected set; }

        /// <summary>
        /// A list of context variables that this controller requires for operation
        /// </summary>
        public List<string> Requires { get; protected set; }

        /// <summary>
        /// A list of context variables that this controller places onto the context
        /// </summary>
        public List<string> Provides { get; protected set; }

        /// <summary>
        /// A list of fields marked as FormFields
        /// </summary>
        public Dictionary<string, MemberInfo> FormFields { get; protected set; }

        /// <summary>
        /// A list of fields marked as FormFields
        /// </summary>
        public Dictionary<string, MemberInfo> RequestFields { get; protected set; }

        /// <summary>
        /// A list of fields marked as FormFields
        /// </summary>
        public Dictionary<string, MemberInfo> SessionFields { get; protected set; }

        /// <summary>
        /// A list of fields marked as CookieFields, along with auxilliary information (Outbound flag)
        /// </summary>
        public Dictionary<string, CookieFieldDescriptor> CookieFields { get; protected set; }

        /// <summary>
        /// Dictionary to store memberWrappers
        /// </summary>
        private Dictionary<string, IMemberInfo> membersWrappers;


        /// <summary>
        /// The controller type
        /// </summary>
        public MemberInfo ControllerType { get; private set; }

        /// <summary>
        /// Gets the name of the controller type.
        /// </summary>
        /// <value>The name of the controller type.</value>
        public string ControllerTypeName { get { var tp = ControllerType as Type; if (tp == null) return ControllerType.Name; else return tp.FullName; } }

        /// <summary>
        /// Gets or sets the default template.
        /// </summary>
        /// <value>The default template.</value>
        public Dictionary<RenderType, string> DefaultTemplates { get; private set; }

        /// <summary>
        /// Our logger
        /// </summary>
        private ILogger logger;


        /// <summary>
        /// list to store BindPointDescriptors
        /// </summary>
        private List<BindPointDescriptor> targets;


        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDescriptor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        internal ControllerDescriptor(ILogger logger)
        {
            targets = new List<BindPointDescriptor>();
            DependsOn = new List<string>();
            Provides = new List<string>();
            Requires = new List<string>();
            FormFields = new Dictionary<string, MemberInfo>();
            RequestFields = new Dictionary<string, MemberInfo>();
            SessionFields = new Dictionary<string, MemberInfo>();
            CookieFields = new Dictionary<string, CookieFieldDescriptor>();
            DefaultTemplates = new Dictionary<RenderType, string>();

            membersWrappers = new Dictionary<string, IMemberInfo>();

            this.logger = logger;
        }


        /// <summary>
        /// Builds the <see cref="BindPointDescriptor"/> based on the supplied bind attribute
        /// </summary>
        /// <param name="attribute">The Bind attribute.</param>
        protected virtual void ProcessNonEmptyBind(BindAttribute attribute)
        {
            if (BindPointUtilities.IsVerbQualified(attribute.Target))
                targets.Add(new BindPointDescriptor(
                                BindPointUtilities.VerbNormalize(attribute.Target),
                                attribute.ControllerBindType,
                                attribute.Priority,
                                this));
            else
                // if not verb qualified, make it work for all verbs
                foreach (string verb in BindPointUtilities.HttpVerbs)
                    targets.Add(new BindPointDescriptor(
                                    BindPointUtilities.Combine(verb, attribute.Target),
                                    attribute.ControllerBindType,
                                    attribute.Priority,
                                    this));
        }

        /// <summary>
        /// Infers the <see cref="BindPointDescriptor"/> based on controller type name, and all Http verbs.
        /// </summary>
        protected virtual void ProcessEmptyBind()
        {
            foreach (string verb in BindPointUtilities.HttpVerbs)
                targets.Add(new BindPointDescriptor(
                                BindPointUtilities.Combine(verb, ControllerTypeName.Replace('.', '/')),
                                BindType.Before,
                                -1,
                                this));
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDescriptor"/> class.
        /// </summary>
        /// <param name="t">The t.</param>
        protected ControllerDescriptor(MemberInfo t, ILogger logger)
            : this(logger)
        {
            ControllerType = t;

            // load the renderwith attribute
            IterateAttributes<RenderWithAttribute>(t, false,
                (attrib) =>
                {
                    if (DefaultTemplates.ContainsKey(attrib.RenderType))
                        throw new ConfigurationErrorsException(
                            String.Format("Multilpe RenderWith attributes specified for {0} on controller {1}",
                                            attrib.RenderType,
                                            GetType().FullName));

                    DefaultTemplates.Add(attrib.RenderType, attrib.Template);
                }, null);
        }

        /// <summary>
        /// Iterates over a list of attributes taken from the targetType that are
        /// of type attributeType, with the supplied "inherit" flag.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to iterate over. This type parameter will be passed a the parameter to the nonEmpty delegate</typeparam>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <param name="nonEmpty">The statement to invoke on the elements of a non-empty result set.</param>
        /// <param name="empty">The statement to invoke on an empty result set.</param>
        static private void IterateAttributes<T>(MemberInfo targetType, bool inherit, Action<T> nonEmpty, Action empty)
        {
            T[] attribs = targetType.GetCustomAttributes(typeof(T), inherit) as T[];

            if (attribs.Length > 0)
                foreach (T attribute in attribs)
                    nonEmpty(attribute);
            else if (empty != null)
                empty();
        }

        /// <summary>
        /// Determines whether the specified target type is marked by the attribute.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="markerAttribute">The marker attribute.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>
        /// 	<c>true</c> if the specified target type is marked; otherwise, <c>false</c>.
        /// </returns>
        static private bool IsMarked(MemberInfo targetType, Type markerAttribute, bool inherit)
        {
            return targetType.GetCustomAttributes(markerAttribute, inherit).Length > 0;
        }

        /// <summary>
        /// Iterates over a list of members taken off of the target parameter based on the binding flags supplied
        /// </summary>
        /// <param name="target">the target type</param>
        /// <param name="flags">the binding flags to filter the members by</param>
        /// <param name="nonEmpty">the statement to apply to each found member</param>
        static private void IterateMembers(Type target, BindingFlags flags, Action<MemberInfo> nonEmpty)
        {
            foreach (MemberInfo info in target.GetMembers(flags))
                nonEmpty(info);
        }

        /// <summary>
        /// Populates the descriptor. This method will load all available data off of the 
        /// metadata placed onto the given controller type.
        /// </summary>
        /// <param name="t">The t.</param>
        public void PopulateDescriptor()
        {
            var type = ControllerType as Type;
            if (type == null)
                throw new ApplicationException("This method should not be called for non-class controllers.");
            // load all of the Bind attributes
            IterateAttributes<BindAttribute>(type, false, ProcessNonEmptyBind, ProcessEmptyBind);
            IterateMembers(type, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                (member) =>
                {
                    try
                    {
                        MemberWrapper mw = new MemberWrapper(member);
                        membersWrappers.Add(mw.Name, mw);
                        // all fields that are not marked as required or depends-on are defaulted to "provided"
                        if ((!IsMarked(member, typeof(RequiresAttribute), true) &&
                            !IsMarked(member, typeof(DependsOnAttribute), true)) &&
                            (IsMarked(member, typeof(SessionAttribute), true) ||
                            IsMarked(member, typeof(RequestAttribute), true)))
                            Provides.Add(member.Name);

                        IterateAttributes<DependsOnAttribute>(member, true,
                            (attribute) => { DependsOn.Add(attribute.Name ?? member.Name); }, null);

                        IterateAttributes<RequiresAttribute>(member, true,
                            (attribute) => { Requires.Add(attribute.Name ?? member.Name); }, null);

                        IterateAttributes<ProvidesAttribute>(member, true,
                            (attribute) => { var name = attribute.Name ?? member.Name; if (!Provides.Contains(name)) Provides.Add(name); }, null);

                        IterateAttributes<CookieFieldAttribute>(member, true,
                            (attribute) => { CookieFields.Add(attribute.Name ?? member.Name, new CookieFieldDescriptor(member, attribute.Outbound)); }, null);

                        IterateAttributes<FormFieldAttribute>(member, true,
                            (attribute) => { FormFields.Add(attribute.Name ?? member.Name, member); }, null);

                        IterateAttributes<RequestAttribute>(member, true,
                            (attribute) => { RequestFields.Add(attribute.Name ?? member.Name, member); }, null);

                        IterateAttributes<SessionAttribute>(member, true,
                            (attribute) => { SessionFields.Add(attribute.Name ?? member.Name, member); }, null);
                    }
                    catch (ArgumentException ex)
                    {
                        logger.Report(Exceptions.DuplicateField, type.Name, member.Name);

                        throw ex;
                    }
                });

            SetDefaultResourceScope(Provides, "Provided");
            SetDefaultResourceScope(DependsOn, "Optional");
            SetDefaultResourceScope(Requires, "Required");
        }

        /// <summary>
        /// Sets the default resource scope for resources without one
        /// </summary>
        private void SetDefaultResourceScope(List<string> resourceList, string resourceType)
        {
            foreach (string resource in resourceList)
                if (!RequestFields.ContainsKey(resource) &&
                    !SessionFields.ContainsKey(resource) &&
                    !FormFields.ContainsKey(resource) &&
                    !CookieFields.ContainsKey(resource))
                {
                    MemberInfo[] member = ((Type)ControllerType).GetMember(resource, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    // it's an aliased field, nothing to do here.
                    if (member == null || member.Length != 1)
                        continue;

                    logger.Report(Messages.UnspecifiedScope, resourceType, ControllerTypeName, resource);
                    RequestFields.Add(resource, member[0]);
                }
        }

        /// <summary>
        /// Creates the descriptor.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static IControllerDescriptor CreateDescriptor(MemberInfo t, ILogger logger)
        {
            IControllerDescriptor ret = new ControllerDescriptor(t, logger);

            ret.PopulateDescriptor();

            return ret;
        }

        /// <summary>
        /// Creates the descriptor.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="binds">The binds.</param>
        /// <param name="dependsOn">The depends on.</param>
        /// <param name="requires">The requires.</param>
        /// <param name="provides">The provides.</param>
        /// <param name="cookieFields">The cookie fields.</param>
        /// <param name="formFields">The form fields.</param>
        /// <param name="requestFields">The request fields.</param>
        /// <param name="sessionFields">The session fields.</param>
        /// <returns></returns>
        public static IControllerDescriptor CreateDescriptorRaw(MemberInfo t, IEnumerable<string> dependsOn, IEnumerable<string> requires,
            IEnumerable<string> provides, IDictionary<string, CookieFieldDescriptor> cookieFields,
            IDictionary<string, MemberInfo> formFields, IDictionary<string, MemberInfo>
            requestFields, IDictionary<string, MemberInfo> sessionFields, IEnumerable<BindAttribute> binds, ILogger logger)
        {
            ControllerDescriptor ret = new ControllerDescriptor(t, logger);

            Action<IList<string>, IEnumerable<string>> copyList = (target, source) =>
            { if (source == null) return; foreach (string i in source) target.Add(i); };

            Action<IDictionary<string, MemberInfo>, IDictionary<string, MemberInfo>> copyDict = (source, target) =>
            { if (source == null) return; foreach (string key in source.Keys) target.Add(key, source[key]); };

            Action<IDictionary<string, CookieFieldDescriptor>, IDictionary<string, CookieFieldDescriptor>> copyCookieDict = (source, target) =>
            { if (source == null) return; foreach (string key in source.Keys) target.Add(key, source[key]); };

            bool empty = true;
            foreach (BindAttribute attrib in binds)
            {
                ret.ProcessNonEmptyBind(attrib);
                empty = false;
            }
            if (empty)
                ret.ProcessEmptyBind();


            copyList(ret.DependsOn, dependsOn);
            copyList(ret.Requires, requires);
            copyList(ret.Provides, provides);

            copyDict(ret.FormFields, formFields);
            copyDict(ret.RequestFields, requestFields);
            copyDict(ret.SessionFields, sessionFields);

            copyCookieDict(ret.CookieFields, cookieFields);

            return ret;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            var o = obj as IControllerDescriptor;

            return o.ControllerTypeName.CompareTo(ControllerTypeName);
        }

        #region IMethodsControllerDesc Members

        /// <summary>
        /// Gets a value indicating - whether controller is a security controller - useful for methods engine
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is a security controller; otherwise, <c>false</c>.
        /// </value>
        public bool IsSecurity
        {
            get
            {
                return (typeof(ISecurityController).IsAssignableFrom(ControllerType as Type));
            }
        }

        /// <summary>
        /// Returns name of the resource type for resource.
        /// </summary>
        /// <param name="resourceName">resource name</param>
        /// <returns>Resource type name</returns>
        public string GetResourceType(string resourceName)
        {
            if (membersWrappers.ContainsKey(resourceName))
            {
                return membersWrappers[resourceName].Type;
            }
              
            // Next string works very slow.
            //logger.Report(Warnings.ResourceNotFound, resourceName, ControllerTypeName);
            return "dummyType";
        }

        #endregion

        #region IMethodsControllerDesc Members

        /// <summary>
        /// A list of bind points linked to this controller for methods engine.
        /// </summary>
        /// <value>The list of bind points linked to this controller.</value>
        IEnumerable<IMethodsBindPointDesc> IMethodsControllerDesc.Targets
        {
            get 
            {
                return targets.Select(bpd => (IMethodsBindPointDesc)bpd);
            }
        }

        #endregion
    }
}
