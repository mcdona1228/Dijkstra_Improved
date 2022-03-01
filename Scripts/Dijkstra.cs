using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dijkstra
{

    public class NodeRecord : IComparable<NodeRecord>
    {
        public Node node;
        public Connect connect;
        public float costSoFar = 10f;


        public int CompareTo(NodeRecord other)
        {
            if(other == null)
            {
                return 1;
            }
            return (int)(costSoFar - other.costSoFar);
        }
    }

    class PathfindingList
    {
        List<NodeRecord> nodeRecords = new List<NodeRecord>();

        public void add(NodeRecord n)
        {
            nodeRecords.Add(n);
        }
        public void remove(NodeRecord n)
        {
            nodeRecords.Remove(n);
        }
        public NodeRecord smallElement()
        {
            nodeRecords.Sort();
            return nodeRecords[0];
        }
        public int length()
        {
            return nodeRecords.Count;
        }
        public bool contains(Node node)
        {
            foreach(NodeRecord n in nodeRecords)
            {
                if(n.node == node)
                {
                    return true;
                }
            }
            return false;
        }
        public NodeRecord find(Node node)
        {
            foreach (NodeRecord n in nodeRecords)
            {
                if (n.node == node)
                {
                    return n;
                }
            }
            return null;
        }
    }
    public static List<Connect> pathfind(Graph graph, Node start, Node goal)
    {
        NodeRecord startRecord = new NodeRecord();
        startRecord.node = start;
        startRecord.connect = null;
        startRecord.costSoFar = 0;

        PathfindingList open = new PathfindingList();
        open.add(startRecord);
        PathfindingList closed = new PathfindingList();

        NodeRecord current = new NodeRecord();
        while (open.length() > 0)
        {
            current = open.smallElement();

            if(current.node == goal)
            {
                break;
            }

            List<Connect> connects = graph.getConnect(current.node);

            foreach(Connect connect in connects)
            {
                Node endNode = connect.getToNode();
                float endNodeCost = current.costSoFar + connect.getCost();

                NodeRecord endNodeRecord = new NodeRecord();

                if (closed.contains(endNode))
                {
                    continue;
                }
                else if (open.contains(endNode))
                {
                    endNodeRecord = open.find(endNode);
                    
                    if (endNodeRecord != null && endNodeRecord.costSoFar < endNodeCost)
                    {
                        continue;
                    }
                }
                else
                {
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.node = endNode;
                }

                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connect = connect;

                if (!open.contains(endNode))
                {
                    open.add(endNodeRecord);
                }
            }
            open.remove(current);
            closed.add(current);
        }
        if(current.node != goal)
        {
            return null;
        }
        else
        {
            List<Connect> path = new List<Connect>();

            while (current.node != start)
            {
                path.Add(current.connect);
               // Debug.Log(current.connect);
                Node fromNode = current.connect.getFromNode();
                current = closed.find(fromNode);
            }
            path.Reverse();
            return path;
        }
    }
    
}
