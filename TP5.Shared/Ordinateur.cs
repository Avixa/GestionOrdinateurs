using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TP5.Shared;

[Table("Ordinateur")]
public class Ordinateur
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Marque { get; set; }

    // état de l'ordinateur (enum dans EtatOrdinateur.cs)
    public EtatOrdinateur Etat { get; set; }
    
    //valeur par defaut du stock de 0
    [DefaultValue(0)]
    public int Stock { get; set; }

    //valeur par defaut du prix de 0
    [DefaultValue(0)]
    public decimal Prix { get; set; }

    //indique si l'ordinateur est vendu
    public bool Vendu { get; set; } = false;

    // toutes les caracteristiques technique d'un ordi
    public Caracteristiques Caracteristiques { get; set; }

    //reparation de l'ordinateur
    [JsonIgnore]
    public List<Reparation> Reparations { get; set; } = new();

    // affiche les infos de l'ordinateur
    public string GetInfos()
    {
        return $"#{Id} {Nom} {Marque} {Caracteristiques} {Etat}";
    }

    // pour changer l'etat d'un ordinateur
    public void SetState(EtatOrdinateur nvEtat)
    {
        Etat = nvEtat;
    }
}