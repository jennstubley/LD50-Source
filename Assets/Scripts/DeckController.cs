using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController Instance;

    public GameObject CardPrefab;
    public GameObject HandPanel;
    public GameObject InPlayPanel;
    public GameObject NewCardPanel;
    public GameObject KnightPrefab;
    public GameObject LaserPrefab;

    public List<Card> Deck = new List<Card>();
    public List<Card> Hand = new List<Card>();
    public List<Card> DiscardPile = new List<Card>();
    public List<Card> NewCardOptions = new List<Card>();
    public Card InPlayCard;
    public int Energy;
    public int MaxEnergy = 3;
    public bool DuplicateActive;
    public bool NextFreeActive;
    public bool ExtraDamageActive;

    private List<Card> lastHand = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;

        Energy = MaxEnergy;
        foreach (CardData data in CardData.StartingHand)
        {
            Deck.Add(new Card(data));
        }
        Shuffle();
        Draw(5);
        FillHand();
    }

    public void SelectNewCard(Card selectedCard)
    {
        if (!NewCardPanel.activeSelf) return;
        if (!NewCardOptions.Contains(selectedCard)) return;

        DiscardPile.Add(selectedCard);
        NewCardOptions.Clear();
        NewCardPanel.SetActive(false);
        StartNextTurn();
    }

    public void SelectChallenge(Challenge challenge)
    {
        if (InPlayCard == null) return;
        if (InPlayCard.Data.Effect == CardData.EffectType.AttackClosest)
        {
            if (challenge.Symbols.Contains(InPlayCard.Data.Name))
            {
                ChallengeController.Instance.CompleteChallenge(challenge);
                DiscardInPlay();
            }
        }
    }

    public void Play(Card card)
    {
        if (NewCardPanel.activeSelf)
        {
            SelectNewCard(card);
            return;
        }

        if (Energy < card.Data.Cost && !NextFreeActive) return;

        if (!NextFreeActive)
        {
            Energy -= card.Data.Cost;
        }
        if (NextFreeActive && card.Data.Cost > 0) NextFreeActive = false;
        Hand.Remove(card);
        InPlayCard = card;
        GameObject cardObj = GameObject.Instantiate(CardPrefab);
        cardObj.GetComponent<CardUI>().SetCard(card);
        cardObj.transform.SetParent(InPlayPanel.transform);
        cardObj.transform.localScale = new Vector3(1, 1, 1);
        int extraDamageModifier = ExtraDamageActive ? 2 : 1;

        if (card.Data.Effect == CardData.EffectType.Draw)
        {
            Draw(card.Data.Number);
            DiscardInPlay();
        }

        if (card.Data.Effect == CardData.EffectType.AttackClosest)
        {
            ExtraDamageActive = DuplicateActive;
            ZombieController.Instance.AttackClosest(card.Data.Number * extraDamageModifier);
            DiscardInPlay();
        }

        if (card.Data.Effect == CardData.EffectType.AttackRandom)
        {
            ExtraDamageActive = DuplicateActive;
            ZombieController.Instance.AttackRandom(card.Data.Number * extraDamageModifier);
            DiscardInPlay();
        }

        if (card.Data.Effect == CardData.EffectType.Knight)
        {
            ExtraDamageActive = DuplicateActive;
            List<Vector3> destinations = new List<Vector3>() { new Vector3(0, -3f, 0) };
            destinations.AddRange(ZombieController.Instance.GetRandomZombies(3 * extraDamageModifier).Select(z => z.position));
            ZombieController.Instance.DestinationAttack(destinations, 500, KnightPrefab);
            DiscardInPlay();
        }

        if (card.Data.Effect == CardData.EffectType.HorizontalLaser)
        {
            ExtraDamageActive = DuplicateActive;
            Vector3 target = ZombieController.Instance.GetRandomZombies(1)[0].position;
            int direction = UnityEngine.Random.Range(0, 1) < 0.5f ? -1 : 1;
            List<Vector3> destinations = new List<Vector3>() { new Vector3(-7.5f * direction, target.y, 0), new Vector3(7f * direction,  target.y, 0) };
            ZombieController.Instance.DestinationAttack(destinations, 15 * extraDamageModifier, LaserPrefab);
            DiscardInPlay();
        }

        if (card.Data.Effect == CardData.EffectType.VerticalLaser)
        {
            ExtraDamageActive = DuplicateActive;
            Vector3 target = ZombieController.Instance.GetRandomZombies(1)[0].position;
            List<Vector3> destinations = new List<Vector3>() { new Vector3(target.x, -3.5f, 0), new Vector3(target.x, 5.5f, 0) };
            ZombieController.Instance.DestinationAttack(destinations, 25 * extraDamageModifier, LaserPrefab);
            DiscardInPlay();
        }

        if (card.Data.Effect == CardData.EffectType.Repair)
        {
            ZombieController.Instance.WallHealth = Mathf.Min(ZombieController.Instance.MaxWallHealth, ZombieController.Instance.WallHealth + card.Data.Number);
            DiscardInPlay();
        }

        if (card.Data.Effect == CardData.EffectType.Shield)
        {
            if (ZombieController.Instance.Shield < ZombieController.Instance.MaxShield)
            {
                AudioManager.Instance.PlayShield();
            }
            ZombieController.Instance.Shield = Mathf.Min(ZombieController.Instance.MaxShield, ZombieController.Instance.Shield + card.Data.Number);
            DiscardInPlay();
        }

        if (card.Data.Effect == CardData.EffectType.NextFree)
        {
            NextFreeActive = true;
            DiscardInPlay();
        }

        if (card.Data.Effect == CardData.EffectType.ExtraDamage)
        {
            ExtraDamageActive = true;
            DiscardInPlay();
        }

        if (DuplicateActive)
        {
            DuplicateActive = false;
            Energy += card.Data.Cost;
            Card copyCard = new Card(card.Data, true);
            Play(copyCard);
        }

        if (card.Data.Effect == CardData.EffectType.Duplicate)
        {
            DuplicateActive = true;
            DiscardInPlay();
        }
    }

    public void DiscardInPlay()
    {
        if (!InPlayCard.Temporary)
        {
            DiscardPile.Add(InPlayCard);
        }
        InPlayCard = null;
        Destroy(InPlayPanel.transform.GetChild(0).gameObject);
        InPlayPanel.transform.DetachChildren();

    }

    // Update is called once per frame
    void Update()
    {
        FillHand();
    }

    public void Draw(int num)
    {
        for (int i=0; i<num; ++i)
        {
            if (Deck.Count == 0)
            {
                Shuffle();
                if (Deck.Count == 0) return;
            }
            Hand.Add(Deck[0]);
            Deck.RemoveAt(0);
        }
    }

    public void Shuffle()
    {
        Deck.AddRange(DiscardPile.ToArray());
        DiscardPile.Clear();
        for (int i=0; i<Deck.Count; ++i)
        {
            int j = UnityEngine.Random.Range(i, Deck.Count);
            Card deckI = Deck[i];
            Deck[i] = Deck[j];
            Deck[j] = deckI;
        }
    }

    public void FillHand()
    {
        bool shouldUpdate = Hand.Count != lastHand.Count;
        foreach (Card card in Hand)
        {
            if (!lastHand.Contains(card)) shouldUpdate = true;
        }
        if (!shouldUpdate) return;

        foreach (Transform child in HandPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Card card in Hand)
        {
            GameObject cardObj = GameObject.Instantiate(CardPrefab);
            cardObj.GetComponent<CardUI>().SetCard(card);
            cardObj.GetComponent<CardUI>().InHand = true;
            cardObj.transform.SetParent(HandPanel.transform);
            cardObj.transform.localScale = new Vector3(1, 1, 1);
        }
        lastHand.Clear();
        lastHand.AddRange(Hand);
    }

    public void NextTurn(int turnNum)
    {
        DiscardPile.AddRange(Hand.Where(c => !c.Temporary));
        Hand.Clear();
        if (ZombieController.Instance.GameOver) return;

        if (turnNum % 3 == 0)
        {
            ShowCardPicker();
        }
        else
        {
            StartNextTurn();
        }

    }

    public void ShowCardPicker() {
        NewCardPanel.SetActive(true);
        NewCardOptions.Clear();
        for (int i=0; i<2; ++i)
        {
            NewCardOptions.Add(new Card(CardData.AdvancedCards[UnityEngine.Random.Range(0, CardData.AdvancedCards.Count)]));
        }
        foreach (Transform child in NewCardPanel.transform.GetChild(0).Find("Options").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Card card in NewCardOptions)
        {
            GameObject cardObj = GameObject.Instantiate(CardPrefab);
            cardObj.GetComponent<CardUI>().SetCard(card);
            cardObj.transform.SetParent(NewCardPanel.transform.GetChild(0).Find("Options"));
            cardObj.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void StartNextTurn()
    {
        Draw(5);
        Energy = MaxEnergy;
    }
}
