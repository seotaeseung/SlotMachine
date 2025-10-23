using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SlotMachine : MonoBehaviour
{
    public Reel[] reels;   // 3개 릴 연결
    public float spinDuration = 2f; // 기본 지속 시간
    private int stoppedCount = 0;
    public void Start()
    {
        foreach (var reel in reels)
        {
            reel.onStop += OnReelStop;
        }
    }
    public void SpinStart()
    {
        stoppedCount = 0;
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].spinstart();
        }
    }
    public void SpinStop()
    {
        StartCoroutine(SpinStopCoroutine());
       
    }

    private IEnumerator SpinStopCoroutine()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            float delay = 0.3f + i * Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(delay);
            reels[i].spinstop(spinDuration);
        }
    }
    private void OnReelStop(Reel r)
    {
        stoppedCount++;
        if (stoppedCount == reels.Length)
            CheckResult(); //모든 릴 멈춘 후 한 번만 호출
    }
    public void CheckResult()
    {
        Transform[] results = new Transform[reels.Length];
        for(int i = 0; i< reels.Length; i++)
        {
            results[i] = reels[i].GetCenterSymbol();
            Debug.Log(results[i]);
        }

        if (results[0].name == results[1].name && results[1].name == results[2].name)
        {
            Debug.Log("당첨!");
        }
        else
        {
            Debug.Log("꽝!");
        }
    }
}
