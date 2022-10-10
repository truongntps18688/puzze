using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Manager : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public GameObject tile;
    public GameObject[,] tiles;
    // 
    Sprite[] previousLeft = new Sprite[8];
    Sprite previousBelow = null;

    public static Manager Instance { get; private set; }
    public bool IsShifting { get; set; }
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Vector2 v = new Vector2(1, 1);
        startTile(v.x, v.y);
        transform.position = new Vector3(4, 0.5f, 0);
        transform.Rotate(180, 0, 0);
    }
    private void startTile(float _x, float _y)
    {
        tiles = new GameObject[8, 8];// khoi tao mang 2 chieu 8x8
        float x = transform.position.x; // lay vi tri cua transform
        float y = transform.position.y;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // set position
                Vector3 v3 = new Vector3(x + (_x * i), y + (_y * j), 0);
                GameObject newTile = Instantiate(tile, GetCenter((int)v3.x, (int)v3.y), tile.transform.rotation);
                tiles[i, j] = newTile;
                tiles[i, j].name = "(" + i + " : " + j + ")";
                newTile.transform.parent = transform;
                newTile.transform.Rotate(180, 0, 0);
                // ng?n ch?n l?p 3 
                List<Sprite> listSprite = new List<Sprite>();
                listSprite.AddRange(sprites);
                listSprite.Remove(previousLeft[j]);
                listSprite.Remove(previousBelow);

                int index = Random.Range(0, listSprite.Count);

                Sprite newSprite = listSprite[index];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
                previousLeft[j] = newSprite;
                previousBelow = newSprite;
            }
        }
    }
    public Vector2 GetCenter(int x, int y)
    {
        return new Vector2(transform.position.x - 4 + x, transform.position.y + 4 - y);
    }



    void Update()
    {
        
    }
    void FixedUpdate()
    {
        
    }

    private Sprite getSprite(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
            return null;
        GameObject tile = tiles[x, y];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }
    private SpriteRenderer getSpriteRenderer(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
            return null;
        GameObject tile = tiles[x, y];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer;
    }

    public bool CheckTiles()
    {
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                SpriteRenderer sprite = getSpriteRenderer(x, y);
                List<SpriteRenderer> horizontalMatches = Find_x(x, y, sprite.sprite);
                if (horizontalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(sprite);
                }

                List<SpriteRenderer> verticalMatches = Find_y(x, y, sprite.sprite);
                if (verticalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(verticalMatches);
                    matchedTiles.Add(sprite);
                }
            }
        }
        foreach (SpriteRenderer renderer in matchedTiles)
        {
            renderer.sprite = null;
            StopCoroutine(FindNullTiles());
            StartCoroutine(FindNullTiles());
        }

        return matchedTiles.Count > 0;
    }

    List<SpriteRenderer> Find_x(int x, int y, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = x + 1; i < 8; i++)
        {
            SpriteRenderer nextColumn = getSpriteRenderer(i, y);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }

    List<SpriteRenderer> Find_y(int x, int y, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = y + 1; i < 8; i++)
        {
            SpriteRenderer nextRow = getSpriteRenderer(x, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }






    public IEnumerator FindNullTiles()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (getSprite(x,y) == null)
                {
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                    break;
                }
            }
        }
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .1f)
    {
        IsShifting = true;
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullCount = 0;

        for (int y = yStart; y < 8; y++)
        {
            SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null)
            {
                nullCount++;
            }
            renders.Add(render);
        }

        for (int i = 0; i < nullCount; i++)
        {
            yield return new WaitForSeconds(shiftDelay);
            for (int k = 0; k < renders.Count - 1; k++)
            {
                renders[k].sprite = renders[k + 1].sprite;
                renders[k].size = new Vector2(1f, 1f);
                renders[k + 1].sprite = GetNewSprite(x, 8 - 1);
                renders[k + 1].size = new Vector2(1f, 1f);
            }
        }
        IsShifting = false;
    }
    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(sprites);

        if (x > 0)
        {
            possibleCharacters.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < 8 - 1)
        {
            possibleCharacters.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleCharacters.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }
        return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
    }

}
