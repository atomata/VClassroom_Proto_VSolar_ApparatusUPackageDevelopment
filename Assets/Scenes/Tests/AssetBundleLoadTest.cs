using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleLoadTest : MonoBehaviour
{
    public void DoLoad(string url)
    {
        StartCoroutine(DoReqRoutine(url));
    }

    IEnumerator DoReqRoutine(string url)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        UnityWebRequestAsyncOperation op = req.SendWebRequest();

        while (!op.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        byte[] data = req.downloadHandler.data;

        AssetBundle ab = AssetBundle.LoadFromMemory(data);

        GameObject[] obs = ab.LoadAllAssets<GameObject>();

        Debug.Log($"Loaded {obs.Length} assets");

        foreach(GameObject ob in obs)
        {
            Instantiate(ob);
        }
    }
}
