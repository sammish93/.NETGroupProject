using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1
{
    /// <summary>
    /// Interface that defines the methods that will be
    /// supported by the V1DisplayPictureController API.
    /// </summary>
    public interface V1IUserIcon
    {
        Task<V1UserIcon?> GetById(Guid id);
        Task<V1UserIcon?> GetByUsername(string username);
        Task Add(V1UserIcon icon);
        Task Update(V1UserIcon icon);
        Task Delete(Guid id);
    }
}

