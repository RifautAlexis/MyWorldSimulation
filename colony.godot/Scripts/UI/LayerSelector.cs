using System;
using Godot;

namespace Colony.Godot.Scripts.UI;

public partial class LayerSelector : Control
{
    private VSlider _slider = null!;
    private Label _layerLabel = null!;
    
    public event Action<int>? LayerSelected;

    public void Initialize(int minimumLayerCount,  int maximumLayerCount, int selectedLayer)
    {
        if (maximumLayerCount < minimumLayerCount)
            throw new ArgumentException(
                "Maximum layer must be greater than or equal to minimum layer.");

        if (selectedLayer < minimumLayerCount ||
            selectedLayer > maximumLayerCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(selectedLayer));
        }

        BuildUI(minimumLayerCount, maximumLayerCount, selectedLayer);
    }

    private void BuildUI(int minimumLayerCount,  int maximumLayerCount, int selectedLayer)
    {
        var layout = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(100, 250),
            Alignment = BoxContainer.AlignmentMode.Center,
        };
        
        AddChild(layout);
        
        CreateLayerLabel(selectedLayer);
        layout.AddChild(_layerLabel);
        
        CreateSlider(minimumLayerCount,  maximumLayerCount, selectedLayer);
        layout.AddChild(_slider);

        UpdateLabel((int)_slider.Value);
    }

    private void OnSliderValueChanged(double value)
    {
        var layer = (int)value;

        UpdateLabel(layer);
        
        LayerSelected?.Invoke((int)value);
    }

    private void CreateLayerLabel(int layer)
    {
        _layerLabel = new Label
        {
            Text = $"Layer {layer}",
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        _layerLabel.CustomMinimumSize = new Vector2(100, 30);
    }

    private void CreateSlider(int minimumLayerCount,  int maximumLayerCount, int selectedLayer)
    {
        _slider = new VSlider
        {
            MinValue = minimumLayerCount,
            MaxValue = maximumLayerCount,
            Step = 1,
            Value = selectedLayer,
            CustomMinimumSize = new Vector2(30, 200),
            SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
        };

        _slider.ValueChanged += OnSliderValueChanged;
    }

    private void UpdateLabel(int selectedLayer)
    {
        _layerLabel.Text = $"Layer {selectedLayer}";
    }
}