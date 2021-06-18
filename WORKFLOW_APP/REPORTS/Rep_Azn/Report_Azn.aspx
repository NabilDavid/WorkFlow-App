<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_Azn.aspx.cs" Inherits="WORKFLOW_APP.REPORTS.Rep_Azn.Report_Azn" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
 
         <localreport reportpath="REPORTS/Rep_Azn/Report_AznOrder.rdlc" reportembeddedresource="WORKFLOW_APP.REPORTS.Rep_Azn.Report_AznOrder.rdlc">
             </localreport> 
        </rsweb:ReportViewer>
        
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </div>
    </form>
</body>
</html>
