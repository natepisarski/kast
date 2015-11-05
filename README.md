# Kast
Kast is a program for managing loosely coupled services. This means that it allows programs to communicate with one another as if they were piped into each other. They do not differ wildly from Unix pipes in functionality; however, they differ from them in usability. How, in Unix pipes, would you set up a program to list every file in subdirectories of the current directory? It would be tough.

## Command Line Arguments
### Server
Currently, "kast server" is the only command line argument. This will start the kast server. Planned command line arguments include "kast server list" and "kast server stop"
### Client
From "kast client help":

[] denotes a required argument. {} denotes optional ones. OR denotes choices

Client commands templates are:

kast client box [command] +{args comma,separated,args} {name boxName}

kast client feed |[box syntax]| |[box syntax]| |{name feedName} {option all OR last}

kast client hook |[box syntax]| target |{name hookName} {option first OR last OR innerRemove OR innerKeep}

In places where [box syntax] is accepted, @name is accepted to reference a currently running box in the relay

## License
BSD-3 Clause
