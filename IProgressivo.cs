namespace ProgettoWPFReguig
{
    public interface IProgressivo
    {
        // Funzione per dare consigli all'utente
        string AnalizzaPrestazione(string obiettivoUtente);

        // Funzione per il valore di picco (Massimale o Tempo totale)
        double CalcolaMassimale();

        // Funzione per il calcolo energetico
        double CalcolaCalorie();
    }
}