using Beatemup.Ecs;
using NUnit.Framework;

public class ButtonBufferTests
{
    [Test]
    public void Test_HasBufferedAction()
    {
        // var button = new Button("name");

        var controlComponent = ControlComponent.Default();
        
        Assert.IsFalse(controlComponent.HasBufferedActions(nameof(ControlComponent.right)));
        controlComponent.buffer.Add(nameof(ControlComponent.right));

        Assert.IsTrue(controlComponent.HasBufferedActions(nameof(ControlComponent.right)));
    }
    
    [Test]
    public void Test_HasBufferedActions_List()
    {
        var controlComponent = ControlComponent.Default();
        Assert.IsFalse(controlComponent.HasBufferedActions("a", "b"));
        
        controlComponent.buffer.Add("a");
        Assert.IsFalse(controlComponent.HasBufferedActions("a", "b"));

        controlComponent.buffer.Add("b");
        Assert.IsTrue(controlComponent.HasBufferedActions("a", "b"));
        
        controlComponent.buffer.Add("c");
        Assert.IsFalse(controlComponent.HasBufferedActions("a", "b"));
    }
}
