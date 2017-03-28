using TileExtension;
using System;
using System.Collections.Generic;

public class Game
{
    public const int MaxScore = 150;
	private const int MAXNUM = 6;
	private const int TILE = 28;

    public bool IsGameEnded { get; private set; }

    public GameRole CurrentGameRole { get; private set; }
	public DominoTile Tile { get; private set; }
	public History History { get; private set; }

    private Player player1 = new Player();
    private Player player2 = new Player();

    public Game()
    {
        IsGameEnded = false;
        CurrentGameRole = GameRole.Player1;
		History = new History();

        Init();
        Shuffle();
        Deal(7, GameRole.Player1);
        Deal(7, GameRole.Player2);
    }

    private void Init()
    {
        for (int i = 0; i <= MAXNUM; i++)
        {
            for (int j = i; j <= MAXNUM; j++)
            {
                Domino temp = new Domino(i, j);
				Tile.Dominoes.Add(temp);
            }
        }
    }
    
    private void Shuffle()
    {
		Tile.Dominoes.Shuffle();
    }

    private void Deal(int numbertiles, GameRole role)
    {
		int n = TILE;
        for (int i = 0; i < numbertiles; i++)
        {
            Domino temp = null;
			for(int j = 0; j < n; j++){
				if (Tile.Dominoes[j].Ownership == GameRole.BoneYard) {
					temp = Tile.Dominoes[j];
				}
			}
            if (role == GameRole.Player1)
            {
				temp.Ownership = GameRole.Player1;
				player1.Dominoes.AddLast(temp);
            }
            if (role == GameRole.Player2)
            {
				temp.Ownership = GameRole.Player2;
                player2.Dominoes.AddLast(temp);
            }
			// Shuffle every time or not ??
			Shuffle();
        }        
    }

	public LinkedList<Domino> GetMovableDominoes(GameRole role)
	{
		if (role != CurrentGameRole)
		{
			return null;
		}
		LinkedList<Domino> dominoes = new LinkedList<Domino>();
		Player currentPlayer = GetCurrentPlayer();
        foreach (Domino domino in currentPlayer.Dominoes)
        {
			if (History.HorizontalDominoes.Count == 0)
			{
				dominoes.AddLast(domino);
				continue;
			}
			Domino firstHorizontalDomino = History.HorizontalDominoes.First.Value;
			if (firstHorizontalDomino.Placement.LeftValue == domino.Value1 || firstHorizontalDomino.Placement.LeftValue == domino.Value2)
			{
				dominoes.AddLast(domino);
				continue;
			}
			Domino lastHorizontalDomino = History.HorizontalDominoes.Last.Value;
			if (lastHorizontalDomino.Placement.RightValue == domino.Value1 || lastHorizontalDomino.Placement.RightValue == domino.Value2)
			{
				dominoes.AddLast(domino);
				continue;
			}
			if (History.VerticalDominoes.Count == 0)
			{
				continue;
			}
			Domino firstVerticalDomino = History.VerticalDominoes.First.Value;
			if (firstVerticalDomino.Placement.UpValue == domino.Value1 || firstVerticalDomino.Placement.UpValue == domino.Value2)
			{
				dominoes.AddLast(domino);
				continue;
			}
			Domino lastVerticalDomino = History.VerticalDominoes.Last.Value;
			if (lastVerticalDomino.Placement.DownValue == domino.Value1 || lastVerticalDomino.Placement.DownValue == domino.Value2)
			{
				dominoes.AddLast(domino);
				continue;
			}
		}
		return dominoes;
	}

	public bool DrawCardFromTile(GameRole role, Domino domino)
	{
		if (role != CurrentGameRole || domino.Ownership != GameRole.BoneYard)
		{
			return false;
		}
		Player currentPlayer = GetCurrentPlayer();
		domino.Ownership = role;
		currentPlayer.Dominoes.AddLast(domino);
		return true;
	}

