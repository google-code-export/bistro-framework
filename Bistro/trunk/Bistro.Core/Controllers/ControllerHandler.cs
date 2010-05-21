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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Web;
using System.Collections;
using Bistro.Controllers.Descriptor;
using Bistro.Configuration.Logging;
using Bistro.Controllers.OutputHandling;
using Bistro.Controllers.Descriptor.Data;
using Bistro.Entity;
using Bistro.Interfaces;
using System.Globalization;

namespace Bistro.Controllers
{
    /// <summary>
    /// Manages the state of a controller pre and post processing.
    /// </summary>
	public class ControllerHandler : Bistro.Controllers.IControllerHandler
	{
		enum Messages 
		{
			[DefaultMessage("For FormField file variables, please use Types of HttpPostedFile, string, byte[], or Stream.")]
			InvalidFormFieldFileType,
            [DefaultMessage("Unable to set {0}.{1} to \"{2}\" because \"{3}\"")]
            UnableToSetValue,
            [DefaultMessage("Unable to parse the specific date: {0}")]
            UnableToParseDate
		}

		/// <summary>
		/// The controller managed by this handler. The specific bind-point is determined at the point of invocation.
		/// </summary>
		IControllerDescriptor descriptor;

		/// <summary>
		/// A list of all fields modified by the system
		/// </summary>
		List<MemberInfo> manipulatedFields = new List<MemberInfo>();

        /// <summary>
        /// The mapper associated with this controller
        /// </summary>
        private EntityMapperBase mapper;

        /// <summary>
        /// A mapping of members to formatters used to serialize their input data
        /// </summary>
        Dictionary<MemberInfo, IWebFormatter> formatters = new Dictionary<MemberInfo, IWebFormatter>();

        /// <summary>
        /// Empty object array for no-parameter method signatures
        /// </summary>
        protected object[] EmptyParams = new object[] { };

        /// <summary>
        /// Constructor for our controller
        /// </summary>
        protected ConstructorInfo controllerConstructor;

        /// <summary>
        /// Our logger
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// The application object
        /// </summary>
        private Application application;

		/// <summary>
		/// Initializes a new instance of the <see cref="ControllerHandler"/> class.
		/// </summary>
		/// <param name="controllerType">Type of the controller.</param>
        protected internal ControllerHandler(Application application, IControllerDescriptor descriptor, ILogger logger)
        {
            this.descriptor = descriptor;
            this.logger = logger;
            this.application = application;

            controllerConstructor = ((Type)descriptor.ControllerType).GetConstructor(Type.EmptyTypes);

            foreach (ControllerDescriptor.BindPointDescriptor bindPoint in descriptor.Targets)
                manipulatedFields.AddRange(bindPoint.ParameterFields.Values);
            manipulatedFields.AddRange(descriptor.FormFields.Values);
            manipulatedFields.AddRange(descriptor.RequestFields.Values);
            manipulatedFields.AddRange(descriptor.SessionFields.Values);
            foreach (ControllerDescriptor.CookieFieldDescriptor cookieField in descriptor.CookieFields.Values)
                manipulatedFields.Add(cookieField.Field);

            manipulatedFields.ForEach(
                (mbr) => 
                {
                    var attributes = mbr.GetCustomAttributes(typeof(FormatAsAttribute), false) as FormatAsAttribute[];
                    // Field can be Request and FormField at the same time. In that case - it will be added twice into the collection.
                    if ((attributes.Length != 1) || (formatters.ContainsKey(mbr)))
                        return;
                    
                    formatters.Add(mbr, application.FormatManagerFactory.GetManagerInstance().GetFormatterByFormat(attributes[0].FormatName));
                }
            );

		    BuildMapper(descriptor);
        }

        /// <summary>
        /// Builds the mapper. MapsWith attribute takes priority over InferMappingFor. Also, multiple mapping attributes are currently unsupported.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        protected virtual void BuildMapper(IControllerDescriptor descriptor)
        {
            var mapperAttribute = descriptor.ControllerType.GetCustomAttributes(typeof (MapsWithAttribute), false) as MapsWithAttribute[];
            if (mapperAttribute != null && mapperAttribute.Length == 1)
            {
                mapper = Activator.CreateInstance(mapperAttribute[0].MapperType) as EntityMapperBase;

                if (mapper != null)
                {
                    MapperRepository.Instance.RegisterMapper(mapper);
                    return;
                }
            }

            var inferredAttribute = descriptor.ControllerType.GetCustomAttributes(typeof(InferMappingForAttribute), false) as InferMappingForAttribute[];
            if (inferredAttribute != null && inferredAttribute.Length == 1)
            {
                var mapperType =
                    typeof (EntityMapper<,>).MakeGenericType(new Type[]
                                                                 {
                                                                     descriptor.ControllerType as Type,
                                                                     inferredAttribute[0].TargetType
                                                                 });
                mapper =
                    Activator.CreateInstance(mapperType) as EntityMapperBase;

                // configure the mapper based on the strict flag.
                if (inferredAttribute[0].Strict)
                    mapper.InferStrictOnly();
                else
                    mapper.InferOnly();

                MapperRepository.Instance.RegisterMapper(mapper);
            }
        }

