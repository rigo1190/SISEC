﻿using BL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISEC.Formas.Catalogos
{
    public partial class Fideicomisos : System.Web.UI.Page
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
            gridFideicomisos.DataSource = uow.FideicomisoBusinessLogic.Get().ToList();
            gridFideicomisos.DataBind();
        }


        private void BindControles()
        {
            int idFideicomiso = Utilerias.StrToInt(_IDFideicomiso.Value);

            Fideicomiso obj = uow.FideicomisoBusinessLogic.GetByID(idFideicomiso);

            txtClave.Value = obj.Clave;
            txtDescripcion.Value = obj.Descripcion;

        }

        private bool ValidarEliminarFideicomiso(Fideicomiso obj)
        {
            if (obj.DetalleFideicomisos.Count() > 0)
                return false;

            if (obj.DetalleSubFideicomisos.Count() > 0)
                return false;

            return true;
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Fideicomiso obj;
            int idFideicomiso = Utilerias.StrToInt(_IDFideicomiso.Value);
            string M = string.Empty;

            if (_Accion.Value.Equals("N"))
                obj = new Fideicomiso();
            else
                obj = uow.FideicomisoBusinessLogic.GetByID(idFideicomiso);

            obj.Clave = txtClave.Value;
            obj.Descripcion = txtDescripcion.Value;

            if (_Accion.Value.Equals("N"))
            {
                obj.FechaCaptura = DateTime.Now;
                obj.UsuarioCaptura = Session["Login"].ToString();
                uow.FideicomisoBusinessLogic.Insert(obj);
            }
            else
            {
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModifica = Session["Login"].ToString();
                uow.FideicomisoBusinessLogic.Update(obj);
            }

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

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
            _IDFideicomiso.Value = gridFideicomisos.DataKeys[row.RowIndex].Value.ToString();
            _Accion.Value = "A";

            BindControles();

            divCaptura.Style.Add("display", "block");
            divEncabezado.Style.Add("display", "none");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");

        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string M = "Se ha eliminado correctamente";

            int idFideicomiso = Utilerias.StrToInt(_IDFideicomiso.Value);

            Fideicomiso obj = uow.FideicomisoBusinessLogic.GetByID(idFideicomiso);

            if (!ValidarEliminarFideicomiso(obj))
            {
                M = "No se puede eliminar el registro, se encuentra en uso por otros módulos.";
                lblMsgError.Text = M;
                divMsgError.Style.Add("display", "none");
                divMsgSuccess.Style.Add("display", "none");
                return;
            }

            uow.FideicomisoBusinessLogic.Delete(obj);
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


            BindGrid();

            lblMsgSuccess.Text = M;
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "block");


        }


        

        protected void gridFideicomisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridFideicomisos.PageIndex = e.NewPageIndex;
            BindGrid();
            divEncabezado.Style.Add("display", "block");
            divMsgError.Style.Add("display", "none");
            divMsgSuccess.Style.Add("display", "none");
        }

        protected void gridFideicomisos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgBtnEliminar = (ImageButton)e.Row.FindControl("imgBtnEliminar");

                int id = Utilerias.StrToInt(gridFideicomisos.DataKeys[e.Row.RowIndex].Values["ID"].ToString());

                if (imgBtnEliminar != null)
                    imgBtnEliminar.Attributes.Add("onclick", "fnc_ColocarID(" + id + ")");

            }
        }

        


    }
}