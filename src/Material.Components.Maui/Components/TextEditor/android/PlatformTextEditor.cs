using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Graphics.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using PointF = Microsoft.Maui.Graphics.PointF;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace Material.Components.Maui.Platform;

public class PlatformTextEditor : TextView
{
    public override int SelectionStart => Selection.GetSelectionStart(this.EditableText);
    public override int SelectionEnd => Selection.GetSelectionEnd(this.EditableText);
    public override bool OnCheckIsTextEditor() => true;
    protected override bool DefaultEditable => true;
    public override bool FreezesText
    {
        get => true;
        set => base.FreezesText = value;
    }
    protected override IMovementMethod DefaultMovementMethod => ArrowKeyMovementMethod.Instance;

    public new event EventHandler<EditTextChangedEventArgs> TextChanged;

    public event EventHandler<SelectionChangedArgs> SelectionChanged;

    public PlatformTextEditor(Context context, IDrawable drawable = null)
        : base(context, null, Android.Resource.Attribute.EditTextStyle, 0)
    {
        this.scale = this.Resources.DisplayMetrics.Density;
        this.canvas = new PlatformCanvas(context);
        this.scalingCanvas = new ScalingCanvas(this.canvas);
        this.Drawable = drawable;

        this.Focusable = true;
        this.FocusableInTouchMode = true;
        this.SetTextIsSelectable(true);
        this.SetCursorVisible(false);
        this.SetPadding(0, 0, (int)(3 * this.scale), 0);
    }

    protected override void OnSelectionChanged(int selStart, int selEnd)
    {
        base.OnSelectionChanged(selStart, selEnd);
        this.SelectionChanged?.Invoke(this, new(selStart, selEnd));
    }

    protected override void OnTextChanged(
        Java.Lang.ICharSequence text,
        int start,
        int lengthBefore,
        int lengthAfter
    )
    {
        base.OnTextChanged(text, start, lengthBefore, lengthAfter);
        this.TextChanged?.Invoke(this, new(this.Text, new(this.SelectionStart, this.SelectionEnd)));
    }

    public override void SetText(Java.Lang.ICharSequence text, BufferType type)
    {
        base.SetText(text, BufferType.Editable);
    }

    public void SetSelection(int index)
    {
        Selection.SetSelection(this.EditableText, index);
    }

    public void SetSelection(int start, int end)
    {
        Selection.SetSelection(this.EditableText, start, end);
    }

    //------------------------------------------------------------------------------------------------//

    public Color BackgroundColor
    {
        get => this.backgroundColor;
        set
        {
            this.backgroundColor = value;
            this.Invalidate();
        }
    }

    public IDrawable Drawable
    {
        get => this.drawable;
        set
        {
            this.drawable = value;
            this.Invalidate();
        }
    }

    int width;
    int height;
    readonly PlatformCanvas canvas;
    readonly ScalingCanvas scalingCanvas;
    IDrawable drawable;
    readonly float scale = 1;
    Color backgroundColor;

    IGraphicsView graphicsView;
    RectF bounds;
    bool dragStarted;
    PointF[] lastMovedViewPoints = Array.Empty<PointF>();
    bool pressedContained = false;

    public override void Draw(Canvas androidCanvas)
    {
        if (this.drawable == null)
            return;

        var dirtyRect = new RectF(0, 0, this.width, this.height);

        this.canvas.Canvas = androidCanvas;
        if (this.backgroundColor != null)
        {
            this.canvas.FillColor = this.backgroundColor;
            this.canvas.FillRectangle(dirtyRect);
            this.canvas.FillColor = Colors.White;
        }

        this.scalingCanvas.ResetState();
        this.scalingCanvas.Scale(this.scale, this.scale);
        //Since we are using a scaling canvas, we need to scale the rectangle
        dirtyRect.Height /= this.scale;
        dirtyRect.Width /= this.scale;
        this.drawable.Draw(this.scalingCanvas, dirtyRect);
        this.canvas.Canvas = null;
    }

    protected override void OnSizeChanged(int width, int height, int oldWidth, int oldHeight)
    {
        base.OnSizeChanged(width, height, oldWidth, oldHeight);
        this.width = width;
        this.height = height;
    }

    protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
    {
        base.OnLayout(changed, left, top, right, bottom);
        if (changed)
        {
            var width = right - left;
            var height = bottom - top;
            this.bounds = new RectF(0, 0, width / this.scale, height / this.scale);
        }
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        base.OnTouchEvent(e);
        var touchCount = e.PointerCount;
        var touchPoints = new PointF[touchCount];
        for (var i = 0; i < touchCount; i++)
            touchPoints[i] = new PointF(e.GetX(i) / this.scale, e.GetY(i) / this.scale);

        var actionMasked = e.Action & MotionEventActions.Mask;

        switch (actionMasked)
        {
            case MotionEventActions.Move:
                this.TouchesMoved(touchPoints);
                break;
            case MotionEventActions.Down:
            case MotionEventActions.PointerDown:
                this.TouchesBegan(touchPoints);
                break;
            case MotionEventActions.Up:
            case MotionEventActions.PointerUp:
                this.TouchesEnded(touchPoints);
                break;
            case MotionEventActions.Cancel:
                this.TouchesCanceled();
                break;
        }

        return true;
    }

    public void TouchesBegan(PointF[] points)
    {
        this.dragStarted = false;
        this.lastMovedViewPoints = points;
        this.graphicsView?.StartInteraction(points);
        this.pressedContained = true;
    }

    public void TouchesMoved(PointF[] points)
    {
        if (!this.dragStarted)
        {
            if (points.Length == 1)
            {
                var deltaX = this.lastMovedViewPoints[0].X - points[0].X;
                var deltaY = this.lastMovedViewPoints[0].Y - points[0].Y;

                if (Math.Abs(deltaX) <= 3 && Math.Abs(deltaY) <= 3)
                    return;
            }
        }

        this.lastMovedViewPoints = points;
        this.dragStarted = true;
        this.pressedContained = points.Any(this.bounds.Contains);
        this.graphicsView?.DragInteraction(points);
    }

    public void TouchesEnded(PointF[] points)
    {
        this.graphicsView?.EndInteraction(points, this.pressedContained);
    }

    public void TouchesCanceled()
    {
        this.pressedContained = false;
        this.graphicsView?.CancelInteraction();
    }

    public override bool OnHoverEvent(MotionEvent e)
    {
        base.OnHoverEvent(e);
        ArgumentNullException.ThrowIfNull(e);

        var touchCount = e.PointerCount;
        var touchPoints = new PointF[touchCount];
        for (var i = 0; i < touchCount; i++)
            touchPoints[i] = new PointF(e.GetX(i) / this.scale, e.GetY(i) / this.scale);

        var actionMasked = e.Action & MotionEventActions.Mask;

        switch (actionMasked)
        {
            case MotionEventActions.HoverMove:
                this.graphicsView?.MoveHoverInteraction(touchPoints);
                break;
            case MotionEventActions.HoverEnter:
                this.graphicsView?.StartHoverInteraction(touchPoints);
                break;
            case MotionEventActions.HoverExit:
                this.graphicsView?.EndHoverInteraction();
                break;
        }

        return true;
    }

    public void Connect(IGraphicsView graphicsView) => this.graphicsView = graphicsView;

    public void Disconnect() => this.graphicsView = null;
}
