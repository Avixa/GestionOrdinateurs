namespace TP5.API;

// ecris les logs
public class JournalService
{
    // un dossier par type de log
    public const string LogReparations = "Logs/Reparations_Logs/reparations.log";
    public const string LogOrdinateurs = "Logs/Ordinateurs/ordinateurs.log";

    // verrou
    // un seul thread ecrit dans un fichier a la fois
    private static readonly object _verrouFichier = new object();

    public JournalService()
    {
        // 19 on cree les direcrotry de log
        Directory.CreateDirectory("Logs/Reparations_Logs");
        Directory.CreateDirectory("Logs/Ordinateurs");
    }

    // ecrit avec la date dans les logs 
    public void Ecrire(string fichier, string message)
    {
        lock (_verrouFichier)
        {
            // fichier fermer automatiquement a la fin du bloc
            using var writer = new StreamWriter(fichier, append: true);

            // format de date 15/10/2026 - Dell XPS réparé
            writer.WriteLine($"{DateTime.Now:dd/MM/yyyy} - {message}");
        }
    }
}