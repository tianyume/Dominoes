using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName;
    public LinkedList<DominoController> dominoControllers;
	
	void Start()
    {
        dominoControllers = new LinkedList<DominoController>();
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
