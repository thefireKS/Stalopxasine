using UnityEngine;
using UnityEngine.Serialization;

public class ParallaxObject : MonoBehaviour
{
    public float parallaxScaleHorizontal = 1f, parallaxScaleVertical = 1f;
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
        float parallaxHorizontal = (_previousCamPos.x - camPosition.x) * parallaxScaleHorizontal;
        float parallaxVertical = (_previousCamPos.y - camPosition.y) * parallaxScaleVertical;
        var myTransform = transform;
        var position = myTransform.position;
        float backgroundTargetX = position.x + parallaxHorizontal;
        float backgroundTargetY = position.y + parallaxHorizontal;
        Vector3 backgroundTargetPos = new Vector3(backgroundTargetX, backgroundTargetY, position.z);
        transform.position = Vector3.Lerp(position, backgroundTargetPos, smoothing * Time.deltaTime);
        _previousCamPos = camPosition;
    }
}