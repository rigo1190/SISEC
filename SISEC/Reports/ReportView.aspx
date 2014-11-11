<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportView.aspx.cs" Inherits="SISEC.Reports.ReportView" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="<%= ResolveClientUrl("~/Scripts/jquery-1.9.1.js") %>"></script>
    <link href="<%= ResolveClientUrl("~/Content/bootstrap-theme.css") %>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("~/Content/bootstrap.css") %>" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" style="width:1000px">
        <div class="col-lg-12">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
        </div>
    </form>
</body>
</html>
