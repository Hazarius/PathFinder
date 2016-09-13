public class PathFinderResult
{
	public Node[] Path;
	public float estimatedTime;
	public float length;
	public int totalSteps;
	public EStepResult State;

	public PathFinderResult()
	{
		Path = new Node[0];
		State = EStepResult.Idle;
	}
}