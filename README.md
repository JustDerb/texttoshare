Text 2 Share
===========

COM S 430 Project - Team Sauerkautz

Overview
========

Our proposal involves a mass texting system where users can create groups and send mass text messages and other commands to the server.  We will give the users a way of creating their own scripts via the website we will set up.  One example of where this may be handy is with fraternities and sororities.  They can easily send mass texts out to members to tell them something, or can use it to call a designated driver from the Greek community they are in, since sometimes they are too big to know completely everyone.  This system is geared towards groups who need a quick and efficient way of communicating, whereas emails may not work.  People check their phone text messages constantly, but not all do the same with email.

Modular Design
==============
![ Com S 430 UML Diagram](https://f.cloud.github.com/assets/1020220/439146/47b86bfe-b0dd-11e2-9bb9-1e9f0a7b330a.png)

The program will be operating on the same server as the web server, but on different processes.  The breakdown of threads/processes goes as follows:
 - Message Watcher Service (Gmail Email Checker)
 - MySQL
 - Logger
 - Webserver running ASP.net
 - Each LUA plugin will run on its own thread (1 thread per text message received)


Users
-----

Each user can be one of two types of users, a regular user or a developer.  The regular user uses the system by just texting the system (which executes plugins).  A developer has the added privilege of adding their own plugins to the system, and can only edit their own plugins.  

Group Moderator
---------------

If the user would like to create a group for an organization they can.  They can then add people to the group.  Each group will have a unique group identifier with that group to text back to the system to determine which group to run the plugin on.  The group moderators will be able to add/remove people from the group and enable/disable plugins for use within the group.

Super Users
-----------
Super users would be us (the “implementers”).  We have access to everything and can ban people, when we see fit!

LUA
---
LUA is a scripting language that can be taken and implemented within other programs (most notably in SecondLife).  It is written in C, which can be called within C# code.  We will define our own “functions” within the scripting language to give it more power while making it easier to implement since we have the base framework coded for us.

Google Gmail
------------

We are using Gmail since they have an API for texting to mobile phones.  You cannot code your own since this requires a phone plugged in at all times to the server, with a phone plan to go with it… So, to simplify our efforts, Gmail will suite it nicely since all sending/receiving commands can be done with email.

GIT
---

If we have left over time, we will implement a versioning system for the plugins so that Group Moderators may choose to ‘upgrade’ to the newer version or stay with the previous one in case they do not like the added/removed functionality of the plugin.

Components
----------
Stuff that goes into this project:
 - ASP.net Web Server
 - Google Gmail API (with SMTP)
 - LUA (Scripting for Plugins)
 - MySQL (Database backend)
 - Client/Server stuff
 - Database management
 - Security Issues
 - Texting/Emailing
 - Threading
 - [If Time] GIT (Versioning system for plugins)

Sauerkautz: Software engineering with a hint of vegetables.
