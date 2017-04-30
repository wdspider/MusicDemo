# MusicDemo ASP.NET MVC Application
I have recently been asked to provide code samples demonstrating my work by various people; however, up to this point the majority of my work has been on proprietary projects.  As such, I decided to create this demo ASP.NET MVC application over the course of a weekend.

This application is a simple music database consisting of artists, albums, and tracks.  A website is provided for users to manipulate the data within the database.  The user can view existing artists, add a new artist, update an artist, or delete an artist.  Each artist can be associated with a collection of albums which the user can view, add, update, or delete.  Each album can be associated with a collection of tracks which the user can view, add, update, or delete.

#### Frameworks Used
- [ASP.NET MVC](https://www.asp.net/mvc)
- [Bootstrap](http://getbootstrap.com/)
- [Automapper](http://automapper.org/)
- [Ninject](http://www.ninject.org/index.html)
- [Entity Framework](https://github.com/aspnet/EntityFramework6)
- [Moq](https://github.com/moq/moq)
- [MS Test Framework](https://github.com/microsoft/testfx)

## Architecture Overview
There are 3 main components to this application:  Website, Website Backend, and Backend Provider Implementation.

### Website
The *MusicDemo.Website* project contains all of the controllers, view models, and razor views used by the application.  The views have been styled using the [Sandstone Bootstrap theme](http://bootswatch.com/sandstone/).

##### Sample Display Flow
The controller retrieves the backend model from the website backend injected via [Ninject](http://www.ninject.org/index.html).  The backend model is then mapped to the appropriate view model via [Automapper](http://automapper.org/).  Finally, the view model is passed into the rendering view.

##### Sample Edit Flow
The view posts the view model to the controller.  The controller maps the view model to the backend model via [Automapper](http://automapper.org/).  Finally, the controller sends the backend model to the website backend injected via [Ninject](http://www.ninject.org/index.html).

##### Switching To A Different Backend Provider Implementation
Switching to a different backend provider implementation is a straight-forward process:
- Implement the `BackendProvider` abstraction within the *MusicDemo.Website.Backend* project for the new backend provider implementation.
- Update line 15 `config.AddProfile<DBModelMappingProfile>();` in `App_Start\AutoMapperConfig.cs` to point to the new backend provider model mapping profile.
- Update line 15 `DBKernelBindings.Initialize(kernel);` in `App_Start\NinjectBindingConfig.cs` to point to the new backend provider kernel bindings.

### Website Backend
The *MusicDemo.Website.Backend* project contains all of the backend models used by the website.  In addition, the project contains an implementation of the `BackendProvider` abstraction for each backend provider.

#### `BackendProvider` Abstraction Components
The `BackendProvider` abstraction contains 3 main components:  a class that implements `BackendProviders\BackendProvider.cs`, a class that implements an [Automapper](http://automapper.org/) `Profile`, and a class that contains [Ninject](http://www.ninject.org/index.html) kernel bindings.  In a more complex application, the `BackendProviders\BackendProvider.cs` abstract class would be broken down into several abstract classes; perhaps a class for each backend model.

##### `BackendProviders\BackendProvider.cs` Implementation
This class functions as the glue between the `BackendProvider` abstraction and the actual backend provider implementation.  In the case where there are multiple `BackendProvider` abstract classes defined, there would be one of these types of class for each `BackendProvider` abstract class.  
\--- *Example Class: `BackendProviders\Database\DBBackendProvider.cs`*

##### [Automapper](http://automapper.org/) `Profile` Implementation
This class defines all of the [Automapper](http://automapper.org/) mappings used within the `BackendProvider` abstraction implementation.  Generally, these will be the mappings needed to translate from the website backend models to the actual backend provider models and vis versa.
\--- *Example Class: `BackendProviders\Database\DBModelMappingProfile.cs`*

##### [Ninject](http://www.ninject.org/index.html) Kernel Bindings
This class contains a `public static void Initialize(IKernel kernel)` method that initializes any [Ninject](http://www.ninject.org/index.html) kernel bindings used to dynamically inject any actual backend provider implementation components into the `BackendProvider` abstraction implementation.  
\--- *Example Class: `BackendProviders\Database\DBKernelBindings.cs`*

### Backend Provider Implementation
The *MusicDemo.Database* project functions as the backend provider implementation.  The project makes use of the Repository pattern to access an [Entity Framework](https://github.com/aspnet/EntityFramework6) database that stores the artist, album, and track information the user provided via the website.

However, the backend provider implementation does not need to be an [Entity Framework](https://github.com/aspnet/EntityFramework6) database.  The backend provider implementation could just as easily be a SOAP API or a REST API.  Thanks to the `BackendProvider` abstraction within the website backend, the website does not care what the actual backend provider implementation is so long as the user’s artist, album, and track information can be stored and retrieved.