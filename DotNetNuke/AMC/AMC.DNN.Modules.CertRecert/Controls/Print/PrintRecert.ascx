<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PrintRecert.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Print.PrintRecert" %>
<%@ Register TagPrefix="amc" TagName="Supervisor" Src="../../Controls/Common/SupervisorUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="Education" Src="../../Controls/Common/EducationCourseUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ControlSubstanceAuthorization" Src="../../Controls/Common/ControlSubstanceAuthorizationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="BoardCertification" Src="../../Controls/Common/BoardCertificationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="PracticeExperienceDetails" Src="../../Controls/Common/PracticeExperienceDetailsUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ReferenceAndVerification" Src="../../Controls/Common/ReferenceAndVerificationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="StandardQuestionaire" Src="../../Controls/Common/StandardQuestionaireUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="Licensure" Src="../../Controls/Common/Licensure.ascx" %>
<%@ Register TagPrefix="amc" TagName="RecertEligibility" Src="../../Controls/Common/RecertEligibilityUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ProgramProjectActivities" Src="../../Controls/Common/ProgramProjectActivities.ascx" %>
<%@ Register TagPrefix="amc" TagName="Research" Src="../../Controls/Common/Research.ascx" %>
<%@ Register TagPrefix="amc" TagName="OrganizationInvolvement" Src="../../Controls/Common/OrganizationInvolvement.ascx" %>
<%@ Register TagPrefix="amc" TagName="Summary" Src="../../Controls/Common/Summary.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServicePresentation" Src="../../Controls/Common/CommunityServicePresentation.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServicePubication" Src="../../Controls/Common/CommunityServicePubication.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServiceReview" Src="../../Controls/Common/CommunityServiceReview.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServiceVolunteerLeaderShip" Src="../../Controls/Common/CommunityServiceVolunteerLeaderShip.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServiceVolunteerService" Src="../../Controls/Common/CommunityServiceVolunteerService.ascx" %>
<%@ Register TagPrefix="amc" TagName="RecertificationOption" Src="../../Controls/Common/RecertificationOptionUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ContinuingEducation" Src="../../Controls/Common/ContinuingEducation.ascx" %>
<%@ Register TagPrefix="amc" TagName="CategoryCertifiedCME" Src="../../Controls/Common/CategoryCertifiedCME.ascx" %>
<%@ Register TagPrefix="amc" TagName="Publication" Src="../../Controls/Common/Publication.ascx" %>
<%@ Register TagPrefix="amc" TagName="TeachingPresentation" Src="../../Controls/Common/TeachingPresentation.ascx" %>
<%@ Register TagPrefix="amc" TagName="RegistrationUC" Src="../../Controls/Common/RegistrationUC.ascx" %>
<script type="text/javascript">
    var isPrint = true;
    var IsByPassPaymentProcess = '<%= IsByPassPaymentProcess %>';
</script>
<div class="amc-main" id="print-recertification-uc">
    <asp:HiddenField runat="server" ID="hdCurrentReCertOptionCode" />
    <div class="amc-main-title">
        <asp:Label runat="server" ID="lblReCertUC"></asp:Label>
    </div>
    <div class="amc-main-content">
        <div id="printContainer" runat="server">
            <amc:RegistrationUC runat="server" ID="sectionRegistrationUC"></amc:RegistrationUC>
            <amc:RecertificationOption runat="server" ID="sectionRecertificationOptionUC"></amc:RecertificationOption>
            <amc:Supervisor runat="server" ID="sectionSupervisorUC"></amc:Supervisor>
            <amc:ReferenceAndVerification runat="server" ID="sectionReferenceAndVerificationUC">
            </amc:ReferenceAndVerification>
            <amc:Licensure runat="server" ID="sectionLicensure"></amc:Licensure>
            <amc:ControlSubstanceAuthorization runat="server" ID="sectionControlSubstanceAuthorizationUC">
            </amc:ControlSubstanceAuthorization>
            <amc:BoardCertification runat="server" ID="sectionBoardCertificationUC"></amc:BoardCertification>
            <amc:StandardQuestionaire runat="server" ID="sectionStandardQuestionaireUC"></amc:StandardQuestionaire>
            <amc:recerteligibility runat="server" id="sectionRecertEligibilityUC">
            </amc:recerteligibility>
            <amc:Education runat="server" ID="sectionEducationCourseUC"></amc:Education>
            <amc:CategoryCertifiedCME runat="server" ID="sectionCategoryCertifiedCME"></amc:CategoryCertifiedCME>
            <amc:CommunityServicePresentation runat="server" ID="sectionCommunityServicePresentation">
            </amc:CommunityServicePresentation>
            <amc:CommunityServicePubication runat="server" ID="sectionCommunityServicePubication">
            </amc:CommunityServicePubication>
            <amc:CommunityServiceReview runat="server" ID="sectionCommunityServiceReview"></amc:CommunityServiceReview>
            <amc:CommunityServiceVolunteerLeaderShip runat="server" ID="sectionCommunityServiceVolunteerLeaderShip">
            </amc:CommunityServiceVolunteerLeaderShip>
            <amc:CommunityServiceVolunteerService runat="server" ID="sectionCommunityServiceVolunteerService">
            </amc:CommunityServiceVolunteerService>
            <amc:practiceexperiencedetails runat="server" id="sectionPracticeExperienceDetailsUC">
            </amc:practiceexperiencedetails>
            <amc:ProgramProjectActivities runat="server" ID="sectionProgramProjectActivities">
            </amc:ProgramProjectActivities>
            <amc:Research runat="server" ID="sectionResearch"></amc:Research>
            <amc:OrganizationInvolvement runat="server" ID="sectionOrganizationInvolvement">
            </amc:OrganizationInvolvement>
            <amc:ContinuingEducation runat="server" ID="sectionContinuingEducation"></amc:ContinuingEducation>
            <amc:Publication runat="server" ID="sectionPublication"></amc:Publication>
            <amc:TeachingPresentation runat="server" ID="sectionTeachingPresentation"></amc:TeachingPresentation>
            <amc:Summary runat="server" ID="sectionSummary"></amc:Summary>
        </div>
    </div>
</div>
