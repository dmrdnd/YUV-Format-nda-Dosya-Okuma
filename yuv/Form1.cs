using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace yuv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
 
        int width;
        int height;
        double value=3;
        List<Bitmap> image = new List<Bitmap>();
        int i = 0;
        string road = "";
        Processes process;

        private void button1_Click(object sender, EventArgs e)
        {
            process = new Processes();
            image.Clear();
            width =Convert.ToInt32(textBox1.Text);
            height =Convert.ToInt32(textBox2.Text);
        
            image=  process.ReadYUVFile(value,road,width,height);
       
            panel1.Visible = false;
            button2.Visible = true;
            button5.Visible = true;
        }

     
        
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
            i = 0;                                      
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (i < image.Count)
            {
                pictureBox1.Image = image[i];
                i++;
            }           
        }

        private void Form1_Load(object sender, EventArgs e)
        {          
            timer1.Interval = 200;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                road = openFileDialog1.FileName;
            }
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // comboBox1.Text = comboBox1.Items[0].ToString();
            if (comboBox1.SelectedIndex == 1)
            {
                comboBox1.Text = comboBox1.Items[1].ToString();
                value = 2;
            }
            else if(comboBox1.SelectedIndex==2)
            {
                comboBox1.Text = comboBox1.Items[2].ToString();
                value = 1.5;
            }
            else
            {
                comboBox1.Text = comboBox1.Items[0].ToString();
                value = 3;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            button5.Visible = false;
            button2.Visible = false;
            panel1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for(int i=0; i<image.Count; i++)
            {
                image[i].Save("D:\\Yazlab\\yazlab3\\bitmap\\" + i + "-frame.bmp");  // you save code the bmp image not forget 
            }
        }
    }
}
