using System;
using System.Linq;

namespace ProgettoWPFReguig
{
    public class EsercizioForza : Attivita, IProgressivo
    {
        public EsercizioForza(string nome, string gruppo) : base(nome, gruppo) { }

        // Questa funzione serve a mostrare i chili e i set nella lista
        public override string GetRiepilogo() => ElencoSerie.Count == 0 ? "Inizia!" : $"Set: {ElencoSerie.Count} | Max: {CalcolaMassimale()}kg";

        public double CalcolaMassimale()
        {
            if (ElencoSerie.Count == 0) return 0;
            var ultima = ElencoSerie.Last();
            return Math.Round(ultima.Peso * (1 + (double)ultima.Reps / 30.0), 1);
        }

        public string AnalizzaPrestazione(string ob)
        {
            if (ElencoSerie.Count == 0) return "";
            int r = ElencoSerie.Last().Reps;
            if (ob == "Forza" && r > 6) return "⚠️ Troppe reps per Forza! (Usa 1-5)";
            if (ob == "Ipertrofia" && (r < 8 || r > 12)) return "⚠️ Range Ipertrofia: 8-12 reps";
            return "✅ Range perfetto per l'obiettivo!";
        }

        public double CalcolaCalorie() => Math.Round(ElencoSerie.Sum(s => s.Peso * s.Reps) * 0.05, 1);
    }
}