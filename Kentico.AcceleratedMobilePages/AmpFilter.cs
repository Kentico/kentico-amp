using System;
using System.Text.RegularExpressions;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.OutputFilter;
using CMS.PortalEngine;
using CMS.SiteProvider;
using HtmlAgilityPack;

namespace Kentico.AcceleratedMobilePages
{
    public class AmpFilter
    {
        /// <summary>
        /// String for importing custom elements in head tag
        /// </summary>
        private string customElementsScripts;


        /// <summary>
        /// If the filter is enabled for current page, final HTML will be modified
        /// </summary>
        /// <param name="filter">Output filter</param>
        /// <param name="finalHtml">Final HTML string</param>
        public void OnFilterActivated(ResponseOutputFilter filter, ref string finalHtml)
        {
            int state = CheckStateHelper.GetFilterState();
            if (state == Constants.ENABLED_AND_ACTIVE)
            {
                finalHtml = TransformToAmpHtml(finalHtml);
            }
            else if (state == Constants.ENABLED_AND_INACTIVE)
            {
                finalHtml = AppendAmpHtmlLink(finalHtml);
            }
        }


        /// <summary>
        /// Returns modified HTML in AMP HTML markup
        /// </summary>
        /// <param name="finalHtml">Final HTML string</param>
        private string TransformToAmpHtml(string finalHtml)
        {
            customElementsScripts = "";

            // Process the resulting HTML using Html Agility Pack parser
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(finalHtml);
            RemoveRestrictedElements(doc);
            ResolveComplexElements(doc);
            ReplaceRegularTagsByAmpTags(doc);

            // Do the rest using regular expressions
            finalHtml = InsertCompulsoryMarkupAndCss(doc.DocumentNode.InnerHtml);
            finalHtml = PerformRegexCorrections(finalHtml);

            return finalHtml;
        }


        /// <summary>
        /// Returns final HTML with appended link pointing to AMP version of current page
        /// </summary>
        /// <param name="finalHtml">Final HTML string</param>
        private string AppendAmpHtmlLink(string finalHtml)
        {
            string ampLink = (CMSHttpContext.Current.Request.IsSecureConnection ? Constants.P_HTTPS : Constants.P_HTTP) +
                             SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterDomainAlias") +
                             DocumentContext.CurrentAliasPath +
                             SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSFriendlyURLExtension");
            string metaTag = String.Format(Constants.AMP_AMP_HTML_LINK, ampLink) + Constants.NEW_LINE;
            // Insert meta tag
            finalHtml = Regex.Replace(finalHtml, "</head>", metaTag + "</head>");

            return finalHtml;
        }


        /// <summary>
        /// Removes elements restricted by AMP HTML standard
        /// </summary>
        /// <param name="doc">The complete HtmlDocument</param>
        private void RemoveRestrictedElements(HtmlDocument doc)
        {
            // Remove restricted elements
            foreach (var rule in Constants.XPATH_RESTRICTED_ELEMENTS)
            {
                RemoveElement(doc, rule);
            }

            // Remove attributes
            RemoveAttribute(doc, Constants.XPATH_ATTR_STYLE, Constants.XPATH_ATTR_STYLE_NAME);
        }


        /// <summary>
        /// Performs corrections required by AMP HTML standard using Regular Expressions
        /// These corrections were not possible using Html Agility Pack parser
        /// </summary>
        /// <param name="finalHtml">Final HTML string</param>
        private string PerformRegexCorrections(string finalHtml)
        {
            // Initial html amp tag - replace only first occurrence
            var regex = new Regex(Constants.HTML_TAG);
            finalHtml = regex.Replace(finalHtml, Constants.HTML_REPLACEMENT, 1);

            // Remove conditional comments
            finalHtml = Regex.Replace(finalHtml, Constants.REGEX_CONDITIONAL_COMMENTS, "");

            // Remove restricted attributes
            finalHtml = Regex.Replace(finalHtml, Constants.REGEX_ATTR_ONANY_SUFFIX, "");
            finalHtml = Regex.Replace(finalHtml, Constants.REGEX_ATTR_XML_ATTRIBUTES, "");
            finalHtml = Regex.Replace(finalHtml, Constants.REGEX_ATTR_IAMPANY_SUFFIX, "");

            // Remove restricted attribute's values
            finalHtml = Regex.Replace(finalHtml, Constants.REGEX_NAME_CLASS, "");
            finalHtml = Regex.Replace(finalHtml, Constants.REGEX_NAME_ID, "");

            return finalHtml;
        }

