using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DXApplication2
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        SqlConnection con = new SqlConnection(@"Data Source=CTRLSOFT-CP378L\SQL;Initial Catalog=TEST;Persist Security Info=True;User ID=sa;Password=123");
        double totalClass;
        public Form1()
        {
            InitializeComponent();
        }

        private void openButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabPage1.Show();
            disp();
 
        }

        public void disp() {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select ROLL,NAME,TOTAL_CLASS,ATTEND_CLASS,CT1,CT2,CT3,AVARAGE_MARK,att_mark ATTENDENCE_MARK,TOTAL_MARK from MARKS WHERE session_no='"+comboBox1.Text+"'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        

        private void insertButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabPage2.Show();
           // insert_data();
            //textBox1.Text = "";
            //dataGridView2.Rows.Clear();
           // comboBox1.Text = "";

        }

        public void insert_data()
        {
            try
            {

                con.Open();
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into MARKS(roll,name,session_no) values('" + dataGridView2.Rows[i].Cells[0].Value + "','" + dataGridView2.Rows[i].Cells[1].Value + "','" + textBox1.Text + "')";
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }

            catch {
                MessageBox.Show("THE INSERTED ROLL IS ALREADY EXISTS");
            }

        }

        private void removeButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            remove_data();
            dataGridView1.Refresh();
            disp();
        }

        public void remove_data() {

            try
            {
                int row = dataGridView1.CurrentCell.RowIndex;

                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from marks where roll='" + dataGridView1.Rows[row].Cells[0].Value + "'";
                cmd.ExecuteNonQuery();
                con.Close();
            }

            catch
            {

            }
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void dropDownCombobox(object sender, EventArgs e)
        {

            comboBox1.Items.Clear();
            comboBox4.Items.Clear();
            drop_down_combobox();
            
        }

        public void drop_down_combobox() {

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select distinct session_no from MARKS";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                comboBox1.Items.Add(dr["session_no"]);
                comboBox4.Items.Add(dr["session_no"]);
            }

            con.Close();
        }

        private void cellClick(object sender, DataGridViewCellEventArgs e)
        {
            addMarks();
            int tClass = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[tClass].Cells[2].Value = totalClass.ToString();
        }

        public void addMarks() {

            try
            {
                for (int i = 0; i < dataGridView1.CurrentRow.Index; i++)
                {

                    totalClass = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                    double attendClass = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
                    double atMark = (attendClass / totalClass) * 100;
                    
                    int attMark2;
                    if (atMark >= 95)
                        attMark2 = 8;
                    else if (atMark >= 90 && atMark <= 94)
                        attMark2 = 7;
                    else if (atMark >= 85 && atMark <= 89)
                        attMark2 = 6;
                    else if (atMark >= 80 && atMark <= 84)
                        attMark2 = 5;
                    else if (atMark >= 75 && atMark <= 79)
                        attMark2 = 4;
                    else if (atMark >= 70 && atMark <= 74)
                        attMark2 = 3;
                    else if (atMark >= 60 && atMark <= 69)
                        attMark2 = 2;
                    else if (atMark >= 60 && atMark <= 64)
                        attMark2 = 1;
                    else
                        attMark2 = 0;
                    dataGridView1.Rows[i].Cells[8].Value = attMark2.ToString();

                    double[] arra = new double[3];
                    arra[0] = Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
                    arra[1] = Convert.ToDouble(dataGridView1.Rows[i].Cells[5].Value);
                    arra[2] = Convert.ToDouble(dataGridView1.Rows[i].Cells[6].Value);

                    Array.Sort(arra);
                    double avr = (arra[1] + arra[2]) / 2.0;
                    int avr1 = Convert.ToInt16(avr);
                    int total = Convert.ToInt16(avr) + Convert.ToInt16(attMark2);
                    int total1 = Convert.ToInt16(total);



                    dataGridView1.Rows[i].Cells[7].Value = avr1.ToString();
                    dataGridView1.Rows[i].Cells[9].Value = total1.ToString();


                }
            }

            catch
            {

            }

            

        }

        private void saveButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabPage1.CanFocus) {
                save_data();
               // MessageBox.Show("DATA SAVE SUCCESSFULLY");

                delete_tempData();
                tempData();
            }

            //INSERT DATA
            if (xtraTabPage2.CanFocus) {
                insert_data();
                textBox1.Text = "";
                dataGridView2.Rows.Clear();
                comboBox1.Text = "";
               // MessageBox.Show("DATA INSERT.");
            }

            //cource update information
            if (xtraTabPage3.CanFocus) {
                cource_update();
            }
            //Cource insert option
            if (xtraTabPage4.CanFocus) {
                courceInsert();
                dataGridView3.Rows.Clear();
            }
           // messageReport();
        }

        public void save_data() {

            con.Open();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {


                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update marks set total_class='" + dataGridView1.Rows[i].Cells[2].Value + "',attend_class='" + dataGridView1.Rows[i].Cells[3].Value + "',ct1='" + dataGridView1.Rows[i].Cells[4].Value + "',ct2='" + dataGridView1.Rows[i].Cells[5].Value + "',ct3='" + dataGridView1.Rows[i].Cells[6].Value + "',avarage_mark='" + dataGridView1.Rows[i].Cells[7].Value + "',att_mark='" + dataGridView1.Rows[i].Cells[8].Value + "',total_mark='" + dataGridView1.Rows[i].Cells[9].Value + "' where roll='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                cmd.ExecuteNonQuery();


            }
            con.Close();

        }

        public void tempData() {

            con.Open();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {


                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into temp_mark values('" + dataGridView1.Rows[i].Cells[0].Value + "','"+dataGridView1.Rows[i].Cells[1].Value+"','" + dataGridView1.Rows[i].Cells[4].Value + "','" + dataGridView1.Rows[i].Cells[5].Value + "','" + dataGridView1.Rows[i].Cells[6].Value + "','" + dataGridView1.Rows[i].Cells[7].Value + "','" + dataGridView1.Rows[i].Cells[8].Value + "','" + dataGridView1.Rows[i].Cells[9].Value + "')";
                cmd.ExecuteNonQuery();


            }
            con.Close();

        }

        public void delete_tempData() {

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from temp_mark";
            cmd.ExecuteNonQuery();
            con.Close();

        }

        private void resetButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reset_data();
            disp();
        }

        public void reset_data() {

            con.Open();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {


                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update marks set total_class=0,attend_class=0,ct1=0,ct2=0,ct3=0,avarage_mark=0,att_mark=0,total_mark=0 where roll='" + dataGridView1.Rows[i].Cells[0].Value + "'";
                cmd.ExecuteNonQuery();


            }
            con.Close();

        }

        private void showButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabPage1.Show();
            comboBox1.Text = "";
            showData();
        }

        public void showData() {

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from marks";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;


        }

        private void reportButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f2 = new Form2();
            f2.Show();
            
        }

        private void cource_update_button_click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabPage3.Show();
            
        }
        public void cource_update() {
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update cource_info set cource_code='" + comboBox2.Text + "',cource_title='" + comboBox3.Text + "',session_no='" + comboBox4.Text + "' where id=1";
                cmd.ExecuteNonQuery();
                con.Close();

                // MessageBox.Show("COURCE UPDATE COMPLETE.");

                comboBox2.Text = "";
                comboBox3.Text = "";
                comboBox4.Text = "";
            }

            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void sessionCombobox(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            drop_down_combobox();
        }

        private void courceInfoDropDown(object sender, DragEventArgs e)
        {
            
        }
        public void cource_info_dropdown()
        {

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select cource_code,cource_title from t_cource_info";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                comboBox2.Items.Add(dr["cource_code"]);
                comboBox3.Items.Add(dr["cource_title"]);
            }

            con.Close();
        }

        private void cource_info_dropDown(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            cource_info_dropdown();
        }

        public void messageReport()
        {
            DialogResult result = MessageBox.Show("INSERT DATA SUCCESSFULLY.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void courceInsertButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabPage4.Show();
        }
        public void courceInsert()
        {
            con.Open();
            try
            {
                for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into t_cource_info values('" + dataGridView3.Rows[i].Cells[0].Value + "','" + dataGridView3.Rows[i].Cells[1].Value + "')";
                    cmd.ExecuteNonQuery();
                }  
            }
            catch{
                MessageBox.Show("DATA DUPLICATION PROBLEM");
            }
            con.Close();

        }
    }
}
