using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC.Formas.Catalogos
{
    public partial class StatusAcuerdos : System.Web.UI.Page
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
            gridStatus.DataSource = uow.StatusAcuerdoBusinessLogic.Get().ToList();
            gridStatus.DataBind();
        }


        private void BindControles()
        {
            int idStatus = Utilerias.StrToInt(_IDStatus.Value);

            StatusAcuerdo obj = uow.StatusAcuerdoBusinessLogic.GetByID(idStatus);
            txtClave.Value = obj.Clave;
            chkInicio.Checked = Convert.ToBoolean(obj.Inicial);
            txtDescripcion.Value = obj.Descripcion;
            chkInicio.Enabled = false;

            StatusAcuerdo objInicial = uow.StatusAcuerdoBusinessLogic.Get(e => e.Inicial == true).FirstOrDefault();

            if (objInicial != null)
            {
                if (obj.ID == objInicial.ID)
                    chkInicio.Enabled = true;
            }
            else
                chkInicio.Enabled = true;

        }

        [WebMethod]
        public static string GetValorNuevo(string param)
        {
            UnitOfWork uow = new UnitOfWork();
            StatusAcuerdo obj = uow.StatusAcuerdoBusinessLogic.Get(e => e.Inicial == true).FirstOrDefault();
            return (obj == null).ToString();
            
        }

        private bool ValidarEliminarStatus(StatusAcuerdo obj)
        {
            if (obj.DetalleAcuerdos.Count > 0)
                return false;

            return true;
        }

        private bool ValidarInsercion(bool inicial,StatusAcuerdo objStatus = null)
        {
            StatusAcuerdo obj = null;

            if (objStatus == null)
                obj = uow.StatusAcuerdoBusinessLogic.Get(e => e.Inicial == inicial).FirstOrDefault();
            else
                if (inicial != objStatus.Inicial)
                    obj = uow.StatusAcuerdoBusinessLogic.Get(e => e.Inicial == inicial).FirstOrDefault();

            return obj == null;
        }

        protected void gridStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridStatus.PageIndex = e.NewPageIndex;
            BindGrid();
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDStatus.Value = gridStatus.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            StatusAcuerdo obj;
            int idStatus = Utilerias.StrToInt(_IDStatus.Value);
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new StatusAcuerdo();
            else
                obj = uow.StatusAcuerdoBusinessLogic.GetByID(idStatus);

            obj.Clave = txtClave.Value;
            obj.Descripcion = txtDescripcion.Value;
            obj.Inicial = chkInicio.Checked;

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                uow.StatusAcuerdoBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.StatusAcuerdoBusinessLogic.Update(obj);
            }

            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                foreach (string err in uow.Errors)
                    M += err;

                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "none");
                divCaptura.Style.Add("display", "block");
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

            StatusAcuerdo obj = uow.StatusAcuerdoBusinessLogic.GetByID(idStatus);

            if (!ValidarEliminarStatus(obj))
            {
                M = "No se puede eliminar el registro, se encuentra en uso por otros módulos.";
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divCaptura.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                return;
            }

            uow.StatusAcuerdoBusinessLogic.Delete(obj);
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
            divCaptura.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
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
    }
}