using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(ControlScenes complete)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levelComplete.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        CompleteData data = new CompleteData(complete);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static CompleteData LoadData(ControlScenes complete)
    {
        string path = Application.persistentDataPath + "/levelComplete.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CompleteData data = formatter.Deserialize(stream) as CompleteData;
            stream.Close();

            return data;
        }
        else
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            CompleteData data = new CompleteData(complete);

            formatter.Serialize(stream, data);
            stream.Close();
            return data;
        }
    }
}
