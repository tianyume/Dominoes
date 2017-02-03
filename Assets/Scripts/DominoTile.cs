using System.Collections.Generic;

public class DominoTile
{
    public List<Domino> Dominoes { get; private set; }

    public DominoTile()
    {
        Dominoes = new List<Domino>(28);
    }
}
