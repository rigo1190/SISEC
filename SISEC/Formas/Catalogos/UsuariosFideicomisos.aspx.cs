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
                BindDropDownUsuarios();
                BindDropDownEjercicios();
                BindDropDownFideicomisos();
                BindGrid();
                //ImportarFideicomisosAsignadosEjercicioPasado();
            }   
        }


        private void ImportarFideicomisosAsignadosEjercicioPasado()
        {
            int ejercicioActual = DateTime.Now.Year;
            string M = string.Empty;

            Ejercicio objActual = uow.EjercicioBusinessLogic.Get(e => e.Anio == ejercicioActual).FirstOrDefault();

            if (objActual == null)
            {
                lblMsgError.Text = "No se puede importar la información del ejercicio pasado. No existe un registro para el Ejercicio actual en el CATÁLOGO de EJERCICIOS. Captúrelo y vuelva a intentarlo.";
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divCaptura.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                return;
            }

            if (uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == objActual.ID).Count() == 0)
            {
                lblMsgError.Text = "No se puede importar la información del ejercicio pasado. No existen FIDEICOMISOS relacionados al ejercicio actual. Captúrelos y vuelva a intentarlo.";
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divCaptura.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                return;
            }

            var listActual = (from uf in uow.UsuarioFideicomisoBusinessLogic.Get()
                              join df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == objActual.ID)
                              on uf.DependenciaFideicomisoEjercicioID equals df.ID
                              join f in uow.FideicomisoBusinessLogic.Get()
                              on df.FideicomisoID equals f.ID
                              select new { uf.UsuarioID, FideicomisoID = f.ID });

            ejercicioActual = ejercicioActual - 1;
            Ejercicio objAnterior = uow.EjercicioBusinessLogic.Get(e => e.Anio == ejercicioActual).FirstOrDefault();

            var listAnterior = (from uf in uow.UsuarioFideicomisoBusinessLogic.Get()
                                join df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == objAnterior.ID)
                                on uf.DependenciaFideicomisoEjercicioID equals df.ID
                                join f in uow.FideicomisoBusinessLogic.Get()
                                on df.FideicomisoID equals f.ID
                                select new { uf.UsuarioID, FideicomisoID = f.ID });


            var list = (from anterior in listAnterior
                        join actual in listActual
                        on anterior.FideicomisoID equals actual.FideicomisoID into temp
                        from diferencia in temp.DefaultIfEmpty()
                        select new { anterior.FideicomisoID, anterior.UsuarioID, UsuarioAnterior = (diferencia == null) ? 0 : diferencia.FideicomisoID });


            if (list.Count() == 0)
            {
                lblMsgError.Text = "La información de Fideicomisos asignados a analistas del ejercicio anterior ya existe para el ejercicio actual. No es necesario importar.";
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divCaptura.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                return;
            }

            //SE RECORRE EL RESULTADO PARA CREAR LOS FIDEICOMISOS ASIGNADOS ANTERIORES EN EL EJERCICIO ACTUAL
            foreach (var f in list)
            {
                if (f.UsuarioAnterior == 0)
                {
                    UsuarioFideicomiso objUsuarioFideicomiso = new UsuarioFideicomiso();
                    DependenciaFideicomisoEjercicio objDF = uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.FideicomisoID == f.FideicomisoID && e.EjercicioID == objActual.ID).FirstOrDefault();

                    objUsuarioFideicomiso.UsuarioID = f.UsuarioID;
                    objUsuarioFideicomiso.DependenciaFideicomisoEjercicioID = objDF.ID;

                    uow.UsuarioFideicomisoBusinessLogic.Insert(objUsuarioFideicomiso);
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
                        divMsgError.Style.Add("display", "block");
                        divMsgSuccess.Style.Add("display", "none");
                        divCaptura.Style.Add("display", "none");
                        divEncabezado.Style.Add("display", "block");
                        return;
                    }

                }
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

            int idUsuario = Utilerias.StrToInt(ddlUsuariosFiltro.SelectedValue);

            List<UsuarioFideicomiso> list=null;

            if (idUsuario==0)
                 list=(from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e=>e.EjercicioID==ejercicio)
                        join ud in uow.UsuarioFideicomisoBusinessLogic.Get()
                        on df.ID equals ud.DependenciaFideicomisoEjercicioID
                       select ud).OrderBy(e => e.UsuarioID).ToList();
            else
                list = (from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == ejercicio)
                        join ud in uow.UsuarioFideicomisoBusinessLogic.Get(e=>e.UsuarioID==idUsuario)
                        on df.ID equals ud.DependenciaFideicomisoEjercicioID
                        select ud).OrderBy(e=>e.UsuarioID).ToList();


            gridUsuarios.DataSource = list;
            gridUsuarios.DataBind();
        }


        private void BindControles()
        {
            int idUsuario = Utilerias.StrToInt(_IDUsuario.Value);

            UsuarioFideicomiso obj = uow.UsuarioFideicomisoBusinessLogic.GetByID(idUsuario);
            ddlUsuario.SelectedValue = obj.UsuarioID.ToString();

            DependenciaFideicomisoEjercicio objDependencia = uow.DependenciaFideicomisoEjercicioBusinessLogic.GetByID(obj.DependenciaFideicomisoEjercicioID);
            ddlEjercicio.SelectedValue = objDependencia.EjercicioID.ToString();
            BindDropDownFideicomisos();

            ddlFideicomiso.SelectedValue = obj.DependenciaFideicomisoEjercicioID.ToString();

            
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

            ColocarEjercicioActual();
            ddlEjercicio.SelectedValue = ddlEjercicioFiltro.SelectedValue;
        }

        private void BindDropDownUsuarios()
        {
            ddlUsuario.DataSource = uow.UsuarioBusinessLogic.Get().ToList();
            ddlUsuario.DataValueField = "ID";
            ddlUsuario.DataTextField = "Nombre";
            ddlUsuario.DataBind();

            ddlUsuariosFiltro.DataSource = uow.UsuarioBusinessLogic.Get().ToList();
            ddlUsuariosFiltro.DataValueField = "ID";
            ddlUsuariosFiltro.DataTextField = "Nombre";
            ddlUsuariosFiltro.DataBind();

            ddlUsuariosFiltro.Items.Insert(0,new ListItem("Seleccione...", "0"));
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
            divCaptura.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }


        private bool ValidarInsertado(int idFidiecomiso, int idUsuario,UsuarioFideicomiso objUsuario=null)
        {
            UsuarioFideicomiso obj = null;

            if (objUsuario==null)
               obj = uow.UsuarioFideicomisoBusinessLogic.Get(e => e.DependenciaFideicomisoEjercicioID == idFidiecomiso && e.UsuarioID == idUsuario).FirstOrDefault();
            else
                if (idFidiecomiso!=objUsuario.DependenciaFideicomisoEjercicioID || idUsuario!=objUsuario.UsuarioID)
                    obj = uow.UsuarioFideicomisoBusinessLogic.Get(e => e.DependenciaFideicomisoEjercicioID == idFidiecomiso && e.UsuarioID == idUsuario).FirstOrDefault();

            return obj == null;

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            UsuarioFideicomiso obj;
            int idUsuario = Utilerias.StrToInt(_IDUsuario.Value);
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new UsuarioFideicomiso();
            else
                obj = uow.UsuarioFideicomisoBusinessLogic.GetByID(idUsuario);

            if (!ValidarInsertado(Utilerias.StrToInt(ddlFideicomiso.SelectedValue), Utilerias.StrToInt(ddlUsuario.SelectedValue),!_Accion.Value.Equals("N")?obj:null))
            {
                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divCaptura.Style.Add("display", "block");
                divEncabezado.Style.Add("display", "none");
                lblMsgError.Text = "Ya existe ese registro. Intente con otros valores.";
                return;
            }

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
                divCaptura.Style.Add("display", "block");
                divEncabezado.Style.Add("display", "none");
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

            uow.UsuarioFideicomisoBusinessLogic.Delete(obj);
            uow.SaveChanges();

            if (uow.Errors.Count > 0) //Si hubo errores
            {
                M = string.Empty;
                foreach (string cad in uow.Errors)
                    M += cad;

                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divCaptura.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                return;
            }


            BindGrid();

            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDUsuario.Value = gridUsuarios.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();

            ddlEjercicio.Enabled = false;
            ddlUsuario.Enabled = false;

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void ddlEjercicioFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEjercicio.SelectedValue = ddlEjercicioFiltro.SelectedValue;
            BindDropDownFideicomisos();
            BindGrid();
            divCaptura.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void ddlUsuariosFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();

            divCaptura.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void ddlEjercicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownFideicomisos();
            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void btnNuevo_ServerClick(object sender, EventArgs e)
        {
            _Accion.Value = "N";

            ddlEjercicio.SelectedValue = ddlEjercicioFiltro.SelectedValue;

            BindDropDownFideicomisos();

            if (ddlFideicomiso.Items.Count > 0)
                ddlFideicomiso.SelectedValue = ddlFideicomiso.Items[0].Value;

            if (!ddlUsuariosFiltro.SelectedValue.Equals("0"))
                ddlUsuario.SelectedValue = ddlUsuariosFiltro.SelectedValue;

            ddlFideicomiso.Enabled = true;
            ddlEjercicio.Enabled = true;
            ddlUsuario.Enabled = true;

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        

       
    }
}