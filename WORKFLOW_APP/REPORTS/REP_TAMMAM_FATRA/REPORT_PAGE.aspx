<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="REPORT_PAGE.aspx.cs" Inherits="WORKFLOW_APP.REPORTS.REP_TAMMAM_FATRA.REPORT_PAGE" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>تقـرير الضبـاط  المتأخـرين</title>

    
      <%--<script src="../../assets/js/jquery-2.1.4.min.js"></script>--%>
    <script src="../../Scripts/assets/js/jquery-2.1.4.min.js"></script>
    <!-- Required meta tags -->
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
	<%--<link rel="icon" href="../../images/sakr_groub.jpeg" type="../../image/png">--%>
    <link href="../../Content/themes/base/minified/jquery.ui.dialog.min.css" rel="stylesheet" />
   <%--<link href="../../Content/themes/base/minified/jquery.ui.dialog.min.css" rel="stylesheet" />--%>
  
	<!-- Bootstrap CSS -->
 

    <link href="../../Scripts/jqwidgets/styles/jqx.bootstrap.css" rel="stylesheet" />

	<%--<link rel="stylesheet" href="../../vendors/linericon/style.css">--%>
	<%--<link rel="stylesheet" href="../../css/font-awesome.min.css">--%>
    <link href="../../Scripts/assets/css/font-awesome.min.css" rel="stylesheet" />
      <link href="../../Content/themes/base/jquery.ui.dialog.css" rel="stylesheet" />
    <link href="../../Content/themes/base/jquery-ui.css" rel="stylesheet" />
    <link href="../../Scripts/APP_CSS/animate.css" rel="stylesheet" />
	<link rel="stylesheet" href="../../css/magnific-popup.css">
	<%--<link rel="stylesheet" href="../../vendors/owl-carousel/owl.carousel.min.css">--%>
<%--	<link rel="stylesheet" href="../../vendors/lightbox/simpleLightbox.css">
	<link rel="stylesheet" href="../../vendors/nice-select/css/nice-select.css">
	<link rel="stylesheet" href="../../vendors/jquery-ui/jquery-ui.css">
    
	<link rel="stylesheet" href="../../vendors/animate-css/animate.css">
    <link rel="stylesheet" href="../../css/style.css">
    <link href="../../vendor/animate/animate.css" rel="stylesheet" />--%>
     <%--<link rel="stylesheet" href="../assets/css/bootstrap.min.css" />--%>
    <%--<link rel="stylesheet" href="../assets/font-awesome/4.5.0/css/font-awesome.min.css" />--%>

    <!-- page specific plugin styles -->

    <!-- text fonts -->
    <%--<link rel="stylesheet" href="../assets/css/fonts.googleapis.com.css" />--%>

    <!-- ace styles -->
    <%--<link rel="stylesheet" href="../assets/css/ace.min.css" class="ace-main-stylesheet" />--%>

    <!--[if lte IE 9]>
			<link rel="stylesheet" href="assets/css/ace-part2.min.css" class="ace-main-stylesheet" /><link href="../assets/css/ace-ie.min.css" rel="stylesheet" />
		<![endif]-->
<%--    <link rel="stylesheet" href="../assets/css/ace-skins.min.css" />
    <link rel="stylesheet" href="../assets/css/ace-rtl.min.css" />
	
