<%@ Page Title="" Language="C#" MasterPageFile="~/Navegador.Master" AutoEventWireup="true" CodeBehind="Fichas.aspx.cs" Inherits="SISEC.Formas.Fichas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function fnc_NuevaFicha() {
            $("#<%= divGrid.ClientID %>").css("display", "none");
            $("#<%= divCaptura.ClientID %>").css("display", "block");
            $("#<%= _Accion.ClientID %>").val("N");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= divDatosFideicomiso.ClientID %>").css("display", "none");
            $("#<%= txtDescripcion.ClientID %>").val("");
            $("#<%= txtArchivoAdjunto.ClientID %>").val("");
        }

        function fnc_AbrirArchivo(ruta, id, caller) {
            window.open(ruta + '?i=' + id + '&c=' + caller, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,menubar=no,width=750,height=700,top=0');
        }

        function fnc_ColocarIDFicha(id) {
            $("#<%= _IDFicha.ClientID %>").val(id);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="alert alert-success alert-dismissable">
                        <h4><i class="fa fa-crosshairs"></i> <strong>Fichas Técnicas</strong></h4>  
                    </div>
                </div>
            </div>

            <div class="row">
                 <div class="col-md-12">
                    <div id="divFideicomiso" runat="server" class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Fideicomiso</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-12">
                                <asp:DropDownList ID="ddlFideicomisos" OnSelectedIndexChanged="ddlFideicomisos_SelectedIndexChanged"  runat="server" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>                                         
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="divEncabezado">
                 <div class="col-md-12">
                    <div id="divGrid" runat="server" class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i> Lista de Fichas Técnicas</h3>
                        </div>
                        
                        <div class="panel-body">
                            <div class="col-md-12">
                                <asp:GridView ID="gridFichas" OnPageIndexChanging="gridFichas_PageIndexChanging" OnRowDataBound="gridFichas_RowDataBound" ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" CssClass="table" runat="server" AutoGenerateColumns="false" >
                                    <Columns>
                                         <asp:TemplateField HeaderText="Acciones">
                                            <ItemTemplate>
                                                <asp:ImageButton  ID="imgBtnEdit" OnClick="imgBtnEdit_Click" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" />
                                                <asp:ImageButton  ID="imgBtnEliminar" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png" data-toggle="modal" data-target="#myModal"/>
                                            </ItemTemplate>
                                            <HeaderStyle BackColor="#EEEEEE" />
                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Fideicomiso" SortExpression="Año">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblFideicomiso"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Descripción" SortExpression="Año">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Archivo adjunto" SortExpression="Año">
                                            <ItemTemplate>
                                                <asp:Label ID="lblArchivo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ver Archivo" SortExpression="Año">
                                            <ItemTemplate>
                                                <button type="button" runat="server" id="btnVer"><span class="glyphicon glyphicon-search"></span></button>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                        </asp:TemplateField>
                                        
                                    </Columns> 
                                    
                                    <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                </asp:GridView>
                             </div>

                             <div class="col-md-3">
                                <button type="button" id="btnNuevo" onclick="fnc_NuevaFicha();" class="btn btn-default" value="Nuevo">Nuevo</button>
                            </div>

                        </div>
                            
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="divCaptura" style="display:none">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-fw"></i>Datos de la Ficha Técnica</h3>
                        </div>
                         <div class="panel-body">
                            <div class="col-md-12 ">
                                <div class="form-group">
                                    <div id="divDatosFideicomiso" runat="server">
                                        <label>Fideicomiso:</label>
                                        <input type="text" disabled="disabled" name="prueba" id="txtFideicomiso" runat="server" class="form-control"  />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label>Descripción</label>
                                    <textarea type="text" name="prueba" runat="server" class="form-control" id="txtDescripcion" />
                                </div>
                                <div class="form-group">
                                    <label>Archivo adjunto actual:</label>
                                    <input type="text" disabled="disabled" name="prueba" id="txtArchivoAdjunto" runat="server" class="form-control"  />
                                </div>
                                <div class="form-group">
                                    <label>Archivo Normatividad:</label>
                                    <asp:FileUpload ID="fileUpload" runat="server" />
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" CssClass="btn btn-default" ></asp:Button>
                                    <asp:Button ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" Text="Cancelar" CssClass="btn btn-default" ></asp:Button>
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
        <input type="hidden" runat="server" id="_Accion" />
        <input type="hidden" runat="server" id="_IDFicha" />
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
</asp:Content>
