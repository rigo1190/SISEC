<%@ Page Title="" Language="C#" MasterPageFile="~/SelectorEjercicio.Master" AutoEventWireup="true" CodeBehind="SeleccionarEjercicio.aspx.cs" Inherits="SISEC.SeleccionarEjercicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container col-md-offset-4 col-md-4">
             

        <div class="panel panel-success">

            <div class="panel-heading">
                <h3 class="panel-title">Seleccione ejercicio</h3>
            </div>

            <div class="panel-body">
                <div class="form-group">
                   <label for="ddlEjercicios" class="control-label" runat="server">Ejercicios:</label>
                    <div class="col-lg-12">
                        <asp:DropDownList ID="ddlEjercicios" runat="server" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>                                         
                    </div>
                 </div>
            </div>

            <div class="panel-footer clearfix">

                <div class="pull-right">                
                    <asp:Button ID="btnSeleccionar" runat="server" OnClick="btnSeleccionar_Click" Text="Seleccionar" CssClass="btn btn-default" ></asp:Button>
                    <button type="reset" class="btn btn-default">Cancelar</button>
                </div>

            </div>
           
    </div>
    
</asp:Content>
