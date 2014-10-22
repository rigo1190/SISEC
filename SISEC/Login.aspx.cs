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
    public partial class Login : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
               uow = new UnitOfWork();
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            string strlogin = hiddenLogin.Value;
            string strContrasena = hiddenContrasena.Value;

            Usuario user = uow.UsuarioBusinessLogic.Get(u => u.Login == strlogin && u.Password == strContrasena).FirstOrDefault();

            if (user != null)
            {
                //Se resuelve el tipo de usuario

                if (user.TipoUsuarioID == 1)
                {
                    //Nos vamos a la pantalla de administrador
                }
                else
                {
                    Session.Timeout = 60;
                    Session["Login"] = user.Login;
                    Session["UserID"] = user.ID;

                    Response.Redirect("~/SeleccionarEjercicio.aspx");
                }

            }

            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "script", "fnc_ShowMensaje()", true);
                lblMensajes.Text = "Nombre de usuario o contraseña incorrectos";
                lblMensajes.CssClass = "error";
            }
        }



    }
}