using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MultiFaceRec
{
    public partial class Searchdate : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\Users\\Dell\\Documents\\ABFR.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");
        public Searchdate()
        {

            InitializeComponent();
            
        
        }

        private void Searchdate_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'attendanceDataSet.attendance' table. You can move, or remove it, as needed.
            
            this.attendanceTableAdapter.Fill(this.attendanceDataSet.attendance);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet dsBind = new DataSet();
            string query = "select * from attendance where roll_no=" + textBox1.Text;
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

            adapter.Fill(dsBind);
            dataGridView1.DataSource = dsBind.Tables[0];

            dataGridView1.Columns[0].ReadOnly = true;
        }
    }
}
