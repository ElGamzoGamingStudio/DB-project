using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathToSuccess.Models;

namespace PathToSuccess.BL
{
    public class Buffer
    {
        public readonly List<Task> TaskBuffer;
        public readonly List<Step> StepBuffer;
        public List<Criteria> CriteriaBuffer;
        public List<TimeRule> TimeRuleBuffer; 

        public Buffer(List<Task> tasks, List<Step> steps)
        {
            TaskBuffer = tasks;
            StepBuffer = steps;
        }

        public Buffer(List<Task> tasks, List<Step> steps, List<Criteria> criterias, List<TimeRule> timeRules)
        {
            TaskBuffer = tasks;
            StepBuffer = steps;
            CriteriaBuffer = criterias;
            TimeRuleBuffer = timeRules;
        }

        public Buffer(Buffer bufferToCopy)
        {
            TaskBuffer = bufferToCopy.TaskBuffer.ToList();
            StepBuffer = bufferToCopy.StepBuffer.ToList();
            CriteriaBuffer = bufferToCopy.CriteriaBuffer.ToList();
            TimeRuleBuffer = bufferToCopy.TimeRuleBuffer.ToList();
        }
    }

    public static class ChangesBuffer
    {
        public static Buffer CurrentState { get; private set; }
        private static List<Buffer> _buffer;



        public static void Initialize()
        {
            _buffer=new List<Buffer>();
            var state = new Buffer(DAL.SqlRepository.Tasks.Cast<Task>().ToList(),
                                   DAL.SqlRepository.Steps.Cast<Step>().ToList(),
                                   DAL.SqlRepository.Criterias.Cast<Criteria>().ToList(),
                                   DAL.SqlRepository.TimeRules.Cast<TimeRule>().ToList());
            _buffer.Add(state);
            CurrentState = state;
        }

        public static void Undo()
        {
            int index = _buffer.IndexOf(CurrentState);
            if (index == 0) return;
            CurrentState = _buffer[index - 1];
        }

        public static void CaptureChanges(Buffer buf)
        {
            int index = _buffer.IndexOf(CurrentState);
            if (index != _buffer.Count - 1) _buffer.RemoveRange(index + 1, _buffer.Count - index - 1);
            _buffer.Add(buf);
            CurrentState = buf;
        }

        public static void SaveChanges()
        {
            foreach (var task in CurrentState.CriteriaBuffer)
            {
                var t = (Criteria)DAL.SqlRepository.Criterias.Find(task.Id);
                if (t == null)
                    DAL.SqlRepository.Criterias.Add(task);
                else
                {
                    if (t.Equals(task))
                        continue;
                    t.CurrentValue = task.CurrentValue;
                    t.TargetValue = task.TargetValue;
                    t.Unit = task.Unit;
                }
            }
            var temp2 = new List<Criteria>();
            foreach (var entity in DAL.SqlRepository.Criterias.Cast<Criteria>())
            {
                if (!CurrentState.CriteriaBuffer.Contains(entity))
                {
                    temp2.Add(entity);
                }
            }
            DAL.SqlRepository.Criterias.RemoveRange(temp2);

            foreach (var task in CurrentState.TimeRuleBuffer)
            {
                var t = (TimeRule)DAL.SqlRepository.TimeRules.Find(task.Id);
                if (t == null)
                    DAL.SqlRepository.TimeRules.Add(task);
                else
                {
                    if (t.Equals(task))
                        continue;
                    t.IsPeriodic = task.IsPeriodic;
                    t.IsUserApproved = task.IsUserApproved;
                    t.ScheduleId = task.ScheduleId;
                    t.Schedule = task.Schedule;
                }
            }
            var temp3 = new List<TimeRule>();
            foreach (var entity in DAL.SqlRepository.TimeRules.Cast<TimeRule>())
            {
                if (!CurrentState.TimeRuleBuffer.Contains(entity))
                {
                    temp3.Add(entity);
                }
            }
            DAL.SqlRepository.Criterias.RemoveRange(temp3);

            foreach (var task in CurrentState.TaskBuffer)
            {
                var t =(Task) DAL.SqlRepository.Tasks.Find(task.Id);
                if (t == null)
                    DAL.SqlRepository.Tasks.Add(task);
                else
                {
                    if(t.Equals(task))
                        continue;
                    t.ImportanceName = task.ImportanceName;
                    t.Importance = task.Importance;
                    t.ParentId = task.ParentId;
                    t.Parent = task.Parent;
                    t.UrgencyName = task.UrgencyName;
                    t.Urgency = task.Urgency;
                    t.BeginDate = task.BeginDate;
                    t.EndDate = task.EndDate;
                    t.Description = task.Description;
                }
            }
            var temp = new List<Task>();
            foreach (var entity in DAL.SqlRepository.Tasks.Cast<Task>())
            {
                if (Task.GetOldestParent(entity).Id==Application.CurrentTree.MainTaskId && !CurrentState.TaskBuffer.Contains(entity))
                {
                    temp.Add(entity);
                }
            }
            DAL.SqlRepository.Tasks.RemoveRange(temp);

            foreach (var step in CurrentState.StepBuffer)
            {
                var t = (Step)DAL.SqlRepository.Steps.Find(step.Id);
                if (t == null)
                    DAL.SqlRepository.Steps.Add(step);
                else
                {
                    if (t.Equals(step))
                        continue;
                    t.ImportanceName = step.ImportanceName;
                    t.Importance = step.Importance;
                    t.TaskId = step.TaskId;
                    t.ParentTask = step.ParentTask;
                    t.UrgencyName = step.UrgencyName;
                    t.Urgency = step.Urgency;
                    t.BeginDate = step.BeginDate;
                    t.EndDate = step.EndDate;
                    t.Description = step.Description;
                    t.CriteriaId = step.CriteriaId;
                    t.Criteria = step.Criteria;
                    t.TimeRuleId = step.TimeRuleId;
                    t.TimeRule = step.TimeRule;
                }
            }
            var temp1 = new List<Step>();
            foreach (var entity in DAL.SqlRepository.Steps.Cast<Step>())
            {
                if (Task.GetOldestParent(entity.ParentTask).Id == Application.CurrentTree.MainTaskId && !CurrentState.StepBuffer.Contains(entity))
                {
                    temp1.Add(entity);
                }
            }
            DAL.SqlRepository.Steps.RemoveRange(temp1);

            

            DAL.SqlRepository.Save();
        }

        public static void Redo()
        {
            int index = _buffer.IndexOf(CurrentState);
            if (index == _buffer.Count - 1) return;
            CurrentState = _buffer[index + 1];
        }
    }
}
