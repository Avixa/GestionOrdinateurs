using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TP5.API;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ReparationsController : ControllerBase
{
    // service des reparations
    private readonly ReparationService _reparService;

    // pour get les ordinateurs
    private readonly IOrdinateurService _ordinateurService;

    // stock de pieces
    private readonly StockService _stockService;

    //les logs console
    private readonly ILogger<ReparationsController> _logger;

    public ReparationsController(ReparationService reparService, IOrdinateurService ordinateurService,
        StockService stockService, ILogger<ReparationsController> logger)
    {
        _reparService = reparService;
        _ordinateurService = ordinateurService;
        _stockService = stockService;
        _logger = logger;
    }

    // lance 3 reparations en meme temps
    [HttpPost("SimulerReparation")]
    public async Task<IActionResult> SimulerReparation()
    {
        _logger.LogInformation("simulation de 3 réparations");

        //recuperation de la liste d'ordinateurs.
        var listOrdinateurs = await _ordinateurService.GetAllAsync();

        // il faut au moins 3 ordinateurs pour la simulation
        if (listOrdinateurs.Count < 3)
        {
            return BadRequest("Il faut au moins 3 ordinateurs en base");
        }

        // un ajout pour verifier que les reparations se font en meme temps
        var chrono = Stopwatch.StartNew();

        // lance les 3 reparations (un technicien chacune) et attend qu'elles soient finies
        await Task.WhenAll(
            _reparService.SimulerReparationAsync(listOrdinateurs[0], "Alice"),
            _reparService.SimulerReparationAsync(listOrdinateurs[1], "Bruno"),
            _reparService.SimulerReparationAsync(listOrdinateurs[2], "Chloé")
        );

        return Ok($"3 réparations terminées en {chrono.ElapsedMilliseconds} ms");
    }

    // liste des reparations avec leur ordinateur
    [HttpGet]
    public async Task<IActionResult> GetAllReparations()
    {
        _logger.LogInformation("récupération des réparations");
        return Ok(await _reparService.GetAllAsync());
    }

    // historique des reparations sous forme de HistoriqueReparationRecord
    [HttpGet("historique")]
    public async Task<IActionResult> GetHistorique()
    {
        _logger.LogInformation("récupération de l'historique des réparations");
        return Ok(await _reparService.GetHistoriqueAsync());
    }

    // 10 taches en meme temps consomment toutes un SSD
    [HttpGet("GetStockSSD")]
    public IActionResult GetStockSSD()
    {
        Parallel.For(0, 10, i => _stockService.ConsommerSSD());

        _logger.LogInformation("stock SSD restant : {Stock}", _stockService.StockSSD);
        return Ok(_stockService.StockSSD);
    }
}