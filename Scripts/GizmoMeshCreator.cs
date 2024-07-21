using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace ExpressGizmos
{
    internal static class GizmoMeshCreator
    {
        public static Mesh CreateWireframeCube(float size)
        {
            var mesh = new Mesh();

            const int vertexCount = 8;
            const int indexCount = 24;
            
            var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3);
            mesh.SetVertexBufferParams(vertexCount, vertexAttributes);
            vertexAttributes.Dispose();
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
            
            var vertices = new NativeArray<Vector3>(vertexCount, Allocator.Temp);
            var halfSize = size / 2;
            vertices[0] = new Vector3(-halfSize, -halfSize, -halfSize);
            vertices[1] = new Vector3(halfSize, -halfSize, -halfSize);
            vertices[2] = new Vector3(halfSize, halfSize, -halfSize);
            vertices[3] = new Vector3(-halfSize, halfSize, -halfSize);
            vertices[4] = new Vector3(-halfSize, -halfSize, halfSize);
            vertices[5] = new Vector3(halfSize, -halfSize, halfSize);
            vertices[6] = new Vector3(halfSize, halfSize, halfSize);
            vertices[7] = new Vector3(-halfSize, halfSize, halfSize);
            mesh.SetVertexBufferData(vertices, 0, 0, vertexCount);
            vertices.Dispose();
            
            var indices = new NativeArray<uint>(indexCount, Allocator.Temp);
            
            // Bottom face
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 1;
            indices[3] = 2;
            indices[4] = 2;
            indices[5] = 3;
            indices[6] = 3;
            indices[7] = 0;
            
            // Top face
            indices[8] = 4;
            indices[9] = 5;
            indices[10] = 5;
            indices[11] = 6;
            indices[12] = 6;
            indices[13] = 7;
            indices[14] = 7;
            indices[15] = 4;
            
            // Connecting edges
            indices[16] = 0;
            indices[17] = 4;
            indices[18] = 1;
            indices[19] = 5;
            indices[20] = 2;
            indices[21] = 6;
            indices[22] = 3;
            indices[23] = 7;
            
            mesh.SetIndexBufferData(indices, 0, 0, indexCount);
            indices.Dispose();
            mesh.bounds = new Bounds(Vector3.zero, Vector3.one * size);
            mesh.SetSubMesh(0, new SubMeshDescriptor(0, indexCount, MeshTopology.Lines));

            return mesh;
        }

        public static Mesh CreateLine(Vector3 start, Vector3 end)
        {
            var mesh = new Mesh();

            var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3);
            mesh.SetVertexBufferParams(2, vertexAttributes);
            vertexAttributes.Dispose();

            mesh.SetIndexBufferParams(2, IndexFormat.UInt16);

            var indices = new NativeArray<ushort>(2, Allocator.Temp);
            var vertices = new NativeArray<Vector3>(2, Allocator.Temp);
           
            vertices[0] = start;
            vertices[1] = end;

            indices[0] = 0;
            indices[1] = 1;

            mesh.SetVertexBufferData(vertices, 0, 0, 2);
            mesh.SetIndexBufferData(indices, 0, 0, 2);
            indices.Dispose();
            vertices.Dispose();

            mesh.SetSubMesh(0, new SubMeshDescriptor(0, 2, MeshTopology.Lines));

            return mesh;
        }

        public static Mesh CreateWireframeSphere(int segments, float radius)
        {
            var vertexCount = (segments + 1) * (segments + 1);
            var indexCount = segments * segments * 8;

            var mesh = new Mesh();
            var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position);
            mesh.SetVertexBufferParams(vertexCount, vertexAttributes);
            vertexAttributes.Dispose();
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt16);
            
            var vertices = new NativeArray<Vector3>(vertexCount, Allocator.Temp);
            var vertexIndex = 0;
            for (var y = 0; y <= segments; y++)
            {
                var v = y / (float)segments;
                var latitude = Mathf.PI * (v - 0.5f);

                for (var x = 0; x <= segments; x++)
                {
                    var u = x / (float)segments;
                    var longitude = 2 * Mathf.PI * u;

                    var xPos = Mathf.Cos(longitude) * Mathf.Cos(latitude);
                    var yPos = Mathf.Sin(latitude);
                    var zPos = Mathf.Sin(longitude) * Mathf.Cos(latitude);

                    vertices[vertexIndex++] = new Vector3(xPos, yPos, zPos) * radius;
                }
            }

            mesh.SetVertexBufferData(vertices, 0, 0, vertexCount);
            vertices.Dispose();
            
            var indices = new NativeArray<ushort>(indexCount, Allocator.Temp);
            var indexBufferIndex = 0;
            for (ushort y = 0; y < segments; y++)
            {
                for (var x = 0; x < segments; x++)
                {
                    var index0 = (ushort)(y * (segments + 1) + x);
                    var index1 = (ushort)(y * (segments + 1) + x + 1);
                    var index2 = (ushort)((y + 1) * (segments + 1) + x);
                    var index3 = (ushort)((y + 1) * (segments + 1) + x + 1);

                    // Vertical lines
                    indices[indexBufferIndex++] = index0;
                    indices[indexBufferIndex++] = index2;

                    // Horizontal lines
                    indices[indexBufferIndex++] = index0;
                    indices[indexBufferIndex++] = index1;

                    // Additional lines to complete the grid
                    indices[indexBufferIndex++] = index2;
                    indices[indexBufferIndex++] = index3;
                    indices[indexBufferIndex++] = index1;
                    indices[indexBufferIndex++] = index3;
                }
            }

            mesh.SetIndexBufferData(indices, 0, 0, indexCount);
            indices.Dispose();
            mesh.bounds = new Bounds(Vector3.zero, Vector3.one * radius * 2);
            mesh.SetSubMesh(0, new SubMeshDescriptor(0, indexCount, MeshTopology.Lines));

            return mesh;
        }

        public static Mesh CreateWireframeCone(int segments, float height, float baseRadius)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();

            // Add the apex of the cone
            vertices.Add(new Vector3(0, height, 0));

            // Generate base vertices
            for (var i = 0; i <= segments; i++)
            {
                var angle = 2 * Mathf.PI * i / segments;
                var x = Mathf.Cos(angle) * baseRadius;
                var z = Mathf.Sin(angle) * baseRadius;
                vertices.Add(new Vector3(x, 0, z));
            }

            // Generate indices for lines
            for (int i = 1; i <= segments; i++)
            {
                // Line from apex to base point
                indices.Add(0);
                indices.Add(i);

                // Line along the base
                indices.Add(i);
                indices.Add(i == segments ? 1 : i + 1);
            }

            var mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);

            return mesh;
        }

        public static Mesh CreateWireframeCylinder(int segments, float height, float radius)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();

            // Generate vertices for top and bottom circles
            for (var y = 0; y <= 1; y++)
            {
                var yPos = y * height;
                for (var i = 0; i <= segments; i++)
                {
                    var angle = 2 * Mathf.PI * i / segments;
                    var x = Mathf.Cos(angle) * radius;
                    var z = Mathf.Sin(angle) * radius;
                    vertices.Add(new Vector3(x, yPos, z));
                }
            }
            
            for (var i = 0; i <= segments; i++)
            {
                // Vertical lines
                indices.Add(i);
                indices.Add(i + segments + 1);

                // Lines along the bases
                if (i < segments)
                {
                    // Bottom base
                    indices.Add(i);
                    indices.Add(i + 1);

                    // Top base
                    indices.Add(i + segments + 1);
                    indices.Add(i + segments + 2);
                }
            }

            var mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);

            return mesh;
        }

        public static Mesh CreateWireframeDisc(int segments, float radius, int rings = 4)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();

            // Add center vertex
            vertices.Add(Vector3.zero);
            
            for (var r = 1; r <= rings; r++)
            {
                var currentRadius = radius * r / rings;
                for (int i = 0; i < segments; i++)
                {
                    var angle = 2 * Mathf.PI * i / segments;
                    var x = Mathf.Cos(angle) * currentRadius;
                    var z = Mathf.Sin(angle) * currentRadius;
                    vertices.Add(new Vector3(x, 0, z));
                }
            }
            
            for (var i = 0; i < segments; i++)
            {
                indices.Add(0);
                indices.Add(i + 1);
            }

            // Generate indices for concentric circles
            for (var r = 1; r <= rings; r++)
            {
                var startIndex = (r - 1) * segments + 1;
                for (int i = 0; i < segments; i++)
                {
                    var current = startIndex + i;
                    var next = (i == segments - 1) ? startIndex : current + 1;
                    indices.Add(current);
                    indices.Add(next);
                }
            }

            var mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);

            return mesh;
        }

        public static Mesh CreateWireframeCapsule(int segments, float height, float radius)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();

            // Generate vertices
            for (var y = 0; y <= segments; y++)
            {
                var v = y / (float)segments;
                float yPos;
                float latitude;

                if (v <= 0.25f) // Bottom hemisphere
                {
                    latitude = Mathf.PI * (v * 2 - 0.5f);
                    yPos = Mathf.Sin(latitude) * radius - height / 2;
                }
                else if (v >= 0.75f) // Top hemisphere
                {
                    latitude = Mathf.PI * ((v - 0.75f) * 2);
                    yPos = Mathf.Sin(latitude) * radius + height / 2;
                }
                else // Cylinder
                {
                    yPos = (v - 0.5f) * height;
                    latitude = 0;
                }

                for (var x = 0; x <= segments; x++)
                {
                    var u = x / (float)segments;
                    var longitude = 2 * Mathf.PI * u;

                    var xPos = Mathf.Cos(longitude) * Mathf.Cos(latitude);
                    var zPos = Mathf.Sin(longitude) * Mathf.Cos(latitude);

                    if (v > 0.25f && v < 0.75f)
                    {
                        xPos = Mathf.Cos(longitude);
                        zPos = Mathf.Sin(longitude);
                    }

                    vertices.Add(new Vector3(xPos * radius, yPos, zPos * radius));
                }
            }
            
            for (var y = 0; y < segments; y++)
            {
                for (var x = 0; x < segments; x++)
                {
                    var isCylinderSection = (y > segments / 4) && (y < 3 * segments / 4);

                    var index0 = y * (segments + 1) + x;
                    var index1 = y * (segments + 1) + x + 1;
                    var index2 = (y + 1) * (segments + 1) + x;
                    var index3 = (y + 1) * (segments + 1) + x + 1;

                    // Vertical lines
                    indices.Add(index0);
                    indices.Add(index2);

                    if (isCylinderSection)
                    {
                        continue;
                    }

                    // Horizontal lines
                    indices.Add(index0);
                    indices.Add(index1);

                    indices.Add(index2);
                    indices.Add(index3);

                    indices.Add(index1);
                    indices.Add(index3);
                }
            }

            var mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);

            return mesh;
        }

        public static Mesh CreateArrow(Vector3 position, Vector3 direction, float length, float headLength, float headWidth, int segments, Color color)
        {
            direction = direction.normalized;
            var arrowTip = position + direction * length;
            var lineTip = arrowTip - direction * headLength;
            var lineMesh = CreateLine(position, lineTip);
            var coneMesh = CreateWireframeCone(segments, headLength, headWidth);
            var lineTransform = Matrix4x4.identity;
            var coneRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
            var coneTransform = Matrix4x4.TRS(lineTip, coneRotation, Vector3.one);

            var combinedMesh = Utils.CombineLineMeshes(
                new[] { lineMesh, coneMesh },
                new[] { lineTransform, coneTransform }
            );
            
            var colors = new Color[combinedMesh.vertexCount];
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            combinedMesh.colors = colors;

            return combinedMesh;
        }
    }
}