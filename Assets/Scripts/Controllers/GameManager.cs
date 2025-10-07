using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Controllers;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool ShouldReturnToLevelSelect;
    
    private static AudioController<Music> _musicAudioController;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        
        _musicAudioController = new AudioController<Music>(GetComponent<AudioSource>(), "Music");
        _musicAudioController.PlayContinuous(Music.TitleScreen);
    }

    public void StartSceneTransition(string sceneName, float fadeDuration = 1f)
    {
        var currentFadeImage = transform.Find("FadeCanvas")?.GetComponent<Image>();
        
        ShouldReturnToLevelSelect = true; 
        
        StartCoroutine(TransitionToScene(sceneName, fadeDuration, currentFadeImage));
    }

    private IEnumerator TransitionToScene(string sceneName, float fadeDuration, Image fadeImageToUse)
    {
        FadeOut(fadeDuration, fadeImageToUse);
        yield return new WaitForSeconds(fadeDuration);

        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f) yield return null; 

        asyncLoad.allowSceneActivation = true;
        yield return null;

        FadeIn(fadeDuration, fadeImageToUse);
        
        var musicToPlay = GetMusic(sceneName);
        _musicAudioController.PlayContinuous(musicToPlay);
    }
    

    private void FadeOut(float fadeDuration, Image imageToFade)
    {
        if (imageToFade is null) return;
        _musicAudioController.Pause();
        
        StartCoroutine(FadeRoutine(imageToFade));
        return;
        
        IEnumerator FadeRoutine(Image targetImage)
        {
            var color = targetImage.color;
            
            color.a = 0f;
            targetImage.color = color;

            while (color.a < 1f)
            {
                color.a += Time.deltaTime / fadeDuration;
                targetImage.color = color;
                yield return null;
            }
        }
    }
    
    private void FadeIn(float fadeDuration, Image imageToFade)
    {
        if (imageToFade is null) return;
        StartCoroutine(FadeRoutine(imageToFade));
        return;

        IEnumerator FadeRoutine(Image targetImage)
        {
            var color = targetImage.color;
            
            color.a = 1f;
            targetImage.color = color;

            while (color.a >= 0f)
            {
                color.a -= Time.deltaTime / fadeDuration;
                targetImage.color = color;
                yield return null;
            }
        }
    }

    private Music GetMusic(string sceneName)
    {
        if (sceneName == "TitleScreen") return Music.TitleScreen;
        
        var levelNumber = int.Parse(sceneName);
        
        return (Music) levelNumber - 1;
    }
    
    
}