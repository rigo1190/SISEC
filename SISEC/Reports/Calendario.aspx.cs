using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private DateTime tempDate; 
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();
            tempDate = Calendar1.TodaysDate;
            
            if (!IsPostBack)
            {
                BindDropDownFideicomisos();
                //BindDropDownTipoSesion();
                //BindDropDownStatusSesion();
            }

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
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

        }
        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {

        }
    }


}