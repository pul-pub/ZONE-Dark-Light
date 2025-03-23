using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Map")]
public class MapObject : ScriptableObject
{
    public List<LocationMeta> map;

    public EntryMeta FindPath(int startID, int targetID)
    {
        LocationMeta startNode = map.Find(l => l.ID == startID);
        LocationMeta targetNode = map.Find(l => l.ID == targetID);

        if (startNode == null || targetNode == null)
        {
            Debug.LogError("Start or target node not found!");
            return null;
        }

        List<LocationMeta> openSet = new List<LocationMeta>();
        HashSet<LocationMeta> closedSet = new HashSet<LocationMeta>();
        openSet.Add(startNode);

        // ������� ��� �������� ������������ ���
        Dictionary<LocationMeta, LocationMeta> parents = new Dictionary<LocationMeta, LocationMeta>();

        parents[startNode] = null;

        while (openSet.Count > 0)
        {
            LocationMeta currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (GetFCost(openSet[i], parents, targetNode) < GetFCost(currentNode, parents, targetNode))
                {
                    currentNode = openSet[i];
                }
            }

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode, parents);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (EntryMeta connection in currentNode.Connections)
            {
                LocationMeta neighbour = map.Find(l => l.ID == connection.locationToID);
                if (neighbour == null || closedSet.Contains(neighbour))
                {
                    continue;
                }

                if (!openSet.Contains(neighbour))
                {
                    parents[neighbour] = currentNode;
                    openSet.Add(neighbour);
                }
            }
        }

        Debug.Log("Path not found!");
        return null;
    }

    private int GetFCost(LocationMeta node, Dictionary<LocationMeta, LocationMeta> parents, LocationMeta targetNode)
    {
        int gCost = GetGCost(node, parents); // ���������� ��������� �� ������
        int hCost = Heuristic(node, targetNode); // ���������
        return gCost + hCost;
    }

    private int GetGCost(LocationMeta node, Dictionary<LocationMeta, LocationMeta> parents)
    {
        int cost = 0;
        LocationMeta current = node;
        while (parents[current] != null)
        {
            cost++;
            current = parents[current];
        }
        return cost;
    }

    private int Heuristic(LocationMeta a, LocationMeta b)
    {
        // ������� ���������: ���������� ��������� (����� �������� �� ����� �������)
        return 1;
    }

    private EntryMeta RetracePath(LocationMeta startNode, LocationMeta endNode, Dictionary<LocationMeta, LocationMeta> parents)
    {
        List<LocationMeta> path = new List<LocationMeta>();
        LocationMeta currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = parents[currentNode];
        }
        path.Reverse();

        // ���� ���� �������� ���� �� ���� ������� ����� ���������, ���������� ������ �������
        if (path.Count > 0)
        {
            LocationMeta firstStepNode = path[0];
            // ������� ���������� �� ��������� ������� � ������ ������� �� ����
            EntryMeta firstStep = startNode.Connections.Find(c => c.locationToID == firstStepNode.ID);
            return firstStep;
        }

        return null; // ���� ���� ���� (��������, ����� � ���� ���������)
    }
}
