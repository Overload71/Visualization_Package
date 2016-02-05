using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Visualization;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Drawing.Drawing2D;
namespace VizLab2
{
    public partial class Form1 : Form
    {
        Mesh m;
        double centerX;
        double centerY;
        List<Vertex> FaceVerts = new List<Vertex>();

        // E:\FCIS\4th Year (SC)\1st Semester\Scientific & Statistical Visualization\Labs\Lab 2\Visualization Data (Meshes)\big.dat
        // E:\FCIS\4th Year (SC)\1st Semester\Scientific & Statistical Visualization\Labs\Lab 2\Visualization Data (Meshes)\fr.dat
        public Form1()
        {
           
            InitializeComponent();
            init();
            comboBox1.Items.Add("X-Axis");
            comboBox1.Items.Add("Y-Axis");
            comboBox1.Items.Add("Z-Axis");

        }
        public void init()
        {
            simpleOpenGlControl1.InitializeContexts();
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(60, this.Width / this.Height, 100, 100);
        }
        public void loadMesh_Wired(Mesh m)
        {
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            foreach (Zone z in m.Zones)
            {
                foreach (Face f in z.Faces)
                {

                    Gl.glBegin(Gl.GL_LINES);
                    foreach (uint edgeIndex in f.Edges)
                    {
                        Edge e = z.Edges[edgeIndex];
                        Gl.glColor3d(0, 0, 0);
                        z.Vertices[e.Start].Position.glTell();
                        z.Vertices[e.End].Position.glTell();


                    }
                    Gl.glEnd();
                    Gl.glFlush();
                }
            }
        }
        public void getCenter(Mesh m)
        {
            double minX=9999, minY=9999, maxX=-9999, maxY=-9999;
            foreach (Zone z in m.Zones)
            {
                foreach (Face f in z.Faces)
                {
                    foreach (uint _edge in f.Edges)
                    {
                        Edge __edge = z.Edges[_edge];
                        if (z.Vertices[__edge.Start].Position.x < minX)
                        {
                            minX = z.Vertices[__edge.Start].Position.x;
                        }
                        if (z.Vertices[__edge.Start].Position.x > maxX)
                        {
                            maxX = z.Vertices[__edge.Start].Position.x;
                        }
                        if (z.Vertices[__edge.Start].Position.y < minY)
                        {
                            minY = z.Vertices[__edge.Start].Position.y;
                        }
                        if (z.Vertices[__edge.Start].Position.y > maxY)
                        {
                            maxY = z.Vertices[__edge.Start].Position.y;
                        }
                    }
                }
            }
            centerX = (maxX + minX) / 2;
            centerY = (maxY + minY) / 2;

        }
        public void loadMesh_Filled(Mesh m)
        {
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            foreach (Zone z in m.Zones)
            {
                foreach (Face f in z.Faces)
                {

                    Gl.glBegin(Gl.GL_POLYGON);
                    foreach (uint edgeIndex in f.Edges)
                    {
                        Edge e = z.Edges[edgeIndex];
                        Gl.glColor3d(0, 0, 0);
                        z.Vertices[e.Start].Position.glTell();
                      //  z.Vertices[e.End].Position.glTell();

                    }
                    Gl.glEnd();
                    Gl.glFlush();
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string filePath = textBox1.Text;
            m = new Mesh(@"E:\FCIS\4th Year (SC)\1st Semester\Scientific & Statistical Visualization\Labs\Lab 2\Visualization Data (Meshes)\fr.dat");
            getCenter(m);
            Gl.glTranslated(-centerX, -centerY, 0.0f);
            Gl.glTranslated(centerX, centerY, 0.0f);
            Gl.glScaled(0.05, 0.05, 0);
            Gl.glTranslated(-centerX, -centerY, 0.0f);
            loadMesh_Wired(m);
            simpleOpenGlControl1.Invalidate();
           
            int NumofValues = m.VarToIndex.Count;
            foreach (string name in m.VarToIndex.Keys)
            {
                comboBox2.Items.Add(name);
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void simpleOpenGlControl1_Load(object sender, EventArgs e)
        {
            
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Gl.glTranslated(centerX, centerY, 0);
            float rotAngle = float.Parse(textBox7.Text);
            if (comboBox1.SelectedIndex == 0)
            {
                Gl.glRotatef(rotAngle, 1, 0, 0);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                Gl.glRotatef(rotAngle, 0, 1, 0);
            }
            else
            {
                Gl.glRotatef(rotAngle, 0, 0, 1);
            }
            Gl.glTranslated(-centerX, -centerY, 0);

            if (!checkBox1.Checked)
                loadMesh_Wired(m);
            else
                loadMesh_Filled(m);
            simpleOpenGlControl1.Invalidate();
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            float xTrans, yTrans, zTrans;
            xTrans = float.Parse(textBox2.Text);
            yTrans = float.Parse(textBox3.Text);
            zTrans = float.Parse(textBox4.Text);
            Gl.glTranslatef(xTrans, yTrans, zTrans);
            if(!checkBox1.Checked)
            {
                loadMesh_Wired(m);

            }
            else
            {
                loadMesh_Filled(m);
            }
            simpleOpenGlControl1.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            float xScale, yScale, zScale;
            xScale = float.Parse(textBox10.Text);
            yScale = float.Parse(textBox9.Text);
            zScale = float.Parse(textBox8.Text);
            Gl.glTranslated(centerX, centerY, 0);
            Gl.glScalef(xScale, yScale, zScale);
            Gl.glTranslated(-centerX, -centerY, 0);

            if(!checkBox1.Checked)
            loadMesh_Wired(m);
            else
            {
                loadMesh_Filled(m);
            }
            simpleOpenGlControl1.Invalidate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //u -> end = 1
            double minOut,maxOut;
            m.GetMinMaxValues(Convert.ToUInt32(m.VarToIndex[comboBox2.Text]),out minOut,out maxOut);
            minDataTB.Text = minOut.ToString();
            maxDataTB.Text = maxOut.ToString();
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                loadMesh_Wired(m);
            else
            {
                loadMesh_Filled(m);
            }
            simpleOpenGlControl1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            LinearGradientBrush b1 = new LinearGradientBrush(new Rectangle(0, 0, 250, 50), Color.Red, Color.Yellow, LinearGradientMode.Horizontal);
            ColorBlend cblend = new ColorBlend(4);
            cblend.Colors = new Color[5] { Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Red };
            cblend.Positions = new float[5] { 0f, 0.25f, 0.5f, 0.75f, 1f };
            b1.InterpolationColors = cblend;
          

            g.FillRectangle(b1, 0, 0, 250, 50);
       
        }
        public void mesh_EdgeColoring(Mesh m)
        {
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            double edge_avg,Cr,Cg,Cb;

            foreach (Zone z in m.Zones)
            {
                foreach (Face f in z.Faces)
                {

                    Gl.glBegin(Gl.GL_LINES);
                    foreach (uint edgeIndex in f.Edges)
                    {

                        
                        Edge e = z.Edges[edgeIndex];
                        double v1_data = z.Vertices[e.Start].Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])];
                        double v2_data = z.Vertices[e.End].Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])];
                        edge_avg = (v1_data + v2_data) / 2;
                        Val_col(double.Parse(minDataTB.Text),double.Parse(maxDataTB.Text),edge_avg,out Cr,out Cg,out Cb);
                        Gl.glColor3d(Cr/255, Cg/255, Cb/255);
                        z.Vertices[e.Start].Position.glTell();
                        z.Vertices[e.End].Position.glTell();




                    }
                    Gl.glEnd();
                    Gl.glFlush();
                }
            }
        }
        public void mesh_FaceColoring(Mesh m)
        {
            
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            double face_avg = 0, Cr, Cg, Cb;

            foreach (Zone z in m.Zones)
            {
                foreach (Face f in z.Faces)
                {

                    double v1_data = z.Vertices[f.Vertices[0]].Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])];
                    double v2_data = z.Vertices[f.Vertices[1]].Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])];
                    double v3_data = z.Vertices[f.Vertices[2]].Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])];
                    face_avg = v1_data + v2_data + v3_data;

  
                    face_avg /= 3;
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    foreach (uint edgeIndex in f.Edges)
                    {


                        Edge e = z.Edges[edgeIndex];
                        Val_col(double.Parse(minDataTB.Text), double.Parse(maxDataTB.Text), face_avg, out Cr, out Cg, out Cb);
                        Gl.glColor3d(Cr / 255, Cg / 255, Cb / 255);
                        z.Vertices[e.Start].Position.glTell();
                    }
                    Gl.glEnd();
                    Gl.glFlush();
                }
            }
        }
        public void mesh_ContourLines(Mesh m)
        {
            double Cr,Cg,Cb;
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            foreach (Zone z in m.Zones)
            {
                foreach (Face f in z.Faces)
                {

                    Gl.glBegin(Gl.GL_LINES);
                    foreach (uint edgeIndex in f.Edges)
                    {
                        Edge e = z.Edges[edgeIndex];
                        Gl.glColor3d(0, 0, 0);
                        z.Vertices[e.Start].Position.glTell();
                        z.Vertices[e.End].Position.glTell();
                     
                          
                     
                      

                    }
                    Gl.glEnd();
                    Gl.glFlush();
                }
            }
            int FaceVertsCount = FaceVerts.Count;
            Gl.glBegin(Gl.GL_LINES);
            for (int i = 0; i < FaceVerts.Count; i++)
            {
                Val_col(double.Parse(minDataTB.Text), double.Parse(maxDataTB.Text), FaceVerts[i].Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])], out Cr, out Cg, out Cb);
                Gl.glColor3d(Cr / 255, Cg / 255, Cb / 255);
                FaceVerts[i].Position.glTell();
            }


                Gl.glEnd();
            Gl.glFlush();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                mesh_EdgeColoring(m);
               simpleOpenGlControl1.Invalidate();
            }
            else
            {
                if (!checkBox1.Checked)
                    loadMesh_Wired(m);
                else
                {
                    loadMesh_Filled(m);
                }
                simpleOpenGlControl1.Invalidate();
            }
           
        }
        public void Val_col(double min, double max, double val , out double Cr, out double Cg, out double Cb)
        {

            double startRange = min;
            double endRange = max;
            double value = val;
            //double i = Math.Floor(5 * ((value - startRange) / (endRange - startRange)));
            //double t = ((value - startRange) / (endRange - startRange));

            //temp
            double deltaS = (endRange - startRange) / 4;
            double dS = (value - startRange) / deltaS;
            double newI = Math.Floor(dS);
            double newT = dS - newI;
          //  double Cr, Cg, Cb;
            Cr = 0;
            Cb = 0;
            Cg = 0;

                if (newI == 4) //red-Red
                {
                    Graphics r = panel1.CreateGraphics();
                    Cr = Color.Red.R + newT * (Color.Red.R - Color.Red.R);
                    Cg = Color.Red.G + newT * (Color.Red.G - Color.Red.G);
                    Cb = Color.Red.B + newT * (Color.Red.B - Color.Red.B);
                }
                else if (newI == 3) //Orange-Red 
                {
                    Graphics r = panel1.CreateGraphics();
                    Cr = Color.Orange.R + newT * (Color.Red.R - Color.Orange.R);
                    Cg = Color.Orange.G + newT * (Color.Red.G - Color.Orange.G);
                    Cb = Color.Orange.B + newT * (Color.Red.B - Color.Orange.B);
                }

                else if (newI == 2) //yellow-Orange
                {
                    Graphics r = panel1.CreateGraphics();
                    Cr = Color.Yellow.R + newT * (Color.Orange.R - Color.Yellow.R);
                    Cg = Color.Yellow.G + newT * (Color.Orange.G - Color.Yellow.G);
                    Cb = Color.Yellow.B + newT * (Color.Orange.B - Color.Yellow.B);
                }

                else if (newI == 1) //green-Yellow
                {
                    Graphics r = panel1.CreateGraphics();
                    Cr = 0.0 + newT * (Color.Yellow.R - 0.0);
                    Cg = 255.0 + newT * (Color.Yellow.G - 255.0);
                    Cb = 0.0 + newT * (Color.Yellow.B - 0.0);
                }

                else if (newI == 0) //blue-Green
                {
                    Graphics r = panel1.CreateGraphics();
                    Cr = Color.Blue.R + newT * (0.0 - Color.Blue.R);
                    Cg = Color.Blue.G + newT * (255.0 - Color.Blue.G);
                    Cb = Color.Blue.B + newT * (0.0 - Color.Blue.B);
                }
            }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                mesh_FaceColoring(m);
                simpleOpenGlControl1.Invalidate();
            }
            else
            {
                if (!checkBox1.Checked)
                    loadMesh_Wired(m);
                else
                {
                    loadMesh_Filled(m);
                }
                simpleOpenGlControl1.Invalidate();
            }
        }

        private void contourB_Click(object sender, EventArgs e)
        {
            int contourCount = int.Parse(contourTB.Text);
            contourCount++;
            float s_Val = (float.Parse(maxDataTB.Text) - float.Parse(minDataTB.Text)) / contourCount;
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            for (float i = float.Parse(minDataTB.Text); i <= float.Parse(maxDataTB.Text); i += s_Val)
            {
                foreach (Zone z in m.Zones)
                {
                    foreach (Face f in z.Faces)
                    {
                        foreach (uint edg_index in f.Edges)
                        {
                            Edge _edge = z.Edges[edg_index];
                            double vStartData = z.Vertices[_edge.Start].Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])];
                            double vEndData = z.Vertices[_edge.End].Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])];

                            if (vStartData > vEndData)
                            {
                                if (i >= vEndData && i<= vStartData)
                                {
                                    Vertex temp = new Vertex();
                                    temp.Data = new double[2];
                                    temp.Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])] = i;
                                    double alpha = (i - vEndData) / (vStartData - vEndData);
                                    temp.Position.x = z.Vertices[_edge.End].Position.x + (alpha * (z.Vertices[_edge.Start].Position.x - z.Vertices[_edge.End].Position.x));
                                    temp.Position.y = z.Vertices[_edge.End].Position.y + (alpha * (z.Vertices[_edge.Start].Position.y - z.Vertices[_edge.End].Position.y));
                                    temp.Position.z = z.Vertices[_edge.End].Position.z + (alpha * (z.Vertices[_edge.Start].Position.z - z.Vertices[_edge.End].Position.z));
                                    FaceVerts.Add(temp);
                                }
                               
                            }
                            else
                            {
                                if (i >= vStartData && i<= vEndData)
                                {
                                    Vertex temp = new Vertex();
                                    temp.Data = new double[2];

                                    temp.Data[Convert.ToUInt32(m.VarToIndex[comboBox2.Text])] = i;
                                    double alpha = (i - vStartData) / (vEndData - vStartData);
                                    temp.Position.x = z.Vertices[_edge.Start].Position.x + (alpha * (z.Vertices[_edge.End].Position.x - z.Vertices[_edge.Start].Position.x));
                                    temp.Position.y = z.Vertices[_edge.Start].Position.y + (alpha * (z.Vertices[_edge.End].Position.y - z.Vertices[_edge.Start].Position.y));
                                    temp.Position.z = z.Vertices[_edge.Start].Position.z + (alpha * (z.Vertices[_edge.End].Position.z - z.Vertices[_edge.Start].Position.z));
                                    FaceVerts.Add(temp);
                                }
                              

                            }
                        }
                    }
                }
            }
            mesh_ContourLines(m);
            simpleOpenGlControl1.Invalidate();
        }

    
     } 
}
