using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers;
using Bistro.UnitTests.Support.Reflection;
using Bistro.Controllers.Descriptor;
using Bistro.Controllers.Descriptor.Data;

namespace Bistro.UnitTests.Support.CustomManager
{
    public class TestControllerManager : ControllerManager
    {
        public TestControllerManager(Application app) : base(app) { }


        public override void Load()
        {
            //do nothing here
        }


        public void LoadSpecial(IEnumerable<ITypeInfo> controllersList)
        {
            #region IsMarked substitute
            Func<IMemberInfo, string, bool> HasAttribute =
                (testMemberInfo, attrName)
                =>
                {
                    foreach (IAttributeInfo attrInfo in testMemberInfo.Attributes)
                    {
                        if (attrInfo.Type == attrName)
                            return true;
                    }
                    return false;

                };
            #endregion


            foreach (ITypeInfo typeInfo in controllersList)
            {

                IList<string> providesTemp = new List<string>();
                IList<string> requiresTemp = new List<string>();
                IList<string> dependsOnTemp = new List<string>();
                IList<BindAttribute> bindsTemp = new List<BindAttribute>();


                IList<IMemberInfo> allMembers = typeInfo.Properties.OfType<IMemberInfo>().Concat(typeInfo.Fields.OfType<IMemberInfo>()).ToList();


                foreach (IMemberInfo memberInfo in allMembers)
                {


                    if (!HasAttribute(memberInfo, typeof(RequiresAttribute).FullName) &&
                        !HasAttribute(memberInfo, typeof(DependsOnAttribute).FullName) &&
                        (HasAttribute(memberInfo, typeof(SessionAttribute).FullName) ||
                        HasAttribute(memberInfo, typeof(RequestAttribute).FullName)))
                        providesTemp.Add(memberInfo.Name);

                    if (HasAttribute(memberInfo, typeof(DependsOnAttribute).FullName))
                    {
                        dependsOnTemp.Add(memberInfo.Name);
                    }

                    if (HasAttribute(memberInfo, typeof(RequiresAttribute).FullName))
                    {
                        requiresTemp.Add(memberInfo.Name);
                    }

                    if (HasAttribute(memberInfo, typeof(ProvidesAttribute).FullName))
                    {
                        if (!providesTemp.Contains(memberInfo.Name))
                            providesTemp.Add(memberInfo.Name);
                    }
                }

                IList<IAttributeInfo> bindAttrs = typeInfo.Attributes.Where((attrib) => { return attrib.Type == typeof(BindAttribute).FullName; }).ToList();

                foreach (IAttributeInfo bindAttrInfo in bindAttrs)
                {
                    BindAttribute bindAttr = new BindAttribute(bindAttrInfo.Properties["Target"].AsString());
                    bindAttr.ControllerBindType = (BindType)(bindAttrInfo.Properties["ControllerBindType"].AsEnum());
                    bindAttr.Priority = bindAttrInfo.Properties["Priority"].AsNInt32().Value;
                    bindsTemp.Add(bindAttr);
                }



                ControllerDescriptor testDescriptor = ControllerDescriptor.CreateDescriptorRaw(
                    new TestMemberInfo(typeInfo.FullName),
                    dependsOnTemp,
                    requiresTemp,
                    providesTemp,
                    null,
                    null,
                    null,
                    null,
                    bindsTemp,
                    logger);
                RegisterController(testDescriptor);
            }
        }




        protected override void LoadAssembly(System.Reflection.Assembly assm)
        {
            // do nothing
        }
        protected override void LoadType(Type t)
        {
            // also do nothing here - we'll handle all the test load in the Load()
        }

        public override void RegisterController(Bistro.Controllers.Descriptor.ControllerDescriptor descriptor)
        {
            dispatcherFactory.GetDispatcherInstance().RegisterController(descriptor);
        }

    }
}
