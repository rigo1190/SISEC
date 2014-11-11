using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC.Reports
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
                BindDropDownStatus();
                BindGrid(true);
                _Consultado.Value = "N";
                _Ejercicio.Value = Session["Ejercicio"].ToString();
                _URL.Value = ResolveClientUrl("~/Reports/ReportView.aspx");
            }

        }


        private void BindDropDownSesiones()
        {
            int idCalendario = BuscarCalendario();
            ddlSesiones.DataSource = uow.SesionBusinessLogic.Get(e => e.CalendarioID == idCalendario).ToList();
            ddlSesiones.DataTextField = "NumSesion";
            ddlSesiones.DataValueField = "ID";
            ddlSesiones.DataBind();
        }


        private void BindDropDownStatus()
        {
            ddlStatusAcuerdo.DataSource = uow.StatusAcuerdoBusinessLogic.Get();
            ddlStatusAcuerdo.DataTextField = "Descripcion";
            ddlStatusAcuerdo.DataValueField = "ID";
            ddlStatusAcuerdo.DataBind();
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

            ddlFideicomisos.Items.Insert(0, new ListItem("Seleccione...", "0"));

            ddlFideicomisos.SelectedValue = "0";
        }

        private int BuscarCalendario()
        {
            int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idCalendario = 0;

            DAL.Model.Calendario obj = uow.CalendarioBusinessLogic.Get(c => c.DependenciaFideicomisoEjercicioID == idFideicomiso && c.EjercicioID == idEjercicio).FirstOrDefault();

            if (obj != null)
                idCalendario = obj.ID;

            _IDCalendario.Value = idCalendario.ToString();

            return idCalendario;

        }


        private void BindGrid(bool todos=false)
        {
            int idSesion=Utilerias.StrToInt(ddlSesiones.SelectedValue);
            int idStatus=Utilerias.StrToInt(ddlStatusAcuerdo.SelectedValue);
            
            List<Acuerdo> list=null;

            if (!todos)
            {
                if (!txtNumAcuredo.Value.Trim().Equals(string.Empty))
                    list = uow.AcuerdoBusinessLogic.Get(e => e.SesionID == idSesion && e.StatusAcuerdoID == idStatus && e.NumAcuerdo.Contains(txtNumAcuredo.Value)).ToList();
                else
                    list = uow.AcuerdoBusinessLogic.Get(e => e.SesionID == idSesion && e.StatusAcuerdoID == idStatus).ToList();
            }else
                list = uow.AcuerdoBusinessLogic.Get().ToList();

            

            gridAcuerdos.DataSource = list;
            gridAcuerdos.DataBind();

            lblResultado.Text = "Resultado: " + list.Count.ToString() + " registros";
            
        }


        private string GetClaveStatus(int idStatus)
        {
            StatusAcuerdo obj = uow.StatusAcuerdoBusinessLogic.GetByID(idStatus);
            return obj.Descripcion;
        }

        private string GetNumeroSesion(int idSesion)
        {
            Sesion sesion = uow.SesionBusinessLogic.GetByID(idSesion);
            return sesion.NumSesion;
        }

        private string GetClaveFideicomiso(int idCalendario)
        {
            Fideicomiso obj = (from c in uow.CalendarioBusinessLogic.Get(e => e.ID == idCalendario)
                               join d in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get()
                               on c.DependenciaFideicomisoEjercicioID equals d.ID
                               join f in uow.FideicomisoBusinessLogic.Get()
                               on d.FideicomisoID equals f.ID
                               select f).FirstOrDefault();
            if (obj != null)
                return obj.Clave;
            else
                return string.Empty;
        }


        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownSesiones();
        }

        protected void btnConsulta_Click(object sender, EventArgs e)
        {
            BindGrid(false);
            _Consultado.Value = "S";
        }

        protected void gridAcuerdos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idAcuerdo = Utilerias.StrToInt(gridAcuerdos.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                Label lblFideicomiso = (Label)e.Row.FindControl("lblFideicomiso");
                Label lblSesion = (Label)e.Row.FindControl("lblSesion");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");

                Acuerdo obj = uow.AcuerdoBusinessLogic.GetByID(idAcuerdo);

                lblFideicomiso.Text = GetClaveFideicomiso(obj.Sesion.CalendarioID);
                lblStatus.Text = GetClaveStatus(obj.StatusAcuerdoID);
                lblSesion.Text = GetNumeroSesion(obj.SesionID);

            }
        }

        protected void gridAcuerdos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridAcuerdos.PageIndex = e.NewPageIndex;
            if (_Consultado.Value.Equals("N"))
                BindGrid(true);
            else
                BindGrid(false);
        }


    }
}