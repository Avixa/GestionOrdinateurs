using TP5.API;
using TP5.Shared;
using Microsoft.EntityFrameworkCore;

namespace TP5.Tests;

public class OrdinateurServiceTests
{
    //creer un contexte avec une base en memoire
    private ParcInformatiqueContext CreerContext()
    {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ParcInformatiqueContext(options);
    }

    [Fact]
    public async Task AjouterOrdinateur()
    {
        var service = new OrdinateurService(CreerContext(), new JournalService());

        //creer notre ordi
        var ordi = new Ordinateur
        {
            Id = 100,
            Nom = "test",
            Marque = "test",
            Etat = EtatOrdinateur.EnService,
            Caracteristiques = new Caracteristiques(8, 256, TypeOrdinateur.Portable, "test")
        };

        //ajout a la base
        await service.AddOrdinateurAsync(ordi);

        //verifie 
        Assert.NotNull(await service.GetOrdinateurByIdAsync(100));
    }



    [Fact]
    public async Task DeleteOrdinateur_Existant()
    {
        var service = new OrdinateurService(CreerContext(), new JournalService());

        var ordi = new Ordinateur
        {
            Id = 200,
            Nom = "test",
            Marque = "test",
            Etat = EtatOrdinateur.EnService,
            Caracteristiques = new Caracteristiques(8, 256, TypeOrdinateur.Fixe, "test")
        };

        await service.AddOrdinateurAsync(ordi);

        var resultat = await service.DeleteOrdinateurByIdAsync(200);

        Assert.True(resultat);
        Assert.Null(await service.GetOrdinateurByIdAsync(200));
    }



    [Fact]
    public async Task DeleteOrdinateur_Inexistant()
    {
        var service = new OrdinateurService(CreerContext(), new JournalService());
        var resultat = await service.DeleteOrdinateurByIdAsync(200);

        Assert.False(resultat);
    }
}