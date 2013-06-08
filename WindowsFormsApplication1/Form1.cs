using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        StringBuilder text = new StringBuilder();

        public Form1()
        {
            InitializeComponent();
            linkLabel1.Links.Add(0, 0, "www.zipfelmaus.com");
            label_Imagewidth.Text = Convert.ToString(0);
            label_Imageheight.Text = Convert.ToString(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open bitmap";
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Filter = "bmp-file (*.bmp)|*.bmp";//|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
 
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                   try
                   {
                       Bitmap bitmap = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);

                       label_Imagewidth.Text = Convert.ToString(bitmap.Width);
                       label_Imageheight.Text = Convert.ToString(bitmap.Height);

                       text.Remove(0, text.Length);//eventuell vorhandenen Text löschen
                       

                       text.Append("/* # Bitmap to RGB8 converter" + "\r\n" +
                                   "   # created by S. Schuster, 04.2008 - www.zipfelmaus.com" + "\r\n" +
                                   "   # include this file in your AVR project include directory" + "\r\n" +
                                   "*/" + "\r\n\r\n");


                       text.Append("#define image_width (" + Convert.ToString(bitmap.Width) + ")\r\n");
                       text.Append("#define image_height (" + Convert.ToString(bitmap.Height) + ")\r\n"
                                   + "\r\n\r\n");

                       text.Append("unsigned char bitmap[]= " + "\r\n" +
                                   "{" + "\r\n");

                       for (int y = 0; y < bitmap.Height; y++)
                       {
                           for (int x = 0; x < bitmap.Width; x++)
                           {
                               //UInt16 bits_rot, bits_gruen, bits_blau;
                               Color color = bitmap.GetPixel(x, y);

                               uint color8 =
                                    ((uint)color.R / 32) << 5
                                  | ((uint)color.G / 32) << 2
                                  | ((uint)color.B / 64);
                               text.Append("0x" + color8.ToString("X2") + ", ");
                           }
                           text.Append("\r\n");
                       }

                       text.Remove(text.Length-4,4);
                       text.Append("\r\n};"); //das brauchen wir nur für das h.-file nicht für die Ausgabe
                       textBox1.Text = text.ToString();
                   }
                   catch (Exception ex)
                   {
                       MessageBox.Show(ex.Message);
                   }
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           System.Diagnostics.Process.Start(e.Link.LinkData.ToString()); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Set the properties on SaveFileDialog1 so the user is 
            // prompted to create the file if it doesn't exist 
            // or overwrite the file if it does exist.
            saveFileDialog1.CreatePrompt = false;
            saveFileDialog1.OverwritePrompt = true;

            // Set the file name to myText.txt, set the type filter
            // to text files, and set the initial directory to drive C.
            saveFileDialog1.Title = "Save include-file";
            saveFileDialog1.FileName = "bmp";
            saveFileDialog1.DefaultExt = "h";
            saveFileDialog1.Filter = "header files (*.h)|*.h";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter h_file = new StreamWriter(saveFileDialog1.OpenFile()))
                {
                    h_file.Write(text.ToString());
                }
                
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }


    }
}
