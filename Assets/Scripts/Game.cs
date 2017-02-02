public class Game
{
    public const int MaxScore = 150;

    public bool IsGameEnded { get; private set; }
    public GameRole CurrentGameRole { get; private set; }

    private CardTile boneyard = new CardTile();
    private Player player1 = new Player();
    private Player player2 = new Player();
    private History history = new History();

    public Game()
    {
        IsGameEnded = false;
        CurrentGameRole = GameRole.Player1;
        CardTileShuffle();
        DistributeCard();
    }

    private void CardTileShuffle()
    {

    }

    private void DistributeCard()
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
