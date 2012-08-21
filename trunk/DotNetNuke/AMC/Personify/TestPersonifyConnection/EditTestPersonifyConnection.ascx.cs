using System;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using Hvn.TestPersonifyConnection.Components;

namespace Hvn.Modules.TestPersonifyConnection
{
    public partial class EditTestPersonifyConnection : PortalModuleBase
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

       
    }
}