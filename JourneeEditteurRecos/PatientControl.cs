using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VidalAPI.Domain;
using VidalAPI.Services;

namespace ExemplePoso
{
    public partial class PatientControl : UserControl
    {
     VidalAPI.ProductLine vidalProduct;   
      
        public PatientControl(){
            InitializeComponent();
        
           
            //Initialisation de la boîte du choix du sexe

            genderBox.Items.Add(Gender.GENDER_MALE);
            genderBox.Items.Add(Gender.GENDER_FEMALE);

            genderBox.SelectedIndex = 0;

            //Initialisation des insuffisances hépatique :

            hepaticBox.Items.Add(HepaticInsufficiency.HEPATICINSUFFICIENCY_NONE);
            hepaticBox.Items.Add(HepaticInsufficiency.HEPATICINSUFFICIENCY_MODERATE);
            hepaticBox.Items.Add(HepaticInsufficiency.HEPATICINSUFFICIENCY_SEVERE);
            //hepaticBox.Items.Add(ServiceAnalysis.HepaticInsufficiency.HEPATICINSUFFICIENCY_JNULL);

            hepaticBox.SelectedIndex = 0;

            //Initialisation du contexte d'allaitement
            breathFeedingBox.Items.Add(Breastfeeding.BREASTFEEDING_NONE);
            breathFeedingBox.Items.Add(Breastfeeding.BREASTFEEDING_ALL);
            breathFeedingBox.Items.Add(Breastfeeding.BREASTFEEDING_LESS_THAN_ONE_MONTH);
            breathFeedingBox.Items.Add(Breastfeeding.BREASTFEEDING_MORE_THAN_ONE_MONTH);
            //breathFeedingBox.Items.Add(ServiceAnalysis..Breastfeeding.BREASTFEEDING_JNULL);
            breathFeedingBox.SelectedIndex = 0;

        }
        public void setVidalProduct( VidalAPI.ProductLine vidalProduct){
         this.vidalProduct = vidalProduct;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            patientJson.Items.Clear();
            patientJson.Items.Add(patientToJson());
 
        }

         private string patientToJson()
        {
            List<int> allergies = new List<int>();
            foreach (Allergy allergy in allergyPatientListBox.Items)
            {
                if (allergy.Id != null)
                {
                    allergies.Add((int)allergy.Id);
                }
            }

            List<int> molecules = new List<int>();
            foreach (Molecule molecule in moleculePatientListBox.Items)
            {
                if (molecule.Id != null)
                {
                    molecules.Add((int)molecule.Id);
                }
            }

            List<int> cims = new List<int>();
            foreach (Cim10 cim in cimPatientlistBox.Items)
            {
                if (cim.Id != null)
                {
                    cims.Add((int)cim.Id);
                }
            }

            int weight = -1;
            int height = -1;
            int creatin = -1;
            int amen = -1;
            if (poidCheckBox.Checked == false)
            {
                weight = (int)weightBox.Value;
            }
            if (TailleCheckBox.Checked == false)
            {
                height = (int)heightBox.Value;
            }
            if (creatinCheckBox.Checked == false)
            {
                creatin = (int)creatinBox.Value;
            }
            if (amenCheckBox.Checked == false)
            {
                amen = (int)amenBox.Value;
            }


            Gender gender = Gender.GENDER_JNULL;
            Breastfeeding breathFeeding = Breastfeeding.BREASTFEEDING_JNULL;
            HepaticInsufficiency hepatic = HepaticInsufficiency.HEPATICINSUFFICIENCY_JNULL;


            return vidalProduct.GetService<DrugPrescriptionAnalysisService>().PatientFromObjToJson(
                 monthCalendar1.SelectionStart,
                 (genderCheckBox.Checked == false ? (Gender)genderBox.SelectedItem : gender),
                 weight,
                 height,
                 amen,
                 (allaitCheckBox.Checked == false ? (Breastfeeding)breathFeedingBox.SelectedItem : breathFeeding),
                 creatin,
                 (hepatCheckBox.Checked == false ? (HepaticInsufficiency)hepaticBox.SelectedItem : hepatic),
                 allergies,
                 molecules,
                 cims);
            //return "{\"allergyIds\":null,\"breastFeeding\":null,\"creatin\":null,\"dateOfBirth\":63068400000,\"gender\":\"MALE\",\"height\":null,\"hepaticInsufficiency\":null,\"moleculeIds\":[],\"pathologyCim10Ids\":[],\"weeksOfAmenorrhea\":null,\"weight\":null}";


        }
        
       
        private void genderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (genderCheckBox.Checked == true)
            {
                genderBox.Visible = false;
            }
            else
            {
                genderBox.Visible = true;
            }
        }

