using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naive_Bayesian
{
    public partial class Form1 : Form
    {
        public string[] nama_kategori;
        //public string[,] nama_nilai, nilai;
        public int jumlah_kategori, jumlah_data,jumlah_nilai;

        public Form1()
        {
            InitializeComponent();

            button2.Enabled = false;
        }

        #region even

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile1 = new OpenFileDialog();
            openfile1.Filter = "File excel (*.xls; *.xlsx)|*.xls; *xlsx";
            if (openfile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //string pathconn = "Provider = Microsoft.jet.OLEDB.4.0; Data source=" + openfile1.FileName + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";            
                //string pathconn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + openfile1.FileName + ";Extended Properties=Excel 12.0;HDR=Yes;IMEX=1";
                string pathconn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + openfile1.FileName + ";Extended Properties='Excel 12.0;IMEX=1;'";

                //OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + openfile1.FileName + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;';");
                OleDbConnection conn = new OleDbConnection(pathconn);
                OleDbDataAdapter da = new OleDbDataAdapter("Select * from [Sheet1$]", conn);

                //da.TableMappings.Add("Table", "Net-informations.com");
                System.Data.DataSet dt = new System.Data.DataSet();
                da.Fill(dt);
                dataGridView1.DataSource = dt.Tables[0];
                conn.Close();

                jumlah_kategori = dataGridView1.ColumnCount-1;
                nama_kategori = new string[jumlah_kategori];

                jumlah_data = dataGridView1.Rows.Count;

                simpan_nama_kategori();

                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bayesian a = new Bayesian();
            a.Show();
        }

        #endregion

        #region method

        private void simpan_nama_kategori()
        {
            for (int i = 0; i < jumlah_kategori; i++)
            {
                nama_kategori[i] = dataGridView1.Columns[i].HeaderText;
            }
        }

        #endregion

    }
}