        /// <summary>
        /// Resolves more complex restrictions put on specific elements using HTML parser
        /// </summary>
        /// <param name="doc">The complete HtmlDocument</param>
        private void ResolveComplexElements(HtmlDocument doc)
        {
            // Script tags source URLs from settings
            string ampCustomForm = String.Format(Constants.AMP_CUSTOM_ELEMENT_AMP_FORM, SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterFormScriptUrl"));

            // Process <form> tags
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(Constants.XPATH_FORM);
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    // If attribute method="post" action attribute must be replaced by action-xhr
                    if ((node.Attributes["method"] != null) && (node.Attributes["method"].Value.ToLower() == "post"))
                    {
                        if (node.Attributes["action"] != null)
                        {
                            node.Attributes["action"].Name = "action-xhr";
                        }
                    }

                    // Ensure that target attribute has correct value or add target attribute with correct value
                    if ((node.Attributes["target"] == null) || (
                        (node.Attributes["target"] != null) && (node.Attributes["target"].Value.ToLower() != "_top")))
                    {
                        node.SetAttributeValue("target", "_blank");
                    }
                }

                // At least one <form> tag is used, we need to import custom element
                customElementsScripts += ampCustomForm + Constants.NEW_LINE;
            }

            // Process included fonts, only fonts from group of providers are allowed
            nodes = doc.DocumentNode.SelectNodes(Constants.XPATH_FONT_STYLESHEET);

