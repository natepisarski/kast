# Kast
Kast is a program for managing loosely coupled services. This means that it allows programs to communicate with one another as if they were piped into each other. They do not differ wildly from Unix pipes in functionality; however, they differ from them in usability. How, in Unix pipes, would you set up a program to list every file in subdirectories of the current directory? It would be tough.

The proposed way to do this in kast would be
kast feed --marked "ls" "ls -a"

## Command Line Arguments
Kast is in development. Here are the command line arguments I want to have:
### kast-feed
* marked - Destroy the feed once it executes
### kast-hook
* marked - Destroy the hook once it executes
### kast-server
* independent{marked?, name?} - Make a new independent resource

@{id} reference a named worker

## License
BSD-3 Clause
