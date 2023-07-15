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
            ResourceExtension.LightScheme = SchemeExtensions.GetScheme(value);
        }
    }
    private Color dark;
    public Color Dark
    {
        get => this.dark;
        set
        {
            this.dark = value;
            ResourceExtension.DarkScheme = SchemeExtensions.GetScheme(value);
        }
    }

    public MaterialStyles()
    {
        this.InitializeComponent();
        ResourceExtension.MaterialDictionaries = this.MergedDictionaries;
        ResourceExtension.UpdateMaterialColors();

        Application.Current.RequestedThemeChanged += (sender, e) =>
        {
            ResourceExtension.UpdateMaterialColors();
        };
    }
}
