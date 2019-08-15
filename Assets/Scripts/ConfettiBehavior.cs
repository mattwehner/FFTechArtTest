using UnityEngine;

namespace Assets.Scripts
{
    public class ConfettiBehavior : MonoBehaviour
    {
        private Transform _confetti;
        private Rect _screenBounds;

        void Awake()
        {
            _confetti = gameObject.transform;
            _screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);

            //Set random fall speed
            Rigidbody2D rb = _confetti.GetComponent<Rigidbody2D>();
            rb.drag = Random.Range(rb.drag - 0.1f, rb.drag + 0.1f);
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
