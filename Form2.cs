using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practiceFinal
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            db bDatos = new db();
            bDatos.Mostrar(dgv1);
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            db bDatos = new db();
            bDatos.Guardar(txtbox2.Text, txtbox3.Text);
            bDatos.Mostrar(dgv1);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            db bDatos = new db();
            bDatos.SeleccionarFila(dgv1, txtbox1, txtbox2, txtbox3);
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            db bDatos = new db();

            bDatos.Modificar(int.Parse(txtbox1.Text), txtbox2.Text, txtbox3.Text);
            bDatos.Mostrar(dgv1);

        }

        private void btn3_Click(object sender, EventArgs e)
        {
            db bDatos = new db();
            bDatos.Eliminar(int.Parse(txtbox1.Text));
            bDatos.Mostrar(dgv1);
        }
    }
}
