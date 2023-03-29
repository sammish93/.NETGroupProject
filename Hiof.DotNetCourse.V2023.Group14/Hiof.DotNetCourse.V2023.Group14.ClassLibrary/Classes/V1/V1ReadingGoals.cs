using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    [Table("reading_goals", Schema ="dbo")]
    public class V1ReadingGoals
    {
        [Key]
        [Column("id", TypeName = "char(36)")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Column("user_id", TypeName = "nvarchar(36)")]
        public Guid UserId { get; set; }
        [Column("goal_start", TypeName = "datetime")]
        public DateTime GoalStartDate { get; set; }
        [Column("goal_end", TypeName = "datetime")]
        public DateTime GoalEndDate { get; set;}
        [Column("goal_target")]
        public int GoalTarget { get; set; }
        [Column("goal_curr")]
        public int GoalCurrent { get; set; }
        [Column("last_updated", TypeName = "datetime")]
        public DateTime LastUpdated { get; set; }

        public V1ReadingGoals(V1User user, DateTime goalStartDate, DateTime goalEndDate, int goalTarget, int currentTarget)
        {
            Id = Guid.NewGuid();
            UserId = user.Id;
            GoalStartDate = goalStartDate;
            GoalEndDate = goalEndDate;
            GoalTarget = goalTarget;
            GoalCurrent = currentTarget;
            LastUpdated = DateTime.UtcNow;
        }

        public V1ReadingGoals() { }
    }
}
