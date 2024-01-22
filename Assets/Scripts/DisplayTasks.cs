using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayTasks : MonoBehaviour
{
    public GameObject todoElementPrefab;
    public Transform todoListParent;
    public Sprite lowUrgencySprite;
    public Sprite mediumUrgencySprite;
    public Sprite highUrgencySprite;

    private void Start()
    {
        ClearExistingTasks();
        foreach (Task task in TaskManager.Instance.Tasks)
        {
            string minute_adder = "";
            if(task.Minute < 10){
                minute_adder = "0";
            }
            GameObject todoElement = Instantiate(todoElementPrefab, todoListParent);
            todoElement.transform.Find("Panel/Image/title").GetComponent<TextMeshProUGUI>().text = task.Name;
            todoElement.transform.Find("Panel/Image/deadline").GetComponent<TextMeshProUGUI>().text = $"{task.Year}/{task.Month}/{task.Day} {task.Hour}:{minute_adder+task.Minute}";
            todoElement.transform.Find("Panel/Image/memo").GetComponent<TextMeshProUGUI>().text = task.Notes;


            Image urgencyImage = todoElement.transform.Find("Panel/Image").GetComponent<Image>();
            switch (task.Urgency.ToLower())
            {
                case "low":
                    urgencyImage.sprite = lowUrgencySprite;
                    break;
                case "medium":
                    urgencyImage.sprite = mediumUrgencySprite;
                    break;
                case "high":
                    urgencyImage.sprite = highUrgencySprite;
                    break;
            }

            Button deleteButton = todoElement.transform.Find("Panel/Image/trash").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => DeleteTask(task, todoElement));
        }
    }
    private void ClearExistingTasks()
    {
        foreach (Transform child in todoListParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void DeleteTask(Task task, GameObject todoElement)
    {
        TaskManager.Instance.Tasks.Remove(task);
        Destroy(todoElement);
        TaskManager.Instance.SaveTasks(); // 保存するために呼び出します
    }
}
