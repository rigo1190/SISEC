using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC.Formas
{
    public partial class Sesiones : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();
            
            if (!IsPostBack)
            {
                BindDropDownFideicomisos();
                CargarGridPrimerFideicomiso();
                BindDropDownTipoSesion();
                BindDropDownStatusSesion();



                if (ddlFideicomisos.Items.Count == 0)
                {
                    btnCrearCalendario.Enabled = false;
                }
                else
                {
                    if (CalendarioCerrado())
                    {
                        btnCrearCalendario.Enabled = false;
                        divAlerta.Style.Add("display", "block");
                        lblAlerta.Text = "El ejercicio se encuentra cerrado para este fideicomiso, sólo se puede consultar la información.";
                    }
                        
                }


                ddlStatus.Attributes["onchange"] = "fnc_GetDatosStatus(this)";

                //ValidarEjercicioSeleccionado();
                ValidarPermisosUsuario();
            }
        }

        private void ValidarPermisosUsuario()
        {
            int idUser = Utilerias.StrToInt(Session["UserID"].ToString());

            Usuario obj = uow.UsuarioBusinessLogic.GetByID(idUser);

            switch (obj.TipoUsuarioID)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3: //EJECUTIVO
                    gridSesiones.Columns[0].Visible = false;
                    btnGuardar.Enabled = false;
                    btnCrearCalendario.Enabled = false;
                    break;
            }
        }

        private void ValidarEjercicioSeleccionado()
        {
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());

            Ejercicio objEjercicio = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

            if (objEjercicio.Anio != DateTime.Now.Year)
            {
                gridSesiones.Columns[0].Visible = false;
                btnCrearCalendario.Enabled = false;
            }


        }

        private bool CalendarioCerrado()
        {
            int idCalendario = BuscarCalendario();
            Calendario obj = uow.CalendarioBusinessLogic.GetByID(idCalendario);

            return !Convert.ToBoolean(obj!=null?obj.Activo:true);
 
        }

        

        private void CargarGridPrimerFideicomiso()
        {
            if (ddlFideicomisos.Items.Count > 0)
            {
                int idCalendario = BuscarCalendario();
                BindGridSesiones(idCalendario);
            }
        }
        private void BindDropDownTipoSesion()
        {
            ddlTipoSesion.DataSource = uow.TipoSesionBusinessLogic.Get().ToList();
            ddlTipoSesion.DataValueField = "ID";
            ddlTipoSesion.DataTextField = "Descripcion";
            ddlTipoSesion.DataBind();
        }
        private void BindDropDownFideicomisos()
        {
            
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idUser = Utilerias.StrToInt(Session["UserID"].ToString());

            var list = (from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == idEjercicio)
                        join ud in uow.UsuarioFideicomisoBusinessLogic.Get(e=>e.UsuarioID==idUser)
                        on df.ID equals ud.DependenciaFideicomisoEjercicioID
                        join f in uow.FideicomisoBusinessLogic.Get()
                        on df.FideicomisoID equals f.ID
                        select new { df.ID, f.Clave });

            ddlFideicomisos.DataSource = list;
            ddlFideicomisos.DataValueField = "ID";
            ddlFideicomisos.DataTextField = "Clave";
            ddlFideicomisos.DataBind();
        }
        private void BindDropDownStatusSesion()
        {
            ddlStatus.DataSource = uow.StatusSesionBusinessLogic.Get();
            ddlStatus.DataValueField = "ID";
            ddlStatus.DataTextField = "Descripcion";
            ddlStatus.DataBind();
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
        private void BindGridSesiones(int idCalendario)
        {
            List<Sesion> list;
            list = uow.SesionBusinessLogic.Get(e => e.CalendarioID == idCalendario).ToList();
            gridSesiones.DataSource = list;
            gridSesiones.DataBind();
        }
        private List<string> CrearCalendario(int idFideicomiso,int idEjercicio)
        {
            
            string M = string.Empty;
            List<string> R=new List<string>();

            Calendario obj = new Calendario();
            obj.DependenciaFideicomisoEjercicioID = idFideicomiso;
            obj.EjercicioID = idEjercicio;
            obj.Activo = true;
            obj.FechaCaptura = DateTime.Now;
            obj.UsuarioCaptura = Session["Login"].ToString();

            uow.CalendarioBusinessLogic.Insert(obj);
            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                foreach (string e in uow.Errors)
                    M += e;

                R.Add(M);
                return R;
            }

            R.Add(M);
            R.Add(obj.ID.ToString());

            return R;
        }
        private void LimpiarCampos()
        {
            txtNumOficio.Value = string.Empty;
            txtFechaOficio.Value = string.Empty;
            txtNumSesion.Value = string.Empty;
            ddlTipoSesion.SelectedIndex = 0;
            txtFechaProgramada.Value = string.Empty;
            txtHoraProgramada.Value = string.Empty;

            txtLugarReunion.Value = string.Empty;
            txtDescripcion.Value = string.Empty;
            txtObservaciones.Value = string.Empty;
        }
        private void BindControlesSesion()
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            Sesion obj = uow.SesionBusinessLogic.GetByID(idSesion);

            BindControlFideicomiso();
            txtNumOficio.Value = obj.NumOficio;
            txtFechaOficio.Value =String.Format("{0:d}", obj.FechaOficio);
            txtNumSesion.Value = obj.NumSesion;
            ddlTipoSesion.SelectedValue = obj.TipoSesionID.ToString();
            txtLugarReunion.Value = obj.LugarReunion;
            txtDescripcion.Value = obj.Descripcion;
            txtObservaciones.Value = obj.Observaciones;
            ddlStatus.SelectedValue = obj.StatusSesionID.ToString();

            StatusSesion status = uow.StatusSesionBusinessLogic.GetByID(obj.StatusSesionID);

            switch (status.Clave) //Dependiendo el estatus de la sesion se muestran y bindean los controles correspondientes
            {
                case "P": //PROGRAMADA
                    txtFechaProgramada.Value = String.Format("{0:d}", obj.FechaProgramada);
                    txtHoraProgramada.Value = obj.HoraProgramada;
                    divDatosProgramada.Style.Add("display", "block");
                    divDatosReprogramada.Style.Add("display", "none");
                    divDatosCelebrada.Style.Add("display", "none");
                    break;
                case "RP"://REPROGRAMADA
                    txtFechaReprogramada.Value = String.Format("{0:d}", obj.FechaReprogramada);
                    txtHoraReprogramada.Value = obj.HoraReprogramada;
                    divDatosProgramada.Style.Add("display", "none");
                    divDatosReprogramada.Style.Add("display", "block");
                    divDatosCelebrada.Style.Add("display", "none");
                    break;
                case "CE": //CELEBRADA
                    txtFechaCelebrada.Value = String.Format("{0:d}",obj.FechaCelebrada);
                    txtHoraCelebrada.Value = obj.HoraCelebrada;
                    divDatosProgramada.Style.Add("display", "none");
                    divDatosReprogramada.Style.Add("display", "none");
                    divDatosCelebrada.Style.Add("display", "block");
                    break;
                case "C": //CANCELADA
                    divDatosProgramada.Style.Add("display", "none");
                    divDatosReprogramada.Style.Add("display", "none");
                    divDatosCelebrada.Style.Add("display", "none");
                    break;
            }
        }
        private void BindControlFideicomiso()
        {
            DependenciaFideicomisoEjercicio ente = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(Utilerias.StrToInt(ddlFideicomisos.SelectedValue));
            Fideicomiso fidei = uow.FideicomisoBusinessLogic.GetByID(ente.FideicomisoID);

            txtFideicomiso.Value = fidei.Descripcion;
        }
        private string ValidarEliminacion(Sesion obj)
        {
            string M = string.Empty;

            if (obj.DetalleAcuerdos.Count > 0)
                M = "Existen Acuerdos para esta Sesión. Elimine los Acuerdos y vuelva a intentarlo";
            else if (obj.DetalleActas.Count>0)
                M = "Existen Actas para esta Sesión. Elimine las Actas y vuelva a intentarlo";
            else if (obj.DetalleNotas.Count>0)
                M = "Existen Notas para esta Sesión. Elimine las Notas y vuelva a intentarlo";

            return M;
        }

        [WebMethod]
        public static List<string> GetDatosStatus(int _idStatus)
        {
            List<string> R = new List<string>();
            UnitOfWork uow = new UnitOfWork();
            int idStatus = _idStatus;//Utilerias.StrToInt(_idStatus);
            StatusSesion obj = uow.StatusSesionBusinessLogic.GetByID(idStatus);
            R.Add(obj.Clave);

            return R;
        }
        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCalendario = BuscarCalendario();
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
            List<string> R = null;

            if (!CalendarioCerrado())
            {
                if (idCalendario == 0)
                {
                    Ejercicio objEjercicio = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

                    if (objEjercicio.Anio == DateTime.Now.Year)
                    {
                        R = CrearCalendario(idFideicomiso, idEjercicio);

                        if (!R[0].Equals(string.Empty))
                        {
                            //MANEJAR EL ERROR
                            divMsgError.Style.Add("display", "block");
                            divMsgSuccess.Style.Add("display", "none");
                            lblMsgError.Text = R[0];
                            return;
                        }

                        idCalendario = Utilerias.StrToInt(R[1]);

                        btnCrearCalendario.Enabled = true;
                        divAlerta.Style.Add("display", "none");
                    }
                    else
                    {
                        btnCrearCalendario.Enabled = false;
                        divAlerta.Style.Add("display", "block");
                        lblAlerta.Text = "El ejercicio se encuentra cerrado para este fideicomiso, sólo se puede consultar la información.";
                    }

                }
                else
                {
                    btnCrearCalendario.Enabled = true;
                    divAlerta.Style.Add("display", "none");
                }

                
            }
            else
            {
                btnCrearCalendario.Enabled = false;

                divAlerta.Style.Add("display", "block");
                lblAlerta.Text = "El ejercicio se encuentra cerrado para este fideicomiso, sólo se puede consultar la información.";

            }
               

                
            BindGridSesiones(idCalendario);

            ValidarPermisosUsuario();

            divCapturaSesion.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }
        protected void btnCrearCalendario_Click(object sender, EventArgs e)
        {
            
            int idCalendario = 0;
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
            

            idCalendario = BuscarCalendario();

            List<string> R = null;

            if (idCalendario == 0)
            {
                R = CrearCalendario(idFideicomiso, idEjercicio);

                if (!R[0].Equals(string.Empty))
                {
                    //MANEJAR EL ERROR
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");
                    lblMsgError.Text = R[0];
                    return;
                }
                idCalendario = Utilerias.StrToInt(R[1]);
            }

            _IDCalendario.Value = idCalendario.ToString();
            LimpiarCampos();
            BindControlFideicomiso();

            //HABILTAR LA PARTE DE CONTROLES PARA DAR DE ALTA UN NUEVO REGISTRO DE SESION
            divCapturaSesion.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
            _Accion.Value = "N"; //Sera una nueva sesion

            //Por default, en una sesion nueva, el status va a ser programada
            //Se inhabilita el combo de status y se coloca en el index cero
            ddlStatus.Enabled = false;
            ddlStatus.SelectedIndex = 0;
            btnGuardar.Enabled = true;
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int idCalendario = Utilerias.StrToInt(_IDCalendario.Value);
            string M = string.Empty;
            Sesion obj = null;

            if (_Accion.Value.Equals("N"))
                obj = new Sesion();
            else
                obj = uow.SesionBusinessLogic.GetByID(Utilerias.StrToInt(_IDSesion.Value));

            obj.NumOficio = txtNumOficio.Value;
            obj.FechaOficio = Convert.ToDateTime(txtFechaOficio.Value);
            obj.NumSesion = txtNumSesion.Value;
            obj.TipoSesionID = Utilerias.StrToInt(ddlTipoSesion.SelectedValue);
            obj.LugarReunion = txtLugarReunion.Value;
            obj.Descripcion = txtDescripcion.Value;
            obj.Observaciones = txtObservaciones.Value;

            if (_Accion.Value.Equals("N")) //Es nueva sesion
            {
                obj.UsuarioCaptura = Session["Login"].ToString();
                obj.FechaCaptura = DateTime.Now;
                obj.CalendarioID = idCalendario;
                obj.FechaProgramada = Convert.ToDateTime(txtFechaProgramada.Value);
                obj.HoraProgramada = txtHoraProgramada.Value;
                obj.StatusSesionID = uow.StatusSesionBusinessLogic.Get(s=>s.Clave=="P").FirstOrDefault().ID;
                obj.Mes = Convert.ToDateTime(txtFechaProgramada.Value).Month;
                uow.SesionBusinessLogic.Insert(obj);

            }
            else //Se esta actualizando una sesion exisente
            {
                int idStatus = Utilerias.StrToInt(ddlStatus.SelectedValue);
                StatusSesion status = uow.StatusSesionBusinessLogic.GetByID(idStatus);

                switch (status.Clave) //Dependiendo el estatus de la sesion elegido, se llenan los campos correspondientes
                {
                    case "P": //PROGRAMADA
                        obj.FechaProgramada = Convert.ToDateTime(txtFechaProgramada.Value);
                        obj.HoraProgramada = txtHoraProgramada.Value;
                        obj.Mes=Convert.ToDateTime(txtFechaProgramada.Value).Month;
                        break;
                    case "RP"://REPROGRAMADA
                        obj.FechaReprogramada = Convert.ToDateTime(txtFechaReprogramada.Value);
                        obj.HoraReprogramada = txtHoraReprogramada.Value;
                        obj.Observaciones = txtObservaciones.Value;
                        obj.Mes = Convert.ToDateTime(txtFechaReprogramada.Value).Month;
                        break;
                    case "CE": //CELEBRADA
                        obj.FechaCelebrada = Convert.ToDateTime(txtFechaCelebrada.Value);
                        obj.HoraCelebrada = txtHoraCelebrada.Value;
                        break;
                    case "C": //CANCELADA
                        break;
                }

                obj.StatusSesionID = idStatus;
                obj.UsuarioModifica = Session["Login"].ToString();
                obj.FechaModificacion = DateTime.Now;
                uow.SesionBusinessLogic.Update(obj);
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


            BindGridSesiones(idCalendario);

            LimpiarCampos();

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";

            divCapturaSesion.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");


        }
        protected void gridSesiones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");
                ImageButton imgBtnEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");

                int idSesion = Utilerias.StrToInt(gridSesiones.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                Sesion obj=uow.SesionBusinessLogic.GetByID(idSesion);
                StatusSesion status=uow.StatusSesionBusinessLogic.GetByID(obj.StatusSesionID);
                lblStatus.Text = status.Descripcion;

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarIDSesion(" + idSesion + ")");


                if (CalendarioCerrado())
                    imgBtnEliminar.Enabled = false;
                else
                    imgBtnEliminar.Enabled = true;
                


            }
        }
        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDSesion.Value = gridSesiones.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControlesSesion();

            //HABILTAR LA PARTE DE CONTROLES PARA DAR MODIFICACION A UN REGISTRO DE SESION
            divCapturaSesion.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

            //Se habilita el combo de status de sesion
            ddlStatus.Enabled = true;

            if (CalendarioCerrado())
            {
                btnGuardar.Enabled = false;
                divAlerta.Style.Add("display", "block");
                lblAlerta.Text = "El ejercicio se encuentra cerrado para este fideicomiso, sólo se puede consultar la información.";
            }
            else
            {
                btnGuardar.Enabled = true;
                divAlerta.Style.Add("display", "none");
            }
                
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            string M = string.Empty;

            int idSesion = Utilerias.StrToInt(_IDSesion.Value);

            Sesion obj = uow.SesionBusinessLogic.GetByID(idSesion);

            M = ValidarEliminacion(obj); //Se valida si se puede eliminar

            if (!M.Equals(string.Empty))
            {
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }


            M = "Se ha eliminado correctamente";

            //Se elimina el objeto
            uow.SesionBusinessLogic.Delete(obj);
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

            BindGridSesiones(Utilerias.StrToInt(_IDCalendario.Value));

            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");


        }
        protected void gridSesiones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSesiones.PageIndex = e.NewPageIndex;
            BindGridSesiones(Utilerias.StrToInt(_IDCalendario.Value));

            divCapturaSesion.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

            ValidarPermisosUsuario();
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idStatus = Utilerias.StrToInt(ddlStatus.SelectedValue);
            StatusSesion obj = uow.StatusSesionBusinessLogic.GetByID(idStatus);

            switch (obj.Clave)
            {

                case "P": //PROGRAMADA
                    divDatosProgramada.Style.Add("display", "block");
                    divDatosReprogramada.Style.Add("display", "none");
                    divDatosCelebrada.Style.Add("display", "none");
                    break;
                case "RP"://REPROGRAMADA
                    divDatosProgramada.Style.Add("display", "none");
                    divDatosReprogramada.Style.Add("display", "block");
                    divDatosCelebrada.Style.Add("display", "none");
                    break;
                case "CE": //CELEBRADA
                    divDatosProgramada.Style.Add("display", "none");
                    divDatosReprogramada.Style.Add("display", "none");
                    divDatosCelebrada.Style.Add("display", "block");
                    break;
                case "C": //CANCELADA
                    break;
            }

            ValidarPermisosUsuario();

        }

    }

}