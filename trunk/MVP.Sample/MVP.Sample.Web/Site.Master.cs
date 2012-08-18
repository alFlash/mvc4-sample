using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MVP.Base.BaseUserControl;

namespace MVP.Sample.Web
{
    public partial class SiteMaster : MasterPage, IMasterPage
    {


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Implementation of IMasterPage

        public void ShowErrorMessage(string message)
        {
            lblErrorMessage.Text = message;
            //throw new NotImplementedException();
        }

        public void ClearErrorMessage()
        {
            lblErrorMessage.Text = string.Empty;
            //throw new NotImplementedException();
        }

        #endregion
    }
}
