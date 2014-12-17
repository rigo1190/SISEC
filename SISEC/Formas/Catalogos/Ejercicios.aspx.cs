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

        private bool ValidarInsercion(int añoSeñalado, Ejercicio objEjercicio=null)
        {
            Ejercicio obj=null;

            if (objEjercicio==null)
                obj= uow.EjercicioBusinessLogic.Get(e => e.Anio == añoSeñalado).FirstOrDefault();
            else
                if (añoSeñalado != objEjercicio.Anio)
                    obj = uow.EjercicioBusinessLogic.Get(e => e.Anio == añoSeñalado).FirstOrDefault();
                

            return obj == null;
        }

        private void BindControles()
        {
            int idEjercicio = Utilerias.StrToInt(_IDEjercicio.Value);

            Ejercicio obj = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

            txtAnio.Value = obj.Anio.ToString();
            txtDescripcion.Value = obj.Descripcion;

            txtAnio.Disabled = !ValidarEliminarEjercicio(obj);
            
        }


        private bool ValidarEliminarEjercicio(Ejercicio obj)
        {
            if (obj.DetalleCalendarios.Count > 0)
                return false;

            if (obj.DetalleFideicomisos.Count > 0)
                return false;

            return true;

        }

        protected void gridEjercicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");

                int id = Utilerias.StrToInt(gridEjercicios.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarID(" + id + ")");

            }
        }

        protected void gridEjercicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridEjercicios.PageIndex = e.NewPageIndex;
            BindGrid();

            divEncabezado.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDEjercicio.Value = gridEjercicios.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Ejercicio obj;
            int idStatus = Utilerias.StrToInt(_IDEjercicio.Value);
            string M = string.Empty;

            if (_Accion.Value.Equals("N")){
                obj = new Ejercicio();
            }
            else
                obj = uow.EjercicioBusinessLogic.GetByID(idStatus);

            if (!ValidarInsercion(Utilerias.StrToInt(txtAnio.Value),!_Accion.Value.Equals("N") ? obj : null))
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "none");
                divCaptura.Style.Add("display", "block");

                lblMsgError.Text = "Ya existe registro para el año escrito. Intente con otros valores.";

                return;
            }


            obj.Anio = Utilerias.StrToInt(txtAnio.Value);
            obj.Descripcion = txtDescripcion.Value;

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                uow.EjercicioBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.EjercicioBusinessLogic.Update(obj);
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



            BindGrid();

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";

            divEncabezado.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";

            int idEjercicio = Utilerias.StrToInt(_IDEjercicio.Value);

            Ejercicio obj = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

            if (!ValidarEliminarEjercicio(obj))
            {
                M = "No se puede eliminar el registro, se encuentra en uso por otros módulos.";
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                divCaptura.Style.Add("display", "none");
                return;
            }

            uow.EjercicioBusinessLogic.Delete(obj);
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


            BindGrid();

            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");
        }
    }
}