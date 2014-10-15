using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SISEC.Formas
{
    public partial class Normatividad : System.Web.UI.Page
    {
        private UnitOfWork uow;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            uow = new UnitOfWork();

            if (!IsPostBack)
            {
                BindDropDownFideicomisos();
                ddlTipoNormatividad.Attributes["onchange"] = "fnc_MostrarFideicomisos(this)";
            }
        }

        private void BindDropDownFideicomisos()
        {
            int idDependencia = Utilerias.StrToInt(Session["Dependencia"].ToString());
            int idEjercicio = Utilerias.StrToInt(Session["Ejercicio"].ToString());


            var list = (from d in uow.DependenciaBusinessLogic.Get(e => e.ID == idDependencia)
                        join df in uow.DependenciaFideicomisoEjercicioBusinessLogic.Get(e => e.EjercicioID == idEjercicio)
                        on d.ID equals df.DependenciaID
                        join f in uow.FideicomisoBusinessLogic.Get()
                        on df.FideicomisoID equals f.ID
                        select new { df.ID, f.Descripcion }).ToList();

            ddlFideicomisos.DataSource = list;
            ddlFideicomisos.DataValueField = "ID";
            ddlFideicomisos.DataTextField = "Descripcion";
            ddlFideicomisos.DataBind();


        }

        private void BindGridNormatividad()
        {
            int tipoNormatividad = Utilerias.StrToInt(ddlTipoNormatividad.SelectedValue);
            int fideicomiso = 0;
            List<DAL.Model.Normatividad> list = null;

            if (tipoNormatividad == 1) //General
                list = uow.NormatividadBusinessLogic.Get(e => e.TipoNormatividad == 1).ToList();
            else //especifica
            {
                fideicomiso = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);
                list = uow.NormatividadBusinessLogic.Get(e => e.TipoNormatividad == 2 && e.DependenciaFideicomisoEjercicioID==fideicomiso).ToList();
            }

            gridNormatividad.DataSource = list;
            gridNormatividad.DataBind();

        }


        private void Consultar()
        {
            int tipoNormatividad = Utilerias.StrToInt(ddlTipoNormatividad.SelectedValue);

            BindGridNormatividad();

            if (tipoNormatividad == 1)
                divFideicomiso.Style.Add("display", "none");
            else
                divFideicomiso.Style.Add("display", "block");

            divGrid.Style.Add("display", "block");

        }

        public List<string> GuardarArchivo(HttpPostedFile postedFile, int idNorma)
        {

            string M = string.Empty;
            string ruta = string.Empty;
            List<string> R = new List<string>();
            try
            {

                ruta = System.Configuration.ConfigurationManager.AppSettings["ArchivosNormatividad"];

                if (!ruta.EndsWith("/"))
                    ruta += "/";

                ruta += idNorma.ToString() + "/";

                if (ruta.StartsWith("~") || ruta.StartsWith("/"))   //Es una ruta relativa al sitio
                    ruta = Server.MapPath(ruta);


                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                ruta += postedFile.FileName;

                postedFile.SaveAs(ruta);

            }
            catch (Exception ex)
            {
                M = ex.Message;
            }

            R.Add(M);

            return R;

        }

        private string EliminarArchivo(int id, string nombreArchivo)
        {
            string M = string.Empty;
            try
            {
                string ruta = string.Empty;

                //eliminar archivo
                ruta = System.Configuration.ConfigurationManager.AppSettings["ArchivosNormatividad"];

                if (!ruta.EndsWith("/"))
                    ruta += "/";

                ruta += id.ToString() + "/";

                if (ruta.StartsWith("~") || ruta.StartsWith("/"))   //Es una ruta relativa al sitio
                    ruta = Server.MapPath(ruta);

                File.Delete(ruta + "\\" + nombreArchivo);
                Directory.Delete(ruta);

            }
            catch (Exception ex)
            {
                M = ex.Message;
            }


            return M;
        }

        protected void gridNormatividad_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTipo = (Label)e.Row.FindControl("lblTipo");
                int tipo = Utilerias.StrToInt(gridNormatividad.DataKeys[e.Row.RowIndex].Values["TipoNormatividad"].ToString());
                HtmlButton btnVer = (HtmlButton)e.Row.FindControl("btnVer");
                int idNorma = Utilerias.StrToInt(gridNormatividad.DataKeys[e.Row.RowIndex].Values["ID"].ToString());
                string ruta = ResolveClientUrl("~/AbrirDocto.aspx");

                btnVer.Attributes["onclick"] = "fnc_AbrirArchivo('"+ruta+"',"+idNorma+")";

                if (tipo == 1)
                    lblTipo.Text = "General";
                else
                    lblTipo.Text = "Específica";

                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarIDNorma(" + idNorma + ")");

            }
        }

        protected void gridNormatividad_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridNormatividad.PageIndex = e.NewPageIndex;
            BindGridNormatividad();
            divCapturaNormatividad.Style.Add("display", "none");
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            Consultar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Consultar();
            divCapturaNormatividad.Style.Add("display", "none");
            divConsultar.Style.Add("display", "block");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int tipoNormatividad = Utilerias.StrToInt(ddlTipoNormatividad.SelectedValue);
            int idNorma = Utilerias.StrToInt(_IDNorma.Value);
            string ruta = string.Empty;
            string M=string.Empty;

            DAL.Model.Normatividad obj = new DAL.Model.Normatividad();


            List<string> R = new List<string>();

            //Se tiene que almacenar el archivo adjunto, si es que se cargo uno
            if (!fileUpload.PostedFile.FileName.Equals(string.Empty))
            {
                if (fileUpload.FileBytes.Length > 10485296)
                {
                    lblMsgError.Text = "Se ha excedido en el tamaño del archivo, el máximo permitido es de 10 Mb";
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");

                    return;
                }


                obj.Descripcion = txtDescripcion.Value;
                obj.TipoNormatividad = tipoNormatividad;

                if (tipoNormatividad == 2)
                    obj.DependenciaFideicomisoEjercicioID = Utilerias.StrToInt(ddlFideicomisos.SelectedValue);

                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                obj.NombreArchivo = Path.GetFileName(fileUpload.FileName);
                obj.TipoArchivo = fileUpload.PostedFile.ContentType;

                uow.NormatividadBusinessLogic.Insert(obj);
                uow.SaveChanges();

                //Si hubo errores al guardar
                if (uow.Errors.Count > 0)
                {
                    foreach (string err in uow.Errors)
                        M += err;

                    lblMsgError.Text = M;

                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");

                    return;
                }


                R = GuardarArchivo(fileUpload.PostedFile, obj.ID);

                //Si hubo errores
                if (!R[0].Equals(string.Empty))
                {
                    lblMsgError.Text = R[0];
                    divMsgError.Style.Add("display", "block");
                    divMsgSuccess.Style.Add("display", "none");

                    return;
                }
                

            }
            else
            {
                lblMsgError.Text = "No se ha adjuntado ningún archivo";
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");

                return;
            }


            M = "Se ha guardado correctamente";

            Consultar();
            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");

        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            int idNorma = Utilerias.StrToInt(_IDNorma.Value);
            string M = string.Empty;
            string nombreArchivo = string.Empty;

            DAL.Model.Normatividad obj = uow.NormatividadBusinessLogic.GetByID(idNorma);
            nombreArchivo = obj.NombreArchivo;

            uow.NormatividadBusinessLogic.Delete(obj);
            uow.SaveChanges();

            if (uow.Errors.Count > 0) //Si hubo errores
            {
                M = string.Empty;
                foreach (string cad in uow.Errors)
                    M += cad;

                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }

            //Se elimina el archivo fisico
            M=EliminarArchivo(idNorma, nombreArchivo);


            //Si hubo Errores
            if (!M.Equals(string.Empty))
            {
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "block");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }


            BindGridNormatividad();

            lblMsgSuccess.Text = "Se ha eliminado correctamente";
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");



        }


        

        
    }
}