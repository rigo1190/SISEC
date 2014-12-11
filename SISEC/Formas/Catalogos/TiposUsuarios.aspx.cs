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
    public partial class TiposUsuarios : System.Web.UI.Page
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
            gridTipos.DataSource = uow.TipoUsuarioBusinessLogic.Get().ToList();
            gridTipos.DataBind();
        }

        private void BindControles()
        {
            int idTipo = Utilerias.StrToInt(_IDTipo.Value);

            TipoUsuario obj = uow.TipoUsuarioBusinessLogic.GetByID(idTipo);

            txtClave.Value = obj.Clave;
            txtDescripcion.Value = obj.Descripcion;

        }

        private bool ValidarEliminarTipo(TipoUsuario obj)
        {
            if (obj.DetalleUsuarios.Count() > 0)
                return false;

            return true;
        }

        protected void gridTipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");

                int id = Utilerias.StrToInt(gridTipos.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarID(" + id + ")");

            }
        }

        protected void gridTipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridTipos.PageIndex = e.NewPageIndex;
            BindGrid();
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDTipo.Value = gridTipos.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            TipoUsuario obj;
            int idTipo = Utilerias.StrToInt(_IDTipo.Value);
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new TipoUsuario();
            else
                obj = uow.TipoUsuarioBusinessLogic.GetByID(idTipo);

            obj.Clave = txtClave.Value;
            obj.Descripcion = txtDescripcion.Value;

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                uow.TipoUsuarioBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.TipoUsuarioBusinessLogic.Update(obj);
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

            int idTipo = Utilerias.StrToInt(_IDTipo.Value);

            TipoUsuario obj = uow.TipoUsuarioBusinessLogic.GetByID(idTipo);

            if (!ValidarEliminarTipo(obj))
            {
                M = "No se puede eliminar el registro, se encuentra en uso por otros módulos.";
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "none");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }

            uow.TipoUsuarioBusinessLogic.Delete(obj);
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
        }
    }
}