<%@ Page Title="" Language="C#" MasterPageFile="~/Navegador.Master" AutoEventWireup="true" CodeBehind="Acuerdos.aspx.cs" Inherits="SISEC.Reports.Acuerdos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        
        function fnc_AbrirReporte() {

            var izq = (screen.width - 750) / 2
            var sup = (screen.height - 600) / 2
            var param = "";
            param = fnc_ArmarParamentros();
            url = $("#<%= _URL.ClientID %>").val();
            var argumentos = "?c=" + 1 + param;
            url += argumentos;
            window.open(url, 'pmgw', 'toolbar=no,status=no,scrollbars=yes,resizable=yes,directories=no,location=no,menubar=no,width=750,height=500,top=' + sup + ',left=' + izq);
        }


        function fnc_ArmarParamentros() {
            var p = "";

            var fideicomiso=$("#<%= ddlFideicomisos.ClientID %>").val();
            var ejercicio = $("#<%= _Ejercicio.ClientID %>").val();
            var status=$("#<%= ddlStatusAcuerdo.ClientID %>").val();
            var sesion = $("#<%= ddlSesiones.ClientID %>").val();

            if (sesion == null || sesion == undefined)
                sesion = 0;

            p += "&p=" + ejercicio;
            p += "-" + fideicomiso;
            p += "-" + status;
            p += "-" + sesion;

            return p;

        }





    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="alert alert-success alert-dismissable">
                    <h4><i class="fa fa-crosshairs"></i> <strong>Consulta de Acuerdos</strong></h4>
                      
                </div>
            </div>

            <div class="row" runat="server" id="divFiltros">
                <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Filtros</h3>
                        </div>

                        <div class="panel-body">
                            
                                <div class="row col-lg-6">
                                    <div class="form-group">
                                        <label>Fideicomiso:</label>
                                        <asp:DropDownList ID="ddlFideicomisos" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFideicomisos_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>                                         
                                    </div>
                                    <div class="form-group">
                                        <label>Núm. de Sesión:</label>
                                        <asp:DropDownList ID="ddlSesiones" runat="server" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>                                         
                                    </div>
                                </div>
                                <div class="row col-lg-6">
                                    <div class="form-group">
                                        <label>Núm. de Acuerdo:</label>
                                        <input class="form-control" runat="server" id="txtNumAcuredo"/>
                                    </div>
                                    <div class="form-group">
                                        <label>Status Acuerdo:</label>
                                        <asp:DropDownList ID="ddlStatusAcuerdo" runat="server" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>                                         
                                    </div>
                                </div>
                           
                            <div class="form-group">
                                <asp:Button ID="btnConsulta" CssClass="btn btn-default" runat="server" Text="Consultar" OnClick="btnConsulta_Click" />
                            </div>

                        </div>

                    </div>
            </div>

            <div id="divResultado" runat="server" class="row">
                
                 <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa"></i><asp:Label runat="server" ID="lblResultado"></asp:Label></h3>
                        <button type="button" runat="server" onclick="fnc_AbrirReporte()" id="btnVer"><span class="glyphicon glyphicon-print"></span></button>
                    </div>
                    <div class="panel-body">
                        <div class="col-lg-12">
                            <asp:GridView ID="gridAcuerdos" OnRowDataBound="gridAcuerdos_RowDataBound" OnPageIndexChanging="gridAcuerdos_PageIndexChanging" ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" CssClass="table table-striped table-bordered table-hover" runat="server" AutoGenerateColumns="false" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Número de Sesión" SortExpression="Año">
                                        <ItemTemplate>
                                            <asp:Label runat="server" id="lblFideicomiso"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Núm. Sesión" SortExpression="Año">
                                        <ItemTemplate>
                                            <asp:Label runat="server" id="lblSesion"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status Acuerdo" SortExpression="Año">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Núm. de Acuerdo" SortExpression="Año">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "NumAcuerdo")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Acuerdos" SortExpression="Año">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Notas")%>
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
        <input type="hidden" runat="server" id="_Consultado" />
         <input type="hidden" runat="server" id="_Ejercicio" />
         <input type="hidden" runat="server" id="_URL" />
    </div>


</asp:Content>
