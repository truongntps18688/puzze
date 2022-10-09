using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBackground : MonoBehaviour
{
    public GameObject Background;
    void Start()
    {
        float x = transform.position.x; // lay vi tri cua transform
        float y = transform.position.y;

        Vector2 v = Background.GetComponent<SpriteRenderer>().bounds.size;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Vector3 v3 = new Vector3(x + (v.x * i), y + (v.y * j), 0);
                GameObject background = (GameObject)Instantiate(Background, GetCenter((int)v3.x, (int)v3.y), Quaternion.identity);
                background.transform.parent = transform;
                background.name = "(" + i + " : " + j + ")";
            }
        }
        transform.position = new Vector3(4, -0.5f, 0);
    }
    public Vector2 GetCenter(int x, int y)
    {
        return new Vector2(transform.position.x - 4 + x, transform.position.y + 4 - y);
    }
}
