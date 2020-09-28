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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            String login = loginF.Text;
            String password = passwordF.Text;

            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `ussers` WHERE `UserLog` = @l AND `UserPass` = @p", db.GetConnection());
            command.Parameters.Add("@l", MySqlDbType.VarChar).Value = login;
            command.Parameters.Add("@p", MySqlDbType.VarChar).Value = password;
            
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Вы вошли в ваш аккаунт");
                FormI form = new FormI();
                this.Hide();
                form.Show();
            }
            else
                MessageBox.Show("Неверный логин или пароль");
        }

        private void regButt_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 regForm = new Form3();
            regForm.Show();
        }
    }
}
