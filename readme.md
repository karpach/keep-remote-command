# Google Keep Command for Remote Controller
This repository is a plugin for https://github.com/karpach/remote-controller.
The command gives an ability to add items to the specified Google Keep list.

![Settings](Screenshots/Settings.png)

I used https://github.com/kiwiz/gkeepapi as an inspiration for accessing Google Keep unofficial REST API.

Command Name - a name of the command, gives you an ability to control different Google keep lists.

Execution delay - a delay before command starts its execution.

Google Keep Command uses Gmail Email and Gmail Password for Google Keep authorization.

List Id is the ending part of URL when you click on the Google keep list.

Example:
https://keep.google.com/u/1/#LIST/15660dde079.b79ba1bae1031212

List Id = 15660dde079.b79ba1bae1031212

[![Build status](https://ci.appveyor.com/api/projects/status/sok3pagy8i8eolc2?svg=true)](https://ci.appveyor.com/project/karpach/keep-remote-command)