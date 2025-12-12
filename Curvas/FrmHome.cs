using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Curvas
{
    public partial class FrmHome : Form
    {
        public FrmHome()
        {
            InitializeComponent();
        }

        private void bézierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCurvasBéizer frm = new FrmCurvasBéizer();
            frm.MdiParent = this;
            frm.Show();
        }

        private void bSplineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBSpline frm = new FrmBSpline();
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
