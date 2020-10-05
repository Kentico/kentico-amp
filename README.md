[![NuGet](https://img.shields.io/nuget/v/Kentico.AcceleratedMobilePages.svg)](https://www.nuget.org/packages/Kentico.AcceleratedMobilePages/)
# Accelerated Mobile Pages Module for Kentico

Accelerated Mobile Pages Module (AMP Filter) is a custom module for Kentico CMS and EMS. It consists of an [Output Filter](https://docs.kentico.com/k12/configuring-kentico/using-output-filters) for transforming regular HTML to AMP HTML format and a macro method that makes it easy to adjust and fine-tune the rendered output. The project is based on a [master thesis (EN)](https://is.muni.cz/th/409956/fi_m/?lang=en) of Daniel Minarik ([full text](https://is.muni.cz/th/409956/fi_m/thesis.pdf)).

:bulb: Read [how we implemented AMP](https://devnet.kentico.com/articles/google-amping-the-kentico-advantage-site) on Kentico Advantage website.

## Usage

### Installation
1. Install the NuGet package [`Kentico.AcceleratedMobilePages`](https://www.nuget.org/packages/Kentico.AcceleratedMobilePages/)

### Enabling the AMP module

To enable AMP Filter on a site:

1. Go to Settings -> System -> Output filter -> AMP Filter
1. Check that "Enable AMP Filter" is on
1. Set an AMP domain name (e.g.: `amp.domain.tld` if the web is hosted on www.domain.tld).
1. Go to Sites -> Edit site -> Domain aliases
1. Create a new domain alias corresponding with AMP domain name specified earlier

### Activating AMP Filter for a specific page

There are two options for how to activate an AMP Filter for a specific page:

* In Accelerated Mobile Pages application, add pages by selecting them from the site content tree.

* In Pages application, by selecting a particular page from the content tree, navigating to Properties -> AMP Filter, and then selecting "Enable AMP for this page".

### Setting up CSSs

There are several ways of including cascading style sheets into an AMP page:

*	Set a default CSS stylesheet for the whole site (in Settings -> System -> Output filter -> AMP Filter).

*	Set a CSS stylesheet for every AMP page separately (Pages -> Edit -> Properties -> AMP Filter -> uncheck "Use default stylesheet" and select the desired stylesheet)

*	If none of the previous options is set, the AMP Filter will use the regular CSS stylesheet assigned to the page (either the site's default stylesheet or the page-specific one). This option is not recommended, as the stylesheet could be bigger than 50kB (e.g., if it contains styles for the whole website).

### Further customization of AMP pages
The AMP standard offers a lot of components or tags which do not have an ordinary HTML equivalent, and therefore, they can't be automatically injected or replaced in the page's source code.

You can use the `{% AmpFilter.IsAmpPage() %}` macro to find out whether the AMP Filter is enabled and active on the current page. This is useful for showing and hiding different parts of a web page. To do that, use the macro as a visibility condition of a web part.

#### Advanced components
Some advanced components, such as social media embed, advertisement, analytics, video or audio require a [script to be included](https://www.ampproject.org/docs/reference/components) in the head element. To do that, use the Head HTML Code web part.

## How the AMP Filter works

For pages that have the AMP Filter enabled:

* If the page is accessed from a non-AMP domain:
  * AMP Filter will insert an `amphtml` link to the head tag of the page (this allows discovery of AMP-enabled pages)

* If the page is accessed from an AMP domain, the AMP Filter:
  * Removes restricted elements, attributes and their values
  * Replaces regular tags with their AMP HTML equivalents (these 5 tags: img, video, audio, iframe, form)
  * Inserts compulsory AMP markup required by the AMP standard (such as the AMP runtime script, boilerplate code…)
  * Injects a stylesheet as described above

### Transformation of HTML to AMP HTML
This transformation is done via the HtmlAgilityPack library. It creates a DOM structure and the AMP Filter removes and replaces the elements according to the [AMP HTML specification](https://www.ampproject.org/docs/reference/spec).

Some advanced replacements (which were not possible to implement using the HtmlAgilityPack) are done by regular expressions.

### Cascading style sheets
The AMP Filter does not transform cascading stylesheets in any way. The developer must ensure that the stylesheet complies with [AMP HTML specification - stylesheet restrictions](https://www.ampproject.org/docs/reference/spec#stylesheets).


### Global settings
AMP Filter has two types of global settings:

 - Font providers settings - a white-list of font providers of custom fonts available for usage via the `<link>` tag. (Other fonts can be used via the `@font-face` CSS rule.)
 - Script URL settings for five different AMP HTML elements. This allows loading of specific (or latest) versions of the runtime scripts.

Both types of settings depend directly on the AMP HTML specification and should be changed only according to the AMP specification.

## Responsibilities of the developer
Even when using the AMP Filter there are still some things that need to be handled by the developer:

* Write valid CSSs according to the [AMP specification](https://www.ampproject.org/docs/reference/spec#stylesheets).
* Follow the [AMP specification when](https://www.ampproject.org/docs/reference/spec#svg) using SVG tags (AMP Filter does not affect SVG tags at all).
* Use only white-listed font providers via `<link>` tag or use other fonts using `@font-face` CSS rule.
* Explicitly state the size for every image (height and width attribute).
* In case of sites with complex styling, the stylesheets need to be reduced so that they do not exceed 50kB in total.
* If a page contains scripts, on-line forms, social media embeds, advertisements, or other interactive elements, the elements need to be replaced with [AMP extended components](https://www.ampproject.org/docs/reference/components).

## Scenarios covered by the AMP Filter

* For simple sites with CSS totaling no more than 50kB in size (and adhering to the official [AMP rules](https://www.ampproject.org/docs/reference/spec#stylesheets)), there's no need to create AMP-specific stylesheets - the sites will work correctly with the regular stylesheets.

* The AMP filter is mainly intended for simple, static pages, i.e. pages containing regular HTML. Only the `<img>`, `<video>`, `<audio>`, and `<iframe>` interactive elements (adhering to the [AMP HTML Specification](https://www.ampproject.org/docs/fundamentals/spec)) are allowed. Pages that fulfill these requirements will be transformed into the AMP format.

* The static `<form>` element used by ASP.NET web forms to store metadata and viewstate information is also converted to the AMP format. However, standard `<form>` elements included in the page's markup will not be submitted and processed by the server at all. To implement standard on-line form functionality (via the `<form>` tag), refer to the [amp-form](https://www.ampproject.org/docs/reference/components/amp-form) section of the official AMP documentation.

## Developing the module and contributing
 1. Read the [contribution guidelines](https://github.com/Kentico/kentico-amp/blob/master/CONTRIBUTING.md)
 1. Enable the [continuous integration](https://docs.kentico.com/display/k12/Setting+up+continuous+integration) module
 1. Remove `<ObjectType>cms.settingskey</ObjectType>` from the `CMS\App_Data\CIRepository\repository.config`
 1. Serialize all objects to disk
 1. Open a command prompt
 1. Navigate to the root of the project (where the .sln file is)
 1. Fork this repo
 1. Init a git repo and fetch the web part

        git init
        git remote add origin https://github.com/OWNER/kentico-amp.git
        git fetch
        git checkout origin/master -ft

 1. Restore DB data

        Kentico\CMS\bin\ContinuousIntegration.exe -r

 1. Open the solution in Visual Studio
 1. Add the following projects to the solution:
    * `Kentico.AcceleratedMobilePages\Kentico.AcceleratedMobilePages.csproj`
    * `Kentico.AcceleratedMobilePages.Publisher\Kentico.AcceleratedMobilePages.Publisher.csproj`
 1. Reference the added projects from the CMSApp project:
    1. Right-click the CMSApp project and select Add -> Reference...
    1. Click Browse... and locate the projects on your filesystem
    1. Click Add
 1. Rebuild the solution
 1. [Resign all macros](https://docs.kentico.com/k12/macro-expressions/troubleshooting-macros/working-with-macro-signatures)
 1. Optional: Assign the module to one or more sites
 1. Make changes
 1. Use a combination of `git add`, `git commit` and `git push` to transfer your changes to GitHub

        git status
        git commit -a -m "Fix XY"
        git push

 1. Submit a pull request

## Compatibility
Tested with Kentico 12.0 (net461).

## [Questions & Support](https://github.com/Kentico/Home/blob/master/README.md)

## [License](https://github.com/Kentico/kentico-amp/blob/master/LICENSE.txt)

![Analytics](https://kentico-ga-beacon.azurewebsites.net/api/UA-69014260-4/Kentico/kentico-amp?pixel)
