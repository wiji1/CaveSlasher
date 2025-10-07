using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameUIController : MonoBehaviour
{
    private static Player _player;
    private static UIDocument _ui;

    private static VisualElement _root;
    private static VisualElement _healthContainer;
    private static VisualElement[] _healthIcons;

    public RenderTexture renderTexture;

    private RawImage _rawImage;
    private static RectTransform _rawTransform;
    private static Text _coinText;
    
    private static Canvas _canvas;

    private void Start()
    {
        _player = Player.GetPlayer();
        _ui = GetComponent<UIDocument>();
        _root = _ui.rootVisualElement;

        InitializeHearts();
        InitializeCoins();
        UpdateCoins(0);
    }

    private static void InitializeHearts()
    {
        _healthContainer = _root.Q<VisualElement>("HealthContainer");
        _healthIcons = new VisualElement[_player.MaxHealth()];

        _healthContainer.style.flexDirection = FlexDirection.Row;
        _healthContainer.style.alignItems = Align.Center;
        _healthContainer.style.justifyContent = Justify.FlexStart;
        _healthContainer.style.height = 40;

        for (var i = 0; i < _player.MaxHealth(); i++)
        {
            var heartFill = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0,
                    left = 0,
                    width = 40,
                    height = 40,
                    backgroundImage = new StyleBackground(Resources.Load<Texture2D>(Texture.HeartFull.GetPath())),
                    backgroundSize = new BackgroundSize(BackgroundSizeType.Cover),
                    backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat)
                }
            };

            var heartRoot = new VisualElement
            {
                style =
                {
                    width = 40,
                    height = 40,
                    position = Position.Relative
                }
            };

            var bg = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0,
                    left = 0,
                    width = 40,
                    height = 40,
                    backgroundImage = new StyleBackground(Resources.Load<Texture2D>(Texture.HeartBackground.GetPath())),
                    backgroundSize = new BackgroundSize(BackgroundSizeType.Cover),
                    backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat)
                }
            };

            var border = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0,
                    left = 0,
                    width = 40,
                    height = 40,
                    backgroundImage = new StyleBackground(Resources.Load<Texture2D>(Texture.HeartBorder.GetPath())),
                    backgroundSize = new BackgroundSize(BackgroundSizeType.Cover),
                    backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat)
                }
            };

            heartRoot.Add(bg);
            heartRoot.Add(heartFill);
            heartRoot.Add(border);

            _healthContainer.Add(heartRoot);
            _healthIcons[i] = heartFill;
        }

        UpdateHearts();
    }

    private void InitializeCoins()
    {
        _canvas = transform.Find("Canvas").GetComponent<Canvas>();

        var rawGo = new GameObject("CoinIcon", typeof(RawImage));
        rawGo.transform.SetParent(_canvas.transform, false);

        _rawImage = rawGo.GetComponent<RawImage>();
        _rawImage.texture = renderTexture;

        _rawTransform = rawGo.GetComponent<RectTransform>();
        _rawTransform.anchorMin = new Vector2(0, 1);
        _rawTransform.anchorMax = new Vector2(0, 1);
        _rawTransform.pivot = new Vector2(0, 1);
        _rawTransform.sizeDelta = new Vector2(45, 45);
        _rawTransform.anchoredPosition = new Vector2(10, -10);

        var textGo = new GameObject("CoinText", typeof(Text));
        textGo.transform.SetParent(_canvas.transform, false);

        _coinText = textGo.GetComponent<Text>();
        _coinText.font = Resources.Load<UnityEngine.Font>(Font.Cartoon.GetPath());
        _coinText.fontSize = 38;
        _coinText.color = Color.white;
        _coinText.alignment = TextAnchor.MiddleLeft;

        var textRect = textGo.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 1);
        textRect.anchorMax = new Vector2(0, 1);
        textRect.pivot = new Vector2(0, 1);

        textRect.anchoredPosition = new Vector2(20 + _rawTransform.sizeDelta.x + 10, -10 - (_rawTransform.sizeDelta.y / 2f) + 20);
        textRect.sizeDelta = new Vector2(200, 40);
    }

    public static void UpdateCoins(int coins)
    {
        if (_coinText != null)
        {
            _coinText.text = $"x{coins}";
        }
    }

    public static void UpdateHearts()
    {
        for (var i = 0; i < _healthIcons.Length; i++)
        {
            _healthIcons[i].style.display = i < _player.CurrentHealth() ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public static void RemoveUI()
    {
        _ui.gameObject.SetActive(false);
        _canvas.gameObject.SetActive(false);
    }
}
