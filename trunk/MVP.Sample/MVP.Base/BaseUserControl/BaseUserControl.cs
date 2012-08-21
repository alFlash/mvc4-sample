using System;
using System.Collections.Generic;
using System.Web.UI;
using MVP.Base.BasePresenter;

namespace MVP.Base.BaseUserControl
{
    public class BaseUserControl<TPresenter> : UserControl, IBaseUserControl where TPresenter : class, IBasePresenter
    {
        #region Properties

        public TPresenter Presenter { get; set; }
        public virtual string ParentControl { get; set; }
        public virtual List<string> RelatedControls { get; set; }

        #endregion

        #region Public Methods
        /// <summary>
        /// Attaches the event handler.
        /// </summary>
        public virtual void AttachEventHandler() { }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        public string GetResource(string className, string resourceKey)
        {
            var result = GetGlobalResourceObject(className, resourceKey);
            return result != null ? result.ToString() : string.Empty;
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        public string GetResource(string resourceKey)
        {
            return GetResource(GetDefautResourceClassName(), resourceKey);
        }

        /// <summary>
        /// Gets the name of the defaut resource class.
        /// </summary>
        /// <returns></returns>
        public virtual string GetDefautResourceClassName()
        {
            return "Common";
        }

        /// <summary>
        /// Re-load the page.
        /// </summary>
        public virtual void ReLoad (){}

        /// <summary>
        /// Commits the changes.
        /// </summary>
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Presenter = (TPresenter)Activator.CreateInstance(typeof(TPresenter), this);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BaseControlCollection.Register(ParentControl, ID, this);
            AttachEventHandler();
        }

        #endregion

        #region Implementation of IBaseUserControl
        #endregion
    }
}