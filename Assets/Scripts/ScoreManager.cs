using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public TextMeshProUGUI Score1;
    public TextMeshProUGUI Score2;

    public int score1 = 0;
    public int score2 = 0;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;

            DontDestroyOnLoad(gameObject);
    }
    public void OneScore()
    {
        score1++;
        Score1.text = $"{score1}";
    }

    public void TwoScore()
    {
        score2++;
        Score2.text = $"{score2}";
    }



}
