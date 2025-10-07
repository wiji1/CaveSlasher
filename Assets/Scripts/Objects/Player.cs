using System.Collections;
using Interfaces;
using TMPro;
using UnityEngine;

public class Player : Combatant
{
    private GameObject _playerObject;
    private GameObject _audioObject;
    private Collider2D _attackHitboxCollider;
    
    private PlayerController _playerController;
    private PlayerData _playerData;
    private AnimationController<PlayerAnimationParameter> _animationController;
    private CombatantAudioController<PlayerSound> _audioController;

    private TextMeshProUGUI _flashText;
    
    protected override void OnAwake()
    {
        _playerObject = transform.Find("PlayerObject").gameObject;
        _audioObject = transform.Find("AudioSource").gameObject;
        
        _attackHitboxCollider = _playerObject.transform.Find("AttackHitbox").GetComponent<Collider2D>();
        
        _animationController = new AnimationController<PlayerAnimationParameter>(_playerObject.GetComponent<Animator>());
        _audioController = new CombatantAudioController<PlayerSound>(_audioObject.GetComponent<AudioSource>(), "Player");
        _playerController = new PlayerController(this, _animationController, _audioController);
        _playerData = new PlayerData();

        _flashText = GetComponentInChildren<TextMeshProUGUI>();
        tag = "Player";
        
        OnDamaged += OnPlayerDamaged;
        
        _playerController.Awake();
    }
    
    private void Update()
    {
        _playerController.Update();
    }

    private void OnEnable()
    {
        _playerController.OnEnable();
    }
    
    private void OnDisable()
    {
        _playerController.OnDisable();
    }

    private void FixedUpdate()
    {
        _playerController.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _playerController.OnCollisionEnter2D(other);
    }
    
    private void OnPlayerDamaged(IDamageSource damageSource)
    {
        GameUIController.UpdateHearts();
        DamageVignetteController.ShowDamage();
    }

    protected override CombatantStats GetStats()
    {
        return new CombatantStats(
            maxHealth: 5, 
            attackRange: 1.5f, 
            attackCooldown: 0, 
            invulnerabilityDuration: 0.25f, 
            attackDamage: 1, 
            knockbackForce: 8f,
            knockbackDuration: 0.5f
        );
    }

    protected override IAnimationController GetAnimationController()
    {
        return _animationController;
    }

    protected override ICombatantAudioController GetAudioController()
    {
        return _audioController;
    }
    
    protected override void PerformAttack(IDamageable target)
    {
        target.Damage(this);
    }

    protected override void OnDeath()
    {
        _playerController.OnDeath();
        GameManager.Instance.StartSceneTransition("TitleScreen");
    }

    public void FlashText(string text)
    {
        _flashText.text = text;
        StartCoroutine(RemoveTextAfterDelay(5f));
        return;
        
        IEnumerator RemoveTextAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _flashText.text = "";
        }
    }
    
    public GameObject GetPlayerObject() => _playerObject;
    
    public PlayerData GetPlayerData() => _playerData;

    public void PlaySound(PlayerSound sound)
    {
        _audioController.PlaySound(sound);
    }
    
    public override Collider2D GetCollider() => _playerObject.GetComponent<Collider2D>();
    
    protected override Rigidbody2D GetRigidbody() => _playerObject.GetComponent<Rigidbody2D>();

    public Collider2D GetAttackHitboxCollider() => _attackHitboxCollider;
    
    public static Player GetPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        return player?.GetComponent<Player>();
    }
    

}
