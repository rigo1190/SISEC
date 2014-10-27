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
    public partial class Navegador : System.Web.UI.MasterPage
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow=new UnitOfWork();
            string login = Session["Login"].ToString();
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());
            Ejercicio ejercicio = uow.EjercicioBusinessLogic.GetByID(idEjercicio);

            Usuario user = uow.UsuarioBusinessLogic.Get(u => u.Login == login).FirstOrDefault();
            lblUsuario.Text = user.Nombre;
            lblEjercicio.Text = "Ejercicio " + ejercicio.Anio;
            

        }
    }
}