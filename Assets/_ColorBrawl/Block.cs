using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    public string ownerID;
    public Transform visual;
    void Awake()
    {
        visual = gameObject.transform.GetChild(0);
    }

    public void SetOwner(Character character)
    {
        ownerID = character.characterID;
        visual.gameObject.GetComponent<SpriteRenderer>().color = character.sprite.color;
        visual.DOScale(1.2f, 0.05f).OnComplete(() =>
        {
            visual.DOScale(1f, 0.05f);
        });
    }
}
