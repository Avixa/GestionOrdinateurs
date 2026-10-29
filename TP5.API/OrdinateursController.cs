using TP5.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TP5.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdinateursController : ControllerBase
{
    //immutable 
    private readonly IOrdinateurService _service;

    //les logs console
    private readonly ILogger<OrdinateursController> _logger;

    //lecture des param json
    private readonly IConfiguration _config;
    public OrdinateursController(IOrdinateurService service, ILogger<OrdinateursController> logger, IConfiguration config)
    {
        _service = service;
        _logger = logger ;
        _config = config ; 

        _service.OrdinateurAjoute += AfficherMessage;
    }


    //get les ordis dispos
    [HttpGet("disponibles")]
    public async Task<IActionResult> GetOrdinateurDisponibles()
    {
        _logger.LogInformation("récupération des ordinateurs dispo");
        return Ok (await _service.GetDisponiblesAsync());
    }

    //get les ordinateurs du plus cher au moins cher
    [HttpGet("prix")]
    public async Task<IActionResult> GetOrdinateursByPrix()
    {
        _logger.LogInformation("récupération des ordinateurs triés par prix");
        return Ok(await _service.GetByPrixDecroissantAsync());
    }


    //get les ordinateurs 
    [HttpGet]
    public async Task<IActionResult> GetAllOrdinateur()
    {
        _logger.LogInformation("récupération des ordinateurs");

        return Ok(await _service.GetAllAsync());
    }


    //infos d'un ordinateur en fonction de l'id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrdinateurById(int id)
    {
        _logger.LogInformation("récupération de l'ordinateur {Id}", id);

        var ordinateur = await _service.GetOrdinateurByIdAsync(id);

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }


    //infos de l'appli
    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        _logger.LogInformation("récupération des infos de l'appli");

        var nom = _config["Application:Name"];
        var version = _config["Application:Version"];

        return Ok(new { Nom = nom, Version = version });
    }


    // ajoute un ordinateur a la base
    [HttpPost]
    public async Task<IActionResult> AddOrdinateur(Ordinateur ordinateur)
    {
        _logger.LogInformation("ajout de l'ordinateur {Ordinateur}", ordinateur);

        await _service.AddOrdinateurAsync(ordinateur);

        return CreatedAtAction(nameof(GetOrdinateurById), new { id = ordinateur.Id }, ordinateur);
    }


    // update basé sur l'id
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrdinateur(int id, Ordinateur ordinateur)
    {
        _logger.LogInformation("update des informations de l'ordinateur {Id}", id);
        if (!await _service.UpdateOrdinateurByIdAsync(id, ordinateur))
        {
            return NotFound();
        }

        return Ok(await _service.GetOrdinateurByIdAsync(id));
    }


    // deleted basé sur l'id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrdinateur(int id)
    {
        _logger.LogInformation("suppression de l'ordinateur {Id}", id);
        if (!await _service.DeleteOrdinateurByIdAsync(id))
        {
            return NotFound();
        }
        return NoContent();

    }


    //get les ordinateurs d'une marque
    [HttpGet("marque/{marque}")]
    public async Task<IActionResult> GetOrdinateursByMarque(string marque)
    {
        _logger.LogInformation("recherche des ordinateurs de la marque {Marque}", marque);
        return Ok(await _service.GetByMarqueAsync(marque));
    }


    //pour les abonnés 

    // écoute l'event et affiche le message quand trigger 
    private void AfficherMessage (object? sender, OrdinateurAjouteEventArgs e)
    {
        Console.WriteLine("Ordinateur ajouté");
    }

    // diagnostic d'un ordinateur selon son etat et sa RAM
    [HttpGet("{id}/diagnostic")]
    public async Task<IActionResult> GetDiagnostic(int id)
    {
        _logger.LogInformation("diagnostic de l'ordinateur {Id}", id);

        var ordinateur = await _service.GetOrdinateurByIdAsync(id);

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(_service.Diagnostiquer(ordinateur));
    }

    // statistiques du parc
    [HttpGet("statistiques")]
    public IActionResult GetStatistiques()
    {
        _logger.LogInformation("calcul des statistiques");

        // deconstruction du tuple
        var (total, enStock, valeurStock, nbReparations) = _service.CalculerStatistiques();

        return Ok(new { total, enStock, valeurStock, nbReparations });
    }



    // cherche un ordinateur par son nom 
    [HttpGet("inventaire/{nom}")]
    public async Task<IActionResult> Chercher(string nom)
    {
        _logger.LogInformation("recherche de l'ordinateur {Nom} dans l'inventaire", nom);

        var inventaire = new InventaireOrdinateurs();
        inventaire.Ordinateurs = await _service.GetAllAsync();

        var ordinateur = inventaire[nom]; 

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }

    // cherche un ordinateur par son id
    [HttpGet("inventaire/id/{id}")]
    public async Task<IActionResult> ChercherParId(int id)
    {
        _logger.LogInformation("recherche de l'ordinateur {Id} dans l'inventaire", id);

        var inventaire = new InventaireOrdinateurs();
        inventaire.Ordinateurs = await _service.GetAllAsync();

        var ordinateur = inventaire[id];

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }
}