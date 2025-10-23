using UnityEngine;
using System.Collections;
using System;

public class Reel : MonoBehaviour
{
    public Transform[] symbols;           // 인스펙터에 심볼들 넣기
    public float speed = 10f;             // 회전 속도
    public float symbolSpacing = 1f;    // 심볼 간 간격

    private bool canstartButtonclick = true;
    private bool isSpinning = false;
    public Action<Reel> onStop;
    // 회전 시작
    public void spinstart()
    {
        if (canstartButtonclick)
        {
            isSpinning = true;
            canstartButtonclick = false;
            StartCoroutine(SpinRoutine());
        }
    }

    // 회전 코루틴 (무한루프)
    private IEnumerator SpinRoutine()
    {
        while (isSpinning)
        {
            foreach (var s in symbols)
            {
                s.position -= new Vector3(0, speed * Time.deltaTime, 0);

                // 화면 아래로 나가면 위로 보냄
                if (s.position.y < -symbolSpacing * (symbols.Length / 2f))
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
        if (isSpinning) isSpinning = false;
        StartCoroutine(StopRoutine(duration));
    }

    private IEnumerator StopRoutine(float duration)
    {
        float elapsed = 0f;
        float startSpeed = speed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Ease-out (느리게 멈춤)
            float easedT = 1 - Mathf.Pow(1 - t, 2);
            float currentSpeed = Mathf.Lerp(startSpeed, 0f, easedT);

            foreach (var s in symbols)
            {
                s.position -= new Vector3(0, currentSpeed * Time.deltaTime, 0);

                if (s.position.y < -symbolSpacing * (symbols.Length / 2f))
                {
                    float highestY = GetHighestY() + symbolSpacing;
                    s.position = new Vector3(s.position.x, highestY, s.position.z);
                }
            }

            yield return null;
        }

        // 완전 멈춤 처리
        isSpinning = false;
        canstartButtonclick = true;
        SnapSymbols();
        onStop?.Invoke(this);
    }

    // 가장 높은 심볼 Y좌표 찾기
    float GetHighestY()
    {
        float maxY = float.MinValue;
        foreach (var s in symbols)
        {
            if (s.position.y > maxY)
                maxY = s.position.y;
        }
        return maxY;
    }

    // 멈출 때 그리드 맞춤
    void SnapSymbols()
    {
        foreach (var s in symbols)
        {
            float roundedY = Mathf.Round(s.position.y / symbolSpacing) * symbolSpacing;
            s.position = new Vector3(s.position.x, roundedY, s.position.z);
        }
    }

    // 중앙 심볼 찾기
    public Transform GetCenterSymbol()
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var s in symbols)
        {
            float dist = Mathf.Abs(s.position.y - 2.0f);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = s;
            }
        }
        return closest;
    }
}