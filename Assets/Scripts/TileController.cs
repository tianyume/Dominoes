using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileExtension;

public class TileController : MonoBehaviour
{
    const int NUMTILE = 28;
    const int MAXNUM = 6;
    const int NUMDEAL = 7;
    public DominoController exampleDomino;
    public List<DominoController> dominoes;
    public float startPosition = -19f;
    public float interval = 1.3f;
    public float localScale = 2.0f;

	void Start()
    {
        dominoes = new List<DominoController>(28);
        Init();
        //Shuffle();
	}

    void Update()
    {
        // Domino animation
//        ShowTile();
    }

    // For Game
    public void Shuffle()
    {
        dominoes.Shuffle();        
    }

    // For Game
    public void ResetHand()
    {
        foreach (DominoController domino in dominoes)
        {
            domino.ownership = GameRole.BoneYard;
        }
        
    }

//    // For Player
//    public List<DominoController> Deal()
//    {
//        List<DominoController> ret = new List<DominoController>(NUMDEAL);
//        int count = 0;
//        for (int i = 0; i < NUMTILE; i++)
//        {
//            if (dominoes[i].ownership == GameRole.BoneYard)
//            {
//                count++;
//                dominoes[i].ownership = GameRole.Player1;
//                DominoController temp = (DominoController)Instantiate(exampleDomino);
//                temp.SetUpperLowerValues(dominoes[i].upperValue, dominoes[i].lowerValue);
//                ret.Add(temp);
//                if (count >= NUMDEAL)
//                {
//                    break;
//                }
//            }
//        }
//        Shuffle();
//        return ret;
//     }

    public DominoController DrawCard()
    {
        if (IsDrawable())
        {
            for (int i = 0; i < NUMTILE; i++)
            {
                if (dominoes[i].ownership == GameRole.BoneYard)
                {
                    dominoes[i].ownership = GameRole.Player1;
                    DominoController temp = (DominoController)Instantiate(exampleDomino);
                    temp.ownership = GameRole.Player1;
                    temp.SetUpperLowerValues(dominoes[i].upperValue, dominoes[i].lowerValue);
                    return temp;
                }
            }
        }
        return null;
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
        dominoes.Clear();
        for (int i = 0; i <= MAXNUM; i++)
        {
            for (int j = i; j <= MAXNUM; j++)
            {
                DominoController temp = (DominoController)Instantiate(exampleDomino);
                temp.SetUpperLowerValues(i, j);
                dominoes.Add(temp);
            }
        } 
        ShowTile();
    }

    void ShowTile()
    {        
        for (int i = 0; i < dominoes.Count; i++)
        {
//            dominoes[i].transform.position = new Vector3(startPosition+(float)i*interval,0,0);
//            dominoes[i].transform.localScale = new Vector3(localScale, localScale, 0);
            //if (dominoes[i].ownership != GameRole.BoneYard)
            //{
                dominoes[i].transform.position = new Vector3(100, 0, 0);
            //}
        }
    }
}
