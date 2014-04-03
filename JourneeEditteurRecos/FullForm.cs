using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JourneeEditteurRecos
{
    public partial class FullForm : Form
    {
        static VidalAPI.ProductLine vidalProduct = VidalAPI.ProductLine.GetProduct(VidalAPI.ProductLineID.VIDALEXPERT_PRODUCTID);
        public FullForm()
        {            
            InitializeComponent();
            patientControl.setVidalProduct(vidalProduct);
            intiDomain();
        }

        private void intiDomain()
        {
            listBox2.DataSource = vidalProduct.GetService<VidalAPI.Services.RecoService>().SearchAllDomains().ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            listBox1.DataSource = vidalProduct.GetService<VidalAPI.Services.RecoService>().SearchByName(textBox1.Text).ToArray() ;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            VidalAPI.Domain.Reco reco = (VidalAPI.Domain.Reco)listBox1.SelectedItem;
            linkLabel1.Text = "http://www.vidal.fr/recommandations/" + reco.Id +"/"+ reco.Name.ToLower();
            linkLabel2.Text = "http://www.vidalhoptimal.fr/showReco.html?recoId=" + reco.Id + "&&token=dde6533fe38755c9";

            if ((((VidalAPI.Domain.Reco)listBox1.SelectedItem).UrlAsString) != "")
            {
                webBrowser1.Navigate(reco.UrlAsString);
            }
            else
            {
                webBrowser1.Navigate(vidalProduct.GetService<VidalAPI.Services.RecoService>().GetRecoUrl(reco.Id));
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            listBox1.DataSource = vidalProduct.GetService<VidalAPI.Services.RecoService>().SearchByDomainIds(new List<int>(){((VidalAPI.Domain.RecoDomain)listBox2.SelectedItem).Id}).ToArray(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox3.DataSource = vidalProduct.GetService<VidalAPI.Services.IndicationGroupService>().SearchByName(textBox1.Text).ToArray();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.DataSource = vidalProduct.GetService<VidalAPI.Services.RecoService>().SearchByIndicationGroupIds(new List<int>() { ((VidalAPI.Domain.IndicationGroup)listBox3.SelectedItem).Id }).ToArray();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox4.DataSource = vidalProduct.GetService<VidalAPI.Services.SearchDrugService>().SearchDrugs(textBox1.Text, new List<VidalAPI.Domain.DrugType> { VidalAPI.Domain.DrugType.DRUGTYPE_PRODUCT }, new List<VidalAPI.Domain.ProductType>() { VidalAPI.Domain.ProductType.PRODUCTTYPE_VIDAL }, new List<VidalAPI.Domain.MarketStatus>(){VidalAPI.Domain.MarketStatus.MARKETSTATUS_AVAILABLE}).Products.ToArray();
            
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.DataSource = vidalProduct.GetService<VidalAPI.Services.RecoService>().SearchAllRecosByProductIds(new List<int>() { ((VidalAPI.Domain.SearchResultProductDTO)listBox4.SelectedItem).Id }).ToArray();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox5.Items.Add(listBox4.SelectedItem);
            computeSuggestion();
        }

        private void computeSuggestion()
        {
            //String patient = "{\"allergyIds\":[],\"breastFeeding\":\"NONE\",\"creatin\":100,\"dateOfBirth\":1395705600000,\"gender\":\"MALE\",\"height\":165.0,\"hepaticInsufficiency\":\"NONE\",\"moleculeIds\":[],\"pathologyCim10Ids\":[4325,4328,4331],\"weeksOfAmenorrheav\":0,\"weight\":65.0}";
            String patient =  patientControl.getPatientJson();
            List<String> prescription = new List<String>();
            foreach (VidalAPI.Domain.SearchResultProductDTO product in listBox5.Items)
            {
                prescription.Add(vidalProduct.GetService<VidalAPI.Services.DrugPrescriptionAnalysisService>().PrescriptionLineFromObjToJson(-1, -1, VidalAPI.Domain.PosologyFrequencyType.POSOLOGYFREQUENCYTYPE_JNULL, new List<int>(), new List<int>(), product.Id, VidalAPI.Domain.DrugType.DRUGTYPE_PRODUCT));
            }
            listBox1.DataSource = vidalProduct.GetService<VidalAPI.Services.DrugPrescriptionAnalysisService>().SearchRecosByPrescriptions(patient, prescription).Select(l=>l.Recommendation).ToArray();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel2.Text);
        }

        private void patientControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
