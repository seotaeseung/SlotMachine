using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    public Reel[] reels;   // 3�� �� ����
    public float spinDuration = 2f; // �⺻ ���� �ð�
    public void SpinStart()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].spinstart();
        }
    }
    public void SpinStop()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            float duration = spinDuration + i * Random.Range(0.1f,3f);
            reels[i].spinstop(duration);
        }
    }
}
