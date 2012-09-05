using System;
using System.Collections.Generic;
using MVP.Base.BaseUserControl;
using MVP.Base.Common;
using MVP.Sample.Web.IViews;
using MVP.Sample.Web.Presenters;

namespace MVP.Sample.Web.UserControls
{
    public partial class LoginUC : BaseUserControl<LoginUCPresenter>, ILoginUCView
    {

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            PageMode = PageMode.None;
            Presenter.DoAction();
        }

        void BtnCancelClick(object sender, EventArgs e)
        {
            PageMode = PageMode.None;
            Presenter.DoAction();
        }

        void BtnLoginClick(object sender, EventArgs e)
        {
            PageMode = PageMode.View;
            Presenter.DoAction();
        }
        #endregion

        #region Public Methods
        public override void AttachEventHandler()
        {
            base.AttachEventHandler();
            btnLogin.Click += BtnLoginClick;
            btnCancel.Click += BtnCancelClick;
        }

        #endregion

        #region Implementation of IBaseView

        public PageMode PageMode { get; set; }
        public string PageTitle { get; set; }
        public PageStatus PageStatus { get; set; }
        public List<string> ErrorMessages { get; set; }
        public void ShowErrorMessage()
        {
        }

        public void AddErrorMessage(string errorMessage)
        {
        }

        #endregion

        #region Implementation of ILoginUCView

        public string Username
        {
            get { return txtUsername.Text; }
            set { txtUsername.Text = value; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        public string Message
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; }
        }

        #endregion
    }
}