            // List of font providers from settings
            string[] fontProviders = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AmpFilterFontProviders").Split('\n');
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    var elementAllowed = false;
                    if (node.Attributes["href"] != null)
                    {
                        foreach (string provider in fontProviders)
                        {
                            if (node.Attributes["href"].Value.ToLower().StartsWith(provider.Trim()))
                            {
                                elementAllowed = true;
                                break;
                            }
                        }
                    }
                    if (!elementAllowed)
                    {
                        node.Remove();
                    }
                }
            }
        }


        /// <summary>
        /// Replaces standard HTML tags by special AMP HTML tags and appends import scripts for custom elements
        /// </summary>
        /// <param name="doc">The complete HtmlDocument</param>
        private void ReplaceRegularTagsByAmpTags(HtmlDocument doc)
        {
            ReplaceElement(doc, Constants.XPATH_IMG, Constants.XPATH_IMG_REPLACEMENT);
            if (ReplaceElement(doc, Constants.XPATH_VIDEO, Constants.XPATH_VIDEO_REPLACEMENT))
            {
                string ampCustomVideo = String.Format(Constants.AMP_CUSTOM_ELEMENT_AMP_VIDEO, SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterVideoScriptUrl"));
                customElementsScripts += ampCustomVideo + Constants.NEW_LINE;
            }
            if (ReplaceElement(doc, Constants.XPATH_AUDIO, Constants.XPATH_AUDIO_REPLACEMENT))
            {
                string ampCustomAudio = String.Format(Constants.AMP_CUSTOM_ELEMENT_AMP_AUDIO, SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterAudioScriptUrl"));
                customElementsScripts += ampCustomAudio + Constants.NEW_LINE;
            }
            if (ReplaceElement(doc, Constants.XPATH_IFRAME, Constants.XPATH_IFRAME_REPLACEMENT))
            {
                string ampCustomIframe = String.Format(Constants.AMP_CUSTOM_ELEMENT_AMP_IFRAME, SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterIframeScriptUrl"));
                customElementsScripts += ampCustomIframe + Constants.NEW_LINE;
            }
        }


        /// <summary>
        /// Inserts compulsory AMP markup and inline CSS
        /// </summary>
        /// <param name="finalHtml">Final HTML string</param>
        private string InsertCompulsoryMarkupAndCss(string finalHtml)
        {
            // Save the original <head> tag before replacement later
            String headTag = "";
            Match m = Regex.Match(finalHtml, Constants.REGEX_HEAD);
            if (m.Success)
            {
                headTag = m.Value;
            }

            // Script tags source URLs from settings
            string ampRuntimeScript = String.Format(Constants.AMP_SCRIPT, SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterRuntimeScriptUrl"));

            // CSS stylesheet to be inlined to finalHtml
            string styles = GetStylesheetText();

            // Create a link pointing to the regular HTML version of the page
            string canonicalLink = (CMSHttpContext.Current.Request.IsSecureConnection ? Constants.P_HTTPS : Constants.P_HTTP) +
                                   SiteContext.CurrentSite.DomainName + DocumentContext.CurrentAliasPath +
                                   SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSFriendlyURLExtension");

            // Extend the <head> tag with the compulsory markup and CSS styles
            headTag +=  Constants.NEW_LINE +
                        Constants.AMP_CHARSET + Constants.NEW_LINE +
                        ampRuntimeScript + Constants.NEW_LINE +
                        customElementsScripts +
                        String.Format(Constants.AMP_CANONICAL_HTML_LINK, canonicalLink) + Constants.NEW_LINE +
                        Constants.AMP_VIEWPORT + Constants.NEW_LINE +
                        Constants.AMP_BOILERPLATE_CODE + Constants.NEW_LINE +
                        String.Format(Constants.AMP_CUSTOM_STYLE, styles) + Constants.NEW_LINE;
            
            finalHtml = Regex.Replace(finalHtml, Constants.REGEX_HEAD, headTag);
            return finalHtml;
        }


        /// <summary>
        /// Returns CSS stylesheet for current page.
        /// Stylesheet can be:
        ///     - normal css of current page
        ///     - default css for all AMP pages
        ///     - css set as AMP stylesheet for current page
        /// </summary>
        private string GetStylesheetText()
        {
            string cssText = "";

            // Checking which CSS file to use
            ObjectQuery<AmpFilterInfo> q = AmpFilterInfoProvider.GetAmpFilters().WhereEquals("PageNodeGuid", DocumentContext.CurrentPageInfo.NodeGUID.ToString());
            if (q.FirstObject.UseDefaultStylesheet)
            {
                // Get the ID of default AMP CSS
                string defaultID = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterDefaultCSS");
                var cssID = ValidationHelper.GetInteger(defaultID, 0);

                // Default AMP CSS is not set, using ordinary CSS of current page
                if (cssID == 0)
                {
                    cssText = DocumentContext.CurrentDocumentStylesheet.StylesheetText;
                }
                else
                {
                    // Use default AMP CSS stylesheet
                    var cssInfo = CssStylesheetInfoProvider.GetCssStylesheetInfo(cssID);
                    if (cssInfo != null)
                    {
                        cssText = cssInfo.StylesheetText;
                    }
                }
            }
            else
            {
                // Use specific AMP CSS set for this page
                var cssInfo = CssStylesheetInfoProvider.GetCssStylesheetInfo(q.FirstObject.StylesheetID);
                if (cssInfo != null)
                {
                    cssText = cssInfo.StylesheetText;
                }
            }

            // Resolve macros
            cssText = MacroResolver.Resolve(cssText);

            // Resolve client URL
            return HTMLHelper.ResolveCSSClientUrls(cssText, CMSHttpContext.Current.Request.Url.ToString());
        }


        /// <summary>
        /// Removes element using HTML parser.
        /// </summary>
        /// <param name="doc">The complete HtmlDocument</param>
        /// <param name="elementXPath">XPath specifying the element</param>
        private void RemoveElement(HtmlDocument doc, string elementXPath)
        {
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(elementXPath);
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    node.Remove();
                }
            }
        }


        /// <summary>
        /// Replaces element using HTML parser and return true if at least one node was replaced.
        /// </summary>
        /// <param name="doc">The complete HtmlDocument</param>
        /// <param name="xPath">XPath specifying the element</param>
        /// <param name="replacement">New name of the element</param>
        private bool ReplaceElement(HtmlDocument doc, string xPath, string replacement)
        {
            var nodes = doc.DocumentNode.SelectNodes(xPath);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.Name = replacement;
                }
            }
            return (nodes != null);
        }


        /// <summary>
        /// Replaces element's attribute using HTML parser.
        /// </summary>
        /// <param name="doc">The complete HtmlDocument</param>
        /// <param name="xPath">XPath specifying the element</param>
        /// <param name="attrName">Name of the attribute</param>
        private void RemoveAttribute(HtmlDocument doc, string xPath, string attrName)
        {
            var nodes = doc.DocumentNode.SelectNodes(xPath);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    node.Attributes.Remove(attrName);
                }
            }
        }
    }
}
