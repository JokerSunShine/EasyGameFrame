using System.Collections;
using UnityEditor;
public class XluaNameSpaceMapGenerator
{
    [MenuItem("XLua/Gen常用C#语法提示")]
    public static void GenAll()
    {
        EditorCoroutineRunner.StartEditorCoroutine(GenAllSync());
    }

    private static IEnumerator GenAllSync()
    {
        yield return null;
    }
}
