using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AStar : IPathFinder
{
	public List<Node> ClosedNodes { get { return _closedNodes; } }

	private readonly List<Node> _closedNodes;
	private Vector2 _destinationPosition;
	private Maze _maze;
	private Node _start;
	private Node _destination;
	private PathFinderProps _props;
	private int _iterator;
	private int _steps;
	private bool _isPathFounded;	
	private Heap<Node> _heap;
	private readonly float _halfStepCost = Mathf.Sqrt(2f);

	private ISearchStrategy _searchStrategy;
	
	public PathFinderResult Result { get; private set; }

	public AStar()
	{
		_closedNodes = new List<Node>();
	}

	public void Reset()
	{
		_closedNodes.Clear();
		_heap = new MinHeap<Node>();		
		Result = new PathFinderResult();
	}

	public void EstimatePath(Node start, Node destination, Maze maze, PathFinderProps props)
	{
		_props = props;
		_maze = maze;
		_start = start;
		
		_destination = destination;

		_iterator = _props.includeDiagonals ? 1 : 2;
		_isPathFounded = false;
		_steps = 0;

		switch (_props.searchType)
		{
			case ESearchType.Depth:
				_searchStrategy = new DepthStrategy();
				break;

			case ESearchType.Width:
				_searchStrategy = new WidthStrategy();
				break;

			default:
				_searchStrategy = new WidthStrategy();
				break;
		}

		_destinationPosition = new Vector2(_destination.position.x, _destination.position.y);
		_start.distance = _start.cost = (int)Vector2.Distance(new Vector2(start.position.x, start.position.y), _destinationPosition);
		_start.step = 0;		
		_heap.Add(_start);
		Result.State = EStepResult.SearchInProgress;
		if (_props.immediateCalculation)
		{
			var sw = new Stopwatch();

			sw.Start();
			while (Result.State == EStepResult.SearchInProgress)
			{
				DoStepInternal();
			}
			sw.Stop();
			Result.estimatedTime = sw.Elapsed.Milliseconds;			
		}	
	}

	private void DoStepInternal()
	{
		if (_heap.Count > 0)
		{
			var currentNode = _heap.ExtractDominating();
			_closedNodes.Add(currentNode);

			if (currentNode == _destination)
			{				
				_isPathFounded = true;
			}
			else
			{
				_steps++;
				for (int i = 0; i < Directions.directions.Length; i += _iterator)
				{
					var direction = Directions.directions[i];
					var pos = currentNode.position + direction;
					if (pos.x >= _maze.Width || pos.x < 0 || pos.y >= _maze.Height || pos.y < 0)
					{
						continue;
					}
					var node = _maze[pos.x, pos.y];

					if (node.Parent != null || node.weight < _props.minimalWallCost)
					{
						continue;
					}

					var stepCost = i/2f > 0 ? _halfStepCost : 1f;

					_searchStrategy.ProcessNode(node, currentNode, _destinationPosition, stepCost, _props.euristicValue);
					_heap.Add(node);
				}
			}
		}
		if (_isPathFounded)
		{
			Result.totalSteps = _steps;
			Result.Path = ConstructPath(_start, _destination, true);
			Result.length = Result.Path.Length;
			Result.State = EStepResult.PathFounded;
		} 
		else if (_heap.Count == 0)
		{
			Result.State = EStepResult.PathNotFounded;
		}
		else
		{
			Result.State = EStepResult.SearchInProgress;
		}		
	}

	public void DoStep()
	{
		if (!_props.immediateCalculation)
		{
			if (Result.State == EStepResult.SearchInProgress)
			{
				DoStepInternal();
			}			
		}	
	}

	private static Node[] ConstructPath(Node start, Node destination, bool includeStartPoint)
	{		
		var result = new List<Node>();
		var currentNode = destination;
		while (currentNode != start && currentNode != null)
		{
			result.Add(currentNode);
			currentNode = currentNode.Parent;
		}
		if (includeStartPoint)
		{
			result.Add(start);
		}
		result.Reverse();
		return result.ToArray();
	}
}