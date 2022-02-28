using UnityEngine;

namespace Instance.LogAOP
{
    public class AOPTest:AOPContext
    {
        public int TestMethod(int a,int b)
        {
            Debug.Log("Process Test 1");
            return a + b;
        }
        
        public static void Before(ref int a,ref int b)
        {
            Debug.Log("start:");
            a = 200;
            b = 400;
        }
        
        public static void After(int result)
        {
            Debug.Log("End:" +result);
        }
    }
}