using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.UnitTests.Support.Reflection
{
    internal class TestTypeInfo : ITypeInfo
    {
        public class TestAttributeInfo : IAttributeInfo
        {



            public TestAttributeInfo(Type type)
            {
                this.type = type.FullName;
                this.properties = new ParameterCollection();
            }

            public TestAttributeInfo(Type type, params Parameter[] parameters)
            {
                this.type = type.FullName;
                this.properties = new ParameterCollection(parameters);
            }
            string type;
            ParameterCollection properties;

            #region IAttributeInfo Members

            public string Type
            {
                get { return type; }
            }

            public IAttributeProperties Properties
            {
                get { return properties; }
            }

            #endregion
        }

        public class TestFieldInfo : IFieldInfo
        {
            public TestFieldInfo(
                string name,
                string type,
                params TestAttributeInfo[] attributes)
            {
                this.name = name;
                this.type = type;
                this.attributes = attributes;
            }

            string name;
            string type;
            TestAttributeInfo[] attributes;

            #region IFieldInfo Members

            public IEnumerable<IAttributeInfo> Attributes
            {
                get { return new EnumProxy<TestAttributeInfo, IAttributeInfo>(attributes); }
            }

            //public IEnumerable<IAttributeInfo> GetCustomAttributes(Type attributeType, bool inherit)
            //{
            //    return new EnumProxy<TestAttributeInfo, IAttributeInfo>(attributes.Where(attrInfo => { return attrInfo.Type == attributeType.FullName; }));
            //}



            public string Name
            {
                get { return name; }
            }

            public string Type
            {
                get { return type; }
            }

            #endregion

        }

        public class TestPropertyInfo : IPropertyInfo
        {
            public TestPropertyInfo(
                string name,
                string type,
                params TestAttributeInfo[] attributes)
            {
                this.name = name;
                this.type = type;
                this.attributes = attributes;
            }
            string name;
            string type;
            TestAttributeInfo[] attributes;

            #region IPropertyInfo Members

            public IEnumerable<IAttributeInfo> Attributes
            {
                get { return new EnumProxy<TestAttributeInfo, IAttributeInfo>(attributes); }
            }

            //public IEnumerable<IAttributeInfo> GetCustomAttributes(Type attributeType, bool inherit)
            //{
            //    return new EnumProxy<TestAttributeInfo, IAttributeInfo>(attributes.Where(attrInfo => { return attrInfo.Type == attributeType.FullName; }));
            //}


            public string Name
            {
                get { return name; }
            }

            public string Type
            {
                get { return type; }
            }

            #endregion


        }

        public TestTypeInfo(
            string fullName,
            TestAttributeInfo[] attributes,
            TestFieldInfo[] fields,
            TestPropertyInfo[] properties)
        {
            this.fullName = fullName;
            this.attributes = attributes;
            this.fields = fields;
            this.properties = properties;
        }
        string fullName;
        TestAttributeInfo[] attributes;
        TestFieldInfo[] fields;
        TestPropertyInfo[] properties;

        #region ITypeInfo Members

        public bool IsAbstract
        {
            get { return false; }
        }

        public string FullName
        {
            get { return fullName; }
        }

        #region Proxies
        class EnumProxy<TSource, TTarget, TProxy> : IEnumerable<TTarget>
            where TSource : class
            where TTarget : class
            where TProxy : TTarget
        {
            IEnumerable<TSource> list;

            public EnumProxy(IEnumerable<TSource> list)
            {
                this.list = list;
            }

            #region IEnumerable<TTarget> Members

            public IEnumerator<TTarget> GetEnumerator()
            {
                foreach (TSource item in list)
                    yield return (TTarget)Activator.CreateInstance(typeof(TProxy), item);
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }


        class EnumProxy<TSource, TTarget> : IEnumerable<TTarget>
            where TSource : TTarget
            where TTarget : class
        {
            IEnumerable<TSource> list;

            public EnumProxy(IEnumerable<TSource> list)
            {
                this.list = list;
            }

            #region IEnumerable<TTarget> Members

            public IEnumerator<TTarget> GetEnumerator()
            {
                foreach (TSource item in list)
                    yield return (TTarget)item;
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }
        #endregion

        public IEnumerable<IAttributeInfo> GetCustomAttributes(Type attributeType, bool inherit)
        {
            return new EnumProxy<TestAttributeInfo, IAttributeInfo>(attributes.Where(attrInfo => { return attrInfo.Type == attributeType.FullName; }));
        }

        //public IEnumerable<IMemberInfo> GetMember(string name,
        //                                    BindingFlags bindingAttr)
        //{
        //    return GetMember(name, MemberTypes.All, bindingAttr);
        //}

        //public IEnumerable<IMemberInfo> GetMember(string name,
        //                                    MemberTypes type,
        //                                    BindingFlags bindingAttr)
        //{
        //    return Fields.OfType<IMemberInfo>().Union(Properties.OfType<IMemberInfo>()).Where(member => { return member.Name == name; });
        //}

        //public IEnumerable<IMemberInfo> GetMembers(BindingFlags bindingAttr)
        //{
        //    return Fields.OfType<IMemberInfo>().Union(Properties.OfType<IMemberInfo>());
        //}

        public IEnumerable<IAttributeInfo> Attributes
        {
            get { return new EnumProxy<TestAttributeInfo, IAttributeInfo>(attributes); }
        }

        public IEnumerable<IFieldInfo> Fields
        {
            get { return new EnumProxy<TestFieldInfo, IFieldInfo>(fields); }
        }

        public IEnumerable<IPropertyInfo> Properties
        {
            get { return new EnumProxy<TestPropertyInfo, IPropertyInfo>(properties); }
        }

        #endregion


    }
    #region Attribute Parameters
    public class Parameter : IAttributeProperty
    {
        public Parameter(object value)
            : this(null, value)
        {
        }

        public Parameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
        string name;
        object value;

        public string Name { get { return name; } }

        #region IAttributeParameter Members

        public string AsString()
        {
            //return value.ToString();
            if (value == null)
                return null;
            else
                return value.ToString();
        }

        public bool? AsNBoolean() { return AsNBoolean(null); }

        public bool? AsNBoolean(bool? @default)
        {
            if (value != null)
                if (value is bool)
                    return (bool)value;
            return @default;
        }

        public int? AsNInt32() { return AsNInt32(null); }

        public int? AsNInt32(int? @default)
        {
            if (value != null)
                if (value is int)
                    return (int)value;
            return @default;
        }

        public Enum AsEnum() { return AsEnum(null); }
        public Enum AsEnum(Enum @default)
        {
            if (value != null)
                if (value is Enum)
                    return (Enum)value;
            return @default;
        }

        #endregion
    }

    public class ParameterCollection : IAttributeProperties
    {
        public ParameterCollection()
            : this(new Parameter[] { })
        {
        }

        public ParameterCollection(Parameter[] parameters)
        {
            this.parameters = parameters;
            foreach (Parameter p in parameters)
                if (p.Name != null)
                    d.Add(p.Name, p);
        }

        public ParameterCollection(IEnumerable<Parameter> parameters)
        {
            this.parameters = parameters.ToArray();
            foreach (Parameter p in parameters)
                if (p.Name != null)
                    d.Add(p.Name, p);
        }

        Parameter[] parameters;
        Dictionary<string, Parameter> d = new Dictionary<string, Parameter>();

        #region IAttributeParameters Members

        public int Count
        {
            get { return parameters.Length; }
        }

        public IAttributeProperty this[int index]
        {
            get { return parameters[index]; }
        }

        public IAttributeProperty this[string name]
        {
            get
            {
                Parameter result = null;
                if (!d.TryGetValue(name, out result))
                {
                    result = new Parameter(null);
                }
                return result;
            }
        }

        #endregion
    }
    #endregion

}
