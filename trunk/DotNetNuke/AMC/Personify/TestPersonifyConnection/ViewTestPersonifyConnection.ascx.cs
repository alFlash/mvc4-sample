using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using Hvn.TestPersonifyConnection.Components;
using Personify.ApplicationManager;
using TIMSS;
using TIMSS.API.Core;
using TIMSS.API.ProductInfo;
using TIMSS.Enumerations;

namespace Hvn.Modules.TestPersonifyConnection
{
    public partial class ViewTestPersonifyConnection : PersonifyDNNBaseForm
    {
        protected  void Page_Load(object sender, EventArgs e)
        {
            try
            {
                IProductRateCodes codes =
                    TIMSS.Global.GetCollection(this.OrganizationId, this.OrganizationUnitId, NamespaceEnum.ProductInfo,
                                               "ProductRateCodes") as IProductRateCodes;

                if (codes != null)
                {
                    codes.Fill();

                    string a = "";
                }else
                {
                    string a = "";
                }

               IBusinessObjectCollection ps=
                TIMSS.Global.GetCollection(this.OrganizationId, this.OrganizationUnitId, NamespaceEnum.ProductInfo,
                                           "Products");

                ps.Fill();

                if (ps != null)
                {
                    IProducts p = ps as IProducts;

                    string a = "";
                }
                else
                {
                    string a = "";
                }




            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}