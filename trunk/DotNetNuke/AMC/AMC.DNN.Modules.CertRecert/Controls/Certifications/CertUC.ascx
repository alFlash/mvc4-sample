<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CertUC.ascx.vb" Inherits="AMC.DNN.Modules.CertRecert.Controls.Certifications.CertUC" %>
<%@ Register TagPrefix="amc" TagName="Supervisor" Src="../Common/SupervisorUC.ascx" %><%@ Register TagPrefix="amc" TagName="Registration" Src="../Common/RegistrationUC.ascx" %><%@ Register TagPrefix="amc" TagName="ExamChoice" Src="../Common/ExamChoiceUC.ascx" %><%@ Register TagPrefix="amc" TagName="Education" Src="../Common/EducationUC.ascx" %><%@ Register TagPrefix="amc" TagName="ControlSubstanceAuthorization" Src="../Common/ControlSubstanceAuthorizationUC.ascx" %><%@ Register TagPrefix="amc" TagName="BoardCertification" Src="../Common/BoardCertificationUC.ascx" %><%@ Register TagPrefix="amc" TagName="PracticeExperienceDetails" Src="../Common/PracticeExperienceDetailsUC.ascx" %><%@ Register TagPrefix="amc" TagName="ReferenceAndVerification" Src="../Common/ReferenceAndVerificationUC.ascx" %><%@ Register TagPrefix="amc" TagName="StandardQuestionaire" Src="../Common/StandardQuestionaireUC.ascx" %><%@ Register TagPrefix="amc" TagName="Licensure" Src="../Common/Licensure.ascx" %><%@ Register TagPrefix="amc" TagName="ProfessionalPracticeSetting" Src="../Common/ProfessionalPracticeSetting.ascx" %><%@ Register TagPrefix="amc" TagName="ProfessionalPracticeQuestionnaire" Src="../Common/ProfessionalPracticeQuestionnaire.ascx" %><%@ Register TagPrefix="amc" TagName="Scope" Src="../Common/ScopeUC.ascx" %><%@ Register TagPrefix="amc" TagName="Documentation" Src="../Common/DocumentationUC.ascx" %><%@ Register TagPrefix="amc" TagName="ProfessionalPracticeExperience" Src="../Common/ProfessionalPracticeExperienceUC.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2009.1.402.20, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register TagPrefix="amc" TagName="CategoryCertifiedCME" Src="../Common/CategoryCertifiedCME.ascx" %><%@ Register TagPrefix="amc" TagName="RecertEligibility" Src="../Common/RecertEligibilityUC.ascx" %>
<script type="text/javascript">
    var CertificationStepMenuClientId = '<%= rtsStepMenu.ClientID %>';
    var StepCompletedList = '<%= StepCompletedList %>';
    var printPageUrl = '<%= PrintURL %>';

    function OpenPrintPage(url) {
        var newwindow = window.open(url, 'Certification', 'toolbar=yes,scrollbars=yes,,menubar=yes,location=no,resizable=1');
        if (window.focus) { newwindow.focus(); }
        return false;
    }
    function RadTabLoad(sender, args) {
        //fix bugs: don't show overall selected tab
        var selectedTab = sender.get_selectedTab();
        var tabWidth = selectedTab.get_element().offsetWidth;
        var tabRight = jQuery(selectedTab.get_element()).offset().left + tabWidth;
        var arrows = $telerik.getChildrenByTagName(sender.get_levelElement(), "a");
        if (sender._scroller != null) {
            if (arrows[0].offsetLeft < tabRight) {
                sender._scroller._scrollTo(parseInt(sender._scroller._currentPosition + (tabRight - arrows[0].offsetLeft) + arrows[0].offsetWidth));
                var lastTab = sender.get_tabs().getTab(sender.get_tabs().get_count() - 1);
                var lastTabWidth = lastTab.get_element().offsetWidth;
                var lastTabLeft = jQuery(lastTab.get_element()).offset().left + lastTabWidth;
                if (lastTabLeft < jQuery(arrows[0]).offset().left) {
                    sender._scroller._scrollTo(parseInt(sender._scroller._currentPosition - (jQuery(arrows[0]).offset().left - lastTabLeft)));
                }
            }
        }
    }



    var PaymentProcessed = '<%= PaymentProcessed %>';
