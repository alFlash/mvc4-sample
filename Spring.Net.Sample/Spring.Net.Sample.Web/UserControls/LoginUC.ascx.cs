using System;
using System.Web.UI;

namespace Spring.Net.Sample.Web.UserControls
{
    public partial class LoginUC : UserControl
    {
        #region Properties
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

        public IDataProvider LoginProvider { get; set; }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            btnLogin.Click += BtnLoginClick;
        }

        private void BtnLoginClick(object sender, EventArgs e)
        {
            Message = LoginProvider.Login(Username, Password) ? "Successfully logged in." : "Login failed";
        }

        #endregion
    }


    public interface IDataProvider
    {
        bool Login(string username, string password);
    }

    public class LoginProvider : IDataProvider
    {
        public bool Login(string username, string password)
        {
            return username == "hoanggia" && password == "hoanggia";
        }
    }
}