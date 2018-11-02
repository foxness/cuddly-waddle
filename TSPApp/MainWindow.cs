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
        public MainWindow()
        {
            InitializeComponent();
        }

        private string GetRawVertices()
        {
            return @"A 215 -40
B 187 -70
C 130 -80
D 230 -90
E 150 -120
F 540 -50
G 500 -100
H 375 -120
I 470 -130
J 507 -212
K 470 -290
L 380 -380
M 300 -390
N 270 -330
O 250 -380
P 210 -400
R 240 -430
S 280 -420
T 210 -440
U 255 -466
1 580 -50
2 620 -80
3 647 -112
4 669 -144
5 683 -176
6 687 -220
7 691 -260";
        }

        private List<double[]> GetVertices(string raw)
        {
            return raw
                .Replace("\r", "")
                .Split('\n')
                .Select(x => x.Split(' '))
                .Select(x => new[] { Double.Parse(x[1]), Double.Parse(x[2]) })
                .ToList();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            Hide();

            var vertices = GetVertices(GetRawVertices());

            var tsp = new TSP(vertices, 0, 1000, 10, 0.3);
            tsp.Run(100);

            Console.WriteLine("end");
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
