using Mono.Data.Sqlite;
using System.Data.Common;
using System.IO;
using UnityEngine;


public class DatabaseSQLITE : MonoBehaviour
{
    private const string databaseFileName = "PongDatabase.db";
    private string databasePath;
    private string lastStatus = "Waiting for input.";
    private string recentRows = string.Empty;

 
    public void CreateDB()
    {
        using (SqliteConnection connection = new SqliteConnection(databaseFileName))
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
        using (SqliteConnection connection = new SqliteConnection(databaseFileName))
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
