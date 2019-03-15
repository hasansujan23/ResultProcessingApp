using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DXApplication2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            using (TESTEntities db = new TESTEntities()) {
                TEMP_MARKBindingSource.DataSource = db.TEMP_MARK.ToList();
                COURCE_INFOBindingSource.DataSource = db.COURCE_INFO.ToList();
                reportViewer1.RefreshReport();
            }
            //this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
