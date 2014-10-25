using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC
{
    public partial class AbrirDocto : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();
            int id = Utilerias.StrToInt(Request.Params["i"].ToString());
            int caller = Utilerias.StrToInt(Request.Params["c"].ToString());
            string nomCarpeta = string.Empty;
            string nomArchivo = string.Empty;

            switch (caller)
            {
                case 1: //NORMATIVIDAD
                    nomCarpeta = "~/ArchivosNormatividad/";
                    Normatividad norma= uow.NormatividadBusinessLogic.GetByID(id);
                    nomArchivo = norma.NombreArchivo != null ? norma.NombreArchivo.Trim() : string.Empty;
                    break;
                case 2: //NOTAS
                    nomCarpeta = "~/ArchivosNotas/";
                    Notas nota = uow.NotasBusinessLogic.GetByID(id);
                    nomArchivo = nota.NombreArchivo != null ? nota.NombreArchivo.Trim() : string.Empty;
                    break;
                case 3: //ACTAS
                    nomCarpeta = "~/ArchivosActas/";
                    Actas acta=uow.ActasBusinessLogic.GetByID(id);
                    nomArchivo = acta.NombreArchivo != null ? acta.NombreArchivo.Trim() : string.Empty;
                    break;
                case 4: //FICHAS TECNICAS
                    nomCarpeta = "~/ArchivosFichas/";
                    FichaTecnica ficha = uow.FichaTecnicaBusinessLogic.GetByID(id);
                    nomArchivo = ficha.NombreArchivo != null ? ficha.NombreArchivo.Trim() : string.Empty;
                    break;
            }


            if (nomArchivo.Equals(string.Empty))
            {
                lblMsgError.Text = "No existe ningun archivo adjunto";
                divMsgError.Style.Add("display", "block");

            }else
                Response.Redirect(nomCarpeta + id + "/" + nomArchivo);
            

        }
    }
}