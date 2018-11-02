﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Pathfinding {
	
	private static Grid grid;

    public static Grid Grid
    {
        get
        {
            if (grid == null) grid = GameObject.FindObjectOfType<Grid>();
            return grid;
        }
    }

    public static List<Node> FindPath(Vector3 startPos, Vector3 targetPos) {

		Node startNode = Grid.NodeFromWorldPoint(startPos);
		Node targetNode = Grid.NodeFromWorldPoint(targetPos);

		Heap<Node> openSet = new Heap<Node>(Grid.MaxSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);

			if (currentNode == targetNode) {
				return RetracePath(startNode,targetNode);
			}

			foreach (Node neighbour in Grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                        openSet.UpdateItem(neighbour);
                }
			}
		}
        return null;
	}

    public static List<Node> RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
        //grid.path = path;
		return path;
	}

    public static int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}


}