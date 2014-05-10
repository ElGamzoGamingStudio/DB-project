using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("tree", Schema="public")]
    public class Tree
    {
        [Key]
        [Column("tree_id")]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int TreeId { get; set; }

        [Column("user_login")]
        public string TreeUserLogin { get; set; }

        [ForeignKey("TreeUserLogin")]
        public User TreeUser { get; set; }

        [Column("main_task_id")]
        public int MainTaskId { get; set; }

        [ForeignKey("MainTaskId")]
        public Task MainTask { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("last_changes_time")]
        public DateTime LastChangesTime { get; set; }

        public Tree() { }
        public Tree(Tree toCopy)
        {
            Description = toCopy.Description;
            LastChangesTime = toCopy.LastChangesTime;
            Name = toCopy.Name;
            MainTask = new Task(toCopy.MainTask);
            MainTaskId = toCopy.MainTaskId;
            TreeUserLogin = toCopy.TreeUserLogin;
            TreeUser = toCopy.TreeUser;
            TreeId = toCopy.TreeId;
        }
        private static Models.Task generateRoot()
        {
            var u = Urgency.GetLowestUrgency();
            var i = Importance.GetLowestImportance();
            var c = Criteria.CreateCriteria(0, 1, "times");

            return Task.CreateTask(DateTime.Now, Interval.PIOS, u.UrgencyName, i.ImportanceName, i, u, c.Id, c, "Root task", null, -1);
        }

        public static Tree CreateTree(User user, string userLogin, string name, string description)
        {
            var set = DAL.SqlRepository.Trees;
            var tree = (Tree)set.Create(typeof(Tree));

            tree.TreeUser = user;
            tree.TreeUserLogin = userLogin;
            tree.MainTask = generateRoot();
            tree.MainTaskId = tree.MainTask.Id;
            tree.Name = name;
            tree.Description = description;
            tree.LastChangesTime = DateTime.Now;

            set.Add(tree);
            DAL.SqlRepository.Save();

            return tree;
        }

        public static void DeleteTree(Tree tree) //TODO: recursively delete all tasks and steps?
        {
            var set = DAL.SqlRepository.Trees;
            var t = set.Find(tree.TreeId);
            if (t != null)
            {
                set.Remove(t);
                DAL.SqlRepository.Save();
            }
        }

        public static List<Tree> FindTreesForUser(User user)
        {
            return DAL.SqlRepository.Trees
                .Cast<Tree>()
                .Where(x => x.TreeUser.Login == user.Login)
                .ToList<Tree>();
        }

        public static Tree FindTreeWithRoot(Task mainTask)
        {
            return DAL.SqlRepository.Trees
                .Cast<Tree>()
                .FirstOrDefault(x => x.MainTaskId == mainTask.Id);
        }

    }
}
