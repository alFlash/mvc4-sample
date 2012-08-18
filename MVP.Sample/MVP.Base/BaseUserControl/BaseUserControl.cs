using System;
using System.Collections.Generic;
using System.Web.UI;
using MVP.Base.BasePresenter;

namespace MVP.Base.BaseUserControl
{
    public class BaseUserControl : UserControl, IBaseUserControl
    {
        #region Properties
        public IBasePresenter Presenter { get; set; }
        //public virtual string ControlKey { get { return GetType().FullName; } }
        public virtual List<string> ChildControls { get; set; } //Store the fullname of (Children)Usercontrols' Type 
        public virtual string ParentControl { get; set; }

        public virtual List<string> RelatedControls { get; set; }

        #endregion

        #region Public Methods
        public virtual void AttachEventHandler() { }

        public void ShowErrorMessage(string message)
        {
            if (Page.Master == null || !(Page.Master is IMasterPage)) return;
            var masterPage = (IMasterPage)Page.Master;
            masterPage.ShowErrorMessage(message);
        }

        public string GetResource(string className, string resourceKey)
        {
            var result = GetGlobalResourceObject(className, resourceKey);
            return result != null ? result.ToString() : string.Empty;
        }

        public string GetResource(string resourceKey)
        {
            return GetResource(GetDefautResourceClassName(), resourceKey);
        }

        public virtual string GetDefautResourceClassName()
        {
            return "Common";
        }

        public virtual void ReLoad (){}

        public void CommitChanges()
        {
            if (!string.IsNullOrEmpty(ParentControl) && RelatedControls != null && RelatedControls.Count > 0)
            {
                if (BaseControlCollection.Controls.ContainsKey(ParentControl))
                {
                    var parentControl = BaseControlCollection.Controls[ParentControl];
                    if (parentControl != null)
                    {
                        foreach (var controlKey in RelatedControls)
                        {
                            parentControl[controlKey].ReLoad();
                        }
                    }
                }
            }
        }

        #endregion

        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BaseControlCollection.Register(ParentControl, ID, this);
            AttachEventHandler();
        }

        #endregion
    }
}