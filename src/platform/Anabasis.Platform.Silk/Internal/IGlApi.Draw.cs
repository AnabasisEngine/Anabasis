﻿using Anabasis.Graphics.Abstractions;
using Silk.NET.OpenGL;

namespace Anabasis.Platform.Silk.Internal;

public partial interface IGlApi
{
    void DrawArrays(PrimitiveType primitiveType, int first, uint count);
    void DrawElements(PrimitiveType primitiveType, uint count, DrawElementsType indexType, long indexOffset);
    void ClearColor(Color color);
    void DrawArraysInstanced(PrimitiveType primitiveType, int first, uint count, uint instanceCount);
    void ClearFlags(ClearBufferMask mask);
}