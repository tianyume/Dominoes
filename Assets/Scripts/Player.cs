using System.Collections.Generic;

public class Player
{
    public LinkedList<Domino> Dominoes { get; private set; }
    public int Score { get; set; }

    public Player()
    {
        Dominoes = new LinkedList<Domino>();
        Score = 0;
    }
}
