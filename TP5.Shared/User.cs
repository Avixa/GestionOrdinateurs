using System.ComponentModel.DataAnnotations;

namespace TP5.Shared;

public class User
{
    //id de l'user
    public int Id { get; set; }

    [Required]
    public string Login { get; set; } = string.Empty;

    // le role de l'user 
    public RoleEnum Role { get; set; }
}