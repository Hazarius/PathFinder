using UnityEngine;

public interface ISearchStrategy
{
	void ProcessNode(Node node, Node currentNode,Vector2 destination, float stepCost, float euristicValue);
}