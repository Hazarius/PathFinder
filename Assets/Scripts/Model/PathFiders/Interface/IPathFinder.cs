using System.Collections.Generic;

public interface IPathFinder
{	
	List<Node> ClosedNodes { get; }
	PathFinderResult Result { get; }
	void Reset();
	void EstimatePath(Node start, Node destination, Maze maze, PathFinderProps props);
	void DoStep();
}
