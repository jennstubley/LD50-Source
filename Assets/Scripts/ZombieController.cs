using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieController : MonoBehaviour
{
    public static ZombieController Instance;

    public GameObject ZombiePrefab;
    public float ZombieSpeed = 0.5f;
    public GameObject GameOverPanel;
    public GameObject Attacks;

    public bool GameOver;
    public int Turn = 0;
    public int MaxWallHealth = 10;

    public int WallHealth;
    public int Shield = 0;
    public int MaxShield = 10;
    public int BodyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;
        WallHealth = MaxWallHealth;
        SpawnZombies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        if (GameOver) return;
        AudioManager.Instance.PlayNextTurn();
        Turn++;
        WalkZombies();
        SpawnZombies();
        ZombiesAttack();
        DeckController.Instance.NextTurn(Turn);
    }

    public void WalkZombies()
    {
        foreach (Transform child in transform)
        {
            float speed = ZombieSpeed + UnityEngine.Random.Range(-0.05f, 0.05f);
            if (IsAtWall(child.gameObject))
                // Skip zombies at the wall.
                continue;
            child.transform.position = new Vector3(child.transform.position.x, Mathf.Max(child.transform.position.y - ZombieSpeed, -3f), child.transform.position.z);
        }
    }

    public bool IsNearWall(GameObject zoombie)
    {
        return Mathf.Abs(zoombie.transform.position.y + 3f) < 0.1f + ZombieSpeed;
    }

    public bool IsAtWall(GameObject zoombie)
    {
        return Mathf.Abs(zoombie.transform.position.y + 3f) < 0.1f;
    }

    public void SpawnZombies()
    {
        int numToSpawn = Mathf.Max(1, Mathf.RoundToInt(Mathf.Pow(1.2f, Turn+2)));
        for (int i=0; i< numToSpawn; ++i)
        {
            GameObject zombie = GameObject.Instantiate(ZombiePrefab);
            zombie.transform.SetParent(transform);
            zombie.transform.localPosition = new Vector3(UnityEngine.Random.Range(-7f, 7f), 4.2f, 0); 
        }
    }

    public void ZombiesAttack()
    {
        foreach (Transform child in transform)
        {
            if (IsAtWall(child.gameObject))
            {
                if (Shield > 0)
                {
                    Shield -= 1;
                }
                else
                {
                    WallHealth = Mathf.Max(0, WallHealth - 1);
                }
                if (CheckGameOver()) return;
                child.GetComponent<Zombie>().Kill();
            }

        }

    }

    public bool CheckGameOver()
    {
        if (WallHealth <= 0)
        {
            GameOverPanel.SetActive(true);
            GameOver = true;
            Debug.LogError("Game over!!");
            return true;
        }

        return false;
    }

    internal void CompleteAttack(DestinationAttack attack)
    {
        attack.gameObject.transform.SetParent(null);
        Destroy(attack.gameObject);
    }

    public void AttackClosest(int num)
    {
        List<Transform> toDestroy = new List<Transform>();
        List<Transform> allZombies = transform.GetComponentsInChildren<SpriteRenderer>().OrderBy(t => t.transform.position.y).Select(s => s.transform).ToList();
        for (int i=0; i<Mathf.Min(transform.childCount, num); ++i)
        {
            toDestroy.Add(allZombies[i]);
        }
        foreach (Transform child in toDestroy)
        {
            BodyCount++;
            AudioManager.Instance.PlaySplat();
            child.SetParent(null);
            child.GetComponent<Zombie>().Kill();
        }
    }

    internal void DestinationAttack(List<Vector3> destinations, int cap, GameObject Prefab)
    {
        GameObject obj = GameObject.Instantiate(Prefab);
        DestinationAttack destAttack = obj.GetComponent<DestinationAttack>();
        destAttack.SetDestinations(destinations, cap);
        obj.transform.SetParent(Attacks.transform);
    }

    internal List<Transform> GetRandomZombies(int num)
    {
        List<Transform> result = new List<Transform>();
        for (int i = 0; i < Mathf.Min(transform.childCount, num); ++i)
        {
            int rand;
            do
            {
                rand = UnityEngine.Random.Range(0, transform.childCount);
            } while (result.Contains(transform.GetChild(rand)));
            result.Add(transform.GetChild(rand));
        }
        return result;

    }

    public void AttackRandom(int num)
    {
        List<Transform> toDestroy = GetRandomZombies(num);
        foreach (Transform child in toDestroy)
        {
            BodyCount++;
            AudioManager.Instance.PlaySplat();
            child.SetParent(null);
            child.GetComponent<Zombie>().Kill();
        }
    }

    public void AttackTarget(GameObject gameObject)
    {
        BodyCount++;
        AudioManager.Instance.PlaySplat();
        gameObject.transform.SetParent(null);
        gameObject.GetComponent<Zombie>().Kill();
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(1);
    }
}
