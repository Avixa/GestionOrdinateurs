using TP5.Shared;
using Microsoft.EntityFrameworkCore;

namespace TP5.API;

public class OrdinateurService : IOrdinateurService
{
    //acces bdd
    private readonly ParcInformatiqueContext _context;

    // logs
    private readonly JournalService _journal;

    //event ajout de chaque ordinateur
    public event EventHandler<OrdinateurAjouteEventArgs?> OrdinateurAjoute;

    public OrdinateurService(ParcInformatiqueContext context, JournalService journal)
    {
        _context = context;
        _journal = journal;
    }

    //return les ordis d'une meme marque
    public async Task<List<Ordinateur>> GetByMarqueAsync(string marque)
    {
        return await _context.Ordinateurs
            .Where(o => o.Marque.ToLower() == marque.ToLower())
            .ToListAsync();
    }

    //return les ordinateurs tries de maniere decroissante
    public async Task<List<Ordinateur>> GetByPrixDecroissantAsync()
    {
        var ordinateurs = await _context.Ordinateurs.ToListAsync();

        return ordinateurs
            .OrderByDescending(o => o.Prix)
            .ToList();
    }

    //return les ordis qui ont un stock > 0
    public async Task<List<Ordinateur>> GetDisponiblesAsync()
    {
        return await _context.Ordinateurs
            .Where(o => o.Stock > 0)
            .ToListAsync();
    }

    //return tous les ordis de la base
    public async Task<List<Ordinateur>> GetAllAsync()
    {
        return await _context.Ordinateurs.ToListAsync();
    }

    // return l'ordi de l'id
    public async Task<Ordinateur?> GetOrdinateurByIdAsync(int id)
    {
        return await _context.Ordinateurs.FirstOrDefaultAsync(o => o.Id == id);
    }

    //ajoute un ordi dans la bdd et trigger l'event
    public async Task AddOrdinateurAsync(Ordinateur ordinateur)
    {
        _context.Ordinateurs.Add(ordinateur);
        await _context.SaveChangesAsync();

        _journal.Ecrire(JournalService.LogOrdinateurs, $"{ordinateur.Nom} ajouté");

        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs(ordinateur));
    }

    //modifie les settings d'un ordi
    public async Task<bool> UpdateOrdinateurByIdAsync(int id, Ordinateur modifie)
    {
        var existant = await GetOrdinateurByIdAsync(id);
        if (existant == null)
        {
            return false;
        }

        existant.Nom = modifie.Nom;
        existant.Marque = modifie.Marque;
        existant.Caracteristiques = modifie.Caracteristiques;
        existant.SetState(modifie.Etat);
        existant.Stock = modifie.Stock;
        existant.Prix = modifie.Prix;

        await _context.SaveChangesAsync();

        _journal.Ecrire(JournalService.LogOrdinateurs, $"{existant.Nom} modifié");

        return true;
    }

    //supprime un ordi
    public async Task<bool> DeleteOrdinateurByIdAsync(int id)
    {
        var existant = await GetOrdinateurByIdAsync(id);

        if (existant == null)
        {
            return false;
        }

        _context.Ordinateurs.Remove(existant);
        await _context.SaveChangesAsync();

        _journal.Ecrire(JournalService.LogOrdinateurs, $"{existant.Nom} supprimé");

        return true;
    }

    // diagnostic d'un ordi
    public string Diagnostiquer(Ordinateur ordinateur)
    {
        return ordinateur switch
        {
            {
                Etat: EtatOrdinateur.HorsService
            }
                => "Remplacement",

            {
                Etat: EtatOrdinateur.Lent,
                Caracteristiques:
                {
                    RamGo: < 8
                }
            }
                => "Ajouter RAM",
            _
                => "Aucune action"
        };
    }

    // statistiques du parc dans un tuple 
    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations)
        CalculerStatistiques()
    {
        var ordinateurs = _context.Ordinateurs.ToList();

        // nb ordinateurs en base
        int total = ordinateurs.Count;

        // ordinateurs qui ont du stock
        int disponibles = ordinateurs.Count(o => o.Stock > 0);

        // valeur du stock 
        decimal valeurStock = ordinateurs.Sum(o => o.Prix * o.Stock);

        // nombre de reparations en base
        int nbReparations = _context.Reparations.Count();

        return (total, disponibles, valeurStock, nbReparations);
    }
}