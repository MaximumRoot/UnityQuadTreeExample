using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : SurvColliderMonoBehaviour
{
    private float dx, dz;

    public void Init(QuadTree quadTree)
    {
        SetQuadTree(quadTree);
    }
    void Start()
    {
        float theta = Random.Range(0f, 3.14f);
        dx = 2f*Mathf.Cos(theta);
        dz = 2f*Mathf.Sin(theta);
    }
    void Update()
    {
        transform.Translate(dx*Time.deltaTime, 0, dz*Time.deltaTime);
        if (transform.position.x > 20f || transform.position.x < -20f)
        {
            dx = -dx;
        }
        if (transform.position.z > 20f || transform.position.z < -20f)
        {
            dz = -dz;
        }
        quadTree.Move(this);
    }
}
