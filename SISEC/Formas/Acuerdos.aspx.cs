using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SISEC.Formas
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
                BindDropDownStatusAcuerdo();

                if (ddlFideicomisos.Items.Count > 0)
                {
                    int idCalendario = BuscarCalendario();
                    BindGridSesiones(idCalendario);
                }
                
            }
        }

        #region METODOS
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
        }
        private void BindDropDownStatusAcuerdo()
        {
            ddlStatus.DataSource = uow.StatusAcuerdoBusinessLogic.Get();
            ddlStatus.DataValueField = "ID";
            ddlStatus.DataTextField = "Descripcion";
            ddlStatus.DataBind();
        }
        private void BindGridSesiones(int idCalendario)
        {
            List<Sesion> list;
            list = uow.SesionBusinessLogic.Get(e => e.CalendarioID == idCalendario).ToList();
            gridSesiones.DataSource = list;
            gridSesiones.DataBind();

            if (list.Count == 0)
            {
                lblAlerta.Text = "No existen sesiones. Capture nuevas sesiones para poder agregar Acuerdos";
                divAlerta.Style.Add("display", "block");
            }
        }
        private void BindGridSeguimientos()
        {
            int idAcuerdo = Utilerias.StrToInt(_IDAcuerdo.Value);

            List<Seguimiento> list = uow.SeguimientoBusinessLogic.Get(e => e.AcuerdoID == idAcuerdo).ToList();
            gridSeguimientos.DataSource = list;
            gridSeguimientos.DataBind();

        }
        private void BindControlesSeguimiento()
        {
            int idSeguimiento = Utilerias.StrToInt(_IDSeguimiento.Value);

            Seguimiento obj = uow.SeguimientoBusinessLogic.GetByID(idSeguimiento);

            txtDescripcion.Value = obj.Descripcion;

        }
        private string GetClaveFideicomiso()
        {
            DependenciaFideicomisoEjercicio ente = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(Utilerias.StrToInt(ddlFideicomisos.SelectedValue));
            Fideicomiso fidei = uow.FideicomisoBusinessLogic.GetByID(ente.FideicomisoID);
            return fidei.Clave;
        }
        private string GetNumSesion()
        {
            Sesion obj = uow.SesionBusinessLogic.GetByID(Utilerias.StrToInt(_IDSesion.Value));
            return obj.NumSesion;
        }
        private int BuscarCalendario()
        {
            int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idCalendario = 0;

            Calendario obj = uow.CalendarioBusinessLogic.Get(c => c.DependenciaFideicomisoEjercicioID == idFideicomiso && c.EjercicioID == idEjercicio).FirstOrDefault();

            if (obj != null)
                idCalendario = obj.ID;

            _IDCalendario.Value = idCalendario.ToString();

            return idCalendario;

        }
        private void BindGridAcuerdos()
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            gridAcuerdos.DataSource = uow.AcuerdoBusinessLogic.Get(e => e.SesionID == idSesion).ToList();
            gridAcuerdos.DataBind();

        }
        private void BindControlesAcuerdo()
        {
            int idAcuerdo = Utilerias.StrToInt(_IDAcuerdo.Value);

            Acuerdo obj = uow.AcuerdoBusinessLogic.GetByID(idAcuerdo);

            txtNumAcuerdo.Value = obj.NumAcuerdo;
            txtNotas.Value = obj.Notas;
            ddlStatus.SelectedValue = obj.StatusAcuerdoID.ToString();

        }

        #endregion

        #region EVENTOS SESIONES
        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCalendario = BuscarCalendario();
            BindGridSesiones(idCalendario);

            divDetalleAcuerdos.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
        }
        protected void gridSesiones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSesiones.PageIndex = e.NewPageIndex;
            BindGridSesiones(Utilerias.StrToInt(_IDCalendario.Value));
            divDetalleAcuerdos.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }
        protected void gridSesiones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                int idSesion = Utilerias.StrToInt(gridSesiones.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                Sesion obj = uow.SesionBusinessLogic.GetByID(idSesion);
                lblStatus.Text = uow.StatusSesionBusinessLogic.GetByID(obj.StatusSesionID).Descripcion;
            }
        }

        #endregion

        #region EVENTOS ACUERDOS
        protected void gridAcuerdos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridAcuerdos.PageIndex = e.NewPageIndex;
            
            BindGridAcuerdos();

            divDetalleAcuerdos.Style.Add("display", "block");
            divEncabezadoDetalle.Style.Add("display", "block");
            
            divEncabezado.Style.Add("display", "none");
            divMenu.Style.Add("display", "none");
            divCapturaDetalle.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void gridAcuerdos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");
                Label lblFideicomiso = (Label)e.Row.FindControl("lblFideicomiso");
                Label lblSesion = (Label)e.Row.FindControl("lblSesion");

                int idAcuerdo = Utilerias.StrToInt(gridAcuerdos.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarIDAcuerdo(" + idAcuerdo + ")");

                lblFideicomiso.Text = GetClaveFideicomiso();
                lblSesion.Text = GetNumSesion();
                
            }
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDAcuerdo.Value = gridAcuerdos.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";
            _AccionS.Value =string.Empty;

            BindControlesAcuerdo();
            BindGridSeguimientos();

            //Se habilitan los controles
            txtNumAcuerdo.Disabled = false;
            txtNotas.Disabled = false;
            ddlStatus.Enabled = true;
            btnGuardar.Enabled = true;

            divMenu.Style.Add("display", "block");
            divCapturaDetalle.Style.Add("display", "block");
            divEncabezadoSeguimiento.Style.Add("display", "none");
            divCapturaSeguimiento.Style.Add("display", "none");

            divEncabezadoDetalle.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

            
        }

        protected void btnAcuerdos_ServerClick(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((HtmlButton)sender).NamingContainer;

            _IDSesion.Value = gridSesiones.DataKeys[row.RowIndex].Value.ToString();

            BindGridAcuerdos();

            divEncabezado.Style.Add("display", "none");
            divDetalleAcuerdos.Style.Add("display", "block");
            divEncabezadoDetalle.Style.Add("display", "block");
            divCapturaDetalle.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int idSesion = Utilerias.StrToInt(_IDSesion.Value);
            Acuerdo obj = null;
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new Acuerdo();
            else
                obj = uow.AcuerdoBusinessLogic.GetByID(Utilerias.StrToInt(_IDAcuerdo.Value));

            obj.Notas = txtNotas.Value;
            obj.NumAcuerdo = txtNumAcuerdo.Value;
            obj.StatusAcuerdoID = Utilerias.StrToInt(ddlStatus.SelectedValue);

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                obj.SesionID = idSesion;
                uow.AcuerdoBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.AcuerdoBusinessLogic.Update(obj);
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

            _IDAcuerdo.Value = obj.ID.ToString(); //Se coloca el ID del nuevo objeto creado

            BindGridAcuerdos();
            BindGridSeguimientos();

            //Se inhabilitan los controles
            txtNumAcuerdo.Disabled = true;
            txtNotas.Disabled = true;
            ddlStatus.Enabled = false;
            btnGuardar.Enabled = false;

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";

            
            divDetalleAcuerdos.Style.Add("display", "block");
            divCapturaDetalle.Style.Add("display", "block");
            divMenu.Style.Add("display", "block");

            divEncabezado.Style.Add("display", "none");
            divEncabezadoDetalle.Style.Add("display", "none");
            divSeguimiento.Style.Add("display", "none");
            divEncabezadoSeguimiento.Style.Add("display", "none");
            divCapturaSeguimiento.Style.Add("display", "none");
           

            
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";

            int idAcuerdo = Utilerias.StrToInt(_IDAcuerdo.Value);

            Acuerdo obj = uow.AcuerdoBusinessLogic.GetByID(idAcuerdo);

            divEncabezadoDetalle.Style.Add("display", "block");
            divCapturaDetalle.Style.Add("display", "none");
            divSeguimiento.Style.Add("display", "none");
            divMenu.Style.Add("display", "none");

            if (obj.DetalleSeguimientos.Count > 0) //Si existen seguimientos para el acuerdo a eliminar, se notifica al usuario
            {
                lblMsgError.Text = "Existen seguimientos para este Acuerdo. Elimine los seguimientos y vuelva a intentarlo";
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }

            //Se elimina el objeto
            uow.AcuerdoBusinessLogic.Delete(obj);
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

            BindGridAcuerdos();

            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
        }

        #endregion 

        #region EVENTOS SEGUIMIENTOS

        protected void imgBtnEditS_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDSeguimiento.Value = gridSeguimientos.DataKeys[row.RowIndex].Value.ToString();
            _AccionS.Value = "A";

            BindControlesSeguimiento();

            divEncabezadoSeguimiento.Style.Add("display", "none");
            divCapturaSeguimiento.Style.Add("display", "block");
            divSeguimiento.Style.Add("display", "block");
            
            divEncabezadoDetalle.Style.Add("display", "none");
            divCapturaDetalle.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

        }

        protected void btnGuardarS_Click(object sender, EventArgs e)
        {
            int idAcuerdo = Utilerias.StrToInt(_IDAcuerdo.Value);
            Seguimiento obj = null;
            string M = string.Empty;

            if (_AccionS.Value.Equals("N"))
                obj = new Seguimiento();
            else
                obj = uow.SeguimientoBusinessLogic.GetByID(Utilerias.StrToInt(_IDSeguimiento.Value));

            obj.Descripcion = txtDescripcion.Value;


            if (_AccionS.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                obj.AcuerdoID= idAcuerdo;
                uow.SeguimientoBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.SeguimientoBusinessLogic.Update(obj);
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

            BindGridSeguimientos();
            _AccionS.Value = string.Empty;

            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            lblMsgSuccess.Text = "Se ha guardado correctamente";

            divDetalleAcuerdos.Style.Add("display", "block");
            divMenu.Style.Add("display", "block");
            divSeguimiento.Style.Add("display", "block");
            divEncabezadoSeguimiento.Style.Add("display", "block");
            
            divCapturaSeguimiento.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "none");
            divEncabezadoDetalle.Style.Add("display", "none");
            divCapturaDetalle.Style.Add("display", "none");
        }

        protected void gridSeguimientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminarS = (ImageButton)e.Row.FindControl("imgBtnEliminarS");
                int idSeguimiento = Utilerias.StrToInt(gridSeguimientos.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (imgBtnEliminarS != null)
                    imgBtnEliminarS.Attributes.Add("onclick", "fnc_ColocarIDSeguimiento(" + idSeguimiento + ")");

            }
        }

        protected void btnDelS_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";

            int idSeguimiento = Utilerias.StrToInt(_IDSeguimiento.Value);

            Seguimiento obj = uow.SeguimientoBusinessLogic.GetByID(idSeguimiento);

            //Se elimina el objeto
            uow.SeguimientoBusinessLogic.Delete(obj);
            uow.SaveChanges();

            divSeguimiento.Style.Add("display", "block");
            divEncabezadoSeguimiento.Style.Add("display", "block");

            divCapturaSeguimiento.Style.Add("display", "none");
            divEncabezadoDetalle.Style.Add("display", "none");
            divCapturaDetalle.Style.Add("display", "none");
            divSeguimiento.Style.Add("display", "block");
            divMenu.Style.Add("display", "block");



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

            BindGridSeguimientos();

            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
        }

        protected void gridSeguimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSeguimientos.PageIndex = e.NewPageIndex;

            BindGridSeguimientos();

            divMenu.Style.Add("display", "block");
            divSeguimiento.Style.Add("display", "block");
            divEncabezadoSeguimiento.Style.Add("display", "block");
            divCapturaDetalle.Style.Add("display", "none");

        }


        #endregion

    }
}