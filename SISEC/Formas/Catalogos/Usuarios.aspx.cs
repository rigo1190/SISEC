using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC.Formas.Catalogos
{
    public partial class Usuarios : System.Web.UI.Page
    {
        private UnitOfWork uow;
        public string clave = "3ncR1pt4d4"; // Clave de cifrado.
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();
            if (!IsPostBack)
            {
                BindGrid();
                BindDropDownTipoUsuario();
                //EncriptarPasswordExistentes();
            }
        }


        private void EncriptarPasswordExistentes()
        {
            List<Usuario> users = uow.UsuarioBusinessLogic.Get().ToList();

            foreach (Usuario u in users)
            {
                u.Password = Encriptar(u.Password);

                uow.UsuarioBusinessLogic.Update(u);
                uow.SaveChanges();

                if (uow.Errors.Count > 0)
                {
                    string M=string.Empty;
                    foreach (string e in uow.Errors)
                        M += e;
                }
            }
        }
        private void BindGrid()
        {
            gridUsuarios.DataSource = uow.UsuarioBusinessLogic.Get().ToList();
            gridUsuarios.DataBind();
        }

        private void BindDropDownTipoUsuario()
        {
            //int idUsuario = Utilerias.StrToInt(Session["UserID"].ToString());
            //Usuario user = uow.UsuarioBusinessLogic.GetByID(idUsuario);
            //TipoUsuario tipo = uow.TipoUsuarioBusinessLogic.GetByID(user.TipoUsuarioID);

            //if (!Convert.ToBoolean(tipo.EsDesarrollador))
            //    ddlTipoUsuario.DataSource = uow.TipoUsuarioBusinessLogic.Get(e=>e.EsDesarrollador==false).ToList();

            ddlTipoUsuario.DataSource = uow.TipoUsuarioBusinessLogic.Get().ToList();
            ddlTipoUsuario.DataValueField = "ID";
            ddlTipoUsuario.DataTextField = "Descripcion";
            ddlTipoUsuario.DataBind();
        }

        private void BindControles()
        {
            int idUsuario = Utilerias.StrToInt(_IDUsuario.Value);

            Usuario obj = uow.UsuarioBusinessLogic.GetByID(idUsuario);

            txtNombre.Value = obj.Nombre;
            txtPassword.Value = Desencriptar(obj.Password);
            txtPassword.Attributes.Add("type", "password");
            txtLogin.Value = obj.Login;
            chkActivo.Checked = Convert.ToBoolean(obj.Activo);
            chkBloqueado.Checked = Convert.ToBoolean(obj.Bloqueado);
            ddlTipoUsuario.SelectedValue = obj.TipoUsuarioID.ToString();

        }

        private string GetDescripcionTipoUsuario(int id)
        {
            TipoUsuario tipo = (from u in uow.UsuarioBusinessLogic.Get(e => e.ID == id)
                             join t in uow.TipoUsuarioBusinessLogic.Get()
                             on u.TipoUsuarioID equals t.ID
                             select t).FirstOrDefault();

            return tipo.Descripcion;
        }

        private bool ValidarEliminarUsuario(Usuario obj)
        {
            List<Calendario> list= uow.CalendarioBusinessLogic.Get(e=>e.UsuarioCaptura==obj.Login || e.UsuarioModifica==obj.Login).ToList();

            if (list.Count > 0)
                return false;

            return true;
        }


        private bool ValidarInsercion(string login, Usuario objUsuario = null)
        {
            Usuario obj = null;

            if (login.Trim().Equals(string.Empty))
                return false;

            if (objUsuario == null)
                obj = uow.UsuarioBusinessLogic.Get(e => e.Login.ToUpper().Trim() == login.ToUpper().Trim()).FirstOrDefault();
            else
                if (!login.ToUpper().Trim().Equals(objUsuario.Login.ToUpper().Trim()))
                    obj = uow.UsuarioBusinessLogic.Get(e => e.Login == login).FirstOrDefault();

            return obj == null;
        }

        private string Encriptar(string pass)
        {
            //cifrar datos
            byte[] llave; //Arreglo donde guardaremos la llave para el cifrado 3DES.

            byte[] arreglo = UTF8Encoding.UTF8.GetBytes(pass); //Arreglo donde guardaremos la cadena descifrada.

            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateEncryptor(); // Iniciamos la conversión de la cadena
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length); //Arreglo de bytes donde guardaremos la cadena cifrada.
            tripledes.Clear();


            return Convert.ToBase64String(resultado, 0, resultado.Length);
        }

        private string Desencriptar(string cadena)
        {
            byte[] llave;

            byte[] arreglo = Convert.FromBase64String(cadena); // Arreglo donde guardaremos la cadena descovertida.

            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();

            string cadena_descifrada = UTF8Encoding.UTF8.GetString(resultado); // Obtenemos la cadena
            return cadena_descifrada; // Devolvemos la cadena
        }

        protected void gridUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");
                Label lblTipo = (Label)e.Row.FindControl("lblTipo");

                int id = Utilerias.StrToInt(gridUsuarios.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                lblTipo.Text = GetDescripcionTipoUsuario(id);

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarID(" + id + ")");

            }
        }

        protected void gridUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridUsuarios.PageIndex = e.NewPageIndex;
            BindGrid();
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDUsuario.Value = gridUsuarios.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }


        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Usuario obj;
            int idUsuario = Utilerias.StrToInt(_IDUsuario.Value);
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new Usuario();
            else
                obj = uow.UsuarioBusinessLogic.GetByID(idUsuario);

            if (!ValidarInsercion(txtLogin.Value, !_Accion.Value.Equals("N") ? obj : null))
            {
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "none");
                divCaptura.Style.Add("display", "block");

                lblMsgError.Text = "Ya existe registro para el LOGIN escrito. Intente con otros valores.";

                return;
            }


            obj.Nombre = txtNombre.Value;
            obj.Login = txtLogin.Value;
            obj.Password = Encriptar(txtPassword.Value);
            obj.TipoUsuarioID = Utilerias.StrToInt(ddlTipoUsuario.SelectedValue);

            if (_Accion.Value.Equals("N"))
                uow.UsuarioBusinessLogic.Insert(obj);
            else
                uow.UsuarioBusinessLogic.Update(obj);
            

            uow.SaveChanges();

            if (uow.Errors.Count > 0)
            {
                foreach (string err in uow.Errors)
                    M += err;

                //MANEJAR EL ERROR
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
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

            int idTipo = Utilerias.StrToInt(_IDUsuario.Value);

            Usuario obj = uow.UsuarioBusinessLogic.GetByID(idTipo);

            if (!ValidarEliminarUsuario(obj))
            {
                M = "No se puede eliminar el registro, se encuentra en uso por otros módulos.";
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                divCaptura.Style.Add("display", "none");
                return;
            }

            uow.UsuarioBusinessLogic.Delete(obj);
            uow.SaveChanges();

            if (uow.Errors.Count > 0) //Si hubo errores
            {
                M = string.Empty;
                foreach (string cad in uow.Errors)
                    M += cad;

                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                divEncabezado.Style.Add("display", "block");
                divCaptura.Style.Add("display", "none");
                return;
            }


            BindGrid();

            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "block");
            divCaptura.Style.Add("display", "none");
        }
    }
}