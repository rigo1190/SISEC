<%@ Page Title="" Language="C#" MasterPageFile="~/Navegador.Master" AutoEventWireup="true" CodeBehind="Calendario.aspx.cs" Inherits="SISEC.Reports.Calendario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function fnc_CargarDatos(idSesion) {
            
            PageMethods.GetDatosSesion(idSesion, fnc_ColocarDatosSesion);
            
        }

        function fnc_ColocarDatosSesion(response) {

            $("#<%= txtFideicomiso.ClientID %>").val(response[0]);
            $("#<%= txtNumOficio.ClientID %>").val(response[1]);
            $("#<%= txtFechaOficio.ClientID %>").val(response[2]);
            $("#<%= txtNumSesion.ClientID %>").val(response[3]);
            $("#<%= txtTipoSesion.ClientID %>").val(response[4]);
            $("#<%= txtLugarReunion.ClientID %>").val(response[5]);
            $("#<%= txtDescripcion.ClientID %>").val(response[6]);

            switch (response[7]) {
                case "P": //PROGRAMADA
                    $("#<%= txtStatusSesion.ClientID %>").val("PROGRAMADA");
                    $("#<%= txtFechaProgramada.ClientID %>").val(response[8]);
                    $("#<%= txtHoraProgramada.ClientID %>").val(response[9]);

                    $("#<%= divDatosProgramada.ClientID %>").css("display", "block");
                    $("#<%= divDatosReprogramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosCelebrada.ClientID %>").css("display", "none");

                    break;
                case "RP"://REPROGRAMADA
                    $("#<%= txtStatusSesion.ClientID %>").val("REPROGRAMADA");
                    $("#<%= txtFechaReprogramada.ClientID %>").val(response[8]);
                    $("#<%= txtHoraReprogramada.ClientID %>").val(response[9]);
                    $("#<%= txtObservaciones.ClientID %>").val(response[10]);

                    $("#<%= divDatosProgramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosReprogramada.ClientID %>").css("display", "block");
                    $("#<%= divDatosCelebrada.ClientID %>").css("display", "none");
                    break;
                case "CE": //CELEBRADA
                    $("#<%= txtStatusSesion.ClientID %>").val("CELEBRADA");
                    $("#<%= txtFechaCelebrada.ClientID %>").val(response[8]);
                    $("#<%= txtHoraCelebrada.ClientID %>").val(response[9]);

                    $("#<%= divDatosProgramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosReprogramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosCelebrada.ClientID %>").css("display", "block");
                    break;
                case "C": //CANCELADA
                    $("#<%= txtStatusSesion.ClientID %>").val("CELEBRADA");
                    break;
            }

            $("#myModal").modal('show') //Se muestra el modal

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="alert alert-success alert-dismissable">
                    <h4><i class="fa fa-crosshairs"></i> <strong>Agenda</strong></h4>  
                </div>
            </div>


            <div class="row" runat="server" id="divEncabezado">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Datos de Consulta</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label>Fideicomiso</label>
                                    <asp:DropDownList ID="ddlFideicomisos" runat="server" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>                                         
                                </div>
                            </div>

                           <%-- <div class="col-lg-4">
                                <div class="form-group">
                                    <label>Status de la Sesión</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>                                         
                                </div>
                            </div>

                            <div class="col-lg-4">
                                <div class="form-group">
                                    <label>Tipo de Sesión</label>
                                    <asp:DropDownList ID="ddlTipoSesion" runat="server" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>                                         
                                </div>
                            </div>--%>
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
                                    NextPrevFormat="FullMonth"
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
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="smallModal" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabel">Datos de la sesión</h4>
              </div>
              <div class="modal-body">
                     <div class="col-lg-6 ">
                        <div class="form-group">
                            <label>Fideicomiso</label>
                             <input class="form-control" disabled="disabled" runat="server" id="txtFideicomiso"/>
                        </div>
                        <div class="form-group">
                            <label>Número de Oficio:</label>
                            <input class="form-control" disabled="disabled"  runat="server" id="txtNumOficio" />
                        </div>
                        <div class="form-group">
                            <label>Fecha Oficio:</label>
                            <input class="form-control" disabled="disabled"  runat="server" id="txtFechaOficio"/>
                        </div>
                        <div class="form-group">
                            <label>Status Sesión:</label>
                            <input class="form-control" disabled="disabled"  runat="server" id="txtStatusSesion"/>
                        </div>
                        <div id="divDatosProgramada" runat="server">
                            <div class="form-group">
                                <label>Fecha Programada:</label>
                                <input class="form-control" disabled="disabled"  runat="server" id="txtFechaProgramada"/>
                            </div>
                            <div class="form-group">
                                <label>Hora Programada:</label>
                                <input class="form-control" disabled="disabled"  runat="server" id="txtHoraProgramada" />      
                            </div>
                        </div>
                        <div id="divDatosReprogramada" style="display:none" runat="server">
                            <div class="form-group">
                                <label>Fecha Reprogramada:</label>
                                <input class="form-control" disabled="disabled"  runat="server" id="txtFechaReprogramada"/>
                            </div>
                            <div class="form-group">
                                <label>Hora Reprogramada:</label>
                                <input class="form-control" disabled="disabled"  runat="server" id="txtHoraReprogramada" />
                                
                            </div>
                                <div class="form-group">
                                <label>Observaciones</label>
                                <textarea type="text" name="prueba" disabled="disabled"  runat="server" class="form-control" id="txtObservaciones" />
                            </div>
                        </div>       
                        <div id="divDatosCelebrada" style="display:none" runat="server">
                            <div class="form-group">
                                <label>Fecha Celebrada:</label>
                                <input class="form-control" disabled="disabled"  runat="server" id="txtFechaCelebrada"/>
                            </div>
                            <div class="form-group">
                                <label>Hora Celebrada:</label>
                                <input class="form-control" disabled="disabled"  runat="server" id="txtHoraCelebrada" />
                            </div>
                        </div>
                    </div>
                     <div class="col-lg-6 ">
                        <div class="form-group">
                            <label>Número de Sesión</label>
                            <input class="form-control" disabled="disabled" runat="server" id="txtNumSesion"/>
                        </div>
                        <div class="form-group">
                            <label>Tipo de Sesión</label>
                             <input class="form-control" disabled="disabled" runat="server" id="txtTipoSesion"/>
                        </div>
                        <div class="form-group">
                            <label>Lugar de Reunión:</label>
                            <textarea type="text" disabled="disabled" name="prueba" runat="server" class="form-control" id="txtLugarReunion" />
                        </div>
                        <div class="form-group">
                            <label>Asuntos Relevantes:</label>
                            <textarea type="text" disabled="disabled" name="prueba" runat="server" class="form-control" id="txtDescripcion" />
                        </div>
                               
                    </div>
              </div>
              <div class="modal-footer">
                
              </div>
        
            </div>
        </div>
    </div>
</asp:Content>
