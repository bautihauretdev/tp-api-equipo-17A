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
    public partial class ListadoMarcaForm : Form
    {
        private List<Marca> listaMarcas;

        public ListadoMarcaForm(List<Marca> marcas)
        {
            InitializeComponent();
            listaMarcas = marcas;
            dgvMarcas.ReadOnly = true;
            dgvMarcas.DataSource = listaMarcas;
        }

        private void ListadoMarcaForm_Load(object sender, EventArgs e)
        {
            cargarMarcas();
        }
        private void cargarMarcas()
        {
            MarcaNegocio negocio = new MarcaNegocio();
            listaMarcas = negocio.Listar();
            dgvMarcas.DataSource = listaMarcas;
            dgvMarcas.AutoGenerateColumns = true;
            dgvMarcas.Refresh();
        }

        private void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            frmAgregarMarca ventana = new frmAgregarMarca();
            ventana.ShowDialog();
            cargarMarcas();
        }

        private void ListadoMarcaForm_Load_1(object sender, EventArgs e)
        {

        }

        private void dgvMarcas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEliminarMarca_Click(object sender, EventArgs e)
        {
            MarcaNegocio nuevo = new MarcaNegocio();
            Marca seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar la categoría seleccionada?", "Eliminando...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Marca)dgvMarcas.CurrentRow.DataBoundItem;
                    nuevo.Eliminar(seleccionado.id);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cargarMarcas();
        }

        private void btnModificarMarca_Click(object sender, EventArgs e)
        {
            if (dgvMarcas.CurrentRow != null)
            {
                Marca seleccionada = (Marca)dgvMarcas.CurrentRow.DataBoundItem;
                frmAgregarMarca ventana = new frmAgregarMarca(seleccionada);
                ventana.ShowDialog();
                cargarMarcas();
            }
        }
    }
}
