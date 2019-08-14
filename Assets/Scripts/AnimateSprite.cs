using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AnimateSprite : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] _spriteCollection;

        private int _currentFrame;
        private float _timer;
        private float _framerate = .1f;

        private Image _image;

        void Awake()
        {
            _image = gameObject.GetComponent<Image>();
        }

        void Update ()
        {
            _timer += Time.deltaTime;

            if (_timer >= _framerate)
            {
                _timer -= _framerate;
                _currentFrame = ++_currentFrame % _spriteCollection.Length;

                _image.sprite = _spriteCollection[_currentFrame];
            }
        }
    }
}
