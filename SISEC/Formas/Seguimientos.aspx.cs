using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SISEC.Formas
{
    public partial class Seguimientos : System.Web.UI.Page
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
                    ValidarPermisosUsuario();
                }
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
                    gridSeguimientos.Columns[0].Visible = false;
                    btnGuardar.Enabled = false;
                    break;
            }
        }

        private void ValidarEjercicioSeleccionado()
        {
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());

            Ejercicio objEjercicio = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

            if (objEjercicio.Anio != DateTime.Now.Year)
            {
                gridSeguimientos.Columns[0].Visible = false;
            }


        }
        private void BindDropDownFideicomisos()
        {
            int idDependencia = Utilerias.StrToInt(Session["Dependencia"].ToString());
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());


            //var list = (from d in uow.DependenciaBusinessLogic.Get(e => e.ID == idDependencia)
            //            join df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == idEjercicio)
            //            on d.ID equals df.DependenciaID
            //            join f in uow.FideicomisoBusinessLogic.Get()
            //            on df.FideicomisoID equals f.ID
            //            select new { df.ID, f.Descripcion }).ToList();

            var list = (from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e=>e.EjercicioID==idEjercicio)
                        join f in uow.FideicomisoBusinessLogic.Get()
                        on df.FideicomisoID equals f.ID
                        join d in uow.DependenciaBusinessLogic.Get(e=>e.ID==idDependencia)
                        on f.DependenciaID equals d.ID
                        select new { df.ID, f.Descripcion }).ToList();

            ddlFideicomisos.DataSource = list;
            ddlFideicomisos.DataValueField = "ID";
            ddlFideicomisos.DataTextField = "Descripcion";
            ddlFideicomisos.DataBind();


            ValidarPermisosUsuario();


        }
        private void BindGridSesiones(int idCalendario)
        {
            List<Sesion> list;
            list = uow.SesionBusinessLogic.Get(e => e.CalendarioID == idCalendario).ToList();
            gridSesiones.DataSource = list;
            gridSesiones.DataBind();
        }
        private void BindControlFideicomiso()
        {
            DependenciaFideicomisoEjercicio ente = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(Utilerias.StrToInt(ddlFideicomisos.SelectedValue));
            Fideicomiso fidei = uow.FideicomisoBusinessLogic.GetByID(ente.FideicomisoID);

            //txtFideicomiso.Value = fidei.Descripcion;
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
        private void BindGridSeguimientos()
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            //gridSeguimientos.DataSource = uow.SeguimientoBusinessLogic.Get(e=>e.SesionID==idSesion).ToList();
            gridSeguimientos.DataBind();
        }
        private void BindControlesSeguimiento()
        {
            int idSeguimiento = Utilerias.StrToInt(_IDSeguimiento.Value);

            Seguimiento obj = uow.SeguimientoBusinessLogic.GetByID(idSeguimiento);

            txtDescripcion.Value = obj.Descripcion;

        }
        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCalendario = BuscarCalendario();
            BindGridSesiones(idCalendario);
            
            divDetalleSeguimientos.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");

            ValidarPermisosUsuario();
            
        }
        protected void gridSesiones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSesiones.PageIndex = e.NewPageIndex;
            BindGridSesiones(Utilerias.StrToInt(_IDCalendario.Value));
            divDetalleSeguimientos.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

            ValidarPermisosUsuario();
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
        protected void gridSeguimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSeguimientos.PageIndex = e.NewPageIndex;
            BindGridSesiones(Utilerias.StrToInt(_IDCalendario.Value));
            divDetalleSeguimientos.Style.Add("display", "block");
            divEncabezadoDetalle.Style.Add("display", "block");
            
            divCapturaDetalle.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

            ValidarPermisosUsuario();
        }
        protected void gridSeguimientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ctrl = (ImageButton)e.Row.FindControl("imgBtnEliminar");
                
                int idSeguimiento = Utilerias.StrToInt(gridSeguimientos.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                
                if (ctrl != null)
                    ctrl.Attributes.Add("onclick", "fnc_ColocarIDSeguimiento(" + idSeguimiento + ")");

            }
        }
        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDSeguimiento.Value = gridSeguimientos.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControlesSeguimiento();
            
            divEncabezadoDetalle.Style.Add("display", "none");
            divCapturaDetalle.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

        }
        protected void btnSeguimiento_ServerClick(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((HtmlButton)sender).NamingContainer;
            
            _IDSesion.Value = gridSesiones.DataKeys[row.RowIndex].Value.ToString();

            BindGridSeguimientos();
            
            divEncabezado.Style.Add("display", "none");
            divDetalleSeguimientos.Style.Add("display", "block");
            divEncabezadoDetalle.Style.Add("display", "block");
            divCapturaDetalle.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
            
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            Seguimiento obj=null;
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new Seguimiento();
            else
                obj = uow.SeguimientoBusinessLogic.GetByID(Utilerias.StrToInt(_IDSeguimiento.Value));

            obj.Descripcion = txtDescripcion.Value;
            

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                //obj.SesionID = idSesion;
                uow.SeguimientoBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.SeguimientoBusinessLogic.Update(obj);
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

            BindGridSeguimientos();

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";

            divEncabezado.Style.Add("display", "none");
            divDetalleSeguimientos.Style.Add("display", "block");
            divEncabezadoDetalle.Style.Add("display", "block");
            divCapturaDetalle.Style.Add("display", "none");
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";

            int idSeguimiento = Utilerias.StrToInt(_IDSeguimiento.Value);

            Seguimiento obj = uow.SeguimientoBusinessLogic.GetByID(idSeguimiento);

            //Se elimina el objeto
            uow.SeguimientoBusinessLogic.Delete(obj);
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

            BindGridSeguimientos();

            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
        }
    }
}