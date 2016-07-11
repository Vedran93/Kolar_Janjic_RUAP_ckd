using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CallRequestResponseService;

namespace Chronic_kidney_disease
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void predviđanje_Click(object sender, EventArgs e)
        {
            string[] data = new string[]
            {
                tb_age.Text,
                tb_bp.Text,
                cb_sg.Text,
                cb_al.Text,
                cb_su.Text,
                cb_rbc.Text,
                cb_pc.Text,
                cb_pcc.Text,
                cb_ba.Text,
                tb_bcr.Text,
                tb_bu.Text,
                tb_sc.Text,
                tb_sod.Text,
                tb_pot.Text,
                tb_hemo.Text,
                tb_pcv.Text,
                tb_wbcc.Text,
                tb_rbc.Text,
                cb_htn.Text,
                cb_dm.Text,
                cb_cad.Text,
                cb_appet.Text,
                cb_pe.Text,
                cb_ane.Text,
                "class"
            };

            RequestResponse.InvokeRequestResponseService(data).Wait();
            string predictedClass = getClass(RequestResponse.Result);
            Rezultat.Text = predictedClass;
            tb_msg.Text = "Response: " + RequestResponse.Result;
        }

        //Pronalaženje stringa koji pokazuje klasu u rezultatu
        private string getClass(string output)
        {
            //Idemo od kraja dok ne dođemo do prvih navodnika
            int c1 = output.Length - 1;
            while (output[c1] != '"') c1--;
            //Onda od tog mjesta opet tražimo navodnike
            int c2 = c1 - 1;
            while (output[c2] != '"') c2--;

            //Još jednom da dođemo do drugog seta navodnika
            c1 = c2 - 1;
            while (output[c1] != '"') c1--;
            c2 = c1 - 1;
            while (output[c2] != '"') c2--;

            //Na kraju pokupimo string između navodnika
            string predictedClass = output.Substring(c2 + 1, c1 - c2 - 1);
            return predictedClass;
        }
    }
}
