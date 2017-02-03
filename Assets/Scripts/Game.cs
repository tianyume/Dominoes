public class Game
{
    public const int MaxScore = 150;

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
        Shuffle();
        Deal();
    }

    private void Shuffle()
    {

    }

    private void Deal()
    {

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
