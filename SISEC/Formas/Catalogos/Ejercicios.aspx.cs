using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC.Formas.Catalogos
{
    public partial class Ejercicios : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            gridEjercicios.DataSource = uow.EjercicioBusinessLogic.Get().ToList();
            gridEjercicios.DataBind();
        }


        private void BindControles()
        {
            int idEjercicio = Utilerias.StrToInt(_IDEjercicio.Value);

            StatusSesion obj = uow.StatusSesionBusinessLogic.GetByID(idEjercicio);

            txtAnio.Value = obj.Clave;
            txtDescripcion.Value = obj.Descripcion;

        }


        protected void gridEjercicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gridEjercicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        protected void btnDel_Click(object sender, EventArgs e)
        {

        }
    }
}