<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Spring.Net.Sample.Web.Default" %>

<%@ Register Src="~/UserControls/LoginUC.ascx" TagPrefix="uc1" TagName="LoginUC" %>


<spring:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</spring:Content>
<spring:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <uc1:loginuc runat="server" id="LoginUC" />
    </div>
</spring:Content>
