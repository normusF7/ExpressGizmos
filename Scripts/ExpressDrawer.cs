using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ExpressGizmos
{
    internal class ExpressDrawer
    {
        private readonly Material _material;
        private readonly List<Gizmo> _gizmos = new();
        private readonly MaterialPropertyBlock _propertyBlock = new();
        private static readonly int ColorPropertyID = Shader.PropertyToID("_Color");
        private const string LineMaterialPath = "ExpressGizmos/Line";

        public ExpressDrawer()
        {
            _material = Resources.Load<Material>(LineMaterialPath);
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        }

        public void AddCubeGizmo(Vector3 position, Vector3 size, Quaternion rotation, Color color, float endTime)
        {
            var mesh = GizmoMeshCreator.CreateWireframeCube(1);
            _gizmos.Add(new Gizmo(mesh, Matrix4x4.TRS(position, rotation, size), color, endTime));
        }

        public void AddSphereGizmo(Vector3 position, float radius, int segments, Color color, float endTime)
        {
            var mesh = GizmoMeshCreator.CreateWireframeSphere(segments, radius);
            _gizmos.Add(new Gizmo(mesh, Matrix4x4.TRS(position, Quaternion.identity, Vector3.one * radius), color,
                endTime));
        }

        public void AddConeGizmo(float radius, float height, int segments, Vector3 position, Quaternion rotation, Color color, float endTime)
        {
            var mesh = GizmoMeshCreator.CreateWireframeCone(segments, height, radius);
            _gizmos.Add(new Gizmo(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), color, endTime));
        }

        public void AddCapsuleGizmo(Vector3 position, Quaternion rotation, float radius, float height, int segments, Color color, float endTime)
        {
            var mesh = GizmoMeshCreator.CreateWireframeCapsule(segments, height, radius);
            _gizmos.Add(new Gizmo(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), color, endTime));
        }

        public void AddDiscGizmo(float radius, int segments, int rings, Vector3 position, Quaternion rotation, Color color, float endTime)
        {
            var mesh = GizmoMeshCreator.CreateWireframeDisc(segments, radius, rings);
            _gizmos.Add(new Gizmo(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), color, endTime));
        }

        public void AddCylinderGizmo(Vector3 position, Quaternion rotation, float radius, float height, int segments, Color color, float endTime)
        {
            var mesh = GizmoMeshCreator.CreateWireframeCylinder(segments, height, radius);
            _gizmos.Add(new Gizmo(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), color, endTime));
        }

        public void AddLineGizmo(Vector3 start, Vector3 end, Color color, float endTime)
        {
            var mesh = GizmoMeshCreator.CreateLine(start, end);
            _gizmos.Add(new Gizmo(mesh, Matrix4x4.identity, color, endTime));
        }

        public void AddArrowGizmo(Vector3 start, Vector3 direction, float length, Color color, float endTime)
        {
            var mesh = GizmoMeshCreator.CreateArrow(start, direction, length, length / 8, length / 12, 12, color);
            _gizmos.Add(new Gizmo(mesh, Matrix4x4.identity, color, endTime));
        }

        private void OnBeginCameraRendering(ScriptableRenderContext _, Camera camera)
        {
            for (var i = _gizmos.Count - 1; i >= 0; i--)
            {
                _propertyBlock.SetColor(ColorPropertyID, _gizmos[i].Color);
                Graphics.DrawMesh(_gizmos[i].Mesh, _gizmos[i].Matrix, _material, 0, camera, 0, _propertyBlock);

                if (Time.time >= _gizmos[i].EndTime)
                {
                    _gizmos[i].Dispose();
                    _gizmos.RemoveAt(i);
                }
            }
        }
    }
}
