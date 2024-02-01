using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class DisplayTasks : MonoBehaviour
{
    public GameObject todoElementPrefab;
    public Transform todoListParent;
    public Sprite lowUrgencySprite;
    public Sprite mediumUrgencySprite;
    public Sprite highUrgencySprite;
    private int currentPage = 0;
    private const int TasksPerPage = 5;
    private List<Task> paginatedTasks;
    private Vector2 touchStart;

private void Start()
{
    UpdatePaginatedTasks();
    DisplayCurrentPage();
}

private void UpdatePaginatedTasks()
{
    paginatedTasks = TaskManager.Instance.Tasks
        .Skip(currentPage * TasksPerPage)
        .Take(TasksPerPage)
        .ToList();
}

private void DisplayCurrentPage()
{
    ClearExistingTasks();

    foreach (Task task in paginatedTasks)
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
        UpdatePaginatedTasks();
        DisplayCurrentPage();
    }

private void Update()
{
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            float swipeDistance = touch.position.y - touchStart.y;
            if (Mathf.Abs(swipeDistance) > 50) // 最小スワイプ距離
            {
                if (swipeDistance > 0)
                {
                    SwipeUp();
                }
                else
                {
                    SwipeDown();
                }
            }
        }
    }
}

private void SwipeDown()
{
    if (currentPage > 0)
    {
        currentPage--;
        UpdatePaginatedTasks();
        DisplayCurrentPage();
    }
}

private void SwipeUp()
{
    if ((currentPage + 1) * TasksPerPage < TaskManager.Instance.Tasks.Count)
    {
        currentPage++;
        UpdatePaginatedTasks();
        DisplayCurrentPage();
    }
}

}
