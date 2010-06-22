using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Bistro.Entity
{
    /// <summary>
    /// Wrapper class for seamlessly dealing with properties and members
    /// </summary>
    public class MemberAccessor
    {
        private PropertyInfo property;
        private FieldInfo field;

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <value>The member.</value>
        public MemberInfo Member { get { return (MemberInfo)property ?? field; }}

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessor"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        public MemberAccessor(MemberInfo member)
        {
            property = member as PropertyInfo;
            if (property == null)
                field = member as FieldInfo;
        }

        /// <summary>
        /// Gets the declared type of the property or field.
        /// </summary>
        /// <value>The type.</value>
        public Type TargetType
        {
            get
            {
                if (property != null)
                    return property.PropertyType;

                return field.FieldType;
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        public void SetValue(object instance, object value)
        {
            if (property != null)
                property.SetValue(instance, value, null);
            else
                field.SetValue(instance, value);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public object GetValue(object instance)
        {
            if (property != null)
                return property.GetValue(instance, null);

            return field.GetValue(instance);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can be read.
        /// </summary>
        /// <value><c>true</c> if this instance can be read; otherwise, <c>false</c>.</value>
        public bool CanRead
        {
            get
            {
                if (property != null)
                    return property.CanRead;

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can be written.
        /// </summary>
        /// <value><c>true</c> if this instance can be written; otherwise, <c>false</c>.</value>
        public bool CanWrite
        {
            get
            {
                if (property != null)
                    return property.CanWrite;

                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new NullReferenceException();

            var target = obj as MemberAccessor;
            if (target == null)
                return false;

            return Member.Equals(target.Member);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Member.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Member.Name;
        }
    }
}
