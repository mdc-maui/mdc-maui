namespace Material.Components.Maui.Handlers;

public partial class TextEditorHandler : IViewHandler
{
    public static IPropertyMapper<BaseTextEditor, TextEditorHandler> Mapper { get; set; } =
        new PropertyMapper<BaseTextEditor, TextEditorHandler>(ViewMapper)
        {
            [nameof(BaseTextEditor.Drawable)] = MapDrawable,
            [nameof(BaseTextEditor.Text)] = MapText,
            [nameof(BaseTextEditor.SelectionRange)] = MapSelectionRange,
            [nameof(BaseTextEditor.FontSize)] = MapFontSize,
        };

    public static CommandMapper<BaseTextEditor, TextEditorHandler> CommandMapper { get; set; } =
        new(ViewCommandMapper) { [nameof(BaseTextEditor.Invalidate)] = MapInvalidate };

    public TextEditorHandler() : base(Mapper, CommandMapper) { }

    public TextEditorHandler(IPropertyMapper mapper) : base(mapper ?? Mapper, CommandMapper) { }

    public TextEditorHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper) { }
}
