﻿using UnityEngine;

public class DepthStrategy : ISearchStrategy
{
	public void ProcessNode(Node node, Node currentNode, Vector2 destination, float stepCost, float euristicValue)
	{
		node.distance = Vector2.Distance(new Vector2(node.position.x, node.position.y), destination);

		node.step = currentNode.step + stepCost;

		node.cost = node.weight + node.distance;

		node.Parent = currentNode;
	}
}