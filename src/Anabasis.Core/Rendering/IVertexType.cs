using Anabasis.Core.Handles;
using Silk.NET.OpenGL;

namespace Anabasis.Core.Rendering;

public interface IVertexType
{
    public static abstract void EstablishVertexFormat(VertexArrayBindingIndex bindingIndex, GL gl,
        VertexArrayHandle vertexArray);
}