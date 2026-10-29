using TP5.Shared;

namespace TP5.API;

//infos envoyés aux abonnés quand on ajoute un ordinateur
public class OrdinateurAjouteEventArgs : EventArgs
{
    //l'ordi ajouté
    public Ordinateur Ordinateur {get;}

    public OrdinateurAjouteEventArgs (Ordinateur ordinateur)
    {
        Ordinateur = ordinateur;
    }


}