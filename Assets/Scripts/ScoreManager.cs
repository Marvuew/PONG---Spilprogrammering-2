using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    
    public TextMeshProUGUI Score1;
    public TextMeshProUGUI Score2;

    public static int score1 = 0;
    public static int score2 = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (!instance == this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        Score1.text = $"{score1}";
        Score2.text = $"{score2}";
        
    }


}
