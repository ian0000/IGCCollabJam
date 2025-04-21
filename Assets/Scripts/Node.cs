using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 Position;
    public Node Parent;
    public float G;
    public float H;
    public float F => G + H;

    public Node(Vector2 pos, Node parent, float g, float h)
    {
        Position = pos;
        Parent = parent;
        G = g;
        H = h;
    }
}
