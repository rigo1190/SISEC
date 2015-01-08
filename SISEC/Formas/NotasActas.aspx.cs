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
    public partial class NotasActas : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();

            if (!IsPostBack) 
            {
                BindDropDownFideicomisos();

                if (ddlFideicomisos.Items.Count > 0)
                {
                    int idCalendario = BuscarCalendario();
                    BindGridSesiones(idCalendario);

                    //ValidarEjercicioSeleccionado();
                }

                if (CalendarioCerrado())
                {
                    divAlerta.Style.Add("display", "block");
                    lblAlerta.Text = "El ejercicio se encuentra cerrado para este fideicomiso, sólo se puede consultar la información.";
                }
                
            }
        }

        private bool CalendarioCerrado()
        {
            int idCalendario = BuscarCalendario();
            Calendario obj = uow.CalendarioBusinessLogic.GetByID(idCalendario);

            return obj!=null ? !Convert.ToBoolean(obj.Activo) : false;

        }

        private void ValidarEjercicioSeleccionado()
        {
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());

            Ejercicio objEjercicio = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

            if (objEjercicio.Anio != DateTime.Now.Year)
            {
                gridNotas.Columns[0].Visible = false;
                gridActas.Columns[0].Visible = false;

            }


        }

        #region METODOS
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

       
        
        private void BindGridSesiones(int idCalendario)
        {
            List<Sesion> list;
            list = uow.SesionBusinessLogic.Get(e => e.CalendarioID == idCalendario).ToList();
            gridSesiones.DataSource = list;
            gridSesiones.DataBind();

            if (list.Count==0)
            {
                lblAlerta.Text = "No existen sesiones. Capture nuevas sesiones para poder agregar Notas y Actas";
                divAlerta.Style.Add("display", "block");
            }else
               divAlerta.Style.Add("display", "none");
        }
        private void BindGridNotas()
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            gridNotas.DataSource = uow.NotasBusinessLogic.Get(e => e.SesionID == idSesion).ToList();
            gridNotas.DataBind();
        }
        private void BindGridActas()
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            gridActas.DataSource = uow.ActasBusinessLogic.Get(e => e.SesionID == idSesion).ToList();
            gridActas.DataBind();
        }
        private void BindControlesNota()
        {
            int id = Utilerias.StrToInt(_IDNota.Value);
            Notas obj = uow.NotasBusinessLogic.GetByID(id);
            txtFideicomisoN.Value = GetClaveFideicomiso();
            txtNumeroSesionN.Value = GetNumSesion();
            txtDescripcionN.Value = obj.Descripcion;

            if (obj.NombreArchivo != null)
                if (!obj.NombreArchivo.Equals(string.Empty))
                    txtArchivoAdjuntoN.Value = obj.NombreArchivo;
                else
                    txtArchivoAdjuntoN.Value = "No existe archivo adjunto";
            else
                txtArchivoAdjuntoN.Value= "No existe archivo adjunto";
        }
        private void BindControlesActa()
        {
            int id = Utilerias.StrToInt(_IDActa.Value);
            Actas obj = uow.ActasBusinessLogic.GetByID(id);
            txtDescripcionA.Value = obj.Descripcion;
            txtFideicomisoA.Value = GetClaveFideicomiso();
            txtNumeroSesionA.Value = GetNumSesion();

            if (obj.NombreArchivo != null)
                if (!obj.NombreArchivo.Equals(string.Empty))
                    txtArchivoAdjuntoA.Value = obj.NombreArchivo;
                else
                    txtArchivoAdjuntoA.Value = "No existe archivo adjunto";
            else
                txtArchivoAdjuntoA.Value = "No existe archivo adjunto";

        }
        private int BuscarCalendario()
        {
            int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idCalendario = 0;

            Calendario obj = uow.CalendarioBusinessLogic.Get(c => c.DependenciaFideicomisoEjercicioID == idFideicomiso && c.EjercicioID == idEjercicio).FirstOrDefault();

            if (obj != null)
                idCalendario = obj.ID;

            _IDCalendario.Value = idCalendario.ToString();

            return idCalendario;

        }
        private string GetClaveFideicomiso()
        {
            DependenciaFideicomisoEjercicio ente = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(Utilerias.StrToInt(ddlFideicomisos.SelectedValue));
            Fideicomiso fidei = uow.FideicomisoBusinessLogic.GetByID(ente.FideicomisoID);
            return fidei.Clave;
        }
        private string GetNumSesion()
        {
            Sesion obj = uow.SesionBusinessLogic.GetByID(Utilerias.StrToInt(_IDSesion.Value));
            return obj.NumSesion;
        }
        public string GuardarArchivo(HttpPostedFile postedFile, int id, string nombreCarpeta)
        {

            string M = string.Empty;
            string ruta = string.Empty;

            try
            {
                ruta = System.Configuration.ConfigurationManager.AppSettings[nombreCarpeta];

                if (!ruta.EndsWith("/"))
                    ruta += "/";

                ruta += id.ToString() + "/";

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
        private string EliminarArchivo(int id, string nombreArchivo, string nombreCarpeta)
        {
            string M = string.Empty;
            try
            {
                string ruta = string.Empty;

                //eliminar archivo
                ruta = System.Configuration.ConfigurationManager.AppSettings[nombreCarpeta];

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
        #endregion

        #region EVENTOS SESIONES
        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCalendario = BuscarCalendario();
            BindGridSesiones(idCalendario);


            divEncabezado.Style.Add("display", "block");
            divNotasActas.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
            
        }
        protected void btnNotas_ServerClick(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((HtmlButton)sender).NamingContainer;

            _IDSesion.Value = gridSesiones.DataKeys[row.RowIndex].Value.ToString();

            BindGridNotas();
            BindGridActas();

            txtFideicomisoN.Value = GetClaveFideicomiso();
            txtFideicomisoA.Value = GetClaveFideicomiso();
            txtNumeroSesionN.Value = GetNumSesion();
            txtNumeroSesionA.Value = GetNumSesion();

            divEncabezado.Style.Add("display", "none");
            divNotasActas.Style.Add("display", "block");
            divNotas.Style.Add("display", "block");
            divEncabezadoNotas.Style.Add("display", "block");
            divMenu.Style.Add("display", "block");
            divCapturaNotas.Style.Add("display", "none");
            divEncabezadoActas.Style.Add("display", "none");
            divCapturaActas.Style.Add("display", "none");

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

            _AccionN.Value = string.Empty;
            _AccionA.Value = string.Empty;

            if (CalendarioCerrado())
            {
                divAlerta.Style.Add("display", "block");
                lblAlerta.Text = "El ejercicio se encuentra cerrado para este fideicomiso, sólo se puede consultar la información.";

                btnCrearNota.Disabled = true;
                btnCrearActa.Disabled = true;
            }
            else
            {
                divAlerta.Style.Add("display", "none");
                btnCrearNota.Disabled = false;
                btnCrearActa.Disabled = false;
            }

        }
        protected void gridSesiones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSesiones.PageIndex = e.NewPageIndex;
            BindGridSesiones(Utilerias.StrToInt(_IDCalendario.Value));

            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
            divNotasActas.Style.Add("display", "none");
        }
        protected void gridSesiones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                int idSesion = Utilerias.StrToInt(gridSesiones.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                Sesion obj = uow.SesionBusinessLogic.GetByID(idSesion);
                lblStatus.Text = uow.StatusSesionBusinessLogic.GetByID(obj.StatusSesionID).Descripcion;
            }
        }
        #endregion

        #region EVENTOS ACTAS
        protected void gridActas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridActas.PageIndex = e.NewPageIndex;
            BindGridActas();

            divMenu.Style.Add("display", "block");
            divActas.Style.Add("display", "block");
            divEncabezadoActas.Style.Add("display", "block");
            divCapturaActas.Style.Add("display", "none");
            divNotas.Style.Add("display", "none");

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }
        protected void gridActas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminarA");
                Label lblFideicomiso = (Label)e.Row.FindControl("lblFideicomisoA");
                Label lblSesion = (Label)e.Row.FindControl("lblSesionA");
                Label lblArchivoA = (Label)e.Row.FindControl("lblArchivoA");

                int idActa = Utilerias.StrToInt(gridActas.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                Actas acta = uow.ActasBusinessLogic.GetByID(idActa);

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarIDActa(" + idActa + ")");

                lblFideicomiso.Text = GetClaveFideicomiso();
                lblSesion.Text = GetNumSesion();

                if (acta.NombreArchivo != null)
                    if (!acta.NombreArchivo.Equals(string.Empty))
                        lblArchivoA.Text = acta.NombreArchivo;
                    else
                        lblArchivoA.Text = "No existe archivo adjunto";
                else
                    lblArchivoA.Text = "No existe archivo adjunto";

                //Se coloca la fucnion a corespondiente para visualizar el DOCUMENTO ADJUNTO 
                HtmlButton btnVer = (HtmlButton)e.Row.FindControl("btnVerA");
                string ruta = ResolveClientUrl("~/AbrirDocto.aspx");
                btnVer.Attributes["onclick"] = "fnc_AbrirArchivo('" + ruta + "'," + idActa + "," + 3 + ")";


                if (CalendarioCerrado())
                {
                    imgBtnEliminar.Enabled = false;
                }
                else
                {
                    imgBtnEliminar.Enabled = true;
                }


            }
        }
        protected void imgBtnEditA_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDActa.Value = gridActas.DataKeys[row.RowIndex].Value.ToString();
            _AccionA.Value = "A";

            BindControlesActa();


            divNotas.Style.Add("display", "none");
            divActas.Style.Add("display", "block");
            divCapturaActas.Style.Add("display", "block");
            divEncabezadoActas.Style.Add("display", "none");

            divMenu.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

            if (CalendarioCerrado())
            {
                btnGuardarA.Enabled = false;
                fileUploadA.Enabled = false;
            }
            else
            {
                btnGuardarA.Enabled = false;
                fileUploadA.Enabled = false;
            }

        }
        protected void btnGuardarA_Click(object sender, EventArgs e)
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            int idActa = Utilerias.StrToInt(_IDActa.Value);
            string nomAnterior = string.Empty;

            string M = string.Empty;
            Actas obj = null;


            if (_AccionA.Value.Equals("N"))
                obj = new Actas();
            else
                obj = uow.ActasBusinessLogic.GetByID(idActa);

            nomAnterior = obj.NombreArchivo;

            obj.Descripcion = txtDescripcionA.Value;
            obj.NombreArchivo = fileUploadA.FileName.Equals(string.Empty) ? obj.NombreArchivo : Path.GetFileName(fileUploadA.FileName);
            obj.TipoArchivo = fileUploadN.PostedFile.ContentType;

            if (_AccionA.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                obj.SesionID = idSesion;
                uow.ActasBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();

                if (nomAnterior != null)
                {
                    if (!nomAnterior.Equals(obj.NombreArchivo))  //Se elimina el archivo anterior
                        if (!nomAnterior.Equals(string.Empty))
                            M = EliminarArchivo(obj.ID, nomAnterior, "ArchivosActas");
                }

                uow.ActasBusinessLogic.Update(obj);
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

            _IDActa.Value = obj.ID.ToString(); //Se coloca el ID del nuevo objeto creado

            //Se almacena el archivo
            if (!fileUploadA.PostedFile.FileName.Equals(string.Empty))
            {
                if (fileUploadA.FileBytes.Length > 10485296)
                {
                    lblMsgError.Text = "Se ha excedido en el tamaño del archivo, el máximo permitido es de 10 Mb";
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");

                    return;
                }

                M = GuardarArchivo(fileUploadA.PostedFile, obj.ID, "ArchivosActas");

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

            BindGridActas();

            _AccionA.Value = string.Empty;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";
            divActas.Style.Add("display", "block");
            divEncabezadoActas.Style.Add("display", "block");
            divCapturaActas.Style.Add("display", "none");

            divMenu.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            //ocutasr controles de NOTAS
            divNotas.Style.Add("display", "none");

        }
        protected void btnDelA_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";
            string nombreArchivo;
            int idActa = Utilerias.StrToInt(_IDActa.Value);

            Actas obj = uow.ActasBusinessLogic.GetByID(idActa);
            nombreArchivo = obj.NombreArchivo;

            divMenu.Style.Add("display", "block");
            divActas.Style.Add("display", "block");
            divEncabezadoActas.Style.Add("display", "block");
            divCapturaActas.Style.Add("display", "none");
            divNotas.Style.Add("display", "none");

            //Se elimina el objeto
            uow.ActasBusinessLogic.Delete(obj);
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

            BindGridActas();

            //Se elimina el archivo fisico
            if (nombreArchivo != null)
            {
                if (!nombreArchivo.Equals(string.Empty))
                {
                    M = EliminarArchivo(idActa, nombreArchivo, "ArchivosActas");
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

        #region EVENTOS NOTAS
        protected void gridNotas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridActas.PageIndex = e.NewPageIndex;
            BindGridNotas();

            divMenu.Style.Add("display", "block");
            divActas.Style.Add("display", "block");
            divEncabezadoNotas.Style.Add("display", "block");
            divCapturaNotas.Style.Add("display", "none");
            divActas.Style.Add("display", "none");

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }
        protected void gridNotas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminarN");
                Label lblFideicomiso = (Label)e.Row.FindControl("lblFideicomisoN");
                Label lblSesion = (Label)e.Row.FindControl("lblSesionN");
                Label lblArchivoN = (Label)e.Row.FindControl("lblArchivoN");

                int idNota = Utilerias.StrToInt(gridNotas.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                Notas nota = uow.NotasBusinessLogic.GetByID(idNota);

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarIDNota(" + idNota + ")");

                lblFideicomiso.Text = GetClaveFideicomiso();
                lblSesion.Text = GetNumSesion();

                if (nota.NombreArchivo != null)
                    if (!nota.NombreArchivo.Equals(string.Empty))
                        lblArchivoN.Text = nota.NombreArchivo;
                    else
                        lblArchivoN.Text = "No existe archivo adjunto";
                else
                    lblArchivoN.Text = "No existe archivo adjunto";

                //Se coloca la fucnion a corespondiente para visualizar el DOCUMENTO ADJUNTO 
                HtmlButton btnVer = (HtmlButton)e.Row.FindControl("btnVerN");
                string ruta = ResolveClientUrl("~/AbrirDocto.aspx");
                btnVer.Attributes["onclick"] = "fnc_AbrirArchivo('" + ruta + "'," + idNota + "," + 2 + ")";
            }
        }
        protected void imgBtnEditN_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDNota.Value = gridNotas.DataKeys[row.RowIndex].Value.ToString();
            _AccionN.Value = "A";

            BindControlesNota();


            divActas.Style.Add("display", "none");
            divNotas.Style.Add("display", "block");
            divCapturaNotas.Style.Add("display", "block");
            divEncabezadoNotas.Style.Add("display", "none");

            divMenu.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

            if (CalendarioCerrado())
            {
                btnGuardarN.Enabled = false;
                fileUploadN.Enabled = false;
            }
            else
            {
                btnGuardarN.Enabled = true;
                fileUploadN.Enabled = true;
            }

        }
        protected void btnGuardarN_Click(object sender, EventArgs e)
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            int idNota = Utilerias.StrToInt(_IDNota.Value);
            string nomAnterior = string.Empty;

            string M = string.Empty;
            Notas obj = null;


            if (_AccionN.Value.Equals("N"))
                obj = new Notas();
            else
                obj = uow.NotasBusinessLogic.GetByID(idNota);

            nomAnterior = obj.NombreArchivo;

            obj.Descripcion = txtDescripcionN.Value;
            obj.NombreArchivo = fileUploadN.FileName.Equals(string.Empty) ? obj.NombreArchivo : Path.GetFileName(fileUploadN.FileName);
            obj.TipoArchivo = fileUploadN.PostedFile.ContentType;

            if (_AccionN.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                obj.SesionID = idSesion;
                uow.NotasBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();

                if (nomAnterior != null)
                {
                    if (!nomAnterior.Equals(obj.NombreArchivo))  //Se elimina el archivo anterior
                        if (!nomAnterior.Equals(string.Empty))
                            M = EliminarArchivo(obj.ID, nomAnterior, "ArchivosNotas");
                }

                uow.NotasBusinessLogic.Update(obj);
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

            _IDNota.Value = obj.ID.ToString(); //Se coloca el ID del nuevo objeto creado

            //Se almacena el archivo
            if (!fileUploadN.PostedFile.FileName.Equals(string.Empty))
            {
                if (fileUploadN.FileBytes.Length > 10485296)
                {
                    lblMsgError.Text = "Se ha excedido en el tamaño del archivo, el máximo permitido es de 10 Mb";
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");

                    return;
                }

                M = GuardarArchivo(fileUploadN.PostedFile, obj.ID, "ArchivosNotas");

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

            BindGridNotas();

            _AccionN.Value = string.Empty;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";
            divNotas.Style.Add("display", "block");
            divEncabezadoNotas.Style.Add("display", "block");
            divCapturaNotas.Style.Add("display", "none");

            divMenu.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            //ocutasr controles de Actas
            divActas.Style.Add("display", "none");

        }
        protected void btnDelN_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";
            string nombreArchivo;
            int idNota = Utilerias.StrToInt(_IDNota.Value);

            Notas obj = uow.NotasBusinessLogic.GetByID(idNota);
            nombreArchivo = obj.NombreArchivo;

            divMenu.Style.Add("display", "block");
            divNotas.Style.Add("display", "block");
            divEncabezadoNotas.Style.Add("display", "block");
            divCapturaNotas.Style.Add("display", "none");
            divActas.Style.Add("display", "none");

            //Se elimina el objeto
            uow.NotasBusinessLogic.Delete(obj);
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

            BindGridNotas();

            //Se elimina el archivo fisico
            if (nombreArchivo != null)
            {
                if (!nombreArchivo.Equals(string.Empty))
                {
                    M = EliminarArchivo(idNota, nombreArchivo, "ArchivosNotas");
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