using BL;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
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
            string parametros=Request.Params["p"].ToString();
            string nomReporte=GetNombreReporte(caller);
            ReportDocument rdc = new ReportDocument();

            rdc.FileName = Server.MapPath("~/Reports/" + nomReporte);
            
            if (!parametros.Equals(string.Empty))
                CargarParametros(caller, parametros,ref rdc);

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
                    break;
            }
        }




        private void CargarReporte(ReportDocument rdc)
        {
            CrystalReportViewer1.ReportSource = rdc;
            string user = "sa";
            string pass = "081995";
            string server = @"RIGO-PC\SQLEXPRESS";
            string db = "SISEF";


            TableLogOnInfo Logon = new TableLogOnInfo();

            foreach (CrystalDecisions.CrystalReports.Engine.Table t in rdc.Database.Tables)
            {
                Logon = t.LogOnInfo;
                Logon.ConnectionInfo.ServerName = server;
                Logon.ConnectionInfo.DatabaseName = db;
                Logon.ConnectionInfo.UserID = user;
                Logon.ConnectionInfo.Password = pass;
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
                    t.ApplyLogOnInfo(Logon);
                }
            }

            CrystalReportViewer1.DataBind();
        }


        private string GetNombreReporte(int caller)
        {
            string nombreReporte = string.Empty;

            switch (caller)
            {
                case 1:
                    nombreReporte = "rptAcuerdos.rpt";
                    break;
            }


            return nombreReporte;

        }




    }
}