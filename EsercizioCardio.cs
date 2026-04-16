using System;
using System.Linq;

namespace ProgettoWPFReguig
{
    public class EsercizioCardio : Attivita, IProgressivo
    {
        public EsercizioCardio(string nome, string gruppo) : base(nome, gruppo) { }

        public override string GetRiepilogo() => ElencoSerie.Count == 0 ? "0 min" : $"{ElencoSerie.Sum(s => s.Peso)} min | Cardio";

        public double CalcolaMassimale() => ElencoSerie.Sum(s => s.Peso);

        public string AnalizzaPrestazione(string ob) => CalcolaMassimale() < 20 ? "🏃 Sessione breve, punta a 20+ min!" : "⚡ Ottimo lavoro cardio!";

        public double CalcolaCalorie() => ElencoSerie.Sum(s => Math.Round(s.Peso * s.Velocita * (1 + s.Inclinazione / 10) * 0.1, 1));
    }
}