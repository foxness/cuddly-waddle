using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace TSPApp
{
    public partial class MainWindow : Form
    {
        private const int vertexSize = 5;

        private TSP tsp;
        private List<float[]> vertices;
        private List<int> bestState;

        private Brush vertexBrush;
        private Pen pathPen;
        private Color bgColor;

        public MainWindow()
        {
            InitializeComponent();

            vertices = GetVertices(GetRawVertices());
            tsp = new TSP(vertices, 0, 1000, 10, 0.3);
            tsp.onReportGeneration += OnReportGeneration;

            vertexBrush = new SolidBrush(Color.White);
            pathPen = new Pen(Color.White);
            bgColor = canvasBox.BackColor;
        }

        private void OnReportGeneration(List<int> bestState)
        {
            this.bestState = bestState;
            canvasBox.Refresh();
        }

        private string GetRawVertices()
        {
            return @"A 215 40
B 187 70
C 130 80
D 230 90
E 150 120
F 540 50
G 500 100
H 375 120
I 470 130
J 507 212
K 470 290
L 380 380
M 300 390
N 270 330
O 250 380
P 210 400
R 240 430
S 280 420
T 210 440
U 255 466
1 580 50
2 620 80
3 647 112
4 669 144
5 683 176
6 687 220
7 691 260";
        }

        private List<float[]> GetVertices(string raw)
        {
            return raw
                .Replace("\r", "")
                .Split('\n')
                .Select(x => x.Split(' '))
                .Select(x => new[] { Single.Parse(x[1]), Single.Parse(x[2]) })
                .ToList();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            // nothing
        }

        private void DrawState(Graphics gfx, List<int> state)
        {
            gfx.DrawLines(pathPen, state.Select(x => new PointF(vertices[x][0], vertices[x][1])).ToArray());
        }

        private void DrawVertices(Graphics gfx)
        {
            foreach (var vertex in vertices)
            {
                gfx.FillEllipse(vertexBrush, vertex[0], vertex[1], vertexSize, vertexSize);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            //new Thread(() =>
            //{
            //    Thread.CurrentThread.IsBackground = true;
            //    tsp.Run(40);
            //}).Start();

            tsp.Run(40);
        }

        private void canvasBox_Paint(object sender, PaintEventArgs e)
        {
            var gfx = e.Graphics;

            gfx.Clear(bgColor);

            if (bestState != null)
            {
                DrawState(gfx, bestState);
            }

            DrawVertices(gfx);
        }
    }
}
