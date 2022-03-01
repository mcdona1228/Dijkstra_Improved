using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind : Kinematic
{
    public Node start;
    public Node goal;
    Graph myGraph;

    Follow myMoveType;
    LookWhereGoing myRotateType;

    GameObject[] myPath;

    void Start()
    {
        myRotateType = new LookWhereGoing();
        myRotateType.character = this;
        myRotateType.target = myTarget;

        Graph myGraph = new Graph();
        myGraph.Build();
        List<Connect> path = Dijkstra.pathfind(myGraph, start, goal);

        //Debug.Log(path);
        myPath = new GameObject[path.Count + 1];
       
        int i = 0;
        foreach (Connect c in path)
        {
            Debug.Log("From " + c.getFromNode() + " to " + c.getToNode() + " @" + c.getCost());
            myPath[i] = c.getFromNode().gameObject;
            i++;
        }
        myPath[i] = goal.gameObject;

        myMoveType = new Follow();
        myMoveType.character = this;
        myMoveType.path = myPath;
    }

    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.angular = myRotateType.getSteering().angular;
        steeringUpdate.linear = myMoveType.getSteering().linear;
        base.Update();
    }
}
