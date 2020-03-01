using System.Web.Mvc;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.XA.Foundation.Mvc.Controllers;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace Hackathon.Project.Website.Controllers
{
    public class FormController : StandardController
    {
        public const string RootItemToCreate = "/sitecore/content/Hackathon/New site/Data/Hackathon Teams";
        public const string TemplateItemToCreate = "/sitecore/templates/Project/Hackathon/Hackathon Team";
        public const string HomeItem = "/sitecore/content/Hackathon/New site/Home";
        // GET: Form
        public ActionResult Form()
        {
            return View("View");
        }

        [HttpPost]
        public ActionResult Index(string name, string email, string country)
        {
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                var db = Sitecore.Configuration.Factory.GetDatabase("master");
                var parentItem = db.GetItem(RootItemToCreate, Language.Current);
                var templateItem = db.GetItem(TemplateItemToCreate);
                var template = db.GetTemplate(templateItem.ID);

                var homeItem = db.GetItem(HomeItem);
                var currentHackathon = homeItem.Fields["CurrentHackathon"].Value;
                var selectedHackathon = db.GetItem(new ID(currentHackathon));


                var createdItem = parentItem.Add(name, template);
                createdItem.Editing.BeginEdit();
                createdItem.Fields["Name"].Value = name;
                createdItem.Fields["Email"].Value = email;
                createdItem.Fields["Country"].Value = country;
                createdItem.Editing.EndEdit();


                selectedHackathon.Editing.BeginEdit();
                Sitecore.Data.Fields.MultilistField multiSelectField = selectedHackathon.Fields["Teams"];
                multiSelectField.Add(createdItem.ID.ToString());
                selectedHackathon.Editing.EndEdit();
            }

            return Redirect("/");
        }
    }

}