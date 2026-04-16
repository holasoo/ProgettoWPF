using System;
using System.Collections.Generic;

namespace ProgettoWPFReguig
{
    // Questa classe serve a contenere TUTTI i possibili dati di una serie o sessione
    public class DatiSerie
    {
        public double Peso { get; set; }        // Kg (Pesi) o Minuti (Cardio)
        public int Reps { get; set; }           // Ripetizioni (Pesi)
        public double Velocita { get; set; }    // Km/h (Cardio)
        public double Inclinazione { get; set; } // % Pendenza (Cardio)

        // Costruttore per i Pesi (Peso e Ripetizioni)
        public DatiSerie(double p, int r) { Peso = p; Reps = r; }

        // Costruttore per il Cardio (Minuti, Velocità, Inclinazione)
        public DatiSerie(double min, double vel, double incl)
        {
            Peso = min; // Salviamo i minuti nel campo Peso
            Velocita = vel;
            Inclinazione = incl;
        }
    }

    public abstract class Attivita
    {
        public string Nome { get; set; }
        public string GruppoMuscolare { get; set; }

        // REQUISITO COLLECTIONS: Lista interna che salva ogni serie fatta
        public List<DatiSerie> ElencoSerie { get; set; } = new List<DatiSerie>();

        public Attivita(string nome, string gruppo)
        {
            Nome = nome;
            GruppoMuscolare = gruppo;
        }

        public abstract string GetRiepilogo();
        public override string ToString() => GetRiepilogo();
    }
}