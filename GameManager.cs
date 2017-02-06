using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public static GameManager instance = null;

    private Text levelText;
    private GameObject levelImage;
    private PlatformManager platformScript;
    private int level = 1;
    private bool doingSetup;


	// Use this for initialization
	void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        platformScript = GetComponent<PlatformManager>();

        var timer = System.Diagnostics.Stopwatch.StartNew();
        InitGame();
        timer.Stop();
        var elapsed = timer.Elapsed;
        Debug.Log(elapsed);

	}

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;

        InitGame();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void InitGame()
    {
        doingSetup = true;
        level = 1;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "World " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        platformScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
