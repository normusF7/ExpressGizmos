using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ExpressGizmos
{
    public readonly struct Gizmo : IDisposable
    {
        public readonly Mesh Mesh;
        public readonly Matrix4x4 Matrix;
        public readonly float EndTime;
        public readonly Color Color;

        public Gizmo(Mesh mesh, Matrix4x4 matrix, Color color, float endTime)
        {
            Mesh = mesh;
            Matrix = matrix;
            EndTime = endTime;
            Color = color;
        }

        public void Dispose()
        {
            Object.Destroy(Mesh);
        }
    }
}
