# CrossHMI

## Subject

This project is meant to prove how vaiable is it to push Human-Machine-Interfaces from huge monitoring systems into small, mobile devices. Achieving such feat would allow to make monitoring easier and more versatile depending on the given circumstances. In other words we are dealing with reactive visualization of asynchronous data on mobile devices.

## Goal 

The goal is to provide mobile application that is able to gather data from nearby devices, which are emitting various data. This data shall be presented in human-readable form taking into consideration localisation of both device and mobile HMI as well as the semantics represented by the type of data.

## Scope

* Building cross-platform mobile application using Xamarin.
* Incorporating [OPC-UA-OOI](https://github.com/mpostol/OPC-UA-OOI) library to handle data acquistion.
* Building layer of abstraction on top of mentioned library so it's more feasible to use with MVVM pattern.
* Presenting the data to the user in appropriate graphical way.
* Corelating geographical position of the user with the postion of the device in order to present most relevant data first.

## Related work

None yet.
