﻿using System.Runtime.InteropServices;
using Anabasis.Graphics.Abstractions.Buffer;
using Anabasis.Platform.Silk.Internal;
using Anabasis.Utility;
using Silk.NET.OpenGL;

namespace Anabasis.Platform.Silk.Buffers;

public class SilkVertexArray<TIndex> : SilkGlObject<VertexArrayHandle>, IVertexArray<TIndex>
    where TIndex : unmanaged
{
    internal SilkVertexArray(IGlApi gl) : base(gl, gl.CreateVertexArray()) {
    }


    public override IDisposable Use() {
        Gl.BindVertexArray(Handle);
        return new GenericDisposer(() => Gl.BindVertexArray(new VertexArrayHandle(0)));
    }

    public override void Dispose() {
        GC.SuppressFinalize(this);
        Gl.DeleteVertexArray(Handle);
    }

    public void BindVertexBuffer<TVertex>(IBufferObject<TVertex> bufferObject, IBindingIndex bindingIndex)
        where TVertex : unmanaged {
        BufferObjectHandle buffer = Guard.IsType<SilkBufferObject<TVertex>>(bufferObject).Handle;
        SilkBindingIndex idx = Guard.IsType<SilkBindingIndex>(bindingIndex);
        Gl.VertexArrayVertexBuffer(Handle, idx, buffer, 0, (uint)Marshal.SizeOf<TVertex>());
    }

    public void BindIndexBuffer(IBufferObject<TIndex> bufferObject) {
        BufferObjectHandle buffer = Guard.IsType<SilkBufferObject<TIndex>>(bufferObject).Handle;
        Gl.VertexArrayElementBuffer(Handle, buffer);
    }

    public void DrawArrays(DrawMode drawMode, int first, uint count) {
        PrimitiveType primitiveType = drawMode switch {
            DrawMode.Triangles => PrimitiveType.Triangles,
            _ => throw new ArgumentOutOfRangeException(nameof(drawMode), drawMode, null),
        };
        Gl.DrawArrays(primitiveType, first, count);
    }

    public void DrawArraysInstanced(DrawMode drawMode, int first, uint count, uint instances) {
        PrimitiveType primitiveType = drawMode switch {
            DrawMode.Triangles => PrimitiveType.Triangles,
            _ => throw new ArgumentOutOfRangeException(nameof(drawMode), drawMode, null),
        };
        Gl.DrawArraysInstanced(primitiveType, first, count, instances);
    }

    public unsafe void DrawElements(DrawMode drawMode, uint count, uint indexOffset) {
        PrimitiveType primitiveType = drawMode switch {
            DrawMode.Triangles => PrimitiveType.Triangles,
            _ => throw new ArgumentOutOfRangeException(nameof(drawMode), drawMode, null),
        };
        DrawElementsType indexType = Type.GetTypeCode(typeof(TIndex)) switch {
            TypeCode.Byte => DrawElementsType.UnsignedByte,
            TypeCode.UInt16 => DrawElementsType.UnsignedShort,
            TypeCode.UInt32 => DrawElementsType.UnsignedInt,
            _ => throw new ArgumentOutOfRangeException(nameof(TIndex)),
        };
        Gl.DrawElements(primitiveType, count, indexType, (indexOffset * sizeof(TIndex)));
    }
}