using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Tema2_SBC
{
    public partial class Form1 : Form
    {
        XDocument fapte;
        bool programareCheck;
        bool durataCheck;
        bool varstaCheck;
        bool antecedenteCheck;
        bool compusCheck;

        public Form1()
        {
            InitializeComponent();
            fapte = XDocument.Load("C:\\Users\\mirel\\OneDrive\\Desktop\\Materiale\\Anul4_Sem1\\SBC\\Teme\\Tema_2\\Tema_2\\Fapte.xml");


            varstaCheck = checkBox2.Checked;
            antecedenteCheck = checkBox3.Checked;
            durataCheck = checkBox4.Checked;
            compusCheck = checkBox1.Checked;
            comboBox1.Items.Add("Varsta");
            comboBox1.Items.Add("DurataProgramarii");

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            compusCheck = checkBox1.Checked;
        }


        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            varstaCheck = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            antecedenteCheck = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            durataCheck = checkBox4.Checked;
        }

        private string MakeStringPacienti(List<Pacient> pacienti)
        {
            StringBuilder rez = new StringBuilder();

            foreach (Pacient pacient in pacienti)
            {
                rez.AppendLine($"Nume: {pacient.Nume}");
                rez.AppendLine($"Varsta: {pacient.Varsta}");
                rez.AppendLine($"Antecedente: {pacient.Antecedente}");
                rez.AppendLine($"Specialitate: {pacient.Specialitate}");
                rez.AppendLine($"Nume Medic: {pacient.Nume_Medic}");
                rez.AppendLine($"Durata Programare: {pacient.Durata_programare} minute");
                rez.AppendLine($"Data Programare: {pacient.Data_programare}");
                rez.AppendLine($"Ora Programare: {pacient.Ora_programare}");
                rez.AppendLine("\r\n-------------------\r\n");
            }

            return rez.ToString();
        }


        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedChoice = comboBox1.SelectedItem.ToString();

            if (selectedChoice == "Varsta")
            {
                // Assuming fapte is your XDocument
                IEnumerable<XElement> pacienti = fapte.Descendants("pacient").OrderBy(pacient => (int)pacient.Element("varsta"));

                if (pacienti.Any())
                {
                    // Display the information for all pacient
                    StringBuilder result = new StringBuilder();

                    foreach (XElement pacient in pacienti)
                    {
                        string numePacient = (string)pacient.Attribute("nume");
                        int varstaPacient = (int)pacient.Element("varsta");


                        // Append the information for each patient
                        result.AppendLine($" Nume: {numePacient},  Varsta: {varstaPacient} ");
                    }

                    // Display the result in the TextBox
                    textBox1.Text = result.ToString();
                }
                else
                {
                    textBox1.Text = "No patients found.";
                }
            }


            if (selectedChoice == "DurataProgramarii")
            {
                // Assuming fapte is your XDocument
                IEnumerable<XElement> pacienti = fapte.Descendants("pacient").OrderBy(pacient => (int)pacient.Element("durata_programare"));


                if (pacienti.Any())
                {
                    // Display the information for all pacient
                    StringBuilder result = new StringBuilder();

                    foreach (XElement pacient in pacienti)
                    {
                        string numePacient = (string)pacient.Attribute("nume");
                        string numeSpecialitate = (string)pacient.Element("specialitate");
                        int durataProgramare = (int)pacient.Element("durata_programare");

                        // Append the information for each patient
                        result.AppendLine($" Nume: {numePacient}, " +
                            $" Specialitate:{numeSpecialitate}, " +
                            $" Durata Programarii: {durataProgramare} ");
                    }

                    // Display the result in the TextBox
                    textBox1.Text = result.ToString();
                }
                else
                {
                    textBox1.Text = "No patients found.";
                }
            }


        }


        private void button4_Click(object sender, EventArgs e)
        {
            string numePacient = textBox5.Text;
            string specialitate = textBox7.Text;
            string numeeMedic = textBox8.Text;


            if (!string.IsNullOrEmpty(numePacient))
            {
                string detaliiPacient = DetaliiPacient(numePacient);
                textBox1.Text = detaliiPacient;
            }
            else
            {
                textBox1.Text = "Introduceți numele pacientului.";
            }

            if (!string.IsNullOrEmpty(numeeMedic))
            {
                string detaliiMedic = DetaliiMedic(numeeMedic);
                textBox1.Text = detaliiMedic;
            }


            if (!string.IsNullOrEmpty(specialitate))
            {
                string detaliiSpecialitate = DetaliiSpecialitate(specialitate);
                textBox1.Text = detaliiSpecialitate;
            }


        }


        private void button6_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                MessageBox.Show("Selectați cel puțin o opțiune pentru căutare.", "Avertisment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IEnumerable<XElement> pacienti = fapte.Descendants("pacient");

            // Filtrare în funcție de vârstă
            if (varstaCheck)
            {
                pacienti = pacienti.Where(VarstaPacient);

            }

            // Filtrare în funcție de durata programare
            if (durataCheck)
            {
                pacienti = pacienti.Where(DurataProgramarii);


            }

            if (antecedenteCheck)
            {
                pacienti = pacienti.Where(AntecedentePacient);


            }

            if (compusCheck)
            {
                pacienti = pacienti.Where(CompusPacient);


            }

            // Afișează rezultatul în textBox1
            textBox1.Text = MakeStringPacienti(pacienti.Select(p => new Pacient(p)).ToList());
        }


        /*
         // Metoda pentru a transforma lista de pacienți în format text
         private string MakeStringPacienti(List<Pacient> pacienti)
         {
             string rez = "";
             foreach (Pacient pacient in pacienti)
             {
                 rez += "Nume: " + pacient.Nume + "\r\n"; 
                 rez += "Varsta: " + pacient.Varsta +"\r\n"; 
                 rez += "Antecedente: " + pacient.Antecedente +"\r\n"; 
                 rez += "Specialitate: " + pacient.Specialitate + "\r\n"; 
                 rez += "Nume Medic: " + pacient.Nume_Medic + "\r\n"; 
                 rez += "Durata Programare: " + pacient.Durata_programare + "\r\n";
                 rez += "Data Programare: " + pacient.Data_programare + "\r\n";
                 rez += "Ora Programare: " + pacient.Ora_programare;
                 rez += "\r\n\r\n-------------------\r\n\r\n";
             }
             return rez;
         }
        */


        /*
        private bool TreceFiltrare(XElement element)
        {
            // pacientul sa aiba o programare la una dintre specialitati
            if (programareCheck && !ProgramarePacient(element)) return false;
            // varsta pacientului sa fie peste 18 ani
            if (varstaCheck && !VarstaPacient(element)) return false;
            // durata programarii sa fie mai mica de 1h
            if (durataCheck && !DurataProgramarii(element)) return false;
            // pacientul sa aiba un istoric medical
            if (antecedenteCheck && !AntecedentePacient(element)) return false;

            return true;
        }

       /*

        //Regula 1
        // Regula pentru a afișa detalii despre pacienți în funcție de nume
        private string DetaliiPacient(string nume)
        {
            List<Pacient> pacientiList = new List<Pacient>();

            IEnumerable<XElement> pacientiXml = fapte.Descendants("pacient").Where(pacient => ((string)pacient.Attribute("nume")).Contains(nume));
            if (pacientiXml.Count() == 0) return "Acest nume de pacient nu există";

            foreach (XElement element in pacientiXml)
            {
                if (TreceFiltrare(element))
                {
                    Pacient pacient = new Pacient(element);
                    pacientiList.Add(pacient);
                }


            }

            return MakeStringPacienti(pacientiList);
        }
        */

        //Regula 1
        // Regula pentru a afișa detalii despre un pacient după nume
        private string DetaliiPacient(string numePacient)
        {
            List<Pacient> pacientiList = new List<Pacient>();
            IEnumerable<XElement> pacientiXml = fapte.Descendants("pacient").Where(pacient => ((string)pacient.Attribute("nume")).Equals(numePacient));

            if (pacientiXml.Any())
            {
                // Display the information for the pacient
                StringBuilder result = new StringBuilder();

                foreach (XElement pacientElement in pacientiXml)
                {
                    Pacient pacient = new Pacient(pacientElement);
                    pacientiList.Add(pacient);

                    //string numePacient = pacient.Nume;
                    int varstaPacient = pacient.Varsta;
                    string antecedentePacient = pacient.Antecedente;
                    string specialitate = pacient.Specialitate;
                    string numeMedic = pacient.Nume_Medic;
                    int durataProgramare = pacient.Durata_programare;
                    string dataProgramare = pacient.Data_programare;
                    string oraProgramare = pacient.Ora_programare;

                    // Append the information for the pacient
                    result.AppendLine($"Nume: {numePacient}");
                    result.AppendLine($"Varsta: {varstaPacient}");
                    result.AppendLine($"Antecedente: {antecedentePacient}");
                    result.AppendLine($"Specialitate: {specialitate}");
                    result.AppendLine($"Nume Medic: {numeMedic}");
                    result.AppendLine($"Durata Programare: {durataProgramare} minute");
                    result.AppendLine($"Data Programare: {dataProgramare}");
                    result.AppendLine($"Ora Programare: {oraProgramare}");
                    result.AppendLine("-------------------");
                }

                // Return the result
                return result.ToString();
            }

            return "Niciun pacient găsit pentru numele specificat.";
        }




        //Regula 2
        // Regula pentru a afișa detalii despre pacienți, numele medicului și durata programării pentru o specialitate dată
        private string DetaliiSpecialitate(string specialitate)
        {
            IEnumerable<XElement> pacientiXml = fapte.Descendants("pacient").Where(pacient => ((string)pacient.Element("specialitate")).Equals(specialitate));
            if (pacientiXml.Count() == 0) return $"Nu există aceasta specialitatea {specialitate}";

            StringBuilder result = new StringBuilder();

            foreach (XElement element in pacientiXml)
            {
                string numePacient = (string)element.Attribute("nume");
                string numeMedic = (string)element.Element("nume_medic");
                int durataProgramare = (int)element.Element("durata_programare");

                result.AppendLine($"Nume Pacient: {numePacient}");
                result.AppendLine($"Nume Medic: {numeMedic}");
                result.AppendLine($"Durata Programare: {durataProgramare} minute");
                result.AppendLine("-------------------");
            }

            return result.ToString();
        }

        // Regula 3
        // Regula pentru a afișa detalii despre pacienți, specializarea, data și ora programării pentru un medic dat
        private string DetaliiMedic(string numeMedic)
        {
            IEnumerable<XElement> pacientiXml = fapte.Descendants("pacient").Where(pacient => ((string)pacient.Element("nume_medic")).Equals(numeMedic));
            if (pacientiXml.Count() == 0) return $"Nu există acest medic {numeMedic}";

            StringBuilder result = new StringBuilder();

            foreach (XElement element in pacientiXml)
            {
                string numePacient = (string)element.Attribute("nume");
                string specialitate = (string)element.Element("specialitate");
                int durataProgramare = (int)element.Element("durata_programare");
                string dataProgramare = (string)element.Element("data_programarii");
                string oraProgramare = (string)element.Element("ora_programarii");

                result.AppendLine($"Nume Pacient: {numePacient}");
                result.AppendLine($"Specialitate: {specialitate}");
                result.AppendLine($"Durata Programare: {durataProgramare} minute");
                result.AppendLine($"Data Programare: {dataProgramare}");
                result.AppendLine($"Ora Programare: {oraProgramare}");
                result.AppendLine("-------------------");
            }

            return result.ToString();
        }

        //Regula 4
        // Regula pentru a afișa detalii despre pacienți, numele medicului și durata programării pentru o anumită dată
        private string DetaliiData(string dataCautata)
        {
            IEnumerable<XElement> pacientiXml = fapte.Descendants("pacient").Where(pacient => ((string)pacient.Element("data_programare")).Equals(dataCautata));
            if (pacientiXml.Count() == 0) return $"Nu există programări pentru data de {dataCautata}";

            StringBuilder result = new StringBuilder();

            foreach (XElement element in pacientiXml)
            {
                string numePacient = (string)element.Attribute("nume");
                string numeMedic = (string)element.Element("nume_medic");
                int durataProgramare = (int)element.Element("durata_programare");

                result.AppendLine($"Nume Pacient: {numePacient}");
                result.AppendLine($"Nume Medic: {numeMedic}");
                result.AppendLine($"Durata Programare: {durataProgramare} minute");
                result.AppendLine("-------------------");
            }

            return result.ToString();
        }

        //Regula 5
        // Regula verifica daca pacientul are programare la o specialitate
        private bool ProgramarePacient(XElement element)
        {
            // Specifică specialitățile căutate aici, fie prin variabile globale sau orice alt mecanism
            List<string> specialitatiCautate = new List<string> { "cardiologie", "neurologie", "diabetologie" };

            return element != null && specialitatiCautate.Any(specialitate => specialitate == (string)element.Element("specialitate"));
        }



        // Regula 6
        // Pacient peste 30 ani
        private bool VarstaPacient(XElement element)
        {
            if (element == null)
            {
                // Pacientul nu a fost găsit în XML
                return false;
            }

            // Extragem varsta pacientului din XML
            int varstaPacient = (int)element.Element("varsta");

            // Verificăm dacă pacientul are peste 30 de ani
            return varstaPacient > 30;
        }


        // Regula 7 
        // Verifică dacă durata programării este sub 30 de minute
        private bool DurataProgramarii(XElement element)
        {
            if (element == null)
            {
                // Pacientul nu a fost găsit în XML
                return false;
            }

            // Extragem durata programării a pacientului din XML
            int durataProgramare = (int)element.Element("durata_programare");

            // Verificăm dacă durata programării este sub 60 de minute
            return durataProgramare < 30;
        }

        // Regula 8
        // Verifică dacă pacientul are antecedente, are un istoric medical
        private bool AntecedentePacient(XElement element)
        {
            if (element == null)
            {
                // Pacientul nu a fost găsit în XML
                return false;
            }

            // Extragem antecedentele pacientului din XML
            string antecedentePacient = (string)element.Element("antecedente");

            // Verificăm dacă pacientul are antecedente și nu este "fara_antecedente"
            return !string.IsNullOrEmpty(antecedentePacient) && !antecedentePacient.Equals("fara_antecedente", StringComparison.OrdinalIgnoreCase);
        }

        //Regula    
        //Pacientii peste 18 ani cu programare la dr_smith

        // Regula 9
        // Verifică dacă pacientul are peste 18 ani și are programare la dr_smith
        private bool CompusPacient(XElement element)
        {
           
                // Extragem varsta pacientului din atribut
                int varstaPacient = (int)element.Element("varsta");

                // Extragem "nume_medic" din element, nu din atribut
                string numeMedic = (string)element.Element("nume_medic");

                // Verificăm dacă pacientul are atributul "nume_medic" și este "dr_smith"
                return numeMedic.Equals("dr_dark", StringComparison.OrdinalIgnoreCase) && varstaPacient > 18;
            }
    

        //Regula 9
        // Afiseaza cel mai in varsta pacient
        private void button1_Click(object sender, EventArgs e)
        {
            IEnumerable<XElement> pacienti = fapte.Descendants("pacient").OrderByDescending(pacient => (int)pacient.Element("varsta"));
            textBox2.Text = DetaliiPacient((string)pacienti.ElementAt(0).Attribute("nume"));
        }

        //Regula 10
        //Afiseaza cel mai tanar pacient
        private void button2_Click(object sender, EventArgs e)
        {
            IEnumerable<XElement> pacienti = fapte.Descendants("pacient").OrderBy(pacient => (int)pacient.Element("varsta"));
            textBox3.Text = DetaliiPacient((string)pacienti.ElementAt(0).Attribute("nume"));
        }

        //Regula 11
        // Regula pentru a calcula si afisa media de varstei pacientilor
        private void CalculMedieVarstaPacienti()
        {
            float medieVarsta = 0;
            IEnumerable<XElement> pacienti = fapte.Descendants("pacient");

            foreach (XElement element in pacienti)
                medieVarsta += (int)element.Element("varsta");

           // textBox4.Text = (medieVarsta / pacienti.Count()).ToString() + " ani";
            textBox4.Text = Math.Round((medieVarsta / pacienti.Count())).ToString() + " ani";

        }

        // Apelul regulii 11
        private void button3_Click(object sender, EventArgs e)
        {
            CalculMedieVarstaPacienti();
        }

        //Regula 12
        // Regula pentru a afișa numarul de programari sub 30
        private int NumarProgramariSub30Minute()
        {
            IEnumerable<XElement> pacienti = fapte.Descendants("pacient").Where(pacient => (int)pacient.Element("durata_programare") < 30);
            return pacienti.Count();
        }

        // Apelul regulii
        private void button5_Click(object sender, EventArgs e)
        {
            int numarProgramari = NumarProgramariSub30Minute();
            textBox6.Text = $"Numar: {numarProgramari}";
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        
    }
}
