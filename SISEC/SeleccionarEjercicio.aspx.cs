using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC
{
    public partial class SeleccionarEjercicio : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();
            
            if (!IsPostBack)
            {
                BindDropDownEjercicio();
                _Ruta.Value = ResolveClientUrl("~/Formas/Sesiones.aspx");
            }
        }

        private void BindDropDownEjercicio()
        {
            ddlEjercicios.DataSource = uow.EjercicioBusinessLogic.Get().ToList();
            ddlEjercicios.DataValueField = "ID";
            ddlEjercicios.DataTextField = "Anio";
            ddlEjercicios.DataBind();
        }

        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            Session["Ejercicio"] = ddlEjercicios.SelectedValue;

            int idEjercicio = Utilerias.StrToInt(ddlEjercicios.SelectedValue);

            Ejercicio objEjercicio = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

            if (objEjercicio.Anio != DateTime.Now.Year)
                ClientScript.RegisterStartupScript(this.GetType(), "script", "fnc_Mensaje()", true);
            else
                Response.Redirect("~/Formas/Sesiones.aspx");

        }
    }
}