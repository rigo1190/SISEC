<%@ Page Title="" Language="C#" MasterPageFile="~/SelectorEjercicio.Master" AutoEventWireup="true" CodeBehind="SeleccionarEjercicio.aspx.cs" Inherits="SISEC.SeleccionarEjercicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function fnc_Mensaje() {
            
            jAlert('El ejercicio que seleccionó ya ha sido cerrado. Solo se podrá consultar la información', 'Ejercicio Cerrado', function (r) {
                if (r) {
                    var ruta = $("#<%=_Ruta.ClientID %>").val();
                    document.location = (ruta);
                }
            });
 
        }

    </script>
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

    <div runat="server" style="display:none">
        <input type="hidden" runat="server" id="_Ruta" />
    </div>
    
    <div >
        <fieldset>
			<legend>Alert</legend>
			<p>
				<input runat="server" id="alert_button" type="button" value="Show Alert" />
			</p>
		</fieldset>
    </div>

</asp:Content>
