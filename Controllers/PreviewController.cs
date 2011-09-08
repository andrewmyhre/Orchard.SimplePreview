using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.ContentManagement;
using System.Text;
using Orchard.Core.Routable.Models;
using Orchard.Core.Common.Models;

namespace SimplePreview.Controllers
{
    public class PreviewController : Controller
    {
        dynamic Shape { get; set; }
        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }
        private IContentManager _cms = null;

        public PreviewController(IOrchardServices services, IShapeFactory shapeFactory, IContentManager cms)
        {
            Services = services;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
            Shape = shapeFactory;
            _cms = cms;
        }

        private void ResponseWrite(string data)
        {
            Services.WorkContext.HttpContext.Response.Write(data);
        }

        public ActionResult Index(string pagename)
        {
            var route = _cms
                .Query()
                .ForPart<RoutePart>()
                .Where<RoutePartRecord>(rp => rp.Path.Contains(pagename))
                .List();

            if (route.Count() == 0)
                return new HttpNotFoundResult();

            var contentItem = route.FirstOrDefault().ContentItem;

            var versions = contentItem.Parts.Where(p => p is IEnumerable<CommonPartVersionRecord>).FirstOrDefault();
            var content = contentItem.Parts.Where(p => p is BodyPart).FirstOrDefault();
            if (content == null)
                return new HttpNotFoundResult();

            var latest = Services.ContentManager
                .GetAllVersions(content.Id)
                .Where(v=>v.VersionRecord.Latest)
                .FirstOrDefault();

            if (latest == null)
                return new HttpNotFoundResult();

            return View(latest.Parts.Where(p => p is BodyPart).FirstOrDefault());
        }
    }
}