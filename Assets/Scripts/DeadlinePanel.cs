
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeadlinePanel : MonoBehaviour
{
    public TMP_Dropdown yearDropdown;
    public TMP_Dropdown monthDropdown;
    public TMP_Dropdown dayDropdown;
    public TMP_Dropdown hourDropdown;
    public TMP_Dropdown minuteDropdown;

    void Start()
    {
        InitializeDropdown(yearDropdown, 2020, 2030, "Year");
        InitializeDropdown(monthDropdown, 1, 12, "Month");
        InitializeDropdown(dayDropdown, 1, 31, "Day", false); // 最初はロック
        InitializeDropdown(hourDropdown, 0, 23, "Hour");
        InitializeDropdown(minuteDropdown, 0, 59, "Min");

        // Add listeners for year and month to handle the change event
        yearDropdown.onValueChanged.AddListener(delegate { YearMonthChanged(); });
        monthDropdown.onValueChanged.AddListener(delegate { YearMonthChanged(); });
    }

    void InitializeDropdown(TMP_Dropdown dropdown, int start, int end, string placeholderText, bool interactable = true)
    {
        dropdown.ClearOptions();
        List<string> options = new List<string> { placeholderText };
        for (int i = start; i <= end; i++)
        {
            options.Add(i.ToString());
        }
        dropdown.AddOptions(options);
        dropdown.interactable = interactable;
    }

    // This function is called when the year or month dropdown value is changed
    void YearMonthChanged()
    {
        // If either year or month dropdowns are on the placeholder value, keep the day dropdown locked
        if (yearDropdown.value == 0 || monthDropdown.value == 0)
        {
            dayDropdown.interactable = false;
            dayDropdown.value = 0; // Reset to placeholder
        }
        else
        {
            dayDropdown.interactable = true;
            int year = int.Parse(yearDropdown.options[yearDropdown.value].text);
            int month = int.Parse(monthDropdown.options[monthDropdown.value].text);
            UpdateDayDropdown(year, month);
        }
    }

    void UpdateDayDropdown(int year, int month)
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);
        InitializeDropdown(dayDropdown, 1, daysInMonth, "Day");
    }
}
