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
        private const int vertexSize = 3;

        private TSP tsp;
        private List<float[]> vertices;
        private List<int> bestState;

        private Brush vertexBrush;
        private Pen pathPen;
        private Color bgColor;

        private Thread workerThread;
        private bool working;

        private bool editMode;

        private delegate void UpdateStatusDelegate(int generation, List<int> bestState, double bestFitness);
        private UpdateStatusDelegate updateStatusDelegate;

        public MainWindow()
        {
            InitializeComponent();

            vertices = GetVertices(GetRawVertices());
            tsp = new TSP(vertices, 0, 1000, 10, 0.3);

            vertexBrush = new SolidBrush(Color.White);
            pathPen = new Pen(Color.Gray);
            bgColor = canvasBox.BackColor;

            working = false;
            editMode = false;

            updateStatusDelegate += UpdateStatus;
        }

        private void UpdateStatus(int generation, List<int> bestState, double bestFitness)
        {
            this.bestState = bestState;
            Text = InfoToString(generation, bestState, bestFitness);
            canvasBox.Refresh();
        }

        private string InfoToString(int generation, List<int> bestState, double bestFitness)
        {
            return $"{generation} - {-bestFitness:0.00} - {StateToString(bestState)}";
        }

        private string StateToString(List<int> state)
        {
            return String.Join("", state.Select(x => (char)(x + 'a')));
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
                gfx.FillEllipse(vertexBrush, vertex[0] - vertexSize / 2f, vertex[1] - vertexSize / 2f, vertexSize, vertexSize);
            }
        }

        private void HeavyOperation()
        {
            if (tsp.ResetStatus)
            {
                tsp.InitWithRandomPopulation();
            }
            
            while (working)
            {
                tsp.NextGeneration();
                var (bestState, bestFitness) = tsp.GetBestState();
                Invoke(updateStatusDelegate, tsp.CurrentGeneration, bestState, bestFitness);
            }
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

        private void toggleButton_Click(object sender, EventArgs e)
        {
            if (working)
            {
                working = false;
            }
            else
            {
                working = true;

                editButton.Enabled = false;

                workerThread = new Thread(new ThreadStart(HeavyOperation));
                workerThread.Start();
            }

            resetButton.Enabled = !working;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            workerThread.Abort();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            tsp.Reset();
            bestState = null;
            Text = "TSP App";
            canvasBox.Refresh();
            editButton.Enabled = true;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (editMode)
            {
                tsp = new TSP(vertices, 0, 1000, 10, 0.3);
            }

            editMode = !editMode;

            toggleButton.Enabled = !editMode;
            resetButton.Enabled = toggleButton.Enabled;
        }

        private void canvasBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!editMode)
            {
                return;
            }

            vertices.Add(new float[] { e.X, e.Y });
            canvasBox.Refresh();
        }
    }
}
