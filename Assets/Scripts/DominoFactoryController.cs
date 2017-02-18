using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoFactoryController : MonoBehaviour {
    public Sprite[] sprites;

    public Sprite CloneSprite(int index)
    {
        if (index > sprites.Length)
        {
            return null;
        }
        return Instantiate<Sprite>(sprites[index]);
    }
}
