using TileExtension;
using System;

public class Game
{
    public const int MaxScore = 150;
    public const int TILE = 6;

    public bool IsGameEnded { get; private set; }
    public GameRole CurrentGameRole { get; private set; }

    private DominoTile boneyard = new DominoTile();
    private Player player1 = new Player();
    private Player player2 = new Player();
    private History history = new History();

    public Game()
    {
        IsGameEnded = false;
        CurrentGameRole = GameRole.Player1;
        Init();
        Shuffle();
        Deal(7, GameRole.Player1);
        Deal(7, GameRole.Player2);
    }

    private void Init()
    {
        for (int i = 0; i <= TILE; i++)
        {
            for (int j = i; j <= TILE; j++)
            {
                Domino temp = new Domino(i, j, false, DominoDirection.NotSpecified, GameRole.BoneYard);
                boneyard.Dominoes.Add(temp);
            }
        }
    }
    
    private void Shuffle()
    {
        boneyard.Dominoes.Shuffle();        
    }

    private void Deal(int numbertiles, GameRole role)
    {
        int n = boneyard.Dominoes.Count;
        for (int i = 0; i < numbertiles; i++)
        {
            //boneyard.Dominoes.Shuffle();
            Domino temp = boneyard.Dominoes[n - 1];
            boneyard.Dominoes.RemoveAt(n - 1);
            player1.Dominoes.AddFirst(temp);

            if (role == GameRole.Player1)
            {
                player1.Dominoes.AddFirst(temp);
            }
            if (role == GameRole.Player2)
            {
                player2.Dominoes.AddFirst(temp);
            }            
        }
        
    }

    public bool PlayDomino(GameRole gameRole, Domino domino)
    {
        if (!gameRole.Equals(CurrentGameRole))
        {
            return false;
        }
        // TODO
        return true;
    }

    public void Reset()
    {

    }
}
