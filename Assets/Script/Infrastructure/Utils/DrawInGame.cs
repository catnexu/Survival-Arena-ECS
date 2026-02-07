using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Infrastructure
{
    public sealed class DrawInGame : MonoBehaviour
    {
        private static readonly List<Record> s_records = new(256);
        private static Material s_material;

        private void Awake()
        {
            if (!GetComponent<Camera>())
            {
                Debug.LogError($"{nameof(DrawInGame)} script needs to be placed on object with Camera component");
            }
        }

        private void OnPostRender()
        {
            if (!s_material)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");

                s_material = new Material(shader)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }

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
        
        public static void Box(Vector2 center, Quaternion rotation, Vector2 size, Color color, float duration = 0f)
        {
            CreateRecord(center, rotation, size, color, duration, EType.Box);
        }

        public static void WireBox(Vector2 center, Quaternion rotation, Vector2 size, Color color, float duration = 0f)
        {
            CreateRecord(center, rotation, size, color, duration, EType.WireBox);
        }

        private static void CreateRecord(Vector2 center, Quaternion rotation, Vector2 size, Color color, float duration, EType type)
        {
            Record record = new Record()
            {
                Type = type,
                Matrix = Matrix4x4.TRS(center, rotation, size),
                Color = color,
                TimeToComplete = Time.time + duration,
            };

            s_records.Add(record);
        }

        private static void Clear()
        {
            s_records.Clear();
        }

        private static void DrawRecord(Record record)
        {
            GL.PushMatrix();
            s_material.SetPass(0);
            GL.MultMatrix(record.Matrix);

            switch (record.Type)
            {
                case EType.WireBox:
                    DrawBox(record, GL.LINES);
                    break;
                case EType.Box:
                    DrawBox(record, GL.QUADS);
                    break;
            }

            GL.PopMatrix();
        }

        private static void DrawBox(Record record, int glMode)
        {
            GL.Begin(glMode);
            GL.Color(record.Color);
            DrawWireBox(new Bounds(Vector3.zero, Vector3.one));
            GL.End();
        }

        private static void DrawWireBox(Bounds bounds)
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            Vector3 bl = new Vector3(min.x, min.y, 0f);
            Vector3 br = new Vector3(min.x, max.y, 0f);
            Vector3 tl = new Vector3(max.x, min.y, 0f);
            Vector3 tr = new Vector3(max.x, max.y, 0f);
            
            DrawLine(bl, br);
            DrawLine(br, tr);
            DrawLine(tr, tl);
            DrawLine(tl, bl);
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
        }

        private enum EType
        {
            None = 0,
            WireBox = 1,
            Box = 2,
        }
    }
}