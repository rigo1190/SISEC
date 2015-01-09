using BL;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC.Reports
{
    public partial class ReportView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int caller = Utilerias.StrToInt(Request.Params["c"].ToString());
            string parametros=Request.Params["p"] !=null ? Request.Params["p"].ToString() : string.Empty;
            string nomReporte=GetNombreReporte(caller);
            ReportDocument rdc = new ReportDocument();

            rdc.FileName = Server.MapPath("~/Reports/"+nomReporte);

            if (!parametros.Equals(string.Empty))
                CargarParametros(caller, parametros, ref rdc);

            CargarReporte(rdc);
        }

        private void  CargarParametros(int caller,string parametros,ref ReportDocument rdc)
        {
            string[] primerArray = parametros.Split('-');
            
            switch (caller)
            {
                case 1: //ACUERDOS

                    rdc.SetParameterValue("@ejercicio", primerArray[0]);
                    rdc.SetParameterValue("@fideicomiso", primerArray[1]);
                    rdc.SetParameterValue("@status", primerArray[2]);
                    rdc.SetParameterValue("@sesion", primerArray[3]);
                    
                    //RAngo de Fechas
                    DateTime fechaInicio = Convert.ToDateTime(primerArray[4]);
                    DateTime fechaFin = Convert.ToDateTime(primerArray[5]);
                    rdc.SetParameterValue("@fechaInicio", fechaInicio.ToString("yyyy-MM-dd") + " 00:00:00");
                    rdc.SetParameterValue("@fechaFin", fechaFin.ToString("yyyy-MM-dd") + " 00:00:00");

                    break;

                case 2: //SINTESIS INFORMATIVA
                    
                    break;

                case 3: //AGENDA DE SESIONES
                    break;

                case 4: //SEGUIMIENTOS DE ACUERDOS
                    rdc.SetParameterValue("@AcuerdoID", primerArray[0]);

                    break;

                case 5: //HISTORICO SESIONES

                    break;

                case 6: //HISTORICO SINTESIS INFORMATIVA
                    
                    break;
            }
        }




        private void CargarReporte(ReportDocument rdc)
        {
            string M = string.Empty;


                string user = System.Configuration.ConfigurationManager.AppSettings["user"];
                string pass = System.Configuration.ConfigurationManager.AppSettings["pass"];
                string server = System.Configuration.ConfigurationManager.AppSettings["server"];
                string db = System.Configuration.ConfigurationManager.AppSettings["db"];
                

                rdc.SetDatabaseLogon(user, pass, server, db);
                rdc.DataSourceConnections[0].SetConnection(server, db, false);
                rdc.DataSourceConnections[0].SetLogon(user, pass);
                
                
                TableLogOnInfo Logon = new TableLogOnInfo();

                foreach (CrystalDecisions.CrystalReports.Engine.Table t in rdc.Database.Tables)
                {
                    Logon = t.LogOnInfo;
                    Logon.ConnectionInfo.ServerName = server;
                    Logon.ConnectionInfo.DatabaseName = db;
                    Logon.ConnectionInfo.UserID = user;
                    Logon.ConnectionInfo.Password = pass;
                    Logon.ConnectionInfo.Type = ConnectionInfoType.SQL;
                    Logon.ConnectionInfo.IntegratedSecurity = false;
                    t.ApplyLogOnInfo(Logon);

                }

                foreach (ReportDocument subreport in rdc.Subreports)
                {
                    foreach (CrystalDecisions.CrystalReports.Engine.Table t in rdc.Database.Tables)
                    {
                        Logon = t.LogOnInfo;
                        Logon.ConnectionInfo.ServerName = server;
                        Logon.ConnectionInfo.DatabaseName = db;
                        Logon.ConnectionInfo.UserID = user;
                        Logon.ConnectionInfo.Password = pass;
                        Logon.ConnectionInfo.Type = ConnectionInfoType.SQL;
                        Logon.ConnectionInfo.IntegratedSecurity = false;
                        t.ApplyLogOnInfo(Logon);

                    }
                }

                CrystalReportViewer1.ReportSource = rdc;
                CrystalReportViewer1.DataBind();

        }


        private string GetNombreReporte(int caller)
        {
            string nombreReporte = string.Empty;

            switch (caller)
            {
                case 1: //ACUERDOS
                    nombreReporte = "rptAcuerdosSeguimientos.rpt";
                    break;

                case 2: //FICHAS TECNICAS
                    nombreReporte = "rptSintesisInformativa.rpt";
                    break;

                case 3: //AGENDA DE SESIONES
                    nombreReporte = "rptCalendarioSesiones.rpt";
                    break;

                case 4: //SEGUIMIENTOS DE ACUERDO
                    nombreReporte = "rptSeguimientos.rpt";
                    break;

                case 5: //HISTORICO SESIONES
                    nombreReporte = "rptSesionesHistorico.rpt";
                    break;

                case 6: //HISTORICO SINTESIS INFORMATIVA
                    nombreReporte = "rptSintesisInformativaHistorico.rpt";
                    break;
            }

            return nombreReporte;

        }

    }
}