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
    public partial class Search_update : Form
    {
        public Search_update()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;

            string ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\Users\\Dell\\Documents\\ABFR.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            con = new SqlConnection(ConnectionString);
            con.Open();

            
            string CommandText = "SELECT * from student WHERE (roll_no LIKE @Find)";
            cmd = new SqlCommand(CommandText);
            cmd.Connection = con;
cmd.Parameters.Add(
                new SqlParameter(
                "@Find", // The name of the parameter to map
                System.Data.SqlDbType.NVarChar, // SqlDbType values
                20, // The width of the parameter
                "roll_no"));  // The name of the source column


cmd.Parameters["@Find"].Value = textBox2.Text;

           
            rdr = cmd.ExecuteReader();

            // Fill the list box with the values retrieved

            if (rdr.Read())
            {
                
                textBox1.Text = rdr["name"].ToString();
                textBox3.Text = rdr["division"].ToString();
                textBox4.Text = rdr["address"].ToString();
                textBox5.Text = rdr["contact_no"].ToString();
                pictureBox1.ImageLocation = rdr["image_add"].ToString();
            }
            else
            {
                MessageBox.Show("No Data Found");
                textBox1.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                pictureBox1.ImageLocation = "";
            }


            

            // ...Code to add the control to the form...

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sure?", "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (textBox1.Text != "" || textBox3.Text != "" || textBox4.Text != "" || textBox5.Text != "" || pictureBox1.ImageLocation != "" || textBox2.Text != "")
                {
                    try
                    {
                        SqlConnection con = null;
                        SqlCommand cmd = null;

                        string ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\Users\\Dell\\Documents\\ABFR.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
                        con = new SqlConnection(ConnectionString);
                        con.Open();


                        string CommandText = "update student set name='" + textBox1.Text + "',division='" + textBox3.Text + "',address='" + textBox4.Text + "',contact_no='" + textBox5.Text + "',image_add= '" + pictureBox1.ImageLocation + "' where roll_no=" + textBox2.Text;
                        cmd = new SqlCommand(CommandText);
                        cmd.Connection = con;



                        cmd.ExecuteReader();
                        MessageBox.Show("Updated");
                    }

                    catch (Exception)
                    {
                        MessageBox.Show("Enter Data");
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Search_update_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sure?", "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (textBox1.Text != "" || textBox3.Text != "" || textBox4.Text != "" || textBox5.Text != "" || pictureBox1.ImageLocation != "" || textBox2.Text != "")
                {
                    try
                    {
                        SqlConnection con = null;
                        SqlCommand cmd = null;

                        string ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\Users\\Dell\\Documents\\ABFR.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
                        con = new SqlConnection(ConnectionString);
                        con.Open();


                        string CommandText = "delete from student where roll_no=" + textBox2.Text;
                        cmd = new SqlCommand(CommandText);
                        cmd.Connection = con;



                        cmd.ExecuteReader();
                        MessageBox.Show("Deleted");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Enter Data");
                    }
                }
                else
                {
                    MessageBox.Show("Fields are empty");
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.Cancel)
            {
                //Set image in picture box
                pictureBox1.ImageLocation = dlg.FileName;
                //Provide file path in txtImagePath text box.
                

            }
        }
    }
}
