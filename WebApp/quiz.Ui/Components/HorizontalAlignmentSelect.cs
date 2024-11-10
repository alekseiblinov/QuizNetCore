using Microsoft.AspNetCore.Components;

namespace quiz.Ui.Components;

/// <summary>
/// Контрол для выбора горизонтального выравнивания контрола на экране.
/// </summary>
public partial class HorizontalAlignmentSelect
{
    private string? _selectedItemValue;

    /// <summary>
    /// Перечень всех вариантов горизонтального выравнивания контрола на экране.
    /// По материалам https://stackoverflow.com/questions/57932850/how-to-make-two-way-binding-on-blazor-component
    /// </summary>
    private Dictionary<string, string> HorizontalAlignmentTypes { get; set; }

    /// <summary>
    /// ID выбранного элемента.
    /// </summary>
    [Parameter]
    public string? SelectedItemValue
    {
        get => _selectedItemValue;
        set
        {
            if (_selectedItemValue == value)
            {
                return;
            }

            _selectedItemValue = value;
            SelectedItemValueChanged.InvokeAsync(value);
        }
    }

    /// <summary>
    /// Текст выбранного элемента.
    /// </summary>
    [Parameter]
    public string? SelectedItemText { get; set; }

    /// <summary>
    /// Событие, возникающее при смене выбранного элемента выпадающего списка пользователем.
    /// </summary>
    [Parameter]
    public EventCallback<string> SelectedItemValueChanged { get; set; }
        
    public HorizontalAlignmentSelect()
    {
        // Заполнение элементов выпадающего списка.
        HorizontalAlignmentTypes = new Dictionary<string, string>();
        HorizontalAlignmentTypes.Add("start", "Слева");
        HorizontalAlignmentTypes.Add("end", "Справа");
        HorizontalAlignmentTypes.Add("center", "По центру");
    }
}