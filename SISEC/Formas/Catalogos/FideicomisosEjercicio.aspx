﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="FideicomisosEjercicio.aspx.cs" Inherits="SISEC.Formas.Catalogos.FideicomisosEjercicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function fnc_Nuevo() {
            $("#<%= divCaptura.ClientID %>").css("display", "block");
            $("#<%= divEncabezado.ClientID %>").css("display", "none");
            $("#<%= _Accion.ClientID %>").val("N");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        function fnc_Cancelar() {
            $("#<%= divCaptura.ClientID %>").css("display", "none");
            $("#<%= divEncabezado.ClientID %>").css("display", "block");
            $("#<%= _Accion.ClientID %>").val("");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

        function fnc_ColocarID(id) {
            $("#<%= _IDFideicomiso.ClientID %>").val(id);
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="alert alert-success alert-dismissable">
                        <h4><i class="fa fa-crosshairs"></i> <strong>Catálogo de Fideicomisos por Ejercicio</strong></h4>  
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="divEncabezado">
                <div class="col-lg-12">

                    <div class="panel panel-default">
                        <div class="panel-heading">
                             <h3 class="panel-title"><i class="fa"></i> Lista de Fideicomisos por Ejercicio</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-2">
                                <label>Ejercicio:</label>
                                
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEjercicioFiltro" OnSelectedIndexChanged="ddlEjercicioFiltro_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>

                            <div class="col-lg-12">
                                <div><p>&nbsp;</p></div>
                                <asp:GridView ID="gridFideicomisos" OnRowDataBound="gridFideicomisos_RowDataBound" OnPageIndexChanging="gridFideicomisos_PageIndexChanging" ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" CssClass="table table-striped table-bordered table-hover" runat="server" AutoGenerateColumns="false" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Acciones">
                                            <ItemTemplate>
                                                <asp:ImageButton  ID="imgBtnEdit" OnClick="imgBtnEdit_Click" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" />
                                                <asp:ImageButton  ID="imgBtnEliminar" ToolTip="Borrar" runat="server" ImageUrl="~/img/close.png" data-toggle="modal" data-target="#myModal"/>
                                            </ItemTemplate>
                                            <HeaderStyle BackColor="#EEEEEE" />
                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Fideicomiso asignado" SortExpression="Año">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFideicomiso" runat="server"></asp:Label> 
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ejercicio" SortExpression="Año">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEjercicio" runat="server"></asp:Label> 
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" SortExpression="Año">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActivo" Enabled="false" Checked='<%#Convert.ToBoolean(Eval("Activo")) %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns> 
                                    <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                </asp:GridView>

                                <button type="button" onclick="fnc_Nuevo();" id="btnNuevo" class="btn btn-default" value="Nuevo">Nuevo</button>
                                <asp:Button OnClick="btnImportar_Click" class="btn btn-default" ID="btnImportar" runat="server" Text="Importar fideicomisos ejercicio anterior" />

                            </div>
                            
                        </div>
                    </div>

                </div>
            </div>

            <div class="row" style="display:none" runat="server" id="divCaptura" >
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Datos del Tipo de Usuario</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label>Ejercicio:</label>
                                    <asp:DropDownList ID="ddlEjercicio" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label>Fideicomiso a asignar:</label>
                                    <asp:DropDownList ID="ddlFideicomiso" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label>Activo:</label>
                                    <asp:CheckBox ID="chkActivo" runat="server" />
                                </div>
                                <div class="form-group">
                                    <asp:Button OnClick="btnGuardar_Click" ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-default" ></asp:Button>
                                    <button type="button" onclick="fnc_Cancelar();" class="btn btn-default">Cancelar</button> 
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

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="smallModal" aria-hidden="true">
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
                <asp:Button ID="btnDel" OnClick="btnDel_Click" runat="server" CssClass="btn btn-default" Text="Aceptar"  />
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
              </div>
        
            </div>
        </div>
    </div>


    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_IDFideicomiso" />
        <input type="hidden" runat="server" id="_Accion" />
    </div>



</asp:Content>
