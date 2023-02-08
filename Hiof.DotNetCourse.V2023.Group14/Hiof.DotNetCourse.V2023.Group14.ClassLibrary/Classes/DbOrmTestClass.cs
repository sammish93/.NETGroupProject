using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes
{
    [Table("Test", Schema = "dbo")]
    public class DbOrmTestClass
    {
        [Key]
        public int Id { get; set; }
        [Column("full_name")]
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
