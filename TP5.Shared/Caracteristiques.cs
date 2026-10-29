namespace TP5.Shared;

// caracteristiques techniques d'un ordinateur
public record Caracteristiques
{
    public int RamGo { get; set; }
    public TypeOrdinateur TypeOrdi { get; set; }
    public int StockageGo { get; set; }
    public string Processeur { get; set; }

    //constructeur 
    public Caracteristiques(int ramGo, int stockageGo, TypeOrdinateur typeOrdi, string processeur)
    {
        RamGo = ramGo;
        StockageGo = stockageGo;
        TypeOrdi = typeOrdi;
        Processeur = processeur;
    }
}