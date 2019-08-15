using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 ToWorldPosition(this Vector3 point, Camera camera)
        {
            Vector2 mousePos = new Vector2
            {
                x = point.x,
                y = point.y
            };

            Vector3 worldPoint = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.farClipPlane));

            return worldPoint;
        }
    }
}
