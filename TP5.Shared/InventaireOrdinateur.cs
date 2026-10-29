namespace TP5.Shared;

// collection d'ordinateurs avec deux indexeurs
public class InventaireOrdinateurs
{
    // ordis de l'inventaire
    public List<Ordinateur> Ordinateurs { get; set; } = new();

    // indexeur par id
    public Ordinateur? this[int index]
    {
        get { return Ordinateurs.FirstOrDefault(o => o.Id == index); }
    }

    // indexeur par nom
    public Ordinateur? this[string reference]
    {
        get { return Ordinateurs.FirstOrDefault(o => o.Nom.ToLower() == reference.ToLower()); }
    }
}