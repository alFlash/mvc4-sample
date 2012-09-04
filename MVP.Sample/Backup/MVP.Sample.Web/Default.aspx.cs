using System;
using System.Collections.Generic;
using System.Web.UI;

namespace MVP.Sample.Web
{
    public partial class _Default : Page
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ucDependencyUC1.ParentControl = string.Format("Sample.{0}", ID); //Must be Unique
            ucDependencyUC1.RelatedControls = new List<string>
                                                  {
                                                      "ucDependencyUC2"
                                                  };

            ucDependencyUC2.ParentControl = string.Format("Sample.{0}", ID); //Must be Unique
            ucDependencyUC2.RelatedControls = new List<string>
                                                  {
                                                      "ucDependencyUC1"
                                                  };
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}
