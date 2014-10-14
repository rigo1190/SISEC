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
    public partial class Acuerdos : System.Web.UI.Page
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
                }
            }
        }

        private void BindDropDownFideicomisos()
        {
            int idDependencia = Utilerias.StrToInt(Session["Dependencia"].ToString());
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());


            var list = (from d in uow.DependenciaBusinessLogic.Get(e => e.ID == idDependencia)
                        join df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == idEjercicio)
                        on d.ID equals df.DependenciaID
                        join f in uow.FideicomisoBusinessLogic.Get()
                        on df.FideicomisoID equals f.ID
                        select new { df.ID, f.Descripcion }).ToList();

            ddlFideicomisos.DataSource = list;
            ddlFideicomisos.DataValueField = "ID";
            ddlFideicomisos.DataTextField = "Descripcion";
            ddlFideicomisos.DataBind();
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

        private void BindGridAcuerdos()
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            gridAcuerdos.DataSource = uow.AcuerdoBusinessLogic.Get(e => e.SesionID == idSesion).ToList();
            gridAcuerdos.DataBind();

        }

        private void BindControlesAcuerdo()
        {
            int idAcuerdo = Utilerias.StrToInt(_IDAcuerdo.Value);

            Acuerdo obj = uow.AcuerdoBusinessLogic.GetByID(idAcuerdo);

            txtNotas.Value = obj.Notas;

        }

        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCalendario = BuscarCalendario();
            BindGridSesiones(idCalendario);

            divDetalleAcuerdos.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
        }

        protected void gridSesiones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSesiones.PageIndex = e.NewPageIndex;
            BindGridSesiones(Utilerias.StrToInt(_IDCalendario.Value));
            divDetalleAcuerdos.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
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

        protected void gridAcuerdos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridAcuerdos.PageIndex = e.NewPageIndex;
            BindGridSesiones(Utilerias.StrToInt(_IDCalendario.Value));
            divDetalleAcuerdos.Style.Add("display", "block");
            divEncabezadoDetalle.Style.Add("display", "block");

            divCapturaDetalle.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void gridAcuerdos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ctrl = (ImageButton)e.Row.FindControl("imgBtnEliminar");

                int idAcuerdo = Utilerias.StrToInt(gridAcuerdos.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (ctrl != null)
                    ctrl.Attributes.Add("onclick", "fnc_ColocarIDAcuerdo(" + idAcuerdo + ")");

            }
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDAcuerdo.Value = gridAcuerdos.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControlesAcuerdo();

            divEncabezadoDetalle.Style.Add("display", "none");
            divCapturaDetalle.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnAcuerdos_ServerClick(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((HtmlButton)sender).NamingContainer;

            _IDSesion.Value = gridSesiones.DataKeys[row.RowIndex].Value.ToString();

            BindGridAcuerdos();

            divEncabezado.Style.Add("display", "none");
            divDetalleAcuerdos.Style.Add("display", "block");
            divEncabezadoDetalle.Style.Add("display", "block");
            divCapturaDetalle.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            Acuerdo obj = null;
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new Acuerdo();
            else
                obj = uow.AcuerdoBusinessLogic.GetByID(Utilerias.StrToInt(_IDAcuerdo.Value));

            obj.Notas = txtNotas.Value;
            obj.NumAcuerdo = txtNumAcuerdo.Value;

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                obj.SesionID = idSesion;
                uow.AcuerdoBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.AcuerdoBusinessLogic.Update(obj);
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

            BindGridAcuerdos();

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";

            divEncabezado.Style.Add("display", "none");
            divDetalleAcuerdos.Style.Add("display", "block");
            divEncabezadoDetalle.Style.Add("display", "block");
            divCapturaDetalle.Style.Add("display", "none");
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";

            int idAcuerdo = Utilerias.StrToInt(_IDAcuerdo.Value);

            Acuerdo obj = uow.AcuerdoBusinessLogic.GetByID(idAcuerdo);

            //Se elimina el objeto
            uow.AcuerdoBusinessLogic.Delete(obj);
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

            BindGridAcuerdos();

            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
        }




    }
}