	public bool PlayDomino(GameRole role, Domino dominoInHand, Domino dominoOnBoard)
    {
		if (IsGameEnded || !role.Equals(CurrentGameRole) || !IsDominoInHand(role, dominoInHand)) {
            return false;
        }
		Player currentPlayer = GetCurrentPlayer();
		if (dominoOnBoard == null && History.HorizontalDominoes.Count == 0) {
			RemoveDominoInHand(currentPlayer, dominoInHand);
			History.HorizontalDominoes.AddLast(dominoInHand);
			if (dominoInHand.Value1 == dominoInHand.Value2)
			{
				dominoInHand.Placement.Direction = DominoDirection.Vertical;
				dominoInHand.Placement.UpValue = dominoInHand.Value1;
				dominoInHand.Placement.DownValue = dominoInHand.Value2;
				History.Spinner = dominoInHand;
				History.VerticalDominoes.AddLast(dominoInHand);
			}
			else
			{
				dominoInHand.Placement.Direction = DominoDirection.Horizontal;
				dominoInHand.Placement.LeftValue = dominoInHand.Value1;
				dominoInHand.Placement.RightValue = dominoInHand.Value2;
			}
			ScoreByPlaying(currentPlayer);
			NextTurn();
			return true;
		}

		if (dominoOnBoard.Equals(History.HorizontalDominoes.First))
		{
			if (dominoInHand.Value1 == dominoInHand.Value2)
			{
				if (dominoInHand.Value1 == dominoOnBoard.Placement.LeftValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddFirst(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Vertical;
					dominoInHand.Placement.UpValue = dominoInHand.Value1;
					dominoInHand.Placement.DownValue = dominoInHand.Value2;
					if (History.Spinner == null)
					{
						History.Spinner = dominoInHand;
						History.VerticalDominoes.AddLast(dominoInHand);
					}
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				return false;
			}
			else
			{
				if (dominoInHand.Value1 == dominoOnBoard.Placement.LeftValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddFirst(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Horizontal;
					dominoInHand.Placement.LeftValue = dominoInHand.Value2;
					dominoInHand.Placement.RightValue = dominoInHand.Value1;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				else if (dominoInHand.Value2 == dominoOnBoard.Placement.LeftValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddFirst(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Horizontal;
					dominoInHand.Placement.LeftValue = dominoInHand.Value1;
					dominoInHand.Placement.RightValue = dominoInHand.Value2;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				return false;
			}
		}
		else if (dominoOnBoard.Equals(History.HorizontalDominoes.Last))
		{
			if (dominoInHand.Value1 == dominoInHand.Value2)
			{
				if (dominoInHand.Value1 == dominoOnBoard.Placement.RightValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddLast(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Vertical;
					dominoInHand.Placement.UpValue = dominoInHand.Value1;
					dominoInHand.Placement.DownValue = dominoInHand.Value2;
					if (History.Spinner == null)
					{
						History.Spinner = dominoInHand;
						History.VerticalDominoes.AddLast(dominoInHand);
					}
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				return false;
			}
			else
			{
				if (dominoInHand.Value1 == dominoOnBoard.Placement.RightValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddLast(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Horizontal;
					dominoInHand.Placement.LeftValue = dominoInHand.Value1;
					dominoInHand.Placement.RightValue = dominoInHand.Value2;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				else if (dominoInHand.Value2 == dominoOnBoard.Placement.RightValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddLast(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Horizontal;
					dominoInHand.Placement.LeftValue = dominoInHand.Value2;
					dominoInHand.Placement.RightValue = dominoInHand.Value1;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				return false;
			}
		}
		else if (dominoOnBoard.Equals(History.VerticalDominoes.First))
		{
			if (dominoInHand.Value1 == dominoInHand.Value2)
			{
				if (dominoInHand.Value1 == dominoOnBoard.Placement.UpValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddFirst(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Horizontal;
					dominoInHand.Placement.LeftValue = dominoInHand.Value1;
					dominoInHand.Placement.RightValue = dominoInHand.Value2;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				return false;
			}
			else
			{
				if (dominoInHand.Value1 == dominoOnBoard.Placement.UpValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddFirst(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Vertical;
					dominoInHand.Placement.UpValue = dominoInHand.Value2;
					dominoInHand.Placement.DownValue = dominoInHand.Value1;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				else if (dominoInHand.Value2 == dominoOnBoard.Placement.UpValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddFirst(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Vertical;
					dominoInHand.Placement.UpValue = dominoInHand.Value1;
					dominoInHand.Placement.DownValue = dominoInHand.Value2;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				return false;
			}
		}
		else if (dominoOnBoard.Equals(History.VerticalDominoes.Last))
		{
			if (dominoInHand.Value1 == dominoInHand.Value2)
			{
				if (dominoInHand.Value1 == dominoOnBoard.Placement.DownValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddLast(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Horizontal;
					dominoInHand.Placement.LeftValue = dominoInHand.Value1;
					dominoInHand.Placement.RightValue = dominoInHand.Value2;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				return false;
			}
			else
			{
				if (dominoInHand.Value1 == dominoOnBoard.Placement.DownValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddLast(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Vertical;
					dominoInHand.Placement.UpValue = dominoInHand.Value1;
					dominoInHand.Placement.DownValue = dominoInHand.Value2;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				else if (dominoInHand.Value2 == dominoOnBoard.Placement.DownValue)
				{
					RemoveDominoInHand(currentPlayer, dominoInHand);
					History.HorizontalDominoes.AddLast(dominoInHand);
					dominoInHand.Placement.Direction = DominoDirection.Vertical;
					dominoInHand.Placement.UpValue = dominoInHand.Value2;
					dominoInHand.Placement.DownValue = dominoInHand.Value1;
					ScoreByPlaying(currentPlayer);
					if (currentPlayer.Dominoes.Count == 0)
					{
						ScoreByEndingHand(currentPlayer);
					}
					NextTurn();
					return true;
				}
				return false;
			}
		}
		return false;
    }

    public void Reset()
    {
		for (int i = 0; i < TILE; i++)
		{
			Tile.Dominoes[i].Ownership = GameRole.BoneYard;			
		}
		player1.Dominoes.Clear();
		player2.Dominoes.Clear();
		// clear history
		History.HorizontalDominoes.Clear();
		History.VerticalDominoes.Clear();
	}

	private Player GetCurrentPlayer()
	{
		if (CurrentGameRole == GameRole.Player1)
		{
			return player1;
		}
		else if (CurrentGameRole == GameRole.Player2)
		{
			return player2;
		}
		return null;
	}

	private bool IsDominoInHand(GameRole role, Domino domino)
	{
		if (role == GameRole.Player1)
		{
			return player1.Dominoes.Contains(domino);
		}
		else if (role == GameRole.Player2)
		{
			return player2.Dominoes.Contains(domino);
		}
		return false;
	}

	private void RemoveDominoInHand(Player player, Domino domino)
	{
		player.Dominoes.Remove(domino);
	}

	private void ScoreByPlaying(Player player)
	{
		int score = 0;
		if (History.HorizontalDominoes.Count == 1)
		{
			Domino domino = History.HorizontalDominoes.First.Value;
			score += domino.Value1 + domino.Value2;
			if (score % 5 == 0)
			{
				AddScore(player, score);
			}
			return;
		}

		Domino firstHorizontalDomino = History.HorizontalDominoes.First.Value;
		if (firstHorizontalDomino.Placement.Direction == DominoDirection.Vertical)
		{
			score += firstHorizontalDomino.Placement.UpValue + firstHorizontalDomino.Placement.DownValue;
		}
		else if (firstHorizontalDomino.Placement.Direction == DominoDirection.Vertical)
		{
			score += firstHorizontalDomino.Placement.LeftValue;
		}
		else
		{
			throw new Exception("Error: Domino not specified");
		}
		Domino lastHorizontalDomino = History.HorizontalDominoes.Last.Value;
		if (lastHorizontalDomino.Placement.Direction == DominoDirection.Vertical)
		{
			score += lastHorizontalDomino.Placement.UpValue + lastHorizontalDomino.Placement.DownValue;
		}
		else if (lastHorizontalDomino.Placement.Direction == DominoDirection.Vertical)
		{
			score += lastHorizontalDomino.Placement.RightValue;
		}
		else
		{
			throw new Exception("Error: Domino not specified");
		}

		if (History.VerticalDominoes.Count == 0)
		{
			if (score % 5 == 0)
			{
				AddScore(player, score);
			}
			return;
		}
		else
		{
			Domino firstVerticalDomino = History.VerticalDominoes.First.Value;
			if (firstVerticalDomino != History.Spinner)
			{
				if (firstVerticalDomino.Placement.Direction == DominoDirection.Vertical)
				{
					score += firstVerticalDomino.Placement.UpValue;
				}
				else if (firstVerticalDomino.Placement.Direction == DominoDirection.Horizontal)
				{
					score += firstVerticalDomino.Placement.LeftValue + firstVerticalDomino.Placement.RightValue;
				}
				else
				{
					throw new Exception("Error: Domino not specified");
				}
			}
			Domino lastVerticalDomino = History.VerticalDominoes.First.Value;
			if (lastVerticalDomino != History.Spinner)
			{
				if (lastVerticalDomino.Placement.Direction == DominoDirection.Vertical)
				{
					score += lastVerticalDomino.Placement.UpValue;
				}
				else if (lastVerticalDomino.Placement.Direction == DominoDirection.Horizontal)
				{
					score += lastVerticalDomino.Placement.LeftValue + lastVerticalDomino.Placement.RightValue;
				}
				else
				{
					throw new Exception("Error: Domino not specified");
				}
			}
			if (score % 5 == 0)
			{
				AddScore(player, score);
			}
		}
	}

	private void ScoreByEndingHand(Player player)
	{
		Player opponent = null;
		if (player == player1)
		{
			opponent = player2;
		}
		else
		{
			opponent = player1;
		}
		int score = 0;
        foreach (Domino domino in opponent.Dominoes)
		{
			score += domino.Value1 + domino.Value2;
		}
		if (score % 5 <= 2)
		{
			AddScore(player, score / 5 * 5);
		}
		else
		{
			AddScore(player, score / 5 * 5 + 5);
		}
	}

	private void AddScore(Player player, int score)
	{
		player.Score += score;
		if (player.Score >= MaxScore)
		{
			IsGameEnded = true;
		}
	}

	private void NextTurn()
	{
		if (CurrentGameRole == GameRole.Player1)
		{
			CurrentGameRole = GameRole.Player2;
		}
		else if (CurrentGameRole == GameRole.Player2)
		{
			CurrentGameRole = GameRole.Player1;
		}
	}

	public Player GetPlayerState(GameRole role)
	{
		if (role == GameRole.Player1)
		{
			return player1;
		}
		else if (role == GameRole.Player2)
		{
			return player2;
		}
		return null;
	}

	public Player GetOpponentState(GameRole role)
	{
		if (role == GameRole.Player1)
		{
			return player2;
		}
		else if (role == GameRole.Player2)
		{
			return player1;
		}
		return null;
	}
}
