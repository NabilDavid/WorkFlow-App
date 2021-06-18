<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TalabM2morya.aspx.cs" Inherits="WORKFLOW_APP.REPORTS.WebForm1" %>

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
    
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="700px">
           
            <LocalReport ReportPath="REPORTS/Rep_gwab_mam/TalabM2m.rdlc" ReportEmbeddedResource="WORKFLOW_APP.REPORTS.Rep_gwab_mam.TalabM2m.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                </DataSources>
                </LocalReport>
        </rsweb:ReportViewer>

        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="DataSet1TableAdapters."></asp:ObjectDataSource>

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    </div>
    </form>
</body>
</html>
