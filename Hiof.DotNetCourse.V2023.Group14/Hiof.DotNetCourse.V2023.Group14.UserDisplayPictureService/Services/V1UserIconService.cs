using System.Drawing;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Data;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Services
{
    /// <summary>
    /// V1UserIconService implements V1UserIcon interface. Concrete
    /// implementation of the interface, that will connect to the database
    /// and execute the different operations.
    /// </summary>
    public class V1UserIconService : V1IUserIcon
    {
        private readonly UserIconContext _context;

        public V1UserIconService(UserIconContext context)
        {
            _context = context;
        }

        public async Task Add(V1UserIcon icon)
        {
            await _context.UserIcons.AddAsync(icon);
            await _context.SaveChangesAsync();
        }

        public async Task<V1UserIcon?> GetById(Guid id)
        {
            return await _context.UserIcons.FindAsync(id);

        }

        public async Task<V1UserIcon?> GetByUsername(string username)
        {
            return await _context.UserIcons.SingleOrDefaultAsync(u => u.Username == username);

        }

        public async Task Update(V1UserIcon icon)
        {
            var existingIcon = await _context.UserIcons.FindAsync(icon.Id);

            if (existingIcon != null)
            {
                existingIcon.Username = icon.Username;
                existingIcon.DisplayPicture = icon.DisplayPicture;

                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(Guid id)
        {
            var existingIcon = await _context.UserIcons.FindAsync(id);

            if (existingIcon != null)
            {
                _context.UserIcons.Remove(existingIcon);
                await _context.SaveChangesAsync();
            }
        }

        // Method used to get a hold on the images
        // for the GUI.
        public System.Drawing.Image ConvertByteArrayToImage(V1UserIcon icon)
        {

            using MemoryStream stream = new(icon.DisplayPicture);
            return System.Drawing.Image.FromStream(stream);
        }
    }
}

