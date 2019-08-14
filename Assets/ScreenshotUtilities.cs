using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class ScreenshotUtilities : MonoBehaviour
    {
        [HideInInspector]
        public static string ScreenshotPath;
        
        private static Camera _camera;
        private static Image _imageDisplay;
        private static bool _captureScreenshot = false;
        [HideInInspector]
        public static bool _screenshotCaptured = false;

        private void Awake()
        {
            _camera = gameObject.GetComponent<Camera>();
        }

        void OnPostRender () {
            if (_captureScreenshot)
            {
                _captureScreenshot = false;
                ScreenshotPath = SaveScreenshotToFile();
                _screenshotCaptured = true;
            }
        }

        public static void Capture(int width, int height, Image renderTo)
        {
            _screenshotCaptured = false;
            _camera.targetTexture = RenderTexture.GetTemporary(width, height, 32);
            _imageDisplay = renderTo;
            _captureScreenshot = true;
        }

        private string SaveScreenshotToFile()
        {
            RenderTexture textureFromCamera = _camera.targetTexture;
            Texture2D render = new Texture2D(textureFromCamera.width, textureFromCamera.height, TextureFormat.ARGB32, false);
            Rect frame = new Rect(0,0, render.width, render.height);
            render.ReadPixels(frame, 0, 0);

            string filePath = Application.dataPath + "/screenshots/screentshot_" + DateTime.Now.ToString("MMddyyyTHHmm") + ".png";
            byte[] byteArray = render.EncodeToPNG();

            //Save file
            System.IO.File.WriteAllBytes(filePath, byteArray);
            Debug.Log("Screenshot saved to: "+ filePath);

            //RenderToImage

            if (_imageDisplay != null)
            {
                render.Apply();
                _imageDisplay.sprite = Sprite.Create(render, frame, new  Vector2(0,0));
            }

            //Cleanup
            RenderTexture.ReleaseTemporary(textureFromCamera);
            _camera.targetTexture = null;

            return filePath;
        }
    }
}
