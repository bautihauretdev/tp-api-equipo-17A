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
    public partial class frmMenuCategorias : Form
    {
        private List<Categoria> listaCategorias;
         
        public frmMenuCategorias(List<Categoria> categorias)
        {
            InitializeComponent();
            listaCategorias = categorias;
            dgvCategorias.ReadOnly = true;
            dgvCategorias.DataSource = listaCategorias;
        }

        private void frmMenuCategorias_Load(object sender, EventArgs e)
        {
            cargarCategorias();
        }

        private void cargarCategorias()
        {
            try
            {
            CategoriaNegocio negocio = new CategoriaNegocio();
            listaCategorias = negocio.Listar();
            dgvCategorias.DataSource = listaCategorias;
            dgvCategorias.AutoGenerateColumns = true;
            dgvCategorias.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregarCategorias_Click(object sender, EventArgs e)
        {
            frmAltaCategoria alta = new frmAltaCategoria();
            alta.ShowDialog();
            cargarCategorias();
        }

        private void btnModificarCategorias_Click(object sender, EventArgs e)
        {
            Categoria seleccionada;
            seleccionada = (Categoria)dgvCategorias.CurrentRow.DataBoundItem;

            frmAltaCategoria modificar = new frmAltaCategoria(seleccionada);
            modificar.ShowDialog();
            cargarCategorias();
        }

        private void btnEliminarCategoria_Click(object sender, EventArgs e)
        {
            CategoriaNegocio nuevo = new CategoriaNegocio();
            Categoria seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar la categoría seleccionada?", "Eliminando...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Categoria)dgvCategorias.CurrentRow.DataBoundItem;
                    nuevo.Eliminar(seleccionado.Id);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cargarCategorias();
        }
    }
}

