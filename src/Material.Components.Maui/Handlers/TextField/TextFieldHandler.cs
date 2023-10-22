namespace Material.Components.Maui.Handlers;

public partial class TextFieldHandler : IViewHandler
{
    public static IPropertyMapper<TextField, TextFieldHandler> Mapper { get; set; } =
        new PropertyMapper<TextField, TextFieldHandler>(ViewMapper)
        {
            [nameof(TextField.Drawable)] = MapDrawable,
            [nameof(TextField.Text)] = MapText,
            [nameof(TextField.FontSize)] = MapFontSize,
            [nameof(TextField.FontFamily)] = MapFontAttributes,
            [nameof(TextField.FontWeight)] = MapFontAttributes,
            [nameof(TextField.FontIsItalic)] = MapFontAttributes,
            [nameof(TextField.SelectionRange)] = MapSelectionRange,
            [nameof(TextField.TextAlignment)] = MapTextAlignment,
            [nameof(TextField.EditablePadding)] = MapEditablePadding,
            [nameof(TextField.InputType)] = MapInputType,
            [nameof(TextField.IsReadOnly)] = MapIsReadOnly,
        };

    public static CommandMapper<TextField, TextFieldHandler> CommandMapper { get; set; } =
        new(ViewCommandMapper) { [nameof(TextField.Invalidate)] = MapInvalidate };

    public TextFieldHandler() : base(Mapper, CommandMapper) { }

    public TextFieldHandler(IPropertyMapper mapper) : base(mapper ?? Mapper, CommandMapper) { }

    public TextFieldHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper) { }

    static partial void MapText(TextFieldHandler handler, TextField virtualView);

    static partial void MapSelectionRange(TextFieldHandler handler, TextField virtualView);

    static partial void MapFontSize(TextFieldHandler handler, TextField virtualView);

    static partial void MapFontAttributes(TextFieldHandler handler, TextField virtualView);

    static partial void MapTextAlignment(TextFieldHandler handler, TextField virtualView);

    static partial void MapEditablePadding(TextFieldHandler handler, TextField virtualView);

    static partial void MapInputType(TextFieldHandler handler, TextField virtualView);

    static partial void MapIsReadOnly(TextFieldHandler handler, TextField virtualView);
}
