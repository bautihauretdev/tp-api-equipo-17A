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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnMarcas_Click(object sender, EventArgs e)
        {
            MarcaNegocio negocio = new MarcaNegocio();
            List<Marca> marcas = negocio.Listar();

            ListadoMarcaForm vista = new ListadoMarcaForm(marcas);
            vista.ShowDialog();
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            List<Categoria> categorias = negocio.Listar();

            frmMenuCategorias vista = new frmMenuCategorias(categorias);
            vista.ShowDialog();
        }

        private void btnArticulos_Click(object sender, EventArgs e)
        {
            frmMenuArticulos ventanaMenuArticulos = new frmMenuArticulos();
            ventanaMenuArticulos.ShowDialog();
        }
    }
}
