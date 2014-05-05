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
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
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

        public Tree(User user, Task mainTask, string name, string description)
        {
            TreeUser = user;
            TreeUserLogin = user.Login;
            MainTask = mainTask;
            MainTaskId = mainTask.Id;
            Name = name;
            Description = description;
            LastChangesTime = DateTime.Now;
        }

        public static void CreateTree(Tree tree) // TODO: create default nodes
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Tree>();
            set.Add(tree);
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        public static void DeleteTree(Tree tree) //TODO: recursively delete all tasks and steps?
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Tree>();
            var t = set.Find(tree.TreeId);
            if (t != null)
            {
                set.Remove(t);
                DAL.SqlRepository.DBContext.SaveChanges();
            }
        }

        public static List<Tree> FindTreesForUser(User user)
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Tree>()
                .Cast<Tree>()
                .Where(x => x.TreeUser.Login == user.Login)
                .ToList<Tree>();
        }

        public static Tree FindTreeWithRoot(Task mainTask)
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Tree>()
                .Cast<Tree>()
                .FirstOrDefault(x => x.MainTaskId == mainTask.Id);
        }
        public static List<Step> GetAllChildrenSteps()
        {
            //TODO: should return children steps of all levels of this tree
            return null;
        }

    }
}
