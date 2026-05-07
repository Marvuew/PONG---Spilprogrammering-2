using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    public TextMeshProUGUI Score1;
    public TextMeshProUGUI Score2;

    public int score1 = 0;
    public int score2 = 0;

    float maxPoints = 10;

    public void OneScore()
    {
        score1++;
        Score1.text = $"{score1}";
        if (score1 >= maxPoints)
        {
            GameManager.instance.TransferScore(score1, score2);
            GameManager.instance.OnPlaySessionEnded.Invoke();
            return;
        }
    }

    public void TwoScore()
    {
        score2++;
        Score2.text = $"{score2}";
        if (score2 >= maxPoints)
        {
            GameManager.instance.TransferScore(score1, score2);
            GameManager.instance.OnPlaySessionEnded.Invoke();
            return;
        }
    }

    public void Reset()
    {
        score1 = 0;
        score2 = 0;
        Score1.text = $"{score1}";
        Score2.text = $"{score2}";
    }



}
