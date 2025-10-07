using UnityEngine;
using UnityEngine.UI;

public class DamageVignetteController : MonoBehaviour
{
    public Material vignetteMaterial;
    private static float _intensity;
    private Image _vignetteImage;
    
    private void Start()
    {
        SetupVignetteUI();
    }
    
    private void SetupVignetteUI()
    {
        var canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            var canvasGo = new GameObject("VignetteCanvas");
            canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000;
        }
        
        var vignetteGo = new GameObject("DamageVignette");
        vignetteGo.transform.SetParent(canvas.transform, false);
        
        _vignetteImage = vignetteGo.AddComponent<Image>();
        _vignetteImage.material = vignetteMaterial;
        _vignetteImage.raycastTarget = false;
        _vignetteImage.color = Color.white;
        
        var rect = _vignetteImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
    
    private void Update()
    {
        if (_intensity > 0)
        {
            _intensity -= Time.deltaTime * 2f;
            _intensity = Mathf.Max(0, _intensity);
        }
        
        vignetteMaterial.SetFloat("_Intensity", _intensity);

    }
    
    public static void ShowDamage()
    {
        _intensity = 1f;
    }
}