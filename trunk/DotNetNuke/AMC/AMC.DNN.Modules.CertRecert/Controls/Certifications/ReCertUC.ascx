<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ReCertUC.ascx.vb" Inherits="AMC.DNN.Modules.CertRecert.Controls.Certifications.ReCertUC" %>
<%@ Register TagPrefix="amc" TagName="Supervisor" Src="../Common/SupervisorUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="Education" Src="../Common/EducationCourseUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ControlSubstanceAuthorization" Src="../Common/ControlSubstanceAuthorizationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="BoardCertification" Src="../Common/BoardCertificationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="PracticeExperienceDetails" Src="../Common/PracticeExperienceDetailsUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ReferenceAndVerification" Src="../Common/ReferenceAndVerificationUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="StandardQuestionaire" Src="../Common/StandardQuestionaireUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="Licensure" Src="../Common/Licensure.ascx" %>
<%@ Register TagPrefix="amc" TagName="RecertEligibility" Src="../Common/RecertEligibilityUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ProgramProjectActivities" Src="../Common/ProgramProjectActivities.ascx" %>
<%@ Register TagPrefix="amc" TagName="Research" Src="../Common/Research.ascx" %>
<%@ Register TagPrefix="amc" TagName="OrganizationInvolvement" Src="../Common/OrganizationInvolvement.ascx" %>
<%@ Register TagPrefix="amc" TagName="Summary" Src="../Common/Summary.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServicePresentation" Src="../Common/CommunityServicePresentation.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServicePubication" Src="../Common/CommunityServicePubication.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServiceReview" Src="../Common/CommunityServiceReview.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServiceVolunteerLeaderShip" Src="../Common/CommunityServiceVolunteerLeaderShip.ascx" %>
<%@ Register TagPrefix="amc" TagName="CommunityServiceVolunteerService" Src="../Common/CommunityServiceVolunteerService.ascx" %>
<%@ Register TagPrefix="amc" TagName="RecertificationOption" Src="../Common/RecertificationOptionUC.ascx" %>
<%@ Register TagPrefix="amc" TagName="ContinuingEducation" Src="../Common/ContinuingEducation.ascx" %>
<%@ Register TagPrefix="amc" TagName="CategoryCertifiedCME" Src="../Common/CategoryCertifiedCME.ascx" %>
<%@ Register TagPrefix="amc" TagName="Publication" Src="../Common/Publication.ascx" %>
<%@ Register TagPrefix="amc" TagName="TeachingPresentation" Src="../Common/TeachingPresentation.ascx" %>
<%@ Register TagPrefix="amc" TagName="RegistrationUC" Src="../Common/RegistrationUC.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2009.1.402.20, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register TagPrefix="amc" TagName="ProfessionalPracticeSetting" Src="../Common/ProfessionalPracticeSetting.ascx" %>
<script type="text/javascript">
    var ReCertificationStepMenuClientId = '<%= rtsStepMenu.ClientID %>';
    var StepCompletedList = '<%= StepCompletedList %>';
    var printPageUrl = '<%= PrintURL %>';
    var PaymentProcessed = '<%= PaymentProcessed %>';
    var RecertificationCircleJson = IsStringNullOrEmpty('<%= RecertificationCircleJson %>') ? null : eval('(' + '<%= RecertificationCircleJson %>' + ')');
    //fix bug: rad tab don't show the lastest tab
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
	
		
        jQuery("#[id*=rtsStepMenu]").delegate("a", "click", function (ev) {
            ev.stopPropagation();
            var lastElement = jQuery('#[id*=rtsStepMenu] .rtsLast');
            var left = lastElement.position().left;
            var widthLimit = jQuery('#[id*=rtsStepMenu]').width() - (lastElement.width() + 20);
            var nextButton = jQuery('#[id*=rtsStepMenu] a.rtsNextArrowDisabled');
            if (left >= widthLimit && nextButton.length > 0) {
                nextButton.removeClass('rtsNextArrowDisabled');
                nextButton.addClass('rtsNextArrow');
                sender.repaint();
                if (!$telerik.isIE) {
                    sender._scroller.repaint();
                } else {
                    var scrool = jQuery('.rtsScroll');
                    var valMargin = scrool.css('margin-left').replace('px', '');
                    var newMargin = parseInt(valMargin) - (36);
                    scrool.css('margin-left', newMargin);
                }
            }
        });
    }

    function OpenPrintPage(url) {
        var newwindow = window.open(url, 'Certification', 'toolbar=yes,scrollbars=yes,menubar=yes,location=no,resizable=1');
        if (window.focus) { newwindow.focus(); }
        return false;
    }
    var DateTimeFormat = '<%= DateTimeFormat %>';
