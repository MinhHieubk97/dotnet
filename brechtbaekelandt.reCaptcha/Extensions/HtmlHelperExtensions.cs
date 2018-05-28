using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Newtonsoft.Json.Converters;

namespace brechtbaekelandt.reCaptcha.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString InvisibleReCaptchaFor(this IHtmlHelper htmlHelper, string publicKey, string elementId, string @event = "click", string beforeReCaptcha = null, bool useCookie = false)
        {
            @event = @event ??  "click";

            return new HtmlString(BuildReCaptchaForElementHtml(publicKey, elementId, @event, beforeReCaptcha, useCookie));
        }

        private static string BuildReCaptchaForElementHtml(string publicKey, string elementId, string @event, string beforeCheck, bool useCookie)
        {
            var builder = new StringBuilder();

            var containerId = Guid.NewGuid();

            builder.Append(BuildReCaptchaContainerHtml(publicKey, containerId));
            builder.Append("");
            builder.Append(BuildReCaptchaScript(elementId, containerId, @event, beforeCheck, useCookie));
            builder.Append("");

            return builder.ToString();
        }

        private static string BuildReCaptchaContainerHtml(string publicKey, Guid containerId)
        {
            return $"<div class=\"g-recaptcha\" id=\"{containerId}\" data-sitekey=\"{publicKey}\" data-size=\"invisible\"></div>";
        }

        private static string BuildReCaptchaScript(string elementId, Guid containerId, string @event, string beforeCheck, bool useCookie)
        {
            var script =
                $@"<script type=""text/javascript"">
                window.brechtbaekelandt = window.brechtbaekelandt || {{}};
                window.brechtbaekelandt.reCaptcha = window.brechtbaekelandt.reCaptcha || {{
                    isReCaptchaApiScriptAlreadyInPage: false,
                    isReCaptchaApiScriptLoaded: false,
                    isInitialized: false,                    
                    
                    captchaConfigs: [],

                    initializeReCaptchas: function() {{
                        this.captchaConfigs.forEach(config => {{ config.initialize() }})
                       
                        this.isInitialized = true;
                    }},
                
                    getReCaptchaWidgetIdForElement: function(elementId) {{
                        var config = this.captchaConfigs.find(function(config) {{
                            return config.elementId === elementId;
                        }});

                        return config ? config.widgetId : null;
                    }},

                    getReCaptchaContainerIdForElement: function(elementId) {{
                         var config = this.captchaConfigs.find(function(config) {{
                            return config.elementId === elementId;
                        }});

                        return config ? config.containerId : null;
                    }},

                    getReCaptchaConfigForElement: function(elementId) {{
                         var config = this.captchaConfigs.find(function(config) {{
                            return config.elementId === elementId;
                        }});

                        return config;
                    }},

                    executeReCaptchaForElement: function(elementId) {{
                        var widgetId = this.getReCaptchaWidgetIdForElement(elementId);

                        return grecaptcha.execute(widgetId);
                    }},
                    
                    getReCaptchaResponseForElement: function(elementId) {{
                        var widgetId = this.getReCaptchaWidgetIdForElement(elementId);

                        return grecaptcha.getResponse(widgetId);
                    }}
                }}
                
                window.brechtbaekelandt.reCaptcha.captchaConfigs.push(
                {{
                    containerId: ""{containerId}"",
                    elementId: ""{elementId}"",
                    widgetId: null,
                    event: ""{@event}"",
                    eventObject: null,
                    useCookie: {useCookie.ToString().ToLower()},
                    isInitialized: false,                        
                    data: {{}},
                   
                    get initialize() {{
                        var self = this;

                        return () => {{
                            var element = document.getElementById(self.elementId);                                    
                            var elementClone = element.cloneNode(true);
                            
                            elementClone[""on"" + (self.event != ""enter"" ? self.event : ""keyup"")] = self.eventHandler;

                            // get the original value and selectedIndex (for <select>)                         
                            elementClone.onfocus = () => {{
                                self.data.originalValue = elementClone.value;
                                self.data.originalIndex = elementClone.selectedIndex                                    
                            }}
                            
                            element.id += ""_Original"";                               
                            element.style.display = ""none"";

                            element.parentNode.insertBefore(elementClone, element);

                            self.widgetId = grecaptcha.render(self.containerId, {{ callback: self.callback, inherit: true }});

                            self.isInitialized = true;                                
                        }}
                    }},

                    get before() {{
                        return {(string.IsNullOrEmpty(beforeCheck) ? "null" : beforeCheck)};
                    }},

                    get eventHandler() {{
                        var self = this;

                        return (ev) => {{
                            self.eventObject =ev;

                            if(self.event === ""enter"") {{
                                if(self.eventObject.keyCode !== 13) {{
                                    return;
                                }}
                            }}

                            ev.preventDefault();
                            ev.stopImmediatePropagation();

                            // clear the cookie on the document
                            document.cookie = ""g-recaptcha-response=; expires=Thu, 01 Jan 1970 00:00:01 GMT; path=/"";

                            self.data.newValue = ev.target.value;                           

                            if(ev.target.nodeName === ""SELECT"") {{                                  
                                self.data.newIndex = ev.target.selectedIndex;  
                                ev.target.value = self.data.originalValue;
                                ev.target.selectedIndex = self.data.originalIndex;
                                ev.target.options[ev.target.selectedIndex].selected = true;
                            }}

                            if(typeof self.before === ""function"") {{
                                if(self.before(self.elementId)) {{
                                    grecaptcha.execute(self.widgetId);
                                }}
                            }} else {{
                                grecaptcha.execute(self.widgetId);
                            }}
                        }}
                    }},

                    get callback() {{
                        var self = this;

                        return (response) => {{

                            if(self.useCookie) {{
                                // set cookie
                                var date = new Date();

                                // set the period in which the cookie will expire (30 seconds);
                                date.setTime(date.getTime() + 30000);

                                document.cookie = ""g-recaptcha-response="" + response + ""; expires="" + date.toUTCString() + ""; path=/"";
                            }}

                            var element = document.getElementById(self.elementId + ""_Original"");
                            var clonedElement = document.getElementById(self.elementId);

                            // set the id's to the original values so when triggering the even the event.target id is correct
                            element.id = self.elementId;
                            clonedElement.id = self.elementId + ""_Cloned"";

                            // set the value to the new value
                            clonedElement.value = self.data.newValue;
                            element.value = self.data.newValue;

                            if(clonedElement.nodeName === ""SELECT"" && element.nodeName === ""SELECT"") {{     
                                // set the selected index to the new index
                                clonedElement.selectedIndex = self.data.newIndex;
                                element.selectedIndex = self.data.newIndex;
                                element.options[element.selectedIndex].selected = true;
                            }}                                                               

                            switch (self.event) {{
                                case ""click"": element.click(); break;
                                case ""focus"": element.focus(); break;
                                case ""blur"": element.blur(); break;
                                default: element.dispatchEvent(self.eventObject);
                            }}

                            // reset the id's after the event trigger
                            element.id = self.elementId + ""_Original"";
                            clonedElement.id = self.elementId;

                            grecaptcha.reset(self.widgetId);                           
                        }}
                    }}
                }});
                
                if (!window.brechtbaekelandt.reCaptcha.isReCaptchaApiScriptAlreadyInPage) {{
                    window.brechtbaekelandt.reCaptcha.isReCaptchaApiScriptAlreadyInPage = true;

                    var captchaScript = document.createElement('script');
                    captchaScript.type = 'text/javascript';
                    captchaScript.async = true;
                    captchaScript.defer = true;
                    captchaScript.src = 'https://www.google.com/recaptcha/api.js?onload=reCaptchaApiLoaded&render=explicit';

                    var head = document.getElementsByTagName('head')[0];
                    head.appendChild(captchaScript)

                    function reCaptchaApiLoaded() {{
                        window.brechtbaekelandt.reCaptcha.isReCaptchaApiScriptLoaded = true; 
                        window.brechtbaekelandt.reCaptcha.initializeReCaptchas();
                    }}
                }};                                
                </script>";

            return script;
        }
    }
}
