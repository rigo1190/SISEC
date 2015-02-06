using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SISEC.Reports
{
    public partial class SintesisInformativa : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();
            if (!IsPostBack)
            {
                BindDropDownFideicomisos();
                _Ejercicio.Value = Session["Ejercicio"].ToString();
                _URL.Value = ResolveClientUrl("~/Reports/ReportView.aspx");
                _IDUser.Value = Session["UserID"].ToString();

                string M = string.Empty;
                
                BindGrid();
                
                M=CrearObjetoReporte();

                if (!M.Equals(string.Empty))
                {
                    divMsgError.Attributes.Add("display", "block");
                    lblMsgError.Text = M;
                }
            }
        }


        private string CrearObjetoReporte()
        {
            int idUser = Utilerias.StrToInt(_IDUser.Value);
            bool eliminados=uow.RptSintesisInformativaBusinessLogic.DeleteAll(e=>e.UsuarioID==idUser);
            string M = string.Empty;

            if (eliminados)
            {
                uow.SaveChanges();

                //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }



                int idFideicomiso = Utilerias.StrToInt(_IDFideicomiso.Value);

                //SE OBTIENEN TODOS LOS REGISTROS
                var listSintesis = (from f in uow.FideicomisoBusinessLogic.Get()
                                    join si in uow.FichaTecnicaBusinessLogic.Get()
                                    on f.ID equals si.FideicomisoID
                                    select new
                                    {
                                        FichaTecnicaID = si.ID, //ID de la ficha tecnica
                                        FideicomisoID = f.ID,
                                        Nombre = f.Descripcion, //Nombre del Fideicomiso
                                        ResponsableOperativo = si.ResponsableOperativo,
                                        Finalidad = si.Finalidad,
                                        Creacion = si.Creacion,
                                        Formalizacion = si.Formalizacion,
                                        Partes = si.Partes,
                                        Modificaciones = si.Modificaciones,
                                        Integracion = si.ComiteTecnico,
                                        Reglas = si.ReglasOperacion,
                                        Estructura = si.EstructuraAdministrativa,
                                        Calendario = si.Calendario,
                                        Presupuesto = si.PresupuestoAnual,
                                        Situacion = si.SituacionPatrimonial
                                    });

                if (idFideicomiso > 0)//SE FILTRA POR EL FIDEICOMISO SELECCIONADO, SI ES QUE SE ELIGIO ALGUNO 
                {

                    listSintesis = (from f in uow.FideicomisoBusinessLogic.Get(e=>e.ID==idFideicomiso)
                                    join si in uow.FichaTecnicaBusinessLogic.Get()
                                    on f.ID equals si.FideicomisoID
                                    select new
                                    {
                                        FichaTecnicaID = si.ID, //ID de la ficha tecnica
                                        FideicomisoID = f.ID,
                                        Nombre = f.Descripcion, //Nombre del Fideicomiso
                                        ResponsableOperativo = si.ResponsableOperativo,
                                        Finalidad = si.Finalidad,
                                        Creacion = si.Creacion,
                                        Formalizacion = si.Formalizacion,
                                        Partes = si.Partes,
                                        Modificaciones = si.Modificaciones,
                                        Integracion = si.ComiteTecnico,
                                        Reglas = si.ReglasOperacion,
                                        Estructura = si.EstructuraAdministrativa,
                                        Calendario = si.Calendario,
                                        Presupuesto = si.PresupuestoAnual,
                                        Situacion = si.SituacionPatrimonial
                                    });
                }

                
                foreach (var item in listSintesis)
                {

                    List<PropertyInfo> propiedades= item.GetType().GetProperties().ToList();

                    foreach (PropertyInfo propiedad in propiedades)
                    {
                        DAL.Model.rptSintesisInformativa rpt=null;

                        switch (propiedad.Name)
                        {
                            case "ResponsableOperativo":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Responsable Operativo";
                                 rpt.ValorCampo = item.ResponsableOperativo;
                                break;
                            case "Finalidad":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Finalidad del Fideicomiso";
                                 rpt.ValorCampo = item.Finalidad;
                                break;
                            case "Creacion":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Creación";
                                 rpt.ValorCampo = item.Creacion;
                                break;
                            case "Formalizacion":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Formalización";
                                 rpt.ValorCampo = item.Formalizacion;
                                break;
                            case "Partes":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Partes del Fideicomiso";
                                 rpt.ValorCampo = item.Partes;
                                break;
                            case "Modificaciones":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Modificaciones al Decreto de Creación y Contrato de Fideicomiso";
                                 rpt.ValorCampo = item.Modificaciones;
                                break;
                            case "Integracion":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Integración del Comité Técnico";
                                 rpt.ValorCampo = item.Integracion;
                                break;
                            case "Reglas":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Reglas de Operación";
                                 rpt.ValorCampo = item.Reglas;
                                break;
                            case "Estructura":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Estructura Administrativa";
                                 rpt.ValorCampo = item.Estructura;
                                break;
                            case "Calendario":
                                 Ejercicio ejercicio=uow.EjercicioBusinessLogic.GetByID(Utilerias.StrToInt(Session["Ejercicio"].ToString()));
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Calendario de Sesiones " + ejercicio.Anio;
                                 rpt.ValorCampo = item.Calendario;
                                break;
                            case "Presupuesto":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Presupuesto Anual";
                                 rpt.ValorCampo = item.Presupuesto;
                                break;
                            case "Situacion":
                                 rpt = new DAL.Model.rptSintesisInformativa();
                                 rpt.NombreCampo = "Situacion Patrimonial";
                                 rpt.ValorCampo = item.Situacion;
                                break;
                        }

                        if (rpt != null)
                        {
                            rpt.FichaTecnicaID = item.FichaTecnicaID;
                            rpt.FideicomisoID = item.FideicomisoID;
                            rpt.NombreFideicomiso = item.Nombre;
                            rpt.UsuarioID = idUser;

                            uow.RptSintesisInformativaBusinessLogic.Insert(rpt);
                            uow.SaveChanges();

                            
                            if (uow.Errors.Count > 0)
                            {
                                foreach (string m in uow.Errors)
                                    M += m;

                                return M;
                            }

       
                        }
                    }

                }

            }

            return M;

        }

        [WebMethod]
        public static string CrearSintesisHistorico(int idSintesis)
        {
            UnitOfWork uow = new UnitOfWork();
            int idUser = Utilerias.StrToInt(HttpContext.Current.Session["UserID"].ToString());

            bool eliminados = uow.RptSintesisInformativaHistoricoBusinessLogic.DeleteAll(e=>e.UsuarioID==idUser);

            string M = string.Empty;
            

            if (eliminados)
            {
                uow.SaveChanges();

                //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }

                var listSintesis = (from f in uow.FideicomisoBusinessLogic.Get()
                                    join si in uow.FichaTecnicaBusinessLogic.Get(e => e.ID == idSintesis)
                                    on f.ID equals si.FideicomisoID
                                    join sih in uow.FichaTecnicaHistoricoBusinessLogic.Get()
                                    on si.ID equals sih.FichaTecnicaID
                                    select new
                                    {
                                        FichaTecnicaID = sih.ID, //ID de la ficha tecnica
                                        FideicomisoID = f.ID,
                                        Nombre = f.Descripcion, //Nombre del Fideicomiso
                                        ResponsableOperativo = sih.ResponsableOperativo,
                                        Finalidad = sih.Finalidad,
                                        Creacion = sih.Creacion,
                                        Formalizacion = sih.Formalizacion,
                                        Partes = sih.Partes,
                                        Modificaciones = sih.Modificaciones,
                                        Integracion = sih.ComiteTecnico,
                                        Reglas = sih.ReglasOperacion,
                                        Estructura = sih.EstructuraAdministrativa,
                                        Calendario = sih.Calendario,
                                        Presupuesto = sih.PresupuestoAnual,
                                        Situacion = sih.SituacionPatrimonial,
                                        FechaCaptura=sih.FechaCaptura
                                    });

                foreach (var item in listSintesis)
                {

                    List<PropertyInfo> propiedades = item.GetType().GetProperties().ToList();

                    foreach (PropertyInfo propiedad in propiedades)
                    {
                        DAL.Model.rptSintesisInformativaHistorico rpt = null;

                        switch (propiedad.Name)
                        {
                            case "ResponsableOperativo":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Responsable Operativo";
                                rpt.ValorCampo = item.ResponsableOperativo;
                                break;
                            case "Finalidad":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Finalidad del Fideicomiso";
                                rpt.ValorCampo = item.Finalidad;
                                break;
                            case "Creacion":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Creación";
                                rpt.ValorCampo = item.Creacion;
                                break;
                            case "Formalizacion":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Formalización";
                                rpt.ValorCampo = item.Formalizacion;
                                break;
                            case "Partes":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Partes del Fideicomiso";
                                rpt.ValorCampo = item.Partes;
                                break;
                            case "Modificaciones":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Modificaciones al Decreto de Creación y Contrato de Fideicomiso";
                                rpt.ValorCampo = item.Modificaciones;
                                break;
                            case "Integracion":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Integración del Comité Técnico";
                                rpt.ValorCampo = item.Integracion;
                                break;
                            case "Reglas":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Reglas de Operación";
                                rpt.ValorCampo = item.Reglas;
                                break;
                            case "Estructura":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Estructura Administrativa";
                                rpt.ValorCampo = item.Estructura;
                                break;
                            case "Calendario":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Calendario de Sesiones ";
                                rpt.ValorCampo = item.Calendario;
                                break;
                            case "Presupuesto":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Presupuesto Anual";
                                rpt.ValorCampo = item.Presupuesto;
                                break;
                            case "Situacion":
                                rpt = new DAL.Model.rptSintesisInformativaHistorico();
                                rpt.NombreCampo = "Situacion Patrimonial";
                                rpt.ValorCampo = item.Situacion;
                                break;
                        }

                        if (rpt != null)
                        {
                            rpt.FichaTecnicaID = item.FichaTecnicaID;
                            rpt.FideicomisoID = item.FideicomisoID;
                            rpt.NombreFideicomiso = item.Nombre;
                            rpt.FechaModificacion = item.FechaCaptura;
                            rpt.UsuarioID = idUser;

                            uow.RptSintesisInformativaHistoricoBusinessLogic.Insert(rpt);
                            uow.SaveChanges();


                            if (uow.Errors.Count > 0)
                            {
                                foreach (string m in uow.Errors)
                                    M += m;

                                return M;
                            }


                        }
                    }

                }

            }

            return M;
        }

        [WebMethod]
        public static List<string> Test(string[] args)
        {
            List<string> R=new List<string>();

            R.Add("Funcion exitosa");



            return R;
        }

        private void BindGrid()
        {
            int idFideicomiso = Utilerias.StrToInt(_IDFideicomiso.Value);
            List<FichaTecnica> list;

            if (idFideicomiso == 0)
                list = uow.FichaTecnicaBusinessLogic.Get().ToList();
            else
                list = uow.FichaTecnicaBusinessLogic.Get(e => e.FideicomisoID == idFideicomiso).ToList();

            lblResultado.Text = "Resultado: " + list.Count.ToString() + " registros";

            gridSintesis.DataSource = list;

            gridSintesis.DataBind();
        }

        private void BindDropDownFideicomisos()
        {

            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idUser = Utilerias.StrToInt(Session["UserID"].ToString());

            var list = (from df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == idEjercicio)
                        join ud in uow.UsuarioFideicomisoBusinessLogic.Get(e => e.UsuarioID == idUser)
                        on df.ID equals ud.DependenciaFideicomisoEjercicioID
                        join f in uow.FideicomisoBusinessLogic.Get()
                        on df.FideicomisoID equals f.ID
                        select new { f.ID, f.Clave });

            ddlFideicomisos.DataSource = list;
            ddlFideicomisos.DataValueField = "ID";
            ddlFideicomisos.DataTextField = "Clave";
            ddlFideicomisos.DataBind();

            ddlFideicomisos.Items.Insert(0, new ListItem("Seleccione...", "0"));

            ddlFideicomisos.SelectedValue = "0";
            _IDFideicomiso.Value = "0";
        }

        private string GetClaveFideicomiso(int idDependenciaFideicomiso)
        {
            Fideicomiso obj = (from d in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.ID == idDependenciaFideicomiso)
                               join f in uow.FideicomisoBusinessLogic.Get()
                               on d.FideicomisoID equals f.ID
                               select f).FirstOrDefault();
            
            return obj != null ? obj.Clave : string.Empty;

        }

        protected void btnConsulta_Click(object sender, EventArgs e)
        {
            _IDFideicomiso.Value = ddlFideicomisos.SelectedValue;
            BindGrid();
            CrearObjetoReporte();
        }

        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            _IDFideicomiso.Value = ddlFideicomisos.SelectedValue;
            BindGrid();
            
            string M = string.Empty;
            divMsgError.Attributes.Add("display", "none");
            
            M=CrearObjetoReporte();

            if (!M.Equals(string.Empty))
            {
                divMsgError.Attributes.Add("display", "block");
                lblMsgError.Text = M;
            }
        }

        protected void gridSintesis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idDependenciaFideicomiso = Utilerias.StrToInt(gridSintesis.DataKeys[e.Row.RowIndex].Values["FideicomisoID"].ToString());
                Label lblFideicomiso = (Label)e.Row.FindControl("lblFideicomiso");
                HtmlButton btnVer = (HtmlButton)e.Row.FindControl("btnVerHistorico"); 

                lblFideicomiso.Text = GetClaveFideicomiso(idDependenciaFideicomiso);

                btnVer.Attributes["onclick"] = "fnc_CrearHistorico(" + gridSintesis.DataKeys[e.Row.RowIndex].Values["ID"] + ")";
            }
        }

        protected void gridSintesis_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSintesis.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}