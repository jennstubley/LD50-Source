using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeController : MonoBehaviour
{
    public static ChallengeController Instance;

    public GameObject ChallengePrefab;
    public GameObject ChallengePanel;
    public List<string> PossibleSymbols;
    public List<Challenge> ActiveChallenges;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;

        PossibleSymbols = new List<string>();
        ActiveChallenges = new List<Challenge>();
        foreach (CardData data in CardData.StartingHand)
        {
            if (data.Name == "") continue;
            PossibleSymbols.Add(data.Name);
        }
        for (int i=0; i<3; ++i)
        {
            ActiveChallenges.Add(new Challenge(PossibleSymbols[UnityEngine.Random.Range(0, PossibleSymbols.Count)]));
        }
        FillChallenges();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FillChallenges()
    {
        foreach (Transform child in ChallengePanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Challenge challenge in ActiveChallenges)
        {
            GameObject challengeObj = GameObject.Instantiate(ChallengePrefab);
            challengeObj.GetComponentInChildren<Text>().text = challenge.Text();
            challengeObj.GetComponent<ChallengeUI>().SetChallenge(challenge);
            challengeObj.transform.SetParent(ChallengePanel.transform);
        }
    }

    public void CompleteChallenge(Challenge challenge)
    {
        ActiveChallenges.Remove(challenge);
        FillChallenges();
    }
}

public class Challenge
{
    public List<string> Symbols;

    public Challenge(string symbol)
    {
        Symbols = new List<string>() { symbol };
    }

    public string Text()
    {
        string result = "";
        foreach (string symbol in Symbols)
        {
            if (result != "") result += " ";
            result += symbol;
        }
        return result;
    }
}
