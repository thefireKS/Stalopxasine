using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    public float parallaxScale = 1f;
    public float smoothing = 1f;

    private Transform _cam;
    private Vector3 _previousCamPos;

    private void Awake()
    {
        if (Camera.main != null) _cam = Camera.main.transform;
    }

    private void Start()
    {
        _previousCamPos = _cam.position;
    }

    private void Update()
    {
        var camPosition = _cam.position;
        float parallax = (_previousCamPos.x - camPosition.x) * parallaxScale;
        var myTransform = transform;
        var position = myTransform.position;
        float backgroundTargetX = position.x + parallax;
        Vector3 backgroundTargetPos = new Vector3(backgroundTargetX, position.y, position.z);
        transform.position = Vector3.Lerp(position, backgroundTargetPos, smoothing * Time.deltaTime);
        _previousCamPos = camPosition;
    }
}