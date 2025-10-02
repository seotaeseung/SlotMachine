using UnityEngine;
using System.Collections;

public class Reel : MonoBehaviour
{
    public Transform[] symbols; // 인스펙터에 심볼들 넣기
    public float speed = 10f;    // 내려가는 속도
    public float symbolSpacing = 1.1f; // 심볼 간 간격 (Y축)

    private bool isSpinning = false;

    public void spinstart()
    {
        isSpinning = true;
        StartCoroutine(SpinRoutine());
    }
    
    private IEnumerator SpinRoutine()
    {
        while (isSpinning)
        {
            foreach (var s in symbols)
            {
                s.position -= new Vector3(0, speed * Time.deltaTime, 0);

                // 화면 밖으로 나간 심볼을 위로 올려서 무한 루프처럼 보이게
                if (s.position.y < -5f)
                {
                    float highestY = GetHighestY() + symbolSpacing;
                    s.position = new Vector3(s.position.x, highestY, s.position.z);
                }
            }
            yield return null;
        }
        
    }
    public void spinstop(float duration)
    {
        isSpinning = false;
        StartCoroutine(Stop(duration));
    }
    public IEnumerator Stop(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // 심볼 이동
            foreach (var s in symbols)
            {
                s.position -= new Vector3(0, speed * Time.deltaTime, 0);

                if (s.position.y < -symbolSpacing * (symbols.Length / 2))
                {
                    float highestY = GetHighestY();
                    s.position = new Vector3(s.position.x, highestY + symbolSpacing, s.position.z);
                }
            }
            yield return null;
        }
        SnapSymbols();
    }

    float GetHighestY()
    {
        float maxY = float.MinValue;
        foreach (var s in symbols)
        {
            if (s.position.y > maxY)
            {
                maxY = s.position.y;
            }
        }
        return maxY;
    }
    
    void SnapSymbols()
    {
        foreach (var s in symbols)
        {
            float roundedY = Mathf.Round(s.position.y / symbolSpacing) * symbolSpacing;
            s.position = new Vector3(s.position.x, roundedY, s.position.z);
        }
    }
}
