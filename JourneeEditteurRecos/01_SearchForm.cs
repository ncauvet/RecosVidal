using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VidalAPI.Services;
using VidalAPI.Domain;

namespace JourneeEditteurRecos
{
    public partial class SearchForm : Form
    {
        static VidalAPI.ProductLine vidalProduct = VidalAPI.ProductLine.GetProduct(VidalAPI.ProductLineID.VIDALEXPERT_PRODUCTID);
        public SearchForm()
        {
            InitializeComponent();
            InitDomain();
            //patientControl.setVidalProduct(vidalProduct);

        }

        private void InitDomain()
        {
            listBox2.DataSource = vidalProduct.GetService<RecoService>().SearchAllDomains().ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = vidalProduct.GetService<RecoService>().SearchByName(textBox1.Text).ToArray();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate(((Reco)listBox1.SelectedItem).UrlAsString);
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listBox1.DataSource = vidalProduct.GetService<RecoService>().SearchByDomainIds(new List<int>(){((RecoDomain)listBox2.SelectedItem).Id}).ToArray();
        }
    }
        
}
