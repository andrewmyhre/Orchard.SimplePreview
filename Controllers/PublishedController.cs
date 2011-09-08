using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Routable.Models;
using Orchard.Core.Common.Models;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;

namespace SimplePreview.Controllers
{
    public class PublishedController : Controller
    {
        public IOrchardServices Services { get; private set; }
        private IContentManager _cms = null;
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public PublishedController(IOrchardServices services, IShapeFactory shapeFactory, IContentManager cms)
        {
            Services = services;
            _cms = cms;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public ActionResult Index(string pagename)
        {
            var route = _cms
                .Query("v3contentitem")
                .ForPart<RoutePart>()
                .Where<RoutePartRecord>(rp => rp.Path.Contains(pagename))
                .List();

            var contentItem = route.First().ContentItem;
            var content = contentItem.Parts.Where(p => p is BodyPart).FirstOrDefault();
            if (content == null)
                return new HttpNotFoundResult();

            return View(content);
        }
    }
}