# OPCUA.CrossHMI

Cross-platform Human Machine Interface(HMI) based on OPCUA standard using Xamarin.

## Topic

Object Oriented Internet - reactive visualization of asynchronous data on mobile devices compliant with Internet of Things(IoT) concept. 

## Goals:

Building an architecture and its implementation for given set of devices, which will allow development of GUI for presenation of asynchronous data coming from IoT devices. Following metadata of received data will be assesed before rendering:
* Type of data.
* Distance between the data source and the user.

## Thesis:

Development will prove that it's possible to build unviversal cross-platform HMI for selected library sourcing the asynchronous data on mobile devices.

## Features&Limitations:

* Selection of one library that will provide the data in accordance with IoT concept.
  * https://github.com/mpostol/OPC-UA-OOI
* Selection of framework and technology stack used for implementation which will allow development on various mobile devices.
  * Xamarin.Forms - https://github.com/xamarin/Xamarin.Forms
* Development of application architecure based on Dependency Injection(DI)
  * MVVM
     * MvvmLight - https://github.com/lbugnion/mvvmlight
  * IoC/DI
     * AutoFac - https://github.com/autofac/Autofac
* Implementation of mechanism allowing to select context of received data.
* Implementation of shared configuration for all application components.
