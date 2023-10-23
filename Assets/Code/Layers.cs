public enum Layers
{
    Default         = 0,
    TransparentFX   = 1,
    IgnoreRaycast   = 2,
    Water           = 4,
    UI              = 5,
    Player          = 6,
    Ground          = 10,
    Environment     = 11
}
// Warning: Layer 31 is used internally by the Editor’s Preview window mechanics. To prevent clashes, do not use this layer.