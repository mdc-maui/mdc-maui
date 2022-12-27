using Microsoft.Maui.Controls.Internals;
using System.Reflection;

namespace Material.Components.Maui.Extensions;

internal static class ViewExtensions
{
    private static MethodInfo InvalidateMeasureMethod;

    internal static void SendInvalidateMeasure(this View view)
    {
        ((VisualElement)view.Parent)?.InvalidateMeasureNonVirtual(InvalidationTrigger.Undefined);
        ((VisualElement)view.Parent)?.InvalidateMeasureNonVirtual(
            InvalidationTrigger.VerticalOptionsChanged
        );
        ((VisualElement)view.Parent)?.InvalidateMeasureNonVirtual(
            InvalidationTrigger.HorizontalOptionsChanged
        );
        ((VisualElement)view.Parent)?.InvalidateMeasureNonVirtual(
            InvalidationTrigger.RendererReady
        );
        ((VisualElement)view.Parent)?.InvalidateMeasureNonVirtual(
            InvalidationTrigger.MeasureChanged
        );
        ((VisualElement)view.Parent)?.InvalidateMeasureNonVirtual(
            InvalidationTrigger.SizeRequestChanged
        );
    }

    private static MethodInfo GetMethod()
    {
        var type = typeof(VisualElement);
        return type.GetMethod("InvalidateMeasure", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}
