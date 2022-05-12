namespace Anabasis.Platform.Abstractions;

public enum AnabasisPlatformLoopStep
{
    Initialization,
    PostInitialization,
    PreUpdate,
    Update,
    PostUpdate,
    PreRender,
    Render,
    PostRender,
    // TimeUpdate,
}