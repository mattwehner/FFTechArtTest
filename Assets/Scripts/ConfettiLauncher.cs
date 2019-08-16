using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ConfettiLauncher : MonoBehaviour
    {
        [SerializeField]
        private GameObject _confetti;

        [Header("Launcher Settings")]
        [SerializeField]
        private int _confettiAmount = 30;
        [SerializeField]
        private float _launchForce = 10000f;
        [SerializeField]
        private float _randomizeForce = 30f;
        [SerializeField]
        private float _spreadAmount = 100f;


        private AudioSource _launchSound;
        private List<GameObject> _confettiCollection;

        void Awake()
        {
            _launchSound = gameObject.GetComponent<AudioSource>();
        }

        void Start()
        {
            SpawnConfetti();
            LaunchConfetti();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                SpawnConfetti();
                LaunchConfetti();
            }
        }

        private void LaunchConfetti()
        {
            _launchSound.Play();
            foreach (var confetti in _confettiCollection)
            {
                float randomForce = Random.Range(_launchForce - _randomizeForce, _launchForce + _randomizeForce);
                confetti.GetComponent<Rigidbody2D>().AddRelativeForce(confetti.transform.up * randomForce, ForceMode2D.Impulse);
            }
            _launchSound.Stop();
        }

        private void SpawnConfetti()
        {
            _confettiCollection = new List<GameObject>();

            for (int i = 0; i < _confettiAmount; i++)
            {
                GameObject confetti = Instantiate(_confetti, transform.position, Quaternion.identity, transform);
                _confettiCollection.Add(confetti);

                //Assign Random Color
                confetti.GetComponent<Image>().color = Random.ColorHSV(0,1,0.6f,1,1,1);

                //Rotate confetti randomly for spread
                float zRotation = confetti.transform.rotation.z;
                float rotateAmount = Random.Range(zRotation - _spreadAmount, zRotation + _spreadAmount);
                confetti.transform.Rotate(0,0, rotateAmount);
            }
        }
    }
}
