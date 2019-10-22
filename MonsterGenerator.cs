using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject monsterPrefab;
    private QuadTree quadTree;
    private int id=0;

    void Start()
    {
        quadTree = new QuadTree(0, 0, 40, 40);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float nx = Random.Range(-20f, 20f);
            float nz = Random.Range(-20f, 20f);
            GameObject newMonster = Instantiate(monsterPrefab) as GameObject;
            newMonster.name = id.ToString();
            Monster script = newMonster.GetComponent<Monster>();
            script.Init(quadTree);
            newMonster.transform.position = new Vector3(nx, 0, nz);
            id += 1;

            quadTree.Add(script);
        }
    }
}
