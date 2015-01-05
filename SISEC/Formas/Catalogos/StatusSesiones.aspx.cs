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
    public partial class StatusSesiones : System.Web.UI.Page
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
            gridStatus.DataSource = uow.StatusSesionBusinessLogic.Get().ToList();
            gridStatus.DataBind();
        }


        private void BindControles()
        {
            int idStatus = Utilerias.StrToInt(_IDStatus.Value);

            StatusSesion obj = uow.StatusSesionBusinessLogic.GetByID(idStatus);

            txtClave.Value = obj.Clave;
            txtDescripcion.Value = obj.Descripcion;

        }

        private void ValidarInsercion(string clave, StatusSesion objStatus)
        {
            StatusSesion obj = null;

            if (objStatus == null)
                obj = uow.StatusSesionBusinessLogic.Get(e => e.Clave == clave).FirstOrDefault();

        }
        private bool ValidarEliminarStatus(StatusSesion obj)
        {
            if (obj.DetalleSesiones.Count > 0)
                return false;

            if (obj.DetalleSesionesHistorico.Count > 0)
                return false;

            return true;
        }

        protected void gridStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");

                int id = Utilerias.StrToInt(gridStatus.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarID(" + id + ")");

            }
        }

        protected void gridStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridStatus.PageIndex = e.NewPageIndex;
            BindGrid();
            divEncabezado.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDStatus.Value = gridStatus.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();
            txtClave.Disabled = true;
            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            StatusSesion obj;
            int idStatus = Utilerias.StrToInt(_IDStatus.Value);
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new StatusSesion();
            else
                obj = uow.StatusSesionBusinessLogic.GetByID(idStatus);

            obj.Clave = txtClave.Value;
            obj.Descripcion = txtDescripcion.Value;

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                uow.StatusSesionBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.StatusSesionBusinessLogic.Update(obj);
            }

            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                foreach (string err in uow.Errors)
                    M += err;

                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                divCaptura.Style.Add("display", "none");
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

            int idStatus = Utilerias.StrToInt(_IDStatus.Value);

            StatusSesion obj = uow.StatusSesionBusinessLogic.GetByID(idStatus);

            if (!ValidarEliminarStatus(obj))
            {
                M = "No se puede eliminar el registro, se encuentra en uso por otros módulos.";
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                divCaptura.Style.Add("display", "none");
                return;
            }

            uow.StatusSesionBusinessLogic.Delete(obj);
            uow.SaveChanges();

            if (uow.Errors.Count > 0) //Si hubo errores
            {
                M = string.Empty;
                foreach (string cad in uow.Errors)
                    M += cad;

                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                divCaptura.Style.Add("display", "none");
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