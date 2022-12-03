namespace Material.Components.Maui.Styles;

public partial class MaterialStyles : ResourceDictionary
{
    private Color light;
    public Color Light
    {
        get => this.light;
        set
        {
            this.light = value;
            MaterialComponentsExtensions.LightScheme = SchemeExtensions.GetScheme(value);
        }
    }
    private Color dark;
    public Color Dark
    {
        get => this.dark;
        set
        {
            this.dark = value;
            MaterialComponentsExtensions.DarkScheme = SchemeExtensions.GetScheme(value);
        }
    }

    public MaterialStyles()
    {
        this.InitializeComponent();
        MaterialComponentsExtensions.ColorRes = this.MergedDictionaries.First(
            x => x.GetType() == typeof(Colors)
        );
        MaterialComponentsExtensions.UpdateMaterialColors();

        Application.Current.RequestedThemeChanged += (sender, e) =>
        {
            MaterialComponentsExtensions.UpdateMaterialColors();
        };
    }
}
