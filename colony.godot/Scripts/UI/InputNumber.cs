using System;
using System.Globalization;
using Godot;

namespace Colony.Godot.Scripts.UI;

public enum NumberInputMode
{
    Integer,
    Double,
}

public partial class InputNumber : LineEdit
{
    private string _lastValidText = string.Empty;
    private NumberInputMode Mode { get; set; } = NumberInputMode.Integer;

    public double MinValue { get; set; }

    public double MaxValue { get; set; } = double.MaxValue;

    public bool AllowNegative { get; set; }
    public double? Value { get; set; }

    public event Action<double?>? ValueChanged;

    public override void _Ready()
    {
        _lastValidText = Value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;

        TextChanged += OnTextChanged;
        FocusExited += OnFocusExited;
    }

    public override void _ExitTree()
    {
        TextChanged -= OnTextChanged;
        FocusExited -= OnFocusExited;
    }

    private void OnTextChanged(string text)
    {
        if (!TryValidate(text, out var value))
        {
            RestoreLastValidText();
            return;
        }

        if (text == _lastValidText) return;

        _lastValidText = text;
        Value = value;

        NotifyValueChanged();
    }

    private void OnFocusExited()
    {
        TryValidate(Text, out var value);
    }

    private void NotifyValueChanged()
    {
        ValueChanged?.Invoke(Value);
    }

    private static bool AreEqual(double first,
                                 double second)
    {
        return Math.Abs(first - second) < 0.0000001;
    }


    private bool TryValidate(string text,
                             out double? value)
    {
        value = null;

        // Empty input is allowed.
        if (string.IsNullOrEmpty(text)) return true;

        // Negative values
        if (!AllowNegative && text.Contains('-')) return false;

        // Integer
        if (Mode == NumberInputMode.Integer)
        {
            if (!int.TryParse(
                    text,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out var integerValue))
                return false;

            if (integerValue < MinValue ||
                integerValue > MaxValue)
                return false;

            value = integerValue;

            return true;
        }

        // Double
        if (!double.TryParse(
                text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var doubleValue))
            return false;

        if (doubleValue < MinValue ||
            doubleValue > MaxValue)
            return false;

        value = doubleValue;

        return true;
    }

    private void RestoreLastValidText()
    {
        var caretPosition = CaretColumn;

        Text = _lastValidText;

        CaretColumn = Math.Min(
            caretPosition,
            Text.Length);
    }
}