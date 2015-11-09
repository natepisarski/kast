# Kast
Kast is a program for managing loosely coupled binary services in a variety of ways. Loosely Coupled Services (LSC) are a light coupling between components of a greater system. In the case of kast, these are system binaries.

Kast takes the output from programs and feeds it to others, optionally causing them to take in the output of the calling program, in much the same way that Unix pipes do. For instance, to output every line in a directory, it could be implemented as such:

```
box ls +args -R,-a name reader+
feed |@ls| |box cat| last
```

## Command Line Arguments
### Server
Server currently accepts the argument "file" and a configuration file in Sections syntax. See Sections.cs in source for a tutorial on how sections are laid out. It can also run standalone by assigning the default arguments to itself.
### Client
From "kast client help":

[] denotes a required argument. {} denotes optional ones. OR denotes choices

Client commands templates are:

kast client box [command] +{args comma,separated,args} {name boxName}

kast client feed |[box syntax]| |[box syntax]| |{name feedName} {option all OR last}

kast client hook |[box syntax]| target |{name hookName} {option first OR last OR innerRemove OR innerKeep}

kast client unlist name

In places where [box syntax] is accepted, @name is accepted to reference a currently running box in the relay

## License
BSD-3 Clause
