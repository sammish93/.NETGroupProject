using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    public class V1LibraryCollection
    {
        public Guid? UserId { get; set; }
        public int? Items { get; set; }
        public IList<V1LibraryEntry>? Entries { get; set; }

        public V1LibraryCollection() { }
    }
}
