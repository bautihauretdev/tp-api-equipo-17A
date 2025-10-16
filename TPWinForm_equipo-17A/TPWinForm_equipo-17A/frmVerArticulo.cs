using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace TPWinForm_equipo_17A
{
    public partial class frmVerArticulo : Form
    {
        private Articulo articulo = null;
        private int imagenActual = 0;

        public frmVerArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void frmVerArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            cbMarca.DataSource = marcaNegocio.Listar();
            cbMarca.DisplayMember = "descripcion";
            cbMarca.ValueMember = "id";

            cbCategoria.DataSource = categoriaNegocio.Listar();
            cbCategoria.DisplayMember = "Descripcion";
            cbCategoria.ValueMember = "Id";

            txtCodigo.Text = articulo.Codigo;
            txtNombre.Text = articulo.Nombre;
            txtDescripcion.Text = articulo.Descripcion;
            txtPrecio.Text = articulo.Precio.ToString();
            cbMarca.SelectedValue = articulo.Marca.id;
            cbCategoria.SelectedValue = articulo.Categoria.Id;
            txtImagen.Text = articulo.ImagenUrl;
            cargarImagen(articulo.ImagenUrl);

            if (articulo.Imagenes != null && articulo.Imagenes.Count > 0)
            {
                txtImagen.Text = articulo.Imagenes.Count > 0 ? articulo.Imagenes[0].ImagenURL : "";
                txtImagen2.Text = articulo.Imagenes.Count > 1 ? articulo.Imagenes[1].ImagenURL : "";
                txtImagen3.Text = articulo.Imagenes.Count > 2 ? articulo.Imagenes[2].ImagenURL : "";
            }
            else
            {
                txtImagen.Text = "";
                txtImagen2.Text = "";
                txtImagen3.Text = "";
            }

            // Controles para que sea sólo lectura:
            txtCodigo.ReadOnly = true;
            txtNombre.ReadOnly = true;
            txtDescripcion.ReadOnly = true;
            txtPrecio.ReadOnly = true;
            txtImagen.ReadOnly = true;
            cbMarca.Enabled = false;
            cbCategoria.Enabled = false;
            txtImagen2.ReadOnly = true;
            txtImagen3.ReadOnly = true;

            //muestra la primera imagen si hay

            if (articulo.Imagenes != null && articulo.Imagenes.Count > 0)
            {
                cargarImagen(articulo.Imagenes[imagenActual].ImagenURL);
            }
            else
            {
                cargarImagen("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }

            //habilita/deshabilita botones segun cantidad de imagenes
            btnAnteriorImg.Enabled = articulo.Imagenes.Count > 1;
            btnSiguienteImg.Enabled = articulo.Imagenes.Count > 1;

        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulos.Load(imagen);
            }
            catch (Exception)
            {
                pbxArticulos.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");

            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAnteriorImg_Click(object sender, EventArgs e)
        {
            if (articulo.Imagenes == null || articulo.Imagenes.Count == 0) return;
            imagenActual = (imagenActual - 1 + articulo.Imagenes.Count) % articulo.Imagenes.Count;
            cargarImagen(articulo.Imagenes[imagenActual].ImagenURL);
        }

        private void btnSiguienteImg_Click(object sender, EventArgs e)
        {
            if (articulo.Imagenes == null || articulo.Imagenes.Count == 0) return;
            imagenActual = (imagenActual + 1) % articulo.Imagenes.Count;
            cargarImagen(articulo.Imagenes[imagenActual].ImagenURL);
        }
    }
}
