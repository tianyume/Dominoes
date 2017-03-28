using UnityEngine;
using System.Collections.Generic;

public class DominoController : MonoBehaviour
{
    struct Values
    {
        int value1;
        int value2;

        public Values(int value1, int value2)
        {
            if (value1 <= value2)
            {
                this.value1 = value1;
                this.value2 = value2;
            }
            else
            {
                this.value1 = value2;
                this.value2 = value1;
            }
        }
    }

    public enum Direction
    {
        Horizontal, Vertical
    }
    
    static readonly Dictionary<Values, int> dict = new Dictionary<Values, int>
    {
        {new Values(6, 6), 0},
        {new Values(5, 6), 1},
        {new Values(4, 6), 2},
        {new Values(3, 6), 3},
        {new Values(2, 6), 4},
        {new Values(1, 6), 5},
        {new Values(0, 6), 6},
        {new Values(5, 5), 7},
        {new Values(4, 5), 8},
        {new Values(3, 5), 9},
        {new Values(2, 5), 10},
        {new Values(1, 5), 11},
        {new Values(0, 5), 12},
        {new Values(4, 4), 13},
        {new Values(3, 4), 14},
        {new Values(2, 4), 15},
        {new Values(1, 4), 16},
        {new Values(0, 4), 17},
        {new Values(3, 3), 18},
        {new Values(2, 3), 19},
        {new Values(1, 3), 20},
        {new Values(0, 3), 21},
        {new Values(2, 2), 22},
        {new Values(1, 2), 23},
        {new Values(0, 2), 24},
        {new Values(1, 1), 25},
        {new Values(0, 1), 26},
        {new Values(0, 0), 27}
    };

    public DominoFactoryController dominoFactory;
    public bool isObservableByAll;
    public GameRole ownership;

    public Direction direction;

    public int upperValue;
    public int lowerValue;
    public int leftValue;
    public int rightValue;

    public float offsetHorizontal;
    public float offsetVertical;

    float beforeClickPositionX = 0;
    float beforeClickPositionY = 0;

    public bool isClicked = false;

    public delegate void OnClickDelegate(DominoController dominoController);
    public OnClickDelegate onClick;
    private int spriteIndex;
    private void Update()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {

            beforeClickPositionX = transform.position.x;
            beforeClickPositionY = transform.position.y;
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (direction == Direction.Vertical)
            {
                if (position.x <= beforeClickPositionX + Constants.dominoWidth / 2 && position.x >= beforeClickPositionX - Constants.dominoWidth / 2 && position.y <= beforeClickPositionY + Constants.dominoHeight / 2 && position.y >= beforeClickPositionY - Constants.dominoHeight / 2)
                {
                    if (onClick != null)
                    {
                        //isClicked = !isClicked;
                        onClick(this);
                    }
                }
            }
            else
            {
                if (position.x <= beforeClickPositionX + Constants.dominoHeight / 2 && position.x >= beforeClickPositionX - Constants.dominoHeight / 2 && position.y <= beforeClickPositionY + Constants.dominoWidth / 2 && position.y >= beforeClickPositionY - Constants.dominoWidth / 2)
                {
                    if (onClick != null)
                    {
//                        isClicked = !isClicked;
                        onClick(this);
                    }
                }
            }
        }
        GetComponent<SpriteRenderer>().sprite = dominoFactory.CloneSprite(spriteIndex);
        //if (Input.GetMouseButtonUp(0))
        //{
        //    isPicked = false;
        //    transform.position = initialPosition;
        //}
        //if (isPicked && Input.GetMouseButton(0))
        //{
        //    transform.position = new Vector3(position.x, position.y);
        //}
    }

    public void SetUpperLowerValues(int upper, int lower)
    {
        Values values = new Values(upper, lower);
        spriteIndex = dict[values];

        direction = Direction.Vertical;

        upperValue = upper;
        lowerValue = lower;
        leftValue = -1;
        rightValue = -1;

        if (upper > lower)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
    }

    public void SetLeftRightValues(int left, int right)
    {
        Values values = new Values(left, right);
        spriteIndex = dict[values];

        direction = Direction.Horizontal;

        leftValue = left;
        rightValue = right;
        upperValue = -1;
        lowerValue = -1;

        if (left > right)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
    }
}
