using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public DominoController exampleDomino;
    public List<DominoController> dominoControllers;

	void Start()
    {
        dominoControllers = new List<DominoController>(28);
        // Init dominoControllers
	}

    void Update()
    {
        // Domino animation
    }

    // For Game
    void Shuffle()
    {
        
    }

    // For Game
    void Reset()
    {
    }

    // For Player
    DominoController Deal()
    {
        //TODO
        return null;
    }
}
