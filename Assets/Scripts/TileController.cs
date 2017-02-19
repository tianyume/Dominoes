using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileExtension;

public class TileController : MonoBehaviour
{
    const int NUMTILE = 28;
    const int MAXNUM = 6;
    const int NUMDEAL = 5;
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
    public List<DominoController> Deal()
    {
        List<DominoController> ret = new List<DominoController>(NUMDEAL);
        int count = 0;
        for (int i = 0; i < NUMTILE; i++)
        {
            if (dominoes[i].ownership == GameRole.BoneYard)
            {
                count++;
                dominoes[i].ownership = GameRole.Player1;
                DominoController temp = (DominoController)Instantiate(exampleDomino);
                temp.SetUpperLowerValues(dominoes[i].upperValue, dominoes[i].lowerValue);
                ret.Add(temp);
                if (count >= NUMDEAL)
                {
                    break;
                }
            }
        }
        return ret;
        /*if (dominoes[index].ownership == GameRole.BoneYard)
        {
            dominoes[index].ownership = GameRole.Player1;
            return dominoes[index];
        }
        else
        {
            return null;
        }*/
    }

    public bool IsDrawable()
    {
        foreach (DominoController temp in dominoes)
        {
            if (temp.ownership == GameRole.BoneYard)
            {
                return true;
            }             
        }
        return false;
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
                temp.SetUpperLowerValues(i, j);
//                temp.SetValues(new DominoController.Values(i,j));
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
