﻿using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ReadingGoalService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hiof.DotNetCourse.V2023.Group14.ReadingGoalService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0/goals")]
    public class V1ReadingGoalsController : Controller
    {
        private readonly ReadingGoalsContext _readingGoalsContext;

        public V1ReadingGoalsController(ReadingGoalsContext readingGoalsContext)
        {
            _readingGoalsContext= readingGoalsContext;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateReadingGoal(V1ReadingGoals readingGoal)
        {
            if (readingGoal != null)
            {
                var existingReadingGoal = await _readingGoalsContext.ReadingGoals
                    .Where(x => (x.UserId == readingGoal.UserId) && 
                    ((x.GoalStartDate.Date <= readingGoal.GoalStartDate.Date && x.GoalEndDate.Date >= readingGoal.GoalStartDate.Date)
                    || (x.GoalStartDate.Date <= readingGoal.GoalEndDate.Date && x.GoalEndDate.Date >= readingGoal.GoalEndDate.Date)))
                    .FirstOrDefaultAsync();

                if (existingReadingGoal != null)
                {
                    return BadRequest("You already have a reading goal during this time period!");
                }

                

                await _readingGoalsContext.ReadingGoals.AddAsync(readingGoal);
                await _readingGoalsContext.SaveChangesAsync();

                return Ok();
            }

            return BadRequest("Reading goal cannot be null");
        }

        [HttpGet]
        [Route("getGoalByUserId")]
        public async Task<ActionResult> GetGoalByUserId(Guid userId)
        {
            var goal = await (from goals in _readingGoalsContext.ReadingGoals
                              where goals.UserId == userId
                              select goals).ToListAsync();
            if (goal.IsNullOrEmpty())
            {
                return NotFound("There are no existing readings!");
            }
            else
            {
                return Ok(goal);
            }
        }

        [HttpGet]
        [Route("getGoalId")]
        public async Task<ActionResult> GetGoalIdUsingUserIdAndDate(Guid userId, DateTime goalDate)
        {
            var goal = await _readingGoalsContext.ReadingGoals
                    .Where(x => x.UserId == userId && x.GoalStartDate.Date <= goalDate.Date && x.GoalEndDate.Date >= goalDate.Date)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

            if (goal == Guid.Empty)
            {
                return NotFound("No reading goals found!");
            }

            return Ok(goal);
        }
        [HttpGet]
        [Route("getRecentGoals")]
        public async Task<ActionResult> GetRecentReadingGoals(Guid userId)
        {
            var goal = await _readingGoalsContext.ReadingGoals
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.GoalEndDate.Date)
                .FirstOrDefaultAsync();

            if(goal == null) 
            {
                return NotFound("No Goals Yet");
            }
            return Ok(goal);
        }

        [HttpPut]
        [Route("incrementGoal")]
        public async Task<ActionResult> IncrementReadingGoal(Guid Id, int incrementAmount)
        {
            var goal = await _readingGoalsContext.ReadingGoals
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (goal == null)
            {
                return NotFound();
            }

            goal.GoalCurrent += incrementAmount;
            goal.LastUpdated = DateTime.UtcNow;

            await _readingGoalsContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("modify")]
        public async Task<ActionResult> ModifyReadingGoal(Guid id, V1ReadingGoals updatedReadingGoal)
        {
            var existingReadingGoal = await _readingGoalsContext.ReadingGoals.FindAsync(id);

            if (existingReadingGoal == null)
            {
                return NotFound("Reading Goal doesn't exist");
            }

            existingReadingGoal.GoalStartDate = updatedReadingGoal.GoalStartDate;
            existingReadingGoal.GoalEndDate = updatedReadingGoal.GoalEndDate;
            existingReadingGoal.GoalTarget = updatedReadingGoal.GoalTarget;
            existingReadingGoal.LastUpdated = DateTime.UtcNow;


            _readingGoalsContext.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult> DeleteReadingGoal(Guid id)
        {
            var existingReadingGoal = await _readingGoalsContext.ReadingGoals.FindAsync(id);

            if (existingReadingGoal == null)
            {
                return NotFound("Reading goal doesn't exist");
            }

            _readingGoalsContext.ReadingGoals.Remove(existingReadingGoal);
            await _readingGoalsContext.SaveChangesAsync();

            return Ok();
        }


    }
}
