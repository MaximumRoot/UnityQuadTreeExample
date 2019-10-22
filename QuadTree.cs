using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadNode
{
    public float cx, cy, width, height;
    public int count, subCount;
    public ISurvCollider listHeader;
    public QuadNode[] subArea;
    public QuadNode parent;

    internal QuadNode(QuadNode parent, float cx, float cy, float width, float height)
    {
        this.parent = parent;
        this.cx = cx;
        this.cy = cy;
        this.width = width;
        this.height = height;
        count = 0;
        subCount = 0;

        listHeader = new ColliderListHeader();
    }
}

public class QuadTree
{
    private int __max__ = 8;
    private QuadNode root;

    public QuadTree(float cx, float cy, float width, float height)
    {
        root = new QuadNode(null, cx, cy, width, height);
    }

    public void Add(ISurvCollider newCollider)
    {
        QuadNode locationNode = FindLocation(newCollider);
        AddToQuadNode(locationNode, newCollider);
        //Debug.Log(Dump());
    }

    public void Move(ISurvCollider movedCollider)
    {
        QuadNode beforeQuadNode, afterQuadNode;
        beforeQuadNode = movedCollider.node;
        afterQuadNode = FindLocation(movedCollider);
        if (beforeQuadNode != afterQuadNode)
        {
            RemoveFromQuadNode(beforeQuadNode, movedCollider);
            AddToQuadNode(afterQuadNode, movedCollider);
            //Debug.Log(Dump());
        }
    }

    public void Remove(ISurvCollider removedCollider)
    {
        RemoveFromQuadNode(removedCollider.node, removedCollider);
    }

    private QuadNode FindLocation(ISurvCollider survCollider)
    {
        float x, y;
        x = survCollider.x;
        y = survCollider.y;
        QuadNode search = root;
        while (search.subArea != null)
        {
            int index = 0;
            index += (x > search.cx)?(1):(0);
            index += (y > search.cy)?(2):(0);
            search = search.subArea[index];
        }
        return search;
    }

    private void AddToQuadNode(QuadNode quadNode, ISurvCollider newCollider)
    {
        AddToList(quadNode.listHeader, newCollider);
        CountUpdate(quadNode, 1);
        newCollider.node = quadNode;
        if (IsQuadNodeFull(quadNode))
        {
            DivideQuadNode(quadNode);
        }
    }

    private void RemoveFromQuadNode(QuadNode quadNode, ISurvCollider removedCollider)
    {
        RemoveFromList(removedCollider);
        CountUpdate(quadNode, -1);
        removedCollider.node = null;
    }

    private void DivideQuadNode(QuadNode parentQuadNode)
    {
        CreateSubArea(parentQuadNode);

        ISurvCollider movedCollider;
        while (parentQuadNode.listHeader.next != null)
        {
            int index = 0;
            movedCollider = RetrieveFromQuadNode(parentQuadNode);
            index += (movedCollider.x > parentQuadNode.cx)?(1):(0);
            index += (movedCollider.y > parentQuadNode.cy)?(2):(0);
            AddToQuadNode(parentQuadNode.subArea[index], movedCollider);
        }
    }

    private ISurvCollider RetrieveFromQuadNode(QuadNode quadNode)
    {
        ISurvCollider retrieved = quadNode.listHeader.next;
        RemoveFromList(retrieved);
        CountUpdate(quadNode, -1);
        return retrieved;
    }

    private bool IsQuadNodeFull(QuadNode quadNode)
    {
        return quadNode.count > __max__;
    }

    private void CreateSubArea(QuadNode quadNode)
    {
        quadNode.subArea = new QuadNode[4];
        quadNode.subArea[0] = new QuadNode(quadNode,
                                         quadNode.cx - quadNode.width/4.0f, 
                                         quadNode.cy - quadNode.height/4.0f, 
                                         quadNode.width/2.0f, quadNode.height/2.0f);
        quadNode.subArea[1] = new QuadNode(quadNode,
                                         quadNode.cx + quadNode.width/4.0f, 
                                         quadNode.cy - quadNode.height/4.0f, 
                                         quadNode.width/2.0f, quadNode.height/2.0f);
        quadNode.subArea[2] = new QuadNode(quadNode,
                                         quadNode.cx - quadNode.width/4.0f, 
                                         quadNode.cy + quadNode.height/4.0f, 
                                         quadNode.width/2.0f, quadNode.height/2.0f);
        quadNode.subArea[3] = new QuadNode(quadNode,
                                         quadNode.cx + quadNode.width/4.0f, 
                                         quadNode.cy + quadNode.height/4.0f, 
                                         quadNode.width/2.0f, quadNode.height/2.0f);
    }

    private void AddToList(ISurvCollider listHeader, ISurvCollider newCollider)
    {
        newCollider.next = listHeader.next;
        if (listHeader.next != null)
            listHeader.next.prev = newCollider;
        listHeader.next = newCollider;
        newCollider.prev = listHeader;
    }

    private void RemoveFromList(ISurvCollider removedCollider)
    {
        if (removedCollider.next != null)
            removedCollider.next.prev = removedCollider.prev;
        removedCollider.prev.next = removedCollider.next;
        removedCollider.next = null;
        removedCollider.prev = null;
    }

    private void CountUpdate(QuadNode quadNode, int num)
    {
        QuadNode cur = quadNode.parent;
        quadNode.count += num;
        while (cur != null)
        {
            cur.subCount += num;
            cur = cur.parent;
        }
    }

    private string Dump()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        _Dump(root, sb);
        return sb.ToString();
    }

    private void _Dump(QuadNode search, System.Text.StringBuilder sb)
    {
        sb.Append("node["+search.cx+", "+search.cy+"] ("
                   +search.count+", "+search.subCount+")\n");
        if (search.subArea != null)
        {
            for (int i=0; i<4; i++)
            {
                _Dump(search.subArea[i], sb);
            }
        }

        ISurvCollider _tmp_ = search.listHeader.next;
        while (_tmp_ != null)
        {
            sb.Append(_tmp_);
            sb.Append(", ");
            _tmp_ = _tmp_.next;
        }
        sb.Append("\n");
    }
}
