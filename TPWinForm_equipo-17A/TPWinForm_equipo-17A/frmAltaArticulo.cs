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
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;

        public frmAltaArticulo()
        {
            InitializeComponent();
        }
        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio(); 
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio(); 
            cbMarca.DataSource = marcaNegocio.Listar();
            cbMarca.DisplayMember = "descripcion";
            cbMarca.ValueMember = "id";

            cbCategoria.DataSource = categoriaNegocio.Listar();
            cbCategoria.DisplayMember = "Descripcion";
            cbCategoria.ValueMember = "Id";

            if (articulo!= null)
            {
                txtCodigo.Text = articulo.Codigo;
                txtNombre.Text = articulo.Nombre;
                txtDescripcion.Text = articulo.Descripcion;
                txtPrecio.Text = articulo.Precio.ToString();
                cbMarca.SelectedValue = articulo.Marca.id;
                cbCategoria.SelectedValue = articulo.Categoria.Id;
                btnAgregar.Text = "Modificar";
                this.Text = "Modificar Artículo";
                if (articulo.Imagenes != null && articulo.Imagenes.Count > 0)
                {
                    txtImagen.Text = articulo.Imagenes.Count > 0 ? articulo.Imagenes[0].ImagenURL : "";
                    txtUrlImagen2.Text = articulo.Imagenes.Count > 1 ? articulo.Imagenes[1].ImagenURL : "";
                    txtUrlImagen3.Text = articulo.Imagenes.Count > 2 ? articulo.Imagenes[2].ImagenURL : "";
                }

            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                if (articulo == null)
                    articulo = new Articulo();

                //Validaciones
                if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                {
                    MessageBox.Show("El campo Código no puede estar vacío.");   
                }

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre no puede estar vacío.");
                    return;
                }

                decimal precio;
                if (!decimal.TryParse(txtPrecio.Text, out precio))
                {
                    MessageBox.Show("El precio debe ser un número válido.");
                    return;
                }

                if (precio <= 0)
                {
                    MessageBox.Show("El precio debe ser mayor a cero.");
                    return;
                }

                //Valida codigo duplicado solo si es nuevo
                if (articulo.Id == 0 && negocio.ObtenerIdPorCodigo(txtCodigo.Text) != -1)
                {
                    MessageBox.Show("Ya existe un artículo con ese código.");
                    return;
                }

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cbMarca.SelectedItem;
                articulo.Categoria = (Categoria)cbCategoria.SelectedItem;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.Imagenes = new List<Imagen>();
                if (!string.IsNullOrWhiteSpace(txtImagen.Text))
                    articulo.Imagenes.Add(new Imagen(0, articulo.Id, txtImagen.Text));
                if (!string.IsNullOrWhiteSpace(txtUrlImagen2.Text))
                    articulo.Imagenes.Add(new Imagen(0, articulo.Id, txtUrlImagen2.Text));
                if (!string.IsNullOrWhiteSpace(txtUrlImagen3.Text))
                    articulo.Imagenes.Add(new Imagen(0, articulo.Id, txtUrlImagen3.Text));

                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);

               
                    if (!string.IsNullOrEmpty(txtImagen.Text))
                    {
                        negocio.ModificarImagen(articulo.Id, txtImagen.Text);
                    }
                    MessageBox.Show("Artículo modificado exitosamente.");
                }
                else
                {
                    negocio.Agregar(articulo);
                    int nuevoId = negocio.ObtenerIdPorCodigo(articulo.Codigo);
                    if (nuevoId != -1 && !string.IsNullOrEmpty(txtImagen.Text))
                    {
                        negocio.AgregarImagen(nuevoId, txtImagen.Text);
                    }
                    MessageBox.Show("Artículo agregado exitosamente.");
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el artículo: " + ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagen.Text);
        }
        private void cargarImagen(string imagen)
        {
            try
            {
               pbxArticulos.LoadAsync(imagen);
            }
            catch (Exception)
            {
                pbxArticulos.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");

            }
        }

        private void btnAgregar_BackColorChanged(object sender, EventArgs e)
        {

        }

        private void pbxArticulos_Click(object sender, EventArgs e)
        {

        }
    }
}
