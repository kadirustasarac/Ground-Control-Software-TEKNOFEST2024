using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Misya_Yüksek_İrtifa_Yer_İstasyonu
{
  

    public class ObjLoader
    {
        public List<Vector3> Vertices { get; private set; }
        public List<Vector3> Normals { get; private set; }
        public List<int[]> Faces { get; private set; }

        public ObjLoader()
        {
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            Faces = new List<int[]>();
        }

        public void Load(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("v "))
                    {
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                        float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                        Vertices.Add(new Vector3(x, y, z));
                    }
                    else if (line.StartsWith("vn "))
                    {
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                        float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                        Normals.Add(new Vector3(x, y, z));
                    }
                    else if (line.StartsWith("f "))
                    {
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        int[] face = new int[9];
                        for (int i = 1; i <= 3; i++)
                        {
                            string[] indices = parts[i].Split('/');
                            face[(i - 1) * 3 + 0] = int.Parse(indices[0]) - 1;
                            face[(i - 1) * 3 + 1] = indices.Length > 1 && indices[1] != "" ? int.Parse(indices[1]) - 1 : 0;
                            face[(i - 1) * 3 + 2] = indices.Length > 2 ? int.Parse(indices[2]) - 1 : 0;
                        }
                        Faces.Add(face);
                    }
                }
            }
        }

        public void Draw()
        {
            GL.Begin(PrimitiveType.Triangles);
            foreach (var face in Faces)
            {
                for (int i = 0; i < 3; i++)
                {
                    GL.Normal3(Normals[face[i * 3 + 2]]);
                    GL.Vertex3(Vertices[face[i * 3]]);
                }
            }
            GL.End();
        }
    }
}