        /// <summary>
		/// Gets the controller instance.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		/// <param name="requestContext">The request context.</param>
		/// <returns></returns>
		public virtual IController GetControllerInstance(ControllerInvocationInfo info, HttpContextBase context, IContext requestContext)
		{
			IController instance = controllerConstructor.Invoke(EmptyParams) as IController;
			instance.Initialize();

			// the order here matters. we may have something that's both a session field
			// and a request field. want to make sure that it gets set in a consistent fashion

			// also, keep checking for presence of values. this way, if something is
			// marked as more than one scope, and is present in one but not the other,
			// it won't get overriden with a null.

			foreach (string field in info.Parameters.Keys)
			    // not all path parameters may be present on the controller
			    if (info.BindPoint.ParameterFields.ContainsKey(field))
			        SetValue(
			            instance, 
			            info.BindPoint.ParameterFields[field], 
			            info.Parameters[field]);



            if (context != null)
            {
                if (context.Session != null)
                {
                    // asking for a non-existant session value will simply return null
                    ArrayList allSessionFields = new ArrayList(context.Session.Keys);
                    foreach (string sessionField in descriptor.SessionFields.Keys)
                        if (allSessionFields.Contains(sessionField))
                            SetValue(instance, descriptor.SessionFields[sessionField], context.Session[sessionField]);
                }

                //TODO: both the allCookies and allFormFields collections should be computed once per request, not for every controller
                List<string> allCookies = new List<string>(context.Request.Cookies.AllKeys);
                foreach (string cookie in descriptor.CookieFields.Keys)
                    if (allCookies.Contains(cookie))
                        SetValue(instance, descriptor.CookieFields[cookie].Field, context.Request.Cookies[cookie].Value);

                HttpFileCollectionBase files = context.Request.Files;
                foreach (string file in files.AllKeys)
                    if (descriptor.FormFields.ContainsKey(file))
                        SetFileValue(instance, descriptor.FormFields[file], files[file]);

                List<string> allFormFields = new List<string>(context.Request.Form.AllKeys);
                foreach (string formField in descriptor.FormFields.Keys)
                    if (allFormFields.Contains(formField))
                        SetValue(instance, descriptor.FormFields[formField], context.Request.Form[formField]);
            }

			foreach (string requestField in descriptor.RequestFields.Keys)
				if (requestContext.Contains(requestField))
					SetValue(instance, descriptor.RequestFields[requestField], requestContext[requestField]);

            if (mapper != null)
            {
                IMappable mappable = instance as IMappable;
                if (mappable != null)
                    mappable.Mapper = mapper;
            }

			return instance;
		}


		/// <summary>
		/// Prepares the controller for a new lifecycle.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <param name="context">The http context for the current request.</param>
		/// <param name="requestContext"></param>
		public virtual void ReturnController(IController controller, HttpContextBase context, IContext requestContext)
		{
			// populate outbound values
			foreach (string cookie in descriptor.CookieFields.Keys)
                if (descriptor.CookieFields[cookie].Outbound)
                {
                    HttpCookie newCookie = context.Response.Cookies[cookie] ?? new HttpCookie(cookie);
                    var val = GetValue(controller, descriptor.CookieFields[cookie].Field);
                    if (val != null)
                        newCookie.Value = Convert.ToString(val);

                    newCookie.Expires = val == null ? DateTime.MinValue : DateTime.MaxValue;
                    context.Response.Cookies.Add(newCookie);
                }

			foreach (string requestField in descriptor.RequestFields.Keys)
				requestContext[requestField] = GetValue(controller, descriptor.RequestFields[requestField]);

			foreach (string sessionField in descriptor.SessionFields.Keys)
				context.Session[sessionField] = GetValue(controller, descriptor.SessionFields[sessionField]);

			controller.Recycle();

			foreach (MemberInfo info in manipulatedFields)
				SetValue(controller, info, null);
		}

