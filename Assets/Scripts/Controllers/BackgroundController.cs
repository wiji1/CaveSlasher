using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float _startPos;
    private float _length;
    private float _initialCameraX;
    
    [Header("Scrolling Settings")]
    public float scrollSpeed = 2f;
    public bool autoScroll = true;
    
    [Header("Parallax Settings")]
    public new GameObject camera;
    public float parallaxEffect = 0.5f;
    
    private void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _initialCameraX = camera.transform.position.x;
    }

    private void FixedUpdate()
    {
        if (autoScroll)
        {
            var moveSpeed = scrollSpeed * parallaxEffect;
            transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
            
            if (transform.position.x <= _startPos - _length)
                transform.position = new Vector3(_startPos, transform.position.y, transform.position.z);
        }
        else
        {
            var effectiveCameraPosition = camera.transform.position.x - _initialCameraX;
            var distance = effectiveCameraPosition * parallaxEffect;
            var movement = effectiveCameraPosition * (1 - parallaxEffect);
            
            transform.position = new Vector3(_startPos + distance, transform.position.y, transform.position.z);
            
            if (movement > _startPos + _length) _startPos += _length;
            else if (movement < _startPos - _length) _startPos -= _length;
        }
    }
}