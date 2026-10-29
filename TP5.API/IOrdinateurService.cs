using TP5.API;
using TP5.Shared;

public interface IOrdinateurService
{
    // event a chaque d'ajout d'ordi
    event EventHandler<OrdinateurAjouteEventArgs> OrdinateurAjoute;

    //retourne les ordis dispos
    public Task<List<Ordinateur>> GetDisponiblesAsync();

    //retourne les ordi d'une marque
    public Task<List<Ordinateur>> GetByMarqueAsync(string marque);

    // retourne les ordinateurs du plus cher au moins cher
    public Task<List<Ordinateur>> GetByPrixDecroissantAsync();



    // retourne tous les ordinateurs
    public Task<List<Ordinateur>> GetAllAsync();

    //retourne l'ordinateur qui possede l'id demande
    public Task<Ordinateur?> GetOrdinateurByIdAsync(int id);

    // ajoute un ordinateur a notre liste
    public Task AddOrdinateurAsync(Ordinateur ordi);

    //modifie un ordinateur 
    public Task<bool> UpdateOrdinateurByIdAsync(int id, Ordinateur modif);

    // suppression d'un ordinateur '
    public Task<bool> DeleteOrdinateurByIdAsync(int id);


    // diagnostic d'un ordinateur selon son etat et sa RAM
    public string Diagnostiquer(Ordinateur ordinateur);

    // statistiques du parc
    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques();
}