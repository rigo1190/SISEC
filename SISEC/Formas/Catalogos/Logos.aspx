<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Logos.aspx.cs" Inherits="SISEC.Formas.Catalogos.Logos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function fnc_NuevaImagen() {
            
            $("#<%= divEncabezado.ClientID %>").css("display", "none");
            $("#<%= divCaptura.ClientID %>").css("display", "block");
            $("#<%= _Accion.ClientID %>").val("N");
            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= txtDescripcion.ClientID %>").val("");
            $("#<%= txtArchivoAdjunto.ClientID %>").val("");
        }

        function fnc_Cancelar() {

            $("#<%= divMsgError.ClientID %>").css("display", "none");
            $("#<%= divMsgSuccess.ClientID %>").css("display", "none");
            $("#<%= divEncabezado.ClientID %>").css("display", "block");
            $("#<%= divCaptura.ClientID %>").css("display", "none");

            return false;
        }

        
        function fnc_Validar() {

            var file = document.getElementById("<%=fileUpload.ClientID%>");
            var path = file.value;
            var tiposArchivo = ["png", "jpg", "jpeg"];

            if (path == "") {
                $("#<%= divMsgError.ClientID %>").css("display", "block");
                $("#<%= lblMsgError.ClientID %>").text("No se ha cargado ningún archivo. Intente de nuevo");
            }

            var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
            var archivoValido = false;

            for (var i = 0; i < tiposArchivo.length; i++) {
                if (ext == tiposArchivo[i]) {
                    archivoValido = true;
                    break;
                }
            }

            if (!archivoValido) {
                $("#<%= divMsgError.ClientID %>").css("display", "block");
                $("#<%= lblMsgError.ClientID %>").text("La extensión del archivo no es válida. Intente con otro archivo");
            }

            return archivoValido;
    }


    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="alert alert-success alert-dismissable">
                    <h4><i class="fa fa-crosshairs"></i> <strong>Imagenes de Logos</strong></h4>
                </div>
            </div>
        </div>

        <div id="divEncabezado" runat="server" class="row">
            <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa"></i>Logos disponibles</h3>
                    </div>
                    <div class="panel-body">
                        <div class="col-lg-12">
                            <asp:GridView ID="grid" ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" CssClass="table table-striped table-bordered table-hover" runat="server" AutoGenerateColumns="false" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Acciones">
                                            <ItemTemplate>
                                                <asp:ImageButton  ID="imgBtnEdit" OnClick="imgBtnEdit_Click" ToolTip="Editar" runat="server" ImageUrl="~/img/Edit1.png" />
                                            </ItemTemplate>
                                            <HeaderStyle BackColor="#EEEEEE" />
                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" Width="50px" BackColor="#EEEEEE" />
                                        </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripción" SortExpression="Año">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Nombre Archivo" SortExpression="Año">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Nombre")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ver Imagen" SortExpression="Año">
                                        <ItemTemplate>
                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns> 
                                <PagerSettings FirstPageText="Primera" LastPageText="Ultima" Mode="NextPreviousFirstLast" NextPageText="Siguiente" PreviousPageText="Anterior" />
                            </asp:GridView>
                        </div>

                        <div class="col-lg-3">
                            <button type="button" id="btnNuevo" onclick="fnc_NuevaImagen();" class="btn btn-default" value="Nuevo">Nuevo</button>
                        </div>

                    </div>
                </div>

        </div>

        <div class="row" runat="server" id="divCaptura" style="display:none">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Datos de la Imagen</h3>
                        </div>
                         <div class="panel-body">
                            <div class="col-lg-12 ">
                                <div class="form-group">
                                    <label>Descripción</label>
                                    <textarea type="text" name="prueba" runat="server" class="form-control" id="txtDescripcion" />
                                </div>
                                <div class="form-group">
                                    <label>Imagen adjunta actual:</label>
                                    <input type="text" disabled="disabled" name="prueba" id="txtArchivoAdjunto" runat="server" class="form-control"  />
                                </div>
                                <div class="form-group">
                                    <label>Imagen:</label>
                                    <asp:FileUpload ID="fileUpload" runat="server" />
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnGuardar" OnClientClick="return fnc_Validar();" OnClick="btnGuardar_Click" runat="server" Text="Guardar" CssClass="btn btn-default" ></asp:Button>
                                    <asp:Button ID="btnCancelar" OnClientClick="return fnc_Cancelar();" runat="server" Text="Cancelar" CssClass="btn btn-default" ></asp:Button>
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
    
     <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_Accion" />
        <input type="hidden" runat="server" id="_IDLogo" />
    </div>


</asp:Content>
