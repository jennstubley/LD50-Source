using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    private DeckController deckController;

    // Start is called before the first frame update
    void Start()
    {
        deckController = FindObjectOfType<DeckController>();
    }

    // Update is called once per frame
    void Update()
    {
        RectMask2D mask = GetComponent<RectMask2D>();
        switch (deckController.Energy)
        {
            case 0:
                mask.padding = new Vector4(200, 0, 0, 0);
                break;
            case 1:
                mask.padding = new Vector4(130, 0, 0, 0);
                break;
            case 2:
                mask.padding = new Vector4(70, 0, 0, 0);
                break;
            default:
                mask.padding = new Vector4(0, 0, 0, 0);
                break;
        }
    }
}
