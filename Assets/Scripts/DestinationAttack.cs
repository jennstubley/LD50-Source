using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationAttack : MonoBehaviour
{
    private List<Vector3> destinations = new List<Vector3>();
    private int cap;
    private int numAttacked;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (destinations.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinations[0], 0.15f);
            if (Vector3.Distance(transform.position, destinations[0]) <= 0.1f)
            {
                destinations.RemoveAt(0);
            }
        }
        else
        {
            ZombieController.Instance.CompleteAttack(this);
        }
    }

    public void SetDestinations(List<Vector3> destinations, int cap)
    {
        transform.position = destinations[0];
        this.destinations = destinations;
        this.cap = cap;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Transform parent = other.gameObject.transform.parent;
        if (parent != null && parent.gameObject.name == "Battlefield")
        {
            ZombieController.Instance.AttackTarget(other.gameObject);
            numAttacked++;
            if (numAttacked >= cap) destinations.Clear();
        }
    }
}
