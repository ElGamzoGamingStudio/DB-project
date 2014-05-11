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
            DAL.SqlRepository.Tasks.RemoveRange(DAL.SqlRepository.Tasks.Cast<Task>().ToList());
            DAL.SqlRepository.Tasks.AddRange(CurrentState.TaskBuffer);
            DAL.SqlRepository.Steps.RemoveRange(DAL.SqlRepository.Steps.Cast<Step>().ToList());
            DAL.SqlRepository.Steps.AddRange(CurrentState.StepBuffer);
            DAL.SqlRepository.Criterias.RemoveRange(DAL.SqlRepository.Criterias.Cast<Criteria>().ToList());
            DAL.SqlRepository.Criterias.AddRange(CurrentState.CriteriaBuffer);
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
