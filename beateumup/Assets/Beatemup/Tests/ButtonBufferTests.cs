using Beatemup.Ecs;
using NUnit.Framework;

public class ButtonBufferTests
{
    [Test]
    public void TestDoubleTap()
    {
        // var button = new Button("name");

        var controlComponent = ControlComponent.Default();
        
        Assert.IsFalse(controlComponent.IsPreviousAction(nameof(ControlComponent.right)));

        controlComponent.right.UpdatePressed(true);
        controlComponent.buffer.Add(nameof(ControlComponent.right));
        
        Assert.IsFalse(controlComponent.IsPreviousAction(nameof(ControlComponent.right)));

        controlComponent.right.UpdatePressed(true);
        controlComponent.buffer.Add(nameof(ControlComponent.right));
        
        Assert.IsTrue(controlComponent.IsPreviousAction(nameof(ControlComponent.right)));
    }
}
