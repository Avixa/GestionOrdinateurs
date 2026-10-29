using TP5.API;
using TP5.Shared;
using Microsoft.EntityFrameworkCore;

namespace TP5.Tests;

public class ReparationServiceTests
{
    private ParcInformatiqueContext CreerContext()
    {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ParcInformatiqueContext(options);
    }

    [Fact]
    public async Task CreerReparation()
    {
        
        var context = CreerContext();
        var service = new ReparationService(context, new JournalService());

        // ordi a reparer 
        var ordi = new Ordinateur
        {
            Id = 300,
            Nom = "test",
            Marque = "test",
            Etat = EtatOrdinateur.EnPanne,
            Caracteristiques = new Caracteristiques(8, 256, TypeOrdinateur.Portable, "test")
        };
        context.Ordinateurs.Add(ordi);
        await context.SaveChangesAsync();

        // creer reparation
        await service.SimulerReparationAsync(ordi, "Alice");

        var reparation = await context.Reparations.FirstOrDefaultAsync(r => r.OrdinateurId == 300);
        Assert.NotNull(reparation);
        Assert.Equal("Alice", reparation.Technicien);
        Assert.Equal("Over", reparation.Statut);
    }
}