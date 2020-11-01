### Background

In issue #24599, @SteveSandersonMS gave a full and very helpful description of how Blazor renders and updates values through two-way binding. This was further
touched upon in #20244, where Steve asked if we could raise an issue with a limited example - this is our response to that request.

@MarkStega and I have modified [Material.Blazor](https://material-blazor.com) to adjust to the situation as described by Steve, and are fully comfortable that
our needs are met. That said, we believe that the "bounciness" that Blazor can exhibit, in particular when components are surrounded by a cascading falue with
`IsFixed="false"` will lead other developers to experience issues. To demonstrate how this can come about we have prepared a repo at https://github.com/simonziegler/BlazorBounce.

### Examining Bounce with our repo

Fork the repo and you will have a Blazor WebAssembly project that copies a limited amount of Material.Blazor's code for select input components. It does this
twice, once with our debouncing mechanism in place and another with that mechanism disabled. We debounce by comparing an inbound change to the `Value` parameter
via two way binding with a cached value. Two way binding for Material.Blazor has complexity because we need to allow Google's [Material Components Web ("MCW")](https://github.com/material-components/material-components-web/tree/master/packages)
to take control of component behaviour by our returning false from `ShouldRender()`. See [our documentation site's two way binding article for more](https://material-blazor.com/docs/articles/TwoWayBinding.html).

We present a 3x2 matrix of examples, with some guidance on how to view bounce (or lack thereof) on the demo index page itself:

- All examples have two selects that are each bound to the same variable.
- The first three examples use a fully debounced select (using a cached value), while the second three lack debouncing.
- Within each set of three examples we then either have no cascading value, one with `IsFixed="true"` and one with `IsFixed="false"`.

You can see how bounce is exhibited by opening developer tools and looking at logging messages that our code prints with `Console.WriteLine()`. You will notice
that the final example entitled "Bouncy / Fixed-False Cascading Value" exhibits the bounce that Steve describes.

### Why we think bounce is a problem stored up for the future

To be clear: as it stands this bounce wouldn't cause issues for Material.Blazor. We believe that this is luck rather than anything else (along with our explicit debouncing in production).
To understand why consider the following set of events that are occuring in the bouncy case:

- The user changes one of the two selects from "red" to "orange".
- Blazor applies the following two-way binding updates in this order:
  - first it changes that select's value from orange back to red.
  - next the same select (the one that the user interacted with) gets changed from red back to orange as it should be
  - last the other select's value gets changed from red to orange.

So why do we think this is a problem? The answer is that we are using a third party web component framework, and each time `Value` is updated, we make a JSInterop
call to that framework's library. It turns out that MCW is well built (no surprise there) and does not emit a change event when you use
their JS functions to change a component's value. If they did emit a change, you would see the selects oscillating forever - we used to see this before we worked
out how to use MCW's JS library properly.

So the issue here is a complex one of the interaction between two foundations that don't know of each other's existence: Blazor and (in our case) MCW.
Blazor exhibits a certain amount of bounce and fortunately MCW subdues this effectively. Other component libraries in the future may not, and indeed there is no
guarantee that MCW, presently in version 7.0.0 will continue to do so in the future.

We therefore strongly believe that Blazor would be substantially more robust if this bounce could be eliminated at source, preferably from .NET 6 onwards. We think
it would be even better if Blazor were able to completely avoid attempting to render a component if its record of a Value matched that of a component's record, thus
eliminating the potential for future RCLs such as Material.Blazor experiencing our growing pains and needing to resort to your issues and the Blazor community
for assistance.