</script>
<div class="amc-main" id="recertification-uc">
    <div class="amc-main-title">
        <asp:Label runat="server" ID="lblReCertUC"></asp:Label>
    </div>
    <telerik:RadTabStrip runat="server" ID="rtsStepMenu" Width="910px" ScrollChildren="true"
        SelectedIndex="0" ValidationGroup="RadTabReCertValidation" OnClientLoad="RadTabLoad" CausesValidation="False" >
    </telerik:RadTabStrip>
    <%--<script type="text/javascript">
            function repositionSrollArrows(sender) {
                var arrows = $telerik.getChildrenByTagName(sender.get_levelElement(), "a");
                sender.get_element().appendChild(arrows[0]);
                sender.get_element().appendChild(arrows[1]);
                arrows[0].style.position = "static";
                arrows[0].style.marginTop = "-18px";
                arrows[1].style.marginTop = "-18px";
                arrows[1].style.position = "static";
                sender.get_levelElement().style.width = sender.get_levelElement().offsetWidth - arrows[0].offsetWidth + "px";
            } 
            </script>--%>
    <div class="amc-main-content">
        <div class="amc-disabled-container"></div>
        <div class="amc-error-message">
            <asp:Label runat="server" ID="lblMessage"></asp:Label>
            <div>
                <asp:ValidationSummary runat="server" ID="vldsGeneral" ValidationGroup="AmcGeneralGroup"/>
            </div>
        </div>
        <asp:MultiView runat="server" ID="mvCertification" EnableViewState="True">
            <asp:View runat="server" ID="RecertificationOptionUC">
                <amc:RecertificationOption runat="server" ID="sectionRecertificationOptionUC"></amc:RecertificationOption>
            </asp:View>
            <asp:View runat="server" ID="SupervisorUC">
                <amc:Supervisor runat="server" ID="sectionSupervisorUC"></amc:Supervisor>
            </asp:View>
            <asp:View runat="server" ID="ReferenceAndVerificationUC">
                <amc:ReferenceAndVerification runat="server" ID="sectionReferenceAndVerificationUC">
                </amc:ReferenceAndVerification>
            </asp:View>
            <asp:View runat="server" ID="Licensure">
                <amc:Licensure runat="server" ID="sectionLicensure"></amc:Licensure>
            </asp:View>
            <asp:View runat="server" ID="ControlSubstanceAuthorizationUC">
                <amc:ControlSubstanceAuthorization runat="server" ID="sectionControlSubstanceAuthorizationUC">
              
                </amc:ControlSubstanceAuthorization>
            </asp:View>
            <asp:View runat="server" ID="BoardCertificationUC">
                <amc:BoardCertification runat="server" ID="sectionBoardCertificationUC"></amc:BoardCertification>
            </asp:View>
            <asp:View runat="server" ID="StandardQuestionaireUC">
                <amc:StandardQuestionaire runat="server" ID="sectionStandardQuestionaireUC"></amc:StandardQuestionaire>
            </asp:View>
            <asp:View runat="server" ID="RecertEligibilityUC">
                <amc:recerteligibility runat="server" id="sectionRecertEligibilityUC">
                </amc:recerteligibility>
            </asp:View>
            <asp:View runat="server" ID="EducationCourseUC">
                <amc:Education runat="server" ID="sectionEducationCourseUC"></amc:Education>
            </asp:View>
            <asp:View runat="server" ID="CategoryCertifiedCME">
                <amc:CategoryCertifiedCME runat="server" ID="sectionCategoryCertifiedCME"></amc:CategoryCertifiedCME>
            </asp:View>
            <asp:View runat="server" ID="ProfessionalPracticeSetting">
                <amc:ProfessionalPracticeSetting runat="server" ID="sectionProfessionalPracticeSetting"
                    FormId="CertUC" SectionId="ProfessionalPracticeSetting"></amc:ProfessionalPracticeSetting>
            </asp:View>
            <asp:View runat="server" ID="CommunityServicePresentation">
                <amc:CommunityServicePresentation runat="server" ID="sectionCommunityServicePresentation">
              
                </amc:CommunityServicePresentation>
            </asp:View>
            <asp:View runat="server" ID="CommunityServicePubication">
                <amc:CommunityServicePubication runat="server" ID="sectionCommunityServicePubication">
                
                </amc:CommunityServicePubication>
            </asp:View>
            <asp:View runat="server" ID="CommunityServiceReview">
                <amc:CommunityServiceReview runat="server" ID="sectionCommunityServiceReview"></amc:CommunityServiceReview>
            </asp:View>
            <asp:View runat="server" ID="CommunityServiceVolunteerLeaderShip">
                <amc:CommunityServiceVolunteerLeaderShip runat="server" ID="sectionCommunityServiceVolunteerLeaderShip">
         
                </amc:CommunityServiceVolunteerLeaderShip>
            </asp:View>
            <asp:View runat="server" ID="CommunityServiceVolunteerService">
                <amc:CommunityServiceVolunteerService runat="server" ID="sectionCommunityServiceVolunteerService">
                
                </amc:CommunityServiceVolunteerService>
            </asp:View>
            <asp:View runat="server" ID="PracticeExperienceDetailsUC">
                <amc:practiceexperiencedetails runat="server" id="sectionPracticeExperienceDetailsUC">
                </amc:practiceexperiencedetails>
            </asp:View>
            <asp:View runat="server" ID="ProgramProjectActivities">
                <amc:ProgramProjectActivities runat="server" ID="sectionProgramProjectActivities">
             
                </amc:ProgramProjectActivities>
            </asp:View>
            <asp:View runat="server" ID="Research">
                <amc:Research runat="server" ID="sectionResearch"></amc:Research>
            </asp:View>
            <asp:View runat="server" ID="OrganizationInvolvement">
                <amc:OrganizationInvolvement runat="server" ID="sectionOrganizationInvolvement">
             
                </amc:OrganizationInvolvement>
            </asp:View>
            <asp:View runat="server" ID="ContinuingEducation">
                <amc:ContinuingEducation runat="server" ID="sectionContinuingEducation"></amc:ContinuingEducation>
            </asp:View>
            <asp:View runat="server" ID="Publication">
                <amc:Publication runat="server" ID="sectionPublication"></amc:Publication>
            </asp:View>
            <asp:View runat="server" ID="TeachingPresentation">
                <amc:TeachingPresentation runat="server" ID="sectionTeachingPresentation"></amc:TeachingPresentation>
            </asp:View>
            <asp:View runat="server" ID="Summary">
                <amc:Summary runat="server" ID="sectionSummary"></amc:Summary>
            </asp:View>
            <asp:View runat="server" ID="RegistrationUC">
                <amc:RegistrationUC runat="server" ID="sectionRegistrationUC"></amc:RegistrationUC>
            </asp:View>
        </asp:MultiView>
    </div>
    <div class="amc-main-footer">
        <asp:Button runat="server" ID="btnBack" resourcekey="Back.Text" Visible="False" CausesValidation="False"/>
        <asp:Button runat="server" ID="btnSave" resourcekey="Save.Text" ValidationGroup="AmcGeneralGroup" />
        <asp:Button runat="server" ID="btnNext" resourcekey="Next.Text" ValidationGroup="AmcGeneralGroup" />
        <asp:Button runat="server" ID="btnSubmit" resourcekey="Submit.Text" Enabled="False" ValidationGroup="AmcGeneralGroup" />
        <asp:Button runat="server" ID="btnPrint" resourcekey="Print.Text" OnClientClick="OpenPrintPage(printPageUrl);return false;"
            Enabled="False"  ValidationGroup="AmcGeneralGroup"/>
        <asp:Button runat="server" ID="btnCancel" resourcekey="Cancel.Text" CausesValidation="False" />
    </div>
    <asp:HiddenField runat="server" ID="hdCurrentReCertOptionCode" />
    <asp:HiddenField runat="server" ID="hdCurrentSectionPopupOpenningId" />
    <asp:HiddenField runat="server" ID="hdCurrentSectionPopupOpenningTitle" />
    <asp:HiddenField runat="server" ID="hdSaveSuccessful" />
    <asp:HiddenField runat="server" ID="hdStepCompletedList" />
    <asp:HiddenField runat="server" ID="hdAllTabCompleted"/>
</div>
