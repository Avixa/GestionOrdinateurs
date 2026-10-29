using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP5.Shared;

namespace TP5.API;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // acces bdd
    private readonly ParcInformatiqueContext _context;

    public UsersController(ParcInformatiqueContext context)
    {
        _context = context;
    }

    // renvoie tous les users
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    // user par login
    [HttpGet("{login}")]
    public async Task<ActionResult<User>> GetByLogin(string login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

        // 404 si aucun user pour le login
        if (user == null)
        {
            return NotFound($"L'utilisateur {login} n'existe pas");
        }

        return Ok(user);
    }
}