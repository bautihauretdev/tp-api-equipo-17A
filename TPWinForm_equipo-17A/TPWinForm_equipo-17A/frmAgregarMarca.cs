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
    public partial class frmAgregarMarca : Form
    {
        private Marca marca = null;
        public frmAgregarMarca(Marca marca) {
            InitializeComponent();
            this.marca = marca;
        }

        public frmAgregarMarca()
        {
            InitializeComponent();

        }

        private void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            Marca nuevaMarca = new Marca();
            MarcaNegocio negocio = new MarcaNegocio();

            try
            {
                if (marca == null) 
                    marca = new Marca();

                marca.descripcion = txtNombre.Text;

                if (marca.id != 0) 
                {
                    negocio.Modificar(marca);
                    MessageBox.Show("La marca ha sido modificada exitosamente");
                }
                else 
                {
                    negocio.Agregar(marca);
                    MessageBox.Show("La marca ha sido agregada exitosamente");
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmAgregarMarca_Load(object sender, EventArgs e)
        {
            if (marca != null && marca.id != 0)
            {
              
                txtNombre.Text = marca.descripcion;
                this.Text = "Modificar marca"; 
                lblAgregarMarca.Text = "Modificar marca"; 
                btnAgregarMarca.Text = "Modificar"; 
            }
            else
            {
              
                this.Text = "Agregar nueva marca";
                lblAgregarMarca.Text = "Agregar nueva marca";
                btnAgregarMarca.Text = "Agregar";
            }
        }

        private void lblAgregarMarca_Click(object sender, EventArgs e)
        {

        }
    }
}
