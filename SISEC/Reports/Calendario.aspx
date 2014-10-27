<%@ Page Title="" Language="C#" MasterPageFile="~/Navegador.Master" AutoEventWireup="true" CodeBehind="Calendario.aspx.cs" Inherits="SISEC.Reports.Calendario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="alert alert-success alert-dismissable">
                    <h4><i class="fa fa-crosshairs"></i> <strong>Agenda</strong></h4>  
                </div>
            </div>


            <div class="row" runat="server" id="divEncabezado">
                <div class="col-lg-12">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa"></i>Fideicomiso</h3>
                    </div>
                    <div class="panel-body">
                        <div class="col-lg-12">
                            <asp:DropDownList ID="ddlFideicomisos" runat="server" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>                                         
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="divAgenda">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                         <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Agenda de sesiones</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-12">
                                <asp:Calendar ID="Calendar1" runat="server" OnDayRender="Calendar1_DayRender" OnSelectionChanged="Calendar1_SelectionChanged" OnVisibleMonthChanged="Calendar1_VisibleMonthChanged"
                                    DayStyle-Height="100" DayStyle-Width="75" DayStyle-HorizontalAlign="Left"
                                    DayStyle-verticalalign="Top"
                                    DayStyle-Font-Name="Arial" DayStyle-Font-Size="12" 
                                    NextPrevFormat="FullMonth" SelectionMode="Day" 
                                    TitleStyle-Font-Bold="False" TitleStyle-Font-Name="Verdana" 
                                    TitleStyle-Font-Size="12" BackColor="white" BorderColor="#000000"
                                    CellPadding="2" CellSpacing="2" 
                                    SelectedDayStyle-BackColor="#faebd7"
                                    SelectedDayStyle-ForeColor="#000000"
                                    OtherMonthDayStyle-ForeColor="#C0C0C0" DayStyle-BorderStyle="Solid" 
                                    DayStyle-BorderWidth="1" TodayDayStyle-ForeColor="Black" Height="600" 
                                    DayHeaderStyle-Font-Name="Verdana"
                                    Width="750"
                                    >
                                </asp:Calendar>                                   
                             </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_IDCalendario" />
        <input type="hidden" runat="server" id="_IDSesion" />
    </div>
</asp:Content>
