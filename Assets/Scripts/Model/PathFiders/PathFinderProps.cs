public class PathFinderProps
{
	public bool immediateCalculation;
	public bool includeDiagonals;
	public byte minimalWallCost;
	public ESearchType searchType;
	//public bool includeStepInCost;
	public bool invertNodeWeight;
	public float euristicValue;
}