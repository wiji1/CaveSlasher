using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject coinsObject;
    private Player _player;
    private Animator _animator;
    
    private bool _isOpen;
    
    private void Awake()
    {
        _player = Player.GetPlayer();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isOpen) return;
        
        var childCount = coinsObject.transform.childCount;
        
        if (childCount == 0)
        {
            _player.FlashText("The door has opened...");
            _player.PlaySound(PlayerSound.DoorOpen);
            
            _animator.SetTrigger("Open");
            _isOpen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isOpen || other != _player.Collider()) return;
        GameManager.Instance.StartSceneTransition("TitleScreen", 2f);
        
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        var currentSceneIndex = int.Parse(currentScene);
        
        _player.PlaySound(PlayerSound.Teleport);
        
        LevelManager.UnlockNextLevel(currentSceneIndex);

    }
}