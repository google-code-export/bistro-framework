using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.FSharp.ProjectSystem;

namespace FSharp.ProjectExtender.Project.Excluded
{
    //[ComVisible(true)]
    //public class NodeProperties : LocalizableProperties, IVsBrowseObject, ISpecifyPropertyPages, IVsGetCfgProvider, IVsSpecifyProjectDesignerPages, 
    //    EnvDTE80.IInternalExtenderProvider

    //{
    //    ExcludedNode node;
    //    public NodeProperties(ExcludedNode node)
    //    {
    //        this.node = node;
    //    }

    //    #region properties
    //    //[Browsable(false)]
    //    //[AutomationBrowsable(false)]
    //    //public HierarchyNode Node
    //    //{
    //    //    get { return this.node; }
    //    //}

    //    /// <summary>
    //    /// Used by Property Pages Frame to set it's title bar. The Caption of the Hierarchy Node is returned.
    //    /// </summary>
    //    [Browsable(false)]
    //    public virtual string Name
    //    {
    //        get { return this.node.Path; }
    //    }

    //    #endregion

    //    #region IVsBrowseObject Members

    //    public int GetProjectItem(out IVsHierarchy pHier, out uint pItemid)
    //    {
    //        pHier = node;
    //        pItemid = node.ItemId;
    //        return VSConstants.S_OK;
    //    }

    //    #endregion

    //    #region ISpecifyPropertyPages methods
    //    public virtual void GetPages(CAUUID[] pages)
    //    {
    //        pages[0] = new CAUUID();
    //        pages[0].cElems = 0;
    //    }
    //    #endregion

    //    #region IVsGetCfgProvider methods
    //    public virtual int GetCfgProvider(out IVsCfgProvider p)
    //    {
    //        p = null;
    //        return VSConstants.E_NOTIMPL;
    //    }
    //    #endregion
    
    //    #region IVsSpecifyProjectDesignerPages
    //    /// <summary>
    //    /// Implementation of the IVsSpecifyProjectDesignerPages. It will retun the pages that are configuration independent.
    //    /// </summary>
    //    /// <param name="pages">The pages to return.</param>
    //    /// <returns></returns>
    //    public virtual int GetProjectDesignerPages(CAUUID[] pages)
    //    {
    //        pages[0] = new CAUUID();
    //        pages[0].cElems = 0;
    //        return VSConstants.S_OK;
    //    }
    //    #endregion


    //    #region IInternalExtenderProvider Members

    //    public bool CanExtend(string ExtenderCATID, string ExtenderName, object ExtendeeObject)
    //    {
    //        return false;
    //    }

    //    public object GetExtender(string ExtenderCATID, string ExtenderName, object ExtendeeObject, EnvDTE.IExtenderSite ExtenderSite, int Cookie)
    //    {
    //        return null;
    //    }

    //    public object GetExtenderNames(string ExtenderCATID, object ExtendeeObject)
    //    {
    //        return null;
    //    }

    //    #endregion
    //}

    /// <summary>
    /// Enables a managed object to expose properties and attributes for COM objects.
    /// </summary>
    [ComVisible(true)]
    public class LocalizableProperties : ICustomTypeDescriptor
    {
        #region ICustomTypeDescriptor
        public virtual AttributeCollection GetAttributes()
        {
            AttributeCollection col = TypeDescriptor.GetAttributes(this, true);
            return col;
        }

        public virtual EventDescriptor GetDefaultEvent()
        {
            EventDescriptor ed = TypeDescriptor.GetDefaultEvent(this, true);
            return ed;
        }

        public virtual PropertyDescriptor GetDefaultProperty()
        {
            PropertyDescriptor pd = TypeDescriptor.GetDefaultProperty(this, true);
            return pd;
        }

        public virtual object GetEditor(Type editorBaseType)
        {
            object o = TypeDescriptor.GetEditor(this, editorBaseType, true);
            return o;
        }

        public virtual EventDescriptorCollection GetEvents()
        {
            EventDescriptorCollection edc = TypeDescriptor.GetEvents(this, true);
            return edc;
        }

        public virtual EventDescriptorCollection GetEvents(System.Attribute[] attributes)
        {
            EventDescriptorCollection edc = TypeDescriptor.GetEvents(this, attributes, true);
            return edc;
        }

        public virtual object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public virtual PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection pcol = GetProperties(null);
            return pcol;
        }

        public virtual PropertyDescriptorCollection GetProperties(System.Attribute[] attributes)
        {
            ArrayList newList = new ArrayList();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this, attributes, true);

            for (int i = 0; i < props.Count; i++)
                newList.Add(CreateDesignPropertyDescriptor(props[i]));

