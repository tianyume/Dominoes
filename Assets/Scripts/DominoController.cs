using UnityEngine;

public class DominoController : MonoBehaviour
{
    public int value1;
    public int value2;

    //private Domino domino;
    //private Vector3 initialPosition;
    //private bool isPicked;

	void Start ()
    {
        //domino = new Domino(value1, value2);
	}

    private void Update()
    {
        //Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (position.x <= transform.position.x + dominoWidth / 2 && position.x >= transform.position.x - dominoWidth / 2 && position.y <= transform.position.y + dominoHeight / 2 && position.y >= transform.position.y - dominoHeight / 2)
        //    {
        //        initialPosition = transform.position;
        //        isPicked = true;
        //    }
        //}
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
}
