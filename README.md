# Accelerated Mobile Pages Module for Kentico

Accelerated Mobile Pages Module (AMP Filter) is a custom module for Kentico CMS and EMS. It consists of an [Output Filter](https://docs.kentico.com/k10/configuring-kentico/using-output-filters) for transforming regular HTML to AMP HTML format and a macro method that makes it easy to adjust and fine-tune the rendered output. The project is based on a [master thesis (EN)](https://is.muni.cz/th/409956/fi_m/?lang=en) of Daniel Minarik ([full text](https://is.muni.cz/th/409956/fi_m/thesis.pdf)).

## Usage

### Installation  


### Enabling the AMP Module

To enable AMP Filter on a site:

1. Go to Settings -> Content -> Output filter -> AMP Filter
2. Check "Enable AMP Filter"
3. Set an AMP domain name (e.g.: `amp.domain.tld` if the web is hosted on www.domain.tld).
4. Go to Sites -> Edit site -> Domain aliases
5. Create a new domain alias corresponding with AMP domain name specified earlier

### Activating AMP Filter for a Specific Page

There are two options how to do it:

* In Accelerated Mobile Pages application, add pages by selecting them from the site content tree.

*	In Pages application, by selecting a particular page from the content tree, navigating to Properties -> AMP filter, and then selecting "Enable AMP for this page".

### Setting the CSS Stylesheets

There are several options how to include CSS stylesheets into an AMP page:

*	Set a default CSS stylesheet for the whole site (in Settings -> Content -> Output filter -> AMP Filter).

*	Set a CSS stylesheet for every AMP page separately by unchecking "Use default stylesheet" in properties of a page.

*	If none of the previous options is set, the AMP Filter will use the regular CSS stylesheet assigned to the page (either the site's default stylesheet, or the page-specifc one). This option is not recommended as the stylesheet could be bigger than 50kB (e.g. if it contains styles for the whole website).

* All the options above can be combined throughout the website.

### Customizing the Content of AMP Pages

The AMP standard offers a lot of components or tags which do not have an ordinary HTML equivalent, therefore they can't be inserted automatically into the page's source code. This is how to use extended AMP components and at the same time a method how to use regular HTML for customization of an AMP page:
 
* Add new web part to the page: Static HTML
* Set this macro as the visibility condition for the web part:

`{% StartsWith(Settings.AMPFilterDomainAlias,System.Domain) %}`

(this macro ensures that web part will be visible only on pages accessed from AMP domain)

* Place any HTML code into the web part - for example some extended AMP component such as social media embed, advertisement, analytics, video, audio, …
* If an extended AMP component has been used, it needs to be included in the head element (required by AMP standard):
  * Add a new web part to the page: Head HTML Code
  * Set the previous macro as enable condition
  * For every extended AMP component, insert the import script into the head webpart

## How AMP Filter Works

After AMP Filter is triggered on a specific page, which has the AMP Filter enabled:

* If the page is accessed from non-AMP domain

  * AMP Filter will just insert the amphtml link to the head tag of the page (it serves as an information, that this particular page has also an AMP version)

* If the page is accessed from AMP domain, AMP Filter

  * Removes restricted elements, attributes and attributes’ values
  * Replaces regular tags by their AMP HTML equivalents (these 5 tags: img, video, audio, iframe, form)
  * Inserts compulsory AMP markup required by AMP standard (such as AMP runtime script, boilerplate code…)
  * Inserts the stylesheet (as mentioned above, it depends on the settings of particular AMP page - either it is default AMP stylesheet, specific AMP stylesheet or regular stylesheet)

### Transformation of HTML to AMP HTML

This transformation is performed using HtmlAgilityPack library. It creates the DOM structure and AMP Filter removes and replaces the elements according to the [AMP HTML specification](https://www.ampproject.org/docs/reference/spec).

AMP filter performs few corrections using regular expressions. The reason is, that these corrections were not possible using parser from HtmlAgilityPack library.

### CSS Stylesheets

AMP Filter does not transform CSS stylesheets in any way. The developer must ensure the stylesheet complies with the [AMP HTML specification - stylesheet restrictions](https://www.ampproject.org/docs/reference/spec#stylesheets).
 

### Global Settings

AMP Filter has two types of global settings. Font providers settings and script URL settings for five different AMP HTML elements. Both types of settings depend directly on the AMP HTML specification and should be changed only according to the AMP specification.

Font providers is a list of white-listed font providers of custom fonts available for use via <link> tag. Other fonts can be used via @font-face CSS rule.

Script URL settings solves the situation, when AMP releases new version of AMP runtime script or other scripts (the version is a part of the URL).

## Responsibilities of the Author

* Write valid CSS stylesheets according to the [AMP specification](https://www.ampproject.org/docs/reference/spec#stylesheets)
* Follow the [AMP specificationwhen](https://www.ampproject.org/docs/reference/spec#svg) using SVG tags (AMP Filter does not affect SVG tags at all)
* Use only white-listed font providers via <link> tag or use other fonts using @font-face CSS rule
* Explicitly state the size for every image (height and width attribute)
* In case of sites with complex styling, the stylesheets need to be reduced not to exceed 50kB in total.
* If a page contains scripts, social media embeds, advertisements, or other interactive elements, the elements need to be replaced with [AMP extended components](https://www.ampproject.org/docs/reference/components).

## Scenarios Solved by AMP Filter

* In case of simple sites with CSSs smaller than 50kB (that don't break any [AMP rules](https://www.ampproject.org/docs/reference/spec#stylesheets)), there's no need to create AMP-specific stylesheets - the sites will work correctly with the regular stylesheets.

* AMP Filter works best with simple pages. If a page contains only regular HTML without interactive elements (except for `<img>`, `<video>`, `<audio>`, `<iframe>`, `<form>` elements) AMP Filter will transform the page to AMP format.

## Developing the module and contributing
 1. Read the [contribution guidelines](https://github.com/Kentico/kentico-amp/blob/master/CONTRIBUTING.md)
 2. Remove `<ObjectType>cms.settingskey</ObjectType>` from the `CMS\App_Data\CIRepository\repository.config`
 3. Enable the [continuous integration](https://docs.kentico.com/display/K9/Setting+up+continuous+integration) module
 4. Serialize all objects to disk
 5. Open a command prompt
 6. Navigate to the root of the project (where the .sln file is)
 7. Fork this repo
 8. Init a git repo and fetch the web part
  
         git init
         git remote add origin https://github.com/OWNER/kentico-amp.git
         git fetch
         git checkout origin/master -ft

 9. Restore DB data
  
         Kentico\CMS\bin\ContinuousIntegration.exe -r
 10. Open the web project in Visual Studio
 11. Add `AcceleratedMobilePages\AcceleratedMobilePages.csproj` to the solution
 12. Add reference from CMSApp to AcceleratedMobilePages.csproj
 13. Build the solution
 14. [Resign all macros](https://docs.kentico.com/k10/macro-expressions/troubleshooting-macros/working-with-macro-signatures)
 15. Make changes
 16. Use combination of `git add`, `git commit` and `git push` to transfer your changes to GitHub
  
        git status
        git commit -a -m "Fixed XY"
        git push

 17. Submit a pull request
  
## Compatibility
Tested with Kentico 10.0 (net46).

## [Questions & Support](https://github.com/Kentico/Home/blob/master/README.md)

## [License](https://github.com/Kentico/kentico-amp/blob/master/LICENSE.txt)
