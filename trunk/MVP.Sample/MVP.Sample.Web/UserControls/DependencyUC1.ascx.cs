using System;
using MVP.Base.BaseUserControl;

namespace MVP.Sample.Web.UserControls
{
    public partial class DependencyUC1 : BaseUserControl
    {
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {

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
    }
}