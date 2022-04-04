using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public CardData Data;
    public bool Temporary;

    public Card(CardData data, bool temp = false)
    {
        Data = data;
        Temporary = temp;
    }
}

public class CardData
{
    public enum EffectType
    {
        AttackClosest,
        AttackRandom,
        Shield,
        Knight,
        HorizontalLaser,
        VerticalLaser,
        Repair,
        Duplicate,
        Draw,
        ExtraDamage,
        NextFree,
    }

    public string Name;
    public EffectType Effect;
    public int Number;
    public int Cost;

    public CardData(string name, EffectType effect, int cost, int number = 0)
    {
        Name = name;
        Effect = effect;
        Number = number;
        Cost = cost;
    }

    public string EffectText()
    {
        switch (Effect)
        {
            case EffectType.AttackClosest:
                return string.Format("Kill {0} closest zombies", Number);
            case EffectType.AttackRandom:
                return string.Format("Kill {0} zombies", Number);
            case EffectType.HorizontalLaser:
                return string.Format("Zap zombies in a horizontal line");
            case EffectType.VerticalLaser:
                return string.Format("Zap zombies in a vertical line");
            case EffectType.Knight:
                return string.Format("Charges {0} zombies, hitting everything in its path", Number);
            case EffectType.Repair:
                return string.Format("Repair Wall +{0}", Number);
            case EffectType.Shield:
                return string.Format("+{0} Shield", Number);
            case EffectType.Duplicate:
                return Number == 1 ? "Next card is played twice" : string.Format("Next card is played {0} times", Number+1);
            case EffectType.Draw:
                return Number == 1 ? "Draw a card" : string.Format("Draw {0} cards", Number);
            case EffectType.ExtraDamage:
                return "Next attack hits twice as many targets";
            case EffectType.NextFree:
                return "Next card played costs 0 energy";
        }
        throw new Exception("Invalid card, failed to produce text");
    }


    public static List<CardData> StartingHand = new List<CardData>()
    {
        new CardData("Throw Rock", EffectType.AttackRandom, 1, 1),
        new CardData("Throw Rock", EffectType.AttackRandom, 1, 1),
        new CardData("Throw Rock", EffectType.AttackRandom, 1, 1),
        new CardData("Throw Rock", EffectType.AttackRandom, 1, 1),
        new CardData("Shield", EffectType.Shield, 1, 1),
        new CardData("Shield", EffectType.Shield, 1, 1),
        new CardData("Shield", EffectType.Shield, 1, 1),
        new CardData("Shield", EffectType.Shield, 1, 1),
    };

    public static List<CardData> AdvancedCards = new List<CardData>()
    {
        new CardData("Shoot", EffectType.AttackClosest, 1, 3),
        new CardData("Catapult", EffectType.AttackRandom, 1, 5),
        new CardData("Firestorm", EffectType.AttackRandom, 2, 10),
        new CardData("Knight", EffectType.Knight, 3, 3),
        new CardData("Laser X", EffectType.HorizontalLaser, 2),
        new CardData("Laser Y", EffectType.VerticalLaser, 2),
        new CardData("Draw", EffectType.Draw, 0, 1),
        new CardData("Mirror", EffectType.Duplicate, 1, 1),
        new CardData("Overpower", EffectType.ExtraDamage, 0),
        new CardData("Free Trial", EffectType.NextFree, 0),
    };
}