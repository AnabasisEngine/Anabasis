﻿using Anabasis.Platform.Silk.Textures;
using Silk.NET.OpenGL;

namespace Anabasis.Platform.Silk.Internal;

internal partial class GlApi
{
    public TextureHandle CreateTexture(TextureTarget target) {
        _gl.CreateTextures(target, 1, out uint value);
        return new TextureHandle(value);
    }

    public void BindTextureUnit(uint unit, TextureHandle handle) {
        _gl.BindTextureUnit(unit, handle.Value);
    }

    public void DeleteTexture(TextureHandle handle) {
        _gl.DeleteTexture(handle.Value);
    }

    public TextureHandle GenTexture() => new(_gl.GenTexture());

    public void TextureView(TextureHandle texture, TextureTarget target, TextureHandle origtexture,
        SizedInternalFormat internalformat,
        uint minlevel, uint numlevels, uint minlayer, uint numlayers) {
        _gl.TextureView(texture.Value, target, origtexture.Value, internalformat, minlevel, numlevels, minlayer,
            numlayers);
    }

    public void TextureStorage1D(TextureHandle texture, uint levels, SizedInternalFormat internalformat, uint width) {
        _gl.TextureStorage1D(texture.Value, levels, internalformat, width);
    }

    public void TextureStorage2D(TextureHandle texture, uint levels, SizedInternalFormat internalformat, uint width,
        uint height) {
        _gl.TextureStorage2D(texture.Value, levels, internalformat, width, height);
    }

    public void TextureStorage3D(TextureHandle texture, uint levels, SizedInternalFormat internalformat, uint width,
        uint height,
        uint depth) {
        _gl.TextureStorage3D(texture.Value, levels, internalformat, width, height, depth);
    }

    public void TextureSubImage1D<T0>(TextureHandle texture, int level, int xoffset, uint width, PixelFormat format,
        PixelType type,
        in T0 pixels)
        where T0 : unmanaged {
        _gl.TextureSubImage1D(texture.Value, level, xoffset, width, format, type, in pixels);
    }

    public void TextureSubImage2D<T0>(TextureHandle texture, int level, int xoffset, int yoffset, uint width,
        uint height,
        PixelFormat format, PixelType type, in T0 pixels)
        where T0 : unmanaged {
        _gl.TextureSubImage2D(texture.Value, level, xoffset, yoffset, width, height, format, type, in pixels);
    }

    public void TextureSubImage3D<T0>(TextureHandle texture, int level, int xoffset, int yoffset, int zoffset,
        uint width, uint height,
        uint depth, PixelFormat format, PixelType type, in T0 pixels)
        where T0 : unmanaged {
        _gl.TextureSubImage3D(texture.Value, level, xoffset, yoffset, zoffset, width, height, depth, format, type, in
            pixels);
    }

    public void GenerateTextureMipmap(TextureHandle texture) => _gl.GenerateTextureMipmap(texture.Value);
}