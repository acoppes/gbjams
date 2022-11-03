using Beatemup.Ecs;
using NUnit.Framework;

public class ButtonBufferTests
{
    [Test]
    public void TestDoubleTap()
    {
        var button = Button.Default();
        
        button.UpdatePressed(false);
        Assert.IsFalse(button.doubleTap);
        
        button.UpdatePressed(true);
        Assert.IsFalse(button.doubleTap);
        
        button.UpdatePressed(true);
        Assert.IsFalse(button.doubleTap);
        
        button.UpdatePressed(false);
        Assert.IsFalse(button.doubleTap);
        
        button.UpdatePressed(true);
        Assert.IsTrue(button.doubleTap);
        
        button.ClearBuffer();
        button.UpdatePressed(true);
        Assert.IsFalse(button.doubleTap);
    }
}
