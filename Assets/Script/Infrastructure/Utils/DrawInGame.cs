using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Utilities
{
    public sealed class DrawInGame : MonoBehaviour
    {
        private static readonly int _zWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int _zTest = Shader.PropertyToID("_ZTest");
        private static readonly List<Record> s_records = new(256);
        private static Material s_material;
        private Camera _camera;

        private void Awake()
        {
            s_material = CreateMaterial();
            if (!TryGetComponent(out _camera))
            {
                Debug.LogError($"{nameof(DrawInGame)} script needs to be placed on object with Camera component");
            }
        }

        private void OnEnable()
        {
            if (_camera != null)
                RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        private void OnDisable()
        {
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }

        private void OnEndCameraRendering(ScriptableRenderContext context, Camera renderCamera)
        {
            if (renderCamera != _camera)
                return;

            float time = Time.timeSinceLevelLoad;

            for (int i = 0; i < s_records.Count; i++)
            {
                Record record = s_records[i];
                DrawRecord(record);
                if (record.TimeToComplete <= time)
                {
                    s_records.RemoveAt(i--);
                }
            }
        }

        private void OnDestroy()
        {
            Clear();
        }

        private static Material CreateMaterial()
        {
            Material material = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            material.SetInt(_zWrite, 0);
            material.SetInt(_zTest, (int) CompareFunction.Always);
            return material;
        }

        public static void WireBox(Vector3 center, Quaternion rotation, Vector3 size, Color color, float duration = 0f)
        {
            DrawBox(center, rotation, size, color, EType.WireBox, duration);
        }

        public static void Box(Vector3 center, Quaternion rotation, Vector3 size, Color color, float duration = 0f)
        {
            DrawBox(center, rotation, size, color, EType.Box, duration);
        }

        public static void WireCircle(Vector3 center, Quaternion rotation, float radius, Color color, float duration = 0f)
        {
            DrawCircle(center, rotation, radius, color, EType.WireCircle, duration);
        }

        public static void Circle(Vector3 center, Quaternion rotation, float radius, Color color, float duration = 0f)
        {
            DrawCircle(center, rotation, radius, color, EType.Circle, duration);
        }

        private static void DrawBox(Vector3 center, Quaternion rotation, Vector3 size, Color color, EType type, float duration = 0f)
        {
            var record = new Record()
            {
                Type = type,
                Matrix = Matrix4x4.TRS(center, rotation, size),
                Color = color,
                TimeToComplete = Time.timeSinceLevelLoad + duration,
            };

            s_records.Add(record);
        }

        private static void DrawCircle(Vector3 center, Quaternion rotation, float radius, Color color, EType type, float duration = 0f)
        {
            var record = new Record()
            {
                Type = type,
                Matrix = Matrix4x4.TRS(center, rotation, Vector3.one),
                Radius = radius,                                       
                Color = color,
                TimeToComplete = Time.timeSinceLevelLoad + duration,
            };

            s_records.Add(record);
        }

        private static void Clear()
        {
            s_records.Clear();
        }

        private static void DrawRecord(Record record)
        {
            s_material.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(record.Matrix);

            switch (record.Type)
            {
                case EType.None:
                    break;
                case EType.WireBox:
                    GL.Begin(GL.LINES);
                    GL.Color(record.Color);
                    DrawWireBox(new Bounds(Vector3.zero, Vector3.one));
                    GL.End();
                    break;
                case EType.Box:
                    GL.Begin(GL.QUADS);
                    GL.Color(record.Color);
                    DrawBox(new Bounds(Vector3.zero, Vector3.one));
                    GL.End();
                    break;
                case EType.WireCircle:
                    GL.Begin(GL.LINES);
                    GL.Color(record.Color);
                    DrawWireCircleGeometry(64, record.Radius);
                    GL.End();
                    break;
                case EType.Circle:
                    GL.Begin(GL.TRIANGLES);
                    GL.Color(record.Color);
                    DrawCircleGeometry(64, record.Radius);
                    GL.End();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GL.PopMatrix();
        }

        private static void DrawCircleGeometry(int segments, float radius)
        {
            float angleStep = 2f * Mathf.PI / segments;
            Vector3 center = Vector3.zero;

            for (int i = 0; i < segments; i++)
            {
                float angle1 = i * angleStep;
                float angle2 = (i + 1) * angleStep;

                Vector3 p1 = new Vector3(Mathf.Cos(angle1) * radius, 0f, Mathf.Sin(angle1) * radius);
                Vector3 p2 = new Vector3(Mathf.Cos(angle2) * radius, 0f, Mathf.Sin(angle2) * radius);

                GL.Vertex(center);
                GL.Vertex(p1);
                GL.Vertex(p2);
            }
        }

        private static void DrawWireCircleGeometry(int segments, float radius)
        {
            float angleStep = 2f * Mathf.PI / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle1 = i * angleStep;
                float angle2 = (i + 1) * angleStep;

                Vector3 p1 = new Vector3(Mathf.Cos(angle1) * radius, 0f, Mathf.Sin(angle1) * radius);
                Vector3 p2 = new Vector3(Mathf.Cos(angle2) * radius, 0f, Mathf.Sin(angle2) * radius);

                GL.Vertex(p1);
                GL.Vertex(p2);
            }
        }

        private static void DrawWireBox(Bounds bounds)
        {
            var min = bounds.min;
            var max = bounds.max;

            DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z));
            DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(max.x, min.y, max.z));
            DrawLine(new Vector3(max.x, min.y, max.z), new Vector3(min.x, min.y, max.z));
            DrawLine(new Vector3(min.x, min.y, max.z), new Vector3(min.x, min.y, min.z));

            DrawLine(new Vector3(min.x, max.y, min.z), new Vector3(max.x, max.y, min.z));
            DrawLine(new Vector3(max.x, max.y, min.z), new Vector3(max.x, max.y, max.z));
            DrawLine(new Vector3(max.x, max.y, max.z), new Vector3(min.x, max.y, max.z));
            DrawLine(new Vector3(min.x, max.y, max.z), new Vector3(min.x, max.y, min.z));

            DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(min.x, max.y, min.z));
            DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(max.x, max.y, min.z));
            DrawLine(new Vector3(max.x, min.y, max.z), new Vector3(max.x, max.y, max.z));
            DrawLine(new Vector3(min.x, min.y, max.z), new Vector3(min.x, max.y, max.z));
        }

        private static void DrawBox(Bounds bounds)
        {
            var min = bounds.min;
            var max = bounds.max;

            GL.Vertex3(min.x, min.y, min.z);
            GL.Vertex3(max.x, min.y, min.z);
            GL.Vertex3(max.x, max.y, min.z);
            GL.Vertex3(min.x, max.y, min.z);

            GL.Vertex3(min.x, min.y, max.z);
            GL.Vertex3(min.x, max.y, max.z);
            GL.Vertex3(max.x, max.y, max.z);
            GL.Vertex3(max.x, min.y, max.z);

            GL.Vertex3(min.x, min.y, min.z);
            GL.Vertex3(min.x, max.y, min.z);
            GL.Vertex3(min.x, max.y, max.z);
            GL.Vertex3(min.x, min.y, max.z);

            GL.Vertex3(max.x, min.y, min.z);
            GL.Vertex3(max.x, min.y, max.z);
            GL.Vertex3(max.x, max.y, max.z);
            GL.Vertex3(max.x, max.y, min.z);

            GL.Vertex3(min.x, max.y, min.z);
            GL.Vertex3(max.x, max.y, min.z);
            GL.Vertex3(max.x, max.y, max.z);
            GL.Vertex3(min.x, max.y, max.z);

            GL.Vertex3(min.x, min.y, min.z);
            GL.Vertex3(min.x, min.y, max.z);
            GL.Vertex3(max.x, min.y, max.z);
            GL.Vertex3(max.x, min.y, min.z);
        }
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawLine(Vector3 a, Vector3 b)
        {
            GL.Vertex(a);
            GL.Vertex(b);
        }

        private struct Record
        {
            public EType Type;
            public Matrix4x4 Matrix;
            public Color Color;
            public float TimeToComplete;
            public float Radius;
        }

        private enum EType
        {
            None,
            WireBox,
            Box,
            WireCircle,
            Circle,
        }
    }
}