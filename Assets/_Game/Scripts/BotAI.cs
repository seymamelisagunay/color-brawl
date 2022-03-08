using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;

public class BotAI : MonoBehaviour
{
    public float MaxActionInterval;
    
    private float nextActionTime;
    private Character character;
    void Start()
    {
        nextActionTime = Time.time + 0.5f;
        character = gameObject.GetComponent<Character>();
 
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextActionTime) {
            character.JumpUp();
            nextActionTime = Time.time + Random.Range(0.2f, MaxActionInterval);
        }
    }
}
