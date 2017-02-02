using System.Collections.Generic;

public class History
{
    public Domino CenterDomino { get; set; }
    public LinkedList<Domino> HorizontalDominoes { get; private set; }
    public LinkedList<Domino> VerticalDominoes { get; private set; }

    public History()
    {
        CenterDomino = null;
        HorizontalDominoes = new LinkedList<Domino>();
        VerticalDominoes = new LinkedList<Domino>();
    }
}