<%@ Page Title="" Language="C#" MasterPageFile="~/Navegador.Master" AutoEventWireup="true" CodeBehind="SintesisInformativa.aspx.cs" Inherits="SISEC.Reports.SintesisInformativa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function fnc_AbrirReporte(caller) {

            var izq = (screen.width - 750) / 2
            var sup = (screen.height - 600) / 2
            var param = "";
            param = fnc_ArmarParamentros();
            url = $("#<%= _URL.ClientID %>").val();
            var argumentos = "?c=" + caller + param;
            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);

            //if (param != "") {
                
            //}
        }


        function fnc_ArmarParamentros() {
            var p = "";
            var msg = "";
            return p;

        }

        function fnc_CrearHistorico(idSintesis) {
            PageMethods.CrearSintesisHistorico(idSintesis, fnc_EjecutarReporte)
        }

        function fnc_EjecutarReporte(response){
            fnc_AbrirReporte(6);
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>
    <div id="page-wrapper">
        <div class="container-fluid">
            
            <div class="row">
                <div class="alert alert-success alert-dismissable">
                    <h4><i class="fa fa-crosshairs"></i> <strong>Consulta de Síntesis Informativa</strong></h4>
                      
                </div>
            </div>


            <div class="row" runat="server" id="divFiltros">
                <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Filtros</h3>
                        </div>

                        <div class="panel-body">
                            
                            <div class="row col-lg-12">
                                <div class="form-group">
                                    <label>Fideicomiso:</label>
                                    <asp:DropDownList ID="ddlFideicomisos" OnSelectedIndexChanged="ddlFideicomisos_SelectedIndexChanged" runat="server" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>                                         
                                </div>
                            </div>

                           <%-- <div class="form-group">
                                <asp:Button ID="btnConsulta" OnClick="btnConsulta_Click" CssClass="btn btn-default" runat="server" Text="Consultar" />
                            </div>--%>

                        </div>

                    </div>
            </div>
            

            <div id="divResultado" runat="server" class="row">
                
                 <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="col-md-11">
                            <h3 class="panel-title"><i class="fa"></i><asp:Label runat="server" ID="lblResultado"></asp:Label></h3>
                        </div>
                        <button type="button" runat="server" onclick="fnc_AbrirReporte(2)" id="btnVer"><span class="glyphicon glyphicon-print"></span></button>
                    </div>
                    <div class="panel-body">
                        <div class="col-lg-12">
                            <asp:GridView ID="gridSintesis" OnPageIndexChanging="gridSintesis_PageIndexChanging" OnRowDataBound="gridSintesis_RowDataBound" ShowHeaderWhenEmpty="true" DataKeyNames="ID,FideicomisoID" AllowPaging="true" CssClass="table table-striped table-bordered table-hover" runat="server" AutoGenerateColumns="false" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Nombre Fideicomiso" SortExpression="Año">
                                        <ItemTemplate>
                                            <asp:Label runat="server" id="lblFideicomiso"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Responsable Operativo" SortExpression="Año">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "ResponsableOperativo")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Finalidad del Fideicomiso" SortExpression="Año">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Finalidad")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Consultar Histórico Síntesis" SortExpression="Año">
                                        <ItemTemplate>
                                            <button type="button" runat="server" id="btnVerHistorico"><span class="glyphicon glyphicon-print"></span></button>
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
    </div>


    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_IDCalendario" />
        <input type="hidden" runat="server" id="_IDFideicomiso" />
        <input type="hidden" runat="server" id="_Consultado" />
        <input type="hidden" runat="server" id="_Ejercicio" />
        <input type="hidden" runat="server" id="_URL" />
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
</asp:Content>
