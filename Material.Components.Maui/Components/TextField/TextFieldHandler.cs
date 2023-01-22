using Topten.RichTextKit;

namespace Material.Components.Maui.Core;

public partial class TextFieldHandler
{
    public static PropertyMapper<TextField, TextFieldHandler> Mapper =
        new()
        {
            [nameof(TextField.Text)] = MapText,
            [nameof(TextField.CaretPosition)] = MapCaretPosition,
            [nameof(TextField.InternalFocus)] = MapInternalFocus,
            [nameof(TextField.EnableTouchEvents)] = MapEnableTouchEvents,
            [nameof(TextField.IgnorePixelScaling)] = MapIgnorePixelScaling,
        };

    public static CommandMapper<TextField, TextFieldHandler> CommandMapper =
        new() { [nameof(TextField.InvalidateSurface)] = OnInvalidateSurface, };

    private readonly EditTextManager editTextManager;

    public TextFieldHandler() : base(Mapper, CommandMapper)
    {
        this.editTextManager = new EditTextManager(this);
    }

    public TextFieldHandler(PropertyMapper mapper, CommandMapper commands)
        : base(mapper ?? Mapper, commands ?? CommandMapper)
    {
        this.editTextManager = new EditTextManager(this);
    }

    private static void MapText(TextFieldHandler handler, TextField view)
    {
        if (handler.editTextManager.Text != view.Text)
        {
            handler.editTextManager.Text = view.Text;
            handler.editTextManager.Range = new TextRange(0);
        }
    }

    private static void MapCaretPosition(TextFieldHandler handler, TextField view)
    {
        handler.editTextManager.CaretPosition = view.CaretPosition;
    }
}
