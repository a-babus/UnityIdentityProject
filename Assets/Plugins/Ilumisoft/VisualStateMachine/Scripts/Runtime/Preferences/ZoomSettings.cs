﻿namespace Ilumisoft.VisualStateMachine
{
    using UnityEngine;

    [System.Serializable]
    public class ZoomSettings 
    {
        public static readonly float MinZoomFactor = 0.5f;
        public static readonly float MaxZoomFactor = 2.0f;

        [SerializeField]
        private float zoomFactor = 1.0f;

        public float ZoomFactor
        {
            get => this.zoomFactor;
            set
            {
                this.zoomFactor = Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor);
            }
        }
    }
}