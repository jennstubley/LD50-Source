using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeUI : MonoBehaviour
{
    public Challenge Challenge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetChallenge(Challenge challenge)
    {
        Challenge = challenge;
    }

    public void Select()
    {
        DeckController.Instance.SelectChallenge(Challenge);
    }
}
