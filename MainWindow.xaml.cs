using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProgettoWPFReguig
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, List<string>> dbEsercizi = new Dictionary<string, List<string>>()
        {
            { "Petto", new List<string> { "Panca Piana", "Panca Inclinata", "Chest Press", "Croci", "Dips", "Pectoral Machine" } },
            { "Dorso", new List<string> { "Lat Machine", "Rematore", "Trazioni", "Pulley", "Stacco da Terra" } },
            { "Gambe", new List<string> { "Squat", "Leg Press", "Leg Extension", "Leg Curl", "Calf Raise", "Affondi" } },
            { "Spalle", new List<string> { "Military Press", "Alzate Laterali", "Shoulder Press", "Facepull" } },
            { "Braccia", new List<string> { "Curl Bilanciere", "Hammer Curl", "Pushdown Tricipiti", "French Press" } },
            { "Addominali", new List<string> { "Crunch", "Plank", "Leg Raise", "Mountain Climbers" } },
            { "Cardio", new List<string> { "Tapis Roulant", "Cyclette", "Ellittica", "Vogatore" } }
        };

        private List<Attivita> schedaSessione = new List<Attivita>();
        private Attivita esercizioAttivo;

        // Questa PILA (Stack) serve a memorizzare l'ordine degli esercizi inseriti.
        // Segue la logica LIFO (Last In, First Out): l'ultimo inserito è il primo a essere annullato.
        private Stack<Attivita> storicoEsercizi = new Stack<Attivita>();

        public MainWindow()
        {
            InitializeComponent();
            foreach (var gruppo in dbEsercizi.Keys) CmbGruppo.Items.Add(gruppo);
        }

        private void CmbGruppo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbGruppo.SelectedItem == null) return;
            string gruppo = CmbGruppo.SelectedItem.ToString();

            if (gruppo.Equals("Cardio", StringComparison.OrdinalIgnoreCase))
            {
                PanelPesi.Visibility = Visibility.Collapsed;
                PanelCardio.Visibility = Visibility.Visible;
            }
            else
            {
                PanelPesi.Visibility = Visibility.Visible;
                PanelCardio.Visibility = Visibility.Collapsed;
            }

            CmbEsercizi.Items.Clear();
            foreach (var es in dbEsercizi[gruppo].OrderBy(x => x)) CmbEsercizi.Items.Add(es);
        }

        private void BtnAvviaEsercizio_Click(object sender, RoutedEventArgs e)
        {
            if (CmbEsercizi.SelectedItem == null) return;
            string nome = CmbEsercizi.SelectedItem.ToString();
            string gruppo = CmbGruppo.SelectedItem.ToString();

            if (gruppo == "Cardio") esercizioAttivo = new EsercizioCardio(nome, gruppo);
            else esercizioAttivo = new EsercizioForza(nome, gruppo);

            // Questa riga serve a salvare l'esercizio nella PILA prima di aggiungerlo alla lista.
            storicoEsercizi.Push(esercizioAttivo);

            schedaSessione.Add(esercizioAttivo);
            AggiornaUI();
            BrdFeedback.Visibility = Visibility.Collapsed;
        }

        // Questa funzione serve a implementare il tasto "Annulla" usando la Pila.
        private void BtnAnnullaUltimo_Click(object sender, RoutedEventArgs e)
        {
            if (storicoEsercizi.Count > 0)
            {
                // La funzione Pop() toglie l'ultimo elemento dalla Pila.
                Attivita rimosso = storicoEsercizi.Pop();

                // Lo togliamo anche dalla lista della scheda.
                schedaSessione.Remove(rimosso);

                // Impostiamo come attivo l'esercizio precedente (se esiste).
                esercizioAttivo = schedaSessione.LastOrDefault();

                AggiornaUI();
                BrdFeedback.Visibility = Visibility.Collapsed;
                MessageBox.Show($"Esercizio '{rimosso.Nome}' rimosso correttamente.");
            }
            else
            {
                MessageBox.Show("Non ci sono esercizi da annullare!");
            }
        }

        private void BtnAggiungiSerie_Click(object sender, RoutedEventArgs e)
        {
            if (esercizioAttivo == null) return;
            try
            {
                if (esercizioAttivo is EsercizioForza f)
                    f.ElencoSerie.Add(new DatiSerie(double.Parse(TxtPeso.Text), int.Parse(TxtReps.Text)));
                else if (esercizioAttivo is EsercizioCardio c)
                    c.ElencoSerie.Add(new DatiSerie(double.Parse(TxtMinuti.Text), double.Parse(TxtVel.Text), double.Parse(TxtIncl.Text)));

                if (esercizioAttivo is IProgressivo prog)
                {
                    string ob = (CmbObiettivo.SelectedItem as ComboBoxItem).Content.ToString();
                    TxtFeedback.Text = prog.AnalizzaPrestazione(ob);
                    BrdFeedback.Visibility = Visibility.Visible;
                }
                AggiornaUI();
            }
            catch { MessageBox.Show("Inserisci solo numeri!"); }
        }

        private void BtnFineAllenamento_Click(object sender, RoutedEventArgs e)
        {
            if (schedaSessione.Count == 0) return;
            double cal = 0, vol = 0;
            HashSet<string> muscoli = new HashSet<string>();

            foreach (var es in schedaSessione)
            {
                if (es is IProgressivo prog)
                {
                    cal += prog.CalcolaCalorie();
                    muscoli.Add(es.GruppoMuscolare);
                    if (es is EsercizioForza f) vol += f.ElencoSerie.Sum(s => s.Peso * s.Reps);
                }
            }
            MessageBox.Show($"🏆 FINE!\n🔥 Calorie: {cal} kcal\n💪 Carico: {vol} kg\n🎯 Muscoli: {string.Join(", ", muscoli)}");
            schedaSessione.Clear();
            storicoEsercizi.Clear(); // Questa riga serve a svuotare la pila alla fine dell'allenamento.
            AggiornaUI();
        }

        private void AggiornaUI() { LstEsercizi.ItemsSource = null; LstEsercizi.ItemsSource = schedaSessione; }

        private void BtnAnnulla_Click(object sender, RoutedEventArgs e)
        {
            schedaSessione.Clear();
            storicoEsercizi.Clear(); // Svuotiamo la pila se resettiamo tutto.
            AggiornaUI();
        }
    }
}