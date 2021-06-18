<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="safr_rep.aspx.cs" Inherits="WORKFLOW_APP.REPORTS.SAFR_REP.safr_rep" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
         <div>
    
    
   
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="700px">
            <LocalReport reportpath="REPORTS/SAFR_REP/safr_rep.rdlc"  ReportEmbeddedResource="WORKFLOW_APP.REPORTS.SAFR_REP.safr_rep.rdlc">
           
                 </LocalReport>
        </rsweb:ReportViewer>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
         </div>
    </form>
</body>
</html>
