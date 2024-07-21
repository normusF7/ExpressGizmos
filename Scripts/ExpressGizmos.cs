using UnityEngine;

namespace ExpressGizmos
{
    public static class ExpressDebug
    {
        private static readonly ExpressDrawer _drawer = new();
        
        public static void DrawSphere(Vector3 position, float radius, int segments, Color color, float duration = 0.0f)
        {
            _drawer.AddSphereGizmo(position, radius, segments, color, Time.time + duration);
        }
        
        public static void DrawSphere(Vector3 position, float radius, Color color, float duration = 0.0f)
        {
            _drawer.AddSphereGizmo(position, radius, 14, color, Time.time + duration);
        }
        
        public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 size, Color color, float duration = 0.0f)
        {
            _drawer.AddCubeGizmo(position, size, rotation, color, Time.time + duration);
        }
        
        public static void DrawCube(Vector3 position, Vector3 size, Color color, float duration = 0.0f)
        {
            _drawer.AddCubeGizmo(position, size, Quaternion.identity, color, Time.time + duration);
        }
        
        public static void DrawCube(Vector3 position, float size, Color color, float duration = 0.0f)
        {
            _drawer.AddCubeGizmo(position, Vector3.one * size, Quaternion.identity, color, Time.time + duration);
        }
        
        public static void DrawCapsule(Vector3 position, Quaternion rotation, float radius, float height, int segments, Color color, float duration = 0.0f)
        {
            _drawer.AddCapsuleGizmo(position, rotation, radius, height, segments, color, Time.time + duration);
        }
        
        public static void DrawCapsule(Vector3 position, float radius, float height, int segments, Color color, float duration = 0.0f)
        {
            _drawer.AddCapsuleGizmo(position, Quaternion.identity, radius, height, segments, color, Time.time + duration);
        }
        
        public static void DrawCapsule(Vector3 position, float radius, float height, Color color, float duration = 0.0f)
        {
            _drawer.AddCapsuleGizmo(position, Quaternion.identity, radius, height, 14, color, Time.time + duration);
        }
        
        public static void DrawCylinder(Vector3 position, Quaternion rotation, float radius, float height, int segments, Color color, float duration = 0.0f)
        {
            _drawer.AddCylinderGizmo(position, rotation, radius, height, segments, color, Time.time + duration);
        }
        
        public static void DrawCylinder(Vector3 position, float radius, float height, int segments, float duration = 0.0f, Color color = default)
        {
            _drawer.AddCylinderGizmo(position, Quaternion.identity, radius, height, segments, color, Time.time + duration);
        }
        
        public static void DrawCylinder(Vector3 position, float radius, float height, Color color)
        {
            _drawer.AddCylinderGizmo(position, Quaternion.identity, radius, height, 12, color, Time.time);
        }
        
        public static void DrawDisc(Vector3 position, Quaternion rotation, float radius, int segments, Color color, float duration = 0.0f)
        {
            _drawer.AddDiscGizmo(radius, segments, 1, position, rotation, color, Time.time + duration);
        }
        
        public static void DrawDisc(Vector3 position, Quaternion rotation, float radius, Color color, float duration = 0.0f)
        {
            _drawer.AddDiscGizmo(radius, 12, 1, position, rotation, color, Time.time + duration);
        }
        
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.0f)
        {
            _drawer.AddLineGizmo(start, end, color, Time.time + duration);
        }
        
        public static void DrawRay(Vector3 start, Vector3 direction, Color color, float duration = 0.0f)
        {
            _drawer.AddLineGizmo(start,  start + direction, color, Time.time + duration);
        }
        
        public static void DrawArrow(Vector3 start, Vector3 direction, float length, Color color, float duration = 0.0f)
        {
            _drawer.AddArrowGizmo(start, direction, length, color, Time.time + duration);
        }
    }
}
