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
    public partial class FideicomisosEjercicio : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();

            if (!IsPostBack)
            {
                BindDropDownEjercicios();
                BindDropDownFideicomisos();
                ColocarEjercicioActual();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            int ejercicio = Utilerias.StrToInt(ddlEjercicioFiltro.SelectedValue);

            gridFideicomisos.DataSource = uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e=>e.EjercicioID==ejercicio).ToList();
            gridFideicomisos.DataBind();
        }


        private void ColocarEjercicioActual()
        {
            int anio = DateTime.Now.Year;

            Ejercicio obj = uow.EjercicioBusinessLogic.Get(e => e.Anio == anio).FirstOrDefault();

            if (obj == null)
                obj = uow.EjercicioBusinessLogic.Get(e => e.Anio == anio - 1).FirstOrDefault();

            ddlEjercicioFiltro.SelectedValue = obj.ID.ToString();

        }
        public string GetClaveFideicomiso(int ID)
        {
            Fideicomiso obj = (from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.ID == ID)
                               join f in uow.FideicomisoBusinessLogic.Get()
                               on df.FideicomisoID equals f.ID
                               select f).FirstOrDefault();

            return obj != null ? obj.Clave : string.Empty;
        }

        public string GetEjercicio(int ID)
        {
            DependenciaFideicomisoEjercicio fidiecomiso = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(ID);
            Ejercicio ejercicio = uow.EjercicioBusinessLogic.GetByID(fidiecomiso.EjercicioID);

            return ejercicio != null ? ejercicio.Anio.ToString() : string.Empty;
        }

        private bool ValidarInsertado(int idFidiecomiso, int idEjercicio)
        {
            DependenciaFideicomisoEjercicio obj = uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.ID == idFidiecomiso && e.EjercicioID == idEjercicio).FirstOrDefault();

            return obj == null;

        }

        private bool ValidacionEliminacion(DependenciaFideicomisoEjercicio obj)
        {
            if (obj.DetalleCalendarios.Count>0)
                return false;

            if (obj.DetalleFichasTecnicas.Count > 0)
                return false;

            if (obj.DetallesNormatividad.Count > 0)
                return false;

            if (obj.DetalleUsuarios.Count > 0)
                return false;

            return true;

        }

        private void BindDropDownEjercicios()
        {
            ddlEjercicio.DataSource = uow.EjercicioBusinessLogic.Get().OrderByDescending(e=>e.Anio).ToList();
            ddlEjercicio.DataValueField = "ID";
            ddlEjercicio.DataTextField = "Anio";
            ddlEjercicio.DataBind();


            ddlEjercicioFiltro.DataSource = uow.EjercicioBusinessLogic.Get().OrderByDescending(e => e.Anio).ToList();
            ddlEjercicioFiltro.DataValueField = "ID";
            ddlEjercicioFiltro.DataTextField = "Anio";
            ddlEjercicioFiltro.DataBind();
        }


        private void BindDropDownFideicomisos()
        {
            ddlFideicomiso.DataSource = uow.FideicomisoBusinessLogic.Get().ToList();
            ddlFideicomiso.DataValueField = "ID";
            ddlFideicomiso.DataTextField = "Clave";
            ddlFideicomiso.DataBind();
        }


        private void ImportarFedeicomisosEjercicioPasado()
        {
            int ejercicioActual = DateTime.Now.Year;
            string M=string.Empty;

            Ejercicio objActual = uow.EjercicioBusinessLogic.Get(e => e.Anio == ejercicioActual).FirstOrDefault();

            if (objActual == null)
            {
                lblMsgError.Text = "No se puede importar la información del ejercicio pasado. No existe un registro para el Ejercicio actual en el CATÁLOGO de EJERCICIOS. Captúrelo y vuelva a intentarlo.";
                divMsgError.Style.Add("display", "none");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }

            List<DependenciaFideicomisoEjercicio> listActual = uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == objActual.ID).ToList();
            Ejercicio objAnterior = uow.EjercicioBusinessLogic.Get(e => e.Anio == ejercicioActual - 1).FirstOrDefault();
            List<DependenciaFideicomisoEjercicio>  listAnterior = uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == objAnterior.ID).ToList();


            var list = (from anterior in listAnterior
                        join actual in listActual
                        on anterior.FideicomisoID equals actual.FideicomisoID into temp
                        from diferencia in temp.DefaultIfEmpty()
                        select new { anterior.FideicomisoID, anterior.Activo, fideicomisoAnterior = (diferencia == null) ? 0 : diferencia.FideicomisoID });

            if (list.Count()==0)
            {
                lblMsgError.Text = "La información de Fideicomisos del ejercicio anterior ya existe para el ejercicio actual. No es necesario importar.";
                divMsgError.Style.Add("display", "none");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }

            //SE RECORRE EL RESULTADO PARA CREAR LOS FIDEICOMISOS ANTERIORES EN EL EJERCICIO ACTUAL
            foreach (var f in list)
            {
                if (f.fideicomisoAnterior == 0)
                {
                    DependenciaFideicomisoEjercicio obj = new DependenciaFideicomisoEjercicio();

                    obj.EjercicioID = objActual.ID;
                    obj.FideicomisoID = f.FideicomisoID;
                    obj.Activo = f.Activo;

                    uow.DependenciaFideicomisoEjercicioBusinessLogic.Insert(obj);
                    uow.SaveChanges();

                    //SI HUBO ERRORES
                    if (uow.Errors.Count > 0)
                    {
                        foreach (string err in uow.Errors)
                            M += err;
                    }

                    if (!M.Equals(string.Empty))
                    {
                        lblMsgError.Text = M;
                        divMsgError.Style.Add("display", "none");
                        divMsgSuccess.Style.Add("display", "none");
                        return;
                    }

                }
            }

        }

        protected void gridFideicomisos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");
                Label lblFideicomiso = (Label)e.Row.FindControl("lblFideicomiso");
                Label lblEjercicio = (Label)e.Row.FindControl("lblEjercicio");

                int id = Utilerias.StrToInt(gridFideicomisos.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarID(" + id + ")");

                lblFideicomiso.Text = GetClaveFideicomiso(id);
                lblEjercicio.Text = GetEjercicio(id);

            }
        }


        private void BindControles()
        {
            int idTipo = Utilerias.StrToInt(_IDFideicomiso.Value);

            DependenciaFideicomisoEjercicio obj = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(idTipo);

            ddlEjercicio.SelectedValue = obj.EjercicioID.ToString();
            ddlFideicomiso.SelectedValue = obj.FideicomisoID.ToString();

        }

        protected void gridFideicomisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridFideicomisos.PageIndex = e.NewPageIndex;
            BindGrid();
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDFideicomiso.Value = gridFideicomisos.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";

            int idFideicomiso = Utilerias.StrToInt(_IDFideicomiso.Value);

            DependenciaFideicomisoEjercicio obj = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(idFideicomiso);

            if (!ValidacionEliminacion(obj))
            {
                M = "No se puede eliminar el registro, se encuentra en uso por otros módulos.";
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "none");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }
            
            
            uow.DependenciaFideicomisoEjercicioBusinessLogic.Delete(obj);
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DependenciaFideicomisoEjercicio obj;
            int idUsuario = Utilerias.StrToInt(_IDFideicomiso.Value);
            string M = string.Empty;

            if (!ValidarInsertado(Utilerias.StrToInt(ddlFideicomiso.SelectedValue), Utilerias.StrToInt(ddlEjercicio.SelectedValue)))
            {
                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                lblMsgError.Text = "Ya existe ese registro. Intente con otros valores.";
                return;
            }


            if (_Accion.Value.Equals("N"))
                obj = new DependenciaFideicomisoEjercicio();
            else
                obj = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(idUsuario);

            obj.FideicomisoID = Utilerias.StrToInt(ddlFideicomiso.SelectedValue);
            obj.EjercicioID = Utilerias.StrToInt(ddlEjercicio.SelectedValue);
            obj.Activo = chkActivo.Checked;

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                uow.DependenciaFideicomisoEjercicioBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.DependenciaFideicomisoEjercicioBusinessLogic.Update(obj);
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
    }
}