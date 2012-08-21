<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EducationCourseUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.EducationCourseUC" EnableViewState="true" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">

    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-educationCourse", isShow);
        AmcCert.SetTitle("add-educationCourse", "Add Information ");
    }
    jQuery(document).ready(function () {
        var bindingList = [['lblCourseTitle', 'txtCourseTitle'],
                           ['lblStartDate', 'txtStartDate'],
                            ['lblCompletionDate', 'txtCompletionDate'],
                            ['lblDegreeValue', 'lstbDegree'],
                           ['lblOrganizationProvidingCourse', 'txtOrganizationProvidingCourse'],
                           ['lblOrganizationApprovingTheCourse', 'txtOrganizationApprovingTheCourse'],
                           ['lblPoint', 'txtApprovedCE']];

        var practicePopup = new AMCTablePopUp(bindingList,
            "educationCourse-uc",
            "tbleducationCourse",
            "add-educationCourse",
            "Add Information ",
            "Edit Item",
            null);
    });
</script>
<div id="educationCourse-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblEducationCourseUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="optiontitle">
         <asp:Label runat="server" ID="lblOptionTitle"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="imgAdd" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" 
                CssClass="fl padleft5" href="javascript:void(0);"></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater runat="server" ID="rpteducationCourse" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tbleducationCourse" border="1px" class="amc-table">
                        <tr class="amc-table-header">
                            <td visible='<%# GetFieldInfo("CourseTitle").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblNameOfPublisherHeader" Text='<%# GetFieldInfo("CourseTitle").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("StartDate").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblStartDateHeader" Text='<%# GetFieldInfo("StartDate").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("CompletionDate").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblTitlePublicationHeader" Text='<%# GetFieldInfo("CompletionDate").FieldValue %>'></asp:Label>
                            </td>
                             <td visible='<%# GetFieldInfo("Degree").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblDegreeHeader" Text='<%# GetFieldInfo("Degree").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("OrganizationProvidingCourse").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lbldateHeader" Text='<%# GetFieldInfo("OrganizationProvidingCourse").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("OrganizationApprovingTheCourse").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblPagesHeader" Text='<%# GetFieldInfo("OrganizationApprovingTheCourse").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblpointHeader" Text='<%# GetFieldInfo("Point").FieldValue %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSumaryPointHeader" Text="Points"></asp:Label>
                            </td>
                            <td class="action-column">
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="amc-table-content">
                        <td visible='<%# GetFieldInfo("CourseTitle").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblCourseTitle" Text='<%# Eval("ProgramTitle") %>'></asp:Label>
                        </td>
                         <td visible='<%# GetFieldInfo("StartDate").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblStartDate" ></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("CompletionDate").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblCompletionDate"></asp:Label>
                        </td>
                         <td visible='<%# GetFieldInfo("Degree").IsEnabled %>' runat="server">
                             <asp:Label runat="server" ID="lblDegree"></asp:Label>
                            <asp:Label ID="lblDegreeValue" runat="server" Text="" CssClass="dn"></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("OrganizationProvidingCourse").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblOrganizationProvidingCourse" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("OrganizationApprovingTheCourse").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblOrganizationApprovingTheCourse" Text='<%# Eval("OrganizationApproving") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" class="text-right">
                            <asp:Label runat="server" ID="lblPoint" ></asp:Label>
                            <asp:Label runat="server" ID="lblPointValue" Text="" CssClass="dn"></asp:Label>
                        </td>
                        <td class="text-right">
                            <asp:Label runat="server" ID="lblSumaryPoint" Text=""></asp:Label>
                        </td>
                        <td class="action-column">
                            <asp:HiddenField runat="server" ID="hdObjectUniqueId" Value='<%# Eval("Guid") %>' />
                            <asp:Image CssClass="pointer" runat="server" ID="imgEdit" ImageUrl="../../Documentation/images/icons/EditIcon.gif">
                            </asp:Image>
                            <asp:ImageButton CssClass="pointer" runat="server" ID="imgItemDelete" ImageUrl="../../Documentation/images/icons/delete_icon1.gif"
                                CommandName="Delete" CommandArgument='<%# Eval("Guid") %>' CausesValidation="False"></asp:ImageButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="totalCE">
        Total Points:
        <asp:Label runat="server" ID="lblTotalCE"></asp:Label>
    </div>
    <div id="add-educationCourse" class="amc-popup">
        <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div class="amc-error-message">
            <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
            <div>
                <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup"/>
            </div>
        </div>
        <table class="width500">
            <tr id="CourseTitle" runat="server">
                <td>
                    <asp:Label runat="server" ID="lblCourseTitle"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250" ID="txtCourseTitle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCourseTitle" ControlToValidate="txtCourseTitle"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                </td>
            </tr>
             <tr id="StartDate" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblStartDate"></asp:Label>
                    </td>
                    <td>
                        <amc:DateTime runat="server" ID="txtStartDate" ValidationGroup="PopupRequiredGroup" CssClass="fl"></amc:DateTime>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqStartDate" ControlToValidate="txtStartDate$txtDatetime"
                            EnableClientScript="True" CssClass="fl" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div class="cb">
                            <asp:CustomValidator runat="server" ID="CustomValidatorStartDate" ControlToValidate="txtStartDate$txtDatetime"
                                ClientValidationFunction="ValidateReCertCircle" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                                resourcekey="InvalidTimeFrame.Text" EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
            <tr id="CompletionDate" runat="server">
                <td>
                    <asp:Label runat="server" ID="lblCompletionDate"></asp:Label>
                </td>
                <td>
                    <amc:DateTime runat="server" ID="txtCompletionDate" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl"></amc:DateTime>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCompletionDate" ControlToValidate="txtCompletionDate$txtDatetime"
                        CssClass="fl" EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div class="cb">
                    <asp:CustomValidator runat="server" ID="cusValidOtherBoard" ControlToValidate="txtCompletionDate$txtDatetime" 
                        ClientValidationFunction="ValidateReCertCircle"  Display="Dynamic" ValidationGroup="PopupRequiredGroup" resourcekey="InvalidTimeFrame.Text"
                        EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                        </div>
                    <asp:CompareValidator ID="Compare3" ControlToValidate="txtCompletionDate$txtDatetime" EnableClientScript="True"
                        ControlToCompare="txtStartDate$txtDatetime" resourcekey="CompareDateEducationCourse.Text"
                        Type="Date" runat="server" Display="Dynamic" Operator="GreaterThanEqual" ValidationGroup="PopupRequiredGroup" />
                </td>
            </tr>
             <tr id="Degree" runat="server">
                <td>
                    <asp:Label runat="server" ID="lblDegree"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDegree" CssClass="dn"></asp:DropDownList>
                    <asp:ListBox runat="server" ID="lstbDegree" SelectionMode="Single" ></asp:ListBox>
                </td>
            </tr>
            <tr id="OrganizationProvidingCourse" runat="server">
                <td>
                    <asp:Label runat="server" ID="lblOrganizationProvidingCourse"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250" ID="txtOrganizationProvidingCourse"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqOrganizationProvidingCourse" ControlToValidate="txtOrganizationProvidingCourse"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="OrganizationApprovingTheCourse" runat="server">
                <td>
                    <asp:Label runat="server" ID="lblOrganizationApprovingTheCourse"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250" ID="txtOrganizationApprovingTheCourse"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqOrganizationApprovingTheCourse"
                        ControlToValidate="txtOrganizationApprovingTheCourse" EnableClientScript="True"
                        Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="Point" runat="server">
                <td>
                    <asp:Label runat="server" ID="lblPoint"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250" ID="txtApprovedCE"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPoint" ControlToValidate="txtApprovedCE"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    <div>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtApprovedCE"
                            resourcekey="NumberValues.Text" ValidationExpression="^[0-9]{1,3}(\.[0-9]{1,9})?$"
                            Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>
        </table>
        <div class="pad5">
            <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            <br />
        </div>
        </asp:Panel>
    </div>
</div>
<div class="page-break"></div>
