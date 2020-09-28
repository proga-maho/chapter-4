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
    public partial class FormR : Form
    {

        private List<RowDB> _data = new List<RowDB>();
        public FormR()
        {
            InitializeComponent();
        }

        private void FormR_Shown(object sender, EventArgs e)
        {
            HeaderOfTheTable();
            dataGridView1.Columns[0].ReadOnly = true;

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
            dataGridView1.ReadOnly = false;
        }

        private void AddDataGrid(RowDB row)
        {
            dataGridView1.Rows.Add(row.num, row.NumRegion, row.height, row.breast);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            dataGridView1.RowCount = Convert.ToInt32(numericUpDownAdd.Value);
            dataGridView1.ReadOnly = false;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            _data.Clear();

            DB _manager = new DB();
            MySqlCommand _command = new MySqlCommand("SELECT * FROM `info`", _manager.GetConnection());
            MySqlDataReader _reader;

            try
            {
                _manager.openConnection();
                _reader = _command.ExecuteReader();

                while(_reader.Read())
                {
                    RowDB row = new RowDB(_reader["num"], _reader["NumRegion"], _reader["height"], _reader["breast"]);
                    _data.Add(row);
                }

                int i = Convert.ToInt32(numericUpDownChoose.Value) - 1;

                if (i > 0 && i < _data.Count)
                {
                    AddDataGrid(_data[i]);
                }
                else
                    MessageBox.Show("Выбран неправильный элемент");
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                _manager.closeConnection();
            }
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _data.Clear();

            DB _manager = new DB();
            MySqlCommand _command = new MySqlCommand("SELECT * FROM `info`", _manager.GetConnection());
            MySqlDataReader _reader;

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            try
            {
                _manager.openConnection();
                _reader = _command.ExecuteReader();

                while(_reader.Read())
                {
                    RowDB row = new RowDB(_reader["num"], _reader["NumRegion"], _reader["height"], _reader["breast"]);
                    _data.Add(row);
                }

                for(int i = 0; i < _data.Count; i++)
                {
                    AddDataGrid(_data[i]);
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                _manager.closeConnection();
            }
        }

        private void выгрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Добавить эти элементы?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DB _manager = new DB();

                try
                {
                    bool add = true;

                    _manager.closeConnection();
                    
                    for(int i=0; i< dataGridView1.Rows.Count; i++)
                    {
                        if (Convert.ToString(this.dataGridView1.Rows[i].Cells[1].Value) != "" &&
                            Convert.ToString(this.dataGridView1.Rows[i].Cells[2].Value) != "" &&
                            Convert.ToString(this.dataGridView1.Rows[i].Cells[3].Value) != "")
                        {
                            string _commandString = "INSERT INTO `info` (`NumRegion`, `height`, `breast`)" +
                                "VALUES(@nr, @ht, @bt)";
                            MySqlCommand _command = new MySqlCommand(_commandString, _manager.GetConnection());

                            _command.Parameters.Add("@nr", MySqlDbType.VarChar).Value = this.dataGridView1.Rows[i].Cells[1].Value;
                            _command.Parameters.Add("@ht", MySqlDbType.VarChar).Value = this.dataGridView1.Rows[i].Cells[2].Value;
                            _command.Parameters.Add("@bt", MySqlDbType.VarChar).Value = Convert.ToString(this.dataGridView1.Rows[i].Cells[3].Value);

                            if (_command.ExecuteNonQuery() != 1)
                                add = false;
                        }
                        else
                            MessageBox.Show("Не все поля заполнены", "Внимание");
                    }

                    if (add)
                        MessageBox.Show("Данные добавленны", "Внимание");
                    else
                        MessageBox.Show("Ошибка добаления данных", "Внимание");
                }
                catch
                {
                    MessageBox.Show("Ошибка БД", "Ошибка");
                }
                finally
                {
                    _manager.closeConnection();
                }
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0)
            {
                if (Convert.ToString(this.dataGridView1.Rows[0].Cells[1].Value) != "" &&
                    Convert.ToString(this.dataGridView1.Rows[0].Cells[2].Value) != "" &&
                    Convert.ToString(this.dataGridView1.Rows[0].Cells[3].Value) != "")
                {
                    string num = Convert.ToString(this.dataGridView1.Rows[0].Cells[0].Value);
                    string NumRegion = Convert.ToString(this.dataGridView1.Rows[0].Cells[1].Value);
                    string height = Convert.ToString(this.dataGridView1.Rows[0].Cells[2].Value);
                    string breast = Convert.ToString(this.dataGridView1.Rows[0].Cells[3].Value);

                    DB _manager = new DB();
                    string _commandString = "UPDATE `info` SET `num` = '" + num + "', " +
                        "`NumRegion` = '" + NumRegion + "', " +
                        "`height` = '" + height + "', " +
                        "`breast` = '" + breast + "', " +
                        "WHERE `info`. `num` = " + num;
                    MySqlCommand _command = new MySqlCommand(_commandString, _manager.GetConnection());

                    try
                    {
                        _manager.openConnection();
                        _command.ExecuteNonQuery();
                        MessageBox.Show("Данные изменены", "Внимание");
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка БД", "Ошибка");
                    }
                    finally
                    {
                        _manager.closeConnection();
                    }
                }
                else
                    MessageBox.Show("Не все поля заполненны", "Внимание");
            }
            else
            {
                DB _manager = new DB();
                _manager.openConnection();
                bool changed = true;

                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    if (Convert.ToString(this.dataGridView1.Rows[0].Cells[1].Value) != "" &&
                   Convert.ToString(this.dataGridView1.Rows[0].Cells[2].Value) != "" &&
                   Convert.ToString(this.dataGridView1.Rows[0].Cells[3].Value) != "")
                    {
                        string num = Convert.ToString(this.dataGridView1.SelectedRows[i].Cells[0].Value);
                        string NumRegion = Convert.ToString(this.dataGridView1.SelectedRows[i].Cells[1].Value);
                        string height = Convert.ToString(this.dataGridView1.SelectedRows[i].Cells[2].Value);
                        string breast = Convert.ToString(this.dataGridView1.SelectedRows[i].Cells[3].Value);


                        string _commandString = "UPDATE `info` SET `num` = '" + num + "', " +
                            "`NumRegion` = '" + NumRegion + "', " +
                            "`height` = '" + height + "', " +
                            "`breast` = '" + breast + "', " +
                            "WHERE `info`. `num` = " + num;
                        MySqlCommand _command = new MySqlCommand(_commandString, _manager.GetConnection());

                        try
                        {
                            if (_command.ExecuteNonQuery() != 1)
                                changed = false;
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка БД", "Ошибка");
                        }
                    }
                    else
                        MessageBox.Show("Не все поля заполнены", "Внимание");
                }

                if (changed)
                    MessageBox.Show("Данные изменены", "Внимание");

                else
                    MessageBox.Show("Не все данные изменены", "Внимание");

                _manager.closeConnection();
            }

        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Удалить эти данные?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if(dataGridView1.SelectedRows.Count == 0)
                {
                    int index = Convert.ToInt32(numericUpDownChoose.Value);

                    if(index > 0 && index <= _data.Count)
                    {
                        DB _manager = new DB();
                        string num = Convert.ToString(this.dataGridView1.Rows[0].Cells[0].Value);
                        string _commandString = "DELETE FROM `info` WHERE `info`. `num` = " + num;
                        MySqlCommand _command = new MySqlCommand(_commandString, _manager.GetConnection());

                        try
                        {
                            _manager.openConnection();
                            _command.ExecuteNonQuery();
                            MessageBox.Show("Данные удалены", "Внимание");
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка БД", "Ошибка");
                        }
                        finally
                        {
                            _manager.closeConnection();
                        }
                    }
                }
                else
                {
                    DB _manager = new DB();
                    _manager.openConnection();
                    bool delete = true;

                    foreach(DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        string num = Convert.ToString(row.Cells[0].Value);
                        string _commandString = "DELETE FROM `info` WHERE `info`. `num` = " + num;
                        MySqlCommand _command = new MySqlCommand(_commandString, _manager.GetConnection());

                        try
                        {
                            dataGridView1.Rows.Remove(row);
                            if (_command.ExecuteNonQuery() != 1)
                                delete = false;
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка БД", "Ошибка");
                        }
                    }

                    if (delete)
                        MessageBox.Show("Данные удалены", "Внимание");
                    else
                        MessageBox.Show("Не все данные удалены", "Внимание");

                    _manager.closeConnection();
                }
            }
        }

        private void удалитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Удалить все данные?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DB _manager = new DB();
                MySqlCommand _command = new MySqlCommand("TRUNCATE TABLE `info`", _manager.GetConnection());

                try
                {
                    _manager.openConnection();

                    _command.ExecuteNonQuery();
                    MessageBox.Show("Данные удалены", "Внимание");
                }
                catch
                {
                    MessageBox.Show("Ошибка удаления", "Ошибка");
                }
                finally
                {
                    _manager.closeConnection();
                }
            }
        }

        private void просмотрДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Перейти в окно просмотра?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FormI form = new FormI();
                this.Hide();
                form.Show();
            }
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Это окно редактирования данных", "Справка");
        }
    }
}
