using System.Collections;
using System.Collections.Generic;
using _ColorBrawl;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;
    public int CollisionCount;
    public LevelManager levelManager;

    void Start() {
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        CollisionCount++;
        if(col.gameObject.tag == "Platform") {
            col.gameObject.GetComponent<Block>().SetOwner(character);
            levelManager.UpdateScore();
        }
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        CollisionCount--;
    }
    public bool Touching() {
        return  CollisionCount > 0;

    }

}
