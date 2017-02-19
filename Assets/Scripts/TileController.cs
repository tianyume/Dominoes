using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileExtension;

public class TileController : MonoBehaviour
{
    const int NUMTILE = 28;
    const int MAXNUM = 6;
    public DominoController exampleDomino;
    public List<DominoController> dominoes;

	void Start()
    {
        dominoes = new List<DominoController>(28);
        Init();
        //Shuffle();
        //ShowCards();
        // Init dominoControllers
	}

    void Update()
    {
        // Domino animation
    }

    // For Game
    public void Shuffle()
    {
        dominoes.Shuffle();        
    }

    // For Game
    public void Reset()
    {
        Init();
    }

    // For Player
    public DominoController Deal(int index)
    {
        //TODO
        if (dominoes[index].Ownership == GameRole.BoneYard)
        {
            return dominoes[index];
        }
        else
        {
            return null;
        }
    }

    void Init()
    {
        if (dominoes.Count>0)
        {
            dominoes.Clear();            
        }
        for (int i = 0; i <= MAXNUM; i++)
        {
            for (int j = i; j <= MAXNUM; j++)
            {
                DominoController temp = (DominoController)Instantiate(exampleDomino);
                temp.SetValues(new DominoController.Values(i,j));
                dominoes.Add(temp);
                //temp.transform.position = new Vector3(i*2, j*2, 0);
            }
        }       
    }

    void ShowCards()
    {
        int count = 0;
        foreach (DominoController temp in dominoes)
        {
            temp.transform.position = new Vector3(count++, 0, 0);
        }
    }
}
