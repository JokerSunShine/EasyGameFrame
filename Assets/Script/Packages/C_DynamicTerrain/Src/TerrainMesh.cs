using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DynamicTerrain
{
    public class TerrainMesh
    {
        private GameObject terrain;
        private UnityEngine.Mesh mesh;
        private TerrainData data;
        private Vector2Int blockSize;

        private Vector3 terrainPos;
        private Vector3[] verts;
        private int[] tris;

        private Material curMat;

        private int prop_Offset = Shader.PropertyToID("_Offset");
        private int prop__MapSize = Shader.PropertyToID("_MapSize");
        private Vector4 _Offset = Vector4.one;
        private Vector4 MapSize = Vector4.zero;
        public TerrainMesh(TerrainData _data,Vector2Int _blockSize)
        {
            data = _data;
            blockSize = _blockSize;
        }

        public void InitMesh(Vector2Int coord,GameObject attachGo)
        {
            terrain = attachGo;
            terrain.layer = LayerMask.NameToLayer("Ground");
            terrainPos.Set(coord.x, 0, coord.y);
            terrain.transform.position = terrainPos;
            MeshFilter mf = terrain.AddComponent<MeshFilter>();
            MeshRenderer mr = terrain.AddComponent<MeshRenderer>();

            mesh = new UnityEngine.Mesh();
            
            
            mesh.normals = new Vector3[mesh.vertices.Length];

            verts = new Vector3[(blockSize.x + 1) * (blockSize.y + 1)];
            Vector2[] uvs = new Vector2[verts.Length];
            for (int i = 0; i < blockSize.x + 1; i++)
            {
                for (int j = 0; j < blockSize.y + 1; j++)
                {
                    verts[i * blockSize.y + i + j].Set(i, 0, j);
                    uvs[i * blockSize.y + i + j].Set(i / 256.0f, j / 256.0f);
                }
            }
            mesh.vertices = verts;
            mesh.uv = uvs;
            tris = new int[blockSize.x * blockSize.y * 6];
            int vertRow = blockSize.x + 1;
            int vertCol = blockSize.y + 1;
            int triIndex = 0;
            for (int i = 0; i < blockSize.y; i++)
            {
                for (int j = 0; j < blockSize.x; j++)
                {
                    tris[triIndex++] = i * vertRow + j;
                    tris[triIndex++] = (i + 1) * vertRow + j + 1;
                    tris[triIndex++] = (i + 1) * vertRow + j;

                    tris[triIndex++] = i * vertRow + j;
                    tris[triIndex++] = i * vertRow + j + 1;
                    tris[triIndex++] = (i + 1) * vertRow + j + 1;

                }
            }

            mesh.triangles = tris;

            mesh.RecalculateBounds();

            mf.mesh = mesh;
            curMat = mr.material = data.mat;


            MapSize.x = data.mapSize.x;
            MapSize.y = data.mapSize.y;
            MapSize.z = data.heightMapData.heightScale;
            MapSize.w = data.heightMapData.heightOffset;
            curMat.SetVector(prop__MapSize, MapSize);
        }

        public void RefreshMesh(Vector2Int coord)
        {
            terrainPos.Set(coord.x, 0, coord.y);
            terrain.transform.position = terrainPos;

            _Offset.x = coord.x;
            _Offset.y = coord.y;
            curMat.SetVector(prop_Offset, _Offset);
        }
    }
}
