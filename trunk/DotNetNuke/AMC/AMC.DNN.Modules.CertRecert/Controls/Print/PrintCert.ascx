<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PrintCert.ascx.vb"
	Inherits="AMC.DNN.Modules.CertRecert.Controls.Print.PrintCert" %>
<%@ Register TagPrefix="amc" TagName="Supervisor" Src="../../Controls/Common/SupervisorUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="Registration" Src="../../Controls/Common/RegistrationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ExamChoice" Src="../../Controls/Common/ExamChoiceUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="Education" Src="../../Controls/Common/EducationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ControlSubstanceAuthorization" Src="../../Controls/Common/ControlSubstanceAuthorizationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="BoardCertification" Src="../../Controls/Common/BoardCertificationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="PracticeExperienceDetails" Src="../../Controls/Common/PracticeExperienceDetailsUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ReferenceAndVerification" Src="../../Controls/Common/ReferenceAndVerificationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="StandardQuestionaire" Src="../../Controls/Common/StandardQuestionaireUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="Licensure" Src="../../Controls/Common/Licensure.ascx" %>
<%@ Register TagPrefix="amc" TagName="ProfessionalPracticeSetting" Src="../../Controls/Common/ProfessionalPracticeSetting.ascx" %>
<%@ Register TagPrefix="amc" TagName="ProfessionalPracticeQuestionnaire" Src="../../Controls/Common/ProfessionalPracticeQuestionnaire.ascx" %>
<%@ Register TagPrefix="amc" TagName="Scope" Src="../../Controls/Common/ScopeUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="Documentation" Src="../../Controls/Common/DocumentationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ProfessionalPracticeExperience" Src="../../Controls/Common/ProfessionalPracticeExperienceUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="RecertEligibility" Src="../../Controls/Common/RecertEligibilityUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="CategoryCertifiedCME" Src="../../Controls/Common/CategoryCertifiedCME.ascx" %>
<script type="text/javascript">
	var isPrint = true;
</script>
<div class="amc-main" id="print-certification-uc">
	<div class="amc-main-title">
		
	</div>
	<div class="amc-main-content">
		<div id="printContainer" runat="server">
			<amc:Registration runat="server" ID="sectionRegistrationUC"></amc:Registration>
			<amc:Supervisor runat="server" ID="sectionSupervisorUC"></amc:Supervisor>
			<amc:ExamChoice runat="server" ID="sectionExamChoiceUC"></amc:ExamChoice>
			<amc:RecertEligibility runat="server" ID="sectionRecertEligibilityUC">
			</amc:RecertEligibility>
			<amc:Education runat="server" ID="sectionEducationUC"></amc:Education>
			<amc:Licensure runat="server" ID="sectionLicensure"></amc:Licensure>
			<amc:ControlSubstanceAuthorization runat="server" ID="sectionControlSubstanceAuthorizationUC">
			</amc:ControlSubstanceAuthorization>
			<amc:CategoryCertifiedCME runat="server" ID="sectionCategoryCertifiedCME"></amc:CategoryCertifiedCME>
			<amc:BoardCertification runat="server" ID="sectionBoardCertificationUC"></amc:BoardCertification>
			<amc:PracticeExperienceDetails runat="server" ID="sectionPracticeExperienceDetailsUC">
			</amc:PracticeExperienceDetails>
			<amc:ReferenceAndVerification runat="server" ID="sectionReferenceAndVerificationUC">
			</amc:ReferenceAndVerification>
			<amc:StandardQuestionaire runat="server" ID="sectionStandardQuestionaireUC"></amc:StandardQuestionaire>
			<amc:ProfessionalPracticeQuestionnaire runat="server" ID="sectionProfessionalPracticeQuestionnaire">
			</amc:ProfessionalPracticeQuestionnaire>
			<amc:ProfessionalPracticeSetting runat="server" ID="sectionProfessionalPracticeSetting">
			</amc:ProfessionalPracticeSetting>
			<amc:Scope runat="server" ID="sectionScopeUC"></amc:Scope>
			<amc:Documentation runat="server" ID="sectionDocumentationUC"></amc:Documentation>
			<amc:ProfessionalPracticeExperience runat="server" ID="sectionProfessionalPracticeExperienceUC">
			</amc:ProfessionalPracticeExperience>
		</div>
	</div>
</div>
