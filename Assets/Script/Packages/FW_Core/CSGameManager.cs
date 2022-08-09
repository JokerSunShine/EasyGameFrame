using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using _3DMath;
using Instance.LogAOP;
using Newtonsoft.Json.Bson;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = _3DMath.Quaternion;
// using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

// using Vector3 = _3DMath.Vector3;

public class CSGameManager : MonoBehaviour
{
    public Dictionary<string,ManagerObject> managerDic = new Dictionary<string,ManagerObject>();
    public bool OpenLog = true;
    /// <summary>
    /// 刷新频率
    /// </summary>
    public int updateFrequency = 5;
    /// <summary>
    /// 当前刷新
    /// </summary>
    private int curFrequency = 0;
    #region 初始化
    private void Awake()
    {
        InitOther();
        RegisterManager();
        RegiseterInterfaceSingleton();
        ManagerListAwakeCallBack();
        AOPTest();
        // RotationMatrix mat = new RotationMatrix();
        // mat.Transition(new EulerAngle(30f / 180 * Mathf.PI,0,0));
        // RotationMatrix mat = new RotationMatrix(0.866f,0,-0.5f,0,1,0,0.5f,0,0.866f);
        // Matrix3x3 inverseMat = Matrix3x3.Inverse(mat);
        // Vector3 vec = new Vector3(10,20,30);
        // RotationMatrix matrix = RotationMatrix.EulerAngleToRotationMatrix(new EulerAngle(0,30f / 180f * Mathf.PI,0));
        // EulerAngle euler1 = EulerAngle.ObjectMatrixToEulerAngle(matrix);
        // EulerAngle euler2 = EulerAngle.InertialMatrixToEulerAngle(matrix);
        // Vector3 v1 = matrix.InertialToObject(vec);
        // Vector3 v2 = matrix.ObjectToInertial(vec);
        // Vector3 v = 
        // Matrix3x3 projectionMat = Matrix3x3.SetShear(Matrix3x3.AxisType.x,1,2);
        // Vector3 projectionScale = vec * projectionMat;
        // float det = Matrix3x3.Determinant(mat);
        // Matrix3x3 value = mat * inverseMat;
        // Vector3 v = new Vector3();
        // CSDebug.Log(euler1);
        //四元数和欧拉角，四元数和矩阵
        EulerAngle euler = new EulerAngle(10 / 180f * Mathf.PI,20 / 180f * Mathf.PI,30 / 180f * Mathf.PI);
        RotationMatrix matrix2 = RotationMatrix.EulerAngleToRotationMatrix(euler);
        Quaternion q = Quaternion.EulerAngleToQuaternion(euler);
        EulerAngle newEuler = EulerAngle.QuaternionToEulerAngle(q);
        
        RotationMatrix matrix = RotationMatrix.EulerAngleToRotationMatrix(euler);
        Quaternion q1 = Quaternion.RotationMatrixToQuaternion(matrix);
        RotationMatrix matrix1 = RotationMatrix.QuaternionToRotationMatrix(q1);
        EulerAngle euler1 = EulerAngle.ObjectMatrixToEulerAngle(matrix);
        Debug.Log(q1);
    }

    private void RegisterManager()
    {
        managerDic["CSPlatformManagerObject"] = new CSPlatformManagerObject(this);
        managerDic["XluaMgr"] = new XluaMgr(this);
        managerDic["CSNetWorkManager"] = new CSNetWorkManager(this);
    }
    
    private void RegiseterInterfaceSingleton()
    {
        InterfaceSingleton.IPlatformManager = GetManagerByName<CSPlatformManagerObject>("CSPlatformManagerObject");
    }

    private void ManagerListAwakeCallBack()
    {
        foreach (ManagerObject manager in managerDic.Values)
        {
            manager.Awake();
        }
    }
    
    /// <summary>
    /// 方法调用拦截
    /// </summary>
    private void AOPTest()
    {
        AOPTest a = new AOPTest();
        a.TestMethod(1,2);
        int c = 3,d = 4;
        Instance.LogAOP.AOPTest.Before(ref c,ref d);
        Instance.LogAOP.AOPTest.After(5);
    }

    private void InitOther()
    {
        CSDebug.LogSwitch = OpenLog;
    }

    private void Start()
    {
        ManagerListStartCallBack();
    }

    private void ManagerListStartCallBack()
    {
        foreach (ManagerObject manager in managerDic.Values)
        {
            manager.Start();
        }
    }
    #endregion

    #region Update
    private void Update()
    {
        if (curFrequency >= updateFrequency)
        {
            ManagerListUpdateCallBack();
            curFrequency = 0;
        }
        curFrequency++;
    }

    private void ManagerListUpdateCallBack()
    {
        foreach (ManagerObject manager in managerDic.Values)
        {
            manager.Update();
        }
    }
    #endregion

    #region Destroy
    private void OnDestroy()
    {
        ManagerListDestroyCallBack();
    }

    private void ManagerListDestroyCallBack()
    {
        foreach (ManagerObject manager in managerDic.Values)
        {
            manager.Destroy();
        }
    }
    #endregion
    
    #region 获取
    /// <summary>
    /// 通过管理类名字获取管理类实例
    /// </summary>
    /// <param name="managerName"></param>
    /// <returns></returns>
    public T GetManagerByName<T>(string managerName) where T:ManagerObject
    {
        ManagerObject manager;
        managerDic.TryGetValue(managerName, out manager);
        T curManager = manager == null ? null: manager as T;
        return curManager;
    }
    #endregion
    
}

