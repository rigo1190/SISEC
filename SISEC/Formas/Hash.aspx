<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Hash.aspx.cs" Inherits="SISEC.Formas.Hash" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="alert alert-success alert-dismissable">
                        <h4><i class="fa fa-crosshairs"></i> <strong>Hash</strong></h4>  
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="divCaptura" >
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa"></i>Desencriptación</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label>Login:</label>
                                    <input type="text" name="prueba" runat="server" class="form-control" id="txtLogin" />
                                </div>
                                <div class="form-group">
                                    <label>Password Encriptado:</label>
                                    <input type="text" name="prueba" runat="server" class="form-control" id="txtPasswordE" />
                                </div>
                                <div class="form-group">
                                    <label>Pasword Desencriptado:</label>
                                    <input type="text" name="prueba" runat="server" class="form-control" id="txtPasswordD" />
                                </div>
                                
                                <div class="form-group">
                                    <asp:Button OnClick="btnDesencriptar_Click" ID="btnDesencriptar" runat="server" Text="Desencriptar" CssClass="btn btn-default" ></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
