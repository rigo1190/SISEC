<%@ Page Title="" Language="C#" MasterPageFile="~/Navegador.Master" AutoEventWireup="true" CodeBehind="NotasActas.aspx.cs" Inherits="SISEC.Formas.NotasActas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#linkNotas").click(function () {
                $("#<%= divNotas.ClientID %>").css("display", "block");
                $("#<%= divActas.ClientID %>").css("display", "none");
                $("#<%= divMsgError.ClientID %>").css("display", "none");
                $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
                $("#<%= divEncabezadoNotas.ClientID %>").css("display", "none");
                $("#<%= divCapturaNotas.ClientID %>").css("display", "block");

                if ($("#<%= _AccionN.ClientID %>").val() == "") {
                    $("#<%= divEncabezadoNotas.ClientID %>").css("display", "block");
                    $("#<%= divCapturaNotas.ClientID %>").css("display", "none");
                }

            });

            $("#linkActas").click(function () {
                $("#<%= divNotas.ClientID %>").css("display", "none");
                $("#<%= divActas.ClientID %>").css("display", "block");
                $("#<%= divMsgError.ClientID %>").css("display", "none");
                $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
                $("#<%= divEncabezadoActas.ClientID %>").css("display", "none");
                $("#<%= divCapturaActas.ClientID %>").css("display", "block");

                if ($("#<%= _AccionA.ClientID %>").val() == "") {
                    $("#<%= divEncabezadoActas.ClientID %>").css("display", "block");
                    $("#<%= divCapturaActas.ClientID %>").css("display", "none");
                }
            });

        });

        function fnc_CrearNuevaActa() {
            $("#<%= divEncabezadoActas.ClientID %>").css("display", "none");
            $("#<%= divCapturaActas.ClientID %>").css("display", "block");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= txtDescripcionA.ClientID %>").val("");
            $("#<%= _AccionA.ClientID %>").val("N");
        }

        function fnc_CrearNuevaNota() {
            $("#<%= divEncabezadoNotas.ClientID %>").css("display", "none");
            $("#<%= divCapturaNotas.ClientID %>").css("display", "block");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= txtDescripcionN.ClientID %>").val("");
            $("#<%= _AccionN.ClientID %>").val("N");
        }

        function fnc_CancelarN() {
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= divEncabezadoNotas.ClientID %>").css("display", "block");
            $("#<%= divCapturaNotas.ClientID %>").css("display", "none");
            $("#<%= _AccionN.ClientID %>").val("");
        }

        function fnc_CancelarA() {
            $("#<%= divEncabezadoActas.ClientID %>").css("display", "block");
            $("#<%= divCapturaActas.ClientID %>").css("display", "none");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= _AccionA.ClientID %>").val("");
        }

        function fnc_Volver() {
            $("#<%= divEncabezado.ClientID %>").css("display", "block");
            $("#<%= divNotasActas.ClientID %>").css("display", "none");
        }

        function fnc_ColocarIDNota(idNota) {
            $("#<%= _IDNota.ClientID %>").val(idNota);
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        function fnc_ColocarIDActa(idActa) {
            $("#<%= _IDActa.ClientID %>").val(idActa);
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        function fnc_AbrirArchivo(ruta, id, caller) {

            var izq = (screen.width - 750) / 2
            var sup = (screen.height - 600) / 2

            window.open(ruta + '?i=' + id + '&c=' + caller, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);

        }

     </script>   
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="alert alert-success alert-dismissable">
                <h4><i class="fa fa-crosshairs"></i> <strong>Notas y Actas</strong></h4>  
            </div>
        </div>
        <div class="row">
            <div class="alert alert-warning" runat="server" id="divAlerta" style="display:none">
                <asp:Label ID="lblAlerta" EnableViewState="false" runat="server" Text="Alerta" CssClass="font-weight:bold"></asp:Label>
            </div>
        </div>

        <div class="row" runat="server" id="divEncabezado">
               
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
                            <div class="col-md-12">
                                <asp:GridView ID="gridSesiones" OnRowDataBound="gridSesiones_RowDataBound" OnPageIndexChanging="gridSesiones_PageIndexChanging" ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" CssClass="table table-striped table-bordered table-hover" runat="server" AutoGenerateColumns="false" >
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

                                        <asp:TemplateField HeaderText="Notas y Actas" >
                                            <ItemTemplate>
                                                    <button type="button" id="btnNotas" onserverclick="btnNotas_ServerClick" runat="server" class="btn btn-default"> <span class="glyphicon glyphicon-file"></span></button> 
                                            </ItemTemplate>                          
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>                                            
                                        </asp:TemplateField>

                                    </Columns> 
                                    <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                </asp:GridView>
                             </div>
                        </div>
                    </div>
                
            </div>

        <div class="row" runat="server" id="divNotasActas">
            <div class="col-md-12">
                
                <div class="row" id="divMenu" style="display:none" runat="server">
                    <ul class="nav nav-tabs nav-justified panel-success">
                        <li class="active"><a id="linkNotas">Notas</a></li>
                        <li class="active"><a id="linkActas">Actas</a></li>
                    </ul>
                </div>
            
                <div class="row" style="display:none" runat="server" id="divNotas" >

                    <div id="divEncabezadoNotas" class="row" runat="server">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title"><i class="fa"></i>Notas</h3>
                                </div>

                                <div class="panel-body">
                                    <div class="col-md-12">
                                        <asp:GridView AutoGenerateColumns="false" OnPageIndexChanging="gridNotas_PageIndexChanging" OnRowDataBound="gridNotas_RowDataBound" CssClass="table table-striped table-bordered table-hover"  ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" runat="server" ID="gridNotas">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Acciones">
                                                    <ItemTemplate>
                                                        <asp:ImageButton  ID="imgBtnEditN" ToolTip="Editar" runat="server" OnClick="imgBtnEditN_Click" ImageUrl="~/img/Edit1.png" />
                                                        <asp:ImageButton  ID="imgBtnEliminarN" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png" data-toggle="modal" data-target="#myModalN"/>
                                                    </ItemTemplate>
                                                    <HeaderStyle BackColor="#EEEEEE" />
                                                    <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Fideicomiso" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFideicomisoN" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Número de Sesión" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSesionN" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Descripción Nota" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "Descripcion")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Archivo adjunto" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblArchivoN" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Ver Archivo" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <button type="button" runat="server" id="btnVerN"><span class="glyphicon glyphicon-search"></span></button>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>     
                                                </asp:TemplateField>

                                            </Columns>
                                            <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                        </asp:GridView>

                                    </div>
                                    <button type="button" runat="server" id="btnCrearNota" onclick="fnc_CrearNuevaNota();" class="btn btn-default" value="Nuevo">Nuevo</button>
                                    <button type="button" id="btnVolverN" onclick="fnc_Volver();" class="btn btn-default">Volver</button>
                                </div>

                            </div>
                        </div>
                    
                    </div>

                    <div id="divCapturaNotas" class="row" runat="server" style="display:none">
                         <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                <h3 class="panel-title"><i class="fa"></i>Datos de la Nota</h3>
                            </div>
                                <div class="panel-body">
                                    <div class="col-md-12 ">

                                        <div class="form-group">
                                            <label>Fideicomiso</label>
                                            <input type="text" disabled="disabled" name="prueba" id="txtFideicomisoN" runat="server" class="form-control"  />
                                        </div>

                                        <div class="form-group">
                                            <label>Número de sesión</label>
                                            <input type="text" disabled="disabled" name="prueba" id="txtNumeroSesionN" runat="server" class="form-control"  />
                                        </div>

                                        <div class="form-group">
                                            <label>Descripción</label>
                                            <textarea type="text" name="prueba" runat="server" class="form-control" id="txtDescripcionN" />
                                        </div>
                                        <div class="form-group">
                                            <label>Archivo adjunto actual:</label>
                                            <input type="text" disabled="disabled" name="prueba" id="txtArchivoAdjuntoN" runat="server" class="form-control"  />
                                        </div>

                                        <div class="form-group">
                                            <label>Archivo Nota:</label>
                                            <asp:FileUpload ID="fileUploadN" runat="server" />
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnGuardarN" OnClick="btnGuardarN_Click" runat="server" Text="Guardar" CssClass="btn btn-default" ></asp:Button>
                                            <button type="button" onclick="fnc_CancelarN();" class="btn btn-default">Cancelar</button> 
                                        </div>
                                
                                    </div>
                           
                                 </div>
                            </div>
                        </div>
                    </div>


                </div>

                <div class="row" style="display:none" runat="server" id="divActas" >

                    <div id="divEncabezadoActas" class="row" runat="server">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                            <div class="panel-heading">
                                <h3 class="panel-title"><i class="fa"></i>Actas</h3>
                            </div>

                            <div class="panel-body">
                                <div class="col-md-12">
                                    <asp:GridView AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gridActas_RowDataBound" OnPageIndexChanging="gridActas_PageIndexChanging"  ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" runat="server" ID="gridActas">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Acciones">
                                                <ItemTemplate>
                                                    <asp:ImageButton  ID="imgBtnEditA" OnClick="imgBtnEditA_Click" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" />
                                                    <asp:ImageButton  ID="imgBtnEliminarA" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png" data-toggle="modal" data-target="#myModalA"/>
                                                </ItemTemplate>
                                                <HeaderStyle BackColor="#EEEEEE" />
                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                            </asp:TemplateField>
                                                
                                            <asp:TemplateField HeaderText="Fideicomiso" SortExpression="Año">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFideicomisoA" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Número de Sesión" SortExpression="Año">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSesionA" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Descripción Acta" SortExpression="Año">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Descripcion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Archivo adjunto" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblArchivoA" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Ver Archivo" SortExpression="Año">
                                                    <ItemTemplate>
                                                        <button type="button" runat="server" id="btnVerA"><span class="glyphicon glyphicon-search"></span></button>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>    
                                                </asp:TemplateField>

                                        </Columns>
                                        <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                    </asp:GridView>

                                </div>
                                 
                                <button type="button" runat="server" id="btnCrearActa" onclick="fnc_CrearNuevaActa();" class="btn btn-default" value="Nuevo">Nuevo</button>
                                <button type="button" id="btnVolverA" onclick="fnc_Volver();" class="btn btn-default">Volver</button>
                            </div>

                        </div>
                        </div>
                    
                    </div>

                    <div id="divCapturaActas" style="display:none" runat="server" class="row">

                         <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                <h3 class="panel-title"><i class="fa"></i>Datos de la Acta</h3>
                            </div>
                                <div class="panel-body">
                                    <div class="col-md-12 ">

                                        <div class="form-group">
                                            <label>Fideicomiso</label>
                                            <input type="text" disabled="disabled" name="prueba" id="txtFideicomisoA" runat="server" class="form-control"  />
                                        </div>

                                        <div class="form-group">
                                            <label>Número de sesión</label>
                                            <input type="text" disabled="disabled" name="prueba" id="txtNumeroSesionA" runat="server" class="form-control"  />
                                        </div>
                                        <div class="form-group">
                                            <label>Descripción</label>
                                            <textarea type="text" name="prueba" runat="server" class="form-control" id="txtDescripcionA" />
                                        </div>

                                        <div class="form-group">
                                            <label>Archivo adjunto actual:</label>
                                            <input type="text" disabled="disabled" name="prueba" id="txtArchivoAdjuntoA" runat="server" class="form-control"  />
                                        </div>

                                        <div class="form-group">
                                            <label>Archivo Nota:</label>
                                            <asp:FileUpload ID="fileUploadA" runat="server" />
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnGuardarA" OnClick="btnGuardarA_Click" runat="server" Text="Guardar" CssClass="btn btn-default" ></asp:Button>
                                            <button type="button" onclick="fnc_CancelarA();" class="btn btn-default">Cancelar</button> 
                                        </div>
                                
                                    </div>
                           
                                 </div>
                            </div>
                        </div>
                    
                    </div>

                </div>
            
            </div>

            <div class="row">
                <div class="col-md-12">
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
        <input type="hidden" runat="server" id="_AccionN" />
        <input type="hidden" runat="server" id="_AccionA" />
        <input type="hidden" runat="server" id="_IDSesion" />
        <input type="hidden" runat="server" id="_IDActa" />
        <input type="hidden" runat="server" id="_IDNota" />
    </div>


    <div class="modal fade" id="myModalN" tabindex="-1" role="dialog" aria-labelledby="smallModal" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabelN">Confirmación</h4>
              </div>
              <div class="modal-body">
                <h3 id="msgContenidoN">¿Está seguro que desea eliminar el registro?</h3>
              </div>
              <div class="modal-footer">
                <asp:Button ID="btnDelN" OnClick="btnDelN_Click" runat="server" CssClass="btn btn-default" Text="Aceptar"  />
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
              </div>
        
            </div>
        </div>
    </div>

    <div class="modal fade" id="myModalA" tabindex="-1" role="dialog" aria-labelledby="smallModal" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabelA">Confirmación</h4>
              </div>
              <div class="modal-body">
                <h3 id="msgContenidoA">¿Está seguro que desea eliminar el registro?</h3>
              </div>
              <div class="modal-footer">
                <asp:Button ID="btnDelA" OnClick="btnDelA_Click" runat="server" CssClass="btn btn-default" Text="Aceptar"  />
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
              </div>
        
            </div>
        </div>
    </div>

</asp:Content>
