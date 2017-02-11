using TileExtension;
using System;
using System.Collections.Generic;

public class Game
{
    public const int MaxScore = 150;
    public const int TILE = 6;

    public bool IsGameEnded { get; private set; }

    public GameRole CurrentGameRole { get; private set; }
//	public DominoTile Boneyard { get; private set; }
	public DominoTile Tile { get; private set; }
	public LinkedList<Domino> Boneyard { get; private set; }
	public History History { get; private set; }

    private Player player1 = new Player();
    private Player player2 = new Player();

    public Game()
    {
        IsGameEnded = false;
        CurrentGameRole = GameRole.Player1;
//		Boneyard = new DominoTile();
		History = new History();

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
//                Domino temp = new Domino(i, j);
//				Boneyard.Dominoes.Add(temp);
            }
        }
    }
    
    private void Shuffle()
    {
//		Boneyard.Dominoes.Shuffle();
    }

    private void Deal(int numbertiles, GameRole role)
    {
//		int n = Boneyard.Dominoes.Count;
        for (int i = 0; i < numbertiles; i++)
        {
            //boneyard.Dominoes.Shuffle();
//			Domino temp = Boneyard.Dominoes[n - 1];
//			Boneyard.Dominoes.RemoveAt(n - 1);
//            player1.Dominoes.AddFirst(temp);

            if (role == GameRole.Player1)
            {
//                player1.Dominoes.AddFirst(temp);
            }
            if (role == GameRole.Player2)
            {
//                player2.Dominoes.AddFirst(temp);
            }            
        }
        
    }

	public bool PlayDomino(GameRole role, Domino dominoInHand, Domino dominoOnBoard)
    {
		if (IsGameEnded || !role.Equals(CurrentGameRole) || !IsDominoInHand(role, dominoInHand)) {
            return false;
        }
		Player currentPlayer = GetCurrentPlayer();
		if (dominoOnBoard == null && History.HorizontalDominoes.Count == 0) {
			History.HorizontalDominoes.AddLast (dominoInHand);
			if (dominoInHand.Value1 == dominoInHand.Value2)
			{
				dominoInHand.Placement.Direction = DominoDirection.Vertical;
				dominoInHand.Placement.UpValue = dominoInHand.Value1;
				dominoInHand.Placement.DownValue = dominoInHand.Value2;
				History.Spinner = dominoInHand;
				// TODO
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
			// TODO
		}
		else if (dominoOnBoard.Equals(History.HorizontalDominoes.Last))
		{
			// TODO
		}
		else if (dominoOnBoard.Equals(History.VerticalDominoes.First))
		{
			// TODO
		}
		else if (dominoOnBoard.Equals(History.VerticalDominoes.Last))
		{
			// TODO
		}
		return false;
    }

    public void Reset()
    {

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
			score += firstHorizontalDomino.Placement.UpValue;
			score += firstHorizontalDomino.Placement.DownValue;
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
			score += lastHorizontalDomino.Placement.UpValue;
			score += lastHorizontalDomino.Placement.DownValue;
		}
		else if (lastHorizontalDomino.Placement.Direction == DominoDirection.Vertical)
		{
			score += lastHorizontalDomino.Placement.RightValue;
		}
		else
		{
			throw new Exception("Error: Domino not specified");
		}

		if (History.VerticalDominoes.Count <= 1)
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
				score += firstVerticalDomino.Placement.UpValue;
			}
			Domino lastVerticalDomino = History.VerticalDominoes.First.Value;
			if (lastVerticalDomino != History.Spinner)
			{
				score += lastVerticalDomino.Placement.DownValue;
			}
			if (score % 5 == 0)
			{
				AddScore(player, score);
			}
		}
	}

	private void EndGameScore(Player player)
	{
		// TODO
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
