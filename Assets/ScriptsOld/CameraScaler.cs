﻿using UnityEngine;

namespace ScriptsOld
{
    public class CameraScaler : MonoBehaviour {

        public float orthographicSize = 5;
        public float aspect = 1.33333f;
        void Start()
        {
            Camera.main.projectionMatrix = Matrix4x4.Ortho(
                -orthographicSize * aspect, orthographicSize * aspect,
                -orthographicSize, orthographicSize,
                Camera.main.nearClipPlane, Camera.main.farClipPlane);
        }
    }
}
