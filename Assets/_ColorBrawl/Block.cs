using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    public string ownerID;
    public Transform visual;
    public SpriteRenderer blueSprite;
    public SpriteRenderer redSprite;
    void Awake()
    {
    }

    public void SetOwner(Character character)
    {
        ownerID = character.characterID;
        if(ownerID == "red") {
            visual.gameObject.SetActive(false);
            blueSprite.gameObject.SetActive(false);
            redSprite.gameObject.SetActive(true);
        } else if(ownerID == "blue"){
            visual.gameObject.SetActive(false);
            blueSprite.gameObject.SetActive(true);
            redSprite.gameObject.SetActive(false);
        }
        visual.DOScale(.25f, 0.05f).OnComplete(() =>
        {
            visual.DOScale(0.19f, 0.05f);
        });
        blueSprite.transform.DOScale(.25f, 0.05f).OnComplete(() =>
        {
            blueSprite.transform.DOScale(0.19f, 0.05f);
        });
        redSprite.transform.DOScale(.25f, 0.05f).OnComplete(() =>
        {
            redSprite.transform.DOScale(0.19f, 0.05f);
        });
    }
}
