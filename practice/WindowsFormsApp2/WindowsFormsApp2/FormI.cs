using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormI : Form
    {
        public FormI()
        {
            InitializeComponent();
        }


        private void FormI_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void HeaderOfTheTable()
        {
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Номер";
            column1.Width = 100;
            column1.Name = "num";
            column1.Frozen = true;
            column1.CellTemplate = new DataGridViewTextBoxCell();

            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Номер региона";
            column2.Width = 100;
            column2.Name = "NumRegion";
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = new DataGridViewColumn();
            column3.HeaderText = "Рост";
            column3.Width = 100;
            column3.Name = "height";
            column3.CellTemplate = new DataGridViewTextBoxCell();

            var column4 = new DataGridViewColumn();
            column4.HeaderText = "Грудь";
            column4.Width = 100;
            column4.Name = "breast";
            column4.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);
            dataGridView1.Columns.Add(column4);

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        private void AddDataGrid(RowDB row)
        {
            dataGridView1.Rows.Add(row.num, row.NumRegion, row.height, row.breast);
        }

        private void FormI_Shown(object sender, EventArgs e)
        {

            HeaderOfTheTable();

            List<RowDB> _data = new List<RowDB>();

            DB _manager = new DB();
            MySqlCommand _command = new MySqlCommand("SELECT * FROM 'info' ", _manager.GetConnection());
            MySqlDataReader _reader;

            try
            {
                _manager.openConnection();
                _reader = _command.ExecuteReader();

                while (_reader.Read())
                    {
                    RowDB row = new RowDB(_reader["num"], _reader["NumRegion"], _reader["height"], _reader["breast"]);
                    _data.Add(row);
                }

                for (int i = 0; i < _data.Count; i++)
                    AddDataGrid(_data[i]);
            }
            catch
            {
                MessageBox.Show("Ошибка БД!");
            }
            finally
            {
                _manager.closeConnection();
            }
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormR form = new FormR();
            this.Hide();
            form.Show();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Это окно для просмотра данных");
        }
    }
}
