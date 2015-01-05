using BL;
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
    public partial class SintesisInformativa : System.Web.UI.Page
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

        #region METODOS
        private void BindGridFichas()
        {
            int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
            gridFichas.DataSource = uow.FichaTecnicaBusinessLogic.Get(e => e.FideicomisoID == idFideicomiso).ToList();
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
            //txtDescripcion.Value = obj.Descripcion;
            txtArchivoAdjunto.Value = obj.NombreArchivo != null && !obj.NombreArchivo.Equals(string.Empty) ? obj.NombreArchivo : "No existe archivo adjunto";
            txtFideicomiso.Value = GetClaveFideicomiso();
            txtResponsable.Value = obj.ResponsableOperativo;
            txtFinalidad.Value = obj.Finalidad;
            txtCreacion.Value = obj.Creacion;
            txtFormalizacion.Value = obj.Formalizacion;
            txtPartes.Value = obj.Partes;
            txtModificaciones.Value = obj.Modificaciones;
            txtIntegracion.Value = obj.ComiteTecnico;
            txtReglas.Value = obj.ReglasOperacion;
            txtEstructura.Value = obj.EstructuraAdministrativa;
            txtCalendario.Value = obj.Calendario;
            txtPresupuesto.Value = obj.PresupuestoAnual;
            txtSituacion.Value = obj.SituacionPatrimonial;

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
            {
                BindGridFichas();
                txtFideicomiso.Value = GetClaveFideicomiso();
            }
                

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
                        select new { f.ID, f.Clave });

            ddlFideicomisos.DataSource = list;
            ddlFideicomisos.DataValueField = "ID";
            ddlFideicomisos.DataTextField = "Clave";
            ddlFideicomisos.DataBind();


        }
        #endregion

        #region EVENTOS
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

                if (ficha.NombreArchivo != null)
                    if (!ficha.NombreArchivo.Equals(string.Empty))
                        lblArchivo.Text = ficha.NombreArchivo;
                    else
                        lblArchivo.Text = "No existe archivo adjunto";
                else
                    lblArchivo.Text = "No existe archivo adjunto";


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
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }
        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDFicha.Value = gridFichas.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControlesFicha();
            divDatosFideicomiso.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divFideicomiso.Style.Add("display", "none");
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

            obj.FideicomisoID = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
            //obj.Descripcion = txtDescripcion.Value;
            obj.NombreArchivo = fileUpload.FileName.Equals(string.Empty) ? obj.NombreArchivo : Path.GetFileName(fileUpload.FileName);
            obj.TipoArchivo = fileUpload.PostedFile.ContentType;

            //Nuevos campos
            obj.ResponsableOperativo = txtResponsable.Value;
            obj.Finalidad = txtFinalidad.Value;
            obj.Creacion = txtCreacion.Value;
            obj.Formalizacion = txtFormalizacion.Value;
            obj.Partes = txtPartes.Value;
            obj.Modificaciones = txtModificaciones.Value;
            obj.ComiteTecnico = txtIntegracion.Value;
            obj.ReglasOperacion = txtReglas.Value;
            obj.EstructuraAdministrativa = txtEstructura.Value;
            obj.Calendario = txtCalendario.Value;
            obj.PresupuestoAnual = txtPresupuesto.Value;
            obj.SituacionPatrimonial = txtSituacion.Value;

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

            divDatosFideicomiso.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";
            divEncabezado.Style.Add("display", "block");
            divFideicomiso.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            BindGridFichas();
            divFideicomiso.Style.Add("display", "block");
            divDatosFideicomiso.Style.Add("display", "none");
            divCaptura.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
            _Accion.Value = string.Empty;
        }
        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridFichas();

            if (_Accion.Value.Equals("N"))
            {
                divCaptura.Style.Add("display", "block");
                divEncabezado.Style.Add("display", "none");
            }
            else
            {
                divCaptura.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
            }

            txtFideicomiso.Value = GetClaveFideicomiso();

            divFideicomiso.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";
            string nombreArchivo;
            int idFicha = Utilerias.StrToInt(_IDFicha.Value);

            FichaTecnica obj = uow.FichaTecnicaBusinessLogic.GetByID(idFicha);
            nombreArchivo = obj.NombreArchivo;


            divEncabezado.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");
            divFideicomiso.Style.Add("display", "block");

            //Se elimina el objeto
            uow.FichaTecnicaBusinessLogic.Delete(obj);
            uow.SaveChanges();

            if (uow.Errors.Count > 0) //Si hubo errores
            {
                M = string.Empty;
                foreach (string cad in uow.Errors)
                    M += cad;

                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }

            BindGridFichas();

            //Se elimina el archivo fisico
            if (nombreArchivo != null)
            {
                if (!nombreArchivo.Equals(string.Empty))
                {
                    M = EliminarArchivo(idFicha, nombreArchivo);
                    //Si hubo Errores
                    if (!M.Equals(string.Empty))
                    {
                        lblMsgError.Text = M;
                        divMsgError.Style.Add("display", "block");
                        divMsgSuccess.Style.Add("display", "none");
                        return;
                    }
                }

            }


            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
        }
        #endregion
        
    }
}