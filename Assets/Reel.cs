using UnityEngine;
using System.Collections;
using System;

public class Reel : MonoBehaviour
{
    public Transform[] symbols;           // �ν����Ϳ� �ɺ��� �ֱ�
    public float speed = 10f;             // ȸ�� �ӵ�
    public float symbolSpacing = 1f;    // �ɺ� �� ����

    private bool canstartButtonclick = true;
    private bool isSpinning = false;
    public Action<Reel> onStop;
    // ȸ�� ����
    public void spinstart()
    {
        if (canstartButtonclick)
        {
            isSpinning = true;
            canstartButtonclick = false;
            StartCoroutine(SpinRoutine());
        }
    }

    // ȸ�� �ڷ�ƾ (���ѷ���)
    private IEnumerator SpinRoutine()
    {
        while (isSpinning)
        {
            foreach (var s in symbols)
            {
                s.position -= new Vector3(0, speed * Time.deltaTime, 0);

                // ȭ�� �Ʒ��� ������ ���� ����
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

            // Ease-out (������ ����)
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

        // ���� ���� ó��
        isSpinning = false;
        canstartButtonclick = true;
        SnapSymbols();
        onStop?.Invoke(this);
    }

    // ���� ���� �ɺ� Y��ǥ ã��
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

    // ���� �� �׸��� ����
    void SnapSymbols()
    {
        foreach (var s in symbols)
        {
            float roundedY = Mathf.Round(s.position.y / symbolSpacing) * symbolSpacing;
            s.position = new Vector3(s.position.x, roundedY, s.position.z);
        }
    }

    // �߾� �ɺ� ã��
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