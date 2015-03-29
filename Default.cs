using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;

namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels= new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;

        //System.IO.StreamWriter file = new System.IO.StreamWriter("E:\\Attendance Based On Face Recognition\\Attendance\\attendance.txt");


        public FrmPrincipal()
        {
            InitializeComponent();
            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels+1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }
            
            }
            catch(Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Initialize the capture device
            grabber = new Capture(1);
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
            button1.Enabled = true;
        }


        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(textBox1.Text);

                //Show face added in gray scale
                imageBox1.Image = TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(textBox1.Text + "´s face detected and added", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        void FrameGrabber(object sender, EventArgs e)
        {
            label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    //Convert it to Grayscale
                    gray = currentFrame.Convert<Gray, Byte>();

                    //Face Detector
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                  face,
                  1.2,
                  10,
                  Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                  new Size(20, 20));

                    //Action for each element detected
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        t = t + 1;
                        result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        //draw the face detected in the 0th (gray) channel with blue color
                        currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                        if (trainingImages.ToArray().Length != 0)
                        {
                            //TermCriteria for face recognition with numbers of trained images like maxIteration
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                        //Eigen face recognizer
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                           trainingImages.ToArray(),
                           labels.ToArray(),
                           3000,
                           ref termCrit);

                        name = recognizer.Recognize(result);

                            //Draw the label for each face detected and recognized
                        currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.White));

                        }

                            NamePersons[t-1] = name;
                            NamePersons.Add("");


                        //Set the number of faces detected on the scene
                    
                        label3.Text = facesDetected[0].Length.ToString();
                       
                       
                        /*
                        //Set the region of interest on the faces
                        
                        gray.ROI = f.rect;
                        MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                           eye,
                           1.1,
                           10,
                           Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                           new Size(20, 20));
                        gray.ROI = Rectangle.Empty;

                        foreach (MCvAvgComp ey in eyesDetected[0])
                        {
                            Rectangle eyeRect = ey.rect;
                            eyeRect.Offset(f.rect.X, f.rect.Y);
                            currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                        }
                         */

                    }
                        t = 0;

                        //Names concatenation of persons recognized
                    for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                    {
                        names = names + NamePersons[nnn] + ", ";
                        //MessageBox.Show(NamePersons[nnn]);

                        string test = NamePersons[nnn] + ",";

                        System.IO.File.AppendAllText("E:\\Attendance Based On Face Recognition\\Attendance\\attendance.txt", test);
                        
                       
                        //file.WriteLine(NamePersons[nnn]);
                   
                        //file.Close();

                    }
                    //Show the faces procesed and recognized
                    imageBoxFrameGrabber.Image = currentFrame;
                    label4.Text = names;
                    names = "";
                    //Clear the list(vector) of names
                    NamePersons.Clear();

                }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void searchDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
            Search_update ad = new Search_update();
            ad.Show();
        }

        private void addDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Add_data ad = new Add_data();
            ad.Show();

        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
           

        }

        private void start_Click(object sender, EventArgs e)
        {
            start.Enabled = false;
            stop.Enabled = true;
            //MessageBox.Show(DateTime.Today.ToString("dd-MM-yyyy"));
            if (File.Exists("E:\\Attendance Based On Face Recognition\\Attendance\\attendance.txt"))
            {
                File.Delete("E:\\Attendance Based On Face Recognition\\Attendance\\attendance.txt");
            }

        }

        private void stop_Click(object sender, EventArgs e)
        {
            stop.Enabled = false;
            start.Enabled = true;

            string text = System.IO.File.ReadAllText("E:\\Attendance Based On Face Recognition\\Attendance\\attendance.txt");
            int count = 0;

            string date = DateTime.Today.ToString("dd-MM-yyyy");
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;

            string ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\Users\\Rishiraj\\Documents\\Attendance.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            con = new SqlConnection(ConnectionString);
            con.Open();


            string CommandText = "SELECT * from student";
            cmd = new SqlCommand(CommandText);
            cmd.Connection = con;
            rdr = cmd.ExecuteReader();

            // Fill the list box with the values retrieved

            while (rdr.Read())
            {
                string character = rdr["roll_no"].ToString();

                count = TextTool.CountStringOccurrences(text,character);
                if (count > 20)
                {
                    System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection("Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\Users\\Rishiraj\\Documents\\Attendance.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");

                    System.Data.SqlClient.SqlCommand cmd1 = new System.Data.SqlClient.SqlCommand();
                    cmd1.CommandType = System.Data.CommandType.Text;
                    cmd1.CommandText = "INSERT attendance (date,attendance,roll_no) VALUES ('"+date+"','yes','"+character+"')";
                    cmd1.Connection = sqlConnection1;

                    sqlConnection1.Open();
                    cmd1.ExecuteNonQuery();
                    sqlConnection1.Close();
                    //MessageBox.Show("Saved");
                }

            }

            //MessageBox.Show(""+count);


            /*if (File.Exists("E:\\Attendance Based On Face Recognition\\Attendance\\attendance.txt"))
            {
                File.Delete("E:\\Attendance Based On Face Recognition\\Attendance\\attendance.txt");
            }*/
        }

        private void byDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Searchdate ad = new Searchdate();
            ad.Show();
        }

        private void byStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Searchid ad = new Searchid();
            ad.Show();
        }

        private void imageBoxFrameGrabber_Click(object sender, EventArgs e)
        {

        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

    }

    public static class TextTool
    {
        /// <summary>
        /// Count occurrences of strings.
        /// </summary>
        public static int CountStringOccurrences(string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }
    }
}