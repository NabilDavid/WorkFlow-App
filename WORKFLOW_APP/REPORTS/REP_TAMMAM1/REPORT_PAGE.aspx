<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="REPORT_PAGE.aspx.cs" Inherits="WORKFLOW_APP.REPORTS.REP_TAMMAM1.REPORT_PAGE" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <asp:ScriptManager EnablePageMethods="true" ID="ScriptManager2" runat="server"></asp:ScriptManager>

                    <asp:TextBox ID="TextBox2" runat="server" Height="16px"></asp:TextBox>
                    <asp:Label ID="Label2" runat="server" Text="التاريخ"></asp:Label>

                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="عرض" />
                    <asp:Literal ID="ltEmbed" runat="server" />

                    <rsweb:ReportViewer Visible="false" ID="ReportViewer1" runat="server" BorderColor="#333399" BorderStyle="Double" Font-Names="Verdana" Font-Size="8pt" Height="767px" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="1068px">
                        <LocalReport ReportEmbeddedResource="WORKFLOW_APP.REPORTS.REP_TAMMAM1.PERORT_TAMMAM1.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>

                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>