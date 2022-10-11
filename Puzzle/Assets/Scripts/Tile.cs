using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
    private static Tile Selected = null;

    private SpriteRenderer sprite;
    private bool click;



    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Select()
    {
        click = true;
        sprite.color = selectedColor;
        Selected = gameObject.GetComponent<Tile>();
    }
    private void Deselect()
    {
        click = false;
        sprite.color = Color.white;
        Selected = null;
    }

    void OnMouseDown()
    {
        if (sprite.sprite == null || Manager.Instance.IsShifting)
        {
            return;
        }
        if (click)
        {
            Deselect();
        }
        else
        {
            if (Selected == null)
            {
                Select();
            }
            else
            {
                Vector2 v = Selected.transform.position - transform.position;
                if (   (   (v.x == 1 || v.x == -1) && v.y == 0) || (   (v.y == 1 || v.y == -1) && v.x == 0)   )
                {
                    swap(Selected.sprite);
                    Selected.Deselect();
                    ManagerTime.Instance.reducedTime();
                    bool check = Manager.Instance.CheckTiles();
                    if (!check)
                    {
                        do
                        {
                            StopCoroutine(Manager.Instance.FindNullTiles());
                            StartCoroutine(Manager.Instance.FindNullTiles());
                        } while (Manager.Instance.CheckTiles());
                    }
                }
                else
                {
                    Selected.GetComponent<Tile>().Deselect();
                    Select();
                }
            }
        }

    }
    public void swap(SpriteRenderer _sprite)
    {
        if (sprite.sprite == _sprite.sprite) return;
        Sprite rd = sprite.sprite;
        sprite.sprite = _sprite.sprite;
        _sprite.sprite = rd;
    }
}
