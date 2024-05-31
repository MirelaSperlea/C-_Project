
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Tema2_SBC
{
    public class Pacient
    {
        public string Nume { get; set; }
        public int Varsta { get; set; }
        public string Antecedente { get; set; }
        public string Specialitate { get; set; }
        public string Nume_Medic { get; set; }
        public int Durata_programare { get; set; }
        public string Data_programare { get; set; }
        public string  Ora_programare{ get; set; }
        public Pacient(XElement element)
        {
            Nume = (string)element.Attribute("nume");
            Varsta = (int)element.Element("varsta");
            Antecedente = (string)element.Element("antecedente");
            Specialitate = (string)element.Element("specialitate");
            Nume_Medic = (string)element.Element("nume_medic");
            Durata_programare = (int)element.Element("durata_programare");
            Data_programare= (string)element.Element("data_programarii");
            Ora_programare = (string)element.Element("ora_programarii");

        }
    }
}
