using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Find("Overpower").gameObject.SetActive(DeckController.Instance.ExtraDamageActive);
        transform.Find("Free Trial").gameObject.SetActive(DeckController.Instance.NextFreeActive);
        transform.Find("Mirror").gameObject.SetActive(DeckController.Instance.DuplicateActive);
    }
}
