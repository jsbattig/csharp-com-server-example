using System;
using System.Threading;
using TestComObject;
using CLRCppCOMServerTest;
using System.Runtime.InteropServices;

namespace ConsoleAppTest
{
  class Program
  {
    private const int COMCallsCount = 50;
    private const int COMObjectsCount = 1000;
    private const int ThreadsCount = 10;
    private const int LoopsForCallsWithNoInterface = 10000;

    static void Main(string[] args)
    {
      TestCPPCOM();
      Console.WriteLine("******************");
      TestCPPCOMNoInterface();
      Console.WriteLine("******************");
      TestCSharpCOM();
      Console.WriteLine("");
      Console.WriteLine("Press any key to finish");
      Console.ReadLine();
    }

    static void TestCPPCOMNoInterface()
    {
      ClassWithNoInterface obj = new ClassWithNoInterface();
      Console.WriteLine("ClassWithNoInterface.SelfTest(): " + obj.SelfTest());
      var initialTicks = Environment.TickCount;
      for (var i = 0; i < LoopsForCallsWithNoInterface; i++)
        obj.SelfTest();
      Console.WriteLine(LoopsForCallsWithNoInterface + " calls to ClassWithNoInterface.SelfTest(): " + (Environment.TickCount - initialTicks).ToString() + "ms");
      SimpleClass obj2 = new SimpleClass();
      Console.WriteLine("SimpleClass.SelfTest(): " + obj2.SelfTest());
      // Console.WriteLine("Echo:" + obj.Echo(obj2)); This is not supported because Echo receives an object as parameter and ClassWithNoInterface is treated as a COM object automatically
      initialTicks = Environment.TickCount;
      for (var i = 0; i < LoopsForCallsWithNoInterface; i++)
        obj2.SelfTest();
      Console.WriteLine(LoopsForCallsWithNoInterface + " calls to SimpleClass.SelfTest(): " + (Environment.TickCount - initialTicks).ToString() + "ms");
    }

    static void TestCPPCOM()
    {
      Type CPPcomType = Type.GetTypeFromProgID("CLRCppCOMServerTest.ExplosiveClass");
      IExplosiveClass obj = (IExplosiveClass)Activator.CreateInstance(CPPcomType);
      Console.WriteLine("C++ SelfTest: " + obj.SelfTest());
      Console.WriteLine("First call to AutoInc(): " + obj.AutoInc());
      try
      {
        obj.Explode();
      } catch (Exception e)
      {
        Console.WriteLine(e.Message + ". This was expected");
      }
      try
      {
        Console.WriteLine("Second call to AutoInc(): " + obj.AutoInc());
      } catch(COMException)
      {
        Console.WriteLine("machine.config file missing tag legacyCorruptedStateExceptionsPolicy true");
        Console.WriteLine("Re-creating object due to COM object crash and calling AutoInc()");
        obj = (IExplosiveClass)Activator.CreateInstance(CPPcomType);
        Console.WriteLine("Second call to AutoInc(): " + obj.AutoInc());
      }
      try
      {
        obj.ThrowCppException();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.GetType().Name + ":" + e.Message);
      }
      try
      {
        obj.ThrowString();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.GetType().Name + ":" + e.Message);
      }
      Console.WriteLine("Third call to AutoInc(): " + obj.AutoInc());
    }

    static void TestCSharpCOM()
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
            if (i % ThreadsCount == (int)id)
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
      var elapsed = Environment.TickCount - initialTicks;
      Console.WriteLine(callsCount.ToString() + " calls in " + elapsed.ToString() + "ms");
      Console.WriteLine("Speed: " + 60 * callsCount / elapsed + " reqs/s");
    }
  }
}
