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
            Normatividad obj = uow.NormatividadBusinessLogic.GetByID(id);
            Response.Redirect("~/ArchivosNormatividad/" + obj.ID + "/" + obj.NombreArchivo);

        }
    }
}