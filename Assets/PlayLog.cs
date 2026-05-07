using System;
using System.IO;
using UnityEngine;

public class PlayLog : MonoBehaviour
{
    [SerializeField] private string playLogFileName = "play-log.txt";
    [SerializeField] private string playSessionCountFileName = "play-session-count.txt";

    private string playLogFilePath;
    private string playSessionCountFilePath;
    private string playLogStatus;
    private string playSessionCountStatus;

    private int _player1Score;
    private int _player2Score;

    private float ballHits;

    private float playSessionCount;

    private void OnEnable()
    {
        // GameManager.OnPlaySessionEnded += WritePlayLog;
        // GameManager.OnPlaySessionStarted += UpdatePlaySessionCount;
    }


    private void Awake()
    {
        playLogFilePath = Path.Combine(Application.persistentDataPath, playLogFileName);
        playSessionCountFilePath = Path.Combine(Application.persistentDataPath, playSessionCountFileName);
        playLogStatus = "Ready.";
        playSessionCountStatus = "Ready.";
    }

    private void OnGUI()
    {
        // Simple GUI to demonstrate file logging functionality
        // I am using GUILayout for simplicity, but in a production application 
        // you might want to use a more robust UI system that directly integrates
        // with your game's UI, i.e. setting it up directly in the scene
        GUILayout.BeginArea(new Rect(10, 10, 1120, 480 + 200), GUI.skin.box);
        GUILayout.Label("Play Session Logging Demo");
        GUILayout.Label($"Persistent Path: {Application.persistentDataPath}");

        GUILayout.BeginHorizontal();

        DrawColumn();

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawColumn()
    {
        GUILayout.BeginVertical(GUILayout.Width(340));
        GUILayout.Label("Column 1: Simple Log");

        if (GUILayout.Button("Write Play Log Entry"))
        {
            WritePlayLog();
        }

        if (GUILayout.Button("Show Log Contents"))
        {
            playLogStatus = ReadTextFile(playLogFilePath);
        }

        if (GUILayout.Button("Clear Log"))
        {
            File.WriteAllText(playLogFilePath, string.Empty);
            playLogStatus = "Log cleared.";
        }

        if (GUILayout.Button("Update Play Session Count"))
        {
            UpdatePlaySessionCount();
            playSessionCountStatus = $"Current Play Session Count: {playSessionCount}";
        }

        GUILayout.Label("Status / Contents:");
        GUILayout.TextArea(playLogStatus, GUILayout.Height(220));
        GUILayout.Label("Play Session Count Status:");
        GUILayout.TextArea(playSessionCountStatus, GUILayout.Height(60));
        GUILayout.EndVertical();
    }

    private void WritePlayLog()
    {
        Directory.CreateDirectory(Application.persistentDataPath);
        File.AppendAllText(playLogFilePath, $"{DateTime.Now:O} - Play session {playSessionCount} ended {_player1Score}-{_player2Score}. The ball was hit {ballHits} times. {Environment.NewLine}");
        playLogStatus = $"Wrote to: {playLogFilePath}";
    }

    private void UpdatePlaySessionCount()
    {
        Debug.Log("Updating Play Session Count");
        Directory.CreateDirectory(Application.persistentDataPath); // Create a directory if it doesnt exits yet at unitys persistent data path.

        if (!File.Exists(playSessionCountFilePath)) // If the file doesn't exist yet, create it and intialize it to 0
        {
            File.AppendAllText(playSessionCountFilePath, "0");
        }

        var playSessionFile = ReadTextFile(playSessionCountFilePath); // Read the playsession file to get the current playsession count.
        playSessionCount = playSessionFile != string.Empty ? int.Parse(playSessionFile) : 0; // if the playsession file is empty, initialize the count to 0, otherwise parse the count from the file.
        File.WriteAllText(playSessionCountFilePath, string.Empty); // Clear the file before writing the new count to it.
        File.AppendAllText(playSessionCountFilePath, (playSessionCount + 1).ToString()); // Increment the cound and write it back to the file.
    }

    private string ReadTextFile(string path)
    {
        if (!File.Exists(path))
        {
            return "File not found. Write data first.";
        }

        return File.ReadAllText(path);
    }
}

/*public class FileLoggerDemo : MonoBehaviour
{
    // one separate file for each column to demonstrate different use cases and data formats
    [Header("File Names")]
    [SerializeField] private string logFileName = "demo-log.txt";
    [SerializeField] private string typesFileName = "demo-types.txt";
    [SerializeField] private string customFileName = "demo-custom.txt";
    [SerializeField] private string playSessionLogFileName = "play-session-log.txt";
    [SerializeField] private string playSessionCountFileName = "play-session-count.txt";

    private string playSessionLogFilePath;
    private string playSessionCountFilePath;
    private string logFilePath;
    private string typesFilePath;
    private string customFilePath;

    private string playSessionStatus;
    private string logStatus;
    private string typesStatus;
    private string customStatus;

    // Sample data for the types column
    private int sampleInt = 42;
    private float sampleFloat = 3.14f;
    private bool sampleBool = true;
    private string sampleString = "Hello, file!";
    private Vector3 sampleVector = new Vector3(1f, 2f, 3f);
    private int[] intArray = new int[3] { 1, 2, 3 };

    // Custom input/output for the playground column
    private string customInput = "Write anything here...";
    private string customOutput = string.Empty;
    private int arrayCount = 0;

    private void Awake()
    {
        // Unity provides a persistent data path that is safe to write to across different platforms
        // Using other paths may not work on all platforms or may not be writable
        logFilePath = Path.Combine(Application.persistentDataPath, logFileName);
        typesFilePath = Path.Combine(Application.persistentDataPath, typesFileName);
        customFilePath = Path.Combine(Application.persistentDataPath, customFileName);
        playSessionLogFilePath = Path.Combine(Application.persistentDataPath, playSessionLogFileName);
        playSessionCountFilePath = Path.Combine(Application.persistentDataPath, playSessionCountFileName);

        logStatus = "Ready.";
        typesStatus = "Ready.";
        customStatus = "Ready.";

        // ▪ Store logs in a timestamped file per play session.
        Add1ToPlaySession();
        string playSessionCount = ReadTextFile(playSessionCountFilePath);
        WritePlaySessionLog("=== New Play Session Started ===" + "This is the " + playSessionCount + " play Session");
    }

    private void OnGUI()
    {
        // Simple GUI to demonstrate file logging functionality
        // I am using GUILayout for simplicity, but in a production application 
        // you might want to use a more robust UI system that directly integrates
        // with your game's UI, i.e. setting it up directly in the scene
        GUILayout.BeginArea(new Rect(10, 10, 1120, 480 + 200), GUI.skin.box);
        GUILayout.Label("File Logging Demo (System.IO) - Three Files");
        GUILayout.Label($"Persistent Path: {Application.persistentDataPath}");

        GUILayout.BeginHorizontal();

        DrawLogColumn();
        DrawTypesColumn();
        DrawCustomColumn();

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void Add1ToPlaySession()
    {
        Debug.Log("Adding 1 to play session file");
        Directory.CreateDirectory(Application.persistentDataPath); // Create a directory if it doesnt exits yet at unitys persistent data path.

        if (!File.Exists(playSessionCountFilePath)) // If the file doesn't exist yet, create it and intialize it to 0
        {
            File.AppendAllText(playSessionCountFilePath, "0");
        }

        var playSessionFile = ReadTextFile(playSessionCountFilePath); // Read the playsession file to get the current playsession count.
        int playSessionCount = playSessionFile != string.Empty ? int.Parse(playSessionFile) : 0; // if the playsession file is empty, initialize the count to 0, otherwise parse the count from the file.
        File.WriteAllText(playSessionCountFilePath, string.Empty); // Clear the file before writing the new count to it.
        File.AppendAllText(playSessionCountFilePath, (playSessionCount + 1).ToString()); // Increment the cound and write it back to the file.
    }

    private void DrawLogColumn()
    {
        GUILayout.BeginVertical(GUILayout.Width(340));
        GUILayout.Label("Column 1: Simple Log");

        if (GUILayout.Button("Write Log Entry"))
        {
            WriteLog($"Player clicked at {DateTime.Now:T}");
        }

        if (GUILayout.Button("Show Log Contents"))
        {
            logStatus = ReadTextFile(logFilePath);
        }

        if (GUILayout.Button("Clear Log"))
        {
            File.WriteAllText(logFilePath, string.Empty);
            logStatus = "Log cleared.";
        }

        GUILayout.Label("Status / Contents:");
        GUILayout.TextArea(logStatus, GUILayout.Height(220));
        GUILayout.EndVertical();
    }

    private void DrawTypesColumn()
    {
        GUILayout.BeginVertical(GUILayout.Width(360));
        GUILayout.Label("Column 2: Storing Different Data Types");

        sampleInt = IntField("Int", sampleInt);
        sampleFloat = FloatField("Float", sampleFloat);
        sampleBool = GUILayout.Toggle(sampleBool, "Bool");
        GUILayout.Label("String");
        sampleString = GUILayout.TextField(sampleString);

        GUILayout.Label("Vector3 (x,y,z)");
        sampleVector = Vector3Field(sampleVector);

        if (GUILayout.Button("Save Types To File"))
        {
            SaveTypes();
        }

        if (GUILayout.Button("Load Types From File"))
        {
            LoadTypes();
        }

        GUILayout.Label("Status:");
        GUILayout.TextArea(typesStatus, GUILayout.Height(140));
        GUILayout.EndVertical();
    }

    private void DrawCustomColumn()
    {
        GUILayout.BeginVertical(GUILayout.Width(360));
        GUILayout.Label("Column 3: Student Playground");

        GUILayout.Label("Input:");
        customInput = GUILayout.TextArea(customInput, GUILayout.Height(120));
        GUILayout.Label("Array Count:"); // Make Label for Array Count
        arrayCount = IntField("Count", arrayCount); // Set the int arrayCount to the value of the IntField, which allows the user to specify how many elements they want in the array.
        GUILayout.BeginVertical(); // Begin a new vertical group to display the elements of the array
        for (int i = 0; i < arrayCount; i++)
        {
            intArray[i] = IntField("Element " + i, intArray[i]); // Make an intfield in relation to the arraycount, that automatically updated the int array with the values respectively.
        }
        GUILayout.EndVertical(); // End the vertical group for the array elements

        if (GUILayout.Button("Write Custom Data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath);

            // ▪ Store an array inside a text file. 
            // ▪ How can you ensure that the correct length is initialized ?
            var arrayData = string.Join(",", intArray); // Convert the array to a comma-separated string
            File.WriteAllText(customFilePath, customInput + arrayData); // Write the custom input and array data to the file.
            customStatus = $"Wrote {customInput.Length} chars to file.";
        }

        if (GUILayout.Button("Read Custom Data"))
        {
            customOutput = ReadTextFile(customFilePath);
            customStatus = "Read file contents.";
        }

        GUILayout.Label("Output:");
        GUILayout.TextArea(customOutput, GUILayout.Height(120));
        GUILayout.Label($"Status: {customStatus}");
        GUILayout.EndVertical();
    }

    private void WriteLog(string message)
    {
        // Ensure the directory exists before writing
        Directory.CreateDirectory(Application.persistentDataPath);
        // Append the log message to the file with a timestamp
        // Allows to keep a history of log entries instead of overwriting
        File.AppendAllText(logFilePath, $"{DateTime.Now:O} - {message}{Environment.NewLine}");
        logStatus = $"Wrote to: {logFilePath}";
    }

    private void WritePlaySessionLog(string message)
    {
        Directory.CreateDirectory(Application.persistentDataPath);
        File.AppendAllText(playSessionLogFilePath, $"{DateTime.Now:O} - {message}{Environment.NewLine}");
        playSessionStatus = $"Wrote to : {playSessionLogFilePath}";
    }

    private void SaveTypes()
    {
        Directory.CreateDirectory(Application.persistentDataPath);
        var data =
            $"int={sampleInt}{Environment.NewLine}" +
            $"float={sampleFloat}{Environment.NewLine}" +
            $"bool={sampleBool}{Environment.NewLine}" +
            $"string={Escape(sampleString)}{Environment.NewLine}" +
            $"vector={sampleVector.x},{sampleVector.y},{sampleVector.z}{Environment.NewLine}";

        File.WriteAllText(typesFilePath, data);
        typesStatus = "Saved types to file.";
    }

    private void LoadTypes()
    {
        if (!File.Exists(typesFilePath))
        {
            typesStatus = "Types file not found. Save first.";
            return;
        }

        var lines = File.ReadAllLines(typesFilePath);
        foreach (var line in lines)
        {
            var parts = line.Split('=', 2);
            if (parts.Length != 2)
            {
                continue;
            }

            var key = parts[0].Trim();
            var value = parts[1].Trim();
            switch (key)
            {
                case "int":
                    int.TryParse(value, out sampleInt);
                    break;
                case "float":
                    float.TryParse(value, out sampleFloat);
                    break;
                case "bool":
                    bool.TryParse(value, out sampleBool);
                    break;
                case "string":
                    sampleString = Unescape(value);
                    break;
                case "vector":
                    sampleVector = ParseVector3(value);
                    break;
            }
        }

        typesStatus = "Loaded types from file.";
    }

    private string ReadTextFile(string path)
    {
        // Check if the log file exists before trying to read it
        if (!File.Exists(path))
        {
            return "File not found. Write data first.";
        }

        // File provides some basic functions for reading and writing text files, 
        // including reading the entire contents at once
        // Other data formats or larger files may require more complex handling 
        // (e.g., streaming, binary formats, etc.)
        return File.ReadAllText(path);
    }

    private int IntField(string label, int value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(60));
        var text = GUILayout.TextField(value.ToString());
        int.TryParse(text, out value);
        GUILayout.EndHorizontal();
        return value;
    }

    private float FloatField(string label, float value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(60));
        var text = GUILayout.TextField(value.ToString("0.###"));
        float.TryParse(text, out value);
        GUILayout.EndHorizontal();
        return value;
    }

    private Vector3 Vector3Field(Vector3 value)
    {
        GUILayout.BeginHorizontal();
        value.x = FloatFieldInline("X", value.x);
        value.y = FloatFieldInline("Y", value.y);
        value.z = FloatFieldInline("Z", value.z);
        GUILayout.EndHorizontal();
        return value;
    }

    private float FloatFieldInline(string label, float value)
    {
        GUILayout.Label(label, GUILayout.Width(18));
        var text = GUILayout.TextField(value.ToString("0.###"), GUILayout.Width(60));
        float.TryParse(text, out value);
        return value;
    }

    private Vector3 ParseVector3(string value)
    {
        var parts = value.Split(',');
        if (parts.Length != 3)
        {
            return sampleVector;
        }

        float.TryParse(parts[0], out var x);
        float.TryParse(parts[1], out var y);
        float.TryParse(parts[2], out var z);
        return new Vector3(x, y, z);
    }

    private string Escape(string value)
    {
        return value.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r");
    }

    private string Unescape(string value)
    {
        return value.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\\\", "\\");
    }
}*/

