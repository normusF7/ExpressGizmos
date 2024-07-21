using System.Collections.Generic;
using UnityEngine;

namespace ExpressGizmos
{
    public static class Utils
    {
        public static Mesh CombineLineMeshes(Mesh[] meshes, Matrix4x4[] transforms)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var colors = new List<Color>();

            var vertexOffset = 0;

            for (var i = 0; i < meshes.Length; i++)
            {
                var mesh = meshes[i];
                var transform = transforms[i];
                
                var meshVertices = mesh.vertices;
                for (var j = 0; j < meshVertices.Length; j++)
                {
                    vertices.Add(transform.MultiplyPoint3x4(meshVertices[j]));
                }
                
                var meshIndices = mesh.GetIndices(0);
                for (var j = 0; j < meshIndices.Length; j++)
                {
                    indices.Add(meshIndices[j] + vertexOffset);
                }
                
                if (mesh.colors != null && mesh.colors.Length > 0)
                {
                    colors.AddRange(mesh.colors);
                }
                else
                {
                    for (var j = 0; j < meshVertices.Length; j++)
                    {
                        colors.Add(Color.white);
                    }
                }

                vertexOffset += meshVertices.Length;
            }

            var combinedMesh = new Mesh();
            combinedMesh.SetVertices(vertices);
            combinedMesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            combinedMesh.SetColors(colors);

            return combinedMesh;
        }
    }
}