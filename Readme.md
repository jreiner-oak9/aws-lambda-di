# AWS Lambda DI Demonstration

This project demonstrates DI registration in an AWS Lambda. A ServiceProvider is
produced during function construction (cold start of a Lambda) and reused during
subsequent invocations (warm starts).

A simple service which logs messages during construction, method calls, and disposal
has been inherited and registered for Singleton, Scoped, and Transient lifetimes. 
Each dependency is injected several times within a loop to simulate many services 
asking for the same dependencies over the span of an invocation.
* The SingletonService will produce a new instance only during a cold start. You 
  should observe the same id in each for-loop iteration of each invocation until 
  the Lambda goes cold.
* The ScopedService will produce a new instance for each invocation. You should 
  observe the same id in each for-loop iteration, but different id's for each invocation.
* The TransientService will produce a new instance for each service request. You 
  should observer a new id in each for-loop iteration, every invocation.

Regarding lifetime and disposal:
* SingletonService is never disposed, as it remains for the lifetime of the application.
  Lambda will not call a finalizer, so we need to be mindful of any resources which
  require some cleanup!
* ScopedService will be disposed at the end of an invocation.
* TransientService will be disposed as it leaves scope, or possibly at GC's discretion.

h/t to Eyad for suggesting new scope trigger at the beginning of each invocation!