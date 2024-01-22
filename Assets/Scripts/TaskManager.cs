using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }
    public List<Task> Tasks = new List<Task>();

    private const string TasksKey = "Tasks";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTasks();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddTask(Task task)
    {
        Tasks.Add(task);
        SaveTasks();
    }

    public void SaveTasks()
    {
        string json = JsonUtility.ToJson(new Serialization<Task>(Tasks));
        PlayerPrefs.SetString(TasksKey, json);
        PlayerPrefs.Save();
    }

    private void LoadTasks()
    {
        if (PlayerPrefs.HasKey(TasksKey))
        {
            string json = PlayerPrefs.GetString(TasksKey);
            Serialization<Task> tasks = JsonUtility.FromJson<Serialization<Task>>(json);
            Tasks = tasks.ToList();
        }
    }
}

[System.Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> items;
    
    public List<T> ToList() { return items; }

    public Serialization(List<T> items)
    {
        this.items = items;
    }
}
