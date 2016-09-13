using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
	public PathFinderView view;

	public GameObject mainPanel;
	public GameObject selectorPanel;
	public GameObject euristicPanel;

	public Button loadMazeButton;
	public Button resetButton;
	public Button startPointButton;
	public Button destinationPointButton;
	public Toggle searchTypeToggle;

	
	public Button estimateButton;
	public Button stopEstimateButton;
	public Slider euristicSlider;

		

	public int stepCountPerFrame = 50;
	[Range(0,255)]
	public byte wallCost = 50;	

	private Node _start;
	private Node _end;
	private Maze _maze;
	private IPathFinder _pathFinder;

	private bool _drawTriggerEnadled;
	private bool _estimationTriggerEnabled;	
	private bool _immediateCalc;
	private bool _includeDiagonals;
	private bool _searchTypeToggleValue;
	private bool _isStartNodeSetup;
	private bool _isEndNodeSetup;
	private bool _isImmediateCalculation;
	private bool _invertNodeWeight;

	private ESearchType _searchType;

	void Awake()
	{
		loadMazeButton.onClick.RemoveAllListeners();
		loadMazeButton.onClick.AddListener(OnLoadMazeButtonClick);

		resetButton.onClick.RemoveAllListeners();
		resetButton.onClick.AddListener(OnResetButtonClick);

		startPointButton.onClick.RemoveAllListeners();
		startPointButton.onClick.AddListener(OnSetStartNode);

		destinationPointButton.onClick.RemoveAllListeners();
		destinationPointButton.onClick.AddListener(OnSetDestinationNode);

		estimateButton.onClick.RemoveAllListeners();
		estimateButton.onClick.AddListener(OnEstimateButtonClick);

		stopEstimateButton.onClick.RemoveAllListeners();
		stopEstimateButton.onClick.AddListener(OnStopButtonClick);

		mainPanel.SetActive(false);
		selectorPanel.SetActive(true);
	}

	void Start()
	{
		_pathFinder = new AStar();
		UpdateSearchType();		
	}

	private void UpdateSearchType()
	{
		_searchTypeToggleValue = searchTypeToggle.isOn;
		euristicPanel.gameObject.SetActive(!_searchTypeToggleValue);
		_searchType = _searchTypeToggleValue ? ESearchType.Depth : ESearchType.Width;
	}

	#region UI buttons implemetation

	public void OnLoadMazeButtonClick()
	{
		OnStopButtonClick();
		mainPanel.SetActive(false);
		selectorPanel.SetActive(true);
		view.SetInfo("", EMessageType.Normal);
	}

	public void ToggleImmediate()
	{
		_immediateCalc = !_immediateCalc;
		view.SetInfo("Step visualization: " + (_immediateCalc ? "ON" : "OFF"), EMessageType.Normal);
	}

	public void ToggleDiagonals()
	{
		_includeDiagonals = !_includeDiagonals;
		view.SetInfo((_includeDiagonals ? "Use" : "Don't use") + " Diagonals", EMessageType.Normal);
	}

	public void ToggleType()
	{
		UpdateSearchType();
		view.SetInfo("Mode: " + _searchType, EMessageType.Normal);
	}

	public void ToggleInvertWeight()
	{
		_invertNodeWeight = !_invertNodeWeight;
	}

	private void OnSetStartNode()
	{
		_isStartNodeSetup = true;
		view.SetInfo("Select start node", EMessageType.Normal);
	}

	private void OnSetDestinationNode()
	{
		_isEndNodeSetup = true;
		view.SetInfo("Select destination node", EMessageType.Normal);
	}

	private void OnResetButtonClick()
	{
		view.ResetTextures();
		view.SetInfo("", EMessageType.Normal);
		if (_pathFinder != null)
		{
			_pathFinder.Reset();			
		}
		if (_maze != null)
		{
			_maze.Reset();
		}
	}

	private void OnStopButtonClick()
	{
		_estimationTriggerEnabled = false;		
	}

	private void OnEstimateButtonClick()
	{
		if (_maze != null)
		{
			_pathFinder.Reset();

			if (_start != null && _end != null)
			{
				var props = new PathFinderProps
				{
					immediateCalculation = _immediateCalc,
					includeDiagonals = _includeDiagonals,
					minimalWallCost = wallCost,
					searchType = _searchType,					
					euristicValue = euristicSlider.value/10f,
					invertNodeWeight = _invertNodeWeight,
				};
				view.SetInfo("Start path estimating", EMessageType.Normal);
				_pathFinder.EstimatePath(_start, _end, _maze, props);
				if (_isImmediateCalculation)
				{
					
				}
				_isImmediateCalculation = _immediateCalc;
				_estimationTriggerEnabled = true;
				_drawTriggerEnadled = true;				
			}
		}
	}

	public void OnMailToButtonClick()
	{
		Application.OpenURL("mailto:serhii.romanenko@ukr.net");
	}

	#endregion

	public void SetInputTexture(Texture inputData)
	{
		SetState(EProgramState.MainWindow);
		_start = null;
		_end = null;

		_maze = new Maze(inputData);
		view.SetMaze(_maze);
		view.ResetTextures();
	}

	public void SetInputTexture(Sprite iputData)
	{
		SetState(EProgramState.MainWindow);
		_start = null;
		_end = null;

		_maze = new Maze(iputData);
		view.SetMaze(_maze);
		view.ResetTextures();
	}

	private void SetState(EProgramState state)
	{
		switch (state)
		{
			case EProgramState.MainWindow:
				mainPanel.SetActive(true);
				selectorPanel.SetActive(false);
				view.SetInfo("", EMessageType.Normal);
				if (_pathFinder != null)
				{
					_pathFinder.Reset();
				}
				break;

			case EProgramState.SelectMaze:
				mainPanel.SetActive(false);
				selectorPanel.SetActive(true);
				break;
		}
	}

	public void OnSetPoint(int w, int h)
	{
		if (_maze!=null && w > 0 && h > 0 && w < _maze.Width && h < _maze.Height)
		{
			var node = _maze[w, h];
			if ((node.position.x % 2) == 0)
			{
				var newPoint = node.position + Point.down;
				node = _maze[newPoint.x, newPoint.y];
			}

			if ((node.position.y % 2) == 0)
			{
				var newPoint = node.position + Point.left;
				node = _maze[newPoint.x, newPoint.y];
			}

			if (_isStartNodeSetup)
			{
				_start = node;
				_isStartNodeSetup = false;
				_isEndNodeSetup = false;
				view.DrawCircle(w, h, 4, Color.blue);
				view.SetInfo("Start node: W: " + w + " H: " + h, EMessageType.Normal);
			}
			if (_isEndNodeSetup)
			{
				_end = node;
				_isStartNodeSetup = false;
				_isEndNodeSetup = false;
				view.DrawCircle(w, h, 4, Color.red);
				view.SetInfo("Destination node: W: " + w + " H: " + h, EMessageType.Normal);
			}	
		}			
	}

	private void PrintResult(PathFinderResult result)
	{
		var sb = new StringBuilder();
		sb.Append("Path founded! Length: ");
		sb.Append(result.length);
		sb.Append(" Total steps: ");
		sb.Append(result.totalSteps);
		if (result.estimatedTime > 0)
		{
			sb.Append(" Estimated time: ");
			sb.Append(result.estimatedTime);
			sb.Append(" ms");
		}
		view.SetInfo(sb.ToString(), EMessageType.Normal);		
	}

	private void Update()
	{
		stopEstimateButton.interactable = _estimationTriggerEnabled;
		view.euristicValue.text = (euristicSlider.value / 10f).ToString("F1");
		if (_pathFinder != null)
		{
			if (_isImmediateCalculation)
			{
				if (_drawTriggerEnadled)
				{
					if (_pathFinder.Result.State == EStepResult.PathFounded || _pathFinder.Result.State == EStepResult.PathNotFounded)
					{
						view.DrawProgress(_pathFinder.ClosedNodes);
						view.DrawPath(_pathFinder.Result.Path);
						PrintResult(_pathFinder.Result);
						_drawTriggerEnadled = false;
						_estimationTriggerEnabled = false;
					}					
				}				
			}
			else
			{
				if (_estimationTriggerEnabled)
				{
					for (var i = 0; i < stepCountPerFrame; i++)
					{
						if (_pathFinder.Result.State == EStepResult.SearchInProgress)
						{
							_pathFinder.DoStep();
						}
						else
						{
							if (_pathFinder.Result.State == EStepResult.PathFounded)
							{
								view.DrawPath(_pathFinder.Result.Path);
								PrintResult(_pathFinder.Result);
							}
							if (_pathFinder.Result.State == EStepResult.PathNotFounded)
							{
								view.SetInfo("Path not found!", EMessageType.Critical);
							}
							_estimationTriggerEnabled = false;
							break;
						}						
					}
					view.DrawProgress(_pathFinder.ClosedNodes);
				}
			}
		}	
	}
}