using Beatemup.Ecs;
using NUnit.Framework;

public class ButtonBufferTests
{
    [Test]
    public void TestDoubleTap()
    {
        // var button = new Button("name");

        var controlComponent = ControlComponent.Default();
        
        Assert.IsFalse(controlComponent.HasBufferedAction(nameof(ControlComponent.right), 2));

        controlComponent.right.UpdatePressed(true);
        controlComponent.buffer.Add(nameof(ControlComponent.right));
        
        Assert.IsFalse(controlComponent.HasBufferedAction(nameof(ControlComponent.right), 2));

        controlComponent.right.UpdatePressed(true);
        controlComponent.buffer.Add(nameof(ControlComponent.right));
        
        Assert.IsTrue(controlComponent.HasBufferedAction(nameof(ControlComponent.right), 2));
    }
}