            return new PropertyDescriptorCollection((PropertyDescriptor[])newList.ToArray(typeof(PropertyDescriptor))); ;
        }

        public virtual DesignPropertyDescriptor CreateDesignPropertyDescriptor(PropertyDescriptor propertyDescriptor)
        {
            return new DesignPropertyDescriptor(propertyDescriptor);
        }

        public virtual string GetComponentName()
        {
            string name = TypeDescriptor.GetComponentName(this, true);
            return name;
        }

        public virtual TypeConverter GetConverter()
        {
            TypeConverter tc = TypeDescriptor.GetConverter(this, true);
            return tc;
        }

        public virtual string GetClassName()
        {
            return this.GetType().FullName;
        }

        #endregion ICustomTypeDescriptor
    }

    [ComVisible(true)]
    public abstract class ExcludedNode : ItemNode, IOleCommandTarget//, IVsHierarchy
    {
        protected ExcludedNode(ItemList items, ItemNode parent, Constants.ItemNodeType type, string path)
            : base(items, parent, items.GetNextItemID(), type, path)
        { }

        NodeProperties properties;

        internal virtual int GetProperty(int propId, out object property)
        {
            switch ((__VSHPROPID)propId)
            {
                case __VSHPROPID.VSHPROPID_FirstChild:
                case __VSHPROPID.VSHPROPID_FirstVisibleChild:
                    property = FirstChild;
                    return VSConstants.S_OK;

                case __VSHPROPID.VSHPROPID_NextSibling:
                case __VSHPROPID.VSHPROPID_NextVisibleSibling:
                    property = NextSibling;
                    return VSConstants.S_OK;

                case __VSHPROPID.VSHPROPID_Expandable:
                    property = false;
                    return VSConstants.S_OK;

                case __VSHPROPID.VSHPROPID_Caption:
                    property = System.IO.Path.GetFileName(Path);
                    return VSConstants.S_OK;

                case __VSHPROPID.VSHPROPID_IconIndex:
                    property = ImageIndex;
                    return VSConstants.S_OK;

                //case __VSHPROPID.VSHPROPID_BrowseObject:
                //    if (properties == null)
                //        properties = new NodeProperties(this);
                //    property = new DispatchWrapper(properties);
                //    return VSConstants.S_OK;

                //case __VSHPROPID.VSHPROPID_ItemDocCookie:
                //    property = 10000;
                //    return VSConstants.S_OK;

                default:
                    break;
            }
            property = null;
            return VSConstants.DISP_E_MEMBERNOTFOUND;
        }

        protected abstract int ImageIndex { get; }

        #region IOleCommandTarget Members

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup.Equals(Constants.guidStandardCommandSet2K) && nCmdID == (uint)VSConstants.VSStd2KCmdID.INCLUDEINPROJECT)
                return IncludeItem();

            return VSConstants.S_OK;
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup.Equals(Constants.guidStandardCommandSet2K) && prgCmds[0].cmdID == (uint)VSConstants.VSStd2KCmdID.INCLUDEINPROJECT)
                prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED | (uint)OLECMDF.OLECMDF_ENABLED;

            return VSConstants.S_OK;
        }

        #endregion

        protected abstract int IncludeItem();

        #region IVsHierarchy Members

        public int AdviseHierarchyEvents(IVsHierarchyEvents pEventSink, out uint pdwCookie)
        {
            throw new NotImplementedException();
        }

        public int Close()
        {
            throw new NotImplementedException();
        }

        public int GetCanonicalName(uint itemid, out string pbstrName)
        {
            throw new NotImplementedException();
        }

        public int GetGuidProperty(uint itemid, int propid, out Guid pguid)
        {
            throw new NotImplementedException();
        }

        public int GetNestedHierarchy(uint itemid, ref Guid iidHierarchyNested, out IntPtr ppHierarchyNested, out uint pitemidNested)
        {
            throw new NotImplementedException();
        }

        public int GetProperty(uint itemid, int propid, out object pvar)
        {
            throw new NotImplementedException();
        }

        public int GetSite(out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
        {
            throw new NotImplementedException();
        }

        public int ParseCanonicalName(string pszName, out uint pitemid)
        {
            throw new NotImplementedException();
        }

        public int QueryClose(out int pfCanClose)
        {
            throw new NotImplementedException();
        }

        public int SetGuidProperty(uint itemid, int propid, ref Guid rguid)
        {
            throw new NotImplementedException();
        }

        public int SetProperty(uint itemid, int propid, object var)
        {
            throw new NotImplementedException();
        }

        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            throw new NotImplementedException();
        }

        public int UnadviseHierarchyEvents(uint dwCookie)
        {
            throw new NotImplementedException();
        }

        public int Unused0()
        {
            throw new NotImplementedException();
        }

        public int Unused1()
        {
            throw new NotImplementedException();
        }

        public int Unused2()
        {
            throw new NotImplementedException();
        }

        public int Unused3()
        {
            throw new NotImplementedException();
        }

        public int Unused4()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
