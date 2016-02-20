using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naive_Bayesian
{
    public partial class Bayesian : Form
    {
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Form1"];

        int jumlah_kategori, jumlah_data, jumlah_kelas;
        string[] nama_kategori, nama_kelas;
        int[] jumlah_tiap_kelas;

        public Bayesian()
        {
            InitializeComponent();

            init();
        }


        #region even

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index;
            comboBox2.Items.Clear();

            //menambahkan item ke combobox 2
            for (int i = 0; i < jumlah_data; i++)
            {
                if (i == 0)
                {
                    comboBox2.Items.Add(((Form1)f).dataGridView1.Rows[0].Cells[comboBox1.SelectedIndex].Value.ToString());

                }
                else
                {
                    index = comboBox2.FindString(((Form1)f).dataGridView1.Rows[i].Cells[comboBox1.SelectedIndex].Value.ToString());
                    if (index == -1)
                    {
                        comboBox2.Items.Add(((Form1)f).dataGridView1.Rows[i].Cells[comboBox1.SelectedIndex].Value.ToString());
                    }
                }
                if (i == jumlah_data - 1)
                {
                    selek_combobox2();
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int indek_kategori;
                indek_kategori = comboBox1.SelectedIndex;

                //mengubah nilai di datagridview
                for (int i = 0; i < jumlah_kategori * jumlah_kelas; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == nama_kategori[indek_kategori])
                    {
                        dataGridView1.Rows[i].Cells[1].Value = comboBox2.SelectedItem.ToString();
                    }
                }

                hitung_probabilitas();

                hitung_hasil_akhir();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }

        }

        #endregion

        #region method

        private void init()
        {
            int indek;
            Color warna = Color.Green;
            jumlah_kategori = ((Form1)f).jumlah_kategori;
            jumlah_data = ((Form1)f).jumlah_data;
            nama_kategori = ((Form1)f).nama_kategori;

            //isi nama kelas
            cari_banyak_kelas();

            //isi kategori di datagridview
            indek = -1;
            for (int i = 0; i < jumlah_kategori; i++)
            {
                for (int j = 0; j < jumlah_kelas; j++)
                {
                    indek += 1;
                    dataGridView1.Rows.Add(1);
                    dataGridView1.Rows[indek].Cells[0].Value = nama_kategori[i];

                    //dataGridView1.Rows[indek].DefaultCellStyle.BackColor = warna;
                }
            }

            //isi nilai awal untuk tiap kategori
            indek = -1;
            for (int i = 0; i < jumlah_kategori; i++)
            {
                for (int j = 0; j < jumlah_kelas; j++)
                {
                    indek += 1;
                    dataGridView1.Rows[indek].Cells[1].Value = ((Form1)f).dataGridView1.Rows[0].Cells[i].Value.ToString();
                }
            }

            //isi nilai kelas ke datagridview
            indek = -1;
            for (int i = 0; i < jumlah_kategori; i++)
            {
                for (int j = 0; j < jumlah_kelas; j++)
                {
                    indek += 1;
                    dataGridView1.Rows[indek].Cells[2].Value = nama_kelas[j];
                }
            }

            hitung_probabilitas();

            hitung_hasil_akhir();

            //tambah semua kategori ke combobox 1
            for (int i = 0; i < jumlah_kategori; i++)
            {
                comboBox1.Items.Add(nama_kategori[i]);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void cari_banyak_kelas()
        {
            jumlah_kelas = 0;
            int indek = -1;
            string[] tmp = new string[jumlah_data];
            for (int i = 0; i < jumlah_data; i++)
            {
                if (i == 0)
                {
                    indek += 1;
                    jumlah_kelas += 1;
                    tmp[indek] = ((Form1)f).dataGridView1.Rows[i].Cells[jumlah_kategori].Value.ToString();

                }
                else
                {
                    if (tmp.Contains(((Form1)f).dataGridView1.Rows[i].Cells[jumlah_kategori].Value.ToString()) == false)
                    {
                        indek += 1;
                        jumlah_kelas += 1;
                        tmp[indek] = ((Form1)f).dataGridView1.Rows[i].Cells[jumlah_kategori].Value.ToString();
                    }
                }
            }

            nama_kelas = new string[jumlah_kelas];
            jumlah_tiap_kelas = new int[jumlah_kelas];

            //menyimpan nama kelas
            for (int i = 0; i < jumlah_kelas; i++)
            {
                nama_kelas[i] = tmp[i];
            }

            //menghitung jumlah tiap kelas
            for (int i = 0; i < jumlah_data; i++)
            {
                for (int j = 0; j < jumlah_kelas; j++)
                {
                    if (nama_kelas[j] == ((Form1)f).dataGridView1.Rows[i].Cells[jumlah_kategori].Value.ToString())
                    {
                        jumlah_tiap_kelas[j] += 1;
                    }
                }
            }
        }

        private void hitung_probabilitas()
        {
            int indek_kategori, jumlah, indek_kelas;
            string a, b, c, d;
            float hasil;

            for (int i = 0; i < jumlah_kategori * jumlah_kelas; i++)
            {
                jumlah = 0;
                indek_kategori = Array.IndexOf(nama_kategori, dataGridView1.Rows[i].Cells[0].Value.ToString());
                for (int j = 0; j < jumlah_data; j++)
                {
                    //nilai kategori
                    a = ((Form1)f).dataGridView1.Rows[j].Cells[indek_kategori].Value.ToString();
                    b = dataGridView1.Rows[i].Cells[1].Value.ToString();

                    //nilai kelas
                    c = ((Form1)f).dataGridView1.Rows[j].Cells[jumlah_kategori].Value.ToString();
                    d = dataGridView1.Rows[i].Cells[2].Value.ToString();

                    if ((a == b) && (c == d))
                    {
                        jumlah += 1;
                    }
                }
                indek_kelas = Array.IndexOf(nama_kelas, dataGridView1.Rows[i].Cells[2].Value.ToString());
                hasil = (float)jumlah / (float)jumlah_tiap_kelas[indek_kelas];
                dataGridView1.Rows[i].Cells[3].Value = jumlah.ToString() + "/" + jumlah_tiap_kelas[indek_kelas].ToString();
                dataGridView1.Rows[i].Cells[4].Value = hasil.ToString();
            }
        }

        private void hitung_hasil_akhir()
        {
            string tmp;
            Boolean pertama;
            dataGridView2.Rows.Clear();

            //mencetak semua kelas dan perhitungan
            for (int i = 0; i < jumlah_kelas; i++)
            {
                pertama = true;
                dataGridView2.Rows.Add(1);
                dataGridView2.Rows[i].Cells[0].Value = "Kelas = " + nama_kelas[i];

                for (int j = 0; j < jumlah_kategori * jumlah_kelas; j++)
                {
                    if (dataGridView1.Rows[j].Cells[2].Value.ToString() == nama_kelas[i])
                    {
                        if (pertama == false)
                        {
                            tmp = dataGridView1.Rows[j].Cells[3].Value.ToString();
                            dataGridView2.Rows[i].Cells[1].Value += " . " + tmp;
                        }
                        else
                        {
                            tmp = dataGridView1.Rows[j].Cells[3].Value.ToString();
                            dataGridView2.Rows[i].Cells[1].Value += tmp;
                            pertama = false;
                        }
                    }
                    if (j == jumlah_kategori * jumlah_kelas - 1)
                    {
                        tmp = " . " + jumlah_tiap_kelas[i] + "/" + jumlah_data.ToString();
                        dataGridView2.Rows[i].Cells[1].Value += tmp;
                    }
                }
            }

            //menghitung hasil akhir
            float tmp_hasil;
            for (int i = 0; i < jumlah_kelas; i++)
            {
                tmp_hasil = 1;
                for (int j = 0; j < jumlah_kategori * jumlah_kelas; j++)
                {
                    if (dataGridView1.Rows[j].Cells[2].Value.ToString() == nama_kelas[i])
                    {
                        tmp_hasil *= float.Parse(dataGridView1.Rows[j].Cells[4].Value.ToString());
                    }

                    if (j == jumlah_kategori * jumlah_kelas - 1)
                    {
                        tmp_hasil *= ((float)jumlah_tiap_kelas[i] / (float)jumlah_data);
                        dataGridView2.Rows[i].Cells[2].Value = tmp_hasil;
                    }
                }
            }

            //cek nilai terbesar untuk solusi
            int indek_solusi = -1;
            for (int i = 0; i < jumlah_kelas; i++)
            {
                if (i == 0)
                {
                    indek_solusi = 0;
                }
                else
                {
                    if (float.Parse(dataGridView2.Rows[i].Cells[2].Value.ToString()) > float.Parse(dataGridView2.Rows[indek_solusi].Cells[2].Value.ToString()))
                    {
                        indek_solusi = i;
                    }
                }

                if (i == jumlah_kelas - 1)
                {
                    textBox1.Text = nama_kelas[indek_solusi];
                }
            }
        }

        private void selek_combobox2()
        {
            int indek_c1 = comboBox1.SelectedIndex;
            string tmp = dataGridView1.Rows[indek_c1 * jumlah_kelas].Cells[1].Value.ToString();

            int indek_c2 = comboBox2.FindString(tmp);
            comboBox2.SelectedIndex = indek_c2;
        }

        #endregion

    }
}