--%>


    

    <link href="../../Scripts/jqwidgets/styles/jqx.base.css" rel="stylesheet" />
	 
    <script src="../../Scripts/jqwidgets/jqxcore.js"></script>
    <%--<script src="../jqwidgets/jqx-all.js"></script>--%>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxscrollbar.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxdatatable.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxlistbox.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxdropdownlist.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxdata.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxtooltip.js"></script>
     <script type="text/javascript" src="../../Scripts/jqwidgets/jqxbuttons.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets/jqxinput.js"></script>
    <script src="../../Scripts/jqwidgets/jqxradiobutton.js"></script>

    <script src="../../Scripts/jqwidgets/jqxcheckbox.js"></script>
    <script src="../../Scripts/jqwidgets/jqxdatetimeinput.js"></script>
    <script src="../../Scripts/jqwidgets/jqxtabs.js"></script>
    <link href="../../Scripts/jqwidgets/styles/jqx.ui-redmond.css" rel="stylesheet" />
    <link href="../../Scripts/jqwidgets/styles/jqx.orange.css" rel="stylesheet" />
    <script src="../../Scripts/jqwidgets/jqxwindow.js"></script>
    <script src="../../Scripts/jqwidgets/jqxpanel.js"></script>
    <script src="../../Scripts/jqwidgets/jqxnumberinput.js"></script>
    <script src="../../Scripts/jqwidgets/jqxdropdownlist.js"></script>
    <script src="../../Scripts/jqwidgets/jqxdropdownbutton.js"></script>
    <script src="../../Scripts/jqwidgets/jqxcalendar.js"></script>
    <script type="text/javascript" src="../../Scripts/jqwidgets//jqxexpander.js"></script>
    <link href="../../Scripts/jqwidgets/styles/jqx.light.css" rel="stylesheet" />
    <link href="../../Scripts/jqwidgets/styles/jqx.darkblue.css" rel="stylesheet" />
    <link href="../../Scripts/jqwidgets/styles/jqx.blackberry.css" rel="stylesheet" />
    <script src="../../Scripts/jqwidgets/jqxmenu.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.aggregates.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.columnsreorder.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.columnsresize.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.edit.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.export.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.filter.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.grouping.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.pager.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.selection.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.sort.js"></script>
    <script src="../../Scripts/jqwidgets/jqxgrid.storage.js"></script>
    <script src="../../Scripts/js/sweetalert.min.js"></script>
    <link href="../../Scripts/css/sweetalert.css" rel="stylesheet" />

    <script src="REP_SCRIPT.js"></script>


</head>
<body>
    <form id="Form1" runat="server">


        <div class="animated  zoomInDown">
             
            <table style="width:100%" dir="rtl">
                <tr style="width:100%">
                    <td style="width:10%">
                            <div >
            <label>من الفتره</label>
            </div> 
                    </td>
                    <td style="width:30%; text-align:right" >
                            <div class="">
            <div id="from"></div>
            </div>
                    </td>
                        <td style="width:10%">
                                <div class="">
            <label>الي الفتره</label>
            </div>
                    </td>
                        <td style="width:30%;text-align:right">
                                     <div class="">
           <div id="to">

           </div>
            </div>
                    </td>
                        <td style="width:20%">
<div class="">
                   <input id="rep_click" type="button" value="عــرض"   style="width: 82px;cursor:pointer;  "/>
                    
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                      <ContentTemplate>
                           <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                  
               <asp:Button ID="view_report" OnClick="Button1_Click"  runat="server" Text="اضغط "   style="height:55px; display:none; width:88px;border-radius: 24px;  background-repeat: no-repeat;background-position: center;" BackColor="#66FFFF" Font-Bold="True" Font-Italic="True"/>
                          
                         
</ContentTemplate>
</asp:UpdatePanel>
                       
            </div>
                    </td>
                </tr>
            </table>


         <div class="row">
   
        
         
         
    

             
              
             </div>



             </div>
                       <div class="row">
                <div class="col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <%--<asp:ScriptManager EnablePageMethods="true" ID="ScriptManager2" runat="server"></asp:ScriptManager>--%>

                    <asp:TextBox ID="TextBox2"   runat="server" Height="16px" style=" display:none;"></asp:TextBox>
                    <asp:Label ID="Label2" runat="server" Text="التاريخ" style=" display:none;"></asp:Label>

                  
                    <asp:Literal ID="ltEmbed" runat="server" />

                    <rsweb:ReportViewer Visible="false" ID="ReportViewer1" runat="server" BorderColor="#333399" BorderStyle="Double" Font-Names="Verdana" Font-Size="8pt" Height="767px" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="1068px">
                        <LocalReport ReportEmbeddedResource="WORKFLOW_APP.REPORTS.REP_TAMMAM_FATRA.PERORT_TAMMAM1.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>

                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
                    </div>
                           </div>
    </form>
</body>
</html>