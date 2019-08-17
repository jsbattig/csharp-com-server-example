using System;
using System.Threading;
using System.Threading.Tasks;
using TestComObject;

namespace ConsoleAppTest
{
  class Program
  {
    private const int COMCallsCount = 50;
    private const int COMObjectsCount = 1000;
    private const int ThreadsCount = 10;

    static void Main(string[] args)
    {
      Type comType = Type.GetTypeFromProgID("TestComObject.Person");
      IPerson[] person = new IPerson[COMObjectsCount];
      var initialTicks = Environment.TickCount;
      for (var i = 0; i < person.Length; i++)
        person[i] = (IPerson)Activator.CreateInstance(comType);
      Console.WriteLine(person.Length.ToString() + " object created in " + (Environment.TickCount - initialTicks).ToString() + "ms");
      initialTicks = Environment.TickCount;
      Thread[] threads = new Thread[ThreadsCount];
      int callsCount = 0;
      for (var threadId = 0; threadId < ThreadsCount; threadId++)
      {
        (threads[threadId] = new Thread((object id) =>
        {
          for (var i = 0; i < COMObjectsCount; i++)
          {
            if(i % ThreadsCount == (int)id)
              for (var j = 0; j < COMCallsCount; j++)
              {
                person[i].FirstName = "Name " + i.ToString() + " " + j.ToString();
                Interlocked.Increment(ref callsCount);
                var firstName = person[i].FirstName;
                Interlocked.Increment(ref callsCount);
              }
          }
        })).Start(threadId);
      }
      foreach (var thread in threads)
        thread.Join();
      Console.WriteLine(callsCount.ToString() + " calls in " + (Environment.TickCount - initialTicks).ToString() + "ms");
      Console.ReadLine();
    }
  }
}