        private void allaitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (allaitCheckBox.Checked == true)
            {
                breathFeedingBox.Visible = false;
            }
            else
            {
                breathFeedingBox.Visible = true;

            }
        }

        private void poidCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (poidCheckBox.Checked)
            {
                weightBox.Visible = false;
            }
            else
            {
                weightBox.Visible = true;

            }
        }

        private void TailleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TailleCheckBox.Checked)
            {
                heightBox.Visible = false;
            }
            else
            {
                heightBox.Visible = true;
            }
        }

        private void amenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (amenCheckBox.Checked)
            {
                amenBox.Visible = false;
            }
            else
            {
                amenBox.Visible = true;
            }
        }
        private void creatinCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (creatinCheckBox.Checked)
            {
                creatinBox.Visible = false;
            }
            else
            {
                creatinBox.Visible = true;
            }
        }

        private void hepatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hepatCheckBox.Checked)
            {
                hepaticBox.Visible = false;
            }
            else
            {
                hepaticBox.Visible = true;
            }
        }


        internal string getPatientJson()
        {
            if (patientJson != null && patientJson.Items.Count > 0)
            {
                return patientJson.Items[0].ToString();
            }
            return "";
        }
        private void patientLabel_Click(object sender, EventArgs e)
        {
            MessageBox.Show(patientJson.Items[0].ToString());
        }
        //ALLERGY PANEL
        private void allergieButton_Click(object sender, EventArgs e)
        {
            allergyResultlistBox.Items.Clear();
            AllergyList result = vidalProduct.GetService<AllergyService>().SearchByName(allergieTextBox.Text);
            allergyResultlistBox.Items.AddRange(result.ToArray<Allergy>());
        }

        private void allergyAdd_Click(object sender, EventArgs e)
        {
            if (allergyResultlistBox.SelectedItem != null)
            {
                allergyPatientListBox.Items.Add(allergyResultlistBox.SelectedItem);
            }
        }

        private void allergyRemove_Click(object sender, EventArgs e)
        {
            if (allergyPatientListBox.SelectedItem != null)
            {
                allergyPatientListBox.Items.Remove(allergyPatientListBox.SelectedItem);
            }
        }

        //MOLECULES PANEL
        private void moleculeButton_Click(object sender, EventArgs e)
        {
            moleculeSearchListBox.Items.Clear();
            MoleculeList result = vidalProduct.GetService<MoleculeService>().SearchOnlySecuredMoleculeByName(moleculeTextBox.Text);
            moleculeSearchListBox.Items.AddRange(result.ToArray<Molecule>());
        }

        private void moleculeAddButton_Click(object sender, EventArgs e)
        {
            if (moleculeSearchListBox.SelectedItem != null)
            {
                moleculePatientListBox.Items.Add(moleculeSearchListBox.SelectedItem);
            }
        }

        private void moleculeRemoveButton_Click(object sender, EventArgs e)
        {
            if (moleculePatientListBox.SelectedItem != null)
            {
                moleculePatientListBox.Items.Remove(moleculePatientListBox.SelectedItem);
            }
        }


        //Cim10 Panel
        private void cimSearchButton_Click(object sender, EventArgs e)
        {
            cimListBox.Items.Clear();
            Cim10List result = vidalProduct.GetService<Cim10Service>().SearchByName(cimTextBox.Text);
            cimListBox.Items.AddRange(result.ToArray<Cim10>());
        }

        private void cimAddbutton_Click(object sender, EventArgs e)
        {
            if (cimListBox.SelectedItem != null)
            {
                cimPatientlistBox.Items.Add(cimListBox.SelectedItem);
            }
        }

        private void cimRemovebutton_Click(object sender, EventArgs e)
        {
            if (cimPatientlistBox.SelectedItem != null)
            {
                cimPatientlistBox.Items.Remove(cimPatientlistBox.SelectedItem);
            }
        }



        internal bool getPoidCheckBox()
        {
           return poidCheckBox.Checked;
        }

        internal bool getTailleCheckBoxChecked()
        {
            return TailleCheckBox.Checked;
        }

        internal int getHeightBoxValue()
        {
            return (int)heightBox.Value;
        }
        internal int getWeightBoxValue()
        {
            return (int)weightBox.Value;
        }

        internal bool getCreatinCheckBoxChecked()
        {
            return creatinCheckBox.Checked;
        }

        internal int getCreatinBoxValue()
        {
            return (int)creatinBox.Value;
        }

        internal bool getWeeksOfAmenoBoxChecked()
        {
            return amenCheckBox.Checked;
        }
        internal int getWeeksOfAmenValue()
        {
            return (int)amenBox.Value;
        }

       
        internal bool getGenderCheckBoxChecked()
        {
            return genderCheckBox.Checked;
        }

        internal Gender? getGenderBoxSelectedItem()
        {
            return (Gender?)genderBox.SelectedItem;
        }

        internal bool getHepatCheckBoxChecked()
        {
           return hepatCheckBox.Checked;
        }

        internal HepaticInsufficiency? getHepaticBoxSelectedItem()
        {
            return (HepaticInsufficiency?)hepaticBox.SelectedItem;
        }
        
        internal Breastfeeding? getBreastFeedingSelectedItem()
        {
            return (Breastfeeding?)breathFeedingBox.SelectedItem;
        }
        internal bool getBrestFeedingBoxChecked()
        {
            return allaitCheckBox.Checked;
        }
       

        internal DateTime getMonthCalendar1SelectionStart()
        {
            return monthCalendar1.SelectionStart;
        }

        private void moleculeAddButton_Click_1(object sender, EventArgs e)
        {

        }

        private void moleculeRemoveButton_Click_1(object sender, EventArgs e)
        {

        }

        private void allergieButton_Click_1(object sender, EventArgs e)
        {

        }

        private void allergyAdd_Click_1(object sender, EventArgs e)
        {

        }

        private void allergyRemove_Click_1(object sender, EventArgs e)
        {

        }

        private void cimSearchButton_Click_1(object sender, EventArgs e)
        {

        }

        private void cimAddbutton_Click_1(object sender, EventArgs e)
        {

        }

        private void cimRemovebutton_Click_1(object sender, EventArgs e)
        {

        }
    }
    
}
