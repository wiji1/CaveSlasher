using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private const string UnlockedLevelKey = "UnlockedLevel";
    
    public Transform mainMenu;
    
    public GameObject levelButtonPrefab;
    public Transform levelsMenu;

    public Transform levelsGrid;
    
    public string[] levelScenes;
    public int highestExistingLevelIndex;

    private bool _firstOpen = true;
    private static AudioController<UISound> _uiAudioController;
    
    private void Start()
    {
        GenerateLevelButtons();
        UpdateUI();
        
        var shouldReturnToLevelSelect = GameManager.ShouldReturnToLevelSelect;

        if (shouldReturnToLevelSelect)
        {
            OpenLevelsMenu();
            GameManager.ShouldReturnToLevelSelect = false;
        }
        else OpenMainMenu();
    }

    private void Awake()
    {
        _uiAudioController = new AudioController<UISound>(GetComponent<AudioSource>(), "UI");
    }

    public void OpenLevelsMenu()
    {
        if (!_firstOpen) _uiAudioController.PlaySound(UISound.Click);

        mainMenu.gameObject.SetActive(false);
        levelsMenu.gameObject.SetActive(true);
        
        _firstOpen = false;
    }
    
    public void OpenMainMenu()
    {
        if (!_firstOpen) _uiAudioController.PlaySound(UISound.Click);
        
        mainMenu.gameObject.SetActive(true);
        levelsMenu.gameObject.SetActive(false);

        _firstOpen = false;
    }
    
    private void GenerateLevelButtons()
    {
        for (var i = 0; i < levelScenes.Length; i++)
        {
            var buttonObject = Instantiate(levelButtonPrefab, levelsGrid);

            var button = buttonObject.GetComponent<Button>();
            var buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = (i + 1).ToString();
            var levelIndex = i;

            button.onClick.AddListener(() =>
            { 
                _uiAudioController.PlaySound(UISound.Click);
                LoadLevel(levelIndex);
            });
        }
    }
    
    private void UpdateUI()
    {
        var unlockedLevelIndex = PlayerPrefs.GetInt(UnlockedLevelKey, 0);

        for (var i = 0; i < levelsGrid.childCount; i++)
        {
            var button = levelsGrid.GetChild(i).GetComponent<Button>();
            
            if (i <= unlockedLevelIndex && i <= highestExistingLevelIndex)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
                var buttonText = button.transform.Find("Text");
                var lockIcon = button.transform.Find("LockImage");
                
                buttonText.gameObject.SetActive(false);
                lockIcon.gameObject.SetActive(true);
            }
        }
    }
    
    private void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelScenes.Length)
        {
            var sceneToLoad = levelScenes[levelIndex];
            GameManager.Instance.StartSceneTransition(sceneToLoad);
        }
        else
        {
            Debug.LogError("Attempted to load an invalid level index: " + levelIndex);
        }
    }
    
    public static void UnlockNextLevel(int levelIndex)
    {
        var levelToUnlock = levelIndex + 1;
        var unlockedLevelIndex = PlayerPrefs.GetInt(UnlockedLevelKey, 0);

        if (levelToUnlock > unlockedLevelIndex)
        {
            PlayerPrefs.SetInt(UnlockedLevelKey, levelToUnlock);
            PlayerPrefs.Save();
            Debug.Log("Unlocked next level at index: " + levelToUnlock);
        }
    }
 
    public void ResetProgress()
    {
        PlayerPrefs.SetInt(UnlockedLevelKey, 0);
        PlayerPrefs.Save();
        Debug.Log("Player progress has been reset.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
