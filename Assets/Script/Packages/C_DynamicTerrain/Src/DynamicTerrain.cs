using UnityEngine;
namespace DynamicTerrain
{
    [XLua.LuaCallCSharp]
    public class DynamicTerrain
    {
        private DynamicTerrain() { }

        private static DynamicTerrain _instance;
        public static DynamicTerrain Instance {
            get {
                if (_instance == null)
                {
                    _instance = new DynamicTerrain();
                }

                return _instance;
            }
        }


        private Transform mDetector;
        private Rect triggerRect;

        private GameObject terrain;
        private UnityEngine.Mesh mMesh;
        private MeshRenderer mMeshRenderer;
        private Material curMat;


        private Vector2Int blockSize;
        private float mTerrainHeight;
        private float heightScale = 4.1f;

        private Vector2Int curCoord;
        private Vector2Int CurCoord {
            get {
                curCoord.x = (int)mDetector.position.x;
                curCoord.y = (int)mDetector.position.z;
                return curCoord;
            }
        }

        public GameObject TerrainGo
        {
            get {
                return terrain;
            }
        }

        private int mainTexSt_PropID = Shader.PropertyToID("_MainTex_STs");

        public void Init(Transform detector,int blockWidth,int blockHeight,int viewWidth,int viewHight)
        {
            mDetector = detector;
            blockSize = new Vector2Int(blockWidth, blockHeight);

           mMesh = CreateMesh(blockWidth, blockHeight);
           
            terrain = new GameObject("DynamicTerrain");
            mMeshRenderer = terrain.AddComponent<MeshRenderer>();
            MeshFilter mf = terrain.AddComponent<MeshFilter>();
            mf.mesh = mMesh;

            terrain.transform.position = Vector3.one * 10000;
            terrain.transform.localScale = new Vector3(1, heightScale, 1);
            Object.DontDestroyOnLoad(terrain);

            triggerRect.x = -10000;
            triggerRect.y = -10000;
            triggerRect.width = blockWidth - viewWidth;
            triggerRect.height = blockHeight - viewHight;
            Detect();
        }

        public void RefreshTerrain(TerrainData terrainData)
        {
            mTerrainHeight = terrainData.heightMapData.heightOffset;
            terrain.transform.position = new Vector3(terrain.transform.position.x, mTerrainHeight, terrain.transform.position.z);
            mMeshRenderer.sharedMaterial = terrainData.mat;
            mMeshRenderer.sharedMaterial.SetVectorArray(mainTexSt_PropID, terrainData.mainTex_STs);
        }

        public void Destroy()
        {
            mDetector = null;
            if (terrain != null)
            {
                Object.Destroy(terrain);
                terrain = null;
            }

        }

        public void Show(bool bShow)
        {
            if (terrain != null)
            {
                terrain.SetActive(bShow);
            }
        }

        private UnityEngine.Mesh CreateMesh(int width,int height)
        {
            UnityEngine.Mesh mesh = new UnityEngine.Mesh();

            int vertRow = width + 1;
            int vertCol = height + 1;

            Vector3[] verts = new Vector3[vertRow * vertCol];
            for (int i = 0; i < vertCol; i++)
            {
                for (int j = 0; j < vertRow; j++)
                {
                    verts[i * vertRow + j].Set(i, 0, j);
                }
            }
            mesh.vertices = verts;
            int[] tris = new int[width * height * 6];

            int triIndex = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
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

            return mesh;
        }

        public void Update()
        {
            Detect();
        }

        void Detect()
        {
            if (mDetector == null || terrain == null)
            {
                return;
            }
            if (IsTargetOverStep())
            {
                UpdateTriggerRect();
                Trigger();
            }
        }



        private bool IsTargetOverStep()
        {
            return mDetector.position.x < triggerRect.xMin || mDetector.position.x > triggerRect.xMax ||
                mDetector.position.z < triggerRect.yMin || mDetector.position.z > triggerRect.yMax;
        }


        private void UpdateTriggerRect()
        {
            triggerRect.x = mDetector.position.x - triggerRect.width / 2;
            triggerRect.y = mDetector.position.z - triggerRect.height / 2;
        }


        private void Trigger()
        {
            
            //位移取2的整数倍，否则uv计算不正确
            Vector2Int pos = (CurCoord - blockSize / 2) ;
            terrain.transform.position = new Vector3(pos.x, mTerrainHeight, pos.y);

        }

    }
}
