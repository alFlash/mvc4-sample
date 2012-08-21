<%@ Control language="vb" Inherits="AMC.DNN.Modules.CertRecert.ViewAmcCertRecertGui" AutoEventWireup="false" Explicit="True" Codebehind="View.ascx.vb" %>

<script language="javascript" type="text/javascript">
    function deleteDocument(docPath, callback) {
        var xmlhttp;
        if (docPath.length == 0) {
            alert(DocumentNotFound);
            return;
        }
        if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
            xmlhttp = new XMLHttpRequest();
        }
        else {// code for IE6, IE5
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        //certModuleCurrentPageURL is generated in server code
        if (typeof certModuleCurrentPageURL != 'undefined' && certModuleCurrentPageURL.length == 1) {
            xmlhttp.open("GET", certModuleCurrentPageURL[0] + "&DocPath=" + escape(docPath), true);
            xmlhttp.onreadystatechange = function () { callback(xmlhttp); };
            xmlhttp.send(null);
        }
    }

    function processReturnValue(xmlhttp) {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            if (xmlhttp.responseText.startsWith("1")) {
                alert(Success);
            }
            else if (xmlhttp.responseText.startsWith("0")) {
                alert(Fail);
            }
        }
    }


    ParentModulePath = '<%= ModulePath %>';
    FieldIsRequiredString = '<%= FieldIsRequiredString %>';
    InputValueNotCorrect = '<%= InputValueNotCorrect %>';
    var ConfirmDeleteQuestion = '<%= GetResourceText("ConfirmDeleteQuestion.Text") %>';
    var ConfirmResetData = '<%= GetResourceText("ConfirmResetData.Text") %>';
    var ConfirmDeleteFile = '<%= GetResourceText("ConfirmDeleteFile.Text") %>';
    var ConfirmDeleteRowText = '<%= GetResourceText("ConfirmDeleteRowText.Text") %>';
    var Fail = '<%= GetResourceText("Fail.Text") %>';
    var Success = '<%= GetResourceText("Success.Text") %>';
    var DocumentNotFound = '<%= GetResourceText("DocumentNotFound.Text") %>';
    var DeleteFileError = '<%= GetResourceText("DeleteFileError.Text") %>';

   
</script>
<div id="amc-certification-modules">
<asp:PlaceHolder ID="phAMCMain" runat="server"></asp:PlaceHolder>
</div>


 
