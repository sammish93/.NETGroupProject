using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes
{
    // We should think about splitting up each dataset into its own schema. For example, our 'User' class should only be accessible through our 'UserAccountService', and thus 
    // every class used by that service should be in their own schema (e.g. 'Accounts').
    [Table("Tests", Schema = "dbo")]
    public class DbOrmTestClass
    {
        // Annotations to designate/override serialisation by Entity Framework Core. 
        // You can specify many things such as data types, lengths, encoding, nullable etc.
        // *** Here's a list off annotations ***
        // https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("full_name")]
        public string Name { get; set; }
        [Column("age")]
        public int Age { get; set; }

        public DateTime lastActive { get; set; }
    }
}
