using SMIC.EntityFrameworkCore;
using SMIC.MyTasks;
using System.Collections.Generic;
using System.Linq;

namespace SMIC.EntityFrameworkCore.Seed
{
    public class DefaultTestDataForTask
    {
        private readonly SMICDbContext _context;

        private static readonly List<Task> _tasks;

        public DefaultTestDataForTask(SMICDbContext context)
        {
            _context = context;
        }

        static DefaultTestDataForTask()
        {
            _tasks = new List<Task>()
            {
                new Task("Learning ABP deom", "Learning how to use abp framework to build a MPA application."),
                new Task("Make Lunch", "Cook 2 dishs")
            };
        }

        public void Create()
        {
            foreach (var task in _tasks)
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();
            }
        }

    }
}
