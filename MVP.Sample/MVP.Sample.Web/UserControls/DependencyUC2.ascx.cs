using System;
using System.Collections.Generic;
using MVP.Base.BaseUserControl;
using MVP.Base.Common;
using MVP.Sample.Web.IViews;
using MVP.Sample.Web.Presenters;

namespace MVP.Sample.Web.UserControls
{
    public partial class DependencyUC2 : BaseUserControl<DependencyUC2Presenter>, IDependencyUC2View
    {
        #region Members
        
        #endregion

        public override void ReLoad()
        {
            base.ReLoad();
            if (string.IsNullOrWhiteSpace(lblContent.Text))
            {
                lblContent.Text = "Control Updated 1";
            }
            else
            {
                var strArray = lblContent.Text.Split(' ');
                var count = Convert.ToInt32(strArray[strArray.Length - 1]);
                var result = string.Empty;
                for (int i = 0; i < strArray.Length - 1; i++)
                {
                    result += (strArray[i] + ' ');
                }
                result += (count + 1);
                lblContent.Text = result;
            }

            PageMode = PageMode.View;
            Presenter.DoAction();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageMode = PageMode.View;
            Presenter.DoAction();
        }

        public override void AttachEventHandler()
        {
            base.AttachEventHandler();
            btnUpdate.Click += BtnUpdateClick;
        }

        void BtnUpdateClick(object sender, EventArgs e)
        {
            CommitChanges();
        }

        #region Implementation of IBaseView

        public PageMode PageMode { get; set; }
        public string PageTitle { get; set; }
        public PageStatus PageStatus { get; set; }
        public List<string> ErrorMessages { get; set; }
        public void ShowErrorMessage()
        {
            //throw new NotImplementedException();
        }

        public void AddErrorMessage(string errorMessage)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}