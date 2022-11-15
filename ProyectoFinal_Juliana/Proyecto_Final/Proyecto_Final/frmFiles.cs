using Conexion_Proyecto_Final;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proyecto_Final
{
    public partial class frmFiles : Form
    {
        //-------------------------------------------Variables-------------------------------------------------------
        List<string> titles = new List<string>();
        string[] info;
        string tableName = "", email;

        //-------------------------------------------Funciones-------------------------------------------------------
        //Esta función carga la información de los archivos CSV en un DataGridView (CRUD-READ)
        public void LoadCSV(string path, char separator, string fieldSeparator)
        {
            dgvTable.DataSource = null;
            dgvTable.Rows.Clear();
            DataTable dataTable = new DataTable();
            //Se guarda toda la información del CSV en un vector.
            string[] ticket = File.ReadAllLines(path, Encoding.Default);           

            if (ticket.Length > 0)
            {
                string[] finalTicket;
                //Se guarda en una variable el valor de la primera línea del CSV correspondiente a los títulos.
                string firstLine = ticket[0];
                //Se separan los títulos.
                string[] ticketTitles = firstLine.Split(separator);
                int number = 0;
                List<string> list = new List<string>();

                foreach (string currentField in ticketTitles)
                {
                    string value = ticketTitles[number];
                    if (fieldSeparator != "")
                    {
                        value = value.TrimEnd(Convert.ToChar(fieldSeparator));
                        value = value.TrimStart(Convert.ToChar(fieldSeparator));
                    }

                    dataTable.Columns.Add(new DataColumn(value));
                    list.Add(value);
                    number++;
                }
                finalTicket = list.ToArray();

                for (int i = 1; i < ticket.Length; i++)
                {
                    //Se obtiene por línea la información separada ya por campos del CSV y se ingresa al dgv.
                    string[] dataLines = ticket[i].Split(separator);
                    DataRow dgS = dataTable.NewRow();
                    int columnIndex = 0;

                    foreach (string currentField in finalTicket)
                    {
                        string value = dataLines[columnIndex++];
                        if (fieldSeparator != "")
                        {
                            value = value.TrimEnd(Convert.ToChar(fieldSeparator));
                            value = value.TrimStart(Convert.ToChar(fieldSeparator));
                        }
                        dgS[currentField] = value;
                    }
                    dataTable.Rows.Add(dgS);
                }
                //Se ingresa la información de los títulos a la lista que será enviada a la función de creación de tabla posteriormente
                //en el botón "Import".
                titles.Clear();
                for (int i = 0; i < ticketTitles.Length; i++)
                {
                    titles.Add(ticketTitles[i]);
                }
            }
            if (dataTable.Rows.Count > 0)
            {
                dgvTable.DataSource = dataTable;
            }
        }

        public frmFiles(string email)
        {
            InitializeComponent();
            this.email = email;
        }

        //-------------------------------------------Botones-------------------------------------------------------
        private void btnOpen_Click(object sender, EventArgs e)
        {
            ofdEscoger.InitialDirectory = Application.StartupPath;
            ofdEscoger.Filter = "Archivos (*.csv)|*.csv";
            if (ofdEscoger.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileInfo path = new FileInfo(ofdEscoger.FileName);
                    tableName = Path.GetFileNameWithoutExtension(path.Name);
                    LoadCSV(ofdEscoger.FileName, Convert.ToChar(","), "");

                    //Se ingresa toda la información del CSV a la variable global info que será enviada a la función "InsertInfo"
                    //para la inserción de información en la base de datos.
                    info = File.ReadAllLines(ofdEscoger.FileName, Encoding.Default);
                }
                catch { }
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
            frmLogin login = new frmLogin();
            login.Show();
        }

        //Aquí se llama la función de la clase transacciones que crea la tabla y la que inserta
        //la información en ella.
        //En el caso en que haya ID repetidos, saltará error pues la llave primary no permite repeticiones. (CRUD-CREATE)
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (Conexion_Proyecto_Final.Transactions.CreateTable(titles, tableName) == 1)
            {
                MessageBox.Show(Conexion_Proyecto_Final.Transactions.InsertInfo(titles, tableName, info, Convert.ToChar(",")));
                MessageBox.Show(Conexion_Proyecto_Final.Transactions.InsertNewTable(tableName, email));
            }
            else
            {
                MessageBox.Show("Error, no se pudo importar la información a la base de datos");
            } 
        }

        //Aquí se toma la información del dgv y se lleva a la base de datos para cargar los datos
        //que se hayan editado.(CRUD-UPDATE)
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string[] row = new string[dgvTable.RowCount - 1];

            for (int i = 0; i < dgvTable.RowCount - 1; i++)
            {
                for (int k = 0; k < dgvTable.ColumnCount; k++)
                {
                    if (k < dgvTable.ColumnCount - 1)
                    {
                        row[i] = row[i] + "'" + Convert.ToString(dgvTable.Rows[i].Cells[k].Value + "',");
                    }
                    else
                    {
                        row[i] = row[i] + "'" + Convert.ToString(dgvTable.Rows[i].Cells[k].Value + "'");
                    }
                }
            }

            if (Conexion_Proyecto_Final.Transactions.DropTable(tableName) == 1)
            {
                if (Conexion_Proyecto_Final.Transactions.CreateTable(titles, tableName) == 1)
                {
                    if(Conexion_Proyecto_Final.Transactions.EditTable(tableName, row) == 1)
                    {
                        MessageBox.Show("Se actualizó la base de datos correctamente");
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                }
                else
                {
                    MessageBox.Show("No se creó la tabla jeje");
                }
            }
            else
            {
                MessageBox.Show("No se eliminó la tabla jeje");
            }
        }

        //Aquí se llama la función de la clase transacciones que elimina la tabla. (CRUD-DELETE)
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Conexion_Proyecto_Final.Transactions.DropTable(tableName) == 1)
            {
                MessageBox.Show("Se eliminó la tabla correctamente");
            }
            else
            {
                MessageBox.Show("No se eliminó la tabla");
            }
        }

        private void frmFiles_Load(object sender, EventArgs e)
        {

        }
    }
}
