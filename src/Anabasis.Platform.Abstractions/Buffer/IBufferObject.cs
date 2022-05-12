﻿using Anabasis.Platform.Abstractions.Resources;

namespace Anabasis.Platform.Abstractions.Buffer;

public interface IBufferObject<T> : IGraphicsResource
    where T : unmanaged
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="offset"></param>
    /// <remarks>
    /// This call *may* change the actively bound buffer object(s) if necessary for the particular backend graphics api
    /// </remarks>
    void LoadData(ReadOnlySpan<T> data, int offset = 0);
}