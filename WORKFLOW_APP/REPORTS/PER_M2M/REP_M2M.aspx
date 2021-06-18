<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="REP_M2M.aspx.cs" Inherits="WORKFLOW_APP.REPORTS.PER_M2M.REP_M2M" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
 <form id="Form1" runat="server">
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                          <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                  
                                 <%--<asp:Button ID="view_report" OnClick="show_gwab_Click" hidden="true" runat="server" Text=" "  Height="35px" Width="88px" CssClass="size" style="height:55px; margin-top: 190px;border-style: hidden; width:88px;/*background-color: #449aca;*/border-radius: 24px; background-image:url('../Images/colapse.png'); background-size:99%; background-repeat: no-repeat;background-position: center;"/>--%>

                       
                     <div class="row">
                <div class="col-lg-12">
                     
        
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    <div>
           <asp:Literal ID="ltEmbed" runat="server" />
        <rsweb:ReportViewer  ID="ReportViewer1"  Visible="false" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="300px" Height="200px">

    <%--        <LocalReport ReportEmbeddedResource="WORKFLOW_APP.REPORTS.PER_M2M.Report_M2m.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="M2M_DET" />
                </DataSources>
            </LocalReport>--%>

        </rsweb:ReportViewer>  
             
           <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="DataSet1TableAdapters."></asp:ObjectDataSource>
             
    </div>

                </div>
            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>    
         </form>
</body>
</html>
