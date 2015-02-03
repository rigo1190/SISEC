<%@ Page Title="" Language="C#" MasterPageFile="~/Navegador.Master" AutoEventWireup="true" CodeBehind="Sesiones.aspx.cs" Inherits="SISEC.Formas.Sesiones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $('.datepicker').datepicker(
            {
                format: "dd/mm/yyyy"
            });

            $('.hour').timepicker();

        });

        function fnc_Cancelar() {
            $("#<%= divEncabezado.ClientID %>").css("display", "block");
            $("#<%= divCapturaSesion.ClientID %>").css("display", "none");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        function fnc_ColocarIDSesion(idSesion) {
            $("#<%= _IDSesion.ClientID %>").val(idSesion);
        }

        function fnc_GetDatosStatus(sender) {
            var _idStatus = sender.value;
            PageMethods.GetDatosStatus(_idStatus, fnc_ResponseGetDatosStatus);
            
        }

        function fnc_ResponseGetDatosStatus(response) {
            var claveStatus = response[0];

            switch (claveStatus) {
                case "P": //PROGRAMADA
                    $("#<%= divDatosProgramada.ClientID %>").css("display", "block");
                    $("#<%= divDatosReprogramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosCelebrada.ClientID %>").css("display", "none");
                    break;
                case "RP"://REPROGRAMADA
                    $("#<%= divDatosProgramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosReprogramada.ClientID %>").css("display", "block");
                    $("#<%= divDatosCelebrada.ClientID %>").css("display", "none");
                    break;
                case "CE": //CELEBRADA
                    $("#<%= divDatosProgramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosReprogramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosCelebrada.ClientID %>").css("display", "block");
                    break;
                case "C": //CANCELADA
                    $("#<%= divDatosProgramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosReprogramada.ClientID %>").css("display", "none");
                    $("#<%= divDatosCelebrada.ClientID %>").css("display", "none");
                    break;
            }

        }

        function fnc_Validar() {

            //$("#myselect").val();

            var status=$("#<%= ddlStatus.ClientID %>").val();
            var fechaOficio = $("#<%= txtFechaOficio.ClientID %>").val();
            var fecha;

            if (fechaOficio == "" || fechaOficio == null || fechaOficio == undefined) {
                $("#<%= divMsgError.ClientID %>").css("display", "block");
                $("#<%= lblMsgError.ClientID %>").text("Debe indicar la fecha de oficio");
                return false;
            }
                
            var msg = "";
            switch (status) {
                case "1": //PROGRAMADA
                    fecha = $("#<%= txtFechaProgramada.ClientID %>").val();
                    msg = "Debe indicar la fecha programada";
                    break;
                case "2": //REPROGRAMADA
                    fecha = $("#<%= txtFechaReprogramada.ClientID %>").val();
                    msg = "Debe indicar la fecha reprogramada";
                    break;
                case "3": //CELEBRADA
                    fecha = $("#<%= txtFechaCelebrada.ClientID %>").val();
                    msg = "Debe indicar la fecha celebrada";
                    break;
            }


            if (fecha == "" || fecha == null || fecha == undefined) {
                $("#<%= divMsgError.ClientID %>").css("display", "block");
                $("#<%= lblMsgError.ClientID %>").text(msg);
                 return false;
            }

            return true;

        }


        

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="page-wrapper">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <div class="container-fluid">
            
            <div class="row">
                <div class="col-lg-12">
                    <div class="alert alert-success alert-dismissable">
                        <h4><i class="fa fa-crosshairs"></i> <strong>Calendario de Sesiones</strong></h4>  
                    </div>
                </div>
            </div>

            <div class="row">
                 <div class="col-lg-12">
                    <div class="alert alert-warning" runat="server" id="divAlerta" style="display:none">
                        <asp:Label ID="lblAlerta" EnableViewState="false" runat="server" Text="Alerta" CssClass="font-weight:bold"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="divEncabezado">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Seleccione Fideicomiso</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-9">
                                <asp:DropDownList ID="ddlFideicomisos" OnSelectedIndexChanged="ddlFideicomisos_SelectedIndexChanged" runat="server" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>                                         
                             </div>
                            <div class="col-lg-3">
                                <span class="glyphicon glyphicon-plus-sign"></span>
                                <asp:Button ID="btnCrearCalendario" CssClass="btn btn-default" runat="server" OnClick="btnCrearCalendario_Click" Text="Crear Calendario" />                                     
                             </div>
                        </div>
                            
                    </div>

                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i> Lista de Sesiones</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-12">
                                <asp:GridView ID="gridSesiones" ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" OnPageIndexChanging="gridSesiones_PageIndexChanging" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gridSesiones_RowDataBound" runat="server" AutoGenerateColumns="false" >
                                    <Columns>
                                         <asp:TemplateField HeaderText="Acciones">
                                            <ItemTemplate>
                                                <asp:ImageButton  ID="imgBtnEdit" ToolTip="Editar" OnClick="imgBtnEdit_Click" runat="server" ImageUrl="~/img/Edit1.png" />
                                                <asp:ImageButton  ID="imgBtnEliminar" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png" data-toggle="modal" data-target="#myModal"/>
                                            </ItemTemplate>
                                            <HeaderStyle BackColor="#EEEEEE" />
                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Número de Sesión">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "NumSesion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField Visible="false" HeaderText="Fecha Programada" SortExpression="FP">
                                            <ItemTemplate>
                                               <%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FechaProgramada")).ToString("d")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="Hora Programada" SortExpression="HP">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "HoraProgramada")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField Visible="false" HeaderText="Fecha Reprogramada" SortExpression="FRP">
                                            <ItemTemplate>
                                                <%# String.Format("{0:d}",DataBinder.Eval(Container.DataItem, "FechaReprogramada"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="Hora Reprogramada" SortExpression="HRP">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "HoraReprogramada")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField Visible="false" HeaderText="Fecha Celebrada" SortExpression="FCE">
                                            <ItemTemplate>
                                                <%# String.Format("{0:d}",DataBinder.Eval(Container.DataItem, "FechaCelebrada"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="Hora Celebrada" SortExpression="HCE">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "HoraCelebrada")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status Sesión" SortExpression="Año">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asuntos relevantes" SortExpression="Año">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns> 
                                    
                                    <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                </asp:GridView>
                             </div>
                        </div>
                            
                    </div>
                </div>
            </div>
            <div class="row" runat="server" id="divCapturaSesion" style="display:none">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Datos de la Sesión</h3>
                        </div>
                         <div class="panel-body">
                            <div class="col-lg-6 ">
                                <div class="form-group">
                                    <label>Fideicomiso</label>
                                    <textarea type="text" disabled="disabled" name="prueba" runat="server" class="form-control" id="txtFideicomiso" />
                                </div>
                                 <div class="form-group">
                                    <label>Número de Oficio:</label>
                                    <input class="form-control" runat="server" id="txtNumOficio" />
                                </div>
                                <div class="form-group">
                                    <label>Fecha Oficio:</label>
                                    <div class="input-group">
                                        <input class="form-control datepicker" runat="server" id="txtFechaOficio"/>
                                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label>Status Sesión:</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>             
                                </div>

                                <div id="divDatosProgramada" runat="server">
                                    <div class="form-group">
                                        <label>Fecha Programada:</label>
                                        <div class="input-group">
                                             <input class="form-control datepicker" runat="server" id="txtFechaProgramada"/>
                                             <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label>Hora Programada:</label>
                                        <div class="input-group">
                                            <input class="form-control hour" runat="server" id="txtHoraProgramada" />
                                            <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                                        </div>
                                    </div>
                                </div>

                                <div id="divDatosReprogramada" style="display:none" runat="server">
                                    <div class="form-group">
                                        <label>Fecha Reprogramada:</label>
                                        <div class="input-group">
                                             <input class="form-control datepicker" runat="server" id="txtFechaReprogramada"/>
                                             <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label>Hora Reprogramada:</label>
                                        <div class="input-group">
                                            <input class="form-control hour" runat="server" id="txtHoraReprogramada" />
                                            <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                                        </div>
                                    </div>
                                     <div class="form-group">
                                        <label>Observaciones</label>
                                        <textarea type="text" name="prueba" runat="server" class="form-control" id="txtObservaciones" />
                                    </div>
                                </div>
                                
                                <div id="divDatosCelebrada" style="display:none" runat="server">
                                    <div class="form-group">
                                        <label>Fecha Celebrada:</label>
                                        <div class="input-group">
                                             <input class="form-control datepicker" runat="server" id="txtFechaCelebrada"/>
                                             <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label>Hora Celebrada:</label>
                                        <div class="input-group">
                                            <input class="form-control hour" runat="server" id="txtHoraCelebrada" />
                                            <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                                        </div>
                                    </div>
                                </div>
                                
                            </div>
                            <div class="col-lg-6 ">
                                <div class="form-group">
                                    <label>Número de Sesión</label>
                                    <input class="form-control" runat="server" id="txtNumSesion"/>
                                </div>
                                <div class="form-group">
                                    <label>Tipo de Sesión</label>
                                    <asp:DropDownList ID="ddlTipoSesion" runat="server" CssClass="form-control"></asp:DropDownList>                                         
                                </div>
                                <div class="form-group">
                                    <label>Lugar de Reunión:</label>
                                    <textarea type="text" name="prueba" runat="server" class="form-control" id="txtLugarReunion" />
                                </div>
                                 <div class="form-group">
                                    <label>Asuntos Relevantes:</label>
                                    <textarea type="text" name="prueba" runat="server" class="form-control" id="txtDescripcion" />
                                </div>
                               
                            </div>

                            <div class="col-lg-12">
                                 <div class="form-group">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClientClick="return fnc_Validar();" OnClick="btnGuardar_Click" CssClass="btn btn-default" ></asp:Button>
                                    <button type="button" onclick="fnc_Cancelar();" class="btn btn-default">Cancelar</button> 
                                </div>
                            </div>
                         </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel-footer">
                        <div class="alert alert-danger" runat="server" id="divMsgError" style="display:none">
                            <asp:Label ID="lblMsgError" EnableViewState="false" runat="server" Text="" CssClass="font-weight:bold"></asp:Label>
                        </div>
                        <div class="alert alert-success" runat="server" id="divMsgSuccess" style="display:none">
                            <asp:Label ID="lblMsgSuccess" EnableViewState="false" runat="server" Text="" CssClass="font-weight:bold"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_IDCalendario" />
        <input type="hidden" runat="server" id="_Accion" />
        <input type="hidden" runat="server" id="_IDSesion" />
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="smallModal" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabel">Confirmación</h4>
              </div>
              <div class="modal-body">
                <h3 id="msgContenido">¿Está seguro que desea eliminar el registro?</h3>
              </div>
              <div class="modal-footer">
                <asp:Button ID="btnDel" OnClientClick="return fnc_Eliminar();" OnClick="btnDel_Click" runat="server" CssClass="btn btn-default" Text="Aceptar"  />
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
              </div>
        
            </div>
        </div>
    </div>



</asp:Content>
