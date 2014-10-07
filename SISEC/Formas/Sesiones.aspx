<%@ Page Title="" Language="C#" MasterPageFile="~/Navegador.Master" AutoEventWireup="true" CodeBehind="Sesiones.aspx.cs" Inherits="SISEC.Formas.Sesiones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="alert alert-info alert-dismissable">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        <i class="fa fa-info-circle"></i>  <strong>Calendario de Sesiones</strong> 
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-clock-o fa-fw"></i>Seleccione Fideicomiso</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-4">
                                <asp:DropDownList ID="ddlFideicomisos" runat="server" Width="720px" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>                                         
                             </div>
                        </div>
                            
                    </div>

                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-clock-o fa-fw"></i> Detalle de sesiones</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-4">
                                <asp:GridView ID="gridSesiones" CssClass="table" runat="server" AutoGenerateColumns="false" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Descripción" SortExpression="Año">
                                            <ItemTemplate>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Programada" SortExpression="Año">
                                            <ItemTemplate>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns> 
                                    
                                    
                                </asp:GridView>
                             </div>
                        </div>
                            
                    </div>

                   
                </div>
            </div>
        </div>
    </div>

</asp:Content>
