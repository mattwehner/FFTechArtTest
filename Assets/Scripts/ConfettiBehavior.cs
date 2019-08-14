using UnityEngine;

namespace Assets.Scripts
{
    public class ConfettiBehavior : MonoBehaviour
    {
        private Transform _confetti;
        private Rect _screenBounds;
        private bool _startedInBounds;

        void Awake()
        {
            _confetti = gameObject.transform;
            _screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);

            //Set random fall speed
            float drag = _confetti.GetComponent<Rigidbody2D>().drag;
            float randomRange = 0.08f;
            float randomMass = Random.Range(drag - randomRange, drag + randomRange);
            _confetti.GetComponent<Rigidbody2D>().drag = randomMass;
        }

        void Start()
        {
            Vector2 confettiPosition = _confetti.position;
            _startedInBounds = _screenBounds.Contains(confettiPosition);
        }

        void Update ()
        {
            //TODO: Rotate to face down over time

            //Destroy if off screen
            Vector2 confettiPosition = _confetti.position;
            if (!_screenBounds.Contains(confettiPosition))
            {
                Destroy(_confetti.gameObject);
            }
        }
    }
}
