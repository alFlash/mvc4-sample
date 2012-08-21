<%@ Control language="vb" Inherits="DNN.MVP.Sample.ModuleName1.ViewModuleName1" AutoEventWireup="false" Explicit="True" Codebehind="ViewModuleName1.ascx.vb" %>
<%@ Register Src="UserControls/DependencyUC2.ascx" TagPrefix="uc" TagName="DependencyUC2" %>
<%@ Register Src="UserControls/DependencyUC1.ascx" TagPrefix="uc" TagName="DependencyUC1" %>

<div>
    <uc:DependencyUC1 runat="server" ID="ucDependencyUC1" />
</div>
<div>
    <uc:DependencyUC2 runat="server" ID="ucDependencyUC2" />
</div>