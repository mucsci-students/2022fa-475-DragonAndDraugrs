using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{	
	// Place holders to allow connecting to other objects
	public Transform spawnPoint;
	public GameObject player;
	public int level = 0;

	// Flags that control the state of the game
	private float elapsedTime = 0;
	public bool isRunning = false;
	private bool isFinished = false;

	// So that we can access the player's controller from this script
	private FirstPersonController fpsController;

	private bool isPaused = false;
	public GameObject PauseMenu;

	private LevelSelectManager levelSelect;


	// Use this for initialization
	void Start ()
	{
		//Tell Unity to allow character controllers to have their position set directly. This will enable our respawn to work
		Physics.autoSyncTransforms = true;

		Cursor.visible = false;

		// Finds the First Person Controller script on the Player
		fpsController = player.GetComponent<FirstPersonController> ();
	
		// Disables controls at the start.
		fpsController.enabled = false;

		PositionPlayer();

		levelSelect = FindObjectsOfType<LevelSelectManager>()[0];
	}


	//This resets to game back to the way it started
	private void StartGame()
	{
		elapsedTime = 0;
		isRunning = true;
		isFinished = false;

		// Move the player to the spawn point, and allow it to move.
		fpsController.enabled = true;
		PositionPlayer();
	}

	public void RestartLev1()
	{
		isRunning = false;
		isFinished = true;
		fpsController.enabled = false;
		level = 3;
	}

	public void RestartLev2()
	{
		isRunning = false;
		isFinished = true;
		fpsController.enabled = false;
		level = 4;
	}

	public void RestartLev3()
	{
		isRunning = false;
		isFinished = true;
		fpsController.enabled = false;
		level = 5;
	}


	// Update is called once per frame
	void Update ()
	{
		// Add time to the clock if the game is running
		if (isRunning)
		{
			elapsedTime += Time.deltaTime;
		}
		if (Input.GetKeyDown(KeyCode.R))
        {
			ResetLevel();
        }
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isPaused = !isPaused;
			if (isPaused)
            {
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				isRunning = false;
				fpsController.enabled = false;
				PauseMenu.SetActive(true);
				Time.timeScale = 0.0f;
            } else
            {
				Cursor.visible = false;
				if (!isFinished)
                {
					isRunning = true;
					fpsController.enabled = true;
				}
				Cursor.lockState = CursorLockMode.Locked;
				PauseMenu.SetActive(false);
				Time.timeScale = 1.0f;
			}
		}
	}

	public void Resume()
    {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		if (!isFinished)
		{
			isRunning = true;
			fpsController.enabled = true;
		}
		PauseMenu.SetActive(false);
		isPaused = false;
		Time.timeScale = 1.0f;
	}


	//Runs when the player needs to be positioned back at the spawn point
	public void PositionPlayer()
	{
		elapsedTime = 0;
		player.transform.position = spawnPoint.position;
		player.transform.rotation = spawnPoint.rotation;
	}

	//Runs when the player needs to be positioned back at the spawn point
	public void ResetLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}


	// Runs when the player enters the finish zone
	public void FinishedGame()
	{
		isRunning = false;
		isFinished = true;
		fpsController.enabled = false;
	}


	//This section creates the Graphical User Interface (GUI)
	void OnGUI() {
		if (!isPaused)
		{
			int boxWidth = 400;
			if (level == (int)3)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				if (levelSelect.level1Time == -1 || elapsedTime < levelSelect.level1Time)
                {
					levelSelect.level1Time = elapsedTime;
				}
				if (elapsedTime < 51.0 && isFinished)
				{
					levelSelect.level1 = true;
					GUI.Box(new Rect(Screen.width / 2 - 65, 185, 130, 40), "Your Time Was");
					GUI.Label(new Rect(Screen.width / 2 - 10, 200, 30, 30), ((int)elapsedTime).ToString());
					Rect startButton = new Rect(Screen.width / 2 - (boxWidth / 2), Screen.height / 2, boxWidth, 30);
					string message;
					message = "Click to Play Again, or Press Enter to Move to Next Level";
					if (GUI.Button(startButton, message))
					{
						SceneManager.LoadScene(sceneName: "Hallway Level");
					}
					if (Input.GetKeyDown(KeyCode.Return))
					{
						SceneManager.LoadScene(sceneName: "VolcanoLevel");
					}
				}
				else if (elapsedTime > 51.0 && isFinished)
				{
					string message;
					message = "Click or Press Enter to Play Again, Time to Beat is 50 Seconds";
					GUI.Box(new Rect(Screen.width / 2 - 65, 185, 130, 40), "Your Time Was");
					GUI.Label(new Rect(Screen.width / 2 - 10, 200, 30, 30), ((int)elapsedTime).ToString());
					Rect startButton = new Rect(Screen.width / 2 - (boxWidth / 2), Screen.height / 2, boxWidth, 30);
					if (GUI.Button(startButton, message) || Input.GetKeyDown(KeyCode.Return))
					{
						SceneManager.LoadScene(sceneName: "Hallway Level");
					}
				}
			}
			else if (level == (int)4)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				if (levelSelect.level2Time == -1 || elapsedTime < levelSelect.level2Time)
				{
					levelSelect.level2Time = elapsedTime;
				}
				if (elapsedTime < 120.0 && isFinished)
				{
					levelSelect.level2 = true;
					GUI.Box(new Rect(Screen.width / 2 - 65, 185, 130, 40), "Your Time Was");
					GUI.Label(new Rect(Screen.width / 2 - 10, 200, 30, 30), ((int)elapsedTime).ToString());
					Rect startButton = new Rect(Screen.width / 2 - (boxWidth / 2), Screen.height / 2, boxWidth, 30);
					string message;
					message = "Click to Play Again, or Press Enter to Move to Next Level";
					if (GUI.Button(startButton, message))
					{
						SceneManager.LoadScene(sceneName: "VolcanoLevel");
					}
					if (Input.GetKeyDown(KeyCode.Return))
					{
						SceneManager.LoadScene(sceneName: "CityLevel");
					}
				}
				else if (elapsedTime > 120.0 && isFinished)
				{
					string message;
					message = "Click or Press Enter to Play Again, Time to Beat is 120 Seconds";
					GUI.Box(new Rect(Screen.width / 2 - 65, 185, 130, 40), "Your Time Was");
					GUI.Label(new Rect(Screen.width / 2 - 10, 200, 30, 30), ((int)elapsedTime).ToString());
					Rect startButton = new Rect(Screen.width / 2 - (boxWidth / 2), Screen.height / 2, boxWidth, 30);
					if (GUI.Button(startButton, message) || Input.GetKeyDown(KeyCode.Return))
					{
						SceneManager.LoadScene(sceneName: "VolcanoLevel");
					}
				}
			}
			else if (level == (int)5)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				if (levelSelect.level3Time == -1 || elapsedTime < levelSelect.level3Time)
				{
					levelSelect.level3Time = elapsedTime;
				}
				if (elapsedTime < 80.0 && isFinished)
				{
					levelSelect.level3 = true;
					GUI.Box(new Rect(Screen.width / 2 - 65, 185, 130, 40), "Your Time Was");
					GUI.Label(new Rect(Screen.width / 2 - 10, 200, 30, 30), ((int)elapsedTime).ToString());
					Rect startButton = new Rect(Screen.width / 2 - (boxWidth / 2), Screen.height / 2, boxWidth, 30);
					string message;
					message = "Click to Play Again, or Press Enter to Move to Credits";
					if (GUI.Button(startButton, message))
					{
						SceneManager.LoadScene(sceneName: "CityLevel");
					}
					if (Input.GetKeyDown(KeyCode.Return))
					{
						SceneManager.LoadScene(sceneName: "CreditsScene");
					}
				}
				else if (elapsedTime > 80.0 && isFinished)
				{
					string message;
					message = "Click or Press Enter to Play Again, Time to Beat is 80 Seconds";
					GUI.Box(new Rect(Screen.width / 2 - 65, 185, 130, 40), "Your Time Was");
					GUI.Label(new Rect(Screen.width / 2 - 10, 200, 30, 30), ((int)elapsedTime).ToString());
					Rect startButton = new Rect(Screen.width / 2 - (boxWidth / 2), Screen.height / 2, boxWidth, 30);
					if (GUI.Button(startButton, message) || Input.GetKeyDown(KeyCode.Return))
					{
						SceneManager.LoadScene(sceneName: "CityLevel");
					}
				}
			}
			else
			{
				if (!isRunning)
				{
					string message;
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
					if (isFinished)
					{
						message = "Click or Press Enter to Play Again";
					}
					else
					{
						message = "Click or Press Enter to Play";
					}

					//Define a new rectangle for the UI on the screen
					Rect startButton = new Rect(Screen.width / 2 - 120, Screen.height / 2, 240, 30);

					if (GUI.Button(startButton, message) || Input.GetKeyDown(KeyCode.Return))
					{
						//start the game if the user clicks to play
						StartGame();
					}
				}

				// If the player finished the game, show the final time
				if (isFinished)
				{
					GUI.Box(new Rect(Screen.width / 2 - 65, 185, 130, 40), "Your Time Was");
					GUI.Label(new Rect(Screen.width / 2 - 10, 200, 30, 30), ((int)elapsedTime).ToString());
				}
				else if (isRunning)
				{
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
					// If the game is running, show the current time
					GUI.Box(new Rect(Screen.width / 2 - 65, Screen.height - 115, 130, 40), "Your Time Is");
					GUI.Label(new Rect(Screen.width / 2 - 10, Screen.height - 100, 30, 30), ((int)elapsedTime).ToString());
				}
			}
		}
	}
}
