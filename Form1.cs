using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dataset
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable table = new DataTable();
        int selectedRow;
        bool x = false;

        private void LoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Image(*.JPG;*.PNG;*.GIF)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(opf.FileName);
            }
        }

        private void DeleteImage_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.FemaleDemo;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            table.Rows.Add(textBoxLastName.Text, textBoxName.Text, textBoxFather.Text, textBoxPhone.Text, textBoxAdress.Text, textBoxBirthday.Text);
            dataGridView1.DataSource = table;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Father", typeof(string));
            table.Columns.Add("PhoneNumber", typeof(string));
            table.Columns.Add("Adress", typeof(string));
            table.Columns.Add("DateOfBirth", typeof(string));

            table.Rows.Add("Idrisov", "Rinat", "Gilyagetdinovich", "45u4839", "fklajsdflklds", new DateTime(1986, 7, 6).ToString());
            dataGridView1.DataSource = table;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            try
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBoxLastName.Text = row.Cells[0].Value.ToString();
                textBoxName.Text = row.Cells[1].Value.ToString();
                textBoxFather.Text = row.Cells[2].Value.ToString();
                textBoxPhone.Text = row.Cells[3].Value.ToString();
                textBoxAdress.Text = row.Cells[4].Value.ToString();
                textBoxBirthday.Text = row.Cells[5].Value.ToString();
            }
            catch (Exception)
            {
                dataGridView1.Sort(dataGridView1.Columns[e.ColumnIndex], x ? ListSortDirection.Ascending : ListSortDirection.Descending);
                x = !x;
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            DataGridViewRow newDataRow = dataGridView1.Rows[selectedRow];
            newDataRow.Cells[0].Value = textBoxLastName.Text;
            newDataRow.Cells[1].Value = textBoxName.Text;
            newDataRow.Cells[2].Value = textBoxFather.Text;
            newDataRow.Cells[3].Value = textBoxPhone.Text;
            newDataRow.Cells[4].Value = textBoxAdress.Text;
            newDataRow.Cells[5].Value = textBoxBirthday.Text;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                selectedRow = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows.RemoveAt(selectedRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void LoadAll_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Text(*.txt)|*.txt";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(opf.FileName, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        var arr = line.Split('^', StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            for (int i = 0; i < arr.Length; i++)
                            {
                                arr[i] = arr[i].Replace("[etot_simvol]", "^");
                            }
                            table.Rows.Add(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5]);
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }
                    dataGridView1.DataSource = table;
                }
            }
        }

        private async void SaveAll_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfp = new SaveFileDialog();
            Stream myStream;
            if (sfp.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = sfp.OpenFile()) != null)
                {
                    using (StreamWriter sw = new StreamWriter(myStream, Encoding.Default))
                    {
                        try
                        {
                            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                            {
                                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                                {
                                    string data = dataGridView1.Rows[i].Cells[j].Value.ToString().Replace("^", "[etot_simvol]");
                                    await sw.WriteAsync(data + "^");
                                }
                                await sw.WriteLineAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