</script>
<div class="amc-main" id="certification-uc">
    <asp:HiddenField runat="server" ID="hdCurrentSectionPopupOpenningId" />
    <asp:HiddenField runat="server" ID="hdCurrentSectionPopupOpenningTitle" />
    <asp:HiddenField runat="server" ID="hdSaveSuccessful" />
    <asp:HiddenField runat="server" ID="hdStepCompletedList" />
    <div class="amc-main-title">
        <asp:Label runat="server" ID="lblCertUC"></asp:Label>
    </div>
    <telerik:RadTabStrip runat="server" ID="rtsStepMenu" Width="910px" ScrollChildren="true"
        SelectedIndex="0" CausesValidation="False" PerTabScrolling="False" OnClientLoad="RadTabLoad">
    </telerik:RadTabStrip>
    <div class="amc-main-content">
        <div class="amc-disabled-container">
        </div>
        <div class="amc-error-message">
            <asp:Label runat="server" ID="lblMessage"></asp:Label>
            <div>
                <asp:ValidationSummary runat="server" ID="vldsGeneral" ValidationGroup="AmcGeneralGroup" />
            </div>
        </div>

        <asp:MultiView runat="server" ID="mvCertification" EnableViewState="True">
            <asp:View runat="server" ID="RegistrationUC">
                <amc:Registration runat="server" ID="sectionRegistrationUC" FormId="CertUC" SectionId="RegistrationUC">
                </amc:Registration>
            </asp:View>
            <asp:View runat="server" ID="SupervisorUC">
                <amc:Supervisor runat="server" ID="sectionSupervisorUC" FormId="CertUC" SectionId="SupervisorUC">
                </amc:Supervisor>
            </asp:View>
            <asp:View runat="server" ID="ExamChoiceUC">
                <amc:ExamChoice runat="server" ID="sectionExamChoiceUC" FormId="CertUC" SectionId="ExamChoiceUC">
                </amc:ExamChoice>
            </asp:View>
            <asp:View runat="server" ID="RecertEligibilityUC">
                <amc:RecertEligibility runat="server" ID="sectionRecertEligibilityUC" FormId="CertUC"
                    SectionId="RecertEligibilityUC">
                </amc:RecertEligibility>
            </asp:View>
            <asp:View runat="server" ID="EducationUC">
                <amc:Education runat="server" ID="sectionEducationUC" FormId="CertUC" SectionId="EducationUC">
                </amc:Education>
            </asp:View>
            <asp:View runat="server" ID="CategoryCertifiedCME">
                <amc:CategoryCertifiedCME runat="server" ID="sectionCategoryCertifiedCME"></amc:CategoryCertifiedCME>
            </asp:View>
            <asp:View runat="server" ID="Licensure">
                <amc:Licensure runat="server" ID="sectionLicensure" FormId="CertUC" SectionId="Licensure">
                </amc:Licensure>
            </asp:View>
            <asp:View runat="server" ID="ControlSubstanceAuthorizationUC">
                <amc:ControlSubstanceAuthorization runat="server" ID="sectionControlSubstanceAuthorizationUC"
                    FormId="CertUC" SectionId="ControlSubstanceAuthorizationUC"></amc:ControlSubstanceAuthorization>
            </asp:View>
            <asp:View runat="server" ID="BoardCertificationUC">
                <amc:BoardCertification runat="server" ID="sectionBoardCertificationUC" FormId="CertUC"
                    SectionId="BoardCertificationUC"></amc:BoardCertification>
            </asp:View>
            <asp:View runat="server" ID="PracticeExperienceDetailsUC">
                <amc:PracticeExperienceDetails runat="server" ID="sectionPracticeExperienceDetailsUC"
                    FormId="CertUC" SectionId="PracticeExperienceDetailsUC">
                </amc:PracticeExperienceDetails>
            </asp:View>
            <asp:View runat="server" ID="ReferenceAndVerificationUC">
                <amc:ReferenceAndVerification runat="server" ID="sectionReferenceAndVerificationUC"
                    FormId="CertUC" SectionId="ReferenceAndVerificationUC"></amc:ReferenceAndVerification>
            </asp:View>
            <asp:View runat="server" ID="StandardQuestionaireUC">
                <amc:StandardQuestionaire runat="server" ID="sectionStandardQuestionaireUC" FormId="CertUC"
                    SectionId="StandardQuestionaireUC"></amc:StandardQuestionaire>
            </asp:View>
            <asp:View runat="server" ID="ProfessionalPracticeQuestionnaire">
                <amc:ProfessionalPracticeQuestionnaire runat="server" ID="sectionProfessionalPracticeQuestionnaire"
                    FormId="CertUC" SectionId="ProfessionalPracticeQuestionnaire"></amc:ProfessionalPracticeQuestionnaire>
            </asp:View>
            <asp:View runat="server" ID="ProfessionalPracticeSetting">
                <amc:ProfessionalPracticeSetting runat="server" ID="sectionProfessionalPracticeSetting"
                    FormId="CertUC" SectionId="ProfessionalPracticeSetting"></amc:ProfessionalPracticeSetting>
            </asp:View>
            <asp:View runat="server" ID="ScopeUC">
                <amc:Scope runat="server" ID="sectionScopeUC" FormId="CertUC" SectionId="ScopeUC">
                </amc:Scope>
            </asp:View>
            <asp:View runat="server" ID="DocumentationUC">
                <amc:Documentation runat="server" ID="sectionDocumentationUC" FormId="CertUC" SectionId="DocumentationUC">
                </amc:Documentation>
            </asp:View>
            <asp:View runat="server" ID="ProfessionalPracticeExperienceUC">
                <amc:ProfessionalPracticeExperience runat="server" ID="sectionProfessionalPracticeExperienceUC"
                    FormId="CertUC" SectionId="ProfessionalPracticeExperienceUC"></amc:ProfessionalPracticeExperience>
            </asp:View>
        </asp:MultiView>
    </div>
    <div class="amc-main-footer">
        <asp:Button runat="server" ID="btnBack" resourcekey="Back.Text" Visible="False" CausesValidation="False" />
        <asp:Button runat="server" ID="btbSave" resourcekey="Save.Text" ValidationGroup="AmcGeneralGroup" />
        <asp:Button runat="server" ID="btnNext" resourcekey="Next.Text" ValidationGroup="AmcGeneralGroup" />
        <asp:Button runat="server" ID="btnSubmit" resourcekey="Submit.Text" Enabled="False"
            ValidationGroup="AmcGeneralGroup" />
        <asp:Button runat="server" ID="btnPrint" resourcekey="Print.Text" OnClientClick="OpenPrintPage(printPageUrl);return false;"
            Enabled="False" ValidationGroup="AmcGeneralGroup" />
        <asp:Button runat="server" ID="btnCancel" resourcekey="Cancel.Text" CausesValidation="False" />
    </div>
</div>
