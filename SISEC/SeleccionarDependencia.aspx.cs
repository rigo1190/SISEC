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
    public partial class SeleccionarDependencia : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();

            if (!IsPostBack)
            {
                BindDropDownEjercicio();
                BindDropDownDependencia();
            }

        }

        private void BindDropDownEjercicio()
        {
            ddlEjercicios.DataSource = uow.EjercicioBusinessLogic.Get().ToList();
            ddlEjercicios.DataValueField = "ID";
            ddlEjercicios.DataTextField = "Anio";
            ddlEjercicios.DataBind();
        }

        private void BindDropDownDependencia()
        {
            ddlDependecia.DataSource = GetDependencias();
            ddlDependecia.DataValueField = "ID";
            ddlDependecia.DataTextField = "Descripcion";
            ddlDependecia.DataBind();
        }


        private List<Dependencia> GetDependencias()
        {
            List<Dependencia> list = null;
            string login=Session["Login"].ToString();

            list = (from d in uow.DependenciaBusinessLogic.Get()
                    join u in uow.UsuarioDependenciaBusinessLogic.Get(u => u.Usuario.Login == login)
                    on d.ID equals u.DependenciaID
                    select d).ToList();

            return list;
        }

        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            Session["Dependencia"] = ddlDependecia.SelectedValue;
            Session["Ejercicio"] = ddlEjercicios.SelectedValue;
            Response.Redirect("~/Formas/Sesiones.aspx");
        }
    }
}