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
    public partial class PrescriptionForm : Form
    {
        static VidalAPI.ProductLine vidalProduct = VidalAPI.ProductLine.GetProduct(VidalAPI.ProductLineID.VIDALEXPERT_PRODUCTID);
        public PrescriptionForm()
        {
            InitializeComponent();
            InitDomain();
            patientControl1.setVidalProduct(vidalProduct);

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
            Reco reco = ((Reco)listBox1.SelectedItem);
            if (reco != null && reco.UrlAsString != null && reco.UrlAsString!="")
            {
                webBrowser1.Navigate(reco.UrlAsString);
            }
            else
            {
                webBrowser1.Navigate(vidalProduct.GetService<RecoService>().GetRecoUrl(reco.Id));
            }
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listBox1.DataSource = vidalProduct.GetService<RecoService>().SearchByDomainIds(new List<int>(){((RecoDomain)listBox2.SelectedItem).Id}).ToArray();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox3.DataSource = vidalProduct.GetService<IndicationGroupService>().SearchByName(textBox1.Text).ToArray();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox4.DataSource = vidalProduct.GetService<ProductService>().SearchByName(textBox1.Text).ToArray();
        }

        private void listBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listBox1.DataSource = vidalProduct.GetService<RecoService>().SearchByIndicationGroupIds(new List<int>(){((IndicationGroup)listBox3.SelectedItem).Id}).ToArray();
        }

        private void listBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listBox1.DataSource = vidalProduct.GetService<RecoService>().SearchAllRecosByProductIds(new List<int>(){((Product)listBox4.SelectedItem).Id }).ToArray();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.DataSource =  vidalProduct.GetService<DrugPrescriptionAnalysisService>().SearchRecosByPrescriptions(patientControl1.getPatientJson(), new List<string>()).Select(l=>l.Recommendation).ToArray();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Product product = ((Product)listBox4.SelectedItem);
            listBox5.Items.Add(product);
            computeContextualRecos();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem!=null){
            listBox5.Items.Remove(listBox5.SelectedItem);
            }
            computeContextualRecos();
        }

        private void button7_Click(object sender, EventArgs e)
        {
         listBox5.Items.Clear();
         computeContextualRecos();
        }
        private void computeContextualRecos()
        {
            List<string> prescription = new List<string>();
            foreach (Product product in listBox5.Items)
            {
                prescription.Add(vidalProduct.GetService<DrugPrescriptionAnalysisService>().PrescriptionLineFromObjToJson(-1,-1,PosologyFrequencyType.POSOLOGYFREQUENCYTYPE_JNULL,new List<int>(),new List<int>(),product.Id,DrugType.DRUGTYPE_PRODUCT));
            }
            listBox1.DataSource = vidalProduct.GetService<DrugPrescriptionAnalysisService>().SearchRecosByPrescriptions(patientControl1.getPatientJson(), new List<string>()).Select(l=>l.Recommendation).ToArray();
             
        }
    }
        
}
