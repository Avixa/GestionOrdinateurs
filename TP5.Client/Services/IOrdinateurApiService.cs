using TP5.Shared;

namespace TP5.Client.Services;

public interface IOrdinateurApiService
{
    Task<List<Ordinateur>?> GetOrdinateursAsync();
}