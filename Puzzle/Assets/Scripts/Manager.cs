using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public List<Sprite> sprites = new List<Sprite>();
    public GameObject tile;
    public GameObject falling;
    public GameObject[,] tiles;

    // 
    Sprite[] previousLeft = new Sprite[8];
    Sprite previousBelow = null;


    private int[] X1 = new int[8];
    private int[] X2 = new int[8];
    private int[] X3 = new int[8];
    private int[] X4 = new int[8];
    private int[] X5 = new int[8];
    private int[] X6 = new int[8];
    private int[] X7 = new int[8];
    private int[] X8 = new int[8];

    
    private int[] Y1 = new int[8];
    private int[] Y2 = new int[8];
    private int[] Y3 = new int[8];
    private int[] Y4 = new int[8];
    private int[] Y5 = new int[8];
    private int[] Y6 = new int[8];
    private int[] Y7 = new int[8];
    private int[] Y8 = new int[8];



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
        addArr();
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
                GameObject newTile = Instantiate(tile, GetCenter( (int)v3.x , (int)v3.y ), tile.transform.rotation);
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
        addArr();
    }

    public int getIndexTiles(int x, int y)
    {
        int index = -1;
        for (int i = 0; i < 5; i++)
        {
            if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == sprites[i])
            {
                index = i;
            }
        }
        return index;
    }

    private void addArr()
    {
        for (int i = 0; i < 8; i++)
        {
            X1[i] = getIndexTiles(0, i);
            X2[i] = getIndexTiles(1, i);
            X3[i] = getIndexTiles(2, i);
            X4[i] = getIndexTiles(3, i);
            X5[i] = getIndexTiles(4, i);
            X6[i] = getIndexTiles(5, i);
            X7[i] = getIndexTiles(6, i);
            X8[i] = getIndexTiles(7, i);

            Y1[i] = getIndexTiles(i, 0);
            Y2[i] = getIndexTiles(i, 1);
            Y3[i] = getIndexTiles(i, 2);
            Y4[i] = getIndexTiles(i, 3);
            Y5[i] = getIndexTiles(i, 4);
            Y6[i] = getIndexTiles(i, 5);
            Y7[i] = getIndexTiles(i, 6);
            Y8[i] = getIndexTiles(i, 7);
        }
    }
    void FixedUpdate()
    {
        startXrd();
    }
    public void startXrd()
    {
        xrd(X1, 0, 1);
        xrd(X2, 1, 1);
        xrd(X3, 2, 1);
        xrd(X4, 3, 1);
        xrd(X5, 4, 1);
        xrd(X6, 5, 1);
        xrd(X7, 6, 1);
        xrd(X8, 7, 1);



        xrd(Y1, 0, 2);
        xrd(Y2, 1, 2);
        xrd(Y3, 2, 2);
        xrd(Y4, 3, 2);
        xrd(Y5, 4, 2);
        xrd(Y6, 5, 2);
        xrd(Y7, 6, 2);
        xrd(Y8, 7, 2);
    }
    public void xrd(int[] _list, int _index, int _xy)
    {
        // _index la stt cua mang: vd X1 la mang 0
        // _XY de kiem tra xem arr do thuoc truc nao c?a Oxy
        try
        {
            Dictionary<int, List<int>> groupDict = new Dictionary<int, List<int>>();
            string characters = string.Join("", _list.ToArray());
            for (int i = 0; i < characters.Length; i++)
            {
                int cInt = int.Parse(characters[i].ToString());
                MatchCollection mColl = Regex.Matches(characters.Substring(i), "^" + characters[i] + "{3,}");
                if (mColl.Count > 0)
                {
                    if (!groupDict.Keys.Contains(cInt))
                    {
                        delet(_list, cInt, _xy, _index);
                        groupDict.Add(cInt, new List<int>());
                    }
                    groupDict[cInt].Add(mColl[0].Length);
                    i += mColl[0].Length - 1;
                }
            }
        }
        catch
        {
            
        }
    }
    public void delet(int[] _list, int _cInt, int _xy,int _index)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 8 ; i++)
        {
            if (_list[i] == _cInt)
            {
                list.Add(i);
            }
        }
        for (int i = 1; i < list.Count - 1; i++)
        {
            int x = list[i] - list[i - 1] - 1;
            int y = list[i + 1] - list[i] - 1;

            if (x == 0 && y == 0)
            {
                Debug.Log(list[i] + "jjjjj" + _index);
                if(_xy == 1)
                {
                    sprite_(_index, list[i], 1);
                }
                if(_xy == 2)
                {
                    sprite_(list[i], _index, 2);
                }
               
            }
        }
    }
    private void sprite_(int i, int j, int z)
    {
        if(z == 1)
        {
            GameObject newTile = Instantiate(falling, new Vector3(i,j,0), tile.transform.rotation);
            newTile.GetComponent<SpriteRenderer>().sprite = tiles[i, j].GetComponent<SpriteRenderer>().sprite;
            tiles[i, j].GetComponent<SpriteRenderer>().sprite = null;
            tiles[i, j - 1].GetComponent<SpriteRenderer>().sprite = null;
            tiles[i, j + 1].GetComponent<SpriteRenderer>().sprite = null;
        }
        if (z == 2)
        {
            tiles[i, j].GetComponent<SpriteRenderer>().sprite = null;
            tiles[i - 1, j].GetComponent<SpriteRenderer>().sprite = null;
            tiles[i + 1, j].GetComponent<SpriteRenderer>().sprite = null;
        }

        StopCoroutine(FindNullTiles());
        StartCoroutine(FindNullTiles());
    }

    public IEnumerator FindNullTiles()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                    break;
                }
            }
        }
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .15f)
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
