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
    public partial class Add_data : Form
    {
        public Add_data()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                System.Data.SqlClient.SqlConnection sqlConnection1 =
        new System.Data.SqlClient.SqlConnection("Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\Users\\Dell\\Documents\\ABFR.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");

                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT into student(roll_no,name,division,address,contact_no,image_add) VALUES (" + textBox2.Text + ", '" + textBox1.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "')";
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();
                cmd.ExecuteNonQuery();
                sqlConnection1.Close();
                MessageBox.Show("Saved");
            
            

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.Cancel)
            {
                //Set image in picture box
                pictureBox1.ImageLocation = dlg.FileName;
                //Provide file path in txtImagePath text box.
                textBox6.Text = dlg.FileName;
                
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

