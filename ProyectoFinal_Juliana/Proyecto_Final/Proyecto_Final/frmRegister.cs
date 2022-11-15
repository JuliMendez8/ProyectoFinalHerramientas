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
    public partial class frmRegister : Form
    {
        //-------------------------------------------Variables-------------------------------------------------------

        //-------------------------------------------Funciones-------------------------------------------------------
        public void clear()
        {
            txtName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtConfirmation.Clear();

            this.Close();
            frmLogin login = new frmLogin();
            login.Show();
        }
        public frmRegister()
        {
            InitializeComponent();
        }

        //-------------------------------------------Botones-------------------------------------------------------
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            frmLogin login = new frmLogin();
            login.Show();
        }

        //Se presentan las condiciones que se deben cumplir para poder llamar la función Register de la clase transactions que 
        //comprueba la información de registro de usuario y se guarda en la base de datos.
        private void btnRegister_Click(object sender, EventArgs e)
        {
            //Si hay algún campo vacío no se ejecuta.
            if (txtName.Text == String.Empty || txtLastName.Text == String.Empty || txtEmail.Text == String.Empty ||
                txtPassword.Text == String.Empty || txtConfirmation.Text == String.Empty)
            {
                MessageBox.Show("Rellene todos los campos, por favor :)");
            }
            //Si la contraseña y la confirmación no concuerdan no se ejecuta.
            else if (txtPassword.Text == txtConfirmation.Text)
            {
                if (Conexion_Proyecto_Final.Transactions.validationEmail(txtEmail.Text) == 0)
                {
                    MessageBox.Show(Conexion_Proyecto_Final.Transactions.Register(txtName.Text, txtLastName.Text,
                        txtEmail.Text, txtPassword.Text));
                    clear();
                }
                else
                {
                    MessageBox.Show("Este correo ya está registrado");
                }
            }
            else
            {
                MessageBox.Show("Las contraseñas deben coincidir. Intentelo de nuevo.");
            }
        }
    }
}
