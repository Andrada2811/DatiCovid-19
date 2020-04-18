using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatiCovid_19_Lucan
{
  
    public class Covid_19
    {
        //Proprietà
        public string Data { get; set; }
        public string Stato { get; set; }
        public int CodiceRegione { get; set; }
        public string NomeRegione { get; set; }
        public string CodiceProvincia { get; set; }
        public string NomeProvincia { get; set; }
        public string SiglaProvincia { get; set; }
        public double Latitudine { get; set; }
        public double Longitudine { get; set; }
        public int CasiTotali{ get; set; }
        public string NoteIt { get; set; }
        public string NoteEn{ get; set; }


        //Metodi
        //Override del metodo ToString();
        public override string ToString()
        {
            return $"{this.Data}  \n Stato: {this.Stato} \n Codice regione: {CodiceRegione} \n Nome regione: {NomeRegione} \n Codice provincia: {CodiceProvincia}" +
                $" \n Nome provincia: {NomeProvincia} \n Sigla provincia: {SiglaProvincia} \n Latitudine: {Latitudine} \n Longitudine: {Longitudine} \n Casi totali: {CasiTotali} " +
                $"\n NoteIt: {NoteIt} \n NoteEn: {NoteEn} \n";
        }
    }
}
 