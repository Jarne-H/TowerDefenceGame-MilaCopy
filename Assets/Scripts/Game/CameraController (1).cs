using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform tracker;
    [SerializeField] Vector2 yLimit;
    float _xRot = 0f;
    float _yRot = 0f;
    float _cameraDistance = 100;

    [SerializeField] Vector2 _offset = Vector2.zero;
    bool _lerpBack;

    bool _disable;
    public void Disable(bool v)
    {
        _disable = v;
    }

    private void Start()
    {
        transform.position = tracker ? tracker.position : Vector3.zero;
        _xRot = transform.eulerAngles.y;
        _yRot = transform.GetChild(0).localEulerAngles.x;
    }

    void Update()
    {
        _cameraDistance -= Input.mouseScrollDelta.y;
        _cameraDistance = Mathf.Clamp(_cameraDistance, 10, 24);
        
        if (!_disable && Input.GetMouseButton(1))
        {
            _xRot += Input.GetAxis("Mouse X") * 1.5f;
            _yRot = Mathf.Clamp(_yRot - Input.GetAxis("Mouse Y") * 1.5f, yLimit.x, yLimit.y);
            
        }
        if (!_disable && Input.GetMouseButton(2))
        {
            _offset.x += Input.GetAxis("Mouse X") * 0.5f;
            _offset.y += Input.GetAxis("Mouse Y") * 0.5f;
        }
        if(!_disable && Input.GetKeyDown(KeyCode.Space))
            _lerpBack = true;

        if (_lerpBack && _offset != Vector2.zero)
        {
            _offset = Vector2.MoveTowards(_offset,Vector2.zero, Time.deltaTime * 3);
        } else
        {
            _lerpBack = false;
        }

        Camera.main.transform.localPosition = new Vector3(-_offset.x,-_offset.y,-_cameraDistance);
        transform.rotation = Quaternion.Euler(0, _xRot, 0);
        transform.GetChild(0).localRotation = Quaternion.Euler(_yRot, 0, 0);
        transform.position = Vector3.Lerp(transform.position, tracker ? tracker.position: Vector3.zero, .6f * Time.deltaTime);
    }
}
