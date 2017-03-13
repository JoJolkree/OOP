using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{

    public class TestHandler
    {
        Observer observer;
        public void Initialize<T>(ObservableStack<T> stack)
        {
            observer = new Observer();
            stack.Add(observer);
        }

        public string GetLog()
        {
            return observer.log;
        }
    }

    public interface IObserver
    {
        void HandleEvent(object eventData);
    }

    public class Observer : IObserver
    {
        internal string log;

        public void HandleEvent(object eventData)
        {
            log += eventData.ToString();
        }
    }

    public interface IObservable
    {
        void Add(IObserver observer);
        void Remove(IObserver observer);
    }


    public class ObservableStack<T> : IObservable
    {

        public delegate void NotifyDelegate(StackEventData<T> observers);

        public event NotifyDelegate onChange;

        public void Add(IObserver observer)
        {
            onChange += observer.HandleEvent;
        }

        public void Remove(IObserver observer)
        {
            onChange -= observer.HandleEvent;
        }

        List<T> data = new List<T>();

        public void Push(T obj)
        {
            data.Add(obj);
            onChange(new StackEventData<T> {IsPushed = true, Value = obj});
        }

        public T Pop()
        {
            if (data.Count == 0)
                throw new InvalidOperationException();
            var result = data[data.Count - 1];
            onChange(new StackEventData<T> { IsPushed = false, Value = result });
            return result;

        }
    }
}
