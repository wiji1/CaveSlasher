using System;
using UnityEngine;

public class PlayerListener : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = Player.GetPlayer();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionObject = collision.gameObject;
        if (collisionObject.CompareTag("Coin")) CollectCoin(collisionObject);
    }
    
    public void OnTriggerEnter2D(Collider2D collider)
    {
        var collisionObject = collider.gameObject;
        if (collisionObject.CompareTag("Coin")) CollectCoin(collisionObject);
    }
    
    private void CollectCoin(GameObject coin)
    {
        _player.GetPlayerData().Coins += 1;
        GameUIController.UpdateCoins(_player.GetPlayerData().Coins);
        
        _player.PlaySound(PlayerSound.CoinPickup);
        
        Destroy(coin);
    }
}
