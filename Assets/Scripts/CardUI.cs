using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Card Card;
    public bool InHand;

    private Transform inner;
    private Vector3 oldPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    internal void SetCard(Card card)
    {
        inner = transform.Find("Inner");
        Card = card;
        Init();
    }

    public void Play()
    {
        DeckController.Instance.Play(Card);
    }

    private void Init()
    {
        inner.Find("Title").GetComponent<TMPro.TextMeshProUGUI>().text = Card.Data.Name;
        inner.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = Card.Data.EffectText();
        inner.Find("Cost").GetComponent<TMPro.TextMeshProUGUI>().text = Card.Data.Cost.ToString();
        string path = Card.Data.Name.Replace(" ", "_").ToLower();
        inner.Find("Back").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
       // if (Card.Temporary)
      //  {
       //     inner.GetComponent<Image>().color = new Color(.56f, .56f, .56f);
       // }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayHover();
        if (!InHand) return;
        oldPos = inner.transform.position;
        inner.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!InHand) return;
        inner.transform.position = oldPos;
    }
}
