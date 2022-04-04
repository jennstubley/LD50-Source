using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    private float splatTime = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ZombieController.Instance.IsNearWall(gameObject))
        {
            transform.Find("Arrow").gameObject.SetActive(true);
        }

        if (splatTime == -1) return;
        splatTime -= Time.deltaTime;
        if (splatTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Kill()
    {
        GetComponent<SpriteRenderer>().color = Color.clear;
        transform.GetChild(0).gameObject.SetActive(true);
        splatTime = Random.Range(0.1f, 0.3f);
    }
}
