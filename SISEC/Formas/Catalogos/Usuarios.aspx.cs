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
    public partial class Usuarios : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();
            if (!IsPostBack)
            {
                BindGrid();
                BindDropDownTipoUsuario();
            }
        }

        private void BindGrid()
        {
            gridUsuarios.DataSource = uow.UsuarioBusinessLogic.Get().ToList();
            gridUsuarios.DataBind();
        }

        private void BindDropDownTipoUsuario()
        {
            ddlTipoUsuario.DataSource = uow.TipoUsuarioBusinessLogic.Get().ToList();
            ddlTipoUsuario.DataValueField = "ID";
            ddlTipoUsuario.DataTextField = "Descripcion";
            ddlTipoUsuario.DataBind();
        }

        private void BindControles()
        {
            int idUsuario = Utilerias.StrToInt(_IDUsuario.Value);

            Usuario obj = uow.UsuarioBusinessLogic.GetByID(idUsuario);

            txtNombre.Value = obj.Nombre;
            txtPassword.Value = obj.Password;
            txtLogin.Value = obj.Login;
            chkActivo.Checked = Convert.ToBoolean(obj.Activo);
            chkBloqueado.Checked = Convert.ToBoolean(obj.Bloqueado);
            ddlTipoUsuario.SelectedValue = obj.TipoUsuarioID.ToString();

        }

        private bool ValidarEliminarUsuario(Usuario obj)
        {
            List<Calendario> list= uow.CalendarioBusinessLogic.Get(e=>e.UsuarioCaptura==obj.Login || e.UsuarioModifica==obj.Login).ToList();

            if (list.Count > 0)
                return false;

            return true;
        }

        protected void gridUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");

                int id = Utilerias.StrToInt(gridUsuarios.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarID(" + id + ")");

            }
        }

        protected void gridUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridUsuarios.PageIndex = e.NewPageIndex;
            BindGrid();
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDUsuario.Value = gridUsuarios.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Usuario obj;
            int idUsuario = Utilerias.StrToInt(_IDUsuario.Value);
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new Usuario();
            else
                obj = uow.UsuarioBusinessLogic.GetByID(idUsuario);

            obj.Nombre = txtNombre.Value;
            obj.Login = txtLogin.Value;
            obj.Password = txtPassword.Value;
            obj.TipoUsuarioID = Utilerias.StrToInt(ddlTipoUsuario.SelectedValue);

            if (_Accion.Value.Equals("N"))
            {
                uow.UsuarioBusinessLogic.Insert(obj);
            }
            else
            {
                uow.UsuarioBusinessLogic.Update(obj);
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

            int idTipo = Utilerias.StrToInt(_IDUsuario.Value);

            Usuario obj = uow.UsuarioBusinessLogic.GetByID(idTipo);

            if (!ValidarEliminarUsuario(obj))
            {
                M = "No se puede eliminar el registro, se encuentra en uso por otros módulos.";
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "none");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }

            uow.UsuarioBusinessLogic.Delete(obj);
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