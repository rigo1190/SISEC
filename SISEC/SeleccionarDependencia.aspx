<%@ Page Title="" Language="C#" MasterPageFile="~/SelectorEjercicio.Master" AutoEventWireup="true" CodeBehind="SeleccionarDependencia.aspx.cs" Inherits="SISEC.SeleccionarDependencia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container col-md-offset-4 col-md-4">
             

        <div class="panel panel-success">

            <div class="panel-heading">
                <h3 class="panel-title">Seleccione el ejercicio y la unidad presupuestal</h3>
            </div>

            <div class="panel-body">
                <div class="form-group">
                   <label for="ddlEjercicios" class="control-label" runat="server">Ejercicios:</label>
                    <div>
                        <asp:DropDownList ID="ddlEjercicios" runat="server" Width="400px" CssClass="form-control" AutoPostBack="False"></asp:DropDownList>                                         
                    </div>
                 </div>
                                 

                <div class="form-group">
                    <label for="ddlUnidadPresupuestal" class="control-label" runat="server">Unidad presupuestal:</label>
                    <div>
                        <asp:DropDownList ID="ddlDependecia" runat="server" Width="400px" CssClass ="form-control" AutoPostBack="False"></asp:DropDownList>
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
