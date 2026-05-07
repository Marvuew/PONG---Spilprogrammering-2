using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    public static TextMeshProUGUI Score1;
    public static TextMeshProUGUI Score2;

    public static int score1 = 0;
    public static int score2 = 0;

    public static void OneScore()
    {
        score1++;
        Score1.text = $"{score1}";
    }

    public static void TwoScore()
    {
        score2++;
        Score2.text = $"{score2}";
    }



}
