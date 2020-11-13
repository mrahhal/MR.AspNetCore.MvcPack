# MR.AspNetCore.MvcPack

[![Build status](https://img.shields.io/appveyor/ci/mrahhal/mr-aspnetcore-mvcpack/master.svg)](https://ci.appveyor.com/project/mrahhal/mr-aspnetcore-mvcpack)

[![NuGet version](https://img.shields.io/nuget/v/MR.AspNetCore.MvcPack.svg)](https://www.nuget.org/packages/MR.AspNetCore.MvcPack)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

A better way for writing controller action filters, inspired by rails.

## Usage

#### Configure

```cs
services
    .AddMvc(...)
    .InitializeMvcPack(); // Add this

services.AddMvcPack(); // Add this
```

#### Packs

A "pack" is a class that configures a certain controller. You can put them anywhere, but as this is inspired by rails I like to put them nested in the controller class:

```cs
public class SomeController : SomeBaseController
{
    public class Pack : MvcPackSupport<SomeController>
    {
        public Pack()
        {
            // Use this if you want to skip a filter configured in a base controller.
            SkipBeforeAction(x => x.AuthorizeSome);

            // Always run a filter.
            BeforeAction(x => x.Some1);

            // Use this to configure a filter to be run `only` when certain actions are selected.
            BeforeAction(x => x.Some2, only: L(
                nameof(SomeController.Get)));

            // Use this to configure a filter to be run on all except certain actions.
            BeforeAction(x => x.Some3, except: L(
                nameof(SomeController.Foo),
                nameof(SomeController.Bar)));
        }
    }

    // This should always be the prototype of method filters.
    protected Task Some1(ActionExecutingContext context)
    {
        // You can inspect action arguments, do authorization, set a result to shortcircuit if necessary, etc...
    }
}
```

## Motive

This is all about factoring out specific controllers' cross cutting concerns and putting them inside reusable methods.

Creating custom filters for each business concern is tedious, and overriding a controller's "OnActionExecuting" loses its appeal when you're inheriting other custom controllers that also have concerns.

Rails solves this in a great way, a major part of it is due to the expressiveness of ruby as a language. Asp.Net Core doesn't give us that flexibility by default, but we can improvise.

Check the samples for more info.

## NestedRouting

Using this together with [NestedRouting](http://github.com/mrahhal/MR.AspNetCore.NestedRouting) will give you a great way for organizing your controllers.

## Samples

Check out the [`samples`](samples) under "samples/" for more practical use cases.
