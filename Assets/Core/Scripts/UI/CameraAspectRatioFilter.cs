﻿using UnityEngine;

namespace Minofall.UI
{
    [RequireComponent(typeof(Camera))]
    public class CameraAspectRatioFilter : MonoBehaviour
    {
        [SerializeField] private float targetAspectRatio = 9f/16f;

        private Camera _camera;

        void Start()
        {
            _camera = GetComponent<Camera>();
            SetCameraAspect();
        }

        void SetCameraAspect()
        {
            float windowAspect = (float)Screen.width / Screen.height;
            float scaleHeight = windowAspect / targetAspectRatio;

            if (scaleHeight < 1.0f)
            {
                // Letterboxing
                Rect rect = _camera.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0.0f;
                rect.y = (1.0f - scaleHeight) / 2.0f;

                _camera.rect = rect;
            }
            else
            {
                //// Pillarboxing
                //float scaleWidth = 1.0f / scaleHeight;

                //Rect rect = _camera.rect;

                //rect.width = scaleWidth;
                //rect.height = 1.0f;
                //rect.x = (1.0f - scaleWidth) / 2.0f;
                //rect.y = 0;

                //_camera.rect = rect;
                _camera.rect = new(0.0f, 0.0f, 1.0f, 1.0f);
            }
        }

        private void Update()
        {
            SetCameraAspect();
        }
    }
}