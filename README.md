# Kast
Kast is a program for loosely-coupled binary microservices, similar to [gearman](http://gearman.org/) but for full binaries. It's like the Unix pipe on steroids.

```shell
box ls +args -R,-a name reader+
feed |@ls| |box cat| last
```

It also lets you do some things that [supervisord](http://supervisord.org/) would traditionally do, like run commands on a schedule, or set up event listeners.

## Command Line Arguments
### Server
`server` runs the Kast server that allows this all to function

### Client
From "`kast client help`":

[] denotes a required argument. {} denotes optional ones. OR denotes choices

Client commands templates are:

```bash
kast client box [command] +{args comma,separated,args} {name boxName}
```

```bash
kast client feed |[box syntax]| |[box syntax]| |{name feedName} {option all OR last}
```

```bash
kast client hook |[box syntax]| target |{name hookName} {option first OR last OR innerRemove OR innerKeep}
```

```bash
kast client unlist name
```

In places where [box syntax] is accepted, @name is accepted to reference a currently running box in the server

## License
BSD-3 Clause
