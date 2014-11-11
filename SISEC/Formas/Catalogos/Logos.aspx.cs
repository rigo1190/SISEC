using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC.Formas.Catalogos
{
    public partial class Logos : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            uow = new UnitOfWork();


            if (!IsPostBack)
            {
                BindGrid();
            }

        }

        private void BindGrid()
        {
            grid.DataSource = uow.ImagenesBusinessLogic.Get().ToList();
            grid.DataBind();
        }

        private void BindControles()
        {
            int id = Utilerias.StrToInt(_IDLogo.Value);

            Imagenes obj = uow.ImagenesBusinessLogic.GetByID(id);
            txtArchivoAdjunto.Value = obj.Nombre;
            txtDescripcion.Value = obj.Descripcion != null ? obj.Descripcion : string.Empty;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string M = string.Empty;
            int idLogo = Utilerias.StrToInt(_IDLogo.Value);
            Imagenes obj;


            if (_Accion.Value.Equals("N"))
                obj = new Imagenes();
            else
                obj = uow.ImagenesBusinessLogic.GetByID(idLogo);

            obj.Descripcion = txtDescripcion.Value;
            obj.Nombre = fileUpload.FileName.Equals(string.Empty) ? obj.Nombre : Path.GetFileName(fileUpload.FileName);
            //Se tiene que almacenar el archivo adjunto, si es que se cargo uno
            if (!fileUpload.PostedFile.FileName.Equals(string.Empty))
            {
                if (fileUpload.FileBytes.Length > 10485296)
                {
                    lblMsgError.Text = "Se ha excedido en el tamaño del archivo, el máximo permitido es de 4 Mb";
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");

                    return;
                }

                HttpPostedFile ImgFile = fileUpload.PostedFile;
                // Almacenamos la imagen en una variable para insertarla en la BD
                Byte[] byteImage = new Byte[fileUpload.PostedFile.ContentLength];
                ImgFile.InputStream.Read(byteImage, 0, fileUpload.PostedFile.ContentLength);

                obj.Imagen = byteImage;
            }

            if (_Accion.Value.Equals("N"))
                uow.ImagenesBusinessLogic.Insert(obj);
            else
                uow.ImagenesBusinessLogic.Update(obj);

            uow.SaveChanges();

            //Si hubo errores
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

            M = "Se ha guardado correctamente";
            BindGrid();

            divCaptura.Style.Add("display", "none");
            divEncabezado.Style.Add("display", "block");
            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");


        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDLogo.Value = grid.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }
    }
}