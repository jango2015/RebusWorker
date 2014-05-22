RebusWorker
===========

Trying out [Rebus](https://github.com/rebus-org/Rebus) as an alternative to [Nservicebus](https://github.com/Particular/NServiceBus)

## Features covered ##
* Pub/sub
* Sagas
* Timeout/defer (local timeoutmanager)
* Using Autofac as DI container
* Custom messageownership resolving (addressing)
* Hosted by [Topshelf](https://github.com/Topshelf/Topshelf)


## Getting started ##
In order to run against local SQL server:

* Clone this repo
* Run the SQL below
* Hit F5 in Visual Studio

```sql
create database 'rebus_test'
```
