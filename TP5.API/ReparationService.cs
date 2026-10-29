using TP5.Shared;
using Microsoft.EntityFrameworkCore;

namespace TP5.API;

public class ReparationService
{
    // historique d'une reparation avec un record
    public record HistoriqueReparationRecord(
        int OrdinateurId,
        string NomOrdinateur,
        string Technicien,
        string TypeReparation,
        DateTime DateIntervention
    );

    // contexte de la requete
    private readonly ParcInformatiqueContext _context;

    private readonly JournalService _journal;

    // verrou pour que les reparations tournent en meme temps et partagent le meme context
    // le verrou fait passer les sauvegardes une par une
    private readonly object _verrouContext = new object();

    public ReparationService(ParcInformatiqueContext context, JournalService journal)
    {
        _context = context;
        _journal = journal;
    }

    // simule une reparation de 2 secondes sur un ordinateur
    public async Task SimulerReparationAsync(Ordinateur ordinateur, string technicien)
    {
        Console.WriteLine($"Debut reparation {ordinateur.Nom}");

        var reparation = new Reparation
        {
            OrdinateurId = ordinateur.Id,
            Technicien = technicien,
            TypeReparation = "Diagnostic",
            DateDebut = DateTime.Now,
            Statut = "En cours"
        };

        // creation de la reparation en base
        // utilise SaveChanges() version non asynchrone
        lock (_verrouContext)
        {
            _context.Reparations.Add(reparation);
            _context.SaveChanges();
        }

        // simule le temps de reparation
        await Task.Delay(2000);

        // fin de la reparation un thread a la fois
        lock (_verrouContext)
        {
            reparation.Statut = "Over";
            _context.SaveChanges();
        }

        _journal.Ecrire(JournalService.LogReparations, $"{ordinateur.Nom} réparé");

        Console.WriteLine($"{ordinateur.Nom} OVer");
    }

    // toutes les reparations avec leur ordinateur
    public async Task<List<Reparation>> GetAllAsync()
    {
        return await _context.Reparations
            .Include(r => r.UnOrdinateur)
            .ToListAsync();
    }

    // historique des reparations
    public async Task<List<HistoriqueReparationRecord>> GetHistoriqueAsync()
    {
        return await _context.Reparations
            .Select(r => new HistoriqueReparationRecord(
                r.OrdinateurId,
                r.UnOrdinateur.Nom,
                r.Technicien,
                r.TypeReparation,
                r.DateDebut))
            .ToListAsync();
    }
}