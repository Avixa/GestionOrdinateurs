namespace TP5.Client.Services;

public interface IReparationApiService
{
    Task<List<HistoriqueReparationRecord>?> GetHistoriqueAsync();
}