using System.Collections;
using Player;
using UnityEngine;

namespace Cinemachine
{
    public class SmoothRotate : MonoBehaviour
    {
        [Header("References")]
        private Controller _controller;
        
        [Header("Flip Rotation Stats")]
        [SerializeField] private float flipRotationTime;

        private Coroutine _turnCoroutine;

        private bool _isFacingRight;

        private void Awake()
        {
            InitializePlayer.OnInitialized += Initialize;
            Controller.OnTurned += CallTurn;
        }

        private void OnDisable()
        {
            InitializePlayer.OnInitialized -= Initialize;
            Controller.OnTurned -= CallTurn;
        }
        
        private IEnumerator FlipLerp()
        {
            float startRotation = transform.localEulerAngles.y;
            float endRotationAmount = DetermineRotation();

            float yRotation = 0;
            float elapsedTime = 0f;

            while (elapsedTime < flipRotationTime)
            {
                elapsedTime += Time.deltaTime;

                yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipRotationTime));
                transform.rotation = Quaternion.Euler(0f,yRotation,0f);

                yield return null;
            }
        }

        private void CallTurn()
        {
            if (_turnCoroutine != null)
            {
                StopCoroutine(_turnCoroutine);
            }
            _turnCoroutine = StartCoroutine(FlipLerp());
        }

        private float DetermineRotation()
        {
            _isFacingRight = !_isFacingRight;
            return _isFacingRight ? 0 : 180f;
        }

        private void Initialize()
        {
            _controller = FindObjectOfType<Controller>();
            Debug.Log("Cinemachine.SmoothRotate: Find controller");
            _isFacingRight = _controller._isGoingRight;
        }
    }
}