		protected void SetFileValue(object instance, MemberInfo member, HttpPostedFileBase file) 
		{
			PropertyInfo pInfo = member as PropertyInfo;
			if (pInfo == null) {
				FieldInfo fInfo = (FieldInfo)member;
				if (file == null || string.IsNullOrEmpty(file.FileName))
					fInfo.SetValue(instance, null);
				else if (fInfo.FieldType == typeof(HttpPostedFileBase))
					fInfo.SetValue(instance, file);
				else if (fInfo.FieldType == typeof(String))
					fInfo.SetValue(instance, ReadFileText(file));
				else if (fInfo.FieldType == typeof(Stream))
					fInfo.SetValue(instance, file.InputStream);
				else if (fInfo.FieldType == typeof(byte[]))
					fInfo.SetValue(instance, ReadFileBytes(file));
				else
					throw new ApplicationException(Messages.InvalidFormFieldFileType.ToString());
			}
			else
				if (file == null || string.IsNullOrEmpty(file.FileName))
					pInfo.SetValue(instance, null, null);
				else if (pInfo.PropertyType == typeof(HttpPostedFileBase))
					pInfo.SetValue(instance, file, null);
				else if (pInfo.PropertyType == typeof(String))
					pInfo.SetValue(instance, ReadFileText(file), null);
				else if (pInfo.PropertyType == typeof(Stream))
					pInfo.SetValue(instance, file.InputStream, null);
				else if (pInfo.PropertyType == typeof(byte[]))
					pInfo.SetValue(instance, ReadFileBytes(file), null);
				else
					throw new ApplicationException(Messages.InvalidFormFieldFileType.ToString());
		}
		byte[] ReadFileBytes(HttpPostedFileBase file) {
			byte[] bytes = new byte[file.ContentLength];
			using (Stream stream = file.InputStream) {
				stream.Read(bytes, 0, file.ContentLength);
			}
			return bytes;
		}
		string ReadFileText(HttpPostedFileBase file) {
			string text = System.Text.UTF8Encoding.UTF8.GetString(ReadFileBytes(file));
			return text;
		}
				
		/// <summary>
		/// Sets the value of a PropertyInfo or FieldInfo represented by member to equal value.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="member">The member.</param>
		/// <param name="value">The value.</param>
		protected void SetValue(object instance, MemberInfo member, object value)
		{
            // potentially valid - bind parameter with no corresponding field
            if (member == null)
                return;

            try
            {
                PropertyInfo pInfo = member as PropertyInfo;
                if (pInfo == null)
                {
                    FieldInfo fInfo = (FieldInfo)member;
                    if (fInfo.FieldType.Equals(typeof(System.DateTime)))
                        ConvertToDateTime(ref value);
                    fInfo.SetValue(instance, value == null ? null : Coerce(value, member, fInfo.FieldType));
                }
                else
                {
                    if (pInfo.CanWrite)
                    {
                        if (pInfo.PropertyType.Equals(typeof(System.DateTime)))
                            ConvertToDateTime(ref value);
                        pInfo.SetValue(instance, value == null ? null : Coerce(value, member, pInfo.PropertyType), null);
                    }
                }
            }
            catch (Exception ex)
            {
                // we should never crash here. handle it and move on.
                logger.Report(Messages.UnableToSetValue, member.DeclaringType.Name, member.Name, Convert.ToString(value), ex.Message);
            }
		}

        //Tries to convert the specific string to System.DateTime current culture.
        private void ConvertToDateTime(ref object value)
        {
            if (value == null)
                return;

            DateTime date;
            if (!DateTime.TryParse(HttpUtility.UrlDecode(value.ToString()), CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
            {
                logger.Report(Messages.UnableToParseDate, value.ToString());
                return;
            }

            value = (object)date;
        }

        /// <summary>
        /// Coerces the value to the specified type. If the types are assignable, nothing is done. 
        /// Otherwise, an attempt is made to invoke Convert.ChangeType().
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private object Coerce(object value, MemberInfo field, Type type)
        {
            // if it's the same, no worries
            if (type.IsAssignableFrom(value.GetType()))
                return value;
            // if it's a value type, or we don't have a formatter that can take care of it, 
            // try the ChangeType option
            else if (type.IsValueType ||
                (application.FormatManagerFactory.GetManagerInstance().GetDefaultFormatter() == null && !formatters.ContainsKey(field)))
            {
                //Nullable<ValueType> is a value-type too, but Convert.ChangeType will fail to convert value to such type.
                //Controller's field can be of such type.
                //Extracting it's ValueType from the Nullable<> generic parameter and converting to that type - resolves that problem.
                return Convert.ChangeType(
                    value,
                    (type.IsGenericType
                        && (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        && !type.ContainsGenericParameters)
                            ? type.GetGenericArguments()[0] : type);
            }
            else
            {
                IWebFormatter formatter;
                if (!formatters.TryGetValue(field, out formatter))
                    formatter = application.FormatManagerFactory.GetManagerInstance().GetDefaultFormatter();

                // we can't find (or don't have) an explicit formatter, 
                // and we don't have a default to fall back to. buh-bye.
                if (formatter == null)
                    throw new InvalidCastException(
                        String.Format(
                            "Unable to convert value for {0}.{1} from {2} to {3}. Specific formatter not found, and no default formatter specified",
                            field.DeclaringType.Name,
                            field.Name,
                            value.GetType().Name,
                            type.Name));

                // value will never be null
                return formatter.Deserialize(type, value.ToString());
            }
        }

		/// <summary>
		/// Gets the value of a PropertyInfo or FieldInfo represented by member.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="member">The member.</param>
		/// <returns></returns>
		protected object GetValue(object instance, MemberInfo member)
		{
			PropertyInfo pInfo = member as PropertyInfo;
            if (pInfo == null)
            {
                FieldInfo fInfo = (FieldInfo)member;
                return fInfo.GetValue(instance);
            }
            else
            {
                if (!pInfo.CanRead)
                    return null;
                else
                    return pInfo.GetValue(instance, null);
            }
		}
	}
}
