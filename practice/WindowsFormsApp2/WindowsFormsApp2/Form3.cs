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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (UserName.Text == "")
            {
                MessageBox.Show("Введите имя!");
                return;
            }
            
            if (UserSureName.Text == "")
            {
                MessageBox.Show("Введите фамилию!");
                return;
            }

            if (loginF.Text == "")
            {
                MessageBox.Show("Введите логин!");
                return;
            }

            if (passwordF.Text == "")
            {
                MessageBox.Show("Введите пароль!");
                return;
            }

            if (check())
                return;

            DB db = new DB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `ussers` (`UserLog`, `UserPass`, `name`, `surname`) VALUES (@log, @pass, @name, @surname)", db.GetConnection());

            command.Parameters.Add("@log", MySqlDbType.VarChar).Value = loginF.Text;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = passwordF.Text;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = UserName.Text;
            command.Parameters.Add("@surname", MySqlDbType.VarChar).Value = UserSureName.Text;

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
                MessageBox.Show("Аккаунт был создан");
            else
                MessageBox.Show("Аккаунт не был создан");
            
            db.closeConnection();
        }

        public Boolean check()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `ussers` WHERE `UserLog` = @l", db.GetConnection());
            command.Parameters.Add("@l", MySqlDbType.VarChar).Value = loginF.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return true;
            }
            else
                return false;
        }
    }
}
