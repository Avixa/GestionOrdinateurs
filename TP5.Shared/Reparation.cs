namespace TP5.Shared;

public class Reparation
{
    public int Id { get; set; }

    // id de l'ordinateur repare
    public int OrdinateurId { get; set; }

    // l'ordinateur repare
    public Ordinateur UnOrdinateur { get; set; }

    public string TypeReparation { get; set; }

    public DateTime DateDebut { get; set; }

    public string Statut { get; set; }

    // nom du technicien qui repare
    public string Technicien { get; set; }
}