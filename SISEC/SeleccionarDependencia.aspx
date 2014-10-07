<%@ Page Title="" Language="C#" MasterPageFile="~/SelectorEjercicio.Master" AutoEventWireup="true" CodeBehind="SeleccionarDependencia.aspx.cs" Inherits="SISEC.SeleccionarDependencia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="alert alert-info alert-dismissable">
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        <i class="fa fa-info-circle"></i>  <strong>Seleccione Dependencia y Ejercicio</strong> 
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-clock-o fa-fw"></i> Ejercicio</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-4">
                                <asp:DropDownList ID="ddlEjercicios" runat="server" Width="720px" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>                                         
                             </div>
                        </div>
                            
                    </div>

                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-clock-o fa-fw"></i> Dependencia</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-4">
                                <asp:DropDownList ID="ddlDependecia" runat="server" Width="720px" CssClass ="form-control" AutoPostBack="True"></asp:DropDownList>
                             </div>
                        </div>
                            
                    </div>

                    <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" CssClass="btn btn-default" OnClientClick="return fnc_Validar()" ></asp:Button>
                    <button type="reset" class="btn btn-default">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
