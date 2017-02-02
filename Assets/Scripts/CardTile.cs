using System.Collections.Generic;

public class CardTile
{
    public List<Domino> Dominoes { get; private set; }

    public CardTile()
    {
        Dominoes = new List<Domino>(28);
    }
}
