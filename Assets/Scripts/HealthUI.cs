using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private ZombieController zombieController;

    // Start is called before the first frame update
    void Start()
    {
        zombieController = FindObjectOfType<ZombieController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Find("Health Mask").GetComponent<RectMask2D>().padding = new Vector4(0, 0, (1.0f - (float)zombieController.WallHealth / zombieController.MaxWallHealth) * 135 + 30, 0);
        transform.Find("Shield Mask").GetComponent<RectMask2D>().padding = new Vector4(0, 0, (1.0f- (float)zombieController.Shield / zombieController.MaxShield) * 135 + 30, 0);
    }
}
