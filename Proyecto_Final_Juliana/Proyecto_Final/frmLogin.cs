using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Conexion_Proyecto_Final;

namespace Proyecto_Final
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        //-------------------------------------------Botones-------------------------------------------------------
        private void liblRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            frmRegister registro = new frmRegister();
            registro.Show();
        }

        //Se presentan las condiciones que se deben cumplir para poder llamar la función Login de la clase transactions que 
        //comprueba la información de inicio de sesión.
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (Conexion_Proyecto_Final.Transactions.Login(txtEmail.Text, txtPassword.Text) == 1)
            {
                this.Hide();
                frmFiles files = new frmFiles(txtEmail.Text);
                files.Show();
            }
            else
            {
                MessageBox.Show("The Email or password you entered is incorrect. Please, try again");
            }
        }
    }
}
