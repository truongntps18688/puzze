using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class FindTile : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public GameObject[,] tiles;
    public int[] X0 = new int[8];
    public int[] X1 = new int[8];
    public int[] X2 = new int[8];
    public int[] X3 = new int[8];
    public int[] X4 = new int[8];
    public int[] X5 = new int[8];
    public int[] X6 = new int[8];
    public int[] X7 = new int[8];

    public int[] Y0 = new int[8];
    public int[] Y1 = new int[8];
    public int[] Y2 = new int[8];
    public int[] Y3 = new int[8];
    public int[] Y4 = new int[8];
    public int[] Y5 = new int[8];
    public int[] Y6 = new int[8];
    public int[] Y7 = new int[8];

    public static FindTile Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void getTiles(GameObject[,] _tiles)
    {
        tiles = _tiles;
        Debug.Log("?ã l?y tiles:" + tiles.Length);
    }

    void Start()
    {

    }

    void Update()
    {
        addRom(); addRomY();
    }
    void FixedUpdate()
    {
        nrdXY();
    }
    public void nrdXY()
    {
        xrd(X0, 0);
        xrd(X1, 1);
        xrd(X2, 2);
        xrd(X3, 3);
        xrd(X4, 4);
        xrd(X5, 5);
        xrd(X6, 6);
        xrd(X7, 7);
        xrdY(Y0, 0);
        xrdY(Y1, 1);
        xrdY(Y2, 2);
        xrdY(Y3, 3);
        xrdY(Y4, 4);
        xrdY(Y5, 5);
        xrdY(Y6, 6);
        xrdY(Y7, 7);
    }
    private void xrd(int[] list, int x)
    {
        Dictionary<int, List<int>> groupDict = new Dictionary<int, List<int>>();
        string characters = string.Join("", list.ToArray());
        for (int i = 0; i < characters.Length; i++)
        {
            int cInt = int.Parse(characters[i].ToString());
            MatchCollection mColl = Regex.Matches(characters.Substring(i), "^" + characters[i] + "{3,}");
            if (mColl.Count > 0)
            {
                if (!groupDict.Keys.Contains(cInt))
                {
                    delet(cInt, x);
                    groupDict.Add(cInt, new List<int>());
                }
                groupDict[cInt].Add(mColl[0].Length);
                i += mColl[0].Length - 1;
            }
        }
    }
    private void xrdY(int[] list, int y)
    {
        Dictionary<int, List<int>> groupDict = new Dictionary<int, List<int>>();
        string characters = string.Join("", list.ToArray());
        for (int i = 0; i < characters.Length; i++)
        {
            int cInt = int.Parse(characters[i].ToString());
            MatchCollection mColl = Regex.Matches(characters.Substring(i), "^" + characters[i] + "{3,}");
            if (mColl.Count > 0)
            {
                if (!groupDict.Keys.Contains(cInt))
                {
                    deletY(cInt, y);
                    groupDict.Add(cInt, new List<int>());
                }
                groupDict[cInt].Add(mColl[0].Length);
                i += mColl[0].Length - 1;
            }
        }
    }
    // l?y t?t c? stt tile cùng 1 sprite
    private void CompactDelet(List<int> list, int _i, int _index)
    {
        for (int i = _i; i < 8 + _i; i++)
        {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>().sprite == sprites[_index])
            {
                list.Add(i);
            }
        }
    }
    private void CompactDeletY(List<int> list, int _i, int _index)
    {
        for (int i = _i; i < 64; i += 8)
        {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>().sprite == sprites[_index])
            {
                list.Add(i);
            }
        }
    }
    // truy?n stt tile vào t? m?ng ?? x? lý
    public void delet(int _index, int _x)
    {
        List<int> list = new List<int>();
        switch (_x)
        {
            case 0:
                CompactDelet(list, 0, _index);
                break;
            case 1:
                CompactDelet(list, 8, _index);
                break;
            case 2:
                CompactDelet(list, 16, _index);
                break;
            case 3:
                CompactDelet(list, 24, _index);
                break;
            case 4:
                CompactDelet(list, 32, _index);
                break;
            case 5:
                CompactDelet(list, 40, _index);
                break;
            case 6:
                CompactDelet(list, 48, _index);
                break;
            case 7:
                CompactDelet(list, 56, _index);
                break;
        }

        for (int i = 1; i < list.Count - 1; i++)
        {
            int x = list[i] - list[i - 1] - 1;
            int y = list[i + 1] - list[i] - 1;

            if (x == 0 && y == 0)
            {
                sprite_(list[i]);
            }
        }
    }


    public void deletY(int _index, int _y)
    {
        List<int> list = new List<int>();
        switch (_y)
        {
            case 0:
                CompactDeletY(list, 0, _index);
                break;
            case 1:
                CompactDeletY(list, 1, _index);
                break;
            case 2:
                CompactDeletY(list, 2, _index);
                break;
            case 3:
                CompactDeletY(list, 3, _index);
                break;
            case 4:
                CompactDeletY(list, 4, _index);
                break;
            case 5:
                CompactDeletY(list, 5, _index);
                break;
            case 6:
                CompactDeletY(list, 6, _index);
                break;
            case 7:
                CompactDeletY(list, 7, _index);
                break;
        }
        for (int i = 1; i < list.Count - 1; i++)
        {
            int x = list[i] - list[i - 1] - 8;
            int y = list[i + 1] - list[i] - 8;

            if (x == 0 && y == 0)
            {
                Debug.Log(i);
                Debug.Log(list[i]);
                spriteY_(list[i]);
            }
        }
    }
    private void sprite_(int index)
    {
        transform.GetChild(index).GetComponent<SpriteRenderer>().sprite = null;
        transform.GetChild(index + 1).GetComponent<SpriteRenderer>().sprite = null;
        transform.GetChild(index - 1).GetComponent<SpriteRenderer>().sprite = null;
        StopCoroutine(Manager.Instance.FindNullTiles());
        StartCoroutine(Manager.Instance.FindNullTiles());
    }
    private void spriteY_(int index)
    {
        transform.GetChild(index).GetComponent<SpriteRenderer>().sprite = null;
        transform.GetChild(index + 8).GetComponent<SpriteRenderer>().sprite = null;
        transform.GetChild(index - 8).GetComponent<SpriteRenderer>().sprite = null;
        StopCoroutine(Manager.Instance.FindNullTiles());
        StartCoroutine(Manager.Instance.FindNullTiles());
    }
    // rút g?n
    private void Compact(int i, int x, int[] list)
    {
        for (int index = 0; index < sprites.Count; index++)
        {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>().sprite == sprites[index])
            {
                list[i - x] = index;
            }
        }
    }
    private void CompactY(int i, int x, int[] list)
    {
        for (int index = 0; index < sprites.Count; index++)
        {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>().sprite == sprites[index])
            {
                list[(i - x) / 8] = index;
            }
        }
    }
    // add m?ng ?? x? lý
    public void addRom()
    {
        for (int i = 0; i < 8; i++)
        {
            Compact(i, 0, X0);
        }
        for (int i = 8; i < 16; i++)
        {
            Compact(i, 8, X1);
        }
        for (int i = 16; i < 24; i++)
        {
            Compact(i, 16, X2);
        }
        for (int i = 24; i < 32; i++)
        {
            Compact(i, 24, X3);
        }
        for (int i = 32; i < 40; i++)
        {
            Compact(i, 32, X4);
        }
        for (int i = 40; i < 48; i++)
        {
            Compact(i, 40, X5);
        }
        for (int i = 48; i < 56; i++)
        {
            Compact(i, 48, X6);
        }
        for (int i = 56; i < 64; i++)
        {
            Compact(i, 56, X7);
        }
    }
    public void addRomY()
    {
        for (int i = 0; i < 64; i += 8)
        {
            CompactY(i, 0, Y0);
        }
        for (int i = 1; i < 64; i += 8)
        {
            CompactY(i, 1, Y1);
        }
        for (int i = 2; i < 64; i += 8)
        {
            CompactY(i, 2, Y2);
        }
        for (int i = 2; i < 64; i += 8)
        {
            CompactY(i, 2, Y2);
        }
        for (int i = 3; i < 64; i += 8)
        {
            CompactY(i, 3, Y3);
        }
        for (int i = 4; i < 64; i += 8)
        {
            CompactY(i, 4, Y4);
        }
        for (int i = 5; i < 64; i += 8)
        {
            CompactY(i, 5, Y5);
        }
        for (int i = 6; i < 64; i += 8)
        {
            CompactY(i, 6, Y6);
        }
        for (int i = 7; i < 64; i += 8)
        {
            CompactY(i, 7, Y7);
        }
    }
}
