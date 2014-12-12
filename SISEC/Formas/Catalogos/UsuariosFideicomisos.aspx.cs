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
    public partial class UsuariosFideicomisos : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();

            if (!IsPostBack)
            {
                BindGrid();
                BindDropDownEjercicios();
                BindDropDownUsuarios();
                BindDropDownFideicomisos();
                ColocarEjercicioActual();
            }   
        }


        private void ColocarEjercicioActual()
        {
            int anio = DateTime.Now.Year;

            Ejercicio obj = uow.EjercicioBusinessLogic.Get(e => e.Anio==anio).FirstOrDefault();

            if (obj == null)
                obj = uow.EjercicioBusinessLogic.Get(e => e.Anio == anio - 1).FirstOrDefault();

            ddlEjercicioFiltro.SelectedValue = obj.ID.ToString();

        }


        private void BindGrid()
        {
            int ejercicio = Utilerias.StrToInt(ddlEjercicioFiltro.SelectedValue);
            
            List<UsuarioFideicomiso> list=(from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e=>e.EjercicioID==ejercicio)
                                           join ud in uow.UsuarioFideicomisoBusinessLogic.Get()
                                           on df.ID equals ud.DependenciaFideicomisoEjercicioID
                                           select ud).ToList();

            gridUsuarios.DataSource = list;
            gridUsuarios.DataBind();
        }


        private void BindControles()
        {
            int idUsuario = Utilerias.StrToInt(_IDUsuario.Value);

            UsuarioFideicomiso obj = uow.UsuarioFideicomisoBusinessLogic.GetByID(idUsuario);
            ddlUsuario.SelectedValue = obj.UsuarioID.ToString();
            ddlFideicomiso.SelectedValue = obj.DependenciaFideicomisoEjercicioID.ToString();

            DependenciaFideicomisoEjercicio objDependencia = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(obj.DependenciaFideicomisoEjercicioID);
            ddlEjercicio.SelectedValue = objDependencia.EjercicioID.ToString();
        }


        private void BindDropDownEjercicios()
        {
            ddlEjercicio.DataSource = uow.EjercicioBusinessLogic.Get().ToList();
            ddlEjercicio.DataValueField = "ID";
            ddlEjercicio.DataTextField = "Anio";
            ddlEjercicio.DataBind();


            ddlEjercicioFiltro.DataSource = uow.EjercicioBusinessLogic.Get().ToList();
            ddlEjercicioFiltro.DataValueField = "ID";
            ddlEjercicioFiltro.DataTextField = "Anio";
            ddlEjercicioFiltro.DataBind();
        }

        private void BindDropDownUsuarios()
        {
            ddlUsuario.DataSource = uow.UsuarioBusinessLogic.Get().ToList();
            ddlUsuario.DataValueField = "ID";
            ddlUsuario.DataTextField = "Nombre";
            ddlUsuario.DataBind();
        }

        private void BindDropDownFideicomisos()
        {
            int ejercicio = Utilerias.StrToInt(ddlEjercicio.SelectedValue);

            var list = (from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == ejercicio)
                        join f in uow.FideicomisoBusinessLogic.Get()
                        on df.FideicomisoID equals f.ID
                        select new { df.ID, f.Clave });

            ddlFideicomiso.DataSource = list;
            ddlFideicomiso.DataValueField = "ID";
            ddlFideicomiso.DataTextField = "Clave";
            ddlFideicomiso.DataBind();

        }

        public string GetClaveFideicomiso(int ID)
        {
            Fideicomiso obj = (from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get()
                               join f in uow.FideicomisoBusinessLogic.Get()
                               on df.FideicomisoID equals f.ID
                               join ud in uow.UsuarioFideicomisoBusinessLogic.Get(e=>e.ID == ID)
                               on df.ID equals ud.DependenciaFideicomisoEjercicioID
                               select f).FirstOrDefault();

            return obj != null ? obj.Clave : string.Empty;
        }

        public string GetNombreUsuario(int ID)
        {
            Usuario obj = (from ud in uow.UsuarioFideicomisoBusinessLogic.Get(e => e.ID == ID)
                           join u in uow.UsuarioBusinessLogic.Get()
                           on ud.UsuarioID equals u.ID
                           select u).FirstOrDefault();

            return obj != null ? obj.Nombre : string.Empty;
        }

        public string GetEjercicio(int ID)
        {
            Ejercicio obj = (from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get()
                             join ud in uow.UsuarioFideicomisoBusinessLogic.Get(e => e.ID == ID)
                             on df.ID equals ud.DependenciaFideicomisoEjercicioID
                             join e in uow.EjercicioBusinessLogic.Get()
                             on df.EjercicioID equals e.ID
                             select e).FirstOrDefault();

            return obj != null ? obj.Anio.ToString() : string.Empty;
        }


        
        protected void gridUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");
                Label lblUsuario = (Label)e.Row.FindControl("lblUsuario");
                Label lblFideicomiso = (Label)e.Row.FindControl("lblFideicomiso");
                Label lblEjercicio = (Label)e.Row.FindControl("lblEjercicio");

                int id = Utilerias.StrToInt(gridUsuarios.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarID(" + id + ")");

                lblFideicomiso.Text = GetClaveFideicomiso(id);
                lblUsuario.Text = GetNombreUsuario(id);
                lblEjercicio.Text = GetEjercicio(id);

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


        private bool ValidarInsertado(int idFidiecomiso, int idUsuario)
        {
            UsuarioFideicomiso obj = uow.UsuarioFideicomisoBusinessLogic.Get(e => e.DependenciaFideicomisoEjercicioID == idFidiecomiso && e.UsuarioID == idUsuario).FirstOrDefault();

            return obj == null;

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            UsuarioFideicomiso obj;
            int idUsuario = Utilerias.StrToInt(_IDUsuario.Value);
            string M = string.Empty;

            if (!ValidarInsertado(Utilerias.StrToInt(ddlFideicomiso.SelectedValue), Utilerias.StrToInt(ddlUsuario.SelectedValue)))
            {
                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = "Ya existe ese registro. Intente con otros valores.";
                return;
            }


            if (_Accion.Value.Equals("N"))
                obj = new UsuarioFideicomiso();
            else
                obj = uow.UsuarioFideicomisoBusinessLogic.GetByID(idUsuario);

            obj.DependenciaFideicomisoEjercicioID = Utilerias.StrToInt(ddlFideicomiso.SelectedValue);
            obj.UsuarioID = Utilerias.StrToInt(ddlUsuario.SelectedValue);
            

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                uow.UsuarioFideicomisoBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.UsuarioFideicomisoBusinessLogic.Update(obj);
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

            int idUsuario = Utilerias.StrToInt(_IDUsuario.Value);

            UsuarioFideicomiso obj = uow.UsuarioFideicomisoBusinessLogic.GetByID(idUsuario);

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
    }
}