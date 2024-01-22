using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RegisterTask : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_Dropdown yearDropdown;
    public TMP_Dropdown monthDropdown;
    public TMP_Dropdown dayDropdown;
    public TMP_Dropdown hourDropdown;
    public TMP_Dropdown minuteDropdown;
    public TMP_Dropdown urgencyDropdown;
    public TMP_InputField memoInputField;

    public void OnRegisterButtonClick()
    {
        Task newTask = new Task
        {
            Name = nameInputField.text,
            Year = int.Parse(yearDropdown.options[yearDropdown.value].text),
            Month = int.Parse(monthDropdown.options[monthDropdown.value].text),
            Day = int.Parse(dayDropdown.options[dayDropdown.value].text),
            Hour = int.Parse(hourDropdown.options[hourDropdown.value].text),
            Minute = int.Parse(minuteDropdown.options[minuteDropdown.value].text),
            Urgency = urgencyDropdown.options[urgencyDropdown.value].text,
            Notes = memoInputField.text
        };

        TaskManager.Instance.AddTask(newTask);
    }
}
