namespace TP5.API;

public class StockService
{
    // stock de SSD pour tous les threads
    public int StockSSD = 10;

    // verrou pour qu'un seul thread modifie le stock en meme temps
    private readonly object _verrouStock = new object();

    // retire un SSD du stock
    // avec l'etape 17 on a un resultat incohérent on obtient plus que 0 car les threads
    // lisent le stock en meme temps 

    //verrou ncessaire donc
    public void ConsommerSSD()
    {
        // un seul thread en meme temps dans ce bloc, les autres attendent
        lock (_verrouStock)
        {
            var stock = StockSSD;
            Task.Delay(10).Wait();
            StockSSD = stock - 1;
        }
    }
}