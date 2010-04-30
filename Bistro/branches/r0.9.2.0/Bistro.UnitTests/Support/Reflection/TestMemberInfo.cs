using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Bistro.UnitTests.Support.Reflection
{
    public class TestMemberInfo : MemberInfo
    {
        public TestMemberInfo(string typeName)
            : base()
        {
            name = typeName;
        }

        private string name;

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override Type DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return (object[])Array.CreateInstance(attributeType, 0);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override MemberTypes MemberType
        {
            get { throw new NotImplementedException(); }
        }

        public override Type ReflectedType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
