<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BoardCertificationUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.BoardCertificationUC" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<!-- javascript -->
<script type="text/javascript">
    var BoardCertification_RecertificationDateClientID = '<%= rqRecertificateDate.ClientID %>';
    var BoardCertification_CompareEndateClientID = '<%= cpRecertificationDate.ClientID %>';
    var BoardCertification_RecertificationCircleID = '<%= CustomValidator2.ClientID %>';
    function ValidateEmptyCheckbox(oSrouce, args) {
        var myCheckBox = document.getElementById('<%= chkNoneRecertDate.ClientID %>');
        if (!myCheckBox.checked) {
            args.IsValid = false;
        }
        else {
            args.IsValid = true;
        }
    }

    //override validate recert circle date for board certification
    function BoardValidateReCertCircle(sender, args) {
        if (jQuery('#popupEditBoard #[id*=chkNoneRecertDate]').prop('checked') == true) {
            args.IsValid = true;
        } else {
            ValidateReCertCircle(sender, args);
        }

    }
    //validate txtBoardCertification in case of choosing other board
    function ValidOtherBoard(sender, args) {
        args.IsValid = false;
        var boardChosenValue = jQuery('#popupEditBoard #[id*=ddlBoardCertificate]').val();
        if (boardChosenValue != 'OTHER' && boardChosenValue != 'ABMSSUBSPEC') {
            args.IsValid = true;
        } else {
            if (IsStringNullOrEmpty(jQuery('#popupEditBoard #[id*=txtBoardCertification]').val())) {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }
    }
    function attachHandleForDdlBoardCertification() {
        var boardChosenValue = jQuery('#popupEditBoard #[id*=ddlBoardCertificate]').val();
        jQuery('#popupEditBoard #[id*=txtBoardCertification]').parent("div").hide();
        jQuery('#popupEditBoard #[id*=txtBoardCertification]').hide();
        if (boardChosenValue == 'OTHER' || boardChosenValue == 'ABMSSUBSPEC') {
            jQuery('#popupEditBoard #[id*=txtBoardCertification]').parent("div").show();
            jQuery('#popupEditBoard #[id*=txtBoardCertification]').show();
        }
        jQuery('#popupEditBoard').delegate('[id*=ddlBoardCertificate]', 'click',
            function (ev) {
                var valueChange = jQuery('#popupEditBoard #[id*=ddlBoardCertificate]').val();
                var txtBoardCertification =
                    jQuery('#popupEditBoard #[id*=txtBoardCertification]');
                if (valueChange == 'OTHER' || valueChange == 'ABMSSUBSPEC') {
                    if (AMCTablePopUp.CurrentRow != null) {
                        var otherBoardValue = jQuery(AMCTablePopUp.CurrentRow).find('#[id*=lblBoardCertification]');
                        if (otherBoardValue.length > 0) {
                            txtBoardCertification.val(otherBoardValue.text());
                        }
                    }
                    txtBoardCertification.parent("div").show(300);
                    txtBoardCertification.show();
                } else {
                    txtBoardCertification.parent("div").hide(300);
                    
                }
                ev.stopPropagation();
            });
    }
    function attachHandleForCheckBoxNoneRecertDate() {
        var myVal = document.getElementById(BoardCertification_RecertificationDateClientID);
        var compareEndDate = document.getElementById(BoardCertification_CompareEndateClientID);
        var recertifcationCirclevld = document.getElementById(BoardCertification_RecertificationCircleID);
        var trRecertDate = jQuery(jQuery('#popupEditBoard #[id*=RecertificateDate]')[0]);
        trRecertDate.show();
        jQuery('#popupEditBoard #[id*=chkNoneRecertDate]').prop('checked', false);
        if (myVal != null) {
            ValidatorEnable(myVal, true);
            jQuery(myVal).hide();
        }
        if (compareEndDate != null) {
            ValidatorEnable(compareEndDate, true);
            jQuery(compareEndDate).hide();
        }
        if (recertifcationCirclevld != null) {
            ValidatorEnable(recertifcationCirclevld, true);
            jQuery(recertifcationCirclevld).hide();
        }
        if (AMCTablePopUp.CurrentRow != null) {
            var hdNoneRecertDateCtl = jQuery(AMCTablePopUp.CurrentRow).find('#[id*=hdNoneRecertDate]');
            if (hdNoneRecertDateCtl.length > 0 && hdNoneRecertDateCtl.val() == 'True') {
                jQuery('#popupEditBoard #[id*=chkNoneRecertDate]').prop('checked', true);
                trRecertDate.hide();
                if (myVal != null) {
                    ValidatorEnable(myVal, false);
                    jQuery(myVal).hide();
                }
                if (compareEndDate != null) {
                    ValidatorEnable(compareEndDate, false);
                    jQuery(compareEndDate).hide();
                }
                if (recertifcationCirclevld != null) {
                    ValidatorEnable(recertifcationCirclevld, false);
                    jQuery(recertifcationCirclevld).hide();
                }
                jQuery('#popupEditBoard #[id*=txtRecertificateDate]').val('');
            }
        }
        jQuery('#popupEditBoard').delegate('[id*=chkNoneRecertDate]', 'click',
            function (ev) {
                if (jQuery('#popupEditBoard #[id*=chkNoneRecertDate]').prop('checked') == true) {
                    trRecertDate.hide(300);
                    if (myVal != null) {
                        ValidatorEnable(myVal, false);
                        jQuery(myVal).hide();
                    }
                    if (compareEndDate != null) {
                        ValidatorEnable(compareEndDate, false);
                        jQuery(compareEndDate).hide();
                    }
                    if (recertifcationCirclevld != null) {
                        ValidatorEnable(recertifcationCirclevld, false);
                        jQuery(recertifcationCirclevld).hide();
                    }
                } else {
                    jQuery('#popupEditBoard #[id*=txtRecertificateDate]').val('');
                    trRecertDate.show(200);
                    if (myVal != null) {
                        ValidatorEnable(myVal, true);
                        jQuery(myVal).hide();
                    }
                    if (compareEndDate != null) {
                        ValidatorEnable(compareEndDate, true);
                        jQuery(compareEndDate).hide();
                    }
                    if (recertifcationCirclevld != null) {
                        ValidatorEnable(recertifcationCirclevld, true);
                        jQuery(recertifcationCirclevld).hide();
                    }
                }
                ev.stopPropagation();
            });
    }


    jQuery(document).ready(function () {
        var boardPopupBindingList = [["lblBoardType", "ddlBoardCertificate"],
            ["lblCertificateNumber", "txtCertificateNumber"],
            ["lblCertificateDate", "txtCertificateDate"],
            ["lblBoardCertification", "txtBoardCertification"],
            ["lblRecertificateDate", "txtRecertificateDate"], ['hlAttachedDocumentName', 'fuUploadFileAttachment'],
            ['hlAttachedDocumentName', 'hlUploadFileAttachment']];
        var boardPopupControl = new AMCTablePopUp(boardPopupBindingList,
            "uc_board_certificate",
            "tbl-board",
            "popupEditBoard",
            "Add Information ",
            "Edit Item ", null);

        jQuery('#uc_board_certificate').delegate('#add-new-row', 'click', function (ev) {
            AMCTablePopUp.CurrentRow = null;
            attachHandleForDdlBoardCertification();
            attachHandleForCheckBoxNoneRecertDate();
            ev.stopPropagation();
        });
        jQuery('#uc_board_certificate').delegate('[id*=imgEdit]', 'click', function (ev) {
            attachHandleForDdlBoardCertification();
            attachHandleForCheckBoxNoneRecertDate();
            ev.stopPropagation();
        });

        jQuery('#uc_board_certificate').delegate('[id*=imgAddAttachment]', 'click', function (ev) {
            attachHandleForDdlBoardCertification();
            attachHandleForCheckBoxNoneRecertDate();
            ev.stopPropagation();
        });

    });
</script>
<div id="uc_board_certificate" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblBoardCertificationUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="Image1" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5"
                href="javascript:void(0);"></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater runat="server" ID="rptSubCertificate" EnableViewState="True">
                <HeaderTemplate>
                    <table border="1px" class="amc-table" id="tbl-board">
                        <tr class="amc-table-header">
                            <td runat="server" visible='<%# GetFieldInfo("Board").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblABMSBoardName" Text='<%# GetFieldInfo("Board").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("CertificateNumber").IsEnabled %>'>
                                <asp:Label runat="server" ID="Label1" Text='<%# GetFieldInfo("CertificateNumber").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("CertificateDate").IsEnabled %>'>
                                <asp:Label runat="server" ID="Label4" Text='<%# GetFieldInfo("CertificateDate").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("RecertificateDate").IsEnabled %>'>
                                <asp:Label runat="server" ID="Label2" Text='<%# GetFieldInfo("RecertificateDate").FieldValue %>'></asp:Label>
                            </td>
                            <td class="add-column" runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblAttachDocument" Text='<%# GetFieldInfo("UploadFileAttachment").FieldValue %>'></asp:Label>
                            </td>
                            <td class="action-column">
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="amc-table-content" id="item-content">
                        <td runat="server" visible='<%# GetFieldInfo("Board").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblBoard"></asp:Label>
                            <asp:Label runat="server" ID="lblBoardType" Text='<%# Eval("IssuingBodyString") %>'
                                CssClass="dn"></asp:Label>
                            <asp:Label runat="server" ID="lblBoardCertification" CssClass="dn" Text='<%# GetFieldInfo("IssuingBodyText").FieldValue %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("CertificateNumber").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblCertificateNumber" Text='<%# Eval("IssuedNumber") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("CertificateDate").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblCertificateDate"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("RecertificateDate").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblRecertificateDate"></asp:Label>
                            <asp:HiddenField runat="server" ID="hdNoneRecertDate" />
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'
                            class="add-column">
                            <asp:HiddenField runat="server" ID="hdAttachedDocumentName" />
                            <div class="add-image dn margin-auto" id="imgAddAttachment" runat="server">
                            </div>
                            <asp:HyperLink runat="server" ID="hlAttachedDocumentName" Target="_blank"></asp:HyperLink>
                        </td>
                        <td class="action-column">
                            <asp:HiddenField runat="server" ID="hdObjectUniqueId" Value='<%# Eval("Guid") %>' />
                            <asp:Image CssClass="pointer" runat="server" ID="imgEdit" ImageUrl="../../Documentation/images/icons/EditIcon.gif">
                            </asp:Image>
                            <asp:ImageButton CssClass="pointer" runat="server" ID="imgItemDelete" ImageUrl="../../Documentation/images/icons/delete_icon1.gif"
                                CommandName="Delete" CommandArgument='<%# Eval("Guid") %>' CausesValidation="False">
                            </asp:ImageButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div class="page-break">
</div>
<!-- popup -->
<div id="popupEditBoard" class="amc-popup">
    <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div>
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                <div>
                    <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                </div>
            </div>
            <table>
                <tr id="Board" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblBoard"></asp:Label>
                    </td>
                    <td>
                        <div>
                            <asp:DropDownList runat="server" ID="ddlBoardCertificate">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqBoard" ControlToValidate="ddlBoardCertificate"
                                EnableClientScript="True" ValidationGroup="PopupRequiredGroup" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div id="otherBoardPanel" style="margin-top: 3px" class="dn">
                            <asp:TextBox runat="server" ID="txtBoardCertification" CssClass="width250"></asp:TextBox>
                            <asp:CustomValidator runat="server" ID="cusValidOtherBoard" ControlToValidate="txtBoardCertification"
                                ClientValidationFunction="ValidOtherBoard" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                                ErrorMessage="*" EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
                <tr id="CertificateNumber" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblCertificateNumber"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCertificateNumber" CssClass="width250"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCertificateNumber"
                            ControlToValidate="txtCertificateNumber" EnableClientScript="True" ValidationGroup="PopupRequiredGroup"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="CertificateDate" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblCertificateDate"></asp:Label>
                    </td>
                    <td>
                        <amc:DateTime runat="server" ID="txtCertificateDate" ValidationGroup="PopupRequiredGroup"
                            CssClass="fl"></amc:DateTime>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCertificateDate"
                            ControlToValidate="txtCertificateDate$txtDatetime" EnableClientScript="True"
                            ValidationGroup="PopupRequiredGroup" Display="Dynamic" CssClass="fl"></asp:RequiredFieldValidator>
                        <%--                        <div class="cb">
                            <asp:CustomValidator runat="server" ID="CustomValidator1" ControlToValidate="txtCertificateDate$txtDatetime"
                                ClientValidationFunction="ValidateReCertCircle" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                                resourcekey="InvalidTimeFrame.Text" EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                        </div>--%>
                    </td>
                </tr>
                <tr id="NoneRecertDate" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblNoneRecertDate"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkNoneRecertDate" CssClass="fl" />
                        <%--                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqNoneRecertDate"
                            ControlToValidate="chkNoneRecertDate" EnableClientScript="True"
                            ValidationGroup="PopupRequiredGroup" Display="Dynamic" CssClass="fl"></asp:RequiredFieldValidator>--%>
                        <asp:CustomValidator runat="server" ID="rqNoneRecertDate" ClientValidationFunction="ValidateEmptyCheckbox"
                            Display="Dynamic" ValidationGroup="PopupRequiredGroup" ErrorMessage="*" EnableClientScript="True"
                            ValidateEmptyText="True" CssClass="fl"></asp:CustomValidator>
                    </td>
                </tr>
                <tr id="RecertificateDate" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblRecertificateDate"></asp:Label>
                    </td>
                    <td>
                        <amc:DateTime runat="server" ID="txtRecertificateDate" ValidationGroup="PopupRequiredGroup"
                            CssClass="fl"></amc:DateTime>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqRecertificateDate"
                            ControlToValidate="txtRecertificateDate$txtDatetime" EnableClientScript="True"
                            ValidationGroup="PopupRequiredGroup" Display="Dynamic" CssClass="fl"></asp:RequiredFieldValidator>
                        <div class="cb">
                            <asp:CompareValidator ID="cpRecertificationDate" ControlToValidate="txtRecertificateDate$txtDatetime"
                                ControlToCompare="txtCertificateDate$txtDatetime" resourcekey="CompareDateBoardCert.Text"
                                Type="Date" runat="server" Display="Dynamic" Operator="GreaterThan" ValidationGroup="PopupRequiredGroup" />
                        </div>
                        <div class="cb">
                            <asp:CustomValidator runat="server" ID="CustomValidator2" ControlToValidate="txtRecertificateDate$txtDatetime"
                                ClientValidationFunction="BoardValidateReCertCircle" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                                resourcekey="InvalidTimeFrame.Text" EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
                <tr id="UploadFileAttachment" runat="server">
                    <td>
                        <asp:Label ID="lblUploadFileAttachment" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:FileUpload ID="fuUploadFileAttachment" runat="server" CssClass="fl" />
                        <asp:CustomValidator runat="server" ID="rqUploadFileAttachment" EnableClientScript="True"
                            Display="Dynamic" ValidationGroup="PopupRequiredGroup" ClientValidationFunction="ValidateUploadFile"
                            ControlToValidate="fuUploadFileAttachment" ValidateEmptyText="True" ErrorMessage="*"
                            CssClass="fl"></asp:CustomValidator>
                        <br />
                        <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuUploadFileAttachment"
                            ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="PopupRequiredGroup"
                            resourcekey="RequirePDFFile.Text" Display="Dynamic" />
                        <div id="divUploadFileAttachment">
                            <asp:HyperLink runat="server" ID="hlUploadFileAttachment" CssClass="fl" Text="sample.pdf"
                                Target="_blank"></asp:HyperLink>
                            <asp:ImageButton runat="server" ID="imgDeleteAttachment" CssClass="fl" ImageUrl="../../Documentation/images/icons/delete_icon1.gif" />
                            <asp:HiddenField runat="server" ID="hfDeleteFile" Value="NO" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</div>
