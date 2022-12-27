namespace SampleApp.Views;

public class TestScrollView : ScrollView
{
    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        return base.MeasureOverride(widthConstraint, heightConstraint);
    }

    public override SizeRequest Measure(
        double widthConstraint,
        double heightConstraint,
        MeasureFlags flags = MeasureFlags.None
    )
    {
        return base.Measure(widthConstraint, heightConstraint, flags);
    }

    protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
    {
        return base.OnMeasure(widthConstraint, heightConstraint);
    }

    protected override void OnChildMeasureInvalidated()
    {
        base.OnChildMeasureInvalidated();
    }
}
