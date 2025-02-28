﻿namespace Ilumisoft.VisualStateMachine.Editor
{
    using UnityEditor;
    using UnityEngine;

    public class SelectionRect
    {
        private Rect rect = Rect.zero;

        public bool IsActive => rect != Rect.zero;

        public Vector2 Position
        {
            get => rect.position;
            set => rect.position = value;
        }

        public bool Contains(Vector2 point)
        {
            return rect.Contains(point, true);
        }

        public void Reset()
        {
            rect = Rect.zero;
        }

        public void Drag(Vector2 position)
        {
            rect = new Rect(rect.position, position - rect.position);
        }

        public void Draw()
        {
            if (IsActive)
            {
                EditorGUI.DrawRect(this.rect, GraphColors.SelectionRectColor);
            }
        }
    }
}