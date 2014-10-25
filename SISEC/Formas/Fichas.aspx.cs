﻿using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SISEC.Formas
{
    public partial class Fichas : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();

            if (!IsPostBack)
            {

                BindDropDownFideicomisos();
                CargarGridPrimerFideicomiso();

            }
        }
        private void BindGridFichas()
        {
            int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
            gridFichas.DataSource = uow.FichaTecnicaBusinessLogic.Get(e => e.DependenciaFideicomisoEjercicioID == idFideicomiso).ToList();
            gridFichas.DataBind();
        }
        private string GetClaveFideicomiso()
        {
            DependenciaFideicomisoEjercicio ente = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(Utilerias.StrToInt(ddlFideicomisos.SelectedValue));
            Fideicomiso fidei = uow.FideicomisoBusinessLogic.GetByID(ente.FideicomisoID);
            return fidei.Clave;
        }
        private void BindControlesFicha()
        {
            int id = Utilerias.StrToInt(_IDFicha.Value);
            FichaTecnica obj = uow.FichaTecnicaBusinessLogic.GetByID(id);
            txtDescripcion.Value = obj.Descripcion;
            txtArchivoAdjunto.Value = obj.NombreArchivo != null && !obj.NombreArchivo.Equals(string.Empty) ? obj.NombreArchivo : "No existe archivo adjunto";
        }
        public string GuardarArchivo(HttpPostedFile postedFile, int idFicha)
        {

            string M = string.Empty;
            string ruta = string.Empty;

            try
            {
                ruta = System.Configuration.ConfigurationManager.AppSettings["ArchivosFichas"];

                if (!ruta.EndsWith("/"))
                    ruta += "/";

                ruta += idFicha.ToString() + "/";

                if (ruta.StartsWith("~") || ruta.StartsWith("/"))   //Es una ruta relativa al sitio
                    ruta = Server.MapPath(ruta);


                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                ruta += postedFile.FileName;

                postedFile.SaveAs(ruta);

            }
            catch (Exception ex)
            {
                M = ex.Message;
            }

            return M;

        }
        private string EliminarArchivo(int id, string nombreArchivo)
        {
            string M = string.Empty;
            try
            {
                string ruta = string.Empty;

                //eliminar archivo
                ruta = System.Configuration.ConfigurationManager.AppSettings["ArchivosFichas"];

                if (!ruta.EndsWith("/"))
                    ruta += "/";

                ruta += id.ToString() + "/";

                if (ruta.StartsWith("~") || ruta.StartsWith("/"))   //Es una ruta relativa al sitio
                    ruta = Server.MapPath(ruta);

                File.Delete(ruta + "\\" + nombreArchivo);
                Directory.Delete(ruta);

            }
            catch (Exception ex)
            {
                M = ex.Message;
            }


            return M;
        }
        private void CargarGridPrimerFideicomiso()
        {
            if (ddlFideicomisos.Items.Count > 0)
                BindGridFichas();

        }
        private void BindDropDownFideicomisos()
        {
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idUser = Utilerias.StrToInt(Session["UserID"].ToString());

            var list = (from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == idEjercicio)
                        join ud in uow.UsuarioFideicomisoBusinessLogic.Get(e => e.UsuarioID == idUser)
                        on df.ID equals ud.DependenciaFideicomisoEjercicioID
                        join f in uow.FideicomisoBusinessLogic.Get()
                        on df.FideicomisoID equals f.ID
                        select new { df.ID, f.Clave });

            ddlFideicomisos.DataSource = list;
            ddlFideicomisos.DataValueField = "ID";
            ddlFideicomisos.DataTextField = "Clave";
            ddlFideicomisos.DataBind();


        }
        protected void gridFichas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");
                Label lblFideicomiso = (Label)e.Row.FindControl("lblFideicomiso");
                Label lblArchivo = (Label)e.Row.FindControl("lblArchivo");

                int idFicha = Utilerias.StrToInt(gridFichas.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                DAL.Model.FichaTecnica ficha = uow.FichaTecnicaBusinessLogic.GetByID(idFicha);

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarIDFicha(" + idFicha + ")");

                lblFideicomiso.Text = GetClaveFideicomiso();
                lblArchivo.Text = ficha.NombreArchivo != null && !ficha.NombreArchivo.Equals(string.Empty) ? ficha.NombreArchivo : "No existe archivo adjunto";

                //Se coloca la fucnion a corespondiente para visualizar el DOCUMENTO ADJUNTO 
                HtmlButton btnVer = (HtmlButton)e.Row.FindControl("btnVer");
                string ruta = ResolveClientUrl("~/AbrirDocto.aspx");
                btnVer.Attributes["onclick"] = "fnc_AbrirArchivo('" + ruta + "'," + idFicha + "," + 4 + ")";
            }
        }
        protected void gridFichas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridFichas.PageIndex = e.NewPageIndex;
            BindGridFichas();
            divEncabezado.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");
        }
        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDFicha.Value = gridFichas.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControlesFicha();
            divEncabezado.Style.Add("display", "none");
            divCaptura.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int idFicha = Utilerias.StrToInt(_IDFicha.Value);
            string nomAnterior = string.Empty;

            string M = string.Empty;
            FichaTecnica obj = null;


            if (_Accion.Value.Equals("N"))
                obj = new FichaTecnica();
            else
                obj = uow.FichaTecnicaBusinessLogic.GetByID(idFicha);

            nomAnterior = obj.NombreArchivo;

            obj.Descripcion = txtDescripcion.Value;
            obj.NombreArchivo = fileUpload.FileName.Equals(string.Empty) ? obj.NombreArchivo : Path.GetFileName(fileUpload.FileName);
            obj.TipoArchivo = fileUpload.PostedFile.ContentType;

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                
                uow.FichaTecnicaBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();

                if (nomAnterior != null)
                {
                    if (!nomAnterior.Equals(obj.NombreArchivo))  //Se elimina el archivo anterior
                        if (!nomAnterior.Equals(string.Empty))
                            M = EliminarArchivo(obj.ID, nomAnterior);
                }

                uow.FichaTecnicaBusinessLogic.Update(obj);
            }

            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                foreach (string err in uow.Errors)
                    M += err;

                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = M;
                return;
            }

            _IDFicha.Value = obj.ID.ToString(); //Se coloca el ID del nuevo objeto creado

            //Se almacena el archivo
            if (!fileUpload.PostedFile.FileName.Equals(string.Empty))
            {
                if (fileUpload.FileBytes.Length > 10485296)
                {
                    lblMsgError.Text = "Se ha excedido en el tamaño del archivo, el máximo permitido es de 10 Mb";
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");

                    return;
                }

                M = GuardarArchivo(fileUpload.PostedFile, obj.ID);

                if (!M.Equals(string.Empty))
                {

                    //MANEJAR EL ERROR
                    lblMsgError.Text = M;
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");
                    lblMsgError.Text = M;
                    return;
                }
            }

            BindGridFichas();

            _Accion.Value = string.Empty;

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";
            divEncabezado.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");

        }
    }
}