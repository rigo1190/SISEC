﻿using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC.Reports
{
    public partial class Calendario : System.Web.UI.Page
    {
        private UnitOfWork uow;
        //private DateTime tempDate; 
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();

            if (!IsPostBack)
            {
                BindDropDownFideicomisos();
                BindDropDownMeses();
                //BindDropDownTipoSesion();
                //BindDropDownStatusSesion();
                string M = string.Empty;
                _URL.Value = ResolveClientUrl("~/Reports/ReportView.aspx");

                M=CrearObjetoReporte();

                if (!M.Equals(string.Empty))
                {
                    divMsgError.Attributes.Add("display", "block");
                    lblMsgError.Text = M;
                }

                
            }

        }


        private void BindDropDownMeses()
        {
            for (int i = 1; i <= 12; i++)
            {
                ListItem item = new ListItem();
                item.Text = Utilerias.GetNombreMes(i);
                item.Value = i.ToString();
                ddlMes.Items.Add(item);
            }

            ddlMes.Items.Insert(0, new ListItem("Seleccione...", "0"));
            ddlMes.SelectedValue = "0";
        }


        private string CrearObjetoReporte()
        {
            bool eliminados = uow.RptSesionesBusinessLogic.DeleteAll();
            string M = string.Empty;

            if (eliminados)
            {
                #region CUANDO SE LIMPIO CORRECTAMENTE LA TABLA TEMPORAL DEL REPORTE, SE PROCEDE A LLENARLA NUEVAMENTE

                int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
                Ejercicio objEjercicio = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

                uow.SaveChanges();

                //SI HUBO ERORRES AL ELIMINAR REGISTROS PREVIOS
                if (uow.Errors.Count > 0)
                {
                    foreach (string m in uow.Errors)
                        M += m;

                    return M;
                }

                int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
                int idCalendario = BuscarCalendario();

                #region TODOS LOS REGISTROS, DE TODOS LOS FIDEICOMISOS Y DE TODO EL AÑO

                var listSesiones = (from c in uow.CalendarioBusinessLogic.Get()
                                    join dfe in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get()
                                    on c.DependenciaFideicomisoEjercicioID equals dfe.ID
                                    join f in uow.FideicomisoBusinessLogic.Get()
                                    on dfe.FideicomisoID equals f.ID
                                    join s in uow.SesionBusinessLogic.Get()
                                    on c.ID equals s.CalendarioID
                                    join ts in uow.TipoSesionBusinessLogic.Get()
                                    on s.TipoSesionID equals ts.ID
                                    join ss in uow.StatusSesionBusinessLogic.Get()
                                    on s.StatusSesionID equals ss.ID
                                    select new
                                    {
                                        SesionID = s.ID,
                                        CalendarioID = c.ID,
                                        FideicomisoID = f.ID,
                                        NombreFideicomiso = f.Descripcion,
                                        NumSesion = s.NumSesion,
                                        NumOficio = s.NumOficio,
                                        FechaOficio = s.FechaOficio,
                                        TipoSesion = ts.Descripcion,
                                        StatusSesion = ss.Descripcion,
                                        Mes = s.Mes,
                                        Descripcion = s.Descripcion,
                                        FechaProgramada = s.FechaProgramada,
                                        FechaCelebrada = s.FechaCelebrada,
                                        FechaReprogamada = s.FechaReprogramada,
                                        Observaciones = s.Observaciones,
                                        LugarReunion = s.LugarReunion,
                                        HoraProgramada = s.HoraProgramada,
                                        HoraReprogramada = s.HoraReprogramada,
                                        HoraCelebrada = s.HoraCelebrada
                                    });

                #endregion

                #region TODOS LOS REGISTROS, DE TODOS LOS FIDEICOMISOS, DE ALGUN MES EN PARTICULAR

                if (!ddlMes.SelectedValue.Equals("0"))
                {
                    int mes = Utilerias.StrToInt(ddlMes.SelectedValue);

                    listSesiones = (from c in uow.CalendarioBusinessLogic.Get()
                                    join dfe in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get()
                                    on c.DependenciaFideicomisoEjercicioID equals dfe.ID
                                    join f in uow.FideicomisoBusinessLogic.Get()
                                    on dfe.FideicomisoID equals f.ID
                                    join s in uow.SesionBusinessLogic.Get(e => e.Mes == mes)
                                    on c.ID equals s.CalendarioID
                                    join ts in uow.TipoSesionBusinessLogic.Get()
                                    on s.TipoSesionID equals ts.ID
                                    join ss in uow.StatusSesionBusinessLogic.Get()
                                    on s.StatusSesionID equals ss.ID
                                    select new
                                    {
                                        SesionID = s.ID,
                                        CalendarioID = c.ID,
                                        FideicomisoID = f.ID,
                                        NombreFideicomiso = f.Descripcion,
                                        NumSesion = s.NumSesion,
                                        NumOficio = s.NumOficio,
                                        FechaOficio = s.FechaOficio,
                                        TipoSesion = ts.Descripcion,
                                        StatusSesion = ss.Descripcion,
                                        Mes = s.Mes,
                                        Descripcion = s.Descripcion,
                                        FechaProgramada = s.FechaProgramada,
                                        FechaCelebrada = s.FechaCelebrada,
                                        FechaReprogamada = s.FechaReprogramada,
                                        Observaciones = s.Observaciones,
                                        LugarReunion = s.LugarReunion,
                                        HoraProgramada = s.HoraProgramada,
                                        HoraReprogramada = s.HoraReprogramada,
                                        HoraCelebrada = s.HoraCelebrada
                                    });
                }

                #endregion

                if (idFideicomiso > 0)//SE FILTRA POR EL FIDEICOMISO SELECCIONADO, SI ES QUE SE ELIGIO ALGUNO 
                {
                    if (!ddlMes.SelectedValue.Equals("0"))
                    {
                        int mes = Utilerias.StrToInt(ddlMes.SelectedValue);

                        #region LOS REGISTROS DE ALGUN FIDEICOMISO EN PARTICULAR, DE ALGUN MES EN PARTICULAR

                        listSesiones = (from c in uow.CalendarioBusinessLogic.Get(e => e.ID == idCalendario)
                                        join dfe in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get()
                                        on c.DependenciaFideicomisoEjercicioID equals dfe.ID
                                        join f in uow.FideicomisoBusinessLogic.Get()
                                        on dfe.FideicomisoID equals f.ID
                                        join s in uow.SesionBusinessLogic.Get(e => e.Mes == mes)
                                        on c.ID equals s.CalendarioID
                                        join ts in uow.TipoSesionBusinessLogic.Get()
                                        on s.TipoSesionID equals ts.ID
                                        join ss in uow.StatusSesionBusinessLogic.Get()
                                        on s.StatusSesionID equals ss.ID
                                        select new
                                        {
                                            SesionID = s.ID,
                                            CalendarioID = c.ID,
                                            FideicomisoID = f.ID,
                                            NombreFideicomiso = f.Descripcion,
                                            NumSesion = s.NumSesion,
                                            NumOficio = s.NumOficio,
                                            FechaOficio = s.FechaOficio,
                                            TipoSesion = ts.Descripcion,
                                            StatusSesion = ss.Descripcion,
                                            Mes = s.Mes,
                                            Descripcion = s.Descripcion,
                                            FechaProgramada = s.FechaProgramada,
                                            FechaCelebrada = s.FechaCelebrada,
                                            FechaReprogamada = s.FechaReprogramada,
                                            Observaciones = s.Observaciones,
                                            LugarReunion = s.LugarReunion,
                                            HoraProgramada = s.HoraProgramada,
                                            HoraReprogramada = s.HoraReprogramada,
                                            HoraCelebrada = s.HoraCelebrada
                                        });

                        #endregion

                    }
                    else
                    {
                        #region LOS REGISTROS DE ALGUN FIDEICOMISO EN PARTICULAR, DE TODO EL AÑO

                        listSesiones = (from c in uow.CalendarioBusinessLogic.Get(e => e.ID == idCalendario)
                                        join dfe in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get()
                                        on c.DependenciaFideicomisoEjercicioID equals dfe.ID
                                        join f in uow.FideicomisoBusinessLogic.Get()
                                        on dfe.FideicomisoID equals f.ID
                                        join s in uow.SesionBusinessLogic.Get()
                                        on c.ID equals s.CalendarioID
                                        join ts in uow.TipoSesionBusinessLogic.Get()
                                        on s.TipoSesionID equals ts.ID
                                        join ss in uow.StatusSesionBusinessLogic.Get()
                                        on s.StatusSesionID equals ss.ID
                                        select new
                                        {
                                            SesionID = s.ID,
                                            CalendarioID = c.ID,
                                            FideicomisoID = f.ID,
                                            NombreFideicomiso = f.Descripcion,
                                            NumSesion = s.NumSesion,
                                            NumOficio = s.NumOficio,
                                            FechaOficio = s.FechaOficio,
                                            TipoSesion = ts.Descripcion,
                                            StatusSesion = ss.Descripcion,
                                            Mes = s.Mes,
                                            Descripcion = s.Descripcion,
                                            FechaProgramada = s.FechaProgramada,
                                            FechaCelebrada = s.FechaCelebrada,
                                            FechaReprogamada = s.FechaReprogramada,
                                            Observaciones = s.Observaciones,
                                            LugarReunion = s.LugarReunion,
                                            HoraProgramada = s.HoraProgramada,
                                            HoraReprogramada = s.HoraReprogramada,
                                            HoraCelebrada = s.HoraCelebrada
                                        });

                        #endregion
                    }
                }

                #region SE LLENA LA TABLA TEMPORAL PARA EL REPORTE

                foreach (var item in listSesiones)
                {

                    List<PropertyInfo> propiedades = item.GetType().GetProperties().ToList();

                    foreach (PropertyInfo propiedad in propiedades)
                    {
                        DAL.Model.rptSesiones rpt = null;

                        switch (propiedad.Name)
                        {
                            case "NumSesion":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Número de Sesión";
                                rpt.ValorCampo = item.NumSesion;
                                break;
                            case "NumOficio":
                                Ejercicio ejercicio = uow.EjercicioBusinessLogic.GetByID(Utilerias.StrToInt(Session["Ejercicio"].ToString()));
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Número de Oficio";
                                rpt.ValorCampo = item.NumOficio;
                                break;
                            case "FechaOficio":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Fecha Oficio";
                                rpt.ValorCampo = item.FechaOficio != null ? item.FechaOficio.Value.ToShortDateString() : string.Empty;
                                break;
                            case "TipoSesion":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Tipo Sesión";
                                rpt.ValorCampo = item.TipoSesion;
                                break;
                            case "StatusSesion":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Status Sesión";
                                rpt.ValorCampo = item.StatusSesion;
                                break;
                            case "Mes":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Mes Programada";
                                rpt.ValorCampo = Utilerias.GetNombreMes(item.Mes);
                                break;
                            case "Descripcion":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Descripción";
                                rpt.ValorCampo = item.Descripcion;
                                break;
                            case "FechaProgramada":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Fecha Programada";
                                rpt.ValorCampo = item.FechaProgramada != null ? item.FechaProgramada.Value.ToShortDateString() : string.Empty;
                                break;
                            case "FechaCelebrada":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Fecha Celebrada";
                                rpt.ValorCampo = item.FechaCelebrada != null ? item.FechaCelebrada.Value.ToShortDateString() : string.Empty;
                                break;
                            case "FechaReprogramada":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Fecha Reprogramada";
                                rpt.ValorCampo = item.FechaReprogamada != null ? item.FechaReprogamada.Value.ToShortDateString() : string.Empty;
                                break;

                            case "Observaciones":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Observaciones";
                                rpt.ValorCampo = item.Observaciones;
                                break;
                            case "LugarReunion":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Lugar de Reunión";
                                rpt.ValorCampo = item.LugarReunion;
                                break;
                            case "HoraProgramada":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Hora Programada";
                                rpt.ValorCampo = item.HoraProgramada;
                                break;

                            case "HoraReprogramada":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Hora Reprogramada";
                                rpt.ValorCampo = item.HoraReprogramada;
                                break;

                            case "HoraCelebrada":
                                rpt = new DAL.Model.rptSesiones();
                                rpt.NombreCampo = "Hora Celebrada";
                                rpt.ValorCampo = item.HoraCelebrada;
                                break;


                        }

                        if (rpt != null)
                        {
                            rpt.CalendarioID = item.CalendarioID;
                            rpt.SesionID = item.SesionID;
                            rpt.FideicomisoID = item.FideicomisoID;
                            rpt.NombreFideicomiso = item.NombreFideicomiso;
                            rpt.Mes = item.Mes;
                            rpt.Ejercicio = objEjercicio.Anio;

                            uow.RptSesionesBusinessLogic.Insert(rpt);
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

                #endregion


                #endregion
            }
            else
                M = "No se pudo crear el reporte, intente nuevamente";

            return M;

        }



        private void BindDropDownTipoSesion()
        {
            //ddlTipoSesion.DataSource = uow.TipoSesionBusinessLogic.Get().ToList();
            //ddlTipoSesion.DataValueField = "ID";
            //ddlTipoSesion.DataTextField = "Descripcion";
            //ddlTipoSesion.DataBind();
        }

        private void BindDropDownStatusSesion()
        {
            //ddlStatus.DataSource = uow.StatusSesionBusinessLogic.Get();
            //ddlStatus.DataValueField = "ID";
            //ddlStatus.DataTextField = "Descripcion";
            //ddlStatus.DataBind();
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
                        select new { df.ID, f.Clave });

            ddlFideicomisos.DataSource = list;
            ddlFideicomisos.DataValueField = "ID";
            ddlFideicomisos.DataTextField = "Clave";
            ddlFideicomisos.DataBind();

            ddlFideicomisos.Items.Insert(0, new ListItem("Seleccione...", "0"));
            ddlFideicomisos.SelectedValue = "0";

        }
        private List<Sesion> GetSesiones()
        {
            List<Sesion> list = null;
            int idCalendario;

            if (ddlFideicomisos.Items.Count > 0)
            {
                idCalendario = BuscarCalendario();
                list = uow.SesionBusinessLogic.Get(e => e.CalendarioID == idCalendario).ToList();
            }

            return list;
        }
        private int BuscarCalendario()
        {
            int idFideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            int idCalendario = 0;

            DAL.Model.Calendario obj = uow.CalendarioBusinessLogic.Get(c => c.DependenciaFideicomisoEjercicioID == idFideicomiso && c.EjercicioID == idEjercicio).FirstOrDefault();

            if (obj != null)
                idCalendario = obj.ID;

            _IDCalendario.Value = idCalendario.ToString();

            return idCalendario;

        }

        private int GetEjercicioAño()
        {
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());

            Ejercicio obj = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

            return obj.Anio;

        }


        [WebMethod]
        public static List<string> GetDatosSesion(string idSesion)
        {
            List<string> R = new List<string>();
            string claveStatus = string.Empty;
            UnitOfWork uow = new UnitOfWork();
            Sesion obj = uow.SesionBusinessLogic.GetByID(Utilerias.StrToInt(idSesion));

            R.Add(GetClaveFideicomiso(obj.CalendarioID,uow)); //CLAVE DEL FIDEICOMISO
            R.Add(obj.NumOficio); //NUMERO DE OFICIO
            R.Add(String.Format("{0:d}",obj.FechaOficio)); //FECHA DE OFICIO
            R.Add(obj.NumSesion); //NUMERO DE SESION
            R.Add(GetTipoSesion(obj.TipoSesionID, uow)); //TIPO DE SESION
            R.Add(obj.LugarReunion); //LUGAR DE REUNION
            R.Add(obj.Descripcion); //ASPECTOS RELEVANTES

            claveStatus = GetClaveStatus(obj.StatusSesionID, uow); //CLAVE DE STATUS DE SESION
            R.Add(claveStatus);

            switch (claveStatus)
            {
                case "P": //PROGRAMADA
                    R.Add(String.Format("{0:d}", obj.FechaProgramada)); //FECHA PROGRAMADA
                    R.Add(obj.HoraProgramada); //HORA PROGRAMADA
                    break;
                case "RP"://REPROGRAMADA
                    R.Add(String.Format("{0:d}", obj.FechaReprogramada)); //FECHA REPROGRAMADA
                    R.Add(obj.HoraReprogramada); //HORA REPROGRAMADA
                    R.Add(obj.Observaciones); //OBSERVACIONES
                    break;
                case "CE": //CELEBRADA
                    R.Add(String.Format("{0:d}", obj.FechaCelebrada)); //FECHA CELEBRADA
                    R.Add(obj.HoraCelebrada); //HORA CELEBRADA
                    break;
                case "C": //CANCELADA
                    
                    break;
            }

            return R;
        }
        public static string GetTipoSesion(int idTipoSesion, UnitOfWork uow)
        {
            TipoSesion obj = uow.TipoSesionBusinessLogic.GetByID(idTipoSesion);
            return obj.Descripcion;
        }
        public static string GetClaveStatus(int idStatus,UnitOfWork uow)
        {
            StatusSesion obj = uow.StatusSesionBusinessLogic.GetByID(idStatus);
            return obj.Clave;
        }
        public static string GetClaveFideicomiso(int idCalendario,UnitOfWork uow)
        {
            Fideicomiso obj = (from c in uow.CalendarioBusinessLogic.Get(e=>e.ID==idCalendario)
                               join d in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get()
                               on c.DependenciaFideicomisoEjercicioID equals d.ID
                               join f in uow.FideicomisoBusinessLogic.Get()
                               on d.FideicomisoID equals f.ID
                               select f).FirstOrDefault();
            return obj.Clave; 
        }
        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            string textColor = string.Empty;
            DateTime dayHold = DateTime.MinValue;
            bool multipleDayItem = false;
            bool dayTextHasChanged = false;
            //DateTime dateVisible = new DateTime(GetEjercicioAño(), Utilerias.StrToInt(ddlMes.SelectedValue), 1);

            StringBuilder temp=null;

            List<Sesion> list = GetSesiones();

            if (list != null)
            {
                foreach (Sesion obj in list)
                {
                    if (dayHold != obj.FechaProgramada)
                    {
                        if (dayTextHasChanged)
                            break;

                        multipleDayItem = false;
                        dayHold = Convert.ToDateTime(obj.FechaProgramada);
                    }
                    else
                        multipleDayItem = true;


                    if (e.Day.Date == obj.FechaProgramada)
                    {
                        if (!multipleDayItem)
                            temp = new StringBuilder();
                        else
                            temp.Append("<br>");
                        
                        temp.Append("<a href='#'");
                        temp.Append("<span onclick='fnc_CargarDatos(" + obj.ID + ");' style=color:green;font-family:Arial;font-weight:bold;font-size:11px;");
                        temp.Append("<br>");
                        temp.Append(" " + obj.NumSesion);
                        temp.Append("</span>");
                        temp.Append("</a>");

                        dayTextHasChanged = true;
                    }
                }

                if (dayTextHasChanged)
                    e.Cell.Controls.Add(new LiteralControl(temp.ToString()));

            }

        }

        protected void ddlFideicomisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string M = string.Empty;
            divMsgError.Attributes.Add("display", "none");

            if (!ddlMes.SelectedValue.Equals("0"))
            {
                DateTime dateVisible;

                if (!ddlMes.SelectedValue.Equals("0"))
                    dateVisible = new DateTime(GetEjercicioAño(), Utilerias.StrToInt(ddlMes.SelectedValue), 1);
                else
                    dateVisible = DateTime.Now;

                Calendar1.VisibleDate = dateVisible;

            }

            M = CrearObjetoReporte();

            if (!M.Equals(string.Empty))
            {
                divMsgError.Attributes.Add("display", "block");
                lblMsgError.Text = M;
            }


            
        }

        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string M = string.Empty;
            DateTime dateVisible;

            if (!ddlMes.SelectedValue.Equals("0"))
                dateVisible = new DateTime(GetEjercicioAño(), Utilerias.StrToInt(ddlMes.SelectedValue), 1);
            else
                dateVisible = DateTime.Now;

            Calendar1.VisibleDate = dateVisible;


            M = CrearObjetoReporte();

            if (!M.Equals(string.Empty))
            {
                divMsgError.Attributes.Add("display", "block");
                lblMsgError.Text = M;
            }


        }

        
        
    }


}