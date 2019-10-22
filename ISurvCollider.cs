using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISurvCollider
{
    ISurvCollider next { get; set; }
    ISurvCollider prev { get; set; }
    float x {get;}
    float y {get;}
    QuadNode node { get; set; }
}

public class ColliderListHeader : ISurvCollider
{
    private ISurvCollider nextCollider;

    public ISurvCollider next
    {
        get { return nextCollider; }
        set { nextCollider = value; }
    }

    public ISurvCollider prev
    {
        get { return null; }
        set { ; }
    }

    public float x { get{ return -1; } }
    public float y { get{ return -1; } }
    public QuadNode node { get{ return null; } set{} }
}

public class SurvColliderMonoBehaviour : MonoBehaviour, ISurvCollider
{
    protected QuadTree quadTree;
    private QuadNode quadNode;
    private ISurvCollider nextCollider;
    private ISurvCollider prevCollider;

    public ISurvCollider next
    {
        get { return nextCollider; }
        set { nextCollider = value; }
    }

    public ISurvCollider prev
    {
        get { return prevCollider; }
        set { prevCollider = value; }
    }

    public float x { get{ return transform.position.x; } }
    public float y { get{ return transform.position.z; } }
    public QuadNode node 
    {
        get{ return quadNode; }
        set{ quadNode = value; }
    }

    public void SetQuadTree(QuadTree quadTree)
    {
        this.quadTree = quadTree;
    }

}