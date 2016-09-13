using UnityEngine;

public class InvertedWidthStrategy : ISearchStrategy
{
	public void ProcessNode(Node node, Node currentNode, Vector2 destination, float stepCost, float euristicValue)
	{
		node.distance = Vector2.Distance(new Vector2(node.position.x, node.position.y), destination);

		node.step = currentNode.step + stepCost;

		var weight = 255 - node.weight;
		node.cost = weight + node.distance * euristicValue + node.step;
		node.Parent = currentNode;
	}
}