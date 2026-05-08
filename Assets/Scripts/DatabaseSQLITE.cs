using Mono.Data.Sqlite;
using System.Data.Common;
using System.IO;
using UnityEngine;


public class DatabaseSQLITE : MonoBehaviour
{
    private const string databaseFileName = "PongDatabase.db";
    private string databasePath;

    public void Awake()
    {
        databasePath = Path.Combine(Application.persistentDataPath, databaseFileName);
    }

    public void Start()
    {

    }
    private string ConnectionString => $"URI=file:{databasePath}";
    public void CreateDB()
    {
        Directory.CreateDirectory(Application.persistentDataPath);
        using (SqliteConnection connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS matchhistory (playerLeftScore INT, playerRightScore INT)";
                command.ExecuteNonQuery();
            }

        }

    }
    public void CreateMatchHistory(int playerLeftScore, int playerRightScore)
    {
        using (SqliteConnection connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                string text = "INSERT INTO matchhistory (playerLeftScore, playerRightScore) VALUES ('{0}', '{1}');";

                command.CommandText = string.Format(text, playerLeftScore, playerRightScore);

                command.ExecuteNonQuery();
            }
        }
    }
}
