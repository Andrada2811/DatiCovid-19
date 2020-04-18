using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace DatiCovid_19_Lucan
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Richiamo la classe CancellationTokenSource e ne creo una variabile
        CancellationTokenSource cancToken;


        public MainWindow()
        {
            InitializeComponent();
        }


        //Bottone per caricare i dati
        private void Btn_CaricaDati_Click(object sender, RoutedEventArgs e)
        {
            
            //Richiamo il metodo per popolare la ListBox
            Task.Factory.StartNew(() => CaricaDatiCovid());

            //Creo l'oggetto canToken
            cancToken = new CancellationTokenSource();

            //Dopo aver cliccato il bottone Aggiorna disattivo il bottone e attivo quello per interrompere il caricamento
            Btn_CaricaDati.IsEnabled = false;
            Btn_Interrompi.IsEnabled = true;

            //Svuoto il contenuto della ListBox
            Lst_VediDati1.Items.Clear();

        }


        //Metodo estratto per caricare idati nella ListBox
        private void CaricaDatiCovid()
        {
            string path = @"DatiCovid.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlRoot = xmlDoc.Element("root");
            var xmlRow = xmlRoot.Elements("row");


            //Rallento il caricamneto usando Thread.Sleep()
            Thread.Sleep(1000);


            //Mi scorro i valori della lista del file DatiCovid-19_Lucan
            foreach (var item in xmlRow)
            {
                //Cerco i valori nei diversi tag
                XElement xmlData = item.Element("data");
                XElement xmlStato = item.Element("stato");
                XElement xmlCodiceRegione = item.Element("codice_regione");
                XElement xmlNomeRegione = item.Element("denominazione_regione");
                XElement xmlCodiceProvincia = item.Element("codice_provincia");
                XElement xmlNomeProvincia = item.Element("denominazione_provincia");
                XElement xmlSiglaProvincia = item.Element("sigla_provincia");
                XElement xmlLatitudine = item.Element("lat");
                XElement xmlLongitudine = item.Element("long");
                XElement xmlCasiTotali = item.Element("totale_casi");
                XElement xmlNoteIt = item.Element("note_it");
                XElement xmlNoteEn = item.Element("note_en");


                //Creo l'istanza della classe Covid-19 ----> parte relativa all oggetto Covid-19
                Covid_19 datiCovid = new Covid_19();
                datiCovid.Data = xmlData.Value;
                datiCovid.Stato = xmlStato.Value;
                datiCovid.CodiceRegione = Convert.ToInt32(xmlCodiceRegione.Value);
                datiCovid.NomeRegione = xmlNomeRegione.Value;
                datiCovid.CodiceProvincia = xmlCodiceProvincia.Value;
                datiCovid.NomeProvincia = xmlNomeProvincia.Value;
                datiCovid.SiglaProvincia = xmlSiglaProvincia.Value;
                datiCovid.Latitudine = Convert.ToDouble(xmlLatitudine.Value);
                datiCovid.Longitudine = Convert.ToDouble(xmlLongitudine.Value);
                datiCovid.CasiTotali = Convert.ToInt32(xmlCasiTotali.Value);
                datiCovid.NoteIt = xmlNoteIt.Value;
                datiCovid.NoteEn = xmlNoteEn.Value;


                //Faccio visualizzare le proprietà di Alunno nella ListBox
                Dispatcher.Invoke(() => Lst_VediDati1.Items.Add(datiCovid));


                //Controlliamo se il CancellationToken è stato richiesto
                if (cancToken.Token.IsCancellationRequested)
                {
                    //Cosi facendo lasciamo al metodo carica dati di eseguire l'ultima istruzione che sta eseguendo
                    break;
                }


                //Rallento il caricamento usando Thread.Sleep() per visualizzare un alunno alla volta
                Thread.Sleep(1500);

            }


            //Riabillito il Bottone Aggiorna  e disattivo il bottone per l interruzione del caricamento
            Dispatcher.Invoke(() =>
            {
                Btn_CaricaDati.IsEnabled = true;

                Btn_Interrompi.IsEnabled = false;

                //Mettiamo a null il CancellationToken
                cancToken = null;

            });
        }


        //Bottone per interrompere il caricamento
        private void Btn_Interrompi_Click(object sender, RoutedEventArgs e)
        {
            //Interrompo il caricamento
            cancToken.Cancel();
        }


        //Bottone per aggiornare i dati
        private void Btn_AggiornaDati_Click(object sender, RoutedEventArgs e)
        {
            Covid_19 dati = (Covid_19)Lst_VediDati1.SelectedItem; //effettuo il casting sul elemento selezionato

            int valore = Convert.ToInt32(Txt_Positivi.Text); //aggiorniamo il nuovo valore

            if (dati.CasiTotali != valore) //controlliamo se il valore modificato è diverso dall'originale
            {
                dati.CasiTotali = valore;  //assegniamo il nuovo valore
                MessageBox.Show("Operazione eseguita con successo", "OK");
            }

            Task.Factory.StartNew(() => Scrivi());  //aggiunta nuovo valore al file xml
        }



        //Metodo di scrittura di un file xml nuovo con i nuovi valori
        private void Scrivi()
        {
            string path = @"DatiAggiornati.xml";
            XElement xmlRegioni = new XElement("root");

            foreach (Covid_19 item in Lst_VediDati1.Items)
            {
                XElement xmlRegione = new XElement("row");

                XElement xmlData = new XElement("data", item.Data);
                XElement xmlStato = new XElement("stato", item.Stato);
                XElement xmlCodiceRegione = new XElement("codice_regione", item.CodiceRegione);
                XElement xmlNomeRegione = new XElement("denominazione_regione", item.NomeRegione);
                XElement xmlCodiceProvincia = new XElement("codice_provincia", item.CodiceProvincia);
                XElement xmlNomeProvincia = new XElement("denominazione_provincia", item.NomeProvincia);
                XElement xmlSiglaProvincia = new XElement("sigla_provincia", item.SiglaProvincia);
                XElement xmlLatutudine = new XElement("lat", item.Latitudine);
                XElement xmlLongitudine = new XElement("long", item.Longitudine);
                XElement xmlCasiTotali = new XElement("totale_casi", item.CasiTotali);
                XElement xmlNoteIt = new XElement("note_it", item.NoteIt);
                XElement xmlNoteEn = new XElement("note_en", item.NoteEn);


                xmlRegione.Add(xmlData);
                xmlRegione.Add(xmlStato);
                xmlRegione.Add(xmlCodiceRegione);
                xmlRegione.Add(xmlNomeRegione);
                xmlRegione.Add(xmlCodiceProvincia);
                xmlRegione.Add(xmlNomeProvincia);
                xmlRegione.Add(xmlSiglaProvincia);
                xmlRegione.Add(xmlLatutudine);
                xmlRegione.Add(xmlLongitudine);
                xmlRegione.Add(xmlCasiTotali);
                xmlRegione.Add(xmlNoteIt);
                xmlRegione.Add(xmlNoteEn);


                xmlRegioni.Add(xmlRegione);

            }

            //Salvo i dati
            xmlRegioni.Save(path);
        }


        //Evento per cambiare i dati selezionati dalla ListBox
        private void Lst_VediDati1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Covid_19 dati = (Covid_19)Lst_VediDati1.SelectedItem; //effettuo il casting sul elemento selezionato

            if (dati != null)
            {
                Txt_Regione.Text = dati.NomeRegione.ToString(); //faccio visualizzare il dato selezionato in una textbox

                Txt_Positivi.Text = dati.CasiTotali.ToString(); //faccio visualizzare il totale dei contagiato della regione selezionata

                Txt_Provincia.Text = dati.NomeProvincia.ToString(); //faccio visualizzare il nome della regione in una text box
            }
        }

        
    }
}
