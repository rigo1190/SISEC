using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

                if (ddlFideicomisos.Items.Count == 0)
                    btnCrearCalendario.Enabled = false;

            }
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
            txtFechaOficio.Value = obj.FechaOficio.ToString();
            txtNumSesion.Value = obj.NumSesion;
            ddlTipoSesion.SelectedValue = obj.TipoSesionID.ToString();
            txtFechaProgramada.Value = obj.FechaProgramada.ToString();
            txtHoraProgramada.Value = obj.HoraProgramada;

            txtLugarReunion.Value = obj.LugarReunion;
            txtDescripcion.Value = obj.Descripcion;
            txtObservaciones.Value = obj.Observaciones;
        }
        private void BindControlFideicomiso()
        {
            DependenciaFideicomisoEjercicio ente = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(Utilerias.StrToInt(ddlFideicomisos.SelectedValue));
            Fideicomiso fidei = uow.FideicomisoBusinessLogic.GetByID(ente.FideicomisoID);

            txtFideicomiso.Value = fidei.Descripcion;
        }
        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCalendario = BuscarCalendario();
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
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
                
            BindGridSesiones(idCalendario);

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
            obj.FechaProgramada =Convert.ToDateTime(txtFechaProgramada.Value);
            obj.HoraProgramada = txtHoraProgramada.Value;
            obj.LugarReunion = txtLugarReunion.Value;
            obj.Descripcion = txtDescripcion.Value;
            obj.Observaciones = txtObservaciones.Value;

            if (_Accion.Value.Equals("N"))
            {
                obj.UsuarioCaptura = Session["Login"].ToString();
                obj.FechaCaptura = DateTime.Now;
                obj.CalendarioID = idCalendario;
                obj.StatusSesionID = uow.StatusSesionBusinessLogic.Get(s=>s.Clave=="P").FirstOrDefault().ID;

                uow.SesionBusinessLogic.Insert(obj);

            }
            else
            {
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
                int idSesion = Utilerias.StrToInt(gridSesiones.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                Sesion obj=uow.SesionBusinessLogic.GetByID(idSesion);
                lblStatus.Text = uow.StatusSesionBusinessLogic.GetByID(obj.StatusSesionID).Descripcion;

                ImageButton ctrl = (ImageButton)e.Row.FindControl("imgBtnEliminar");

                if (ctrl != null)
                    ctrl.Attributes.Add("onclick", "fnc_ColocarIDSesion(" + idSesion + ")");
                
            }
        }
        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDSesion.Value = gridSesiones.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControlesSesion();

            //HABILTAR LA PARTE DE CONTROLES PARA DAR DE ALTA UN NUEVO REGISTRO DE SESION
            divCapturaSesion.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";

            int idSesion = Utilerias.StrToInt(_IDSesion.Value);

            Sesion obj = uow.SesionBusinessLogic.GetByID(idSesion);

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

            
        }

    }

}