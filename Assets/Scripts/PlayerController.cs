using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName;
    public List<DominoController> dominoControllers;
	
	void Start()
    {
        dominoControllers = new List<DominoController>(28);
	}

    void Update()
    {
        //Domino animation
    }

    // For Tile
    void AddDomino(DominoController dominoController)
    {
        
    }

    // For Game
    void PlayDomino()
    {
        
    }
}
