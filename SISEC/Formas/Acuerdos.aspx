<%@ Page Title="" Language="C#" MasterPageFile="~/Navegador.Master" AutoEventWireup="true" CodeBehind="Acuerdos.aspx.cs" Inherits="SISEC.Formas.Acuerdos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#linkAcuerdo").click(function () {
                $("#<%= divCapturaDetalle.ClientID %>").css("display", "block");
                $("#<%= divSeguimiento.ClientID %>").css("display", "none");
                $("#<%= divMsgError.ClientID %>").css("display", "none");
                $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            });

            $("#linkSeguimiento").click(function () {
                $("#<%= divCapturaDetalle.ClientID %>").css("display", "none");
                $("#<%= divSeguimiento.ClientID %>").css("display", "block");

                if ($("#<%= _AccionS.ClientID %>").val() == "") {
                    $("#<%= divEncabezadoSeguimiento.ClientID %>").css("display", "block");
                    $("#<%= divCapturaSeguimiento.ClientID %>").css("display", "none");
                } else {
                    $("#<%= divEncabezadoSeguimiento.ClientID %>").css("display", "none");
                    $("#<%= divCapturaSeguimiento.ClientID %>").css("display", "block");
                }

                $("#<%= divMsgError.ClientID %>").css("display", "none");
                $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            });

        });

        function fnc_Cancelar() {
            $("#<%= divEncabezadoDetalle.ClientID %>").css("display", "block");

            $("#<%= divCapturaDetalle.ClientID %>").css("display", "none");
            $("#<%= divMenu.ClientID %>").css("display", "none");

            $("#<%= divEncabezadoSeguimiento.ClientID %>").css("display", "none");
            $("#<%= divCapturaSeguimiento.ClientID %>").css("display", "none");

            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        function fnc_ColocarIDAcuerdo(idSeguimiento) {
            $("#<%= _IDAcuerdo.ClientID %>").val(idSeguimiento);
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        function fnc_CrearNuevoAcuerdo() {
            $("#<%= divEncabezadoDetalle.ClientID %>").css("display", "none");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= divCapturaDetalle.ClientID %>").css("display", "block");
            $("#<%= txtNotas.ClientID %>").val("");
            $("#<%= txtNumAcuerdo.ClientID %>").val("");
            $("#<%= _Accion.ClientID %>").val("N");

            $("#<%= txtNotas.ClientID %>").prop('disabled', false);
            $("#<%= txtNumAcuerdo.ClientID %>").prop('disabled', false);
            $("#<%= ddlStatus.ClientID %>").prop('disabled', false);
            $("#<%= btnGuardar.ClientID %>").prop('disabled', false);

            return false;
        }

        function fnc_Volver() {
            $("#<%= divEncabezado.ClientID %>").css("display", "block");
            $("#<%= divDetalleAcuerdos.ClientID %>").css("display", "none");
            $("#<%= divEncabezadoDetalle.ClientID %>").css("display", "none");
            $("#<%= divCapturaDetalle.ClientID %>").css("display", "none");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        function fnc_CrearNuevoSeguimiento() {
            $("#<%= divEncabezadoSeguimiento.ClientID %>").css("display", "none");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= divCapturaSeguimiento.ClientID %>").css("display", "block");
            $("#<%= txtDescripcion.ClientID %>").val("");
            $("#<%= _AccionS.ClientID %>").val("N");

            return false;
        }

        function fnc_CancelarS() {
            
            $("#<%= divEncabezadoSeguimiento.ClientID %>").css("display", "block");
            $("#<%= divCapturaSeguimiento.ClientID %>").css("display", "none");

            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        function fnc_ColocarIDSeguimiento(idSeguimiento) {
            $("#<%= _IDSeguimiento.ClientID %>").val(idSeguimiento);
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="alert alert-success alert-dismissable">
                        <h4><i class="fa fa-crosshairs"></i> <strong>Acuerdos</strong></h4>  
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
                            <div class="col-lg-12">
                                <asp:DropDownList ID="ddlFideicomisos" OnSelectedIndexChanged="ddlFideicomisos_SelectedIndexChanged" runat="server" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>                                         
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
                                        <asp:TemplateField HeaderText="Número de Sesión" SortExpression="Año">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "NumSesion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Fecha Programada" SortExpression="Año">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "FechaProgramada")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hora Programada" SortExpression="Año">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "HoraProgramada")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status Sesión" SortExpression="Año">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Acuerdos" ItemStyle-CssClass="col-md-2" HeaderStyle-CssClass="panel-footer">
                                            <ItemTemplate>
                                                    <button type="button" id="btnAcuerdos" onserverclick="btnAcuerdos_ServerClick" runat="server" class="btn btn-default"> <span class="glyphicon glyphicon-thumbs-up"></span></button> 
                                            </ItemTemplate>                          
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />                                            
                                        </asp:TemplateField>

                                    </Columns> 
                                    <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                </asp:GridView>
                             </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" style="display:none" id="divDetalleAcuerdos">
                
                <div class="col-lg-12">
                    <div class="row" runat="server" id="divEncabezadoDetalle">
                        <div class="col-lg-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title"><i class="fa"></i>Acuerdos por sesión</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="col-lg-12">
                                        <asp:GridView ID="gridAcuerdos" OnPageIndexChanging="gridAcuerdos_PageIndexChanging" OnRowDataBound="gridAcuerdos_RowDataBound" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover"  ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="Acciones">
                                                    <ItemTemplate>
                                                        <asp:ImageButton  ID="imgBtnEdit" ToolTip="Editar" OnClick="imgBtnEdit_Click" runat="server" ImageUrl="~/img/Edit1.png" />
                                                        <asp:ImageButton  ID="imgBtnEliminar" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png" data-toggle="modal" data-target="#myModal"/>
                                                    </ItemTemplate>
                                                    <HeaderStyle BackColor="#EEEEEE" />
                                                    <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Fideicomiso" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFideicomiso" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Número de Sesión" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSesion" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Número de Acuerdo" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "NumAcuerdo")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Acuerdo" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "Notas")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                        </asp:GridView>
                                        </div>

                                    <div class="col-lg-3">
                                        <button type="button" id="btnCrearAcuerdo" onclick="fnc_CrearNuevoAcuerdo();" class="btn btn-default" value="Nuevo">Nuevo</button>
                                        <button type="button" id="btnVolver" onclick="fnc_Volver();" class="btn btn-default">Volver</button>
                                    </div>
                                
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-12">
                    
                    <div id="divMenu" style="display:none" runat="server">
                        <ul class="nav nav-tabs nav-justified panel-success">
                            <li class="active"><a id="linkAcuerdo" href="#">Acuerdo</a></li>
                            <li class="active"><a id="linkSeguimiento" href="#">Seguimientos</a></li>
                        </ul>
                    </div>
                    
                    
                    <div class="row" style="display:none" runat="server" id="divCapturaDetalle" >
                        <div class="col-lg-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title"><i class="fa"></i>Datos del Acuerdo</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="col-lg-12">
                                        <div class="form-group">
                                            <label>Número de Acuerdo:</label>
                                            <input type="text" name="prueba" runat="server" class="form-control" id="txtNumAcuerdo" />
                                        </div>
                                        <div class="form-group">
                                            <label>Acuerdo:</label>
                                            <textarea type="text" name="prueba" style="height:250px" runat="server" class="form-control" id="txtNotas" />
                                        </div>
                                        <div class="form-group">
                                            <label>Status:</label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn btn-default" ></asp:Button>
                                            <button type="button" onclick="fnc_Cancelar();" class="btn btn-default">Lista Acuerdos</button> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    
                    <div class="row" id="divSeguimiento" style="display:none" runat="server">
                        <div class="col-lg-12">
                            <div class="row" runat="server" id="divEncabezadoSeguimiento">
                                <div class="col-lg-12">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            <h3 class="panel-title"><i class="fa"></i>Seguimientos</h3>
                                        </div>
                                        <div class="panel-body">
                                            <div class="col-lg-12">
                                                <asp:GridView ID="gridSeguimientos" OnPageIndexChanging="gridSeguimientos_PageIndexChanging" OnRowDataBound="gridSeguimientos_RowDataBound"  runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover"  ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" >
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Acciones">
                                                            <ItemTemplate>
                                                                <asp:ImageButton  ID="imgBtnEditS" ToolTip="Editar" OnClick="imgBtnEditS_Click" runat="server" ImageUrl="~/img/Edit1.png" />
                                                                <asp:ImageButton  ID="imgBtnEliminarS" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png" data-toggle="modal" data-target="#modalS"/>
                                                            </ItemTemplate>
                                                            <HeaderStyle BackColor="#EEEEEE" />
                                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Descripción" SortExpression="Año">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "Descripcion")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                                </asp:GridView>
                                                </div>

                                            <div class="col-lg-3">
                                                <button type="button" id="btnCrearSeguimiento" onclick="fnc_CrearNuevoSeguimiento();" class="btn btn-default" value="Nuevo">Nuevo</button>
                                            </div>
                                
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="row" style="display:none" runat="server" id="divCapturaSeguimiento" >
                                <div class="col-lg-12">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            <h3 class="panel-title"><i class="fa"></i>Datos del Seguimiento</h3>
                                        </div>
                                        <div class="panel-body">
                                            <div class="col-lg-12">
                                                <div class="form-group">
                                                    <label>Descripción del Seguimiento</label>
                                                    <textarea type="text" name="prueba" style="height:250px" runat="server" class="form-control" id="txtDescripcion" />
                                                </div>
                                                <div class="form-group">
                                                    <asp:Button ID="btnGuardarS" OnClick="btnGuardarS_Click" runat="server" Text="Guardar" CssClass="btn btn-default" ></asp:Button>
                                                    <button type="button" onclick="fnc_CancelarS();" class="btn btn-default">Cancelar</button> 
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
        <input type="hidden" runat="server" id="_AccionS" />
        <input type="hidden" runat="server" id="_IDSesion" />
        <input type="hidden" runat="server" id="_IDAcuerdo" />
        <input type="hidden" runat="server" id="_IDSeguimiento" />
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
                <asp:Button ID="btnDel" OnClick="btnDel_Click" runat="server" CssClass="btn btn-default" Text="Aceptar"  />
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
              </div>
        
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalS" tabindex="-1" role="dialog" aria-labelledby="smallModal" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabels">Confirmación</h4>
              </div>
              <div class="modal-body">
                <h3 id="msgContenidos">¿Está seguro que desea eliminar el registro?</h3>
              </div>
              <div class="modal-footer">
                <asp:Button ID="btnDelS" OnClick="btnDelS_Click" runat="server" CssClass="btn btn-default" Text="Aceptar"  />
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
              </div>
        
            </div>
        </div>
    </div>

</asp:Content>
